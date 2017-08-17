# Optimizing jQuery

In the below code we create a unique delete function for each customer displayed on the page:

```html
@section scripts
{
    <script>
        $(document).ready(function() {
            $('#customers .js-delete').on("click", function() {
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
    </script>
}
```

This function works and the model displayed fine, but we don't need a unique function for every button. We should instead make it so that all customers call the same single Delete function with their unique Id numbers.

If we change the code like this, we will only have one Delete function for all customers:

```html
@section scripts
{
    <script>
        $(document).ready(function() {
            $('#customers').on("click", ".js-delete", function() {
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
    </script>
}
```

Notice we added a third parameter to the on() method (".js-delete") which makes it so that our function filters and executes on every element in the customers table that has the class "js-delete".
