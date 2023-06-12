using Microsoft.AspNetCore.Mvc;
using OnlineShopPoC;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

Catalog catalog = new Catalog();

app.MapGet("/get_products", GetProducts);//получение всего каталога
app.MapPost("/add_product", AddProduct);//добавление товара в каталог
app.MapGet("/get_product_by_id/{id}", GetProductById);//получение товара по его id
app.MapDelete("/remove_product/{id}", RemoveProduct);//удаление товара
app.MapPut("/update_product", UpdateProduct);//редактирование товара
app.MapDelete("/сlear_сatalog", ClearCatalog);//очистка каталога


List<Product> GetProducts()
{
    return catalog.GetProducts();
}

void AddProduct(Product product)
{
    catalog.AddProduct(product);
}

//IActionResult AddProduct(Product product)
//{
//    catalog.AddProduct(product);
//    return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
//}

void RemoveProduct(Guid id)
{
    catalog.RemoveProduct(id);
}

string GetProductById(Guid id)
{
    var product = catalog.GetProductById(id);
    if (product == null)
    {
        return "Такого товара нет в каталоге!";
    }

    return product.ToString();
}


void UpdateProduct(Product updatedProduct)
{
    catalog.UpdateProduct(updatedProduct);
}

void ClearCatalog()
{
    catalog.ClearCatalog();
}




app.Run();
   

