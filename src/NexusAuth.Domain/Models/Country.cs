using NexusAuth.Domain.Primitives;
using NexusAuth.Domain.ValueObjects.Country;
using NexusAuth.Domain.ValueObjects.User;

namespace NexusAuth.Domain.Models
{
    public sealed class Country : Entity<CountryId>
    {
        public CountryName Name { get; private set; } = null!;

        private Country() { }

        private Country(CountryId id, CountryName name) : base()
        {
            Name = name;
        }

        public static Country Create(string name)
        {
            var genderName = CountryName.Create(name);

            return new Country(default, genderName);
        }
    }
}