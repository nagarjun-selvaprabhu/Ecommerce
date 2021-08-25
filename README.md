# Ecommerce

Technology Stack : ASP.NET , MySql Server , Docker

MicroServices : Product , Order .

## Description

A Simple Microservice architectured ASP.NET Core WEB API Application that is resilient ,  instrumented for capturing metrics
transient/scoped fault handling and structured logging.

Contains Two Microservices Product Which includes the following endpoints GetProd , GetOneProd , UpdateProd , DeleteProd and CreateProd .
And Order makes use of these API endpoints which in-turn calls these GetProd , GetOneProd and CreateProd from OrderController.cs

## Packages Used 
* Polly for resilience which will retry the endpoint two times after inital fail
* Global transient/scoped fault handling using custom middleware in OrderEndpoint. check ExceptionMiddlewareExtensions.cs and startup.cs in ``` app.ConfigureExceptionHandler(); ```
* Prometheus for metrics
* Used NLog for logging ( both console and file )
* Caching using Microsoft.Extensions.Caching.Memory in Product.cs

### Todo 
* The right approach is to leverage the use of message bus like RabbitMQ
* So that there can be two service, One product Service and another one is Order Service
* So that Each Service can subscribe and Exchanges topic in a Queue

## Ways to Start the Application : 

* replace ConnectionStrings in  appsettings.json and start the application locally 
* or replace docker.json in launchsettings.json and do ``` docker-compose up ```


