
using System.Text.RegularExpressions;

Result RegisterValidate(string name, int age, string email) { 
    var nameResult = Validator.ValidateRequiredString(name);
    var ageResult = Validator.ValidateAge(age);
    var emailResult = Validator.ValidateEmail(email);
    var emailResult2 = Validator.ValidateEmailFormat(email);

    return Result.Combine(nameResult, ageResult, emailResult, emailResult2);  
}

var result = RegisterValidate("John", 25, "john@example.com");
if (result.IsSuccess)
{
    Console.WriteLine("Validation 1 succeeded.");
}
else
{
    Console.WriteLine($"Validation 1 failed: {result.ErrorMessage}");
}

var result2 = RegisterValidate("J", 25, "@");
if (result2.IsSuccess)
{
    Console.WriteLine("Validation 2 succeeded.");
}
else
{
    Console.WriteLine($"Validation 2 failed: {result.ErrorMessage}");
}

public static class  Validator
{
    public static Result ValidateRequiredString(string name) { 

        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure("String is required.");
        }

        if (name.Length < 2)
        {
            return Result.Failure("String is too short.");
        }

        return Result.Success();
    }

    public static Result ValidateAge(int age)
    {
        if (age < 0 || age > 120)
        {
            return Result.Failure("Age must be between 0 and 120.");
        }
        return Result.Success();
    }

    public static Result ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return Result.Failure("Email is required.");
        }

        if (!email.Contains('@'))
        {
            return Result.Failure("Email is invalid.");
        }

        return Result.Success();
    }

    public static Result ValidateEmailFormat(string email)
    {
        var emailRegex = new Regex(
            @"^[a-zA-Z0-9!#$%&'*+/=?^_`{|}~.-]+(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~.-]+)*" +
            @"@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?\.)+" +
            @"[a-zA-Z]{2,}$");

        if (!emailRegex.IsMatch(email))
        {
            return Result.Failure("Email format is invalid.");
        }
        return Result.Success();
    }
}

public class Result
{
    public bool IsSuccess { get; }
    public string? ErrorMessage { get; }

    private Result()
    {
        IsSuccess = true;
    }

    private Result(string errorMessage)
    {
        IsSuccess = false;
        ErrorMessage = errorMessage;
    }

    public static Result Success() => new();
    public static Result Failure(string errorMessage) => new(errorMessage);
    public static Result Combine(params Result[] results)
    {
        foreach (var r in results)
        {
            if (!r.IsSuccess)
            {
                return Failure(r.ErrorMessage ?? "Unknown error");
            }
        }

        return Success();
    }
}
