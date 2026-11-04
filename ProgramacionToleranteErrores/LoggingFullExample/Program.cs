using LoggingFullExample;
using Serilog;

var logDirectory = Path.Combine(AppContext.BaseDirectory, "logs");
try
{
    Directory.CreateDirectory(logDirectory);
    Console.WriteLine($"Created folder: {logDirectory}");
}
catch (Exception ex)
{
    Console.WriteLine($"Error creating folder: {ex.Message}");
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

app.MapPost("/checkTotal", (Order order, ILogger<Program> log) =>
{
    log.LogInformation("Calculating total for order: {@Order}", order);
    log.LogInformation("Total items in order: {ItemCount}", order.Details.Count);

    try
    {
        decimal total = 0;

        if (ProductRepository.Products.Count <= 0)
        {
            log.LogWarning("Product repository is empty. No products available for calculation.");
        }

        foreach (var detail in order.Details)
        {
            log.LogDebug("Processing order detail: {@OrderDetail}", detail);
            log.LogDebug("Searching product {IdProduct} with quantity {Quantity}", detail.IdProduct, detail.Quantity);

            var product = ProductRepository.Products.FirstOrDefault(p => p.Id == detail.IdProduct);

            if (product == null)
            {
                log.LogWarning("Product with Id {IdProduct} not found in repository.", detail.IdProduct);
                continue; // Skip this detail if product is not found, we should use some default value or handle it according to business rules. Also we should inform the user about the error on the response
            }
            else
            {
                log.LogDebug("Found product: {ProductName} - Price: {Price}", product.Name, product.Price);
                total += product.Price * detail.Quantity;                
            }
        }

        log.LogDebug("Calculated total: {Total}", total);
        return Results.Ok(total);
    }
    catch (Exception ex)
    {
        log.LogError(ex, "Error calculating total for order: {@Order}", order);
        return Results.Problem(detail: "An error occurred while calculating the total.", statusCode: 500);
    }
});

app.Run();

public record Order(List<OrderDetail> Details);
public record OrderDetail(int IdProduct, int Quantity);
public record Product(int Id, string Name, decimal Price);
