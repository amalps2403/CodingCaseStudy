namespace GAC.IntegrationSolution.Tests;

using GAC.Application.Interfaces;
using GAC.Domain.Entities;
using GAC.IntegrationSolution.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;


public class CustomerControllerTests
{
    private readonly Mock<ICustomerService> _mockCustomerService;
    private readonly CustomerController _controller;
    public CustomerControllerTests()
    {
        _mockCustomerService = new Mock<ICustomerService>();
        _controller = new CustomerController(_mockCustomerService.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsOkWithCustomers()
    {
        // Arrange
        var customers = new List<Customer> { new Customer { Id = Guid.NewGuid(), Name = "Test Customer" } };
        _mockCustomerService.Setup(s => s.GetAllAsync()).ReturnsAsync(customers);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(customers, okResult.Value);
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenCustomerExists()
    {
        var id = Guid.NewGuid();
        var customer = new Customer { Id = id, Name = "Test Customer" };
        _mockCustomerService.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(customer);

        var result = await _controller.GetById(id);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(customer, okResult.Value);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenCustomerDoesNotExist()
    {
        _mockCustomerService.Setup(s => s.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Customer)null);

        var result = await _controller.GetById(Guid.NewGuid());

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Create_ReturnsCreatedAtActionResult()
    {
        var customer = new Customer { Id = Guid.NewGuid(), Name = "New Customer" };
        _mockCustomerService.Setup(s => s.CreateAsync(customer)).ReturnsAsync(customer);

        var result = await _controller.Create(customer);

        var createdAtResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(customer, createdAtResult.Value);
    }

    [Fact]
    public async Task Update_ReturnsNoContent_WhenSuccessful()
    {
        var customer = new Customer { Id = Guid.NewGuid(), Name = "Updated Customer" };
        _mockCustomerService.Setup(s => s.UpdateAsync(customer)).ReturnsAsync(true);

        var result = await _controller.Update(customer.Id, customer);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Update_ReturnsBadRequest_WhenIdMismatch()
    {
        var customer = new Customer { Id = Guid.NewGuid(), Name = "Wrong ID" };

        var result = await _controller.Update(Guid.NewGuid(), customer);

        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task Update_ReturnsNotFound_WhenCustomerDoesNotExist()
    {
        var customer = new Customer { Id = Guid.NewGuid(), Name = "Missing Customer" };
        _mockCustomerService.Setup(s => s.UpdateAsync(customer)).ReturnsAsync(false);

        var result = await _controller.Update(customer.Id, customer);

        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Delete_ReturnsNoContent_WhenSuccessful()
    {
        var id = Guid.NewGuid();
        _mockCustomerService.Setup(s => s.DeleteAsync(id)).ReturnsAsync(true);

        var result = await _controller.Delete(id);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenCustomerDoesNotExist()
    {
        var id = Guid.NewGuid();
        _mockCustomerService.Setup(s => s.DeleteAsync(id)).ReturnsAsync(false);

        var result = await _controller.Delete(id);

        Assert.IsType<NotFoundResult>(result);
    }

}
