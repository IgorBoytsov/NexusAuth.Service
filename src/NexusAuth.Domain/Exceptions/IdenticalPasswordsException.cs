namespace NexusAuth.Domain.Exceptions
{
    public sealed class IdenticalPasswordsException : DomainException
    {
        public IdenticalPasswordsException(string message) : base(message)
        {
        }

        public IdenticalPasswordsException(string message, Exception innerEx) : base(message, innerEx)
        {
        }
    }
}