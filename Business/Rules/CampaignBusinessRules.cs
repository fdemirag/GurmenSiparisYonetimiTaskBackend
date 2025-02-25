using Core.Business.Rules;
using Dapper;
using DataAccess;
using Entities.Concretes;

public class CampaignBusinessRules:BaseBusinessRules
{
    private readonly DapperContext _context;

    public CampaignBusinessRules(DapperContext context)
    {
        _context = context;
    }

    public async Task<decimal> ApplyCampaignDiscountAsync(decimal totalAmount)
    {
        using var connection = _context.CreateConnection();

        // Uygulanabilir kampanya kontrolü
        var applicableCampaign = await connection.QueryFirstOrDefaultAsync<Campaign>(
            "SELECT * FROM campaigns WHERE minimumAmount <= @TotalAmount AND expirationDate >= @CurrentDate ORDER BY minimumAmount DESC LIMIT 1",
            new { TotalAmount = totalAmount, CurrentDate = DateTime.Now });

        if (applicableCampaign != null)
        {
            decimal discountedAmount = totalAmount * (1 - applicableCampaign.DiscountRate);
            return discountedAmount;
        }

        return totalAmount; // Kampanya yoksa orijinal tutar döner
    }
}
