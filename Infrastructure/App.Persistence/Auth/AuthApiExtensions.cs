using App.Application.Dtos.AuthDtos;
using App.Application.Features.Auth.Handlers;
using App.Application.Interfaces.Auth;
using App.Persistence.Auth.Context;
using App.Persistence.Auth.Repositories;
using App.Persistence.Auth.Services.AuthServices;
using App.Persistence.Auth.Services.MailServices;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
namespace App.Persistence.Auth
{
    public static class AuthApiExtensions
    {
        public static void AddAuthApi(this IServiceCollection services,string connectionString)
        {
            services.AddDbContext<AuthDbContext>(options =>
                options.UseSqlite(connectionString));

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAuthTokenService, AuthTokenService>();
            services.AddScoped<IMailService, MailService>();

            services.AddValidatorsFromAssemblyContaining<RegisterCommandValidator>();
            services.AddValidatorsFromAssemblyContaining<ResetPasswordCommandValidator>();
            services.AddValidatorsFromAssemblyContaining<ForgotPasswordCommandValidator>();
            services.AddValidatorsFromAssemblyContaining<LoginCommandValidator>();

            services.AddMediatR(cfg =>
            {
                var assemblies = new[] { typeof(LoginCommandHandler).Assembly };
                cfg.RegisterServicesFromAssemblies(assemblies);
            });
        }
    }
}
