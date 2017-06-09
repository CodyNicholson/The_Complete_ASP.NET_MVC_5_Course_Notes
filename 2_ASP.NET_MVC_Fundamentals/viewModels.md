# View Models

We have only one model property currently, and that is: Name

We have been able to display the Name of our movie, "Shrek!", in our application - but what if we want to list the number of people who have watched that movie?

To add more model properties to our application we use a **View Model**

-

A **View Model** is a model specifically built for a view

It includes any data and rules specific to that view

For our view model we will need to add a Customer class to the Models folder so we can have some more data to work with:

```cs
namespace Vidly.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
```

Now that we have out Customer class we can create a **ViewModel** in our Models folder to hold all our our Movies and Customers:

```cs
namespace Vidly.ViewModels
{
    public class RandomMovieViewModel
    {
        public Movie Movie { get; set; }
        public List<Customer> Customers { get; set; }
    }
}
```

We can now add our Customers and our ViewModel to our MovieController as seen below:

```cs
public ActionResult Random()
{
    var movie = new Movie() { Name = "shrek!" };
    var customers = new List<Customer>
    {
        new Customer { Name = "Customer 1" },
        new Customer { Name = "Customer 2" }
    };

    var viewModel = new RandomMovieViewModel
    {
        Movie = movie,
        Customers = customers
    };

    return View(viewModel);
}
```

Now we are ready to add this content to our View:

```html
@model Vidly.ViewModels.RandomMovieViewModel

@{
    ViewBag.Title = "Random";
    Layout = "~/Views/Shared/_Layout.cshtml";
}

<h2>@Model.Movie.Name</h2>
@Model.Customers
```

Notice that we did have to change the top line to point to the RandomMovieViewModel since that is our ViewModel for our Random Movie View
