# Attribute Routing - Better Then Convention Based

**Attribute Routing** is a simpler way we can call and pass parameters to our actions through the URL

Example:

```cs
namespace Vidly
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
```

The **routes.MapMvcAttributeRoutes()** statement enables Attribute routing

Below you can see how we use it in our actions

```cs
namespace Vidly.Controllers
{
    public class MoviesController : Controller
    {
        [Route("movies/released/{year:regex(\\d{4})}/{month:regex(\\d{2}):range(1,12)}")]
        public ActionResult ByReleaseDate(int year, int month)
        {
            return Content(year + "/" + month);
        }
    }
}
```

We use regular expressions to make sure that the year is four digits and that the month is two digits between 1 and 12
