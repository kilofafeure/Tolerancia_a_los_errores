using Polly.Timeout;
using TimeoutAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddTransient<Policies>();
builder.Services.AddTransient<SlowOperation>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("timeout", async (Policies policies, SlowOperation slowOperation) =>
{
    try
    {
        await policies.TimeoutPolicy(2).ExecuteAsync(async (cancellationToken) =>
        {
            await slowOperation.Execute(cancellationToken);
        }, CancellationToken.None);

        return Results.Ok();

    }
    catch (TimeoutRejectedException toex)
    {
        return Results.Problem($"Polly exception: {toex.Message}");
    }
});

app.Run();
