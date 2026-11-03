var customer = new Customer("John Doe", "USA", 1000m);
var customer2 = new Customer("", "", -1);

var notification = CustomerValidator.Validate(customer);
var notification2 = CustomerValidator.Validate(customer2);

if (!notification.HasErrors)
{
    Console.WriteLine("Customer 1 is valid.");
}
else
{
    Console.WriteLine("Customer 1 has errors:");
    notification.Errors.ToList().ForEach(error => Console.WriteLine($"- {error}"));
}

if (!notification2.HasErrors)
{
    Console.WriteLine("Customer 2 is valid.");
}
else
{
    Console.WriteLine("Customer 2 has errors:");
    notification2.Errors.ToList().ForEach(error => Console.WriteLine($"- {error}"));
}

public record Customer(string Name, string Country, decimal Balance);

public class CustomerValidator
{
    public static Notification Validate(Customer customer)
    {
        var notification = new Notification();
        if (string.IsNullOrWhiteSpace(customer.Name))
        {
            notification.Add("Name is required.");
        }
        if (string.IsNullOrWhiteSpace(customer.Country))
        {
            notification.Add("Country is required.");
        }
        if (customer.Balance < 0)
        {
            notification.Add("Balance cannot be negative.");
        }
        return notification;
    }
}

public class Notification
{
    private readonly List<string> _errors = [];
    public IReadOnlyCollection<string> Errors => _errors.AsReadOnly();
    public bool HasErrors => _errors.Any();
    public void Add(string errorMessage)
    {
        _errors.Add(errorMessage);
    }
}