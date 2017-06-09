# Creating An ASP.NET MVC 5 Project

In Visual Studio: File -> New -> Project -> Templates -> Visual C# -> Web -> ASP.NET Web Application, Visual C# (Select if you want to add to Git or not)

In the list of templates select MVC, do not deploy to Azure Cloud

***

### What Does The Template Include?

In **App_Data** our database files will be stored

-

In **App_Start** there are a few classes that are called when the application starts like RouteConfig, which is where our routing rules are defined

In the **RouteConfig.cs** you will see under routes.MapRoute:

url: "{controller}/{action}/{id}"

which is the format of our http requests: "http://vidly.com/movies/popular"

**movies** calls the movieController, and **popular** calls the action/method in the movieController called popular

"http://vidly.com/movies/edit/1" - Calls the Edit(int id) method of the movieController with the argument: 1

Under routes.MapRoute you will also see:

defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }

Which says that if **only** the controller "movies" is provided in the http request, just call the action "Index"

-

In the **Content** folder we store out CSS files, images, and other client side assets

-

In the **Controllers** folder we store all of our controller .cs files, you will see that three have already been created for us as a part of the template

The **AccountController.cs** has actions/methods for signing in and out,

The **HomeController.cs** which as all the actions relating to the home page

The **ManageController.cs** which provides a number of actions for handling requests for a users profile, like changing a password, enabling two-factor authentication, using social logins like facebook, etc

-

We have the **fonts** folder which should probably be included under content since it is a client side asset

-

We have the **Model** folder where all of our domain classes will be stored

-

Next, we have the **Scripts** folder where all of our javascript files will be stored

-

Finally, we have the **Views** folder

Inside the views folder we have a folder named after each of the controllers in our application

When we use a view in a controller ASP.NET will look for that view in a folder with the same name as that controller

In views we also have a folder called Shared which includes the views that can be used across different controllers

In the views folder we also have a few files like favicon.ico that gives our application an icon, the Global.asax file which is a class that provides hooks for various events in the applications lifecycle (like setting up the routes), the packages.config file is used by our Nuget package manager, our Startup.cs is intended to do everything that the Global.asax file does but it is not ready yet, and last we have the Web.config file that includes the configuration for our application (connection Strings, appSettings, etc)
