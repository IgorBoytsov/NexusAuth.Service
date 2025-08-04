using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexusAuth.Domain.Models;
using NexusAuth.Domain.ValueObjects.Status;
using NexusAuth.Domain.ValueObjects.User;

namespace NexusAuth.Infrastructure.Persistence.Configurations
{
    internal sealed class StatusConfiguration : IEntityTypeConfiguration<Status>
    {
        public void Configure(EntityTypeBuilder<Status> builder)
        {
            builder.ToTable("Statuses");

            builder.HasKey(r => r.Id);

            builder.Property(c => c.Id)
              .HasConversion(
                  countryId => countryId.Value,
                  dbValue => new StatusId(dbValue))
              .HasColumnName("Id")
              .ValueGeneratedOnAdd();

            builder.Property(r => r.Name)
                .HasConversion(
                    roleName => roleName.Value,
                    dbValue => new StatusName(dbValue))
                .HasColumnName("Name")
                .IsRequired();
        }
    }
}