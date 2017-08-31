# Custom Error Pages

In this demonstration, it the home controller's **About()** action we are going to manually throw an exception to simulate a situation where we have an unhandled exception.

```
        public ActionResult About()
        {
            throw new Exception();

            ViewBag.Message = "Your application description page.";

            return View();
        }
```

Now if we build the project and go to /home/about we will get an ugly error message page with details about what went wrong that our client will almost certainly not understand. This also adds a security risk because a hacker can try various inputs in our application to get an idea of how we have written the code. They can also find the version of .NET you are using. Then they can easily do a google search to find vulnerabilities in our architecture.

To solve this issue, in **Web.config** under the system.web section we can add a new element called **customErrors** and we add an attribute **mode** and set its value to on:

```
  <system.web>
    <customErrors mode="On"></customErrors>
```

With this we will always display friendly or custom error messages instead of exception details. You can also set **mode** to RemoteOnly to make custom or friendly error messages display only to machines that are connected remotely. If you run it locally, then you will see the more detailed and ugly error messages.

If we go back to our application and try to go to /home/about we will see a nicer error page which hides the exception details. This comes from **~/Views/Shared/Error.cshtml**. We can customize this page anyway we want by editing this file. Who is serving this view? We don't have a controller for that. That is where one of our global action filters comes into the picture in **FilterConfig.cs**. In this file we have a filter called **HandleErrorAttribute**. This filter is applied to all controllers and all actions. Action filters can be executed before and/or after an action. This filter is executed after an action, and if there is an unhandled exception in that action it will catch it and then render the custom view. This is how we display custom error pages for internal server errors which happen when there is an exception in our application.

However, there is another kind of HTTP error that we need to handle explicitly. This is the 404 errors which indicate that a given resource is not found on the server. Currently it takes us to another ugly page with little styling and confusing information for a client to understand. This page is coming from ASP.NET framework. Now lets try using the **Edit()** action in the customers controller to try to find a customer that doesn't exist. We go to "/customers/edit/23412341". This 404 error page is different from the first. That is because this one comes from IIS. When the status code of the response is set to 404 IIS will generate this error page.

Now we can take a look at a third example. If we want to access a static resource like "/image.gif", again we see the same error page as when we looked for an invalid customer. The customer error page is only rendered when there is an exception in our application. So the HandleErrorAttribute catches the exception and renders the view. But when we access a static resource, there is no exception. In fact, these request didn't even make it to ASP.NET framework. It hits IIS first and IIS can tell from the url pattern that this is a static resource, so it will not hand it off to ASP.NET. The same thing happens when we return **HttpNotFound()** like we do in the **Edit()** action of our customers controller when our customerId is invalid. **HttpNotFound()** just sets the status code to 404, and the rest is handled by IIS. **HandleErrorAttribute** does not work in this case. For this kind of error we need more configuration. Back in **Web.config**, inside custom errors we can add a child element called **error** with **statusCode="404"** and a **redirect="~/404.html**. The ~ character represents the root of the site. With this we are telling MVC framework that is it encounters a 404 error, we should redirect to this 404.html page.

```
  <system.web>
    <customErrors mode="On">
      <error statusCode="404" redirect="~/404.html" />
    </customErrors>
    <sessionState mode="Off"></sessionState>
```

We didn't redirect to an action, and instead redirected to a static page because if another error happens in our MVC application, it gets complicated. This error might mess with the our controller and then the action can't be called. Going directly to a static file means we don't need to worry about the controller or any dependencies. This also makes deployment easier because you can deploy the 404.html file individually since it doesn't need to be compiled. We can create our 404.html file in the root of our project:

```
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <title></title>
</head>
<body>
    <h1>Not Found</h1>
</body>
</html>
```

Now whenever we go to an action that does not exist we will redirect to this page. If we try to go to a static resource like /image.gif then this request will not make it to ASP.NET runtime. So it is served by IIS directly, and results in an ugly IIS error page still.

To change the IIS error page we need to supply it with more configuration. Back in **Web.config** we can scroll down, and inside the system.webServer we can add configuration for IIS. We will add an httpError element to system.webServer and set the attribute **errorMode="Custom"**, meaning we want to display custom error pages at all times, both for local and remote clients. If we want this to display these error pages to remote clients only we can set this to **DetailedLocalOnly**. We then add a remove element to httpErrors to remove the standard error page with the attribute **statusCode="404"**. Then we add an error element to httpErrors with **statusCode="404"**, **path="404.html"**, and **responseMode="File"**. The response mode is very important because if we use any of the other settings then this status code will be changed from 404 to 200. The clients won't get the 404 error, and this is bad for SEO. Now we get our custom error page for static resources.

```
  <system.webServer>
    <httpErrors errorMode="Custom">
      <remove statusCode="404" />
      <error statusCode="404" path="404.html" responseMode="File" />
    </httpErrors>
```
