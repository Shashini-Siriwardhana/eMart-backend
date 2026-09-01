using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using PaymentsService.Clients;
using PaymentsService.Data;
using PaymentsService.Repositories;
using PaymentsService.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("PaymentsDB");

builder.Services.AddDbContext<PaymentDbContext>(options => 
options.UseNpgsql(connectionString));

builder.Services.AddHttpClient<IOrderApiClient, OrderApiClient>(
    client => client.BaseAddress = new Uri(
        builder.Configuration["Services:OrderService"]!
    ));

// Keep enums as int in DB and expose them as strings in API
builder.Services
.AddControllers()
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IPaymentService, PaymentService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
