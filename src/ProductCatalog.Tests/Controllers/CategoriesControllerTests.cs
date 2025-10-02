using NUnit.Framework;
using Moq;
using ProductCatalog.Domain.Interface;
using ProductCatalog.Domain.Models;
using ProductCatalog.Presentation.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductCatalog.Tests.Controllers
{
    public class CategoriesControllerTests
    {
        private Mock<IRepository<Category>> _mockRepo = null!;
        private CategoriesController _controller = null!;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IRepository<Category>>();
            _controller = new CategoriesController(_mockRepo.Object);
        }

        [Test]
        public async Task List_ReturnsOk_WithCategories()
        {
            // Use constructor, not object initializer
            var category = new Category(Guid.NewGuid(), "Test");

            _mockRepo.Setup(r => r.ListAsync())
                .ReturnsAsync(new List<Category> { category });

            var result = await _controller.List() as OkObjectResult;

            Assert.IsNotNull(result);
            Assert.That(result!.Value, Is.InstanceOf<IEnumerable<Category>>());
        }

        [Test]
        public async Task Create_CallsAddAsync_AndReturnsCreated()
        {
            var category = new Category(Guid.NewGuid(), "New");

            var result = await _controller.Create(category);

            _mockRepo.Verify(r => r.AddAsync(category), Times.Once);
            Assert.IsInstanceOf<CreatedAtActionResult>(result);
        }
    }
}
