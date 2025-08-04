namespace NexusAuth.Api.Models.Request
{
    public sealed record LoginUserRequest(string Password, string Login, string Email);
}