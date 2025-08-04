using FluentValidation;
using NexusAuth.Domain.ValueObjects.User;

namespace NexusAuth.Application.Features.Users.Authentication
{
    public sealed class AuthCommandValidator : AbstractValidator<AuthCommand>
    {
        public AuthCommandValidator()
        {
            RuleFor(x => x.Login)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Вы не указали логин")
                .MinimumLength(Login.MIN_LENGTH).WithMessage($"Логин не может быть короче {Login.MIN_LENGTH} символов.")
                .MaximumLength(Login.MAX_LENGTH).WithMessage($"Логин не может быть длиннее {Login.MAX_LENGTH} символов.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Вы не указали пароль.")
                .MinimumLength(10).WithMessage("Пароль не может быть короче 10 символов.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Вы не указали электронный адрес.")
                .EmailAddress().WithMessage("Не валидный адрес электронной почты");
        }

    }
}
