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
app.MapGet("/get_products", GetProducts);//получение всего каталога
app.MapGet("/get_product_by_id", GetProductById);//получение товара по его id
app.MapPost("/add_product", AddProduct);//добавление товара в каталог
app.MapPost("/remove_product", RemoveProduct);//удаление товара
app.MapPost("/update_product", UpdateProduct);//редактирование товара
app.MapPost("/clear_сatalog", ClearCatalog);//очистка каталога

//REST
app.MapGet("/products", GetProducts);//получение всего каталога
app.MapGet("/products/{productId}", GetProductById);//получение товара по его id
app.MapPost("/products", AddProduct);//добавление товара в каталог
app.MapDelete("/products/{productId}", RemoveProduct);//удаление товара
app.MapPut("/products", UpdateProduct);//редактирование товара
app.MapDelete("/products", ClearCatalog);//очистка каталога

app.MapGet("/get_utc_current_time", GetUTCCurrentTime);

app.MapGet("/send_email", SendEmail);

void SendEmail()
{
    using var emailMessage = new MimeMessage();

    emailMessage.From.Add(MailboxAddress.Parse("asp2023pv112@rodion-m.ru"));
    emailMessage.To.Add(MailboxAddress.Parse("onischenko.anna11@gmail.com"));
    emailMessage.Subject = "ѕроверка MimeKit";
    emailMessage.Body = new TextPart()
    {
        Text = "ѕривет!"
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
        return "“акого товара нет в каталоге!";
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
   

