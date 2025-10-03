using Microsoft.AspNetCore.Mvc;
using OrderAPI.Application.DTOs.Requests;
using OrderAPI.Application.Interfaces;

namespace OrderAPI.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _service;

        public OrdersController(IOrderService orderService)
        {
            _service = orderService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _service.CreateOrderAsync(request);

            return Created();
        }
        [HttpGet("id")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            try
            {
                var order = await _service.GetOrderByIdAsync(id);
                return Ok(order);
            }
            catch(Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            return Ok(await _service.GetAllOrdersAsync());
        }
        [HttpPost("{id}/occurrences")]
        public async Task<IActionResult> AddOccurrence(int id, [FromBody] AddOccurrenceRequestDto request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                await _service.AddOccurrenceAsync(id, request);

                return Ok();
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}/occurrences/{occurrenceId}")]
        public async Task<IActionResult> DeleteOccurrence(int id, int occurrenceId)
        {
            await _service.DeleteOccurrenceAsync(id, occurrenceId);
            return NoContent();
        }
    }
}
