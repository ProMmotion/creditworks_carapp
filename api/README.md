# CarApp API — Code Structure and Business Model

## 1. Purpose and scope

This repository contains the backend API for a car catalog/management application. It exposes HTTP endpoints for cars, brands, vehicle models, and categories. The API is built with ASP.NET Core and persists data through Entity Framework Core and SQLite.

This document describes the implementation as it exists in the repository. It distinguishes working concepts from incomplete areas; in particular, ownership is represented in the data model but its HTTP endpoint is currently a stub.

## 2. Technology and runtime

- **Runtime/framework:** .NET 10 (`net10.0`).
- **HTTP framework:** ASP.NET Core controllers.
- **Persistence:** Entity Framework Core 10 with SQLite.
- **API discovery:** ASP.NET Core OpenAPI; Swagger UI is enabled in Development.
- **Nullable reference types and implicit usings:** enabled in `api.csproj`.
- **Development HTTP URL:** `http://localhost:5288` (see `Properties/launchSettings.json`).
- **Development HTTPS URL:** `https://localhost:7044` (also listens on HTTP port 5288).

At startup, `Program.cs` registers services, creates a `CarAppContext` scope, and invokes `DbInitializer.SeedAsync`. The SQLite database file is placed at the operating system's `LocalApplicationData` path under the filename `carapp.db`. CORS currently allows the frontend origin `http://localhost:5173`.

## 3. Repository layout

```text
api/
├── Program.cs                 # Dependency injection, middleware, database startup
├── api.csproj                 # Target framework and NuGet dependencies
├── appsettings*.json          # Logging and host settings
├── Properties/
│   └── launchSettings.json    # Local launch profiles
└── src/
    ├── controllers/           # HTTP routes and request/response mapping
    ├── core/                  # Shared result, range, pagination, and sort types
    ├── data/                  # EF Core DbContext and seed data
    ├── domain/                # Application/business-facing types
    ├── dtos/                  # Input/query DTOs and conversion to domain types
    ├── managers/              # Use-case coordination and business checks
    ├── models/                # EF Core persistence entities and conversions
    └── services/              # Database-facing operations and interfaces
```

Namespaces follow the directory concepts (`controllers`, `domain`, `dtos`, `managers`, `models`, `services`, `data`, and `core`).

### Request flow

For typical controller operations, the flow is:

1. **Controller** binds HTTP route, query, and JSON body values.
2. **DTO** validates and converts API-shaped input into a domain object.
3. **Manager** coordinates a use case and performs applicable business checks.
4. **Service** queries or changes persistence data through `CarAppContext`.
5. **Persistence model** converts between the EF entity and the domain type.
6. The result flows back to the controller, which chooses an HTTP status and response shape.

The boundaries are not uniformly strict: for example, car listing returns domain objects directly and parts of the query implementation live in `CarService`.

## 4. Domain and business concepts

### 4.1 Brand

A brand has an integer database ID, a name, and an image URL. It is the manufacturer-level reference for vehicle models and cars.

### 4.2 Model

A model has an integer ID, a `BrandId`, and a name. It belongs to a brand. For example, the seed data associates Mazda with model `3`, Mercedes with `A class`, Honda with `Civic`, Ferrari with `458`, and Toyota with `Prius`.

### 4.3 Car

A car record stores:

- `Id`
- `BrandId`
- `ModelId`
- `NumberPlate`
- `Vin`
- `Weight`
- `Year`

Brand and model are represented as EF relationships. The domain car object keeps the foreign-key IDs rather than embedding the related objects. The create-car DTO additionally asks for an `Owner`, but current car creation does not persist that owner (see Ownership below).

### 4.4 Category and filter rules

A category has a name, an icon identifier, and a `CarFilter`. Filters can contain brand ID, model ID, weight range, and year range. Category filters support saved/grouped car criteria; the category itself is not automatically applied as a filter by the shown car-list route—the client sends filter values through the cars query.

The `Category` persistence entity stores the filter object in a JSON string column (`FiltersJson`) and exposes a non-mapped `Filters` property that serializes/deserializes it. Seed categories define weight bands:

- **Light:** weight up to 1300.
- **Medium:** weight from 1300 to 2400.
- **Heavy:** weight from 2400 upward.

Car filtering uses an exclusive lower bound and inclusive upper bound (`weight > From && weight <= To`, similarly for year). The seeded boundary values therefore appear in adjacent category ranges in the data, though the exact interval predicates determine membership at those boundaries.

`CategoryManager.ResolveCategory` checks weight ranges by merging ranges from existing categories with the proposed category range. It accepts the category when the merged result is one range, which means the ranges form a continuous chain under the exact-equality merge rule. This check is only applied to category create, patch, and delete flows as currently implemented.

### 4.5 Ownership (incomplete)

The EF entity `Ownership` contains `Id`, `CarId`, and `Owner`, and `CarAppContext` exposes an `Ownerships` set. However:

- There is no ownership domain type, DTO, manager, or service in the current source.
- `OwnershipsController.GetOwnerships()` returns an empty `Ok()` response and does not query data.
- `CarManager.CreateCar` accepts an owner parameter, but ignores it and only calls the car service.

Consequently, the API currently does not provide functional ownership creation or retrieval, and the owner supplied when creating a car is not saved.

## 5. API routes

Controllers use `[Route("api/[controller]")]`. With the configured lowercase URL option, the expected route prefixes are lowercase (`api/cars`, etc.). All controller responses are declared as JSON-producing.

| Method   | Route                    | Behavior                                                                                                                                                                   |
| -------- | ------------------------ | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `GET`    | `/api/cars`              | Lists cars with optional filters, pagination, and sorting. Returns `{ items, total }` using the paginated domain result.                                                   |
| `POST`   | `/api/cars`              | Creates a car from brand/model IDs, plate, VIN, weight, year, and owner. Returns `{ id }` on success or `{ error }` on failure. Owner is currently ignored by persistence. |
| `GET`    | `/api/brands`            | Returns all brands.                                                                                                                                                        |
| `POST`   | `/api/brands`            | Creates a brand from `name` and `imgUrl`; returns `{ id }` or `{ error }`.                                                                                                 |
| `GET`    | `/api/models`            | Returns all models.                                                                                                                                                        |
| `GET`    | `/api/models/{brandId?}` | Returns models, optionally filtered by brand ID. Because the route parameter is optional, the base `/api/models` route also lists all models.                              |
| `POST`   | `/api/models`            | Creates a model from `brandId` and `name`; returns `{ id }` or `{ error }`.                                                                                                |
| `GET`    | `/api/categories`        | Returns all categories and their filters.                                                                                                                                  |
| `POST`   | `/api/categories`        | Creates a category with required name and filters; icon is optional. Returns `{ id }` or `{ error }`.                                                                      |
| `PATCH`  | `/api/categories/{id}`   | Applies supplied category fields to an existing category. Returns the updated category or `{ error }`.                                                                     |
| `DELETE` | `/api/categories/{id}`   | Deletes a category. Returns `204 No Content` on success or `{ error }`.                                                                                                    |
| `GET`    | `/api/ownerships`        | Stub endpoint; currently returns an empty `200 OK`.                                                                                                                        |

### Car query parameters

`GET /api/cars` binds `GetCarDTO` from the query string. It supports:

- `Page` (integer)
- `PageSize` (integer, defaults to 10 when omitted)
- `SortBy` (currently recognized values: `brand`, `year`, `weight`)
- `SortOrder` (`Asc` or `Desc`; defaults to `Asc`)
- Nested `Filters`: `BrandId`, `ModelId`, `Weight.From`, `Weight.To`, `Year.From`, and `Year.To` (exact query-string encoding depends on ASP.NET Core model binding)

The filter predicates are combined as AND conditions. Range lower bounds are exclusive and upper bounds inclusive; absent lower/upper bounds fall back to zero/positive infinity (or max year).

## 6. Persistence model

`CarAppContext` in `src/data/db.cs` defines these sets/tables:

- `Brands`
- `Cars`
- `Categories`
- `Models`
- `Owners`
- `Ownerships`

Startup initialization in `src/data/seed.cs` ensures the database exists, runs migrations, and inserts starter data only when the Brands table is empty. It inserts five brands, one model per brand, and three weight categories. If any brand already exists, the seed method returns without checking whether models or categories are independently missing.

## 7. Core utilities

- **`core.Result<T>`:** lightweight success/failure result with `IsSuccess`, optional `Value`, and optional `Error`; managers and services use it to pass operation results to controllers.
- **`core.Paginated<T>`:** wraps a list of items and a total count.
- **`core.Range`:** nullable `From` and `To` values, used by weight/year filters and category definitions.
- **`core.Ranges`:** merges ranges only when one range's `To` exactly equals the next range's `From`; it is used by category validation.
- **`core.SortOrder`:** `Asc` and `Desc` values.
- **`RequiredWithoutAttribute`:** reusable validation attribute that requires a property when a paired property is absent. It is present in the codebase but is not currently used by the documented DTOs.

## 8. DTOs and validation

The DTOs under `src/dtos` are the HTTP-facing request models. Their `ToDomain()` methods map client input to domain objects.

- `CreateBrandDTO`: requires `Name` and `ImgUrl`.
- `CreateModelDto`: requires `BrandId` and `Name`.
- `CreateCarDTO`: includes required brand, model, owner, weight, and year fields; plate and VIN default to empty strings.
- `GetCarDTO`: maps query parameters to a `CarQuery`; page size and sort direction receive defaults.
- `CreateCategoryDTO`: requires filters and name; icon is optional.
- `UpdateCategoryDTO`: all fields are optional and converted to empty/default values for patch logic.

ASP.NET Core's `[ApiController]` attribute enables automatic model-state validation responses for invalid annotated request bodies.

## 9. Configuration and development

- `Program.cs` configures controllers, OpenAPI, lowercase URLs, CORS, scoped manager/service registrations, and SQLite.
- Swagger UI and the OpenAPI document are mapped only in Development.
- The CORS policy allows `http://localhost:5173` and any header/method. Add the deployed frontend origin when deploying the API.
- HTTPS redirection is currently commented out.

## 10. Known implementation caveats

These are observations from the current code, not assumptions about intended product behavior:

1. **Car owner is not persisted.** The create DTO and manager accept an owner, but the service only inserts a car. A complete implementation should save an `Ownership` row in the same operation (ideally transactionally) and add query/management routes.
2. **Car sorting should be verified/fixed.** `CarService` builds a dictionary of selectors, but passes the selector delegate itself to `OrderBy` / `OrderByDescending` rather than applying it to each car. Sorting may therefore not behave as intended. The `brand` selector also references a `Brand` navigation property; the query may need an explicit include or translated selector depending on EF behavior.
3. **Car pagination offset should be verified/fixed.** The current `Skip(Math.Min(0, PageSize * (Page - 1)))` expression always produces an offset less than or equal to zero. Normal positive page numbers therefore do not skip earlier rows. Validate page and page-size bounds and calculate the intended offset before relying on pagination.
4. **Read paths are synchronous.** List/count operations use synchronous EF queries despite returning from HTTP controller actions; asynchronous EF operations may be preferable under load.
5. **Errors are intentionally broad.** Several persistence methods catch all exceptions and return `0`, losing the underlying diagnostic. Logging specific exceptions would make failures easier to investigate.
6. **Category range semantics are exact.** The range merger joins only equal touching endpoints, and the car filter uses `>` for the lower bound and `<=` for the upper bound. Confirm these choices match desired boundary semantics.
7. **Initialization calls both `EnsureCreated` and `Migrate`.** EF Core applications typically choose a migration-based initialization strategy rather than mixing both. Review this for the deployment/database lifecycle intended by the project.
8. **No authentication or authorization is configured** in the inspected startup code. Do not expose write endpoints publicly without deciding on an access-control policy.

## 11. Build and run

From the `api` directory:

```sh
dotnet restore
dotnet run
```

For local development, the launch profile uses port `5288` for HTTP and `7044` for HTTPS. In Development, visit `/swagger` for Swagger UI and `/openapi/v1.json` for the generated OpenAPI document.

Build without launching the server:

```sh
dotnet build
```
