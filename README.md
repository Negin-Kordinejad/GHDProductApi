# GHD .NET Core Tech Test
- [Description](Description)
- [Architecture and Design Pathern](Architecture and Design Pathern)
- [Logging](Logging)

## Description
This web application is a REST API microservice responsible for CRUD (Create, Read, Update, Delete) operations regarding the `product` entity.

## Architectur & Design Pathern
- GHDProductApi:
  - A REST API Project. injecting mediator to crud oparation.
- GHDProductApi.Core:
  - CQRS design pathern. 
  - Request handlers using the [MediatR] library
  - Pipeline behaviours [MediatR]  that perform actions before executing a request handler
  - Validation logic using [FluentValidation]
  - Map entities to DTOs using [AutoMapper]
- GHDProductApi.Infrastructure:
  - Perform CRUD operations using [EF Core] and [LINQ]
- GHDProductApi.IntegrationTests
  - Tests to satisfy the testing scenarios
## Logging
- Logs the duration it takes for any MediatR command / query handler to execute
- Logs the exceptions that are thrown by the API

## Test
- Swagger open Api / Postman
- Integration tests

## ToDo
- Refactoring
- Add more unit tests.

## Run
 - Add-Migration InitialCreate
