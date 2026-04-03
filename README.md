# Restaurants API

## Overview
Restaurants API is a layered ASP.NET Core Web API for managing restaurants and dishes, with built-in identity, role-based authorization, CQRS, validation, and structured logging.

It follows a clean separation between API, Application, Domain, and Infrastructure layers and uses MySQL via Entity Framework Core.

## Architecture

The solution is organized as:

```text
src/
├── Restaurants.APIs/           # Presentation layer (controllers, middleware, startup)
├── Restaurants.Application/    # CQRS handlers, DTOs, validators, mapping, user context
├── Restaurants.Domain/         # Entities, constants, exceptions, repository contracts
└── Restaurants.Infrastructure/ # EF Core, repositories, identity, auth handlers, seeders

tests/
└── Restaurants.Tests/          # Unit and integration tests
```

### Layer Responsibilities
- **Restaurants.APIs**: Exposes HTTP endpoints, configures middleware, Swagger, and authentication/authorization.
- **Restaurants.Application**: Implements MediatR commands/queries, FluentValidation rules, AutoMapper profiles, and application use-cases.
- **Restaurants.Domain**: Contains core business models and contracts independent from infrastructure details.
- **Restaurants.Infrastructure**: Implements persistence, Identity API endpoints, authorization requirements, and database seeding.

## Key Features
- **Clean architecture style** with clear layer boundaries.
- **CQRS with MediatR** for commands and queries.
- **FluentValidation** for request validation.
- **Entity Framework Core + Pomelo MySQL provider** for persistence.
- **ASP.NET Core Identity API endpoints** mapped under `api/auth`.
- **Role-based authorization** (`Admin`, `Owner`, `User`) plus policy-based authorization.
- **Custom middleware** for centralized error handling and slow-request logging.
- **Serilog logging** to console and rolling log files under `Logs/`.
- **Automatic seed data** for restaurants and roles at startup.

## Tech Stack
- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- Pomelo.EntityFrameworkCore.MySql
- MediatR
- FluentValidation
- AutoMapper
- Serilog
- xUnit, Moq, FluentAssertions

## Getting Started

### Prerequisites
- .NET 8 SDK
- MySQL Server (8.x recommended)
- IDE: Visual Studio 2022 or VS Code

### 1) Configure Connection Strings
Update `src/Restaurants.APIs/appsettings.Development.json`:

```json
{
	"AllowedHosts": "*",
	"ConnectionStrings": {
		"DefaultConnection": "server=localhost;database=restaurantsDB;user=root;password=Database#678;",
		"IdentityConnection": "server=localhost;database=Identityrestaurants;user=root;password=Database#678;"
	},
	"Serilog": {
		"MinimumLevel": {
			"Override": {
				"Microsoft": "Warning",
				"Microsoft.EntityFrameworkCore": "Information"
			}
		},
		"WriteTo": [
			{
				"Name": "Console",
				"Args": {
					"outputTemplate": "[{Timestamp:dd-MM HH:mm:ss} {Level:u3}] |{SourceContext}| {NewLine}{Message:lj}{NewLine}{Exception}"
				}
			},
			{
				"Name": "File",
				"Args": {
					"path": "Logs/Restaurant-API-.log",
					"rollingInterval": "Day",
					"rollOnFileSizeLimit": true
				}
			}
		]
	}
}
```


### 2) Restore Dependencies
```bash
dotnet restore
```

### 3) Apply Database Migrations
From repository root:

```bash
dotnet ef database update --project src/Restaurants.Infrastructure --startup-project src/Restaurants.APIs
```

### 4) Run the API
```bash
dotnet run --project src/Restaurants.APIs
```

Default launch profile URLs include:
- `http://localhost:5171`
- `https://localhost:7181`

Swagger UI:
- `http://localhost:5171/swagger`
- `https://localhost:7181/swagger`

## Authentication & Authorization

### Roles
Seeded roles:
- `Admin`
- `Owner`
- `User`

### Policies
Registered policies:
- `HasNationality`
- `AtLeast20`
- `CreatedAtLeast2Restaurants`

## API Endpoints

### Restaurants
- `GET /api/restaurants` (anonymous)
- `GET /api/restaurants/{id}`
- `POST /api/restaurants` (requires `Owner` role)
- `PATCH /api/restaurants/{id}`
- `DELETE /api/restaurants/{id}`

`GET /api/restaurants` supports query parameters through `GetAllRestaurantsQuery`:
- `searchPhrase`
- `pageNumber` (>= 1)
- `pageSize` (allowed: 5, 10, 15, 20, 50)
- `sortBy` (`Name`, `Description`, `Category`)
- `sortDirection` (`Ascending`, `Descending`)

### Dishes (per Restaurant)
- `POST /api/restaurant/{restaurantId}/dishes`
- `GET /api/restaurant/{restaurantId}/dishes`
- `GET /api/restaurant/{restaurantId}/dishes/{dishId}`
- `DELETE /api/restaurant/{restaurantId}/dishes`

### Identity (Standard + Custom)
Standard ASP.NET Core Identity endpoints 
- `POST /api/auth/register`
- `POST /api/auth/login`
- `POST /api/auth/refresh`
- `POST /api/auth/logout`
- `GET /api/auth/confirmEmail`
- `POST /api/auth/resendConfirmationEmail`
- `POST /api/auth/forgotPassword`
- `POST /api/auth/resetPassword`
- `GET /api/auth/manage/info`
- `POST /api/auth/manage/info`
- `POST /api/auth/manage/2fa`

Custom endpoints in `IdentityController`:
- `PATCH /api/auth/user` (authorized user)
- `POST /api/auth/userRole` (requires `Admin` role)
- `DELETE /api/auth/userRole` (requires `Admin` role)

## Middleware & Logging
- **ErrorHandlingMiddleware** maps:
	- Not Found exceptions -> `404`
	- Forbidden exceptions -> `403`
	- Unhandled exceptions -> `500`
- **RequestTimeLoggingMiddleware** logs slow requests (> 4 seconds).
- **Serilog** writes logs to console and rolling files in `src/Restaurants.APIs/Logs/`.

## Seed Data
On startup, the application seeds:
- Initial restaurant data (sample restaurants + dishes)
- Identity roles (`Admin`, `Owner`, `User`)

## Running Tests
From repository root:

```bash
dotnet test
```

Test project location:
- `tests/Restaurants.Tests`

## Development Workflow
1. Create/update your feature in the appropriate layer.
2. Run tests with `dotnet test`.
3. Run API and inspect endpoints via Swagger.
4. Verify logs under `src/Restaurants.APIs/Logs/` when debugging.

## License
This project is licensed under the MIT License. See `LICENSE` for details.
