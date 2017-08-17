# Convention-Based Routing

This is what our RouteConfig.cs looks like before we make additions:

```cs
namespace Vidly
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
```

-

Here is our RouteConfig.cs after we make our additions:

```cs
namespace Vidly
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "MoviesByReleaseDate",
                "movies/released/{year}/{month}",
                new { controller = "Movies", action = "ByReleaseDate"},
                new { year = @"2015|2016", month = @"\d{2}" });
                // The above line makes it so the year must be 2015 or 2016, and the month must be a 2 digit number);

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
```

Notice that we added a route.MapRoute statement *before* our original route.MapRoute statement

This is because **the order matters** - the framework will first check for /year/month and if it cannot find those parameters in the URL then it will do the default routes.MapRoute statement and handle the url as {controller}/{action}/{id} instead of {controller}/{action}/{year}/{month}
