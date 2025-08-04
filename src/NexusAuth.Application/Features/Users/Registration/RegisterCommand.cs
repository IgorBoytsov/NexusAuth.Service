using MediatR;
using NexusAuth.Domain.Results;

namespace NexusAuth.Application.Features.Users.Registration
{
    public sealed record RegisterCommand(
        string Login, string UserName, 
        string Password, 
        string Email, string? Phone,
        int? IdGender, int? IdCountry) : IRequest<Result>;
}
