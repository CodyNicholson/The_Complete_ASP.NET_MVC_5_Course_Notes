# Anti-Forgery Tokens

If a user logs into your application then quickly goes to another website and then returns, they should still be logged in

This is possible because your logged-in status is kept by the server for around 20 mins in most application

If you go to a hackers website though they can use javascript to get this information and do things in the application while being logged in as you - This is called a Cross-site Request Forgery Attack (CSRF Attack)

To prevent this we use **Anti-Forgery Tokens** that we can hide in the bottom of our views like this:

```html
@model Vidly.ViewModels.CustomerFormViewModel
@{
    ViewBag.Title = "New";
    Layout = "~/Views/Shared/_Layout.cshtml";
}

<h2>New Customer</h2>

@using (@Html.BeginForm("Save", "Customers")) // The "using" block will create an ending "</form> tag for us
{
    @Html.ValidationSummary(true, "Please fix the following errors:")
    <div class="form-group">
        @Html.LabelFor(m => m.Customer.Name)
        @Html.TextBoxFor(m => m.Customer.Name, new {@class = "form-control"})
        @Html.ValidationMessageFor(m => m.Customer.Name)
    </div>
    <div class="form-group">
        @Html.LabelFor(m => m.Customer.MembershipTypeId)
        @Html.DropDownListFor(m => m.Customer.MembershipTypeId, new SelectList(Model.MembershipTypes, "Id", "Name"), "Select Membership Type", new { @class = "form-control" })
        @Html.ValidationMessageFor(m => m.Customer.MembershipTypeId)
    </div>
    <div class="form-group">
        @Html.LabelFor(m => m.Customer.Birthdate)
        @Html.TextBoxFor(m => m.Customer.Birthdate, "{0:d MMM yyyy}", new {@class = "form-control"})
        @Html.ValidationMessageFor(m => m.Customer.Birthdate)
    </div>
    <div class="checkbox">
        <label>
            @Html.CheckBoxFor(m => m.Customer.IsSubscribedToNewsletter) Subscribe to newsletter?
        </label>
    </div>
    @Html.HiddenFor(m => m.Customer.Id)
    @Html.AntiForgeryToken()
    <button type="submit" class="btn btn-primary">Save</button>
}

@section scripts
{
    @Scripts.Render("~/bundles/jqueryval")
}
```

Then we can validate the Anti-Forgery Token by using the [ValidateAntiForgeryToken] annotation above our action

Notice the [ValidateAntiForgeryToken] below

```cs
        [HttpPost] // This is here so that our Action can only be called using HttpPost and not HttpGet. By convention, if your actions modify data they should only be accessible using HttpPost
        [ValidateAntiForgeryToken] // Protects against Cross-site Request Forgery
        public ActionResult Save(Customer customer) // This is called Model Binding. MVC framework will automatically map request data to this object
        {
            // Checks if the entered information is valid based on the Customer Data Annotations
            if (!ModelState.IsValid)
            {
                var viewModel = new CustomerFormViewModel()
                {
                    Customer = customer,
                    MembershipTypes = _context.MembershipTypes.ToList()
                };
                return View("CustomerForm", viewModel);
            }

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

