using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NexusAuth.Application.Mappers.Profiles;
using NexusAuth.Application.Services.Abstractions;
using NexusAuth.Application.Services.Implementations;
using NexusAuth.Infrastructure.Ioc;
using System.Text;

namespace NexusAuth.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })

            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
                    ValidateIssuerSigningKey = true,
                };
            });


            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(InfrastructureDI).Assembly));
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(IValidationService).Assembly)); //Application
            builder.Services.AddValidatorsFromAssembly(typeof(IValidationService).Assembly); //Application
            builder.Services.AddAutoMapper(typeof(UserProfile).Assembly);

            builder.Services.AddScoped<IValidationService, ValidationService>();
            builder.Services.AddSingleton<IPasswordHasher, Argon2PasswordHasher>();

            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

            var app = builder.Build();
            if (app.Environment.IsDevelopment())
                app.MapOpenApi();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}