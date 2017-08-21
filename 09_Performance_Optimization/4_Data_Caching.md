# Data Caching

Sometimes when we want to cache a piece of data, not HTML, we may want to work with this data across different actions or controllers. In this case you might choose to cache it. In some cases you may end up causing unnecessary complexity that ends up being a smell in our code base. If you do your performance profiling first, then you can avoid issues like this.

If you do your profiling and determine that a piece of data is better to be stored in the cache for a limited period of time, then this is how we will do it: In the action, the first time someone hits this end point we will get the data from the database and then store it in the cache. In all subsequent requests we'll get the data from the cache. 

```cs
        public ActionResult Index()
        {
            if(MemoryCache.Default["Genres"] == null)
            {
                MemoryCache.Default["Genres"] = _context.Genres.ToList();
            }

            var genres = MemoryCache.Default["Genres"] as IEnumerable<Genre>;
            return View();
        }
```

**MemoryCache** is the class we use to store pieces of data in the cache for a limited period of time. This class has a static property **Default** which is like a dictionary, so we can index it. For every item we store in the cache we use a key to access it. We will call our key *Genres*. If *Genres* is null, then we assign it to be equal to the list of Genres that we have in our database. After our if statement we set a variable *genres* equal to the genres we stored in our MemoryCache. Since MemoryCache.Default Genres will return an object, we need to cast it to an IEnumerable of type Genre.

This is data creation. We only use this technique only if we really need to and only after we have done performance profiling. By blindly storing data in the cache, you will increase memory consumption of your application. Also, it leads to unnecessary complexities both at the architectural and code level, especially when working with entity framework. If in an action we are modifying data with entity framework and you want to reference some of the objects in the cache, we're going to run into all sorts of issues because those objects are not part of your dbContext. This means that when we save changes we are either going to get duplicate data or exceptions thrown by entity framework. As a work around, we have to attach those objects to dbContext.
