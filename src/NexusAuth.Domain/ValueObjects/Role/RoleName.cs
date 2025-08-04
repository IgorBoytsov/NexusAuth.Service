using NexusAuth.Domain.Exceptions.Guard;
using NexusAuth.Domain.ValueObjects.User;

namespace NexusAuth.Domain.ValueObjects.Role
{
    public sealed record RoleName
    {
        public string Value { get; }

        internal RoleName(string value) => Value = value;

        public static RoleName Create(string name)
        {
            Guard.Against.NullOrEmptyOrWhiteSpace(name, nameof(name));

            return new RoleName(name);
        }

        public static implicit operator string(RoleName roleName) => roleName.Value;
    }
}