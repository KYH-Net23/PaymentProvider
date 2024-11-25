using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentProvider.Entities;
using PaymentProvider.Services;

namespace PaymentProvider.Controllers
{
    [Route("orders")]
    [ApiController]
    public class OrdersController(OrderService service) : ControllerBase
    {
        private readonly OrderService _service = service;

        [HttpGet]
        public async Task<ActionResult<OrderEntity>> Get(int id)
        {
            var order = await _service.GetAsync(id);
            if (order == null)
                return NotFound("Order was not found");
            return Ok(order);
        }
    }
}