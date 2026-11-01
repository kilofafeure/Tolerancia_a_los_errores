
Console.WriteLine("Escribe tu edad: ");
string? sAge = Console.ReadLine();

// Validación sintáctica
if (!int.TryParse(sAge, out int age))
{
    Console.WriteLine($"Entrada inválida: {sAge}. Por favor, ingresa un número.");
    return;
}

// Validación semántica
if (age < 0)
{
    Console.WriteLine("La edad no puede ser negativa.");
    return;
}
else if (age > 120)
{
    Console.WriteLine("La edad ingresada es poco realista.");
    return;
}

Console.WriteLine("La edad es correcta.");