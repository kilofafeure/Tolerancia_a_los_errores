using Polly;

namespace CircuitBreakerAPI
{
    public static class Policies
    {
        public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return Policy<HttpResponseMessage>
                .HandleResult(r => !r.IsSuccessStatusCode)
                .Or<HttpRequestException>()
                .CircuitBreakerAsync(
                    handledEventsAllowedBeforeBreaking: 3,
                    durationOfBreak: TimeSpan.FromSeconds(10),
                    onBreak: (outcome, breakDelay) =>
                    {
                        Console.WriteLine($"Circuit breaker opened for {breakDelay.TotalSeconds} seconds due to: {outcome.Exception?.Message ?? outcome.Result.StatusCode.ToString()}");
                        // You can add additional logic here if needed when the circuit is opened, for example, logging or notifying other parts of the system
                    },
                    onReset: () =>
                    {
                        Console.WriteLine("Circuit breaker reset.");
                        // You can add additional logic here if needed when the circuit is reset, for example, logging or notifying other parts of the system
                    },
                    onHalfOpen: () =>
                    {
                        Console.WriteLine("Circuit breaker is half-open. Next call is a trial.");
                        // You can add additional logic here if needed when the circuit is half-open, for example, logging or notifying other parts of the system
                    }
                );
        }

        public static IAsyncPolicy<HttpResponseMessage> GetAdvancedCircuitBreakerPolicy()
        {
            return Policy<HttpResponseMessage>
                .HandleResult(r => !r.IsSuccessStatusCode)
                .Or<HttpRequestException>()
                .AdvancedCircuitBreakerAsync(
                    failureThreshold: 0.5, // percentage 50%
                    samplingDuration: TimeSpan.FromSeconds(20), // sampling window
                    minimumThroughput: 4, // minimun calls
                    durationOfBreak: TimeSpan.FromSeconds(10),
                    onBreak: (outcome, breakDelay) =>
                    {
                        Console.WriteLine($"Circuit breaker opened for {breakDelay.TotalSeconds} seconds due to: {outcome.Exception?.Message ?? outcome.Result.StatusCode.ToString()}");
                        // You can add additional logic here if needed when the circuit is opened, for example, logging or notifying other parts of the system
                    },
                    onReset: () =>
                    {
                        Console.WriteLine("Circuit breaker reset.");
                        // You can add additional logic here if needed when the circuit is reset, for example, logging or notifying other parts of the system
                    },
                    onHalfOpen: () =>
                    {
                        Console.WriteLine("Circuit breaker is half-open. Next call is a trial.");
                        // You can add additional logic here if needed when the circuit is half-open, for example, logging or notifying other parts of the system
                    }
                );
        }
    }
}
