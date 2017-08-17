# Action Parameters

**Action Parameters** are the input to actions - just like Action Results are the output

When a request comes in an application, ASP.NET MVC automatically maps request data to parameter values for action methods

If an action method takes a parameter the MVCframework looks for a parameter with the same name in the request data

If a parameter with that name exists - the framework will automatically pass the value of the parameter to the target action

This parameter value (In this example: **1**):

- Can be embedded in the URL: /movies/edit/**1**

- Can be in the query string: /movies/edit?id=**1**

- Can be in the form data: id=**1**

***

```cs
namespace Vidly.Controllers
{
    public class MoviesController : Controller
    {
        public ActionResult Edit(int id)
        {
            return Content("id="+id);
        }
    }
}
```

If I run the application and change the URL to be: /Movies/Edit/123

Then I will get a content page with the text: id=123

Since it will grab the id from the URL

Alternatively, I could use the query string: /Movies/Edit?id=123

This will produce the same result as the URL embedded example without having to change any of the application code

***

We *should* use "id" as opposed to another parameter name because {controller}/{action}/{id} is defined in our routeConfig

If we use another parameter name like "movieID" for our Edit action it will work, but if we type movies/edit/1 into the URL bar we will get an error because 1 represents an id, but Edit takes a movieID as a parameter NOT a regular id

***

```cs
public ActionResult Index(int? pageIndex, string sortBy)
{
    if (!pageIndex.HasValue)
        pageIndex = 1;
    if (String.IsNullOrWhiteSpace(sortBy))
        sortBy = "Name";
    return Content(String.Format("pageIndex={0}&sortBy={1}", pageIndex, sortBy));
}
```

Using the above action, if we type /Movies into the URL we will get a content page that will display: pageIndex=1&sortBy=Name

This is because our if statements were true since we did not add any values into pageIndex and sortBy, so it set pageIndex to 1 and sortBy to Name and returned the content page displaying this information
