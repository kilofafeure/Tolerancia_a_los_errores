using Polly;
using System.Text.Json;


//var url = "https://jsonplaceholder.typicode.com/posts";
var url = "https://jsonplaceholder56.typicode.com/posts"; // to test fail
var fileName = "posts.json";

var fallbackPolicy = Policy<List<Post>>
    .Handle<HttpRequestException>()
    .FallbackAsync(
        fallbackValue: await GetInfoByFile(fileName),
        onFallbackAsync: (exception, context) =>
        {
            Console.WriteLine("Fallback: service not responding");
            Console.WriteLine($"Motive: {exception.Exception.Message}");
            Console.WriteLine("Will be used default info");
            return Task.CompletedTask;
        }
    );

var posts = await fallbackPolicy.ExecuteAsync(async () =>
{
    var httpClient = new HttpClient();
    Console.WriteLine("Request for service");

    var response = await httpClient.GetAsync(url);
    response.EnsureSuccessStatusCode();
    var content = await response.Content.ReadAsStringAsync();
    var posts = JsonSerializer.Deserialize<List<Post>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    Console.WriteLine("Request ok");

    // saving info to C:\Desarrollo\ToleranciaErrores\ProgramacionToleranteErrores\FallbackData\bin\Debug\net10.0

    //var fileContent = JsonSerializer.Serialize(posts, new JsonSerializerOptions { WriteIndented = true });
    //await File.WriteAllTextAsync(fileName, fileContent);
    await File.WriteAllTextAsync(fileName, content);

    return posts;
});

posts.ForEach(post =>
{
    Console.WriteLine($"{post.Id}: {post.UserId}: {post.Title}");
});

static async Task<List<Post>> GetInfoByFile(string fileName) {
    if (!File.Exists(fileName))
        return [];

    var fileContent = await File.ReadAllTextAsync(fileName);
    var posts = JsonSerializer.Deserialize<List<Post>>(fileContent,
        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    return posts ?? [];
}

record Post(int UserId, int Id, string Title, string Body);