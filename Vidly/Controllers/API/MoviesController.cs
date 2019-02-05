using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.WebPages;
using AutoMapper;
using Vidly.Dtos;
using Vidly.Models;

namespace Vidly.Controllers.API
{
	public class MoviesController : ApiController
	{

		private ApplicationDbContext _context;
	 

		public MoviesController()
		{
			_context = new ApplicationDbContext();
		}


		public IHttpActionResult GetMovies(string query=null)
		{//.Include(m=>m.Genre)
			//var temp = _context.Genres.ToList().ToString();
			//if(temp!="")
				//System.Diagnostics.Debug.WriteLine(temp+"  wtf");
			 
			

			var moviesquery = _context.Movie.Include(m => m.Genre).Where(m => m.NumberAvailable > 0);

			if (!String.IsNullOrWhiteSpace(query))
				moviesquery = moviesquery.Where(m => m.Name.Contains(query));

			return Ok(moviesquery);

		}

		public IHttpActionResult GetMovie(int id)
		{
			var movie = _context.Movie.SingleOrDefault(c => c.Id == id);

			if (movie == null)
				return NotFound();

			return Ok(Mapper.Map<Movie, MovieDto>(movie));
		}

		[HttpPost]
		public IHttpActionResult CreateMovie(MovieDto movieDto)
		{
			if (!ModelState.IsValid)
				return BadRequest();

			var movie = Mapper.Map<MovieDto, Movie>(movieDto);
			_context.Movie.Add(movie);
			_context.SaveChanges();

			movieDto.Id = movie.Id;
			return Created(new Uri(Request.RequestUri + "/" + movie.Id), movieDto);
		

	    }

		[HttpPut]
		public IHttpActionResult UpdateMovie(int id, MovieDto movieDto)
		{
			if (!ModelState.IsValid)
				return BadRequest();

			var movieInDb = _context.Movie.SingleOrDefault(c => c.Id == id);

			if (movieInDb == null)
				return NotFound();

			Mapper.Map(movieDto, movieInDb);

			_context.SaveChanges();

			return Ok();
		}

		[HttpDelete]
		public IHttpActionResult DeleteMovie(int id)
		{
			var movieInDb = _context.Movie.SingleOrDefault(c => c.Id == id);

			if (movieInDb == null)
				return NotFound();

			_context.Movie.Remove(movieInDb);
			_context.SaveChanges();

			return Ok();
		}

	}
	}
