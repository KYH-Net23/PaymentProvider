using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentProvider.Models;
using PaymentProvider.Services;

namespace PaymentProvider.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController(OrderService service) : ControllerBase
    {
        private readonly OrderService _service = service;

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDetails?>> GetOrder(int id)
        {
            try
            {
                var order = await _service.GetOrderDetailsAsync(id);
                if (order == null) return NotFound();

                return Ok(order);
            }
            catch
            {
                return BadRequest(null!);
            }
        }
    }
}
