# Passing Data To Views

Currently, the way we have been sending data to our views is by passing our model to the view method as seen below

```cs
public ActionResult Random()
        {
            var movie = new Movie() { Name = "Shrek!" };
            return View(movie);
        }
```

There are two other ways to pass data to views, but passing through the View() method is better

***

### Passing Data To Views Using The View Data Dictionary

```cs
public ActionResult RandomViewBag()
{
    // This is just to show you there are other way to send data to the view
    // DO NOT USE VIEW DATA OR VIEW BAG, just pass data through the View()
    var movie = new Movie() { Name = "Shrek!" };

    ViewData["RandomMovie"] = movie;

    return View();
}
```

```cs
@model Vidly.Models.Movie

@{
    ViewBag.Title = "Random";
    Layout = "~/Views/Shared/_Layout.cshtml";
}

<h2>@(((Movie) ViewData["Movie"]).Name)</h2>

```

-

### Passing Data To Views Using ViewBag

```cs
public ActionResult RandomViewBag()
{
    // This is just to show you there are other way to send data to the view
    // DO NOT USE VIEW DATA OR VIEW BAG, just pass data through the View()
    var movie = new Movie() { Name = "Shrek!" };

    ViewBag.Movie = movie;

    return View();
}
```

```cs
@model Vidly.Models.Movie

@{
    ViewBag.Title = "Random";
    Layout = "~/Views/Shared/_Layout.cshtml";
}

<h2>@ViewBag.RandomMovie</h2>

```
