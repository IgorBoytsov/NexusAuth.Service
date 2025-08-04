namespace NexusAuth.Application.Features.Users
{
    public sealed record UserDto(
        string Id ,string Login, string UserName,
        string Email, string? Phone, 
        DateTime DateRegistration,
        int IdStatus, int IdRole, int? IdGender, int? IdCountry);
}