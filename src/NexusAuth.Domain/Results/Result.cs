using NexusAuth.Domain.Guards;
using NexusAuth.Domain.Guards.Extensions;
using System.Text;

namespace NexusAuth.Domain.Results
{
    public class Result<TValue> : Result
    {
        private readonly TValue? _value;

        private Result(TValue? value, bool isSuccess, List<Error> errors) : base(isSuccess, errors)
        {
            if (isSuccess && value is null && default(TValue) is not null)
            {
            }

            _value = value;
        }

        /// <summary>
        /// Возвращает значение в случае успеха, иначе бросает InvalidOperationException
        /// </summary>
        public TValue Value => IsSuccess
            ? _value!
            : throw new InvalidOperationException("Нельзя получить значение из неуспешного результата.");

        /*--Фабричные методы------------------------------------------------------------------------------*/

        public static Result<TValue> Success(TValue value) => new(value, true, []);

        public new static Result<TValue> Failure(Error error) => new(default, false, [error]);

        public new static Result<TValue> Failure(IEnumerable<Error> errors) => new(default, false, errors.ToList());

        /// <summary>
        /// Выполняет одну из двух функций в зависимости от состояния результата и возвращает их результат.
        /// </summary>
        public TResult Match<TResult>(Func<TValue, TResult> onSuccess, Func<IReadOnlyList<Error>, TResult> onFailure) => IsSuccess ? onSuccess(Value) : onFailure(Errors);

        /// <summary>
        /// Выполняет одно из двух действий в зависимости от состояния результата.
        /// </summary>
        public void Switch(Action<TValue> onSuccess, Action<IReadOnlyList<Error>> onFailure)
        {
            if (IsSuccess)
                onSuccess(Value);
            else
                onFailure(Errors);
        }
    }

    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public IReadOnlyList<Error> Errors { get; }

        protected Result(bool isSuccess, List<Error> errors)
        {
            Guard.Against.That(isSuccess && errors.Any(), () => new InvalidOperationException("Нельзя создать успешный результат с ошибками."));
            Guard.Against.That(!isSuccess && !errors.Any(), () => new InvalidOperationException("Нельзя создать провальный результат без ошибок."));

            IsSuccess = isSuccess;
            Errors = errors;
        }

        /*--Фабричные методы------------------------------------------------------------------------------*/

        public static Result Success() => new(true, []);
        public static Result Failure(Error error) => new(false, [error]);
        public static Result Failure(IEnumerable<Error> errors) => new(false, errors.ToList());

        /// <summary>
        /// Выполняет одну из двух функций в зависимости от состояния результата и возвращает их результат.
        /// </summary>
        public TResult Match<TResult>(Func<TResult> onSuccess, Func<IReadOnlyList<Error>, TResult> onFailure) => IsSuccess ? onSuccess() : onFailure(Errors);

        /// <summary>
        /// Выполняет одно из двух действий в зависимости от состояния результата.
        /// </summary>
        public void Switch(Action onSuccess, Action<IReadOnlyList<Error>> onFailure)
        {
            if (IsSuccess)
                onSuccess();
            else
                onFailure(Errors);
        }

        /*--Отображение ошибки в виде строки--------------------------------------------------------------*/

        public string ClientStringMessage => BuildMessage(error => error.ClientMessage ?? "Клиентское сообщение отсутствует.");
        public string SystemStringMessage => BuildMessage(error => error.SystemMessage);

        private string BuildMessage(Func<Error, string> messageSelector)
        {
            if (Errors.Count == 0) return "Ошибок нет.";

            var sb = new StringBuilder();
            sb.AppendLine("Ошибки:");

            for (int i = 0; i < Errors.Count; i++)
            {
                string message = messageSelector(Errors[i]);
                sb.Append($"{i + 1}) ").AppendLine(message);
            }

            return sb.ToString();
        }

        /*--Переопределенные методы-----------------------------------------------------------------------*/

        /// <summary>
        /// Выводит полную информацию об ошибках. Включая системное и клиентское сообщение, а так же их код ошибки.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Ошибки: ");

            for (int i = 0; i < Errors.Count; i++)
                sb.Append($"{i + 1}) ").AppendLine($"Код: [{Errors[i].Code.ToString()}] Статус: [{IsSuccess}] Системная информация: [{Errors[i].SystemMessage}] Клиентская информация: [{Errors[i].ClientMessage}]");

            return sb.ToString();
        }
    }
}