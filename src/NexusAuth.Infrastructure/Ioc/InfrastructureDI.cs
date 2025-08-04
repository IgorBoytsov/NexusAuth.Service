using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NexusAuth.Application.Common.Abstractions;
using NexusAuth.Infrastructure.Persistence;

namespace NexusAuth.Infrastructure.Ioc
{
    public static class InfrastructureDI
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<AuthDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            services.AddScoped<IApplicationDbContext>(provider =>
                    provider.GetRequiredService<AuthDbContext>());
            services.AddScoped<IUnitOfWork>(provider =>
                    provider.GetRequiredService<AuthDbContext>());

            return services;
        }
    }
}