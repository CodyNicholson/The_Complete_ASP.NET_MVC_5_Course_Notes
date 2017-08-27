# Improving Look & Field

The first thing we should fix about our form is the inline text boxes. We want to put them on a separate line. When we apply the typeahead plugin on a textbox this plugin modifies the DOM and adds a few new elements. We get a span element and two inputs all with various styles. To fix our textbox we can wrap the input elements in a div we will name **tt-container**.

```
<form>
    <div class="form-group">
        <label>Customer</label>
        <div class="tt-container">
            <input id="customer" type="text" value="" class="frm-control" />
        </div>
    </div>

    <div class="form-group">
        <label>Movie</label>
        <div class="tt-container">
            <input id="movie" type="text" value="" class="frm-control" />
        </div>
    </div>

    <ul id="movies" class="list-group"></ul>

    <button class="btn btn-primary">Submit</button>
</form>
```

Now we can go to our **typeahead.css** file and add this new class on the bottom:

```
.tt-container {
    position: relative;
}
```

***

The other issue we have is with our list of movies. They appear in a bulleted list. So we add the class **list-group-item** to our list item:

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

Now our look is better, but our list items are taking the entire width of the screen when we really only need a few hundred pixels. So we can add some more bootstrap classes into some new divs that we add to fix this issue:

```
<form>
    <div class="form-group">
        <label>Customer</label>
        <div class="tt-container">
            <input id="customer" type="text" value="" class="frm-control" />
        </div>
    </div>

    <div class="form-group">
        <label>Movie</label>
        <div class="tt-container">
            <input id="movie" type="text" value="" class="frm-control" />
        </div>
    </div>

    <div class="row">
        <div class="col-md-4">
            <ul id="movies" class="list-group"></ul>
        </div>
    </div>

    <button class="btn btn-primary">Submit</button>
</form>
```

The **col-md-4** bootstrap class makes our element take up only a third of the width of the screen because bootstrap divides the screen into 12 sections, and this class makes our element 4 sections wide.
