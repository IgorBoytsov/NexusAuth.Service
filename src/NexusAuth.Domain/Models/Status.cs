using NexusAuth.Domain.Primitives;
using NexusAuth.Domain.ValueObjects.Status;
using NexusAuth.Domain.ValueObjects.User;

namespace NexusAuth.Domain.Models
{
    public sealed class Status : Entity<StatusId>
    {
        public StatusName Name { get; set; } = null!;

        private Status() { }

        public Status(StatusId id, StatusName name) => Name = name;

        public static Status Create(string name)
        {
            var statusNeame = StatusName.Create(name);

            return new Status(default, statusNeame);
        }
    }
}