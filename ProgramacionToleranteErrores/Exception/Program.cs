using ExceptionClasses;

////// dobles no produce error en división por cero, pero enteros sí produce error en división por cero.

try
{
    Console.WriteLine("Write a number: ");
    double a = Convert.ToDouble(Console.ReadLine());

    Console.WriteLine("Write another number: ");
    double b = Convert.ToDouble(Console.ReadLine());

    double result = a / b;

    Console.WriteLine($"The result of the division is: {result}");
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred: {ex.Message}");
}
finally
{
    Console.WriteLine("Execution completed.");
}

try
{
    Console.WriteLine("Write a number: ");
    int c = Convert.ToInt32(Console.ReadLine());

    Console.WriteLine("Write another number: ");
    int d = Convert.ToInt32(Console.ReadLine());

    int result2 = c / d;

    Console.WriteLine($"The result of the division is: {result2}");
}
catch (FormatException)
{
    Console.WriteLine($"You must enter numbers");
}
catch (DivideByZeroException)
{
    Console.WriteLine($"Cannot divide by zero");
}
catch (OverflowException)
{
    Console.WriteLine($"The number is too large to be represented");
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred: {ex.GetType()} - {ex.Message}");
}
finally
{
    Console.WriteLine("Execution completed.");
}

////// control finalización del programa con un bucle do while y una variable booleana para controlar la finalización del programa.

bool isOver = false;

do
{
    try
    {
        Console.WriteLine("Write a number: ");
        int e = Convert.ToInt32(Console.ReadLine());

        Console.WriteLine("Write another number: ");
        int f = Convert.ToInt32(Console.ReadLine());

        int result3 = e / f;

        Console.WriteLine($"The result of the division is: {result3}");

        isOver = true;
    }
    catch (FormatException)
    {
        Console.WriteLine($"You must enter numbers");
    }
    catch (DivideByZeroException)
    {
        Console.WriteLine($"Cannot divide by zero");
    }
    catch (OverflowException)
    {
        Console.WriteLine($"The number is too large to be represented");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred: {ex.GetType()} - {ex.Message}");
    }
    finally
    {
        Console.WriteLine("..................");
    }
}
while (!isOver);

Console.WriteLine("Program finished.");

////// control finalización del programa con un bucle do while y una variable booleana para controlar la finalización del programa,
////// pero con una excepción personalizada.

bool isOver2 = false;

do
{
    try
    {
        Console.WriteLine("Write a number: ");
        int g = Convert.ToInt32(Console.ReadLine());

        Console.WriteLine("Write another number: ");
        int h = Convert.ToInt32(Console.ReadLine());

        if (h < 0)
        {
            throw new Exception("The number cannot be negative");
        }

        int result4 = g / h;

        Console.WriteLine($"The result of the division is: {result4}");

        isOver = true;
    }
    catch (FormatException)
    {
        Console.WriteLine($"You must enter numbers");
    }
    catch (DivideByZeroException)
    {
        Console.WriteLine($"Cannot divide by zero");
    }
    catch (OverflowException)
    {
        Console.WriteLine($"The number is too large to be represented");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred: {ex.GetType()} - {ex.Message}");
    }
    finally
    {
        Console.WriteLine("..................");
    }
}
while (!isOver2);

Console.WriteLine("Program finished.");

////// custom exception class

try
{
    Console.WriteLine("Write a number: ");
    int i = Convert.ToInt32(Console.ReadLine());

    Console.WriteLine("Write another number: ");
    int j = Convert.ToInt32(Console.ReadLine());

    if (j < 0)
    {
        throw new NegativeException("The number cannot be negative");
    }

    double result5 = i / j;

    Console.WriteLine($"The result of the division is: {result5}");
}
catch (NegativeException ex)
{
    Console.WriteLine(ex.Message);
}
catch (Exception ex)
{
    Console.WriteLine($"An error occurred: {ex.Message}");
}
finally
{
    Console.WriteLine("Execution completed.");
}