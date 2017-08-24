# Domain Modeling

We can start solving this problem by creating a controller to handle our new rentals page:

```cs
using System;
using System.Web.Http;
using Vidly.DTOs;

namespace Vidly.Controllers.API
{
    public class NewRentalController : ApiController
    {
        [HttpPost]
        public IHttpActionResult CreateNewRentals(NewRentalDto newRental)
        {
            throw new NotImplementedException();
        }
    }
}
```

This api controller has an action to create new movie rentals. Notice that we have this action decorated with the HTTP post attribute because we will be creating new objects, so to comply with restful conventions we should use HTTP post here.

We also have to create a new DTO to protect the data that will be passed through out new api controller's action:

```cs
using System.Collections.Generic;

namespace Vidly.DTOs
{
    public class NewRentalDto
    {
        public int CustomerId { get; set; }
        public List<int> MovieIds { get; set; }
    }
}
```

Now to implement this API we need to look at our **Domain Model** first. Does our domain model currently support rentals? It does not, we have nothing that describes a rental. So we need to do some domain modeling. 

> A customer can rent **many** movies

> A movie can be rented by **many** customers

So we need a many to many association between these two classes: customer and movie. The association itself needs attributes. When a customer rents a movie, we need to know when they rented that movie and when they returned it. We need an association class called **Rental** that will hold the fields **DateRental** and **DateReturned**. In terms of implementations, this is actually really easy. Our rental class should have those properties you see here, as well as two navigation properties: customer and movie. It also needs an Id, and this is a requirement by any framework so it can create the primary key in the corresponding table.

What we should do is add the rental class to our domain model and use a code-first migration to bring the database up to date

***

### Code

#### Create Rental Model

First we create our Rental Model and put it in the Models folder:

```cs
using System;
using System.ComponentModel.DataAnnotations;

namespace Vidly.Models
{
    public class Rental
    {
        public int Id { get; set; }

        [Required]
        public Customer Customer { get; set; }

        [Required]
        public Movie Movie { get; set; }

        public DateTime DateRented { get; set; }

        public DateTime? DateReturned { get; set; }
    }
}
```

#### Create Rentals Table

We add a new DbSet to the ApplicationDbContext file in the **IdentityModels.cs** file called **Rentals**

```
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        //DbContext is now aware of these classes
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<MembershipType> MembershipTypes { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Rental> Rentals { get; set; }
```

Then we create a new migration and update the database

***

### Implement the API

When starting to implement our API, it is best to forget about all the edge cases validation. These will distract us and get us stuck in an endless loop.

In the **CreateNewRentals()** action in our **NewReleaseController.cs** file we need to load the customer and the movies based on the Id's that our DTO gives us. Then for each movie we need to create a new rental object for that movie and the given customer and then add it to the database.

```
        [HttpPost]
        public IHttpActionResult CreateNewRentals(NewRentalDto newRental)
        {
            var customer = _context.Customers.Single(
                c => c.Id == newRental.CustomerId);

            var movies = _context.Movies.Where(
                m => newRental.MovieIds.Contains(m.Id));

            foreach ( var movie in movies)
            {
                var rental = new Rental
                {
                    Customer = customer,
                    Movie = movie,
                    DateRented = DateTime.Now
                };

                _context.Rentals.Add(rental);
            }

            _context.SaveChanges();

            return Ok();
        }
```

First we get the customer from the context and we do this using the **Single()** method, not **SingleOrDefault()** because we are assuming that the client should be sending us the right customerId since the staff member will select this customerId from a list. If a malicious user want to send us invalid customer Ids, this line will throw an exception and the malicious user will get a vague internal server error in the response. If ever you are building a public API that can be used by various applications, that's a different story. You would use **SingleOrDefault()** and check if the customer that was returned is null, and if so you would return **BadRequest()** with a string argument detailing the error. Our API is for internal use, so writing this extra code does not give us any value.

Very similarly we get the movies from the context using **Where()** to get multiple movies, specifically movies that contain the Id.

Then we iterate over the movies, and for each movie we create a rental object and set the customer, movie, and dateRented for that movie. Then we add the movie (stored in the **rental** variable) to the context.

Then we run **context.SaveChanges()** to save the data to the Rentals table we made. Lastly, we return **Ok()**.

This is a great start for our API, but now we need to add the details

***

### Adding Details To Our API

As a part of our rental process, we need to take into account the availability of a movie. Our domain model currently has the ability to keep track of the availability of a movie because in our movie class we have this property **NumberInStock**, and we also have the **Rentals** class that tells us the number of **ActiveRentals**. The **Availability** of a movie is equal to the **NumberInStock** minus the **Active Rentals**.

The problem is that if we want to give a report to the user to see the movies that are not available, for each movie we have to look up our rentals table to calculate how many active rentals we have for that movie. As the data in our rentals table grows we will see our performance degrade. We need a different solution. **This is not premature optimization**.

What if we add a new property to our ***Movies** class called **NumberAvailable**? Initially, it will be the same as **NumberInStock**. When a customer rents that movie, we decrease it by one. When the customer returns that movie, we increase it by one. This will make it more efficient. **We don't sacrifice the maintainability of our application for premature optimization**. With this solution we will improve the maintainability of our application and optimize its performance at the same time. **Premature optimization happens when you modify the code structure to make it unmaintainable for the sake of improving performance**.

***

### Code

First we add the **NumberAvailable** property to the movie class:

```
    public class Movie
    {
        // This is a plain CLR object/POKO which represents the state and behavior of our application in terms of its problem domain
        public int Id { get; set; }

        public string Name { get; set; }

        [Display(Name = "Genre")]
        [Required]
        public byte GenreId { get; set; }

        [Required]
        public Genre Genre { get; set; }

        [Display(Name = "Release Date")]
        public DateTime ReleaseDate { get; set; }

        [Display(Name = "Date Added")]
        public DateTime DateAdded { get; set; }

        [Display(Name = "Number in Stock")]
        public byte NumberInStock { get; set; }

        public byte NumberAvailable { get; set; }
    }
```

We then add a new code-first migration *but* we modify the generated **Up()** method to include an **Sql()** call that sets the **NumberAvailable** equal to the **NumberInStock**.

```
    public partial class AddNumberAvailableToMovies : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Movies", "NumberAvailable", c => c.Byte(nullable: false));

            Sql("UPDATE Movies SET NumberAvailable = NumberInStock");
        }
        
        public override void Down()
        {
            DropColumn("dbo.Movies", "NumberAvailable");
        }
    }
```

Then we modify the **CreateNewRental()** method in the **NewTentalsController.cs** file by adding a line to decrement the **NumberAvailable** property of the movie that is being rented:

```
    public class NewRentalController : ApiController
    {
        private ApplicationDbContext _context;

        [HttpPost]
        public IHttpActionResult CreateNewRentals(NewRentalDto newRental)
        {
            var customer = _context.Customers.Single(
                c => c.Id == newRental.CustomerId);

            var movies = _context.Movies.Where(
                m => newRental.MovieIds.Contains(m.Id));

            foreach ( var movie in movies)
            {
                movie.NumberAvailable--;

                var rental = new Rental
                {
                    Customer = customer,
                    Movie = movie,
                    DateRented = DateTime.Now
                };

                _context.Rentals.Add(rental);
            }

            _context.SaveChanges();

            return Ok();
        }
    }
```
