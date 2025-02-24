using Business.Abstracts;
using Business.DTOs.OrderDetail;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class OrderDetailsController : ControllerBase
{
    private readonly IOrderDetailService _orderDetailService;

    public OrderDetailsController(IOrderDetailService orderDetailService)
    {
        _orderDetailService = orderDetailService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderDetailDTO>>> GetAll()
    {
        return Ok(await _orderDetailService.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OrderDetailDTO>> GetById(int id)
    {
        return Ok(await _orderDetailService.GetByIdAsync(id));
    }

    [HttpPost]
    public async Task<ActionResult<int>> Add(OrderDetailDTO orderDetailDTO)
    {
        return Ok(await _orderDetailService.AddAsync(orderDetailDTO));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, OrderDetailDTO orderDetailDTO)
    {
        await _orderDetailService.UpdateAsync(id, orderDetailDTO);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _orderDetailService.DeleteAsync(id);
        return NoContent();
    }
}
