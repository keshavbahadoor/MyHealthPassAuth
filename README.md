# MyHealthPassAuth
![alt text](https://lh3.googleusercontent.com/LDXhzmtloQFfJR9MGLG7nNcvh-SOulqZdAXT2rpSMo9Urk5dl6qLnX1vQrQfy94NVbI8jXTT5xa0HdbpdmOg=w1920-h949)
Authentication and authorization library for MyHealthPass application

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes. See deployment for notes on how to deploy the project on a live system.

### Prerequisites

 * Visual Studio 2015 or Later 
 * .NET Core 
   - https://www.microsoft.com/net/download/windows
 * MySQL Instance (Can support multiple databases) 

 
### Setup

* Perform a Git clone of the repository 
* Open solution in Visual Studio (2015 or later) 
* Build the solution 
  - _Build → Build Solution_ 
  

## Usage

### Running Tests
* Open the Test Explorer 
  - _Test → Windows → Test Explorer_ 
* Select _Run All_

### Running Example Usage Project 

* Ensure that data is imported into desired database. This can be done by creating a new database in MySQL and running the included .sql script. 
* Ensure that the connection string is accurate. The library is designed to accept a DbContextOptions object that holds the connection string to supported EF Core data stores (Microsoft SQL Server, MySQL, Postgres, etc.) 
* Set the _MyHealthPassAuthExample_ project as the startup project. 
* Run example usage project. 


### Running Example Usage Project (In Memory Database) 

The in memory database usage project does not require any database connections. 
Test data is built and inserted at runtime. 
 
* Set the _MyHealthPassAuthExampleInMemoryDb_ project as the startup project. 
* Run example usage project. 
 

## Built With

* [.NET Core](https://www.microsoft.com/net/download/windows)   
* [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
 
