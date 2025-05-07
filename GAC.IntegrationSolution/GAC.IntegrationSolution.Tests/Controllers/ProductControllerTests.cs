using GAC.Application.Interfaces;
using GAC.Domain.Entities;
using GAC.IntegrationSolution.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GAC.IntegrationSolution.Tests.Controllers;

public class ProductControllerTests
{
    private readonly Mock<IProductService> _mockProductService;
    private readonly ProductController _controller;

    public ProductControllerTests()
    {
        _mockProductService = new Mock<IProductService>();
        _controller = new ProductController(_mockProductService.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsOkWithProducts()
    {
        // Arrange
        var products = new List<Product> { new Product { Id = Guid.NewGuid(), Title = "Test Product", Description = " moq test description", Dimensions = "23", ProductCode = "I-2002" } };
        _mockProductService.Setup(s => s.GetAllAsync()).ReturnsAsync(products);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(products, okResult.Value);
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenProductExists()
    {
        var id = Guid.NewGuid();
        var product = new Product { Id = id, Title ="sample title", Description =" moq test description", Dimensions = "23", ProductCode ="I-2002" };
        _mockProductService.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(product);

        var result = await _controller.GetById(id);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(product, okResult.Value);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenProductDoesNotExist()
    {
        _mockProductService.Setup(s => s.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Product)null);

        var result = await _controller.GetById(Guid.NewGuid());

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Create_ReturnsCreatedAtActionResult()
    {
        var product = new Product { Id = Guid.NewGuid(), Title = "New Product", Description = " moq test description", Dimensions = "244", ProductCode = "I-2003" };
        _mockProductService.Setup(s => s.CreateAsync(product)).ReturnsAsync(product);

        var result = await _controller.Create(product);

        var createdAtResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(product, createdAtResult.Value);
    }

    [Fact]
    public async Task Update_ReturnsNoContent_WhenSuccessful()
    {
        var product = new Product { Id = Guid.NewGuid(), Title = "Updated Product", Description = " moq test description", Dimensions = "23", ProductCode = "I-2002" };
        _mockProductService.Setup(s => s.UpdateAsync(product)).ReturnsAsync(true);

        var result = await _controller.Update(product.Id, product);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Update_ReturnsBadRequest_WhenIdMismatch()
    {
        var product = new Product { Id = Guid.NewGuid(), Title = "Mismatch", Description = " moq test description", Dimensions = "23", ProductCode = "I-2002" };

        var result = await _controller.Update(Guid.NewGuid(), product);

        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task Update_ReturnsNotFound_WhenUpdateFails()
    {
        var product = new Product { Id = Guid.NewGuid(), Title = "Fail Product", Description = " moq test description", Dimensions = "23", ProductCode = "I-2002" };
        _mockProductService.Setup(s => s.UpdateAsync(product)).ReturnsAsync(false);

        var result = await _controller.Update(product.Id, product);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Delete_ReturnsNoContent_WhenSuccessful()
    {
        var id = Guid.NewGuid();
        _mockProductService.Setup(s => s.DeleteAsync(id)).ReturnsAsync(true);

        var result = await _controller.Delete(id);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenDeleteFails()
    {
        var id = Guid.NewGuid();
        _mockProductService.Setup(s => s.DeleteAsync(id)).ReturnsAsync(false);

        var result = await _controller.Delete(id);

        Assert.IsType<NotFoundResult>(result);
    }
}
