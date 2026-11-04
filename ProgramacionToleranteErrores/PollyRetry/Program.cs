using Polly;

var httpClient = new HttpClient();

var retryPolicy = Policy.Handle<HttpRequestException>()
                    .WaitAndRetryAsync(
                        retryCount: 3,
                        //sleepDurationProvider: attempt => TimeSpan.FromSeconds(2), // fixed await
                        //sleepDurationProvider: attempt => TimeSpan.FromSeconds(attempt * 2), // incremental await
                        sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)), // exponential await
                        //sleepDurationProvider: attempt => TimeSpan.FromSeconds(Random.Shared.Next(3,8)), // random await
                        onRetry: (exception, timeSpan, retryCount, context) =>
                        {
                            Console.WriteLine($"[{context.OperationKey}]: Age {context["age"]}; Attempt {retryCount} after {timeSpan.TotalSeconds} seconds: {exception.Message}");
                        }
                    );

var context = new Context("UniqueId")
{
    ["age"] = 15
};

try
{
    await retryPolicy.ExecuteAsync(async (context) => {

        Console.WriteLine(context.OperationKey);

        var response = await httpClient.GetAsync("https://somesite.io");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Success resonse: {content[..50]}");

    },
    //new Context("UniqueId")
    context
    );
}
catch(HttpRequestException ex)
{
    Console.WriteLine($"An error has ocurred: {ex.Message}");
}