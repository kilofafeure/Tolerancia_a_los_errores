using CircuitBreakerAPI;
using Polly.CircuitBreaker;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddHttpClient("CircuitBreakerAPI", client =>
{
    client.BaseAddress = new Uri("https://jsonplaceholder2.typicode.com/");
}).AddPolicyHandler(Policies.GetCircuitBreakerPolicy());

builder.Services.AddHttpClient("AdvancedCircuitBreakerAPI", client =>
{
    client.BaseAddress = new Uri("https://jsonplaceholder2.typicode.com/");
}).AddPolicyHandler(Policies.GetAdvancedCircuitBreakerPolicy());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/posts", async(IHttpClientFactory factory) =>
{
    var client = factory.CreateClient("CircuitBreakerAPI");

    try
    {
        var response = await client.GetAsync("/posts");
        
        if (!response.IsSuccessStatusCode) {
            return Results.Problem($"Http code: {response.StatusCode}");
        }

        var content = await response.Content.ReadAsStringAsync();
        return Results.Ok(content);
    }
    catch (BrokenCircuitException) {
        return Results.Problem("Circuit opened, service blocked");
    }
    catch (Exception ex)
    {
        return Results.Problem($"Unexpected error: {ex.Message}");
    }
});


app.MapGet("/advancedposts", async (IHttpClientFactory factory) =>
{
    var client = factory.CreateClient("AdvancedCircuitBreakerAPI");

    try
    {
        var response = await client.GetAsync("/posts");

        if (!response.IsSuccessStatusCode)
        {
            return Results.Problem($"Http code: {response.StatusCode}");
        }

        var content = await response.Content.ReadAsStringAsync();
        return Results.Ok(content);
    }
    catch (BrokenCircuitException)
    {
        return Results.Problem("Circuit opened, service blocked");
    }
    catch (Exception ex)
    {
        return Results.Problem($"Unexpected error: {ex.Message}");
    }
});


app.Run();
