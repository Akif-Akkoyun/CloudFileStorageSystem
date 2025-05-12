using App.Application.Dtos.AuthDtos;
using App.Application.Features.File.Handlers;
using App.Application.Interfaces.Auth;
using App.Persistence.Auth;
using App.Persistence.Auth.Repositories;
using App.Persistence.Auth.Services.AuthServices;
using App.Persistence.Auth.Services.MailServices;
using Duende.IdentityModel;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Authentication.Api
{
    public static class AuthExtensions
    {
        public static void AddAuthDataLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
                throw new InvalidOperationException("Connection string is not set in appsettings.json");
            services.AddAuthApi(connectionString);
        }
    }
}