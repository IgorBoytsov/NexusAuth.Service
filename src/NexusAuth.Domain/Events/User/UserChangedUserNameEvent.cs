using NexusAuth.Domain.Primitives;
using NexusAuth.Domain.ValueObjects.User;

namespace NexusAuth.Domain.Events.User
{
    public sealed record UserChangedUserNameEvent(
    Guid IdEvent,
    DateTime OccurredOnUtc,
    Guid UserId,
    UserName NewUserName,
    UserName OldUserName,
    Guid? ChangedByUserId) : IDomainEvent;
}