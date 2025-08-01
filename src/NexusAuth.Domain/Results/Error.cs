using NexusAuth.Domain.Enums;

namespace NexusAuth.Domain.Results
{
    public sealed record Error(ErrorCode Code, string SystemMessage, string? ClientMessage);
}