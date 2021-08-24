# AspCoreRestApiDesign

Contains Three Microservice - AspCoreRestApiDesign , OrderService and ProductService

### Technology Stack 

ASP.Net Web API , InMemoryDatabase , JWT , RabbitMQ

### Steps to start the application

  - After importing the proj to visual code. need to start RabbitMQ Server
  - Can easily start a RabbitMQ Server using this Docker command
  - docker run -it --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3.9-management
  - use this URI to view the dashboard of RabbitMQ http://localhost:15672/#/
  
## Review

* Coding Standard has been maintained throughout the repo
* But Logging has not been implemnted throughout the repo
* RabbitMQ pub/sub is a really good implementation making it a loosely coupled Microservice Architechture
* Three Middleware layers has been implemented logging , timeout and Auth.
* No Global Exception Handling has been implemented
* AccountController doesn't have any Try/Catch statement which makes it very vulnerable
* No Metrics Implementation is found 
* Not a resilient Application

### Description 

* A Shopping site, consists of only three main end-points - Login , Product and Order
* Login focuses mainly on Authorization 
* Product focuses on CRUD of the Model Products.
* Order also focuses on CRUD of the Model Orders.

Each service in detail.

### AccountController.cs

* using any userName/passWord we can login which will generate a JWT Token 
* We need to use that token in Swagger Auth example Bearer << Token >>
* This will Authorize all the end points as " ADMIN "
* API Versioning as been implemented for AccountController so giving diff version will give diff responce

### Product

* Contains GetProducts , GetProdById , AddProd Endpoints
* All the core logic resides in ProductServices.cs in AspCoreRestApiDesign
* For EveryEndpoint CRUD operations are done in Db and then a productQueue is created and is Publised
*  In WorkerService these productQueue is Subcribed 

### Order

* Contains GetOrder , GetOrderById , AddOrder Endpoints
* All the core logic resides in OrderServices.cs in AspCoreRestApiDesign
* For EveryEndpoint CRUD operations are done in Db and then a OrderQueue is created and is Publised
* In ItemOrderWorkerService these OrderQueue is Subcribed 

This is how the Product and Order is Executed.

There is another Queue emailQueue which is not fully integrated in AspCoreRestApiDesign 
but the Subcribe email_exchange is done in SendEmailWorkerService





