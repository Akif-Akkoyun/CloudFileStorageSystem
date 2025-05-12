using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
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
    .MinimumLevel.Information()
    .WriteTo.Logger(lc => lc
        .Filter.ByIncludingOnly(le =>
            le.MessageTemplate.Text.Contains("Request forwarded to") ||
            le.MessageTemplate.Text.Contains("Route not matched") ||
            le.MessageTemplate.Text.Contains("Forwarding failed")
        )
        .WriteTo.File("logs/gateway-log.txt", rollingInterval: RollingInterval.Day)
    )
    .CreateLogger();

builder.Host.UseSerilog();

builder.Host.UseSerilog();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.Use(async (context, next) =>
{
    var path = context.Request.Path.ToString();
    var method = context.Request.Method;
    var statusCode = 0;

    string destinationService = path.StartsWith("/api/auth") ? "Auth.Api"
                            : path.StartsWith("/api/files") ? "FileMetaData.Api"
                            : path.StartsWith("/api/storages") ? "FileStorage.Api"
                            : "Unknown";
    try
    {
        await next();
        statusCode = context.Response.StatusCode;
        if (destinationService == "Unknown")
        {
            Log.Warning("Route not matched | Path: {Path} | Method: {Method}", path, method);
        }
        else
        {
            Log.Information("Request forwarded to: {Service} | Path: {Path} | Method: {Method} | Status: {StatusCode}",
                destinationService, path, method, statusCode);
        }
    }
    catch (Exception ex)
    {
        Log.Error(ex, "Forwarding failed | Service: {Service} | Path: {Path} | Method: {Method}", destinationService, path, method);
        throw;
    }
});
app.MapGet("/health", () => Results.Ok("Gateway API is healthy!"));
app.UseHttpsRedirection();
app.UseForwardedHeaders();
app.UseCors("AllowAllOrigins");
app.UseAuthorization();
app.MapReverseProxy();
await app.RunAsync();