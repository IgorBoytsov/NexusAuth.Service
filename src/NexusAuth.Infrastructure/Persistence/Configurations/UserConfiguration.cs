using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NexusAuth.Domain.Models;
using NexusAuth.Domain.ValueObjects.User;

namespace NexusAuth.Infrastructure.Persistence.Configurations
{
    internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {
        private const string Case_Insensitive = "SQL_Latin1_General_CP1_CI_AS";
        private const string Case_Sensitive = "SQL_Latin1_General_CP1_CS_AS";

        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            /*__IDUser__*/

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id)
              .HasColumnName("Id")
              .ValueGeneratedNever();

            /*__Login__*/

            builder.Property(u => u.Login)
                .HasConversion(
                    login => login.Value,
                    dbValue => new Login(dbValue))
                .HasColumnName("Login")
                .HasMaxLength(Login.MAX_LENGTH)
                .UseCollation(Case_Insensitive)
                .IsUnicode(false)
                .IsRequired();

            builder.HasIndex(u => u.Login, "IX_Users_Login").IsUnique();

            /*__UserName__*/

            builder.Property(u => u.UserName)
                .HasConversion(
                    userName => userName.Value,
                    dbValue => new UserName(dbValue))
                .HasMaxLength(UserName.MAX_LENGTH)
                .UseCollation(Case_Insensitive)
                .HasColumnName("UserName")
                .IsRequired();

            /*__PasswordHash__*/

            builder.Property(u => u.PasswordHash)
                .HasConversion(
                    passHash => passHash.Value,
                    dbValue => new PasswordHash(dbValue))
                .HasColumnName("PasswordHash")
                .IsUnicode(false)
                .IsRequired();

            /*__Email__*/

            builder.Property(u => u.Email)
                .HasConversion(
                    email => email.Value,
                    dbValue => new Email(dbValue))
                .HasColumnName("Email")
                .UseCollation(Case_Insensitive)
                .IsUnicode(false)
                .IsRequired();

            builder.HasIndex(u => u.Email, "IX_Users_Email").IsUnique();

            /*__Phone__*/

            builder.Property(u => u.Phone)
                .HasConversion(
                    phone => phone != null ? phone.Value : null,
                    dbValue => !string.IsNullOrEmpty(dbValue) ? new Phone(dbValue) : null)
                .HasMaxLength(20)
                .HasColumnName("Phone")
                .IsUnicode(false)
                .IsRequired(false);

            builder.HasIndex(u => u.Phone, "IX_Users_Phone")
                .IsUnique()
                .HasFilter("[Phone] IS NOT NULL");

            /*__Dates__*/

            builder.Property(u => u.DateRegistration)
                .HasColumnName("DateRegistration").IsRequired();

            builder.Property(u => u.DateUpdate)
                .HasColumnName("DateUpdate").IsRequired();

            builder.Property(u => u.DateEntry)
                .HasColumnName("DateEntry").IsRequired(false);

            /*__Ids__*/

            builder.Property(u => u.IdStatus)
                .HasConversion(
                    idStatus => idStatus.Value,
                    dbValue => new StatusId(dbValue))
                .HasColumnName("IdStatus")
                .IsRequired();

            builder.Property(u => u.IdRole)
                .HasConversion(
                    idRole => idRole.Value,
                    dbValue => new RoleId(dbValue))
                .HasColumnName("IdRole")
                .IsRequired();

            builder.Property(u => u.IdGender)
                .HasConversion(
                    idGender => idGender.HasValue ? idGender.Value.Value : (int?)null,
                    dbValue => dbValue.HasValue ? new GenderId(dbValue.Value) : (GenderId?)null)
                .HasColumnName("IdGender")
                .IsRequired(false);

            builder.Property(u => u.IdCountry)
                .HasConversion(
                    idCountry => idCountry.HasValue ? idCountry.Value.Value : (int?)null,
                    dbValue => dbValue.HasValue ? new CountryId(dbValue.Value) : (CountryId?)null)
                .HasColumnName("IdCountry")
                .IsRequired(false);

            /*__Связи__*/

            builder.HasOne(u => u.Gender)
                .WithMany()
                .HasForeignKey(u => u.IdGender)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(u => u.Country)
                .WithMany()
                .HasForeignKey(u => u.IdCountry)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(u => u.Role)
                .WithMany()
                .HasForeignKey(u => u.IdRole)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(u => u.Status)
                .WithMany()
                .HasForeignKey(u => u.IdStatus)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Ignore("_domainEvents");
        }
    }
}