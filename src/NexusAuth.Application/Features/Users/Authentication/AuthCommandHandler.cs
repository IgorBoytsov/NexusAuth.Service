using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NexusAuth.Application.Common.Abstractions;
using NexusAuth.Application.Helpers;
using NexusAuth.Application.Services.Abstractions;
using NexusAuth.Domain.Enums;
using NexusAuth.Domain.Results;

namespace NexusAuth.Application.Features.Users.Authentication
{
    public sealed class AuthCommandHandler(IApplicationDbContext context, IPasswordHasher hasher, IMapper mapper, IValidationService validator) : IRequestHandler<AuthCommand, Result<UserDto>>
    {
        private readonly IApplicationDbContext _context = context;
        private readonly IPasswordHasher _hasher = hasher;
        private readonly IMapper _mapper = mapper;
        private readonly IValidationService _validator = validator;

        public async Task<Result<UserDto>> Handle(AuthCommand request, CancellationToken cancellationToken)
        {
            if (RequestGuard.TryGetFailureResult<AuthCommand, UserDto>(request, out var result))
                return result;

            if(ValidationGuard.TryGetFailureResult<UserDto>(await _validator.ValidateAsync(request), out var validationResult))
                return validationResult;

            try
            {
                var storageUser = await _context.Users.FirstOrDefaultAsync(u => u.Login == request.Login && u.Email == request.Email, cancellationToken);

                if (storageUser is null)
                    return Result<UserDto>.Failure(new Error(ErrorCode.NotFound, string.Empty, "Неверный логин или пароль."));

                var verifiablePassword = _hasher.VerifyPassword(request.Password, storageUser.PasswordHash);

                if (!verifiablePassword)
                    return Result<UserDto>.Failure(new Error(ErrorCode.InvalidPassword, string.Empty, "Неверный логин или пароль."));

                storageUser.UpdateLastEntryDate();

                await _context.SaveChangesAsync(cancellationToken);

                var userDto = _mapper.Map<UserDto>(storageUser);

                return Result<UserDto>.Success(userDto);
            }
            catch (Exception ex)
            {
                var systemMessage = $"Произошла ошибка: Основное исключение: {ex.Message}. Внутреннее исключение {ex.InnerException?.Message}. При аутентификации пользователя в {nameof(AuthCommandHandler)}";
                var clientMessage = "Произошла критическая ошибки на стороне сервера при аутентификации";

                return Result<UserDto>.Failure(new Error(ErrorCode.Server, systemMessage, clientMessage));
            }
        }
    }
}