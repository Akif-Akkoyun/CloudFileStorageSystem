using Microsoft.AspNetCore.Http.Features;

namespace FileStorage.Api
{
    public static class FileStorageApiExtensions
    {
        public static void AddFileStorageExtensiosn(this IServiceCollection services)
        {
            services.Configure<FormOptions>(o =>
            {
                o.MultipartBodyLengthLimit = 100_000_000;
            });
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
        }
    }
}
