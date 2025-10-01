using Microsoft.AspNetCore.Mvc;
using OrderAPI.Application.DTOs;
using OrderAPI.Application.Interfaces;

namespace OrderAPI.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet(Name = "AddOccurrenceOrder")]
        public void AddOccurrence(OrderRequestDto request)
        {
            var order = _orderService.GetOrderByIdAsync(request.OrderNumber);

        }

    }
}
