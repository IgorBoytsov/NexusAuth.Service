using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NexusAuth.Application.Common.Abstractions;
using NexusAuth.Application.Helpers;
using NexusAuth.Application.Services.Abstractions;
using NexusAuth.Domain.Enums;
using NexusAuth.Domain.Models;
using NexusAuth.Domain.Results;

namespace NexusAuth.Application.Features.Users.Registration
{
    public sealed class RegisterCommandHandler(IApplicationDbContext context, IPasswordHasher passwordHasher, IValidationService validator) : IRequestHandler<RegisterCommand, Result>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IPasswordHasher _hasher = passwordHasher;
        private readonly IValidationService _validator = validator;

        public async Task<Result> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            if (RequestGuard.TryGetFailureResult<RegisterCommand>(request, out var nullFailureResult))
                return nullFailureResult;

            if (ValidationGuard.TryGetFailureResult(await _validator.ValidateAsync(request), out var failureResult))
                return failureResult;

            try
            {
                string passwordHash = _hasher.HashPassword(request.Password);

                var user = User.Create(request.Login, request.UserName, passwordHash, request.Email, request.Phone, (int)Roles.User, request.IdGender, request.IdCountry);

                await _context.Users.AddAsync(user, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                user.ClearDomainEvents();

                return Result.Success();
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
            {
                var systemMessage = $"Конфликт при сохранении: нарушение уникального индекса. Логин или email уже существует. {ex.Message}";
                var clientMessage = "Пользователь с таким логином или email уже существует.";

                return Result.Failure(new Error(ErrorCode.Conflict, systemMessage, clientMessage));
            }
            catch (Exception ex)
            {
                var systemMessage = $"Произошла ошибка: Основное исключение: {ex.Message}. Внутреннее исключение {ex.InnerException?.Message}. При создание и сохранение данных пользователя в {nameof(RegisterCommandHandler)}";
                var clientMessage = "Произошла критическая ошибка на стороне сервера при регистрации";

                return Result.Failure(new Error(ErrorCode.Save, systemMessage, clientMessage));
            }
        }
    }
}