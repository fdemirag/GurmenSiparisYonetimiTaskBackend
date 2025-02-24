using Business.Abstracts;
using Business.DTOs.Order;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderDTO>>> GetAll()
    {
        return Ok(await _orderService.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderDTO>> GetById(int id)
    {
        return Ok(await _orderService.GetByIdAsync(id));
    }

    [HttpPost]
    public async Task<ActionResult<int>> Add(OrderDTO orderDTO)
    {
        return Ok(await _orderService.AddAsync(orderDTO));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, OrderDTO orderDTO)
    {
        await _orderService.UpdateAsync(id, orderDTO);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _orderService.DeleteAsync(id);
        return NoContent();
    }
}
