# Calling An API Using jQuery

```html
@model IEnumerable<Vidly.Models.Customer>
@{
    ViewBag.Title = "Customers";
    Layout = "~/Views/Shared/_Layout.cshtml";
}
<h2>Customers</h2>
@if (!Model.Any())
{
    <p>We don't have any customers yet.</p>
}
else
{
    <table id="customers" class="table table-bordered table-hover">
        <thead>
            <tr>
                <th>Customer</th>
                <th>Membership Type</th>
                <th>Delete</th>
            </tr>
        </thead>
        <tbody>
            @foreach (var customer in Model)
            {
                <tr>
                    <td>@Html.ActionLink(customer.Name, "Edit", "Customers", new {id = customer.Id}, null)</td>
                    <td>@customer.MembershipType.Name</td> <!-- This must be Eager Loaded in the Customers Controller -->
                    <td>
                        <button data-customer-id="@customer.Id" class="btn-link js-delete">Delete</button>
                    </td>
                </tr>
            }
        </tbody>
    </table>
}
@section scripts
{
    <script>
        $(document).ready(function() {
            $('#customers .js-delete').on("click", function() {
                var button = $(this);
                $.ajax({
                    url: "/api/customers/" + button.attr("data-customer-id"),
                    method: "DELETE",
                    success: function() {
                        console.log("Successful Delete");
                        button.parents("tr").remove();
                    }
                });
            });
        });
    </script>
}
```

Notice near the bottom of the html file you can see we added a Delete button that calls a JavaScript function to delete the customer

The function will go to the /api/customers/ with the id of the customer, and use the Delete method to remove the customer from the database. After this, you remove the table row (tr) from the Customers table.
