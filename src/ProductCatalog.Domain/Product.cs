#nullable enable
using System;

namespace ProductCatalog.Domain.Entities;

public class Product : IComparable<Product>
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public string SKU { get; private set; }
    public decimal Price { get; private set; }
    public int Quantity { get; private set; }
    public Guid CategoryId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    protected Product() { }

    public Product(Guid id, string name, string sku, decimal price, int quantity, Guid categoryId, string? description = null)
    {
        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        Name = string.IsNullOrWhiteSpace(name) ? throw new ArgumentException("Name is required") : name;
        SKU = string.IsNullOrWhiteSpace(sku) ? throw new ArgumentException("SKU is required") : sku;
        Price = price >= 0 ? price : throw new ArgumentException("Price must be non-negative");
        Quantity = quantity >= 0 ? quantity : throw new ArgumentException("Quantity must be non-negative");
        CategoryId = categoryId;
        Description = description;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Update(string? name, string? description, decimal? price, int? quantity, Guid? categoryId)
    {
        if (name is string s && string.IsNullOrWhiteSpace(s)) throw new ArgumentException("Name can't be empty string");
        if (price is decimal p && p < 0) throw new ArgumentException("Price must be non-negative");
        if (quantity is int q && q < 0) throw new ArgumentException("Quantity must be non-negative");

        if (name is string ns) Name = ns;
        if (description is string d) Description = d;
        if (price is decimal pr) Price = pr;
        if (quantity is int qt) Quantity = qt;
        if (categoryId is Guid cid) CategoryId = cid;

        UpdatedAt = DateTime.UtcNow;
    }

    public int CompareTo(Product? other)
    {
        if (other is null) return 1;
        var nameComparison = string.Compare(Name, other.Name, StringComparison.OrdinalIgnoreCase);
        return nameComparison != 0 ? nameComparison : Price.CompareTo(other.Price);
    }
}
