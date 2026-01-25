# SMALL TECH DEMO API

This repo is a small tech demo API. It is a simple CRUD api with 1 DB table Users. 

The assignment is created in c# using .net core 8. It supports logging to a daily rolling log file created through middleware. It also supports client authentication with a api key (guid) in X-API-Key header   

## LIBRARIES USED.

### SWAGGER 
Added for easy api testing.

### SqlLite
I was deciding between Sql server and Sqlite. Sql server is my go-to DB but being that this is a test assignment Sqlite seemd like a better idea because of easier depoloyment of the person testing my application and it seemed like a interesting tech to try. For real production CRUD app it is not a real candidate because of only supporting one conncurent write operation which locks the DB. the db is located in /data and the test db is included in the codebase.

### Dapper 
I was deciding between EF and dapper. Each has its own advantages, here i chose dapper because it is simpler and i wanted to refresh my knowledge of it.

### BCrypt
Used for hashing and verifying user passwords. It generates a unique salt for the passwords so it saves me from saving the salts in User table in DB.

### XUnit
Used for writing unit tests.

### Moq
Used for creating mocks in unit tests.

### DOCKER
This was added for easier deployment of the tested.


## Project organisation
the codebase is split into 4 different projects. the organisation of the project is Api->Core<-Data and Test.

Api is the presentation layer, containing the endpoints.
Core is the application layer, containing the business logic
Data takes care of the Infrastructure part, being just the DB in this case.
Test is the test project containing unit test.

## DEPLOYMENT
To test the application you have 2 options.

The first one is to download the codebase, restore nu-get packages and run the Api project. The database will already be set up.

The second one is downloading the DOCKER file from https://drive.google.com/file/d/1M--zKXX2f35LtOXDoPrCs4ukzN3Fa1Ui/view.
All you have to do is mount the immage (tar file), run a container with it and expose the 8080 port to a port of your choosing.

Mount the immage with: **docker load -i MicrocopApi.tar**

To run the container run: **docker run -d --name api-container -p [custom port]:8080 api:latest**

After running the codebase or running the docker image the swagger ui that u can use to test the application is availiable at localhost:[custom ip]/swagger/index.html.
For authorization you can use the API key [696ec393-f2c8-8332-ba2a-f7254f58ba4a]. In swagger you use it by clicking the green button Authorize in the upper right corner. Inputing the Api key and clicking authorize.

## POSSIBLE IMPROVEMENTS
The docker container should be made in a way where the docker container is pointing to a /data and /logs folder in the host file system. so the database and logs do not get destroyed with the container and are permanent. I was not able to do this because of windows host7ubuntu docker security problems.

Testing
This is my weak point, i have not done much much with auto testing the applications before. This was the biggest learning experinece on the biggest unknow of the assignment. I hope the unit test i wrote for the application are going in the correct direction. 

It would also be good to write some integration tests that would create a mock Sqlite database and test the application from api call to the database.



