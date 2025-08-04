namespace NexusAuth.Domain.ValueObjects.User
{
    public readonly record struct GenderId
    {
        public int Value { get; }

        internal GenderId(int value) => Value = value;

        public static GenderId From(int value)
        {
            if (value <= 0)
                throw new ArgumentException("Id гендера должен быть положительным числом.", nameof(value));

            return new GenderId(value);
        }

        public static implicit operator int(GenderId genderId) => genderId.Value;
    }
}