var retryOperation = new Retry<int>(() =>
{
    Random rand = new();
    int value = rand.Next(0, 10);
    if (value < 7)
    {
        throw new Exception($"Failled operation, incorrect number {value}");
    }

    return value;
}, 10);

try
{
    int result = retryOperation.Execute();
    Console.WriteLine($"Success operation with result: {result}");
}
catch (Exception ex) {
    Console.WriteLine($"Failled operation after some attempts: {ex.Message}");
}

class Retry<T>
{
    private readonly Func<T> _operation;
    private readonly int _maxRetries;

    public Retry(Func<T> operation, int maxRetries = 3)
    {
        _operation = operation;
        _maxRetries = maxRetries;
    }

    public T Execute()
    {
        for (int attempt = 1; attempt <= _maxRetries; attempt++)
        {
            try
            {
                return _operation();
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Attempt {attempt} has failled: {ex.Message}");
            }         
        }

        throw new Exception("All attemps has failed");
    }
}