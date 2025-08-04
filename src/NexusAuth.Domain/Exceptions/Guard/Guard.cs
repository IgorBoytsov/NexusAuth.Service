namespace NexusAuth.Domain.Exceptions.Guard
{
    public static class Guard
    {
        /// <summary>
        /// Предоставляет доступ к набору стандартных проверок.
        /// </summary>
        public static IGuardClause Against { get; } = new GuardClause();
    }
}