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
                throw new KeyNotFoundException("El ID del producto debe ser un entero positivo.");
            }

            var product = _products.FirstOrDefault(p => p.Id == id);

            return product ?? throw new KeyNotFoundException($"Producto con ID {id} no encontrado.");
        }
    }

    public record Product(int Id, string Name);
}
