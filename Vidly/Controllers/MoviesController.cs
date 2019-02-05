using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Vidly.Models;
using Vidly.ViewModels;

namespace Vidly.Controllers
{
	public class MoviesController : Controller
	{
		// GET: Movies/Random

		private ApplicationDbContext _context;

		public MoviesController()
		{
			_context = new ApplicationDbContext();
		}

		protected override void Dispose(bool disposing)
		{
			_context.Dispose();
		}


		public ViewResult Index()
		{
			//var movies = GetMovies();

	 var moviesquery = _context.Movie.Include(m => m.Genre).ToList();

 

	 if (User.IsInRole(RoleName.CanManageMovies))
	 {  return View("List", moviesquery);}


			return View("ReadOnlyList",moviesquery);

		 //	return View();
		}

		[Authorize(Roles = RoleName.CanManageMovies)]
		public ActionResult NewMovie()
		{
			var genres = _context.Genres.ToList();

			var viewModel = new NewMovieViewModel
			{
				 
				Genres = genres
			};
			return View( "NewMovie", viewModel);
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = RoleName.CanManageMovies)]
		public ActionResult Save(Movie movie)
		{

			if (!ModelState.IsValid)
			{
				var viewModel = new NewMovieViewModel(movie)
				{
					
					Genres = _context.Genres.ToList()
				};

				return View("NewMovie", viewModel);
			}

			if (movie.Id == 0)
			{
			movie.DateAdded=DateTime.Now;
			movie.NumberAvailable = movie.NumberInStock;
				_context.Movie.Add(movie);
			}
		 	else
			{
				var movieInDb = _context.Movie.Single(m => m.Id == movie.Id);

				movieInDb.Name = movie.Name;
				movieInDb.ReleaseDate = movie.ReleaseDate;
				movieInDb.GenreId = movie.GenreId;
				movieInDb.NumberInStock = movie.NumberInStock;
				movieInDb.NumberAvailable = movie.NumberInStock;
			}

		 
				_context.SaveChanges();
		 

			return RedirectToAction("Index", "Movies");
		}

		[Authorize(Roles = RoleName.CanManageMovies)]
		public ActionResult Edit(int id)
		{
			var movie = _context.Movie.SingleOrDefault(c => c.Id == id);
			if (movie == null)
				return HttpNotFound();

			var viewModel = new NewMovieViewModel(movie)
			{ 
				Genres = _context.Genres.ToList()

			};

			return View("NewMovie", viewModel);
		}




		[Authorize(Roles = RoleName.CanManageMovies)]
		public ActionResult Details(int id)

		{
			//var customer = _context.Customers.SingleOrDefault(c => c.Id == id);

			var movie = _context.Movie.Include(m => m.Genre).SingleOrDefault(m => m.Id == id);
			if (movie == null)
				return HttpNotFound();

			return View(movie);
		}
		//return Content("Hello World !");
		//return HttpNotFound();
		//	return new EmptyResult();
		//	return  RedirectToAction("Index","Home",new {page =1,sortBy="name"});

		/*
	public ActionResult Edit(int id)
	{
		return Content("ID=" + id);

	}

	//Movies
	public ActionResult Index(int? pageIndex, string sortBy)
	{
		if (!pageIndex.HasValue)
		{

			pageIndex = 1;
		}

		if (sortBy.IsNullOrWhiteSpace())
		{
			sortBy = "Name";
		}

		return Content(String.Format("pageIndex={0}&sortBy={1}", pageIndex, sortBy));
	}

	[Route("movies/released/{year}/{month:regex(\\d{2}):range(1,12)}")]
	public ActionResult ByReleaseDate(int year, int month)
	{

		return Content(year+"/"+month);
	}*/
	}
}