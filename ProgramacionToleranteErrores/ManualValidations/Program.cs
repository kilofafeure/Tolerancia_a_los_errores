
Console.WriteLine("Write your age: ");
string? sAge = Console.ReadLine();

// Validación sintáctica
if (!int.TryParse(sAge, out int age))
{
    Console.WriteLine($"Invalid input: {sAge}. Please enter a valid number.");
    return;
}

// Validación semántica
if (age < 0)
{
    Console.WriteLine("Age cannot be negative.");
    return;
}
else if (age > 120)
{
    Console.WriteLine("Age is not realistic.");
    return;
}

Console.WriteLine("Incorrect age.");