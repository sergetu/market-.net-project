using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: api/products
        // GET: api/products?categoryId=1&minPrice=20&maxPrice=50
        [HttpGet]
        public async Task<IActionResult> GetAllProducts([FromQuery] int? categoryId, [FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice)
        {
            if (categoryId.HasValue || minPrice.HasValue || maxPrice.HasValue)
            {
                minPrice ??= 0;

                var filter = new FilterSearchModel
                {
                    CategoryId = categoryId,
                    MinPrice = (int)minPrice,
                    MaxPrice = (int)maxPrice
                };
                var filteredProducts = await _productService.GetByFilterAsync(filter);
                return Ok(filteredProducts);
            }
            else
            {
                var allProducts = await _productService.GetAllAsync();
                return Ok(allProducts);
            }
        }

        // GET: api/products/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }


        // POST: api/products
        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] ProductModel productModel)
        {
            if (productModel == null)
            {
                return BadRequest();
            }
            try { 
            await _productService.AddAsync(productModel);
            }
            catch
            {
                return BadRequest();
            }
            return CreatedAtAction(nameof(GetProductById), new { id = productModel.Id }, productModel);
        }

        // PUT: api/products/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductModel productModel)
        {
            if (productModel == null || productModel.Id != id)
            {
                return BadRequest();
            }

            var existingProduct = await _productService.GetByIdAsync(id);
            if (existingProduct == null)
            {
                return NotFound();
            }
            try
            {
                await _productService.UpdateAsync(productModel);
            }
            catch
            {

                return BadRequest();
            }
            
            return NoContent();
        }

        // DELETE: api/products/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var existingProduct = await _productService.GetByIdAsync(id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            await _productService.DeleteAsync(id);
            return NoContent();
        }

        // GET: api/products/categories
        [HttpGet("categories")]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _productService.GetAllProductCategoriesAsync();
            return Ok(categories);
        }

        // POST: api/products/categories
        [HttpPost("categories")]
        public async Task<IActionResult> AddCategory([FromBody] ProductCategoryModel categoryModel)
        {
            if (categoryModel == null)
            {
                return BadRequest();
            }
            try
            {
                await _productService.AddCategoryAsync(categoryModel);
            }
            catch
            {
                return BadRequest();
            }
            

            return CreatedAtAction(nameof(GetAllCategories), categoryModel);
        }

        // PUT: api/products/categories/{id}
        [HttpPut("categories/{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] ProductCategoryModel categoryModel)
        {
            if (categoryModel == null || categoryModel.Id != id)
            {
                return BadRequest();
            }

            var existingCategory = await _productService.GetByIdAsync(id);
            if (existingCategory == null)
            {
                return NotFound();
            }

            await _productService.UpdateCategoryAsync(categoryModel);
            return NoContent();
        }

        // DELETE: api/products/categories/{id}
        [HttpDelete("categories/{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var existingCategory = await _productService.GetByIdAsync(id);
            if (existingCategory == null)
            {
                return NotFound();
            }

            await _productService.RemoveCategoryAsync(id);
            return NoContent();
        }
    }
}
