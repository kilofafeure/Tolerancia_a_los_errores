namespace ProductRepository_Exception
{
    public class ProductRepository
    {
        private readonly List<Product> _products = [
            new Product(1, "Laptop"),
            new Product(2, "Smartphone"),
            new Product(3, "Tablet")
        ];

        public Product GetById(int id)
        {
            if (id <= 0)
            {
                throw new KeyNotFoundException("The product ID must be a positive integer.");
            }

            var product = _products.FirstOrDefault(p => p.Id == id);

            return product ?? throw new KeyNotFoundException($"Product with ID {id} not found.");
        }
    }

    public record Product(int Id, string Name);
}
