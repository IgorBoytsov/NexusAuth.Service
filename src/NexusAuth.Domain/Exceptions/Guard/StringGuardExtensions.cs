using System.Diagnostics.CodeAnalysis;

namespace NexusAuth.Domain.Exceptions.Guard
{
    public static class StringGuardExtensions
    {
        /// <summary>
        /// Проверяет, является ли строка null, пустой или состоит только из пробелов.
        /// </summary>
        /// <param name="guardClause">Экземпляр Guard.</param>
        /// <param name="input">Проверяемая строка.</param>
        /// <param name="parameterName">Имя параметра.</param>
        /// <param name="message">Опциональное сообщение об ошибке.</param>
        /// <returns>Входная строка, если проверка пройдена.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static string NullOrEmptyOrWhiteSpace(this IGuardClause guardClause, [NotNull] string? input, string parameterName, string? message = null)
        {
            Guard.Against.Null(input, parameterName, message);

            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentException(message ?? $"Параметр {parameterName} не может быть пустым или состоять из пустых символов.", parameterName);
            }

            return input;
        }
    }
}