var climateGetters = new List<GetClimate>
{
    new("40.7128", "-74.0060"), // New York
    new("34.0522", "-118.2437"), // Los Angeles
    new("51.5074", "-0.1278"), // London
    new("35.6895", "139.6917"), // Tokyo
    new("-99999999999999999999999", "-99999999999999999999999999999"),
};

var getterInfo = new GetterInfo(climateGetters);
var notification = await getterInfo.ExecuteAllAsync();
if (notification.HasErrors)
{
    Console.WriteLine("Errors occurred while fetching weather data:");
    notification.Errors.ToList().ForEach(error => Console.WriteLine($"- {error}"));
}
else
{
    Console.WriteLine("Weather data fetched successfully for all");
}


public class GetterInfo
{
    private readonly List<GetClimate> _getters = [];
    public GetterInfo(List<GetClimate> getters) => _getters = getters;
    public async Task<Notification> ExecuteAllAsync()
    {
        var notification = new Notification();
        foreach (var getter in _getters)
        {
            var result = await getter.GetWeatherAsync();
            if (result.HasErrors)
            {
                foreach (var error in result.Errors)
                {
                    notification.Add(error);
                }
            }
        }
        return notification;
    }
}

public class GetClimate(string lat, string lon)
{
    private readonly HttpClient _httpClient = new();
    private readonly string _latitude = lat;
    private readonly string _longitude = lon;
    private const string APIURL = "https://api.open-meteo.com/v1/forecast?current_weather=true";


    public async Task<Notification> GetWeatherAsync()
    {
        var notification = new Notification();

        try 
        { 
            var response = await _httpClient.GetAsync($"{APIURL}&latitude={_latitude}&longitude={_longitude}");

            if (!response.IsSuccessStatusCode)
            {
                notification.Add($"Failed to fetch weather data ({_latitude}, {_longitude}). Status code: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            notification.Add($"An error occurred while fetching weather data: {ex.Message}");
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
