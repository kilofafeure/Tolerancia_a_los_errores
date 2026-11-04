var retryOperation = new Retry<string>(async () =>
{
    var http = new HttpClient();
    var response = await http.GetAsync("https://somesite.io");
    
    if (!response.IsSuccessStatusCode) {
        throw new Exception($"Operation failled with statusCode: {response.StatusCode}");
    }

    return await response.Content.ReadAsStringAsync();

}, 5, 2000);

try
{
    string result = await retryOperation.Execute();
}
catch (Exception ex) {
    Console.WriteLine($"Failed opertaion after some attempts: {ex.Message}");
}


class Retry<T>
{
    private readonly Func<Task<T>> _operation;
    private readonly int _maxRetries;
    private readonly int _delayMilliseconds;

    public Retry(Func<Task<T>> operation, int maxRetries = 3, int dellayMilliseconds = 1000)
    {
        _operation = operation;
        _maxRetries = maxRetries;
        _delayMilliseconds = dellayMilliseconds;
    }

    public async Task<T> Execute()
    {
        for (int attempt = 1; attempt <= _maxRetries; attempt++)
        {
            try
            {
                return await _operation();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Attempt {attempt} has failled: {ex.Message}");
                await Task.Delay(_delayMilliseconds);
            }
        }

        throw new Exception("All attemps has failed");
    }
}