using FluentValidation;
using Microsoft.EntityFrameworkCore;
using NexusAuth.Application.Common.Abstractions;
using NexusAuth.Domain.ValueObjects.User;

namespace NexusAuth.Application.Features.Users.Registration
{
    public sealed class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        private readonly IApplicationDbContext _context;

        public RegisterCommandValidator(IApplicationDbContext context)
        {
            _context = context; 

            RuleFor(x => x.Login)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Вы не указали логин")
                .MinimumLength(Login.MIN_LENGTH).WithMessage($"Логин не может быть короче {Login.MIN_LENGTH} символов.")
                .MaximumLength(Login.MAX_LENGTH).WithMessage($"Логин не может быть длиннее {Login.MAX_LENGTH} символов.")
                .MustAsync(BeUniqueLogin).WithMessage("Пользователь с таким логином уже существует.");

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Вы не указали имя пользователя.")
                .MinimumLength(UserName.MIN_LENGTH).WithMessage($"Имя пользователя не может быть короче {UserName.MIN_LENGTH} символов.")
                .MaximumLength(UserName.MAX_LENGTH).WithMessage($"Логин не может быть длиннее {UserName.MAX_LENGTH} символов.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Вы не указали пароль.")
                .MinimumLength(10).WithMessage("Пароль не может быть короче 10 символов.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Вы не указали электронный адрес.")
                .EmailAddress().WithMessage("Не валидный адрес электронной почты");

            When(x => x.IdGender.HasValue, () =>
            {
                RuleFor(x => x.IdGender)
                      .GreaterThan(0).WithMessage("ID гендера не может быть меньше 0");
            });

            When(x => x.IdCountry.HasValue, () =>
            {
                RuleFor(x => x.IdCountry)
                      .GreaterThan(0).WithMessage("ID страны не может быть меньше 0");
            });

            When(x => !string.IsNullOrEmpty(x.Phone), () =>
            {
                RuleFor(x => x.Phone)
                      .Must(BeAValidPhoneObject!).WithMessage("{PropertyValue} не является корректным номером телефона. {ErrorMessage}");
            });
        }

        private async Task<bool> BeUniqueLogin(string login, CancellationToken cancellationToken)
           => !await _context.Users.AnyAsync(u => u.Login == login, cancellationToken); // Если не прописывать .UseCollation(), то сравнивать через ((string)u.Login).ToLower()

        private bool BeAValidPhoneObject(RegisterCommand command, string phone, ValidationContext<RegisterCommand> context)
        {
            try
            {
                Phone.Create(phone);
                return true;
            }
            catch (ArgumentException ex)
            {
                context.MessageFormatter.AppendArgument("ErrorMessage", ex.Message);
                return false;
            }
        }
    }
}