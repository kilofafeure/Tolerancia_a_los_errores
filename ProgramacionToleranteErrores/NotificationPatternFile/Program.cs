var importResult = BeerImporter.ImportBeers("data.csv");

if (importResult.Notification.HasErrors)
{
    Console.WriteLine("Errors occurred during beer import:");
    foreach (var error in importResult.Notification.Errors)
    {
        Console.WriteLine(error);
    }
}

Console.WriteLine("-----------------------------------");
Console.WriteLine("Cervezas importadas:");

foreach (var beer in importResult.Beers)
{
    Console.WriteLine($"- {beer.Name}: ${beer.Price}");
}

public record Beer(string Name, decimal Price);
public record ImportResult(List<Beer> Beers, Notification Notification);

public static class BeerImporter
{
    public static ImportResult ImportBeers(string url)
    {
        var notification = new Notification();
        var beers = new List<Beer>();
        var fileData = File.ReadAllLines(url);

        try
        {
            for (int i = 1; i < fileData.Length; i++)
            {
                var rowErrors = new List<string>();
                var row = fileData[i].Split(',');

                if (row.Length != 2)
                {
                    notification.Add($"Invalid data format on line {i + 1}: {fileData[i]}");
                    continue;
                }

                var name = row[0].Trim();
                var price = row[1].Trim();
            
                if (string.IsNullOrWhiteSpace(name))
                {
                    rowErrors.Add($"Invalid beer name on line {i + 1}: {fileData[i]}");
                }

                if (!decimal.TryParse(price, out decimal parsedPrice) || parsedPrice <= 0)
                {
                    rowErrors.Add($"Invalid price format on line {i + 1}: {fileData[i]}");
                }

                if (rowErrors.Any())
                {
                    notification.Add($"Errors on line {i + 1}: {string.Join(", ", rowErrors)}");
                    continue;
                }

                beers.Add(new Beer(name, parsedPrice));
            }

            return new ImportResult(beers, notification);
        }
        catch (Exception ex)
        {
            notification.Add($"An error occurred while importing beers: {ex.Message}");
        }
        return new ImportResult(beers, notification);
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