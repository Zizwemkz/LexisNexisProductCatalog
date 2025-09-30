using Microsoft.EntityFrameworkCore;
using ProductCatalog.Domain.Entities;
using ProductCatalog.Domain.Repositories;
using ProductCatalog.Infrastructure.Data;
using ProductCatalog.Infrastructure.Repositories;
using ProductCatalog.Application.Services;
using System.Text.Json;
using ProductCatalog.Presentation.Middleware;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(opt =>
{
    opt.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ProductCatalog API", Version = "v1" });
});

// Use SQL Server for EF Core
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add health checks for SQL Server
builder.Services.AddHealthChecks()
    .AddSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")!,
        healthQuery: "SELECT 1;",
        name: "sqlserver",
        tags: new[] { "db", "sql", "sqlserver" }
    );

// Register repositories - EF-backed for persistence
builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
builder.Services.AddScoped<ProductSearchEngine>();

var app = builder.Build();

// Ensure database is created and seed data if needed
using (var scope = app.Services.CreateScope())
{
    var svc = scope.ServiceProvider;
    var ctx = svc.GetRequiredService<ApplicationDbContext>();
    ctx.Database.EnsureCreated();

    if (!ctx.Categories.Any())
    {
        var root = new Category(Guid.NewGuid(), "Electronics", "Electronic items");
        var phones = new Category(Guid.NewGuid(), "Mobile Phones", "Smartphones", root.Id);
        var laptops = new Category(Guid.NewGuid(), "Laptops", "Portable computers", root.Id);
        ctx.Categories.AddRange(root, phones, laptops);

        ctx.Products.Add(new Product(Guid.NewGuid(), "iPhone 14", "IP14-001", 999.99m, 10, phones.Id, "Apple smartphone"));
        ctx.Products.Add(new Product(Guid.NewGuid(), "Galaxy S22", "GS22-001", 799.99m, 15, phones.Id, "Samsung smartphone"));
        ctx.Products.Add(new Product(Guid.NewGuid(), "XPS 13", "XPS13-001", 1199.99m, 5, laptops.Id, "Dell laptop"));
        ctx.SaveChanges();
    }
}

app.UseHttpsRedirection();

// custom exception middleware
app.UseMiddleware<CustomExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
