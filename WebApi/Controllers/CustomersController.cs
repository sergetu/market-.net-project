using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Interfaces;
using Business.Models;
using Business.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        // GET: api/customers
        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {
            try
            {
                var customers = await _customerService.GetAllAsync();
                return Ok(customers);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/customers/products/{id}
        [HttpGet("products/{id}")]
        public async Task<IActionResult> GetCustomersByProductId(int id)
        {
            try
            {
                var customers = await _customerService.GetCustomersByProductIdAsync(id);
                return Ok(customers);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/customers/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            try
            {
                var customer = await _customerService.GetByIdAsync(id);
                if (customer == null)
                {
                    return NotFound();
                }
                return Ok(customer);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/customers
        [HttpPost]
        public async Task<IActionResult> AddCustomer([FromBody] CustomerModel customerModel)
        {
            try
            {
                if (customerModel == null)
                {
                    return BadRequest("Invalid customer model");
                }
                //discount value should be between 0 and 100
                if (customerModel.DiscountValue < 0 || customerModel.DiscountValue > 100)
                {
                    return BadRequest("Discount value should be between 0 and 100");
                }
                await _customerService.AddAsync(customerModel);
                return CreatedAtAction(nameof(GetCustomerById), new { id = customerModel.Id }, customerModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/customers/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] CustomerModel customerModel)
        {
            try
            {
                if (customerModel == null || customerModel.Id != id)
                {
                    return BadRequest("Customer model is null or ID mismatch");
                }

                var existingCustomer = await _customerService.GetByIdAsync(id);
                if (existingCustomer == null)
                {
                    return NotFound();
                }

                //discount value should be between 0 and 100
                if (customerModel.DiscountValue < 0 || customerModel.DiscountValue > 100)
                {
                    return BadRequest("Discount value should be between 0 and 100");
                }

                await _customerService.UpdateAsync(customerModel);
                return NoContent();
            }
             catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/customers/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            try
            {
                var existingCustomer = await _customerService.GetByIdAsync(id);
                if (existingCustomer == null)
                {
                    return NotFound();
                }

                await _customerService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
