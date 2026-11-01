using System.Text.RegularExpressions;

var patternName = @"^(?=.{2,50}$)[A-ZÁÉÍÓÚÑ][a-záéíóúñ]+(\s+[A-ZÁÉÍÓÚÑ][a-záéíóúñ]+)*$"; // Expresión regular para validar nombres con mayúscula inicial y longitud entre 2 y 50 caracteres
var patternEmail = @"^[\w -\.]+@([\w -]+\.)+[\w -]{2,4}$"; // Expresión regular para validar emails

var regexName = new Regex(patternName);
var name = "Francisco";

if (regexName.IsMatch(name))
    Console.WriteLine($"{name} es un nombre válido");
else
    Console.WriteLine($"{name} no es un nombre válido");

var regexEmail = new Regex(patternEmail);
var email = "abcdefg@gmail.com";

if (regexEmail.IsMatch(email))
    Console.WriteLine($"{email} es un email válido");
else
    Console.WriteLine($"{email} no es un email válido");
