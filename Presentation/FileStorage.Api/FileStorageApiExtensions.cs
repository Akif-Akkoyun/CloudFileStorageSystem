using Microsoft.AspNetCore.Http.Features;
using Serilog;

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
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Logger(lc => lc
                .Filter.ByIncludingOnly(le =>
                le.MessageTemplate.Text.Contains("File upload requested") ||
                le.MessageTemplate.Text.Contains("File saved successfully") ||
                le.MessageTemplate.Text.Contains("Generated file path") ||
                le.MessageTemplate.Text.Contains("File download requested") ||
                le.MessageTemplate.Text.Contains("File not found for download") ||
                le.MessageTemplate.Text.Contains("File upload attempted with empty or null file"))
                .WriteTo.File("logs/filestorage-log.txt", rollingInterval: RollingInterval.Day))
                .CreateLogger();
        }
    }
}
