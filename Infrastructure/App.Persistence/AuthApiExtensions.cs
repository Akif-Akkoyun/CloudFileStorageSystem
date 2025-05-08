using App.Application.Dtos.AuthDtos;
using App.Application.Features.Commads.AuthCommands;
using App.Application.Interfaces;
using App.Persistence.Context;
using App.Persistence.Repositories;
using App.Persistence.Services.AuthServices;
using App.Persistence.Services.MailServices;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
namespace App.Persistence
{
    public static class AuthApiExtensions
    {
        public static void AddAuthApi(this IServiceCollection services,string connectionString)
        {
            services.AddDbContext<AuthDbContext>(options =>
                options.UseSqlite(connectionString));
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IAuthTokenService), typeof(AuthTokenService));
            services.AddScoped(typeof(IMailService), typeof(MailService));
            services.AddValidatorsFromAssemblyContaining<RegisterCommandValidator>();
            services.AddValidatorsFromAssemblyContaining<ResetPasswordCommandValidator>();
            services.AddValidatorsFromAssemblyContaining<ForgotPasswordCommandValidator>();
            services.AddValidatorsFromAssemblyContaining<LoginCommandValidator>();
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(LoginCommand).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(RegisterCommand).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(ForgotPasswordCommand).Assembly);
            });
        }
    }
}
