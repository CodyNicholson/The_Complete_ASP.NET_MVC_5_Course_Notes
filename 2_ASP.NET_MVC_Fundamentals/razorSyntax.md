# Razor Syntax

By using **Razor Syntax** we can mix our HTML code with C# code

```cshtml
@model Vidly.ViewModels.RandomMovieViewModel

@{
    ViewBag.Title = "Random";
    Layout = "~/Views/Shared/_Layout.cshtml";
}

@*
    This is how you make a
    multiple line comment
*@

@{
    var className = Model.Customers.Count > 1 ? "popular" : null;
}

<h2 class="@className">@Model.Movie.Name</h2>

@if (Model.Customers.Count == 0)
{
    <text>No one has rented this movie before.</text>
}
else
{
    <ul>
        @foreach (var customer in Model.Customers)
        {
            <li>@customer.Name</li>
        }
    </ul>
}
```

As you can see in the above code, we have C# working inside of HTML

All C# statements have an '@' before them

The className variable we declare is initialized based on the amount of Customers we have. If we have more than one Customer (We have two) then className is set equal to "popular". This is important because this variable is then used in our h2 tag as a class name for our movie. This allows us to manipulate the look of the text based on the amount of Customers we have.

Below that you can see that we check if Model.Customers.Count is 0, and if it is we display a message. Otherwise - if we do have Customers - then we display there names in a list

Be using Razor Syntax to integrate C# code with HTML code we can make our pages more dynamic
