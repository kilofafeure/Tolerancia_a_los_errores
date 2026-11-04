using Polly;
using Polly.CircuitBreaker;

var circuitBreakerPolicy = Policy.Handle<HttpRequestException>()
    .CircuitBreakerAsync(
        exceptionsAllowedBeforeBreaking: 2, 
        durationOfBreak: TimeSpan.FromSeconds(5),
            onBreak: (exception, brekDelay) =>
            {
                Console.WriteLine($"Circuit breaker opened for {brekDelay.TotalSeconds} seconds due to: {exception.Message}");
            },
            onReset: () =>
            {
                Console.WriteLine("Circuit breaker reset.");
            },
            onHalfOpen: () =>
            {
                Console.WriteLine("Circuit breaker is half-open. Next call is a trial.");
            }
    );

using var httpClient = new HttpClient();

for (int i = 1; i <= 10; i++)
{
    Console.WriteLine($"Attempt {i}: Calling external API");

    try
    {
        var response = await circuitBreakerPolicy.ExecuteAsync(() => httpClient.GetAsync("https://metalcode2.io/status"));
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine($"Failing external API with code {response.StatusCode}");
        }
        else
        {
            var content = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Ok response: {content}");
        }
    }
    catch (BrokenCircuitException exbc)
    {
        Console.WriteLine($"Circuit opened, call won't be made: {exbc.Message}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error calling external API: {ex.Message}");
    }

    await Task.Delay(2000); // Wait for 1 second before the next attempt
}