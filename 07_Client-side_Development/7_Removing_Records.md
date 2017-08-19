# Removing Records

Currently, if we pres the delete button and confirm deletion the record will not delete. Here is why:

```js
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
```

Notice in the **success()** object the button only removes the **tr** element from the DOM. Since the list of customers is kept internally, the DataTable simply reads the data again and creates a new **tr** element to replace the deleted one upon reloading.

To fix this, instead of working directly with the COM, we should remove the customer from our internal list and then refresh the table. First, we need a reference to our DataTable. On the Customers **Index.html** page we change:

```js
$("#customers").DataTable()
```

To be:

```js
var table = $("#customers").DataTable()
```

***

Now that we have a reference to the table we can call the **remove()** method on the row of the table we want to delete to delete it internally:

```js
            $("#customers").on("click", ".js-delete", function () {
                var button = $(this);

                bootbox.confirm("Are you sure you want to delete this customer?", function(result) {
                    if (result) {
                        $.ajax({
                            url: "/api/customers/" + button.attr("data-customer-id"),
                            method: "DELETE",
                            success: function () {
                                table.row(button.parents("tr")).remove().draw();
                            }
                        });
                    }
                });
                
            });
```

After we call the **remove()** method we then call the **draw()** method to redraw the table with the updated data

The **row()**, **remove()**, and **draw()** method are all a part of the DataTable API
