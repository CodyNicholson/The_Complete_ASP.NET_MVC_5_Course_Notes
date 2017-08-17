# Querying Objects

Now that we have a database we need to let the CustomersController know about the ApplicationDbContext so that our controller can take Customers out of the database and display them on the website.

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
            var customers = _context.Customers.ToList(); //When this is called Entity Framework will not query the database - this is called deferred execution

            return View(customers);
        }

        public ActionResult Details(int id)
        {
            var customer = _context.Customers.SingleOrDefault(c => c.Id == id); //This will make our query execute immediately
 
             if (customer == null)
                 return HttpNotFound();
 
             return View(customer);
         }
    }
}
```

Now that the _context (that has our database data) is available in the controller we can use it to list the Customers on our page and also display each Customers details page as well.

We can manually add data to our application database by clicking on the database file in the App_Data folder. Then we can right-click on the Customers class and click the **Show Table Data** button. This will take us to our table, where we can input the appropriate data into each cell (The Id will be generated automatically after the other data is input). We can see the data input successfully by opening our application.
