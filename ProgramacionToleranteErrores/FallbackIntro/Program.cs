using Polly;

var fallbackPolicy = Policy<decimal>
    .Handle<HttpRequestException>()
    .FallbackAsync(
        fallbackValue: 18.03m, // default value to return when the fallback is triggered
        onFallbackAsync: async (exception, context) =>
        {
            Console.WriteLine($"Fallback: Service not responding, executed due to: {exception.Exception.Message}");
            await Task.CompletedTask;
        });

decimal dollarRate = await fallbackPolicy.ExecuteAsync(async () =>
{
    return await GetDollarRate();
});

Console.WriteLine($"Dollar rate: {dollarRate}");

static async Task<decimal> GetDollarRate()
{
    Console.WriteLine("Calling the service to get the dollar rate...");
    throw new HttpRequestException("Service is down");
}