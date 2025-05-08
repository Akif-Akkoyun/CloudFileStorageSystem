using App.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace App.Persistence
{
    public static class FileMetaDataExtensions
    {
        public static void AddFileMetaDataApi(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<DbContext, FileMetaDataDbContext>(options =>
                options.UseNpgsql(connectionString));
        }
    }
}