using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexusAuth.Domain.Models;
using NexusAuth.Domain.ValueObjects.Role;
using NexusAuth.Domain.ValueObjects.User;

namespace NexusAuth.Infrastructure.Persistence.Configurations
{
    internal sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles");

            builder.HasKey(r => r.Id);

            builder.Property(c => c.Id)
              .HasConversion(
                  countryId => countryId.Value,
                  dbValue => new RoleId(dbValue))
              .HasColumnName("Id")
              .ValueGeneratedOnAdd();

            builder.Property(r => r.Name)
                .HasConversion(
                    roleName => roleName.Value,
                    dbValue => new RoleName(dbValue))
                .HasColumnName("Name")
                .IsRequired();
        }
    }
}