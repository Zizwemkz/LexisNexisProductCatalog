#nullable enable
using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Domain.Entities;
using ProductCatalog.Domain.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ProductCatalog.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly IRepository<Category> _repo;

    public CategoriesController(IRepository<Category> repo) => _repo = repo;

    [HttpGet]
    public async Task<IActionResult> List() => Ok(await _repo.ListAsync());

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Category c)
    {
        await _repo.AddAsync(c);
        return CreatedAtAction(nameof(List), null);
    }

    [HttpGet("tree")]
    public async Task<IActionResult> Tree()
    {
        var all = (await _repo.ListAsync()).ToList();
        var dict = all.ToDictionary(c => c.Id);
        var roots = new List<Category>();
        foreach (var cat in all)
        {
            if (cat.ParentCategoryId is Guid pid && dict.TryGetValue(pid, out var parent)) parent.Children.Add(cat);
            else roots.Add(cat);
        }
        return Ok(roots);
    }
}
