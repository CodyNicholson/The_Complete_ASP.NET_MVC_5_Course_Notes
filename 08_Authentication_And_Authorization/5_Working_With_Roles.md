# Working With Roles

We want to restrict the add, edit, and delete operations to store managers. We have two options to do so. One way is to show or hide various elements depending on the user's privileges. This works well if you have a simple view. If you have a complex view then you will end up with a lot if and else blocks and this will make your view hard to maintain. In that case, it's better to create an entirely new view for users with less privileges.

For the movies view we need to hide the "New Movie" button if the user cannot add new movies. Also, in our table we need to replace this link that takes us to the edit page with another one that takes us to the details page. We should also hide the delete column since normal users cannot delete. Since there is so much to change, we will create an entirely new view for users who do not have permission to add and delete.

We will create a copy of the **Index.cshtml** file at this location with this name: **~/Views/Movies/ReadOnlyList.cshtml**. We will also rename the **Index.cshtml** file to be **List.cshtml** for consistency.

In the **ReadOnlyList.cshtml** file we can delete the action link that renders the New button since we don't want them to create new customers. In our table, we will delete the 'delete' column since they should not have delete access. We should also remove the code that handles the click event of the delete button. Here is what we will have left:

```cs
@model IEnumerable<Vidly.Models.Movie>

@{
    ViewBag.Title = "Movies";
    Layout = "~/Views/Shared/_Layout.cshtml";
}

<h2>Movies</h2>

<table id="movies" class="table table-bordered table-hover">
    <thead>
    <tr>
        <th>Movie</th>
        <th>Genre</th>
    </tr>
    </thead>
    <tbody>
    </tbody>
</table>

@section scripts
{
    <script>
        $(document).ready(function () {
            var table = $("#movies").DataTable({
                ajax: {
                    url: "/api/movies",
                    dataSrc: ""
                },
                columns: [
                    {
                        data: "name"
                    },
                    {
                        data: "genre.name"
                    }
                ]
            });
        });
    </script>
}
```

***

Now we will go to the **~/Controllers/MoviesController.cs**. In the **Index()** action we will add an if statement for control whether or not we go to the **List.cshtml** or the **ReadOnlyList.cshtml**.

```cs
        public ViewResult Index()
        {
            if (User.IsInRole("CanManageMovies"))
                return View("List");

            return View("ReadOnlyList");
        }
```

Now the admin will go to the **List.cshtml** and the guest will be taken to the **ReadOnlyList.cshtml**

***

In the MoviesController we need to declare actions like the **New()** action with the **Authorize** filter with the role property set to "CanManageMovies".

```cs
        [Authorize(Roles = "CanManageMovies")]
        public ActionResult New()
```

In order to get rid of this magic string "CanManageMovies", we can create a new **RoleName.cs** to hold all of our strings in an organized class:

```cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vidly.Models
{
    public static class RoleName
    {
        public const string CanManageMovies = "CanManageMovies";
    }
}
```

Now we can change our filter:

```cs
        [Authorize(Roles = RoleName.CanManageMovies)]
        public ActionResult New()
```

```cs
        public ViewResult Index()
        {
            if (User.IsInRole(RoleName.CanManageMovies))
                return View("List");

            return View("ReadOnlyList");
        }
```
