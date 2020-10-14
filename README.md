

## Technical Details

### Architecture
I've used `Clean Architecture`, `DDD`, `Event Sourcing`, `CQRS`  while I was working on this project. This means that you will see `UseCase` in the solution and everything was grouped and placed under its own use case folder. It's really easy to navigate. I've also used `Martendb`, a .NET Transactional Document DB and Event Store on `PostgreSQL`. You will also see tests such as `UnitTests`, `IntegrationTests` and `AcceptanceTests`. 


### Folder Structure
 Folder structure consists of four separated files, first is `src` which has source files the other one is `tests` which has tests projects. There is also `cicd` and `docker` which has `docker` files and files for `Continuous Integration` respectively.


### Libraries
You can find what libraries I've used the following;

- XUnit
- FluentAssertion
- Xbehave
- Polly
- Serilog
- Swagger
- MartenDB
- Fluent Validation

 ## Build & Run
 To Ensure you have a postgres database running  (ie if you dont have one runnning on your local system already) before starting the applicaiton, navigate to `docker` folder and  run `docker-compose up -d --build`.

 To run the project, navigate to `src/ProjectTemplate.Api` folder and run `dotnet run`.

 
You can use `Swagger` using the following to explore the api.

- [Go to Swagger](https://localhost:5001/swagger/index.html)

You will find the api details in `Swagger` documentation and test it.
