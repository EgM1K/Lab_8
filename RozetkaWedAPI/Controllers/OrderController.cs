using Microsoft.AspNetCore.Mvc;
using RozetkaWedAPI.Models;
using RozetkaWedAPI.Servises.Interfaces;

namespace RozetkaWedAPI.Controllers
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
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _orderService.GetAllAsync();
            return Ok(orders);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null) return NotFound();
            return Ok(order);
        }
        [HttpPost]
        public async Task<IActionResult> Add(Order order)
        {
            var newOrder = await _orderService.AddAsync(order);
            return CreatedAtAction(nameof(GetById), new { id = newOrder.Id }, newOrder);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Order updatedOrder)
        {
            var order = await _orderService.UpdateAsync(id, updatedOrder);
            if (order == null) return NotFound();
            return Ok(order);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _orderService.DeleteAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}