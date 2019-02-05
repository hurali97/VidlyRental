using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Vidly.Dtos;
using Vidly.Models;

namespace Vidly.Controllers.API
{
	public class NewRentalController : ApiController
	{

		private ApplicationDbContext _context;


		public NewRentalController()
		{
			_context = new ApplicationDbContext();
		}


		[HttpPost]
		public IHttpActionResult CreateNewRentals(NewRentalDto newRental)
		{
			var customer = _context.Customers.Single(c => c.Id == newRental.CustomerId);

			var movies = _context.Movie.Where(m => newRental.MovieIds.Contains(m.Id));

			foreach (var movie in movies)
			{
				if (movie.NumberAvailable == 0)
					return BadRequest("Movie is not Available.");



				movie.NumberAvailable--;
				var rental = new Rental
				{
					Customer=customer,
					Movie=movie,
					DateRented=DateTime.Now

				};
				_context.Rentals.Add(rental);
			}

			_context.SaveChanges();
			return Ok();

		//	throw new NotImplementedException();
		}
	}
}
