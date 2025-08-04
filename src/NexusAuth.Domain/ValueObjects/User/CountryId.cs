namespace NexusAuth.Domain.ValueObjects.User
{
    public readonly record struct CountryId
    {
        public int Value { get; }

        internal CountryId(int value) => Value = value;

        public static CountryId From(int value)
        {
            if (value <= 0)
                throw new ArgumentException("Id статуса должен быть положительным числом.", nameof(value));

            return new CountryId(value);
        }

        public static implicit operator int(CountryId countryId) => countryId.Value;
    }
}