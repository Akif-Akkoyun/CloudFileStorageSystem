using App.Application.Dtos.FileDtos;
using App.Application.Features.Auth.Handlers;
using App.Application.Interfaces.File;
using App.Persistence;
using App.Persistence.File.Repositories;
using FluentValidation;

namespace FileMetaData.Api
{
    public static class FileMetaDataExtensions
    {
        public static void AddFileMetaDataLayer(this IServiceCollection services, IConfiguration configuration)
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
            services.AddFileMetaDataApi(connectionString);
        }
    }
}
