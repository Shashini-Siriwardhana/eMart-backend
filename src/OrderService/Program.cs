using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using OrderService.Clients;
using OrderService.Data;
using OrderService.Repositories;
using OrderService.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("OrdersDB");
builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddHttpClient<IProductApiClient, ProductApiClient>(
    client =>
    {
        client.BaseAddress = new Uri(
            builder.Configuration["Services:ProductService"]!
        );
    });

builder.Services.AddHttpClient<ICartApiClient, CartApiClient>(
    client =>
    {
        client.BaseAddress = new Uri(
            builder.Configuration["Services:CartsService"]!
        );
    });

builder.Services
.AddControllers()
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
// Add services to the container.
builder.Services.AddOpenApi();

builder.Services.AddScoped<IOrdersService, OrdersService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
