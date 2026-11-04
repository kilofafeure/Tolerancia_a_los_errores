using Polly;
using Polly.Timeout;

namespace TimeoutAPI
{
    public class Policies
    {
        public IAsyncPolicy TimeoutPolicy(int seconds) {
            
            return Policy.TimeoutAsync(
                TimeSpan.FromSeconds(seconds),
                TimeoutStrategy.Optimistic,
                onTimeoutAsync: (context, timespan, task, exception) => {
                    Console.WriteLine($"Too much time in operation:  {timespan.TotalSeconds} seconds");
                    return Task.CompletedTask;
                }
            );
        }
    }
}
