using MediatR;
using NexusAuth.Domain.Results;

namespace NexusAuth.Application.Features.Users.RecoveryAccess
{
    public sealed record RecoveryAccessCommand(string Login, string Email, string NewPassword) : IRequest<Result>; 
}