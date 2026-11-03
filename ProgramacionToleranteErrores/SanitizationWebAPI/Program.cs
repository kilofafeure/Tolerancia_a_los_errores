using SanitizationWebAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapPost("/people", (People person) =>
{
    var normalizedPerson = PeopleNormalizer.Normalize(person);

    // Here you can process the normalizedPerson object as needed, e.g., save to a database, go to services, etc.

    return Results.Ok($"Received person: {normalizedPerson.Name}, Age: {normalizedPerson.Age}, Email: {normalizedPerson.Email}, Phone: {normalizedPerson.Phone}, Description: {normalizedPerson.Description}, Content: {normalizedPerson.Content}");
});

app.Run();

public class People
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}   