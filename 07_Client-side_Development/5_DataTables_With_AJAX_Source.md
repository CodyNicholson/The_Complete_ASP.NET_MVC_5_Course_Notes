# DataTables With AJAX Source

To use our API as the source for the DataTable we need to provide a configuration object. In this object we need a property called AJAX which is also an object with two properties: **url** and **data**.

```js
@section scripts
{
    <script>
        $(document).ready(function () {
            $("#customers").DataTable({
                ajax: {
                    url: "api/customers",
                    dataSrc: "" 
                }
            });

            $("#customers").on("click", ".js-delete", function () {
                var button = $(this);

                bootbox.confirm("Are you sure you want to delete this customer?", function(result) {
                    if (result) {
                        $.ajax({
                            url: "/api/customers/" + button.attr("data-customer-id"),
                            method: "DELETE",
                            success: function () {
                                button.parents("tr").remove();
                            }
                        });
                    }
                });
                
            });
        });
    <script>
}
```

Imagine our API responded to an object like this:

```
{
    count: 10,
    customers: [
        { ... },
        { ... }
        ...
    ]
}
```

In this case, the customer's property is the actual source of data. So, we would set **dataSrc** = "customers", but in our case the response we get from our API is an array of objects, so this array itself if the actual source of data. Thus, our DataTable doesn't need to go to another object to get it. That's why we set **dataSrc** = "".

Now we need to specify our columns

***

### Columns

For each column we supply an object. In this object we specify various properties of that column. For example, we use the data property to tell the data table where to get data for this column. In the first column we want to display the customer's name, so we set **data** = "name".

With this configuration, DataTable will look at this property of the customer object and use that as plain text t render each row. But in our first column we want to have a link that takes us to the customer's edit page. So, we need to supply a custom rendering function. This function will be called for rendering each row of the table.

In our render function we can add a few different parameters to control rendering. The first parameter we can call **data**, which is the value of this property in our customer object. The second parameter is the **type** for this column and the third parameter is the actual object or roll we are rendering. This roll we will call **customer**. When we call this function we will want to return a string.

```js
            $("#customers").DataTable({
                ajax: {
                    url: "api/customers",
                    dataSrc: "" 
                },
                columns: [
                    {
                        data: "name",
                        render: function (data, type, customer) {
                            return "<a href='/customers/edit/" + customer.id + "'>" + customer.name + "</a>";
                        }
                    },
                    {
                        data: "name"
                    },
                    {
                        data: "id",
                        render: function (data) {
                            return "<button class='btn-link js-delete' data-customer-id=" + data + ">Delete</button>";
                        }
                    }
                ]
            });
```

We add some more columns to hold our other data like the name and the delete button

***

Now that we are using our API to render this table, we don't need to render the customers on the server - so we can get rid of this foreach block:

```cs
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
```

And change it to be this since we want to keep the table structure:

```cs
    <table id="customers" class="table table-bordered table-hover">
        <thead>
            <tr>
                <th>Customer</th>
                <th>Membership Type</th>
                <th>Delete</th>
            </tr>
        </thead>
        <tbody>
        </tbody>
    </table>
```

***

Also, in our action, we don't need to get the list of customers since our data table will send an AJAX request to get it from our API. So we go to our CustomersController.cs and change:

```cs
        public ActionResult Index()
        {
            var customers = _context.Customers.Include(c => c.MembershipType).ToList(); //When this is called Entity Framework will not query the database - this is called deferred execution

            return View(customers);
        }
```

To be:

```cs
        public ActionResult Index()
        {
            return View();
        }
```

***

Lastly, on the Customers **Index.cshtml** page, we can remove the Model check since we can write some simple jQuery code to handle the event in which we don't have any customers. So we remove this:

```cs
@if (!Model.Any())
{
    <p>We don't have any customers yet.</p>
}
```
