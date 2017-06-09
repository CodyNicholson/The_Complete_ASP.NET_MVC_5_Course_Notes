# Making The Form Editable

Currently we have a static Details.cshtml page displaying the each Customer's information. Now we will allow Customers to edit their data in their respective Detail page.

```cs
public ActionResult Edit(int id)
{
    var customer = _context.Customers.SingleOrDefault(c => c.Id == id);

    if (customer == null)
    {
        return HttpNotFound();
    }

    var viewModel = new CustomerFormViewModel()
    {
        Customer = customer,
        MembershipTypes = _context.MembershipTypes.ToList()
    };

    return View("CustomerForm", viewModel);
}
```

Notice I renamed the "New" view to "CustomerForm" and the "NewCustomerViewModel" to "CustomerFormViewModel". Now the view where you display the Customer's details is the same view that you add/update a customer.
