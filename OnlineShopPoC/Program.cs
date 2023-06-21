using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using OnlineShopPoC;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ICatalog, InMemoryCatalog>();
builder.Services.AddSingleton<ITimeProvider, UtcTimeProvider>();
builder.Services.AddScoped<IEmailSender, MailKitSmtpEmailSender>();
//builder.Services.AddHostedService<AppStartedNotificatorBackgroundService>();
builder.Services.AddHostedService<SalesNotificatorBackgroundService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();


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

app.MapGet("/get_utc_current_time", GetUTCCurrentTime);

app.MapGet("/send_email", SendEmail);

void SendEmail()
{
    using var emailMessage = new MimeMessage();

    emailMessage.From.Add(MailboxAddress.Parse("asp2023pv112@rodion-m.ru"));
    emailMessage.To.Add(MailboxAddress.Parse("onischenko.anna11@gmail.com"));
    emailMessage.Subject = "�������� MimeKit";
    emailMessage.Body = new TextPart()
    {
        Text = "������!"
    };

    var password = Environment.GetEnvironmentVariable("smtp_password");

    using (var client = new SmtpClient())
    {
        client.Connect("smtp.beget.com", 25, false);
        client.Authenticate("asp2023pv112@rodion-m.ru", password);
        client.Send(emailMessage);
        client.Disconnect(true);
    }
}

string GetUTCCurrentTime(ITimeProvider timeProvider)
{
    return timeProvider.GetCurrentTime().ToString();
}

List<Product> GetProducts(ICatalog catalog)
{
    return catalog.GetProducts();
}

string GetProductById(Guid productId, ICatalog catalog)
{
    var product = catalog.GetProductById(productId);
    if (product == null)
    {
        return "������ ������ ��� � ��������!";
    }

    return product.ToString();
}

//void AddProduct(Product product, HttpContext context)
//{
//    catalog.AddProduct(product);
//    context.Response.StatusCode = StatusCodes.Status201Created;
//}
IResult AddProduct(Product product, ICatalog catalog)
{
    catalog.AddProduct(product);
    return Results.Created("/add_product", product);
}

void RemoveProduct(Guid productId, ICatalog catalog)
{
    catalog.RemoveProduct(productId);
}

void UpdateProduct(Product updatedProduct, ICatalog catalog)
{
    catalog.UpdateProduct(updatedProduct);
}

void ClearCatalog(ICatalog catalog)
{
    catalog.ClearCatalog();
}


app.Run();
   

