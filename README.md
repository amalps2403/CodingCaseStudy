# GAC Integration Solution Documentation

### Overview
The GAC Integration Solution is a .NET 9-based solution designed to facilitate seamless integration between external Enterprise Resource Planning (ERP) systems and GAC's Warehouse Management System (WMS). The solution consists of two primary integration methods:

- Real-time RESTful APIs: Allow immediate data exchange for key entities such as Products, Purchase Orders (POs), Sales Orders (SOs), and Customers.

- Scheduled Polling and Transformation: Monitors a designated location for legacy XML files, periodically processes them, transforms the data, and integrates it into the WMS.

### Architecture Overview
The GAC Integration Solution follows a clean architecture with distinct layers to ensure separation of concerns, scalability, and maintainability.

### Solution Layers

- **GAC.IntegrationSolution.API**: ASP.NET Core Web API responsible for handling incoming HTTP requests and exposing RESTful APIs.

- **GAC.Application**: Contains business logic, services, interfaces, and Data Transfer Objects (DTOs).

- **GAC.Infrastructure**: Provides access to the database, file systems, external APIs, and scheduling logic, including the WMS adapter.

- **GAC.Domain**: Defines the core entities and enums used across the solution.

- **GAC.IntegrationSolution.Tests**: A project for unit testing, using xUnit or NUnit, ensuring code quality and reliability.

- **GAC.IntegrationSolution.FilePoller**: A worker service responsible for polling and processing legacy XML files on a scheduled basis.

 ### Project Structure

 GAC.IntegrationSolution

    ├── GAC.IntegrationSolution.API  → ASP.NET Core Web API
    ├── GAC.Application              → Business logic (Services, Interfaces, DTOs)
    ├── GAC.Infrastructure           → DB, file access, scheduler, WMS adapter
    ├── GAC.Domain                   → Entities and Enums
    ├── GAC.IntegrationSolution.Tests                → xUnit/ NUnit project
    ├── GAC.IntegrationSolution.FilePoller          → Worker Service for file polling

### Key Components

- **GAC.IntegrationSolution.API**: The entry point for all external requests (such as product data, orders, and customer requests). It processes these requests by interacting with services defined in the GAC.Application layer.

- **GAC.Application**: This layer houses all business logic and interactions with the domain entities, ensuring that the services operate according to the rules defined by the business.

- **GAC.Infrastructure**: Handles access to databases, file systems, external APIs, and integrates with GAC's WMS. It also includes functionality for scheduling tasks like the polling of XML files.

- **GAC.IntegrationSolution.FilePoller**: This worker service runs in the background and is responsible for periodically polling a designated folder for legacy XML files, transforming the data, and submitting it to the WMS via REST APIs.

## How to Run the Project Locally

Follow the steps below to set up and run the GAC Integration Solution on your local development environment.

#### Prerequisites

Ensure the following software is installed:

- .NET 9 SDK: Required to build and run .NET 9 applications.

- SQL Server or LocalDB: A database server to host the GAC database. LocalDB is recommended for lightweight, local development environments.

- Visual Studio 2022+ or VS Code: Integrated Development Environment (IDE) for building and debugging the solution. If you use VS Code, make sure to install the C# extension.

- (Optional) Docker Desktop: Use Docker to containerize and run parts of the solution or its dependencies.

#### Configuration

The configuration for the solution is managed primarily through the appsettings.json file located in the GAC.IntegrationSolution.Api project. You may need to adjust the connection strings and other settings to match your local development environment.

    {
      "ConnectionStrings": {
        "DefaultConnection": "Server=sql-server; Database=GacWmsIntegration; User ID=user-id;Password=pwd; TrustServerCertificate=True"
      },
      "Logging": {
        "LogLevel": {
          "Default": "Information",
          "Microsoft.AspNetCore": "Warning"
        }
      },
      "AllowedHosts": "*"
    }

#### Steps to Run Locally

- Clone the Repository: If you haven't already, clone the repository to your local machine using Git.
- Restore Dependencies: Run the following command in the solution directory to restore NuGet packages:
       dotnet restore
- Set Up Database: Ensure SQL Server (or LocalDB) is running. You may need to create the database manually or run migration scripts to set up the schema.
- Run the Application: Launch the solution in your preferred IDE (Visual Studio or VS Code) or from the command line.
- Test the Application: You can now access the API endpoints using Postman for testing the RESTful APIs, since swagger is not supporting in .Net 9.

#### Testing

The solution includes unit tests to ensure the correctness and reliability of the system.

- Unit Tests: The GAC.UnitTests project contains tests for the application's services, business logic, and data handling. You can run these tests using xUnit.


