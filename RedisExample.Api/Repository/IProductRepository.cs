using RedisExample.Api.Models;

namespace RedisExample.Api.Repository
{
    public interface IProductRepository
    {
        Task<Product> AddProductAsync(Product product);
        Task<List<Product>> GetProducts();
        Task<Product?> GetProduct(int id);
    }
}
