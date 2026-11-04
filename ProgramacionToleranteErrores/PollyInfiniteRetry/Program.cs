using Polly;
using System.Net.NetworkInformation;

string host = "metalcode2.io";

var pingPolicy = Policy.Handle<PingException>()
                    .OrResult<PingReply>(r => r.Status != IPStatus.Success)
                    .WaitAndRetryForeverAsync(
                        sleepDurationProvider: attempt => TimeSpan.FromSeconds(2),
                        onRetry: (result, attempt, time) =>
                        {
                            string error = result.Exception != null ?
                                            result.Exception.Message :
                                            result.Result.Status.ToString();
                            Console.WriteLine($"Attempt {attempt}. Retried: {time.TotalSeconds}  Error: {error}");
                        }
                    );

var context = new Context("Ping")
{
    ["Host"] = host
};

await pingPolicy.ExecuteAsync(async (context) => {

    using var ping = new Ping();
    Console.WriteLine($"Sending ping to [{context["Host"]}]");
    var reply = await ping.SendPingAsync((string)context["Host"], 2000);

    if (reply.Status == IPStatus.Success)
    {
        Console.WriteLine($"Received answer from {reply.Address} in {reply.RoundtripTime}ms");
    }
    else {
        Console.WriteLine($"There is no answer {reply.Status}");
    }

    return reply;
}, context);