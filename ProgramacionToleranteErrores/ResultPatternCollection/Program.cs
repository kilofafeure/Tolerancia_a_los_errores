
var results = new List<Result<decimal>>();
int n = 100;

for (int i = 0; i < n; i++)
{
    decimal number = Random.Shared.Next(-100, 100);
    Result<decimal> result;

    if (number > 0) {
        result = Result<decimal>.Success(number);
    }
    else { 
        result = Result<decimal>.Failure("El número no es positivo");
    }

    results.Add(result);
}

results.ForEach(result =>
{
    if (result.IsSuccess)
    {
        Console.WriteLine($"Número positivo: {result.Value}");
    }
    else
    {
        Console.WriteLine($"Error: {result.ErrorMessage}");
    }
});

public class Result<T>
{
    public bool IsSuccess { get; }
    public T? Value { get; }
    public string? ErrorMessage { get; }

    private Result(T value)
    {
        IsSuccess = true;
        Value = value;
    }

    private Result(string errorMessage)
    {
        IsSuccess = false;
        ErrorMessage = errorMessage;
    }

    public static Result<T> Success(T value) => new(value);
    public static Result<T> Failure(string errorMessage) => new(errorMessage);
}