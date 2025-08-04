using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexusAuth.Domain.Models;
using NexusAuth.Domain.ValueObjects.Country;
using NexusAuth.Domain.ValueObjects.User;

namespace NexusAuth.Infrastructure.Persistence.Configurations
{
    internal sealed class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.ToTable("Countries");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
              .HasConversion(
                  countryId => countryId.Value,
                  dbValue => new CountryId(dbValue))
              .HasColumnName("Id")
              .ValueGeneratedOnAdd();

            builder.Property(c => c.Name)
              .HasConversion(
                  genderName => genderName.Value,
                  dbValue => new CountryName(dbValue))
              .HasMaxLength(CountryName.MAX_LENGTH)
              .HasColumnName("Name")
              .IsRequired();
        }
    }
}