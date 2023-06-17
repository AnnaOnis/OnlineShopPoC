using System.Collections.Concurrent;

namespace OnlineShopPoC
{
    public class InMemoryCatalog : ICatalog
    {
        private ConcurrentDictionary<Guid, Product> _products = new ConcurrentDictionary<Guid, Product>(GenerateProducts(10).ToDictionary(p => p.Id));
        private readonly object _lock = new object();

        public List<Product> GetProducts()
        {
            lock (_lock)
            {
                foreach (var product in _products.Values)
                {
                    CalculateDiscount(product);
                }
            }
            return _products.Values.ToList();
        }

        public Product GetProductById(Guid id)
        {
            if(_products.TryGetValue(id, out Product product))
            {
                lock (_lock)
                {
                    CalculateDiscount(product);
                }
            }
            return product;
        }

        public void AddProduct(Product product)
        {
            lock (_lock)
            {
                CalculateDiscount(product);
            }
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
                lock (_lock)
                {
                    CalculateDiscount(updatedProduct);
                }
                _products.TryUpdate(updatedProduct.Id, updatedProduct, existingProduct);
            }
            
        }

        public void ClearCatalog()
        {
            _products.Clear();
        }

        public Product CalculateDiscount(Product product)
        {
            product.DiscountPrice = product.Price;
            product.DescriptionDiscount = "Сегодня скидок нет!";

            DateTime currentDate = DateTime.Now;
            if (currentDate.DayOfWeek == DayOfWeek.Monday)
            {
                product.DiscountPrice -= product.Price / 100 * 30;
                product.DescriptionDiscount = "Ура! Сегодня скидка 30%!";

            }
            return product;
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
                    DiscountPrice = price,
                    Description = "Описание " + name,
                    DescriptionDiscount = "Сегодня скидок нет!",
                    ProducedAt = producedAt,
                    ExpiredAt = expiredAt,
                    Stock = stock
                };

            }
            return products.ToList();
        }
    }

}
