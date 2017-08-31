# Logging Unhandled Exceptions

To log unhandled exceptions we are going to use another very popular package called **Elmah**. In package manager console run: **install-package Elmah**. **Elmah** stands for **Error Logging Modules and Handlers**. The way it works is that is plugs into the request processing pipeline and if there is an unhandled exception it will grab its details and store it. By default it stores these details into memory but with a small configuration you can have it store exceptions in different kinds of databases. 

If we build the application and go to home/about we should get the exception we hardcoded in earlier for testing. If we go to /elmah.axd we see the exceptions Elmah has captured. If we click on the **Details...** link we will see a page that simulates a yellow screen with exception messages as well as the stack trace. In this case we can see that this exception happened in the about action in the home controller on line 19. If we scroll down we can see the server variables at the time the exception happened. All this data helps us troubleshoot the problem. The elmah endpoint is only available locally. If we want to access this remotely, then you need to do a bit of configuration. If we go to **Web.config** we can go to the location element with path="elmah.axd".

As you can see, Elmah registers a handler for this path. When we hit that endpoint the **ErrorLogPageFactory** is executed. This class is essentially an http handler and it is similar to an action in MVC. Unlike an action, it is not part of a framework. It is a class with one method that gets the request and returns a response and it's a very low level way of processing requests. We have a section called authorization in our location element. We can take it out of the comment and make it a child of "location/system.web". With this code we are authorizing anyone who is in the admin role to access this endpoint. We are denying all other users. We can change roles to users and make the value a specific user. When we save **Web.config** our web application is restarted. Since elmah stores exception details by default in memory, our log will be cleared.

```
  </elmah><location path="elmah.axd" inheritInChildApplications="false">
    <system.web>
      <httpHandlers>
        <add verb="POST,GET,HEAD" path="elmah.axd" type="Elmah.ErrorLogPageFactory, Elmah" />
      </httpHandlers>
      <authorization>
        <allow users="admin@vidly.com,otherUser@vidly.com" />
        <deny users="*" />
      </authorization>
```
