using Business.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticService _statisticService;

        public StatisticsController(IStatisticService statisticService)
        {
            _statisticService = statisticService;
        }

        // GET: api/statistic/popularProducts?productCount=2
        [HttpGet("popularProducts")]
        public async Task<IActionResult> GetMostPopularProducts([FromQuery] int productCount)
        {
            try
            {
                var products = await _statisticService.GetMostPopularProductsAsync(productCount);
                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/statistic/customer/{id}/{productCount}
        [HttpGet("customer/{id}/{productCount}")]
        public async Task<IActionResult> GetCustomerMostFavouriteProducts(int id, int productCount)
        {
            try 
            {
                var products = await _statisticService.GetCustomersMostPopularProductsAsync(productCount, id);
                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/statistic/activity/{customerCount}?startDate=2020-7-21&endDate=2020-7-22
        [HttpGet("activity/{customerCount}")]
        public async Task<IActionResult> GetMostActiveCustomers(int customerCount, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var customers = await _statisticService.GetMostValuableCustomersAsync(customerCount, startDate, endDate);
                return Ok(customers);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/statistic/income/{categoryId}?startDate=2020-7-21&endDate=2020-7-22
        [HttpGet("income/{categoryId}")]
        public async Task<IActionResult> GetIncomeOfCategoryInPeriod(int categoryId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var income = await _statisticService.GetIncomeOfCategoryInPeriod(categoryId, startDate, endDate);
                return Ok(income);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
