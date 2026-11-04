using Polly;
using Polly.Timeout;

var timeoutPollyOptimistic = Policy.TimeoutAsync(
        timeout: TimeSpan.FromSeconds(2),
        timeoutStrategy: TimeoutStrategy.Optimistic,
        onTimeoutAsync: (context, timespan, task, exception) =>
        {
            Console.WriteLine($"[{context["Id"]}] - Too much time in operation:  {timespan.TotalSeconds} seconds");
            return Task.CompletedTask;
        }
    );

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
    }, context, CancellationToken.None);
}
catch (TimeoutRejectedException toex)
{
    Console.WriteLine($"Polly exception: {toex.Message} ");
}
catch (Exception ex)
{
    Console.WriteLine($"Other excepcion: {ex.GetType().Name} - {ex.Message}");
}

Console.WriteLine("Observing during 5s to check it continues executing");
await Task.Delay(5000);

static async Task Operation(TimeSpan delay, CancellationToken token)
{
    Console.WriteLine("Doing something...");
    await Task.Delay(delay, token);
}

// emulates a process its executing during hours
static async Task Execute(CancellationToken token)
{
    while (true)
    {
        Console.WriteLine("Doing something...");
        await Task.Delay(1000, token);
    }
}