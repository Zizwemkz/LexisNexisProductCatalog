#nullable enable
using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Domain.Entities;
using ProductCatalog.Domain.Repositories;
using ProductCatalog.Application.Services;
using ProductCatalog.Application.DTOs;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProductCatalog.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IRepository<Product> _productRepo;
    private readonly ProductSearchEngine _searchEngine;

    public ProductsController(IRepository<Product> productRepo, ProductSearchEngine searchEngine)
    {
        _productRepo = productRepo;
        _searchEngine = searchEngine;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? q, [FromQuery] Guid? categoryId)
    {
        var results = await _searchEngine.SearchAsync(q, categoryId);
        var dtos = results.Select(p => new ProductDto(p.Id, p.Name, p.Description, p.SKU, p.Price, p.Quantity, p.CategoryId, p.CreatedAt, p.UpdatedAt));
        return Ok(dtos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var p = await _productRepo.GetByIdAsync(id);
        if (p is null) return NotFound();
        return Ok(p);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductDto dto)
    {
        var product = new Product(Guid.NewGuid(), dto.Name, dto.SKU, dto.Price, dto.Quantity, dto.CategoryId, dto.Description);
        await _productRepo.AddAsync(product);
        _searchEngine.InvalidateCache();
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, dto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] ProductDto dto)
    {
        var existing = await _productRepo.GetByIdAsync(id);
        if (existing is null) return NotFound();
        existing.Update(dto.Name, dto.Description, dto.Price, dto.Quantity, dto.CategoryId);
        await _productRepo.UpdateAsync(existing);
        _searchEngine.InvalidateCache();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _productRepo.DeleteAsync(id);
        _searchEngine.InvalidateCache();
        return NoContent();
    }

    [HttpPost("manual-bind")]
    public async Task<IActionResult> CreateManualBinding()
    {
        using var reader = new System.IO.StreamReader(Request.Body);
        var body = await reader.ReadToEndAsync();
        var dto = JsonSerializer.Deserialize<ProductDto?>(body);
        if (dto is null) return BadRequest("Invalid payload");
        var product = new Product(Guid.NewGuid(), dto.Name, dto.SKU, dto.Price, dto.Quantity, dto.CategoryId, dto.Description);
        await _productRepo.AddAsync(product);
        _searchEngine.InvalidateCache();
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, dto);
    }
}
