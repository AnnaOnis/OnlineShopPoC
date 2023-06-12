using Microsoft.AspNetCore.Mvc;
using OnlineShopPoC;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

Catalog catalog = new Catalog();

app.MapGet("/get_products", GetProducts);//��������� ����� ��������
app.MapPost("/add_product", AddProduct);//���������� ������ � �������
app.MapGet("/get_product_by_id/{id}", GetProductById);//��������� ������ �� ��� id
app.MapDelete("/remove_product/{id}", RemoveProduct);//�������� ������
app.MapPut("/update_product", UpdateProduct);//�������������� ������
app.MapDelete("/�lear_�atalog", ClearCatalog);//������� ��������


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
        return "������ ������ ��� � ��������!";
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
   

