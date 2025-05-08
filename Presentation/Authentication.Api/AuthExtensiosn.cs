using App.Persistence;

namespace Authentication.Api
{
    public static class AuthExtensiosn
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
