using Microsoft.EntityFrameworkCore;
using NexusAuth.Application.Common.Abstractions;
using NexusAuth.Domain.Models;
using System.Reflection;

namespace NexusAuth.Infrastructure.Persistence
{
    public class AuthDbContext : DbContext, IApplicationDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Country> Countries { get; set; }

        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}