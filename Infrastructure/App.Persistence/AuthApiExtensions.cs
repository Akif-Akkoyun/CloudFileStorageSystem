using App.Application.Features.Commads.AuthCommands;
using App.Application.Interfaces;
using App.Persistence.Context;
using App.Persistence.Repositories;
using App.Persistence.Services.AuthServices;
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
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(LoginCommand).Assembly);
                cfg.RegisterServicesFromAssembly(typeof(RegisterCommand).Assembly);
            });
        }
    }
}
