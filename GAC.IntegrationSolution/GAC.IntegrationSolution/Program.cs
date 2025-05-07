using GAC.Application.Interfaces;
using GAC.Application.Services;
using GAC.Infrastructure.Persistence;
using GAC.IntegrationSolution.API.Middleware;
using GAC.IntegrationSolution.FilePoller.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container  
builder.Services.AddControllers();

// Access Configuration from builder  
var configuration = builder.Configuration;

// Register DbContext  
builder.Services.AddDbContext<AppDbContext>(options =>
       options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));   

// Register Application Services (DI)  
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IPurchaseOrderService, PurchaseOrderService>();
builder.Services.AddScoped<ISalesOrderService, SalesOrderService>();
builder.Services.AddHttpClient();

// Register the background service (FilePollingService)
builder.Services.AddHostedService<FilePollingService>();

builder.Services.AddEndpointsApiExplorer();


var app = builder.Build();


if (app.Environment.IsDevelopment())
{
     
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
