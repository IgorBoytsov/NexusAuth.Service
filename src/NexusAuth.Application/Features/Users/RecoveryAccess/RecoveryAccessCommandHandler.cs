using MediatR;
using Microsoft.EntityFrameworkCore;
using NexusAuth.Application.Common.Abstractions;
using NexusAuth.Application.Helpers;
using NexusAuth.Application.Services.Abstractions;
using NexusAuth.Domain.Enums;
using NexusAuth.Domain.Exceptions;
using NexusAuth.Domain.Results;

namespace NexusAuth.Application.Features.Users.RecoveryAccess
{
    public sealed class RecoveryAccessCommandHandler(IApplicationDbContext context, IValidationService validator, IPasswordHasher hasher) : IRequestHandler<RecoveryAccessCommand, Result>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IValidationService _validator = validator;
        private readonly IPasswordHasher _hasher = hasher;

        public async Task<Result> Handle(RecoveryAccessCommand request, CancellationToken cancellationToken)
        {
            if(RequestGuard.TryGetFailureResult(request, out var resultRequest))
                return resultRequest;

            if (ValidationGuard.TryGetFailureResult(await _validator.ValidateAsync(request), out var validationResult))
                return validationResult;

            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == request.Login && u.Email == request.Email);

                if (user is null)
                    return Result.Success();

                // TODO: [SECURITY] Небезопасный одношаговый процесс восстановления.
                // Реализовать второй шаг - проверка токена из email.

                var newPasswordHash = _hasher.HashPassword(request.NewPassword);

                user.UpdatePassword(newPasswordHash);

                await _context.SaveChangesAsync();

                user.ClearDomainEvents();

                return Result.Success();
            }
            catch (IdenticalPasswordsException ex)
            {
                return Result<UserDto>.Failure(new Error(ErrorCode.IdenticalPasswords, string.Empty, ex.Message));
            }
            catch (Exception ex)
            { 
               // TODO: [LOGGING] Разделить логирование и ответ клиенту.
               // Сейчас системное сообщение (systemMessage) попадает в объект Error. В будущем нужно использовать ILogger для записи systemMessage, а клиенту возвращать только clientMessage.

                var systemMessage = $"Произошла ошибка: Основное исключение: {ex.Message}. Внутреннее исключение {ex.InnerException?.Message}. При восстановление доступа к аккаунту в {nameof(RecoveryAccessCommandHandler)}";
                var clientMessage = "Произошла критическая ошибки на стороне сервера при восстановление доступа";

                return Result<UserDto>.Failure(new Error(ErrorCode.Server, systemMessage, clientMessage));
            }
        }
    }
}