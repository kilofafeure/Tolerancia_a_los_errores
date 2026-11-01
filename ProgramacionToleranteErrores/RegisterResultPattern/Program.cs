
using System.Text.RegularExpressions;

var service = new UserService();

Console.WriteLine("Ingrese su email: "); 
string? email = Console.ReadLine();

Console.WriteLine("Ingrese su password: ");
string? password = Console.ReadLine();

Console.WriteLine("Ingrese su confirmación de password: ");
string? confirmPassword = Console.ReadLine();

var registrationResult = service.RegisterUser(email ?? string.Empty, password ?? string.Empty, confirmPassword ?? string.Empty);

if (registrationResult != null && registrationResult.IsSuccess)
{
    Console.WriteLine($"Usuario registrado exitosamente. ID: {registrationResult.Value?.Id}");
}
else
{
    Console.WriteLine($"Error al registrar usuario: {registrationResult?.ErrorMessage}");
}

public partial class UserService
{
    private readonly List<User> _users = [];

    public Result<User> RegisterUser(string email, string password, string confirmPassword)
    {
        if (string.IsNullOrWhiteSpace(email) || !EmailRegex().IsMatch(email))
        {
            return Result<User>.Failure("Email incorrecto.");
        }

        if (_users.Any(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase)))
        {
            return Result<User>.Failure("El email ya está registrado.");
        }

        if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
        {
            return Result<User>.Failure("Password debe ser de al menos de 6 caracteres.");
        }

        if (password != confirmPassword)
        {
            return Result<User>.Failure("Las contraseñas no coinciden.");
        }

        var user = new User(Guid.NewGuid(), password, email);
        _users.Add(user);

        return Result<User>.Success(user);
    }

    [GeneratedRegex(
        @"^[a-zA-Z0-9!#$%&'*+/=?^_`{|}~.-]+(?:\.[a-zA-Z0-9!#$%&'*+/=?^_`{|}~.-]+)*" +
        @"@(?:[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?\.)+" +
        @"[a-zA-Z]{2,}$")]
    private static partial Regex EmailRegex();
}

public record User(Guid Id, string Password, string Email);

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