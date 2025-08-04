namespace NexusAuth.Api.Models.Request
{
    public sealed record class RegisterUserRequest(
        string Login, string UserName,
        string Password,
        string Email, string? Phone,
        int? IdGender, int? IdCountry);
}
