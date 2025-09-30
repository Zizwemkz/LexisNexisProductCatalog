#nullable enable
using System;
using System.Collections.Generic;

namespace ProductCatalog.Domain.Entities;

public class Category
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public Guid? ParentCategoryId { get; private set; }
    public List<Category> Children { get; } = new();

    protected Category() { }
    public Category(Guid id, string name, string? description = null, Guid? parentCategoryId = null)
    {
        Id = id == Guid.Empty ? Guid.NewGuid() : id;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Description = description;
        ParentCategoryId = parentCategoryId;
    }
}
