# Building An API

First create an API folder in your Controllers folder in your ASP.NET MVC5 application

Inside your API folder, add an empty Web API controller

Next, add the "GlobalConfiguration.Configure(WebApiConfig.Register);" statement at the bottom of your new controller to the to the Application_Start() function in your Global.asax.cs file located at the bottom of the project

***

Here is the finished Customers API:

```cs
namespace Vidly.Controllers.API
{
    public class CustomersController : ApiController
    {
        private ApplicationDbContext _context;

        public CustomersController()
        {
            _context = new ApplicationDbContext();
        }

        // GET /api/customers
        public IEnumerable<Customer> GetCustomers()
        {
            return _context.Customers.ToList();
        }

        // GET /api/customer/id
        public Customer GetCustomer(int id)
        {
            var customer = _context.Customers.SingleOrDefault(c => c.Id == id);

            if (customer == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return customer;
        }

        // POST /api/customers
        [HttpPost] // Since we are creating a resource we use HttpPost
        public Customer CreateCustomer(Customer customer)
        {
            if(!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            _context.Customers.Add(customer);
            _context.SaveChanges();

            return customer;
        }

        // PUT /api/customers/id
        [HttpPut]
        public void UpdateCustomer(int id, Customer customer)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            var customerInDb = _context.Customers.SingleOrDefault(c => c.Id == id);

            if(customerInDb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            customerInDb.Name = customer.Name;
            customerInDb.Birthdate = customer.Birthdate;
            customerInDb.IsSubscribedToNewsletter = customer.IsSubscribedToNewsletter;
            customerInDb.MembershipTypeId = customer.MembershipTypeId;

            _context.SaveChanges();
        }

        // DELETE /api/customers/id
        [HttpDelete]
        public void DeleteCustomer(int id)
        {
            var customerInDb = _context.Customers.SingleOrDefault(c => c.Id == id);

            if (customerInDb == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            _context.Customers.Remove(customerInDb);
            _context.SaveChanges();
        }
    }
}
```

If we go to localhost:123456/api/customers we can see the XML data from our customers table

***

### Testing This API

To test an API effectively we can use the Google Chrome app **Postman REST Client**

1. In the Postman app we can enter the URL for our application's XML data - in this case: localhost:123456/api/customers

If we click send, we can see that data in either JSON or XML

2. Switch the request type by the URL bar from GET to POST since we want to test our ability to add a customer

3. Go ahead and change or rewrite some of the JSON data so that we can try and post a new customer

4. Change the Header to **content-type** and the Value to **application/json**

5. Click send to test our API's POST with the new user we created - if the status is "200 OK" and no error message appears, the API's POST was a success
