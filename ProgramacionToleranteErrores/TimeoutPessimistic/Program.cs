using Polly;
using Polly.Timeout;

// only use pessimistic in legacy programs

var timeoutPollyPessimistic = Policy.TimeoutAsync(
        timeout: TimeSpan.FromSeconds(2),
        timeoutStrategy: TimeoutStrategy.Pessimistic,
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

    await timeoutPollyPessimistic.ExecuteAsync(async (context) =>
    {
        //await Operation(TimeSpan.FromSeconds(10));
        await Execute(); // Althought Timeout Pattern it allways its executing
    }, context);
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

static async Task Operation(TimeSpan delay)
{
    Console.WriteLine("Doing something...");
    await Task.Delay(delay);
}

// emulates a process its executing during hours
static async Task Execute()
{
    while (true) {
        Console.WriteLine("Doing something...");
        await Task.Delay(1000);
    }
}