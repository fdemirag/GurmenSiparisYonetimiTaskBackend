using Business.Abstracts;
using Business.DTOs.Campaign;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampaignsController : ControllerBase
    {
        private readonly ICampaignService _campaignService;

        public CampaignsController(ICampaignService campaignService)
        {
            _campaignService = campaignService;
        }

        // GetAll: Tüm kampanyaları listeleme
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampaignDTO>>> GetAll()
        {
            var campaigns = await _campaignService.GetAllAsync();
            return Ok(campaigns);
        }

        // Add: Yeni kampanya ekleme
        [HttpPost]
        public async Task<ActionResult<int>> Add(CampaignDTO campaignDTO)
        {
            var id = await _campaignService.AddAsync(campaignDTO);
            return CreatedAtAction(nameof(GetAll), new { id }, campaignDTO);
        }

        // ApplyDiscount: Kampanya indirimini uygula
        [HttpPost("apply/{orderId}")]
        public async Task<IActionResult> ApplyDiscount(int orderId, [FromQuery] decimal totalAmount)
        {
            var result = await _campaignService.ApplyDiscountAsync(orderId, totalAmount);

            if (result > 0)
                return Ok("Discount applied successfully.");
            else
                return BadRequest("No active campaign or discount criteria not met.");
        }
    }
}
