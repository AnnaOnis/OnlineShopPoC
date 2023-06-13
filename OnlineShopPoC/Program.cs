using Microsoft.AspNetCore.Mvc;
using OnlineShopPoC;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

ConcurrentCatalog catalog = new ConcurrentCatalog();

//RPC
app.MapGet("/get_products", GetProducts);//��������� ����� ��������
app.MapGet("/get_product_by_id", GetProductById);//��������� ������ �� ��� id
app.MapPost("/add_product", AddProduct);//���������� ������ � �������
app.MapPost("/remove_product", RemoveProduct);//�������� ������
app.MapPost("/update_product", UpdateProduct);//�������������� ������
app.MapPost("/clear_�atalog", ClearCatalog);//������� ��������

//REST
app.MapGet("/products", GetProducts);//��������� ����� ��������
app.MapGet("/products/{productId}", GetProductById);//��������� ������ �� ��� id
app.MapPost("/products", AddProduct);//���������� ������ � �������
app.MapDelete("/products/{productId}", RemoveProduct);//�������� ������
app.MapPut("/products", UpdateProduct);//�������������� ������
app.MapDelete("/products", ClearCatalog);//������� ��������

List<Product> GetProducts()
{
    return catalog.GetProducts();
}

string GetProductById(Guid productId)
{
    var product = catalog.GetProductById(productId);
    if (product == null)
    {
        return "������ ������ ��� � ��������!";
    }

    return product.ToString();
}

void AddProduct(Product product, HttpContext context)
{
    catalog.AddProduct(product);
    context.Response.StatusCode = StatusCodes.Status201Created;
}
//IResult AddProduct(Product product)
//{
//    catalog.AddProduct(product);
//    return Results.Created("/add_product", product);
//}

void RemoveProduct(Guid productId)
{
    catalog.RemoveProduct(productId);
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
   

