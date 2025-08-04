namespace NexusAuth.Domain.Primitives
{
    public interface IDomainEvent
    {
        public Guid IdEvent { get; }
        public DateTime OccurredOnUtc { get; }
    }
}