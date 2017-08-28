# Filtering Records

If we type something in the typeahead textbox it will give us all of the options, and won't filter the options based on what we have typed. We want to filter records on the server. To do this, we need to go to the **Api/CustomerController.cs**. Our **GetCustomers()** method currently looks like this:

```cs
        public IHttpActionResult GetCustomers()
        {
            var customerDtos = _context.Customers
                .Include(c => c.MembershipType)
                .ToList()
                .Select(Mapper.Map<Customer, CustomerDto>);

            return Ok(customerDtos);
        }
```

We will add a nullable string argument to this method called **query**. Instead of called **ToList()** which will immediately execute the query. Customer DTO will be an IQueryable object, so we can modify that query, apply the filter, and then call **ToList()** to execute it. So we can rename our cutomerDto var to **customersQuery**, and declare another variable **customerDtos** and set it **customersQuery.ToList().Select(Mapper.Map(Customer, CustomerDto));**. We will also add an if statement to check if we do have a query, and if we do we will dynamically modify our customers query.

```
        public IHttpActionResult GetCustomers(string query = null)
        {
            var customersQuery = _context.Customers
                .Include(c => c.MembershipType);

            if (!String.IsNullOrWhiteSpace(query))
                customersQuery = customersQuery.Where(c => c.Name.Contains(query));

            var customerDtos = _context.Customers
                .ToList()
                .Select(Mapper.Map<Customer, CustomerDto>);

            return Ok(customerDtos);
        }
```

***

We can do the almost the same for the movies input in the **GetMovies()** method in the **Api/MoviesController.cs**:

```

```

We added the moviesQuery var. We have applied the **Where()** clause here to get only the available movies. If our query has a value, we see if it contains the user input. Then we call **ToList()** on the resulting options and return it. Now we are ready to post the form.
