using Duende.IdentityModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace App.WebUI
{
    public static class AddMvcExtensions
    {
        public static void AddMvcLayer(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddHttpClient("GatewayAPI", (sp, client) =>
            {
                var config = sp.GetRequiredService<IConfiguration>();
                var gatewayUrl = config["GatewaySettings:BaseUrl"];
                client.BaseAddress = new Uri(gatewayUrl!);
            });
            services.AddAutoMapper(typeof(MappingProfile));            

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    
                    {
                        var token = context.Request.Cookies["auth-token"];
                        if (!string.IsNullOrEmpty(token))
                        {
                            context.Token = token;
                        }
                        return Task.CompletedTask;
                    }
                };
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"] ?? throw new InvalidOperationException("Key not found")))
                };
            });
        }
    }
}
