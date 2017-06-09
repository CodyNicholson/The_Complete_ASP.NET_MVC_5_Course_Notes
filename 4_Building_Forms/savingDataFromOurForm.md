# Saving Data From Our Form

To save data from our form we only need to call two methods in our Create() action in our CustomersController.

```cs
[HttpPost] // This is here so that our Action can only be called using HttpPost and not HttpGet. By convention, if your actions modify data they should only be accessible using HttpPost
        public ActionResult Create(Customer customer) // This is called Model Binding. MVC framework will automatically map request data to this object
        {
            _context.Customers.Add(customer); // This does not write customer to the database, this is just saved in local memory
            _context.SaveChanges(); // To persist these changes, we write the customer to the database using the SaveChanges() method

            return RedirectToAction("Index", "Customers");
        }
```

The .Add() method takes the customer and adds it to the table without committing it to the database

The .SaveChanges() method commits the changes to the database
