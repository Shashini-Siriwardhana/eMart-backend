using Microsoft.EntityFrameworkCore;
using CartsService.Data;
using CartsService.Services;
using CartsService.Repositories;
using CartsService.Clients;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("CartsDB");
builder.Services.AddDbContext<CartDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddHttpClient<IProductApiClient, ProductApiClient>(
    client =>
    {
        client.BaseAddress = new Uri(
            builder.Configuration["Services:ProductService"]!
        );
    });

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<ICartRepository, CartRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();