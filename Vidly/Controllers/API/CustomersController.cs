using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Vidly.Dtos;
using Vidly.Models;

namespace Vidly.Controllers.API
{
	public class CustomersController : ApiController
	{
		private ApplicationDbContext _context;

		public CustomersController()
		{
			_context=new ApplicationDbContext();
		}

		// GET /api/customers
		public IHttpActionResult GetCustomers(string Query = null)
		{
			var customerQuery = _context.Customers.Include(c => c.MembershipType);
		
		if (!String.IsNullOrWhiteSpace(Query))
		{
			customerQuery = customerQuery.Where(c => c.Name.Contains(Query));
		}

var customerDtos=customerQuery.ToList().Select(Mapper.Map<Customer, CustomerDto>);


return Ok(customerDtos);

		}

		// GET /api/customers/1
		public IHttpActionResult GetCustomer(int id)
		{
			var customer = _context.Customers.SingleOrDefault(c => c.Id == id);

			if (customer == null)
			{
				return NotFound();
			}

			return Ok(Mapper.Map<Customer, CustomerDto>(customer)); 
		}

		//POST /api/customers
		[HttpPost]
		public IHttpActionResult CreateCustomer(CustomerDto customerdto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}

			var customer = Mapper.Map<CustomerDto, Customer>(customerdto);
			_context.Customers.Add(customer);
			_context.SaveChanges();

			customerdto.Id = customer.Id;

			return Created(new Uri(Request.RequestUri+"/"+customer.Id),customerdto );
		}

		//PUT /api/customers/1
		[HttpPut]
		public IHttpActionResult UpdateCustomer(int id, CustomerDto customerdto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}

			var customerInDb = _context.Customers.SingleOrDefault(c => c.Id == id);

			if (customerInDb == null)
			{
				return NotFound();
			}


			Mapper.Map(customerdto, customerInDb);
		 

			_context.SaveChanges();

			return Ok();
		}

		//DELETE /api/customers/1
		[HttpDelete]
		public IHttpActionResult DeleteCustomer(int id)
		{
			var customerInDb = _context.Customers.SingleOrDefault(c => c.Id == id);

			if (customerInDb == null)
			{
				return NotFound();
			}

			_context.Customers.Remove(customerInDb);
			_context.SaveChanges();
			return Ok();
		}
	}
}
