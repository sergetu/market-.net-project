using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReceiptsController : ControllerBase
    {
        private readonly IReceiptService _receiptService;

        public ReceiptsController(IReceiptService receiptService)
        {
            _receiptService = receiptService;
        }

        // GET: api/receipts
        [HttpGet]
        public async Task<IActionResult> GetAllReceipts()
        {
            try
            {
                var receipts = await _receiptService.GetAllAsync();
                return Ok(receipts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/receipts/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetReceiptById(int id)
        {
            try
            {
                var receipt = await _receiptService.GetByIdAsync(id);
                if (receipt == null)
                {
                    return NotFound();
                }
                return Ok(receipt);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/receipts/{id}/details
        [HttpGet("{id}/details")]
        public async Task<IActionResult> GetReceiptDetails(int id)
        {
            try
            {
                var details = await _receiptService.GetReceiptDetailsAsync(id);
                return Ok(details);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/receipts/{id}/sum
        [HttpGet("{id}/sum")]
        public async Task<IActionResult> GetReceiptSum(int id)
        {
            try
            {
                var sum = await _receiptService.ToPayAsync(id);
                return Ok(sum);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/receipts/period
        [HttpGet("period")]
        public async Task<IActionResult> GetReceiptsByPeriod([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            try
            {
                var receipts = await _receiptService.GetReceiptsByPeriodAsync(startDate, endDate);
                return Ok(receipts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST: api/receipts
        [HttpPost]
        public async Task<IActionResult> AddReceipt([FromBody] ReceiptModel receiptModel)
        {
            try
            {
                if (receiptModel == null)
                {
                    return BadRequest("Invalid receipt model");
                }

                await _receiptService.AddAsync(receiptModel);
                return CreatedAtAction(nameof(GetReceiptById), new { id = receiptModel.Id }, receiptModel);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/receipts/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReceipt(int id, [FromBody] ReceiptModel receiptModel)
        {
            try
            {
                if (receiptModel == null || receiptModel.Id != id)
                {
                    return BadRequest("Receipt model is null or ID mismatch");
                }

                var existingReceipt = await _receiptService.GetByIdAsync(id);
                if (existingReceipt == null)
                {
                    return NotFound();
                }

                await _receiptService.UpdateAsync(receiptModel);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/receipts/{id}/products/add/{productId}/{quantity}
        [HttpPut("{receptId}/products/add/{productId}/{quantity}")]
        public async Task<IActionResult> AddProductToReceipt(int receptId, int productId, int quantity)
        {
            try
            {
                await _receiptService.AddProductAsync(productId, receptId, quantity);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/receipts/{id}/products/remove/{productId}/{quantity}
        [HttpPut("{id}/products/remove/{productId}/{quantity}")]
        public async Task<IActionResult> RemoveProductFromReceipt(int id, int productId, int quantity)
        {
            try
            {
                await _receiptService.RemoveProductAsync(productId, id, quantity);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/receipts/{id}/checkout
        [HttpPut("{id}/checkout")]
        public async Task<IActionResult> CheckOutReceipt(int id)
        {
            try
            {
                await _receiptService.CheckOutAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/receipts/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReceipt(int id)
        {
            try
            {
                var existingReceipt = await _receiptService.GetByIdAsync(id);
                if (existingReceipt == null)
                {
                    return NotFound();
                }

                await _receiptService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }

}
