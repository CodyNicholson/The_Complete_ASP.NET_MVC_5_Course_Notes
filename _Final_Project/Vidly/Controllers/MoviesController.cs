using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Vidly.Migrations;
using System.Web.Mvc;
using Vidly.Models;
using Vidly.ViewModels;

namespace Vidly.Controllers
{
    public class MoviesController : Controller
    {
        private ApplicationDbContext _context;

        public MoviesController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: Movies/Random
        public ActionResult Random()
        {
            var movie = new Movie() { Name = "Shrek!" };
            var customers = new List<Customer>
            {
                new Customer { Name = "Customer 1" },
                new Customer { Name = "Customer 2" }
            };

            var viewModel = new RandomMovieViewModel
            {
                Movie = movie,
                Customers = customers
            };

            return View(viewModel);

            //return content("hello"); // sends plain string content to the application
            //return httpnotfound(); // sends an http not found error to application
            //return new emptyresult(); // sends a blank webpage to application
            //return redirecttoaction("index", "home", new { page = 1, sortby = "name" }); // redirects the user to the home page and sends the page and sortby variables to the url
        }

        //public ActionResult RandomViewBag()
        //{
        //    // This is just to show you there are other way to send data to the view
        //    // DO NOT USE VIEW DATA OR VIEW BAG, just pass data through the View()
        //    var movie = new Movie() { Name = "Shrek!" };

        //    ViewData["RandomMovie"] = movie; // Requires that you modify Random.cshtml
        //    ViewBag.Movie = movie; // Requires that you modify Random.cshtml

        //    return View(movie);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Movie movie)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new MovieFormViewModel(movie)
                {
                    Genres = _context.Genres.ToList()
                };

                return View("MovieForm", viewModel);
            }

            if (movie.Id == 0)
            {
                movie.DateAdded = DateTime.Now;
                _context.Movies.Add(movie); // This does not write customer to the database, this is just saved in local memory
            }
            else
            {
                var movieInDb = _context.Movies.Single(m => m.Id == movie.Id);
                movieInDb.Name = movie.Name;
                movieInDb.ReleaseDate = movie.ReleaseDate;
                movieInDb.GenreId = movie.GenreId;
                movieInDb.NumberInStock = movie.NumberInStock;
            }

            _context.SaveChanges(); // To persist these changes, we write the customer to the database using the SaveChanges() method

            return RedirectToAction("Index", "Movies");
        }

        public ActionResult New()
        {
            var movieGenres = _context.Genres.ToList(); // We cannot pass this to the View() method because later we want to implement editing a customer because there we will also need to pass a Customer to this view. In cases like this we need to create a ViewModel

            var viewModel = new MovieFormViewModel()
            {
                Genres = movieGenres
            };

            return View("MovieForm", viewModel);
        }

        public ActionResult Edit(int id)
        {
            var movie = _context.Movies.SingleOrDefault(m => m.Id == id);

            if (movie == null)
            {
                return HttpNotFound();
            }

            var viewModel = new MovieFormViewModel(movie)
            {
                Genres = _context.Genres.ToList()
            };

            return View("MovieForm", viewModel);
        }

        public ViewResult Index()
        {
            var movies = _context.Movies.Include(m => m.Genre).ToList();

            return View(movies);
        }

        public ActionResult Details(int id)
        {
            var movie = _context.Movies.Include(m => m.Genre).SingleOrDefault(m => m.Id == id);

            if (movie == null)
                return HttpNotFound();

            return View(movie);

        }

        //[Route("movies/released/{year:regex(\\d{4})}/{month:regex(\\d{2}):range(1,12)}")]
        //public ActionResult ByReleaseDate(int year, int month)
        //{
        //    return Content(year + "/" + month);
        //}
    }
}