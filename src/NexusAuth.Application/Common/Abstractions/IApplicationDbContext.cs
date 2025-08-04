using Microsoft.EntityFrameworkCore;
using NexusAuth.Domain.Models;

namespace NexusAuth.Application.Common.Abstractions
{
    public interface IApplicationDbContext : IUnitOfWork
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Country> Countries { get; set; }
    }
}