namespace LoggingFullExample
{
    public static class ProductRepository
    {
        public static readonly List<Product> Products =
        [
            new Product(1, "Product 1", 10.0m),
            new Product(2, "Product 2", 20.0m),
            new Product(3, "Product 3", 30.0m)
        ];
    }
}
