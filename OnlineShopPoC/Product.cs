namespace OnlineShopPoC
{
    /// <summary>
    /// Модель данных для Товара в нашем интернет-магазине
    /// </summary>

    public class Product
    {
        public Product(string name, decimal price) 
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException($"\"{nameof(name)}\" не может быть неопределенным или пустым.", nameof(name));
            }
            if(price < 0) throw new ArgumentNullException(nameof(price));

            Id = Guid.NewGuid();
            Name = name;
            Price = price;
        }
        /// <summary> ID товара </summary>
        public Guid Id { get; init; }
        /// <summary> Название товара </summary>
        public string Name { get; set; }

        /// <summary> Описание товара </summary>
        public string Description { get; set; }

        /// <summary> Цена товара </summary>
        public decimal Price { get; set; }

        /// <summary> Информация о скидках </summary>
        public string DescriptionDiscount { get; set; }

        /// <summary> Цена товара со скидкой</summary>
        public decimal DiscountPrice { get; set; }

        /// <summary> Дата изготовления </summary>
        public DateTime ProducedAt { get; set; }

        /// <summary> Срок годности </summary>
        public DateTime ExpiredAt { get; set; }

        /// <summary> Количество товара в наличии </summary>
        public double Stock { get; set; }

        public override string ToString()
        {
            return $"{Id}  {Name}  {Description}  {Price}  {DiscountPrice}";
        }

    }


}
