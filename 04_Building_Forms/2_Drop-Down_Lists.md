# Drop-Down Menus

We want to add a drop down menu to our New Customer form so that you can select the type of Membership you would like your customer to have. To do this, our New.cshtml view needs to know about membership types, so we need to edit our CustomersController:

```cshtml
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
            var membershipTypes = _context.MembershipTypes.ToList(); // We cannot pass this to the View() method because later we want to implement editing a customer because there we will also need to pass a Customer to this view. In cases like this we need to create a ViewModel
            var viewModel = new NewCustomerViewModel()
            {
                MembershipTypes = membershipTypes
            };

            return View(viewModel);
        }
    }
}
```

We can't just pass the membershipTypes var though the View() method because we will also need to pass through the Customer too and it cannot take both arguments. For that reason we create a ViewModel that includes both the Customer and the MembershipsTypes and then pass through the ViewModel. See the code for the NewCustomerViewModel below.

```cs
namespace Vidly.ViewModels
{
    public class NewCustomerViewModel
    {
        public IEnumerable<MembershipType> MembershipTypes { get; set; } // We use IEnumberable<> here instead of List<> because we will not need to use the addtional functionality a list provides since users will never be adding/removing/updating membership types
        public Customer Customer { get; set; }
    }
}
```
