#nullable enable
using System;

namespace ProductCatalog.Application.DTOs;

public record ProductDto(Guid Id, string Name, string? Description, string SKU, decimal Price, int Quantity, Guid CategoryId, DateTime CreatedAt, DateTime UpdatedAt);
