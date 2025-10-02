using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using ProductCatalog.Domain.Models;
using ProductCatalog.Infrastructure.Data;
using ProductCatalog.Infrastructure.Repositories;

namespace ProductCatalog.Tests.Repositories
{
    public class EfRepositoryTests
    {
        private ApplicationDbContext _context = null!;
        private EfRepository<Product> _repo = null!;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // fresh DB per test
                .Options;

            _context = new ApplicationDbContext(options);
            _repo = new EfRepository<Product>(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task AddAsync_PersistsEntity()
        {
            var product = new Product(Guid.NewGuid(), "Test", "SKU", 10, 2, Guid.NewGuid(), "desc");

            await _repo.AddAsync(product);
            var fetched = await _repo.GetByIdAsync(product.Id);

            Assert.IsNotNull(fetched);
            Assert.That(fetched!.Name, Is.EqualTo("Test"));
        }

        [Test]
        public async Task DeleteAsync_RemovesEntity()
        {
            var product = new Product(Guid.NewGuid(), "Test", "SKU", 10, 2, Guid.NewGuid(), "desc");
            await _repo.AddAsync(product);

            await _repo.DeleteAsync(product.Id);
            var fetched = await _repo.GetByIdAsync(product.Id);

            Assert.IsNull(fetched);
        }

        [Test]
        public async Task ListAsync_ReturnsAllEntities()
        {
            var product1 = new Product(Guid.NewGuid(), "Shoes", "SKU1", 50, 10, Guid.NewGuid(), "desc");
            var product2 = new Product(Guid.NewGuid(), "Hat", "SKU2", 20, 5, Guid.NewGuid(), "desc");

            await _repo.AddAsync(product1);
            await _repo.AddAsync(product2);

            var results = await _repo.ListAsync();

            Assert.That(results.Count(), Is.EqualTo(2));
            Assert.That(results.Select(p => p.Name), Does.Contain("Shoes").And.Contain("Hat"));
        }
    }
}
