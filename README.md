# RESTful Backend API Solution - Technical Assessment

## Overview

This project implements a RESTful Product Management API using ASP.NET Core (.NET 8) and follows Clean Architecture principles. The solution is designed to be scalable, maintainable, secure, and aligned with industry best practices.

The application provides secure Product CRUD operations with JWT Authentication, Refresh Token strategy, Entity Framework Core, SQL Server, FluentValidation, Serilog logging, Swagger/OpenAPI documentation, Docker support, and comprehensive testing.

---

# Problem Statement

Design and implement a RESTful API around Products to perform CRUD operations while demonstrating:

- Secure authentication and authorization
- Data validation
- Layered architecture
- Logging
- Exception handling
- Testing
- Containerized deployment using Docker

---

# High-Level Technical Architecture

```
                        Client / Swagger UI
                               │
                               ▼
                    ASP.NET Core Web API
                               │
        ┌──────────────────────┼──────────────────────┐
        │                      │                      │
 Authentication         Product Controller   Exception Middleware
        │                      │
        ▼                      ▼
                 Application Layer
      (Services, DTOs, Validators, AutoMapper)
                               │
                               ▼
                     Domain Layer
                 (Entities & Business Models)
                               │
                               ▼
                Infrastructure Layer
 (Repositories, Unit of Work, Entity Framework Core)
                               │
                               ▼
                       SQL Server Database
```

---

# Tech Stack

| Technology | Description |
|------------|-------------|
| Framework | .NET 8 |
| Language | C# |
| API Framework | ASP.NET Core Web API |
| Database | SQL Server |
| ORM | Entity Framework Core |
| Authentication | JWT Authentication with Refresh Tokens |
| Validation | FluentValidation |
| Object Mapping | AutoMapper |
| Logging | Serilog |
| API Documentation | Swagger / OpenAPI |
| Testing | xUnit, Moq, WebApplicationFactory |
| Containerization | Docker & Docker Compose |

---

# Project Structure

```
Solution
│
├── API
│   ├── Controllers
│   ├── Middleware
│   ├── Program.cs
│   └── appsettings.json
│
├── Application
│   ├── DTOs
│   ├── Interfaces
│   ├── Mapping
│   ├── Services
│   └── Validators
│
├── Domain
│   ├── Entities
│   └── Common
│
├── Infrastructure
│   ├── Data
│   ├── Identity
│   ├── Repositories
│   └── UnitOfWork
│
├── API.Tests
├── Application.Tests
├── Infrastructure.Tests
│
├── Dockerfile
├── docker-compose.yml
├── README.md
└── CRMProductApi.sln
```



```

# API Design

## Resource-Oriented API

The API follows RESTful principles where each endpoint represents a resource and standard HTTP methods are used to perform operations.

### Product Endpoints

| Method | Endpoint | Description |
|---------|----------|-------------|
| GET | `/api/products` | Retrieve all products with pagination |
| GET | `/api/products/{id}` | Retrieve a product by its unique identifier |
| POST | `/api/products` | Create a new product |
| PUT | `/api/products/{id}` | Update an existing product |
| DELETE | `/api/products/{id}` | Delete a product |

### Authentication Endpoints

| Method | Endpoint | Description |
|---------|----------|-------------|
| POST | `/api/auth/login` | Authenticate user and generate JWT Access Token and Refresh Token |
| POST | `/api/auth/refresh-token` | Generate a new Access Token using a valid Refresh Token |

---

# Request & Response Format

The API communicates using JSON for both requests and responses.

Example Request

```json
{
  "productName": "Laptop"
}
```

Example Response

```json
{
  "id": 1,
  "productName": "Laptop",
  "createdBy": "Admin",
  "createdOn": "2026-07-20T10:30:00"
}
```

The API returns appropriate HTTP status codes including:

- 200 OK
- 201 Created
- 204 No Content
- 400 Bad Request
- 401 Unauthorized
- 403 Forbidden
- 404 Not Found
- 500 Internal Server Error

Global Exception Middleware ensures a consistent JSON error response format.

---

# Database Structure

The application uses SQL Server with Entity Framework Core for data persistence.

### Product

| Column | Type |
|---------|------|
| Id | int |
| ProductName | nvarchar(255) |
| CreatedBy | nvarchar(100) |
| CreatedOn | datetime |
| ModifiedBy | nvarchar(100) |
| ModifiedOn | datetime |

### Item

| Column | Type |
|---------|------|
| Id | int |
| ProductId | int |
| Quantity | int |

---

# Implementation Highlights

## Authentication & Authorization

Authentication is implemented using JWT (JSON Web Token) with Refresh Token support.

Features include:

- JWT Access Token Authentication
- Refresh Token Rotation
- Role-Based Authorization
- Bearer Token Authentication
- Secure Token Validation

Administrative privileges are required to create, update, and delete products.

---

## Error Handling Middleware

A custom Global Exception Middleware is implemented to handle unhandled exceptions across the application.

Responsibilities include:

- Centralized exception handling
- Consistent error response format
- Appropriate HTTP status codes
- Logging of unexpected exceptions

---

## Data Validation using FluentValidation

Request validation is implemented using FluentValidation.

Validation includes:

- Required field validation
- Business rule validation
- Input validation before processing requests
- Meaningful validation error messages

---

## Controller Layer

The API controllers are responsible for:

- Handling HTTP requests
- Returning appropriate HTTP responses
- Authorization
- Model binding
- Logging
- XML documentation comments for API documentation

Controllers remain lightweight by delegating business logic to the Service Layer.

---

## Service Layer

The Service Layer contains the application's business logic.

Responsibilities include:

- Business rule implementation
- DTO mapping using AutoMapper
- Repository coordination
- Validation
- Exception handling

This approach keeps controllers clean and promotes separation of concerns.

---

## Repository Pattern

The Repository Pattern is implemented using Entity Framework Core.

Key features include:

- Generic database operations
- Unit of Work implementation
- Dependency Injection
- Asynchronous database operations
- Improved testability
- Separation of business logic from data access

# Testing Strategy

The solution includes both unit and integration testing to ensure application reliability and maintainability.

### Testing Frameworks

- xUnit
- Moq
- WebApplicationFactory

### Test Coverage

The project includes:

- Unit tests for the Service Layer
- Repository tests
- API Integration tests
- Authentication tests
- CRUD operation tests

Run all tests using:

```bash
dotnet test
```

---

# Performance Considerations

The application is designed with performance in mind by implementing the following techniques:

- Asynchronous programming using Async/Await
- Entity Framework Core query optimization
- Pagination for collection endpoints
- Response Compression (Brotli & GZip)
- Efficient dependency injection
- Optimized database queries using Entity Framework Core

---

# Security Measures

The API follows security best practices.

Implemented security features include:

- JWT Authentication
- Refresh Token Rotation
- Role-Based Authorization
- FluentValidation for input validation
- HTTPS support
- Security Headers Middleware
- CORS Policy
- Authentication using Bearer Tokens

---

# Documentation

## OpenAPI / Swagger

Swagger (OpenAPI) is enabled for all API endpoints and provides interactive API documentation.

### Docker

```
http://localhost:5000/swagger
```

### Local Development

```
https://localhost:7236/swagger
```

Swagger includes:

- API endpoint documentation
- Request and response models
- JWT Authentication support
- Interactive API testing

---

## Code Documentation

XML documentation comments (`///`) have been added to public controllers and API methods.

Since this project is developed using C# and ASP.NET Core, XML documentation comments are used as the standard equivalent of JSDoc for documenting public APIs.

---

## High-Level Authentication Flow

1. User submits valid credentials to the Login endpoint.
2. The API validates the credentials.
3. A JWT Access Token and Refresh Token are generated.
4. The client stores both tokens securely.
5. The Access Token is included in the Authorization header for protected API requests.
6. When the Access Token expires, the client calls the Refresh Token endpoint.
7. The API validates the Refresh Token and issues a new Access Token and Refresh Token.

---

## Environment Setup

### Prerequisites

- .NET 8 SDK
- SQL Server
- Docker Desktop
- Visual Studio 2022
- Git

### Local Setup

```bash
git clone <repository-url>

dotnet restore

dotnet build

dotnet run
```

### Docker Setup

```bash
docker compose up --build
```

Open Swagger:

```
http://localhost:5000/swagger
```

---

## Deployment Procedure

### Local Deployment

```bash
dotnet publish -c Release
```

### Docker Deployment

Build and run the application:

```bash
docker compose up --build
```

Stop the application:

```bash
docker compose down
```

---

# Deployment Configuration

## Dockerfile

The project uses a multi-stage Docker build to create an optimized runtime image.

The Dockerfile performs the following steps:

- Restores project dependencies
- Builds the application
- Publishes the application
- Creates a lightweight ASP.NET Core runtime image
- Exposes port 8080

---

## Docker Compose

The `docker-compose.yml` file provisions the following services:

- ASP.NET Core Web API
- SQL Server 2022
- Persistent SQL Server Docker Volume
- Internal Docker Network
- Environment Variables
- Container Dependencies

Run the application using:

```bash
docker compose up --build
```

---

# Logging

Structured logging is implemented using Serilog.

Application logs are written to:

- Console
- Rolling log files located in the **Logs** directory

---

# Submission

The project is available in a public GitHub repository and includes:

- Complete Source Code
- README.md
- Dockerfile
- docker-compose.yml
- Entity Framework Core Migrations
- Test Projects
- Solution File

---

# Author

**Pallavi Yelmule**