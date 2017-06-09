# Building A New Form

First we will add the Action in the CustomersController to get to our form:

```cs
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
            var customer = _context.Customers.Include(c => c.MembershipType).SingleOrDefault(c => c.Id == id); //This will make our query execute immediately, and we will Eager Load the MembershipType so it is available to the application once it is built
 
             if (customer == null)
                 return HttpNotFound();
 
             return View(customer);
        }

        public ActionResult New()
        {
            return View();
        }
    }
}
```

At the bottom you will see the New() action that will take us to the form we will create

-

### Creating The Form

We can create a new view for the form that we have to name after the Action we added to the Customers class: "New.cshtml"

```cshtml
@model Vidly.Models.Customer
@{
    ViewBag.Title = "New";
    Layout = "~/Views/Shared/_Layout.cshtml";
}

<h2>New Customer</h2>

@using (@Html.BeginForm("Create", "Customers")) // The "using" block will create an ending "</form> tag for us
{
    <div class="form-group">
        @Html.LabelFor(m => m.Name)
        @Html.TextBoxFor(m => m.Name, new {@class = "form-control"})
    </div>
    <div class="form-group">
        @Html.LabelFor(m => m.Birthdate)
        @Html.TextBoxFor(m => m.Birthdate, new {@class = "form-control"})
    </div>
    <div class="checkbox">
        <label>
            @Html.CheckBoxFor(m => m.IsSubscribedToNewsletter) Subscribe to newsletter?
        </label>
    </div>
}
```

So the title of our new form will be "New Customer" and we will create the form using the @Html.BeginForm() method that will start our form tag. By putting the @Html.BeginForm() method inside a @using statement the end tag for our form will be created for us using the dispose() action in the CustomersController.

Inside the body of the form we use bootstrap to do all of the styling for us by adding in some of the bootstrap classes. You can see how we created a textbox for both Model.Name and Model.Birthdate, and a Checkbox for the boolean IsSubscribedToNewsletter variable.
