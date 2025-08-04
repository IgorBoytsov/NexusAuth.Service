using NexusAuth.Domain.Primitives;

namespace NexusAuth.Domain.Events.User
{
    public sealed record UserUpdatePasswordEvent(Guid IdEvent, DateTime OccurredOnUtc, Guid IdUser) : IDomainEvent;
}