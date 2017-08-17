# Adding Validation

We used Data Annotations to make certain fields required and to set string lengths in our models, but we can also use Data Annotations for validation

-

## Three Steps Of Adding Validation

1. Add Data Annotations to your model classes that require validation

```cs
[Required] // Name is not nullable anymore, it is now required
[StringLength(255)] // Name is not unlimited characters anymore, it is 255
public string Name { get; set; }
```

2. Use (!ModelState.IsValid) to change the flow of the program, and if the ModelState is not valid then return the same view

```cs
if (!ModelState.IsValid)
    {
        var viewModel = new CustomerFormViewModel()
    {
        Customer = customer,
        MembershipTypes = _context.MembershipTypes.ToList()
    };
    return View("CustomerForm", viewModel);
}
```

3. Add Validation Messages to the form

```html
<div class="form-group">
    @Html.LabelFor(m => m.Customer.Name)
    @Html.TextBoxFor(m => m.Customer.Name, new {@class = "form-control"})
    @Html.ValidationMessageFor(m => m.Customer.Name)
</div>
```
