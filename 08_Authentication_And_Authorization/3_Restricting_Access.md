# Restricting Access

In ASP.NET MVC we have an filter called **authorize**. A filter can be applied to an action and it will be called by the MVC framework before and after that action or its resolved are executed. When we apply this attribute to an action, before the action is executed, the attribute will check if the current user is logged in or not. If not - it will redirect the user to the login page.

We can also apply the **authorize** filter to an entire controller by putting the filter above the controllers declaration just like we would put it above an action. This is the same as applying the filter to every action in the controller individually.

We can also apply this filter to every action in every controller by registering the filter in our **~/App_Start/FilterConfig.cs** file. We do this by calling the **filters.Add()** function in the **RegisterGlobalFilters()** action found in the **FilterConfig.cs**.

```cs
using System.Web;
using System.Web.Mvc;

namespace Vidly
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new AuthorizeAttribute());
        }
    }
}
```

Now the user has to be logged in in order to see any page in our entire project

***

Let's say we want to allow anonymous users to view the homepage, but no other pages of our application. This can be done by putting the **AllowAnonymous** filter above the **HomeController.cs** file declaration like so:

```cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Vidly.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
```
