using App.Application.Dtos.FileDtos;
using App.Application.Features.File.Handlers;
using App.Application.Interfaces.File;
using App.Persistence.File.Context;
using App.Persistence.File.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace App.Persistence
{
    public static class FileMetaDataExtensions
    {
        public static void AddFileMetaDataApi(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<FileMetaDataDbContext>(options =>
                options.UseNpgsql(connectionString));

            services.AddScoped<IFileRepository, FileRepository>();
            services.AddScoped<IFileShareRepository, FileShareRepository>();
            services.AddValidatorsFromAssemblyContaining<FileValidator>();
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblyContaining<CreateFileMetaDataCommandHandler>();
            });
        }
    }
}