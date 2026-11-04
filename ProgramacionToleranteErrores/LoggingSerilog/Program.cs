using Serilog;

var logDirectory = Path.Combine(AppContext.BaseDirectory, "logs");
try
{
    Directory.CreateDirectory(logDirectory);
    Console.WriteLine($"Carpeta creada: {logDirectory}");
}
catch (Exception ex)
{
    Console.WriteLine($"Error creando carpeta: {ex.Message}");
    throw;
}

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Serilog configuration
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Verbose()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning) // Override Microsoft logs to Warning level
    .WriteTo.Console(
        outputTemplate: "{Timestamp: yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:w3}] {Message:lj} {RequestId}{NewLine}{Exception}"
    ) 
    .WriteTo.File(
        "logs/log-.txt", 
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 7,
        outputTemplate: "{Timestamp: HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj} {Properties}{NewLine}{Exception}"
    )
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/hi", (ILogger<Program> logger) =>
{
    logger.LogTrace("Entering /hi endpoint.");
    logger.LogInformation("Hi there! {endpoint}", new { endpoint = "/hi" });
    logger.LogWarning("This is a warning log message from /hi endpoint.");
    return "Hi world!";
});

app.Run();
