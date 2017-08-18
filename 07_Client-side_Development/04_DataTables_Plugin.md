# DataTables Plugin

The **DataTables Plugin** adds pagination, sorting, and filtering to our tables in the UI. In **package manager console** run the command: **install-package jquery.datatables -version:1.10.11**. Now in the solution explorer, in the *Scripts* folder you'll see a new folder **DataTables** with lots of scripts. We are only going to use two of these scripts which we will include in the **BundleConfig.cs** as seen below.

Go to the **App_Start/BundleConfig.cs** file. Also open the **Views/Shared/Layout.cshtml**. In the bottom of the **Layout.cshtml** change:

```cs
@Scripts.Render("~/bundles/jquery")
@Scripts.Render("~/bundles/bootstrap")
```

To:

```cs
@Scripts.Render("~/bundles/lib")
```

This **lib** bundle will represent third party libraries. We can consolidate all of our libraries here

In our **BundleConfig.cs** change:

```cs
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
```

To be:

```cs
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/lib").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/bootbox.js",
                        "~/Scripts/respond.js",
                        "~/Scripts/datatables/jquery.datatables.js",
                        "~/Scripts/datatables/datatables.bootstrap.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap-lumen.css",
                      "~/Content/datatables/css/datatables.bootstrap.css",
                      "~/Content/site.css"));
        }
```

Notice we added the DataTables library into the **lib** bundle above, so now we can use these scripts anywhere in the project. Also notice we added the DataTables Stylesheet to the **CSS** bundle as well.

***

### Adding The DataTable To The Customers Page

Now we can go to **~/Views/Customers/Index.cshtml** to add the DataTable. In the document.ready, before handling the click event, we want to get a reference to our customers table and simply call the DataTable function on it. We change:

```cs
@section scripts
{
    <script>
        $(document).ready(function() {
            $("#customers").on("click", ".js-delete", function() {
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

To:

```cs
@section scripts
{
    <script>
        $(document).ready(function() {
            $("#customers").DataTable();

            $("#customers").on("click", ".js-delete", function() {
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

***

### Efficiency

Using DataTables only takes one line of code after the dependencies are installed: **$("#customers").DataTable();**. This comes with a cost. If we have more than a few hundred records to work with, it is faster to get raw data from the server than to parse through all of the data with DataTables. This way, instead of returning all the markup from the server, we return JSON objects which are more lightweight.
