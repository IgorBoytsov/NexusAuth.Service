namespace NexusAuth.Api.Models.Request
{
    public sealed record RecoveryAccessRequest(string Login, string Email, string NewPassword);
}
