using NexusAuth.Domain.Primitives;
using NexusAuth.Domain.ValueObjects.Role;
using NexusAuth.Domain.ValueObjects.User;

namespace NexusAuth.Domain.Models
{
    public sealed class Role : Entity<RoleId>
    {
        public RoleName Name { get; set; } = null!;

        private Role() { }

        public Role(RoleId id, RoleName name) => Name = name;

        public static Role Create(string name)
        {
            var roleNeame = RoleName.Create(name);

            return new Role(default, roleNeame);
        }
    }
}