# Bootbox Plug-in

Bootbox is an abstraction of bootstrap that provides a number of simple functions that we can call to create different kinds of dialog boxes

To add Bootbox to the project, open Package Manager Console and type: **install-package bootbox -version:4.3.0**

Now we have Bootbox installed in our project and you can see it in the /Scripts directory

Now we need to add it to our /App-Start/BundleConfig.cs file:

```cs
public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/bootbox.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap-lumen.css",
                      "~/Content/site.css"));
        }
    }
```

We can add Bootbox to the bundles.Add("~/bundles/bootstrap") statement

Notice we did not add the minified Bootbox.js file, this is because the framework knows to minify the JS file on its own

Using Bootbox we can change the the onclick function for the Submit button on our /Views/Customers/Index.cshtml/ from:

```html
@section scripts
{
    <script>
        $(document).ready(function() {
            $('#customers .js-delete').on("click", function() {
                var button = $(this);

                if(confirm("Are you sure you want to delete this customer?"))
                {
                    $.ajax({
                        url: "/api/customers/" + button.attr("data-customer-id"),
                        method: "DELETE",
                        success: function() {
                            button.parents("tr").remove();
                        }
                    });
                }
            });
        });
    </script>
}
```

To:

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

This will give us a much nicer looking confirmation box using the Bootbox/Bootstrap popup rather than the native JavaScript popup
