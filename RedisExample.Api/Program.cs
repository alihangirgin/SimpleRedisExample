using Microsoft.EntityFrameworkCore;
using RedisExample.Api;
using RedisExample.Api.Repository;
using RedisExampleCache;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseInMemoryDatabase("InMemoryDb");
});
// Redis servisini kaydet
var redisConnectionString = builder.Configuration.GetSection("Redis").Value;
builder.Services.AddSingleton<IRedisService, RedisService>(opt => new RedisService(redisConnectionString ?? string.Empty));
// Gerçek ürün repository'sini kaydet
builder.Services.AddScoped<ProductRepository>(); // ProductRepository'yi doðrudan kaydet
builder.Services.AddScoped<IProductRepository, ProductRepositoryWithCache>(serviceProvider =>
{
    var productRepository = serviceProvider.GetRequiredService<ProductRepository>();
    var redisService = serviceProvider.GetRequiredService<IRedisService>();
    return new ProductRepositoryWithCache(productRepository, redisService);
});




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
