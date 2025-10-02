using System;
using System.Linq;
using ProductCatalog.Domain.Models;

namespace ProductCatalog.Application.LinqExtensions;

public static class ProductFilterExtensions
{
    public static IQueryable<Product> FilterByName(this IQueryable<Product> source, string? name)
    {
        if (string.IsNullOrWhiteSpace(name)) return source;
        return source.Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
    }

    public static IQueryable<Product> FilterByCategory(this IQueryable<Product> source, Guid? categoryId)
    {
        if (categoryId is null || categoryId == Guid.Empty) return source;
        return source.Where(p => p.CategoryId == categoryId);
    }
}
