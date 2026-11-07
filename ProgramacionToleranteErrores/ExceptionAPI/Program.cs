using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProductRepository_Exception;
using System.ComponentModel.DataAnnotations;

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

app.UseExceptionHandler(error => {
    error.Run(async context =>
    {
        var ex = context.Features.Get<IExceptionHandlerFeature>()?.Error;
        var (status, title) = ex switch
        {
            ValidationException or ArgumentException or BadHttpRequestException 
                => (StatusCodes.Status400BadRequest, "Error validation"),
            KeyNotFoundException
                => (StatusCodes.Status400BadRequest, "Resource not found"),
            TimeoutException
                => (StatusCodes.Status504GatewayTimeout, "Request timeout"),
            _
                => (StatusCodes.Status500InternalServerError, "Internal server error")
        };

        var problem = new ProblemDetails
        {
            Status = status,
            Title = title,
            Detail = ex?.Message,
            Instance = context.Request.Path
        };

        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = status;
        await context.Response.WriteAsJsonAsync(problem);
    });
});

app.UseHttpsRedirection();

app.MapGet("products/{id}", async (int id, ProductRepository repository) =>
{
    //// sin middleware

    //try
    //{
    //    var product = repository.GetById(id);
    //    return Results.Ok(product);
    //}
    //catch (KeyNotFoundException ex)
    //{
    //    return Results.NotFound(ex.Message);
    //}
    //catch (Exception ex)
    //{
    //    return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
    //}

    //// con middleware
    
    var product = repository.GetById(id);
    return Results.Ok(product);
});


app.UseAuthorization();

app.MapControllers();

app.Run();
