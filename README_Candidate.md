# Project defined definitions

## Workers

- Each android worker is identified by a letter of the alphabet (A, B, etc).
    - Enabled whole string names, can be narrowed to alphabet chars in the endpoint or command validations.
    - OK

- Workers' activities have a start and end time.
    - OK

- Build a component always performed by one worker.
    - OK

- Build  a machine: performed by one or several workers together.
    - OK

- workers can do activities during full day, all days of the year.
    - OK
-  each time a worker finishes an activity needs to rest
    - 2 hours after a component.
    - 4 hours after a machine.
    - - OK

## BDD
### Me as the application user:
- Want to schedule activities in any time (future or past).
    - OK

- When scheduling an activity, want to indicate the start and end times.
    - OK
- Want to indicate the type of activity and the worker or list of workers.
    - OK
- Want to be able to delete activities.
    - OK
### The Application Should:
- indicate when a worker's activity conflicts with other activities of the same worker because their times overlap.
    - OK
- Indicate when an activity is scheduled between the rest time of the workers.
    - OK
- Show a list of the top 10 androids that are working more time in the next 7 days.
    - OK

## Application Implementation

The application was developed using the concepts of domain driven design (DDD) alongside Test Driven Development (TDD), the TDD approach enables writing tests with business requirements in form of code so satisfying these tests means that the businesse requirements will be fullfiled.

The entities and aggregates were designed to contain all the domain-related logic that is in the requirements such as detecting overlapping activities, enabling assigning and removing activities from workers, checking if a worker can work in an activity, et cetera, so consuming the Assembly TechTest.Ryanair.Tycoon.Domain enables to build other applications using the business concepts, thats a good example of domain driven design.

Applying Null Object pattern and Result Pattern to diminish the null handlings during the operation helps diminishing the probability of a null reference exception and exception throwing in general.

The result pattern (Borrowed from functional programming and present in the library Awarean.Sdk.Result developed/maintained by me) is applied in operations to indicate success or failure, whenever a failure happens, we can handle it inside on of the application layers to present a friendly response to the user, or return it directly to indicate that a business constraint was violated instead of throwing exceptions which is costly, helping improve performance and informing errors in an idiomatic way, in case of success, we can leverage the generic nature of the lib to return the object containing relevant data of the operation, the library also enforces using null object pattern, so every object that can be returned using "Result.Fail<T>(T value)" should implement it, helping prevents null handling and possible null reference exceptions.

The application layer was developed using principles from Use Case Driven development, the use cases reflect and should express what the application should be capable of, this way splitting the application in use case for workers and for activities, it was easy to build a solid foundation for the API, the application is responsible for validate that commands are valid, such as checking if the command is null, if the constraints to try executing an operation is valid and returning results that express the validation error. It only depends on abstractions that enables the use case to be executed, for example, when the AssignExistentActivityUseCase needs to get an activity for assigning to works, it does not need to duplicate the logic for getting an activity by id, it only needs to depend on the IGetActivityByIdUseCase to do its job of retrieving an activity, there's other examples inside the use cases.

The Api layer is the most simple, it only exposes the endpoints for receiving requests and translates them to Use Case Commands, it is only responsible to pass data to the Use Cases, the requests know its use cases and know how to turn themselves into the use case command, the command and the application layer does the validation, enabling to concentrate the invariant constrainst to be placed only in one place.

The Ìnfrastructure is composed of in memory repositories that runs on a Dictionary<Guid, T> for each entity/aggregate abstracted upon a layer of repositories and unit of work exposed by the domain, this enables easily switching from an in memory data persistence to an actual database such as SQL Server, PostgreSql, MongoDb or DynamoDB or an Web Request to another service (in a microsservice based environment, message queue), For the application I Decided to focus more on TDD and DDD concepts, so I decided to use a basic and functional approach to the Infrastructure.

## Improvements
- Implement actual data persistence (Relational with entity framework core/Dapper or Non Relational like DynamoDB and AWSSDK.DynamoDBV2).

- Leverage the generic nature of the use cases to implement Mediator Pattern with Mediatr, decoupling API layer even more from the actual application.

- Use Fluent Validation to decrease verbose and repetitive validations in Application Commands.

- Adds More testing for extreme edge cases.

- Run Sonarqube Scan

- Add a pipeline to run tests.