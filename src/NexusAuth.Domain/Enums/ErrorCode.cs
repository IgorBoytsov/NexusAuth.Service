namespace NexusAuth.Domain.Enums
{
    public enum ErrorCode
    {
        None,

        Empty,
        NullValue,

        Validation,

        NotFound,
        Save,
        Update,
        Delete,
        
        Network
    }
}