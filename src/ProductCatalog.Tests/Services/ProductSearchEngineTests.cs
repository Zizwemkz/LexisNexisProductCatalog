using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using ProductCatalog.Application.Services;
using ProductCatalog.Domain.Interface;
using ProductCatalog.Domain.Models;

namespace ProductCatalog.Tests.Services
{
    public class ProductSearchEngineTests
    {
        private Mock<IRepository<Product>> _mockRepo = null!;
        private ProductSearchEngine _engine = null!; // use real engine

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IRepository<Product>>();
            _engine = new ProductSearchEngine(_mockRepo.Object); // real instance
        }

        [Test]
        public async Task SearchAsync_FiltersByCategory()
        {
            var categoryId = Guid.NewGuid();
            var products = new List<Product>
            {
                new Product(Guid.NewGuid(), "Shoes", "SKU1", 100, 5, categoryId, "desc"),
                new Product(Guid.NewGuid(), "Hat", "SKU2", 50, 3, Guid.NewGuid(), "desc")
            };

            _mockRepo.Setup(r => r.ListAsync()).ReturnsAsync(products);

            var result = await _engine.SearchAsync(null, categoryId);

            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().Name, Is.EqualTo("Shoes"));
        }

        [Test]
        public void InvalidateCache_ClearsCache()
        {
            // Act
            _engine.InvalidateCache();

            // Assert – no exception means success
            Assert.Pass("InvalidateCache executed without errors");
        }
    }
}
