# Adding Auto-Completion

To add auto-completion to our **Customer** and **Movie** textboxes on our **New Rental** form we are going to use a jQuery plugin called **Typeahead** that was developed by the Twitter team. So we go to package manager console and run the command: **install-package Twitter.Typeahead**. Now we go to our **BundleConfig.cs** file and add the **~scripts/typeahead.bundle.js** to our **~/bundles/lib**.

```cs
            bundles.Add(new ScriptBundle("~/bundles/lib").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/bootbox.js",
                        "~/Scripts/respond.js",
                        "~/Scripts/datatables/jquery.datatables.js",
                        "~/Scripts/datatables/datatables.bootstrap.js",
                        "~/Scripts/typeahead.bundle.js"
                      ));
```

Next, we need to borrow some styles from the https://twitter.github.io/typeahead.js/ website since this package only contains the scripts and no stylesheets. We also need to take some JavaScript to make it look good, too. 

CSS from website:

```css
.typeahead {
    background-color: #fff;
}

    .typeahead:focus {
        border: 2px solid #0097cf;
    }

.tt-query {
    -webkit-box-shadow: inset 0 1px 1px rgba(0, 0, 0, 0.075);
    -moz-box-shadow: inset 0 1px 1px rgba(0, 0, 0, 0.075);
    box-shadow: inset 0 1px 1px rgba(0, 0, 0, 0.075);
}

.tt-hint {
    color: #999
}

.tt-menu {
    width: 422px;
    margin: 12px 0;
    padding: 8px 0;
    background-color: #fff;
    border: 1px solid #ccc;
    border: 1px solid rgba(0, 0, 0, 0.2);
    -webkit-border-radius: 8px;
    -moz-border-radius: 8px;
    border-radius: 8px;
    -webkit-box-shadow: 0 5px 10px rgba(0,0,0,.2);
    -moz-box-shadow: 0 5px 10px rgba(0,0,0,.2);
    box-shadow: 0 5px 10px rgba(0,0,0,.2);
}

.tt-suggestion {
    padding: 3px 20px;
    font-size: 18px;
    line-height: 24px;
}

    .tt-suggestion:hover {
        cursor: pointer;
        color: #fff;
        background-color: #0097cf;
    }

    .tt-suggestion.tt-cursor {
        color: #fff;
        background-color: #0097cf;
    }

    .tt-suggestion p {
        margin: 0;
    }
```

JS from website:

```js

var bestPictures = new Bloodhound({
  datumTokenizer: Bloodhound.tokenizers.obj.whitespace('value'),
  queryTokenizer: Bloodhound.tokenizers.whitespace,
  prefetch: '../data/films/post_1960.json',
  remote: {
    url: '../data/films/queries/%QUERY.json',
    wildcard: '%QUERY'
  }
});

$('#remote .typeahead').typeahead(null, {
  name: 'best-pictures',
  display: 'value',
  source: bestPictures
});
```

We create a new CSS file called **typeahead.css** to put the css in and then add it to our **BundleConfig.cs** file:

```cs
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/content/bootstrap-lumen.css",
                      "~/content/datatables/css/datatables.bootstrap.css",
                      "~/content/typeahead.css",
                      "~/content/site.css"));
        }
```

We add the JavaScript to our **New.cshtml** view in a script tag:

```
var bestPictures = new Bloodhound({
  datumTokenizer: Bloodhound.tokenizers.obj.whitespace('value'),
  queryTokenizer: Bloodhound.tokenizers.whitespace,
  prefetch: '../data/films/post_1960.json',
  remote: {
    url: '../data/films/queries/%QUERY.json',
    wildcard: '%QUERY'
  }
});

$('#remote .typeahead').typeahead(null, {
  name: 'best-pictures',
  display: 'value',
  source: bestPictures
});
```

First we rename the variable **bestPictures** to be **customers**. **Bloodhound** is the suggestion engine behind typeahead, so it encapsulates all that business logic for auto-completion. Such as calling the backend API, caching the result, and so on. We have a property called **datumTokenizer** which references a function that takes a datum and transforms it into a bunch of string tokens. We give it the **name** property of customers and use whitespace to tokenize them. These tokens are required by **Bloodhound** to do its job. We also have **queryTokenizer** which is a function that transforms query into a bunch or tokens. Next we have **prefetch** which is useful for providing data upon initialization. This prevents additional requests to the server. In this case we don't need it. In the **remote** property we need to set the url, and we need to provide a parameter here in query string format called **%QUERY**. This variable is specified in the wild card. Basically, that the user types in the textbox will be placed here at runtime. With this, later we can filter our customers and return those who match the query. Next, we need to reference our textbox. We tell it to look for an element with **id** equal to **customer**, and add that **id** to our textbox above. For the first argument to the typeahead method we set **minLength** to 3 so that we will only query the server when the user types at least three characters. We can also set **highlight** to true so the characters and the search result that match out query will be bold. We set the **name** property to 'customers'. We set **display** to 'name' so that we display the 'name' property of our customers in the suggestion list. We set the **source** of our Bloodhound object to **customers** since that is the type of object we want to search for in our textbox.

We now need to add the selection event of typeahead. When the user selects a customer, we need to know what user it was so we can send it to the server. We can add the **on()** method with the parameter "typeahead:select", and a callback function with two parameters of its own: **e** for event, and **customer** so we know which one to select. At this point we want to store the customer somewhere. So we can create a new variable at the top called **vm** for *viewmodel*, and initialize it to a blank object. Earlier, we learned about viewmodels on the server and used it to encapsulate data for a given view. Here we have a viewmodel on the client. Now in our callback function when we get this customer, we're going to set **vm.custmerId** equal to **customer.id**. So later, when the user submits the form, you're going to send this vm to our web api.

```
@model dynamic

@{ 
    ViewBag.Title = "New Rental Form";
}

<h2>New Rentals Form</h2>

<form>
    <div class="form-group">
        <label>Customer</label>
        <input id="customer" type="text" value="" class="frm-control" />
    </div>

    <div class="form-group">
        <label>Movie</label>
        <input id="movie" type="text" value="" class="frm-control" />
    </div>

    <button class="btn btn-primary">Submit</button>
</form>

@section scripts
{
    <script>
        $(document).ready(function () {
            var customers = new Bloodhound({
                datumTokenizer: Bloodhound.tokenizers.obj.whitespace('name'),
                queryTokenizer: Bloodhound.tokenizers.whitespace,
                remote: {
                    url: '/api/customers?query=%QUERY',
                    wildcard: '%QUERY'
                }
            });

            $('#customer').typeahead({
                minLength: 3,
                highlight: true
            },{
                    name: 'customers',
                    display: 'name',
                    source: customers
            }).on("typeahead:select", function (e, customer) {
                    vm.customerId = customer.id;
            });

            var movies = new Bloodhound({
                datumTokenizer: Bloodhound.tokenizers.obj.whitespace('name'),
                queryTokenizer: Bloodhound.tokenizers.whitespace,
                remote: {
                    url: '/api/movies?query=%QUERY',
                    wildcard: '%QUERY'
                }
            });

            $('#movie').typeahead({
                minLength: 3,
                highlight: true
            }, {
                    name: 'movies',
                    display: 'name',
                    source: movies
                }).on("typeahead:select", function (e, movie) {

                });
        });
</script>
}
```
