namespace OnlineShopPoC
{
    public class Catalog
    {
        private List<Product> _products = GenerateProducts(10);

        public List<Product> GetProducts() 
        { 
            return _products;
        }

        public Product GetProductById(Guid id) 
        {
            return _products.FirstOrDefault(p => p.Id == id);
        }

        public void AddProduct(Product product)
        { 
            _products.Add(product); 
        }

        public void RemoveProduct(Guid id) 
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                _products.Remove(product);
            }
        }

        public void UpdateProduct(Product updatedProduct)
        {
            var existingProduct = _products.FirstOrDefault(p => p.Id == updatedProduct.Id);
            if (existingProduct != null)
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
