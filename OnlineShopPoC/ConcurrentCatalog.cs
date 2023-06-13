using System.Collections.Concurrent;

namespace OnlineShopPoC
{
    public class ConcurrentCatalog
    {
        private ConcurrentDictionary<Guid, Product> _products = new ConcurrentDictionary<Guid, Product>(GenerateProducts(10).ToDictionary(p => p.Id));

        public List<Product> GetProducts()
        {
            return _products.Values.ToList();
        }

        public Product GetProductById(Guid id)
        {
             _products.TryGetValue(id, out Product product);
            return product;
        }

        public void AddProduct(Product product)
        {
            _products.TryAdd(product.Id, product);
        }

        public void RemoveProduct(Guid id)
        {
            _products.TryRemove(id, out _);          
        }

        public void UpdateProduct(Product updatedProduct)
        {
            if (_products.TryGetValue(updatedProduct.Id, out Product existingProduct))
            {
                // Обновление полей товара
                existingProduct.Name = updatedProduct.Name;
                existingProduct.Description = updatedProduct.Description;
                existingProduct.Price = updatedProduct.Price;
                existingProduct.ProducedAt = updatedProduct.ProducedAt;
                existingProduct.ExpiredAt = updatedProduct.ExpiredAt;
                existingProduct.Stock = updatedProduct.Stock;
            }
        }

        public void ClearCatalog()
        {
            _products.Clear();
        }

        private static List<Product> GenerateProducts(int count)
        {
            var random = new Random();
            var products = new Product[count];

            // Массив реальных названий товаров
            var productNames = new string[]
            {
            "Молоко",
            "Хлеб",
            "Яблоки",
            "Макароны",
            "Сахар",
            "Кофе",
            "Чай",
            "Рис",
            "Масло подсолнечное",
            "Сыр"
            };

            for (int i = 0; i < count; i++)
            {
                var name = productNames[i];
                var price = random.Next(50, 500);
                var producedAt = DateTime.Now.AddDays(-random.Next(1, 30));
                var expiredAt = producedAt.AddDays(random.Next(1, 365));
                var stock = random.NextDouble() * 100;
                products[i] = new Product(name, price)
                {
                    Description = "Описание " + name,
                    ProducedAt = producedAt,
                    ExpiredAt = expiredAt,
                    Stock = stock
                };

            }
            return products.ToList();
        }
    }
}
