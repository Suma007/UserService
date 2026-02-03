
## User Service API

A simple **ASP.NET Core Web API** for managing users.

---

## Features

* Create a user
* Update a user
* Get a user by Id
* SQLite database with a single `Users` table.
* Vertical Slice Architecture (one service per use case)
* Centralised request validation
* Explicit controller-level error handling
* Global exception middleware for unexpected errors
* API versioning via URL segment
* Basic structured logging
* Comprehensive unit tests
* CI pipeline using GitHub Actions

---

## Tech Stack

* **.NET 8**
* **ASP.NET Core Web API**
* **Entity Framework Core**
* **SQLite**
* **Asp.Versioning**
* **Swashbuckle (Swagger)**
* **xUnit + Moq**
* **GitHub Actions**

---

## Architecture Overview

The solution follows a **Vertical Slice Architecture**, where each use case is implemented as a dedicated service.
This keeps features isolated, easy to test, and easier to change independently.

---

## Database

* **SQLite** is used for persistence.
* A single table (`Users`) stores all user data.
* The database is created automatically on startup.
* A local database file (`users.db`) is generated when the API runs.

 Database setup is intended for development and assessment purposes.

 ## User Model

 * Id : string and PK of the table. Is unique for each entry
 * Name: stores the name of the user
 * Email: stores the email of the user
 * Role: stores the role of the user. Can be 'Admin' or 'Customer'
 * UserName: stores the username of the cutomer- must be unique
 * CreatedBy: detail of who created the user
 * UpdatedBy: detail of who last updated the user
 * CreatedDate: detail of when the user was created
 * UpdatedDate: detail of when the user was last updated

---

##  API Endpoints

### Create User

```
POST /api/v1/User
```

### Get User by Id

```
GET /api/v1/User/{userId}
```

### Update User

```
PUT /api/v1/User
```

---


## Running the Application

The webapi is serving at port number 7026. Swagger UI is configured and available.

### Steps

```bash
dotnet restore
dotnet run
```

## Running Tests

```bash
dotnet test
```

---

## Continuous Integration

A GitHub Actions workflow is configured to run on each push and pull request. (This was created with the help of GitHub action creation)

The pipeline:

* Restores dependencies
* Builds the solution
* Runs all unit tests

This ensures the codebase remains stable and testable.
