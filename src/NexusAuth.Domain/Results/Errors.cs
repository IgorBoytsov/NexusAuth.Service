using NexusAuth.Domain.Enums;

namespace NexusAuth.Domain.Results
{
    public static class Errors
    {
        public static class General
        {
            public static Error Empty(string systemMessage, string? clientMessage = null)
                => new(ErrorCode.Empty, systemMessage, clientMessage);

            public static Error NullValue(string systemMessage, string? clientMessage = null)
                => new(ErrorCode.NullValue, systemMessage, clientMessage);
        }
    }
}