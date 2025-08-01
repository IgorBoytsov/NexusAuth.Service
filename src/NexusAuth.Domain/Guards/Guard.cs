namespace NexusAuth.Domain.Guards
{
    public static class Guard
    {
        /// <summary>
        /// Предоставляет доступ к набору стандартных проверок.
        /// </summary>
        public static IGuardClause Against { get; } = new GuardClause();
    }
}