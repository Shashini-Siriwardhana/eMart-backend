using Microsoft.EntityFrameworkCore;
using ProductsService.Data;
using ProductsService.Services;
using ProductsService.Repositories;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("ProductsDB");

builder.Services.AddDbContext<ProductDbContext>(options =>
    options.UseNpgsql(connectionString));

// Add services to the container.
// Add controller support
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Find and map controller endpoints
app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
