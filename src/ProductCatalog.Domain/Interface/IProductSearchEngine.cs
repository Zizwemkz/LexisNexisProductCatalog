using ProductCatalog.Domain.Models;

namespace ProductCatalog.Domain.Interface
{
    public interface IProductSearchEngine
    {
        Task<IEnumerable<Product>> SearchAsync(string? name, Guid? categoryId);
        void InvalidateCache();
    }
}
