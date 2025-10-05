# LexisNexisProductCatalog<div align="center">
  <h1>
      Product Catalog Application
  </h1>
  <p>
    A fully architected .NET product catalog system with clean architecture principles. <br/>
    Designed for modularity, testability, and extensibility. <br/><br/>
    This project demonstrates domain-driven design, layered architecture, and a RESTful API for managing products and categories.
  </p>
</div>

# Key Features

* **Product Management:** Create, update, delete, and list products with categories.
* **Domain Models:** Clear separation of concerns with `Product` and `Category` entities.
* **Clean Architecture:** Separation into Domain, Application, Infrastructure, and Presentation layers.
* **Unit Test Ready:** Interfaces and dependency injection for easy mocking.
* **REST API:** Easily extendable API endpoints for integration with front-end applications.
* **Containerization-Ready:** Infrastructure isolated for future Dockerization or cloud deployment.

# Project Structure

* **Domain (`ProductCatalog.Domain`)**
  - `/Enums` – Enumerations such as product status or categories.
  - `/Interface` – Repository interfaces and domain service contracts.
  - `/Models` – Core entities like `Product` and `Category`.
* **Application (`ProductCatalog.Application`)**
  - Use case implementations for CRUD operations.
  - Business rules and validation logic.
* **Infrastructure (`ProductCatalog.Infrastructure`)**
  - Data access layer, Entity Framework or repository implementations.
  - Configuration and database context setup.
* **Presentation (`ProductCatalog.Presentation`)**
  - API controllers or UI components.
  - Front-end integration or REST endpoints.
* **Tests (`ProductCatalog.Tests`)**
  - Unit tests for domain entities, application services, and infrastructure logic.

# Getting Started

## Prerequisites

- .NET 9.0 SDK or later
- Visual Studio / VS Code / JetBrains Rider
- Docker (optional for containerized execution)

## Setup Steps

1. **Clone the Repository**
    ```bash
    git clone https://github.com/Zizwemkz/LexisNexisProductCatalog.git
    cd product-catalog
    ```

2. **Build the Solution**
    ```bash
    dotnet build
    ```

3. **Run the Application**
    ```bash
    dotnet run --project ProductCatalog.Presentation
    ```

## Docker Startup Steps

1. **Build the Docker image**
    ```bash
    docker build -t product-catalog .
    ```

2. **Run the container**
    ```bash
    docker run --rm -p 5000:5000 product-catalog
    ```

## API Endpoints

| Method | Endpoint                     | Description                    |
|--------|------------------------------|--------------------------------|
| GET    | `/api/products`              | List all products              |
| GET    | `/api/products/{id}`         | Get product by ID              |
| POST   | `/api/products`              | Create a new product           |
| PUT    | `/api/products/{id}`         | Update an existing product     |
| DELETE | `/api/products/{id}`         | Delete a product               |
| GET    | `/api/categories`            | List all categories            |
| GET    | `/api/categories/{id}`       | Get category by ID             |

# Current Project Build

![Architecture Diagram](./designs/build.png)

# Configure Application

```csharp
public static class AppConfig
{
    public const int DefaultPageSize = 20;
    public const string DefaultCurrency = "USD";
}
