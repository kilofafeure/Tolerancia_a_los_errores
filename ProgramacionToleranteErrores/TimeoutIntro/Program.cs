using Polly;
using Polly.Timeout;

var timeoutPollyPessimistic = Policy.TimeoutAsync(
        timeout: TimeSpan.FromSeconds(2),
        timeoutStrategy: TimeoutStrategy.Pessimistic,
        onTimeoutAsync: (context, timespan, task, exception) => {
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
        await Operation(TimeSpan.FromSeconds(10));
    }, context);
}
catch (TimeoutRejectedException toex) {
    Console.WriteLine($"Polly exception: {toex.Message} ");
}
catch (Exception ex) {
    Console.WriteLine($"Other excepcion: {ex.GetType().Name} - {ex.Message}");
}

async Task Operation(TimeSpan delay) {
    Console.WriteLine("Doing something...");
    await Task.Delay(delay);
}