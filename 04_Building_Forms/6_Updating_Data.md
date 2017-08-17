# Updating Data

To update data we have to make it so that if you change the data in the CustomerForm and click the submit button, then the database will be updated with the new data. So we can change the Create() action into the Save() action as seen below:

```cs
[HttpPost] // This is here so that our Action can only be called using HttpPost and not HttpGet. By convention, if your actions modify data they should only be accessible using HttpPost
public ActionResult Save(Customer customer) // This is called Model Binding. MVC framework will automatically map request data to this object
{
    if (customer.Id == 0)
    {
        _context.Customers.Add(customer); // This does not write customer to the database, this is just saved in local memory
    }
    else
    {
        var customerInDb = _context.Customers.Single(c => c.Id == customer.Id);

        customerInDb.Name = customer.Name;
        customerInDb.Birthdate = customer.Birthdate;
        customerInDb.MembershipTypeId = customer.MembershipTypeId;
        customerInDb.IsSubscribedToNewsletter = customer.IsSubscribedToNewsletter;
    }

    _context.SaveChanges(); // To persist these changes, we write the customer to the database using the SaveChanges() method

    return RedirectToAction("Index", "Customers");
}
```

As you can see in the above code, the first thing we do is check whether the Customer exists in the database. If they do not, then we add them, save changes, and redirect to the CustomersController's Index page.

If the Customer *does* exist, then we get the customer from the database using the .Single() method (Not the .SingleOrDefault() method because there should not be default behavior after our null check). Once we have the customer from the system we can set all of its variables, save changes, and redirect to the CustomerController's Index page.
