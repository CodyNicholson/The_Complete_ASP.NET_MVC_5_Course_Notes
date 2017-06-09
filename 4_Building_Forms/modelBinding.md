# Model Binding

Now we can make our Create() action and pass a NewCustomerViewModel object through the method. MVC framework will use **Model Binding** to automatically map request data to this object (It will get all the data from the form).

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
            var membershipTypes = _context.MembershipTypes.ToList(); // We cannot pass this to the View() method because later we want to implement editing a customer because there we will also need to pass a Customer to this view. In cases like this we need to create a ViewModel
            var viewModel = new NewCustomerViewModel()
            {
                MembershipTypes = membershipTypes
            };

            return View(viewModel);
        }

        [HttpPost] //This is here so that our Action can only be called using HttpPost and not HttpGet. By convention, if your actions modify data they should only be accessible using HttpPost
        public ActionResult Create(Customer customer) // This is called Model Binding. MVC framework will automatically map request data to this object
        {
            return View();
        }
    }
}
```
