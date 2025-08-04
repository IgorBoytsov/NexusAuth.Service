namespace NexusAuth.Domain.ValueObjects.User
{
    public readonly record struct RoleId
    {
        public int Value { get; }

        internal RoleId(int value) => Value = value;

        public static RoleId From(int value)
        {
            if (value <= 0)
                throw new ArgumentException("Id роли должен быть положительным числом.", nameof(value));

            return new RoleId(value);
        }

        public static implicit operator int(RoleId id) => id.Value;
    }
}