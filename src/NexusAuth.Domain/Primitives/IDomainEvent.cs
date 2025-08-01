namespace NexusAuth.Domain.Primitives
{
    public interface IDomainEvent
    {
        public Guid Id { get; }
        public DateTime OccurredOnUtc { get; }
    }
}