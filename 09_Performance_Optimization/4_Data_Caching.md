# Data Caching

Sometimes when we want to cache a piece of data, not HTML, we may want to work with this data across different actions or controllers. In this case you might choose to cache it. In some cases you may end up causing unnecessary complexity that ends up being a smell in our code base. If you do your performance profiling first, then you can avoid issues like this.

If you do your profiling and determine that a piece of data is better to be stored in the cache for a limited period of time, then this is how we will do it: In the action, the first time someone hits this end point we will get the data from the database and then store it in the cache. In all subsequent requests we'll get the data from the cache. 