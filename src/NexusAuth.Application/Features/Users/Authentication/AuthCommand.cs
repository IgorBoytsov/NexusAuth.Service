using MediatR;
using NexusAuth.Domain.Results;

namespace NexusAuth.Application.Features.Users.Authentication
{
    public sealed record AuthCommand(string Password, string Login, string Email) : IRequest<Result<UserDto>>;
}
