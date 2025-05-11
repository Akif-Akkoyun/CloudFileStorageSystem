namespace App.WebUI
{
    public static class AddMvcExtensions
    {
        public static void AddMvcLayer(this IServiceCollection services)
        {
            services.AddHttpClient("GatewayAPI", (sp, client) =>
            {
                var config = sp.GetRequiredService<IConfiguration>();
                var gatewayUrl = config["GatewaySettings:BaseUrl"];
                client.BaseAddress = new Uri(gatewayUrl!);
            });
            services.AddAutoMapper(typeof(MappingProfile));
        }
    }
}
