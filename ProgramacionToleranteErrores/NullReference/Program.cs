Person person = new("Miguel", new Address("Barcelona"));
//Person person = null;

//Console.WriteLine($"Hola {person.Name}");

// null - coalescing operator (??) para proporcionar un valor predeterminado si person o person.Name es null
int length = person?.Name?.Length ?? 0;

Console.WriteLine($"El nombre tiene {length} caracteres");

Console.WriteLine($"Hola {person?.Name ?? "desconocido"} que es de la ciudad {person?.Address?.City ?? "desconocida"}");

record Address(string City);
record Person(string Name, Address Address);
