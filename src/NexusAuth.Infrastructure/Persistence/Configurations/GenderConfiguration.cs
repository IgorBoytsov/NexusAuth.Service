using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexusAuth.Domain.Models;
using NexusAuth.Domain.ValueObjects.Gender;
using NexusAuth.Domain.ValueObjects.User;

namespace NexusAuth.Infrastructure.Persistence.Configurations
{
    internal sealed class GenderConfiguration : IEntityTypeConfiguration<Gender>
    {
        public void Configure(EntityTypeBuilder<Gender> builder)
        {
            builder.ToTable("Genders");

            builder.HasKey(x => x.Id);

            builder.Property(c => c.Id)
              .HasConversion(
                  countryId => countryId.Value,
                  dbValue => new GenderId(dbValue)) 
              .HasColumnName("Id")
              .ValueGeneratedOnAdd();

            builder.Property(u => u.Name)
              .HasConversion(
                  genderName => genderName.Value,
                  dbValue => new GenderName(dbValue))
              .HasMaxLength(GenderName.MAX_LENGTH)
              .HasColumnName("Name")
              .IsRequired();
        }
    }
}