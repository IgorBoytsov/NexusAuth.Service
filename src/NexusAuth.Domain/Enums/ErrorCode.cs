namespace NexusAuth.Domain.Enums
{
    public enum ErrorCode
    {
        None,

        Empty,
        NullValue,
        NullValueCommand,

        Validation,

        Conflict,
        NotFound,
        Save,
        Update,
        Delete,
        InvalidPassword,
        IdenticalPasswords,

        Server,
        Network
    }
}