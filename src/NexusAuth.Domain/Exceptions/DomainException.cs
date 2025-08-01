namespace NexusAuth.Domain.Exceptions
{
    public abstract class DomainException : Exception
    {
        protected DomainException(string message) : base(message)
        {

        }

        protected DomainException(string message, Exception innerEx) : base(message, innerEx)
        {

        }
    }
}