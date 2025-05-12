using App.Persistence.Auth;
using Serilog;

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
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Logger(lc => lc
                .Filter.ByIncludingOnly(le =>
                le.MessageTemplate.Text.Contains("Login attempt for email") ||
                le.MessageTemplate.Text.Contains("Login successful for user with email") ||
                le.MessageTemplate.Text.Contains("Unauthorized login attempt") ||
                le.MessageTemplate.Text.Contains("Register attempt with null data") ||
                le.MessageTemplate.Text.Contains("User registration failed for email") ||
                le.MessageTemplate.Text.Contains("New user registered with email") ||
                le.MessageTemplate.Text.Contains("ForgotPassword validation failed for email") ||
                le.MessageTemplate.Text.Contains("ForgotPassword requested for email") ||
                le.MessageTemplate.Text.Contains("RenewPassword validation failed for email") ||
                le.MessageTemplate.Text.Contains("RenewPassword attempted for email") ||
                le.MessageTemplate.Text.Contains("User logged out.") ||
                le.MessageTemplate.Text.Contains("Fetching user with ID") ||
                le.MessageTemplate.Text.Contains("Fetching all users"))
                .WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day))
                .CreateLogger();
        }
    }
}