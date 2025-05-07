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

public class PurchaseOrderControllerTests
{
    private readonly Mock<IPurchaseOrderService> _mockService;
    private readonly PurchaseOrderController _controller;

    public PurchaseOrderControllerTests()
    {
        _mockService = new Mock<IPurchaseOrderService>();
        _controller = new PurchaseOrderController(_mockService.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsOkResult_WithPurchaseOrders()
    {
        // Arrange
        var orders = new List<PurchaseOrder>
    {
        new PurchaseOrder
        {
            Id = Guid.NewGuid(),
            OrderId = "PO123",
            ProcessingDate = DateTime.UtcNow,
            CustomerId = Guid.NewGuid()
        }
    };

        _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(orders);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(orders, okResult.Value);
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenPurchaseOrderExists()
    {
        var id = Guid.NewGuid();
        var order = new PurchaseOrder
        {
            Id = id,
            OrderId = "PO456",
            ProcessingDate = DateTime.UtcNow,
            CustomerId = Guid.NewGuid()
        };

        _mockService.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(order);

        var result = await _controller.GetById(id);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(order, okResult.Value);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenNotExists()
    {
        _mockService.Setup(s => s.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((PurchaseOrder)null!);

        var result = await _controller.GetById(Guid.NewGuid());

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Create_ReturnsCreatedAtActionResult()
    {
        var order = new PurchaseOrder
        {
            Id = Guid.NewGuid(),
            OrderId = "PO789",
            ProcessingDate = DateTime.UtcNow,
            CustomerId = Guid.NewGuid()
        };

        _mockService.Setup(s => s.CreateAsync(order)).ReturnsAsync(order);

        var result = await _controller.Create(order);

        var createdAt = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(order, createdAt.Value);
    }

    [Fact]
    public async Task Update_ReturnsNoContent_WhenSuccessful()
    {
        var order = new PurchaseOrder
        {
            Id = Guid.NewGuid(),
            OrderId = "PO999",
            ProcessingDate = DateTime.UtcNow,
            CustomerId = Guid.NewGuid()
        };

        _mockService.Setup(s => s.UpdateAsync(order)).ReturnsAsync(true);

        var result = await _controller.Update(order.Id, order);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Update_ReturnsBadRequest_WhenIdMismatch()
    {
        var order = new PurchaseOrder { Id = Guid.NewGuid() };

        var result = await _controller.Update(Guid.NewGuid(), order);

        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task Update_ReturnsNotFound_WhenUpdateFails()
    {
        var order = new PurchaseOrder { Id = Guid.NewGuid() };

        _mockService.Setup(s => s.UpdateAsync(order)).ReturnsAsync(false);

        var result = await _controller.Update(order.Id, order);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Delete_ReturnsNoContent_WhenSuccessful()
    {
        var id = Guid.NewGuid();
        _mockService.Setup(s => s.DeleteAsync(id)).ReturnsAsync(true);

        var result = await _controller.Delete(id);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenFails()
    {
        var id = Guid.NewGuid();
        _mockService.Setup(s => s.DeleteAsync(id)).ReturnsAsync(false);

        var result = await _controller.Delete(id);

        Assert.IsType<NotFoundResult>(result);
    }
}
