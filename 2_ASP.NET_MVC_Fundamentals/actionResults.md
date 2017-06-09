# Action Results

In MovieController.cs the method we created returns type **ActionResult**

**ActionResult** is the base class for all action results in ASP.NET MVC

Depending on what an action does, it will return an instance of one of the classes that derive from ActionResult

Below you can see the different actionResults being used within the MovieController, I also included the Movie class and the Random.cshtml view

```cs
namespace Vidly.Controllers
{
    public class MoviesController : Controller
    {
        // GET: Movies/Random
        public ActionResult Random()
        {
            var movie = new Movie() { Name = "Shrek!" };
            return View(movie);
            //return Content("Hello"); // Sends plain string content to the application
            //return HttpNotFound(); // Sends an http not found error to application
            //return new EmptyResult(); // Sends a blank webpage to application
            //return RedirectToAction("Index", "Home", new { page = 1, sortBy = "name" }); // redirects the user to the home page and sends the page and sortBy variables to the url
        }
    }
}
```

```cs
namespace Vidly.Models
{
    public class Movie
    {
        // This is a plain CLR object/POKO which represents the state and behavior of our application in terms of its problem domain
        public int Id { get; set; }
        public string Name { get; set; }

    }
}
```

```html
@model Vidly.Models.Movie

@{
    ViewBag.Title = "Random";
    Layout = "~/Views/Shared/_Layout.cshtml";
}

<h2>@Model.Name</h2>
```
