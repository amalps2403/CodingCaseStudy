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

public class SalesOrderControllerTests
{
    private readonly Mock<ISalesOrderService> _mockService;
    private readonly SalesOrderController _controller;

    public SalesOrderControllerTests()
    {
        _mockService = new Mock<ISalesOrderService>();
        _controller = new SalesOrderController(_mockService.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsOk_WithSalesOrders()
    {
        // Arrange
        var salesOrders = new List<SalesOrder>
        {
            new SalesOrder
            {
                Id = Guid.NewGuid(),
                OrderId = "SO1001",
                ProcessingDate = DateTime.UtcNow,
                CustomerId = Guid.NewGuid(),
                ShipmentAddress = "Warehouse A"
            }
        };

        _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(salesOrders);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(salesOrders, okResult.Value);
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenExists()
    {
        var id = Guid.NewGuid();
        var salesOrder = new SalesOrder
        {
            Id = id,
            OrderId = "SO2002",
            ProcessingDate = DateTime.UtcNow,
            CustomerId = Guid.NewGuid(),
            ShipmentAddress = "Dock 2"
        };

        _mockService.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(salesOrder);

        var result = await _controller.GetById(id);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(salesOrder, okResult.Value);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenNotExists()
    {
        _mockService.Setup(s => s.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((SalesOrder)null!);

        var result = await _controller.GetById(Guid.NewGuid());

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Create_ReturnsCreatedAtAction()
    {
        var salesOrder = new SalesOrder
        {
            Id = Guid.NewGuid(),
            OrderId = "SO3003",
            ProcessingDate = DateTime.UtcNow,
            CustomerId = Guid.NewGuid(),
            ShipmentAddress = "Site B"
        };

        _mockService.Setup(s => s.CreateAsync(salesOrder)).ReturnsAsync(salesOrder);

        var result = await _controller.Create(salesOrder);

        var createdAt = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(salesOrder, createdAt.Value);
        Assert.Equal(nameof(_controller.GetById), createdAt.ActionName);
    }

    [Fact]
    public async Task Update_ReturnsNoContent_WhenSuccessful()
    {
        var salesOrder = new SalesOrder
        {
            Id = Guid.NewGuid(),
            OrderId = "SO4004",
            ProcessingDate = DateTime.UtcNow,
            CustomerId = Guid.NewGuid(),
            ShipmentAddress = "Depot X"
        };

        _mockService.Setup(s => s.UpdateAsync(salesOrder)).ReturnsAsync(true);

        var result = await _controller.Update(salesOrder.Id, salesOrder);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Update_ReturnsBadRequest_WhenIdMismatch()
    {
        var salesOrder = new SalesOrder
        {
            Id = Guid.NewGuid(),
            OrderId = "SO5005"
        };

        var result = await _controller.Update(Guid.NewGuid(), salesOrder);

        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task Update_ReturnsNotFound_WhenFails()
    {
        var salesOrder = new SalesOrder
        {
            Id = Guid.NewGuid(),
            OrderId = "SO6006"
        };

        _mockService.Setup(s => s.UpdateAsync(salesOrder)).ReturnsAsync(false);

        var result = await _controller.Update(salesOrder.Id, salesOrder);

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
