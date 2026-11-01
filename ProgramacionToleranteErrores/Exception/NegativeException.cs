namespace ExceptionClasses
{
    /// <summary>
    /// Summary description for NegativeException
    /// </summary>
    public class NegativeException : Exception
    {
        public NegativeException() : base("No se permiten números negativos")
        {
        }

        public NegativeException(string message) : base(message)
        {
        }

        public NegativeException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
