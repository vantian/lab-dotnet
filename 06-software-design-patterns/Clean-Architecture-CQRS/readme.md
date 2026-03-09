## System Architecture

This API using Clean Architecture + CQRS. The reason I picked this architecture is because I concern the repository on dll package. Which mean I cannot control that repository and I will treated it as external system. In the future we able to replace this repository in the `BizCover.Infrastructure` without touching the business logic of our system.

In this app, I have four projects with following details:

- `BizCover.Api.Cars` = Presentation
- `BizCover.Application` = Business Logic
- `BizCover.Domain` = Entities
- `BizCover.Infrastructure` = Adapters / Persistence
  (in the future if this app have a DB connection, then it should make new project called `BizCover.Persistence` and put `DBContext` and `Migration` there. )

Packages:

- Mediator (CQRS architecture)
- FLuent Validation (Validate request before run business logic)
- Serilog (Logger)
- Microsoft.Extensions.Caching.Memory (Cache)
- NSubstitute (Mocking IMediator in unit testing)

Key Components Overview
`BizCover.Api.Cars/HttpTest/CarsAPI.http` Serves as the HTTP entry point for testing the Cars API
`GlobalExceptionMiddleware` Middleware that intercepts FluentValidation errors or uncaught exceptions, ensuring they are handled properly
`Microsoft.Extensions.Caching.Memory` - Implements in-memory caching for the GetAll results and keep it for 15 seconds to improve the performance
`Result<T>` Provides a standardized response wrapper for all API outputs, the API will response based on StatusCode from this class
`BizCoverExternalCarAdapter` Adapts the BizCover.Repository.Cars into the application layer
