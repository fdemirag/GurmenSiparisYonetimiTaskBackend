using Business.Abstracts;
using Business.DTOs.Customer;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        // GetAll: Tüm müşterileri listeleme
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDTO>>> GetAll()
        {
            var customers = await _customerService.GetAllAsync();
            return Ok(customers);
        }

        // GetById: ID ile müşteri sorgulama
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDTO>> GetById(int id)
        {
            var customer = await _customerService.GetByIdAsync(id);
            if (customer == null)
                return NotFound();

            return Ok(customer);
        }

        // Add: Yeni müşteri ekleme
        [HttpPost]
        public async Task<ActionResult<int>> Add(CustomerDTO customerDTO)
        {
            var id = await _customerService.AddAsync(customerDTO);
            return CreatedAtAction(nameof(GetById), new { id }, customerDTO);
        }

        // Update: Müşteri güncelleme
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CustomerDTO customerDTO)
        {
            // ID'yi path'ten alıyoruz ve DTO'yu güncelliyoruz
            var result = await _customerService.UpdateAsync(id, customerDTO);

            if (result > 0)
                return NoContent(); // Başarılı güncelleme
            else
                return NotFound(); // Müşteri bulunamadı
        }

        // Delete: Müşteri silme
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _customerService.DeleteAsync(id);

            if (result > 0)
                return NoContent(); // Başarılı silme
            else
                return NotFound(); // Müşteri bulunamadı
        }
    }
}
