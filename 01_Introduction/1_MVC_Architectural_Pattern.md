# MVC Architectural Pattern

**MVC** stands for Model, View, Controller and it's an architectural pattern for implementing user interfaces

***

### Model

**Model** represents the application data and behavior in terms of problem domain, and independent of UI

In the Video Rental Application for this class our model will consist of classes like Movie, Customer, Rental, Transaction, etc.

These classes have properties and methods that purely represent the application state and rules - they are not tied to the user interface which means that you can take these classes and use them in a different kind of app, like a desktop of mobile app

They are plain old CLR objects, or POCO

-

### View

The **View** is the HTML markup that we display to the user

-

### Controller

The **Controller** is responsible for handling and HTTP Requests

For example, if we send a request to our app at "vidly.com" our request will look like: "http://vidly.com/movies", then our controller will get all the movies from the database, put them in a view, and return the view to the client of the browser

Methods of a controller are called **actions**

-

### Router

Although it is not represented in the acronym, the **Router** selects the right controller to handle a request based on some rules you define

In our example: "http://vidly.com/movies", the router would be responsible for referring this request to the movieController
