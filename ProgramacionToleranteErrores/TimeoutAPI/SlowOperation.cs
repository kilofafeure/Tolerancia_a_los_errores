namespace TimeoutAPI
{
    public class SlowOperation
    {
        public async Task Execute(CancellationToken token) {
            Console.WriteLine("Start slow op.");

            await Task.Delay(TimeSpan.FromSeconds(5), token);

            Console.WriteLine("End slow op.");
        }
    }
}
