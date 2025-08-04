namespace NexusAuth.Domain.ValueObjects.User
{
    public readonly record struct StatusId
    {
        public int Value { get; }

        internal StatusId(int value) => Value = value;

        public static StatusId From(int value)
        {
            if (value <= 0)
                throw new ArgumentException("Id страны должен быть положительным числом.", nameof(value));

            return new StatusId(value);
        }

        public static implicit operator int(StatusId statusId) => statusId.Value;
    }
}