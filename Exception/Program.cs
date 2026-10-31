
////// dobles no produce error en división por cero, pero enteros sí produce error en división por cero.

try
{
    Console.WriteLine("Escribe un número: ");
    double a = Convert.ToDouble(Console.ReadLine());

    Console.WriteLine("Escribe otro número: ");
    double b = Convert.ToDouble(Console.ReadLine());

    double result = a / b;

    Console.WriteLine($"El resultado de la división es: {result}");
}
catch (Exception ex)
{
    Console.WriteLine($"Se produjo un error: {ex.Message}");
}
finally
{
    Console.WriteLine("Ejecución completada.");
}

try
{
    Console.WriteLine("Escribe un número: ");
    int c = Convert.ToInt32(Console.ReadLine());

    Console.WriteLine("Escribe otro número: ");
    int d = Convert.ToInt32(Console.ReadLine());

    int result2 = c / d;

    Console.WriteLine($"El resultado de la división es: {result2}");
}
catch (FormatException)
{
    Console.WriteLine($"Debes escribir números");
}
catch (DivideByZeroException)
{
    Console.WriteLine($"No se puede dividir por cero");
}
catch (OverflowException)
{
    Console.WriteLine($"El número es demasiado grande para ser representado");
}
catch (Exception ex)
{
    Console.WriteLine($"Se produjo un error: {ex.GetType()} - {ex.Message}");
}
finally
{
    Console.WriteLine("Ejecución completada.");
}

////// control finalización del programa con un bucle do while y una variable booleana para controlar la finalización del programa.

bool isOver = false;

do
{
    try
    {
        Console.WriteLine("Escribe un número: ");
        int e = Convert.ToInt32(Console.ReadLine());

        Console.WriteLine("Escribe otro número: ");
        int f = Convert.ToInt32(Console.ReadLine());

        int result2 = e / f;

        Console.WriteLine($"El resultado de la división es: {result2}");

        isOver = true;
    }
    catch (FormatException)
    {
        Console.WriteLine($"Debes escribir números");
    }
    catch (DivideByZeroException)
    {
        Console.WriteLine($"No se puede dividir por cero");
    }
    catch (OverflowException)
    {
        Console.WriteLine($"El número es demasiado grande para ser representado");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Se produjo un error: {ex.GetType()} - {ex.Message}");
    }
    finally
    {
        Console.WriteLine("..................");
    }
}
while (!isOver);

Console.WriteLine("Programa finalizado.");

////// control finalización del programa con un bucle do while y una variable booleana para controlar la finalización del programa,
////// pero con una excepción personalizada.

bool isOver2 = false;

do
{
    try
    {
        Console.WriteLine("Escribe un número: ");
        int g = Convert.ToInt32(Console.ReadLine());

        Console.WriteLine("Escribe otro número: ");
        int h = Convert.ToInt32(Console.ReadLine());

        if (h < 0)
        { 
            throw new Exception("El número no puede ser negativo");
        }

        int result2 = g / h;

        Console.WriteLine($"El resultado de la división es: {result2}");

        isOver = true;
    }
    catch (FormatException)
    {
        Console.WriteLine($"Debes escribir números");
    }
    catch (DivideByZeroException)
    {
        Console.WriteLine($"No se puede dividir por cero");
    }
    catch (OverflowException)
    {
        Console.WriteLine($"El número es demasiado grande para ser representado");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Se produjo un error: {ex.GetType()} - {ex.Message}");
    }
    finally
    {
        Console.WriteLine("..................");
    }
}
while (!isOver2);

Console.WriteLine("Programa finalizado.");