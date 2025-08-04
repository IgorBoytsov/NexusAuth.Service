using NexusAuth.Domain.Primitives;
using NexusAuth.Domain.ValueObjects.Gender;
using NexusAuth.Domain.ValueObjects.User;

namespace NexusAuth.Domain.Models
{
    public sealed class Gender : Entity<GenderId>
    {
        public GenderName Name { get; private set; } = null!;

        private Gender() { }

        private Gender(GenderId id, GenderName name) : base()
        {
            Name = name;
        }

        public static Gender Create(string name)
        {
            var genderName = GenderName.Create(name);

            return new Gender(default, genderName);
        }
    }
}