#nullable enable
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductCatalog.Application.LinqExtensions;
using ProductCatalog.Domain.Models;
using ProductCatalog.Domain.Interface;

namespace ProductCatalog.Application.Services;

public class ProductSearchEngine : IProductSearchEngine
{
    private readonly IRepository<Product> _productRepository;
    private readonly ConcurrentDictionary<string, IEnumerable<Product>> _cache = new();

    public ProductSearchEngine(IRepository<Product> productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<Product>> SearchAsync(string? name, Guid? categoryId)
    {
        var cacheKey = $"{name ?? ""}:{categoryId?.ToString() ?? ""}";
        if (_cache.TryGetValue(cacheKey, out var cached)) return cached;

        var all = (await _productRepository.ListAsync()).AsQueryable();
        var filtered = all.FilterByName(name).FilterByCategory(categoryId).OrderBy(p => p).ToList();

        _cache[cacheKey] = filtered;
        return filtered;
    }

    public void InvalidateCache() => _cache.Clear();
}
