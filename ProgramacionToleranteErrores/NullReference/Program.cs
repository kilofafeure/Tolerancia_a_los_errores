Person person = new("Miguel", new Address("Barcelona"));
//Person person = null;

//Console.WriteLine($"Hola {person.Name}");

// null - coalescing operator (??) para proporcionar un valor predeterminado si person o person.Name es null
int length = person?.Name?.Length ?? 0;

Console.WriteLine($"The name has {length} characters");

Console.WriteLine($"Hello {person?.Name ?? "unknown"} from the city {person?.Address?.City ?? "unknown"}");

record Address(string City);
record Person(string Name, Address Address);
