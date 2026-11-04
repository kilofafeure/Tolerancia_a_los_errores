using Polly;
using Polly.Timeout;

var timeoutPollyOptimistic = Policy.TimeoutAsync(
        timeout: TimeSpan.FromSeconds(20),
        timeoutStrategy: TimeoutStrategy.Optimistic,
        onTimeoutAsync: (context, timespan, task, exception) =>
        {
            Console.WriteLine($"[{context["Id"]}] - Too much time in operation:  {timespan.TotalSeconds} seconds");
            return Task.CompletedTask;
        }
    );

using var cts = new CancellationTokenSource();

var task = Task.Run(async () =>
{
    try
    {
        var context = new Context
        {
            ["Id"] = "sdfsasdaf"
        };
        await timeoutPollyOptimistic.ExecuteAsync(async (context, token) =>
        {
            //await Operation(TimeSpan.FromSeconds(10), token);
            await Execute(token);
        }, context, cts.Token);
    }
    catch (OperationCanceledException ocex)
    {
        Console.WriteLine($"Cancelled manually: {ocex.Message} ");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Other excepcion: {ex.GetType().Name} - {ex.Message}");
    }
});

await Task.Delay(5000);
Console.WriteLine("Cancelling manually");
cts.Cancel();

await task;

// emulates a process its executing during hours
static async Task Execute(CancellationToken token)
{
    while (true)
    {
        Console.WriteLine("Doing something...");
        await Task.Delay(1000, token);
    }
}