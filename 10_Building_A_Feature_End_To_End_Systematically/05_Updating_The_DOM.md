# Updating The DOM

When the user selects a movie we want to add this movie to a list because a customer may rent multiple movies. So in our rental form we need to create a placeholder for this list.

```
<form>
    <div class="form-group">
        <label>Customer</label>
        <input id="customer" type="text" value="" class="frm-control" />
    </div>

    <div class="form-group">
        <label>Movie</label>
        <input id="movie" type="text" value="" class="frm-control" />
    </div>

    <ul id="movies" class="list-group"></ul>

    <button class="btn btn-primary">Submit</button>
</form>
```

Now back in our callback function we get the element with the Id "movies" and append a list item on with the name of the movie and then we close the list item. You're modifying the elements directly.

```
            $('#movie').typeahead({
                minLength: 3,
                highlight: true
            }, {
                    name: 'movies',
                    display: 'name',
                    source: movies
                }).on("typeahead:select", function (e, movie) {
                    $("#movies").append("<li class='list-group-item'>" + movie.name + "</li>");
                });
```

In this particular example this is not too bad. In more complex applications this might end up being a code smell. That's when we use libraries or frameworks that provide data binding like knockout, angular, react, vue, so on.

Instead of working directly with the DOM we bind out DOM elements to a model, or more accurately a view model. The framework itself will take care of refreshing the DOM when the underlying view model changes. This way we'll have better separation of concerns because our javascript code will not include any HTML markup. Also, unit testing this code will be easier because our code will not be dependent on the availability of a document object model in our unit tests.

When we select a movie, we add this movie to our list. We need to clear the text box after selection. So we get the movie textbox. We cannot use the **val()** method to set its value because we have applied the **typeahead** plugin on it. So to change the value we need to use the typeahead plugin. We call the **typeahead()** method. As the first argument we specify the property we want to update which is **val**, and the second argument is the value.

```
            $('#movie').typeahead({
                minLength: 3,
                highlight: true
            }, {
                    name: 'movies',
                    display: 'name',
                    source: movies
                }).on("typeahead:select", function (e, movie) {
                    $("#movies").append("<li class='list-group-item'>" + movie.name + "</li>");

                    $("#movie").typeahead("val", "");
                });
```

Second, we want to store this movie in our view model so we can send it to the server later. So we go to the top where we declare our view model **vm**. We add a property called **movieIds** and initialize it to an empty array so we can push objects to it. 

```
        $(document).ready(function () {
            var customers = new Bloodhound({
                datumTokenizer: Bloodhound.tokenizers.obj.whitespace('name'),
                queryTokenizer: Bloodhound.tokenizers.whitespace,
                remote: {
                    url: '/api/customers?query=%QUERY',
                    wildcard: '%QUERY'
                }
            });

            var vm = {
                movieIds: []
            };
```

Back in the callback function we use **vm.movieIds.push()** with the argument **movie.id**

```
            $('#movie').typeahead({
                minLength: 3,
                highlight: true
            }, {
                    name: 'movies',
                    display: 'name',
                    source: movies
                }).on("typeahead:select", function (e, movie) {
                    $("#movies").append("<li class='list-group-item'>" + movie.name + "</li>");

                    $("#movie").typeahead("val", "");

                    vm.movieIds.push(movie.id);
                });
```

Now when we select a movie we add it to a list of movies below the textbox
