using NUnit.Framework;
using Moq;
using ProductCatalog.Presentation.Controllers;
using ProductCatalog.Domain.Interface;
using ProductCatalog.Domain.Models;
using ProductCatalog.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProductCatalog.Application.DTOs;

namespace ProductCatalog.Tests.Controllers
{
    public class ProductsControllerTests
    {
        private Mock<IRepository<Product>> _mockRepo = null!;
        private Mock<IProductSearchEngine> _mockSearchEngine = null!;
        private ProductsController _controller = null!;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IRepository<Product>>();
            _mockSearchEngine = new Mock<IProductSearchEngine>();
            _controller = new ProductsController(_mockRepo.Object, _mockSearchEngine.Object);
        }

        [Test]
        public async Task GetById_ReturnsOk_WhenProductExists()
        {
            var product = new Product(Guid.NewGuid(), "Shoes", "SKU123", 100, 5, Guid.NewGuid(), "Nice shoes");

            _mockRepo.Setup(r => r.GetByIdAsync(product.Id))
                     .ReturnsAsync(product);

            var result = await _controller.GetById(product.Id) as OkObjectResult;

            Assert.IsNotNull(result);
            var returnedProduct = result!.Value as Product;
            Assert.That(returnedProduct!.Name, Is.EqualTo("Shoes"));
        }

        [Test]
        public async Task Create_AddsProduct_AndReturnsCreated()
        {
            var dto = new ProductDto(Guid.NewGuid(), "Test", "desc", "SKU", 10, 5, Guid.NewGuid(), DateTime.UtcNow, DateTime.UtcNow);

            var result = await _controller.Create(dto);

            _mockRepo.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Once);
            _mockSearchEngine.Verify(s => s.InvalidateCache(), Times.Once);
            Assert.IsInstanceOf<CreatedAtActionResult>(result);
        }
    }
}
