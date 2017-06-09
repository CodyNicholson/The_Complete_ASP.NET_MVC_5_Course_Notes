# Eager Loading

We can include data from our Customer class on our Index.cshtml file by using the below code:

```cshtml
@model IEnumerable<Vidly.Models.Customer>
@{
    ViewBag.Title = "Customers";
    Layout = "~/Views/Shared/_Layout.cshtml";
}
<h2>Customers</h2>
@if (!Model.Any())
{
    <p>We don't have any customers yet.</p>
}
else
{
    <table class="table table-bordered table-hover">
        <thead>
            <tr>
                <th>Customer</th>
                <th>Discount Rate</th>
            </tr>
        </thead>
        <tbody>
            @foreach (var customer in Model)
            {
                <tr>
                    <td>@Html.ActionLink(customer.Name, "Details", "Customers", new {id = customer.Id}, null)</td>
                    <td>@customer.MembershipType.DiscountRate%</td> <!-- This must be Eager Loaded in the Customers Controller -->
                </tr>
            }
        </tbody>
    </table>
}
```

However, if we try and run this it will complain that it does not known anything about the "@customer.MembershipType.DiscountRate". This is because by default Entity Framework will only load the Customer objects, and not their related objects. So "MembershipType" is null. To solve this problem we need to load the customers and their membership types together using **Eager Loading**.

```cs
using System.Data.Entity;

namespace Vidly.Controllers
{
    public class CustomersController : Controller
    {
        private ApplicationDbContext _context;

        public CustomersController()
        {
            _context = new ApplicationDbContext(); // This is a disposable object, so we need to properly dispose of it
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: Customers
        public ActionResult Index()
        {
            var customers = _context.Customers.Include(c => c.MembershipType).ToList(); //When this is called Entity Framework will not query the database - this is called deferred execution

            return View(customers);
        }

        public ActionResult Details(int id)
        {
            var customer = _context.Customers.SingleOrDefault(c => c.Id == id); //This will make our query execute immediately, and we will Eager Load the MembershipType so it is available to the application once it is built
 
             if (customer == null)
                 return HttpNotFound();
 
             return View(customer);
         }
    }
}
```

Notice the import statement "using System.Data.Entity;" and the Include() method called when creating the customers variable: "_context.Customers.Include(c => c.MembershipType).ToList();". By calling the Include() method and passing through the "c => c.Id == id" we tell the program to load MembershipType at the same time we load the Customers.
