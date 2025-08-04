using NexusAuth.Domain.Primitives;
using NexusAuth.Domain.ValueObjects.User;

namespace NexusAuth.Domain.Events.User
{
    public sealed record UserCreatedEvent(Guid IdEvent, Guid UserId, Email UserEmail, UserName UserName, DateTime OccurredOnUtc) : IDomainEvent;
}