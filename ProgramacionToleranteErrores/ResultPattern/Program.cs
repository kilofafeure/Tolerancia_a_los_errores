var readNumber = new ReadNumber();
var numberResult = readNumber.Read();

if (numberResult.IsSuccess)
{
    Console.WriteLine($"Número ingresado: {numberResult.Value}");
}
else
{
    Console.WriteLine($"Error: {numberResult.ErrorMessage}");
}

public class ReadNumber()
{
    public Result<int> Read()
    {
        Console.Write("Escribe un número: ");
        string? input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input) || !int.TryParse(input, out int number))
        {
            return Result<int>.Failure($"El valor {input} no es un número entero válido.");
        }

        return Result<int>.Success(number);
    }
}

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
