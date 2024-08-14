using System.Text.Json;
using RedisExample.Api.Models;
using RedisExampleCache;

namespace RedisExample.Api.Repository
{
    public class ProductRepositoryWithCache : IProductRepository
    {
        private readonly IProductRepository _repository;
        private readonly IRedisService _redisService;
        private const string productKey = "productCaches";

        public ProductRepositoryWithCache(IProductRepository repository, IRedisService redisService)
        {
            _repository = repository;
            _redisService = redisService;
        }

        public async Task<Product> AddProductAsync(Product product)
        {
            await _repository.AddProductAsync(product);

            if (!await _redisService.CheckKeyExistsAsync(productKey))
                await LoadToCacheFromDbAsync();
            else
                await _redisService.AddItemToHashSetAsync(productKey, product.Id, JsonSerializer.Serialize(product));
            return product;
        }

        public async Task<List<Product>> GetProducts()
        {
            if (!await _redisService.CheckKeyExistsAsync(productKey))
                return await LoadToCacheFromDbAsync();

            var response = new List<Product>();
            var cacheProducts = await _redisService.GetHashSetAsync(productKey);
            foreach (var item in cacheProducts)
            {
                response.Add(JsonSerializer.Deserialize<Product>(item.Value));
            }
            return response;
        }

        public async Task<Product?> GetProduct(int id)
        {
            if (await _redisService.CheckKeyExistsAsync(productKey))
            {
                var cacheProduct = await _redisService.GetHashSetItemAsync(productKey, id);
                return JsonSerializer.Deserialize<Product>(cacheProduct);
            }

            var products = await LoadToCacheFromDbAsync();
            return products.FirstOrDefault(x => x.Id == id);

        }

        private async Task<List<Product>> LoadToCacheFromDbAsync()
        {
            var products = await _repository.GetProducts();
            foreach (var product in products)
            {
                await _redisService.AddItemToHashSetAsync(productKey, product.Id, JsonSerializer.Serialize(product));
            }
            return products;
        }
    }
}
