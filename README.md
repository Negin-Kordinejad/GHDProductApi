# GHD .NET Core Tech Test
- [Description](Description)
- [Architecture and Design Pathern](Architecture and Design Pathern)
- [Logging](Logging)

## Description
This web application is a REST API microservice responsible for CRUD (Create, Read, Update, Delete) operations regarding the `product` entity.

## Architectur & Design Pathern
- WebApplication:
  - A REST API Project. injecting mediator to crud oparation.
- WebApplication.Core:
  - cqrs design pathern. 
  - request handlers using the [MediatR] library
  - pipeline behaviours [MediatR]  that perform actions before executing a request handler
  - validation logic using [FluentValidation]
  - map entities to DTOs using [AutoMapper]
- WebApplication.Infrastructure:
  - perform CRUD operations using [EF Core] and [LINQ]
- WebApplication.IntegrationTests
  - tests to satisfy the testing scenarios
## Logging
- Logs the duration it takes for any MediatR command / query handler to execute
- Logs the exceptions that are thrown by the API

## Test
- Swagger open Apii / Postman
- Integration tests
- 
## Run
 - Add-Migration InitialCreate
