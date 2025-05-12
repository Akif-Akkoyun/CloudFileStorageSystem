using App.Application.Dtos.FileDtos;
using App.Application.Features.Auth.Handlers;
using App.Application.Interfaces.File;
using App.Persistence;
using App.Persistence.File.Repositories;
using FluentValidation;
using Serilog;

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
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Logger(lc => lc
                .Filter.ByIncludingOnly(le =>
                le.MessageTemplate.Text.Contains("Fetching all files for owner") ||
                le.MessageTemplate.Text.Contains("Fetching file metadata by ID") ||
                le.MessageTemplate.Text.Contains("Creating new file metadata for") ||
                le.MessageTemplate.Text.Contains("Updating file metadata with ID") ||
                le.MessageTemplate.Text.Contains("Removing file metadata with ID") ||
                le.MessageTemplate.Text.Contains("Fetching all public files") ||
                le.MessageTemplate.Text.Contains("Fetching files shared with user ID") ||
                le.MessageTemplate.Text.Contains("Fetching all files for owner ID") ||
                le.MessageTemplate.Text.Contains("Toggling visibility for file ID") ||
                le.MessageTemplate.Text.Contains("Sharing file ID") ||
                le.MessageTemplate.Text.Contains("Fetching share info for file ID") ||
                le.MessageTemplate.Text.Contains("Fetching files shared by user ID"))
                .WriteTo.File("logs/filemetadata-log.txt", rollingInterval: RollingInterval.Day))
                .CreateLogger();
        }
    }
}
