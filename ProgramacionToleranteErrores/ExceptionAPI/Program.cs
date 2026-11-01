using ProductRepository_Exception;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<ProductRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("products/{id}", async (int id, ProductRepository repository) =>
{
    try
    {
        var product = repository.GetById(id);
        return Results.Ok(product);
    }
    catch (KeyNotFoundException ex)
    {
        return Results.NotFound(ex.Message);
    }
    catch (Exception ex)
    {
        return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
    }
});


app.UseAuthorization();

app.MapControllers();

app.Run();
