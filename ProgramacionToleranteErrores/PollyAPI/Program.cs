using Polly;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();





builder.Services.AddHttpClient("MetalCodeApi", client =>
{
    client.BaseAddress = new Uri("https://metalcode2.io");
}).AddPolicyHandler(GetRetryPolicy());




builder.Services.AddHttpClient("MetalCodeApiSimpleMode", client =>
{
    client.BaseAddress = new Uri("https://metalcode2.io");
})
.AddTransientHttpErrorPolicy(policyBuilder =>
{
    return policyBuilder.WaitAndRetryAsync(5, 
                                           attempt => TimeSpan.FromSeconds(2), 
                                           onRetry: (response, time, attempt, context) =>
                                           {
                                               Console.WriteLine($"Attempt {attempt}");
                                           });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/checkstatus", async (IHttpClientFactory httpClientFactory) => {
    var client = httpClientFactory.CreateClient("MetalCodeApi");
    try
    {
        Console.WriteLine("Calling external API");
        var response = await client.GetAsync("status");

        if (!response.IsSuccessStatusCode)
            return Results.Problem($"Failling external API with code {response.StatusCode}");

        var content = await response.Content.ReadAsStringAsync();
        return Results.Ok($"Ok response: {content}");

    }
    catch (Exception ex) {
        return Results.Problem($"Error calling external API: {ex.Message}");
    }
});

app.MapGet("/checkstatussimplemode", async (IHttpClientFactory httpClientFactory) => {
    var client = httpClientFactory.CreateClient("MetalCodeApiSimpleMode");
    try
    {
        Console.WriteLine("Calling external API");
        var response = await client.GetAsync("status");

        if (!response.IsSuccessStatusCode)
            return Results.Problem($"Failling external API with code {response.StatusCode}");

        var content = await response.Content.ReadAsStringAsync();
        return Results.Ok($"Ok response: {content}");

    }
    catch (Exception ex)
    {
        return Results.Problem($"Error calling external API: {ex.Message}");
    }
});

app.Run();

IAsyncPolicy<HttpResponseMessage> GetRetryPolicy() {
    return Policy.Handle<HttpRequestException>()
                .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: attempt => TimeSpan.FromSeconds(2),
                    onRetry: (exception, timeSpan, retryCount, context) =>
                    {
                        if (exception.Exception != null)
                        {
                            Console.WriteLine($"Exception: {exception.Exception.Message}");
                        }
                        else
                        {
                            Console.WriteLine($"Error code: {exception.Result.StatusCode}");
                        }

                        Console.WriteLine($"Attempt {retryCount} after {timeSpan.TotalSeconds} seconds...");
                    }
                );
}