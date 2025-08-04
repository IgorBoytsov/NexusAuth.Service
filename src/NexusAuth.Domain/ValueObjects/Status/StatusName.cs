using NexusAuth.Domain.Exceptions.Guard;
using NexusAuth.Domain.ValueObjects.User;

namespace NexusAuth.Domain.ValueObjects.Status
{
    public sealed record StatusName
    {
        public string Value { get; }

        internal StatusName(string value) => Value = value;

        public static StatusName Create(string name)
        {
            Guard.Against.NullOrEmptyOrWhiteSpace(name, nameof(name));

            return new StatusName(name);
        }

        public static implicit operator string(StatusName statusName) => statusName.Value;
    }
}