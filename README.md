AngularRepository
===========

This application shows a list of links about AngularJS which is retrieved from Jeff cunnegan's repository on Github, you can rate any link based on the 
quality of the content once you have checked them out (ideally), so other users can get an idea about if the links are worth to read or not.

The project consists of a .NET Web API connecting to a SQL Server Database and making Web Scraping from a remote site, an AngularJS/.Net Front-End project 
as a Single Page Application which consumes the API and render the retrieved data.

The main thing here is dragging the links out from the Github repository and stored them in the SQL Database, basically I used a web scraping technique in c# to 
download the html content from a determined url and then transform selected nodes (using regex) in a XML document to finally convert that into a JSON formated 
string before returning the value.

Making use of Angular $http service I request the links from the API and display them crafting the site design with some Bootstrap styles included. In an 
asynchronous way each links is identified if it is stored in the dabatabase already so we can get the rate value and display it to the user respectively. Any
request to the database is made using the API, like getting categories, rating, consulting if a link exists in the database etc.

UI

I used X bootstrap template, applied some filters leveraging the power of AngularJS filters to change colors among links boxes rendered dynamically, some jQuery
animation function to fade out and in smoothly in clicking buttons actions when rating,

Database, API and AngularJs project serving a SPA.

A simple SQL Server Database stored our links and categories, in the API I use Entity Framework as ORM to modeling tables and quering data, it makes it more
easy and I feel very comfortable working with this technology. The API project is structured in several folders being one of the most important ones the 
Repository Folder, which has inside it the API controller files that expose the endpoint to request from clientes, and the app folder that represent essentially 
the Front-End side with all the AngularJS files.

Here we have a prototype schema of the project.

**********************************************
**********************************************
----------------------------------------------
**********************************************
**********************************************

This project describe the use of

+ .NET Web API
+ .NET MVC
+ C# Interfaces
+ DTO classes
+ Constraint type Classes
+ Regular Expressions
+ Entity Framework
+ SQL Databases
+ AngularJS
+ Promises
+ jQuery
+ Http Requests/responses
+ Json Manipulating objects
+ CSS
+ CSS media queries
+ Bootstrap
+ Web Scraping
