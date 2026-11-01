using System.ComponentModel.DataAnnotations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapPost("/products", (Product product) =>
{
    var validationContext = new ValidationContext(product);
    var validationResults = new List<ValidationResult>();
    
    if (!Validator.TryValidateObject(product, validationContext, validationResults, true))
    {
        //return Results.ValidationProblem(validationResults.ToDictionary(vr => vr.MemberNames.FirstOrDefault() ?? string.Empty, vr => new[] { vr.ErrorMessage ?? string.Empty }));
        return Results.BadRequest(validationResults.Select(vr => new { vr.MemberNames, vr.ErrorMessage }));
    }

    // Eun una aplicación de ejemplo, normalmente guardarías el producto en una base de datos aquí.
    return Results.Created($"/products/{product.Id}", product);
}); 

app.Run();

public class Product
{
    public int Id { get; set; }

    [Required(ErrorMessage = "The product name is required.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "The product name must be between 3 and 100 characters.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "The product price is required.")]
    [Range(0.01, 10000.00, ErrorMessage = "The product price must be between 0.01 and 10000.00.")]
    public decimal Price { get; set; }
}