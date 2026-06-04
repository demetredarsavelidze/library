# Library Management API

ASP.NET Core REST API built from the provided PDF requirements for a clean/layered
library management system.

## Architecture

- `LibraryManagement.Domain` - entities and repository contracts.
- `LibraryManagement.Application` - DTOs, services, mapping, use-case records, and validation/business rules.
- `LibraryManagement.Infrastructure` - EF Core `LibraryDbContext`, repository implementations, JWT token service, password hashing, and seed data.
- `LibraryManagement.Api` - controllers, middleware, Swagger, authentication, and application startup.

## Features

- Books: full CRUD, pagination, filtering, and sorting.
- Authors: full CRUD, with write endpoints restricted to `admin`.
- Genres: read and create only, with create restricted to `admin`.
- JWT authentication with `admin` and `user` roles.
- DTO responses with computed fields:
  - `BookAge = current year - PublicationYear`
  - `IsThick = Pages > 100`
  - author `Age`
- Service-layer validation for required fields, author minimum age, publication year, and pagination/sort inputs.
- Error handling middleware returning validation problem details, 404s, 401s, and 500s.

## Running

```bash
dotnet restore
dotnet run --project src/LibraryManagement.Api
```

Open Swagger at `https://localhost:7041/swagger` or `http://localhost:5041/swagger`.

The app uses EF Core's in-memory provider and seeds sample authors, genres, books,
and users on startup.

Seeded credentials:

| Role | Username | Password |
| --- | --- | --- |
| admin | `admin` | `Admin123!` |
| user | `reader` | `Reader123!` |

## Main endpoints

- `POST /api/auth/register`
- `POST /api/auth/login`
- `GET /api/books?pageNumber=1&pageSize=10&minPages=100&sortBy=Title&sortDirection=asc`
- `GET /api/books/{id}`
- `POST /api/books` authenticated
- `PUT /api/books/{id}` authenticated
- `DELETE /api/books/{id}` authenticated
- `GET /api/authors`
- `POST /api/authors` admin only
- `PUT /api/authors/{id}` admin only
- `DELETE /api/authors/{id}` admin only
- `GET /api/genres`
- `POST /api/genres` admin only
