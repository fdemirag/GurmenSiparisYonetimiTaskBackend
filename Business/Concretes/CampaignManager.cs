using AutoMapper;
using Business.Abstracts;
using Business.DTOs.Campaign;
using Dapper;
using DataAccess;
using Entities.Concretes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class CampaignManager : ICampaignService
{
    private readonly DapperContext _context;
    private readonly IMapper _mapper;

    public CampaignManager(DapperContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CampaignDTO>> GetAllAsync()
    {
        using var connection = _context.CreateConnection();
        var sql = "SELECT * FROM campaigns";
        var campaigns = await connection.QueryAsync<Campaign>(sql);
        return _mapper.Map<IEnumerable<CampaignDTO>>(campaigns);
    }

    public async Task<int> AddAsync(CampaignDTO campaignDTO)
    {
        using var connection = _context.CreateConnection();
        var campaign = _mapper.Map<Campaign>(campaignDTO);
        var sql = "INSERT INTO campaigns (code, discountRate, minimumAmount, expirationDate) VALUES (@Code, @DiscountRate, @MinimumAmount, @ExpirationDate) RETURNING id";
        return await connection.ExecuteScalarAsync<int>(sql, campaign);
    }

    // Ürün için kampanya uygulama (indirim hesaplama)
    public async Task<decimal> ApplyDiscountAsync(int productId, decimal unitPrice)
    {
        using var connection = _context.CreateConnection();

        // Geçerli kampanyayı al
        var campaign = await connection.QueryFirstOrDefaultAsync<Campaign>(
            "SELECT * FROM campaigns WHERE expirationDate > @Now AND minimumAmount <= @UnitPrice ORDER BY expirationDate ASC LIMIT 1",
            new { Now = DateTime.Now, UnitPrice = unitPrice });

        if (campaign != null)
        {
            // Kampanya varsa, indirim hesapla
            decimal discountAmount = unitPrice * campaign.DiscountRate;
            decimal newPrice = unitPrice - discountAmount;
            return newPrice; // İndirim uygulanmış fiyatı döndür
        }

        return unitPrice; // Kampanya uygulanmazsa orijinal fiyatı döndür
    }

    // Siparişe kampanya indirimini uygulama
    public async Task<int> ApplyDiscountToOrderAsync(int orderId, decimal totalAmount)
    {
        using var connection = _context.CreateConnection();

        // Kampanya geçerliliğini kontrol et
        var campaign = await connection.QueryFirstOrDefaultAsync<Campaign>(
            "SELECT * FROM campaigns WHERE expirationDate > @Now AND minimumAmount <= @TotalAmount ORDER BY expirationDate ASC LIMIT 1",
            new { Now = DateTime.Now, TotalAmount = totalAmount });

        if (campaign != null)
        {
            // Kampanya uygula
            decimal discountedAmount = totalAmount * campaign.DiscountRate;
            decimal newTotalAmount = totalAmount - discountedAmount;

            // Siparişi güncelle
            var updateSql = "UPDATE orders SET totalAmount = @NewTotalAmount, campaignCode = @CampaignCode WHERE id = @OrderId";
            await connection.ExecuteAsync(updateSql, new { NewTotalAmount = newTotalAmount, CampaignCode = campaign.Code, OrderId = orderId });

            return 1; // İndirim başarılı
        }

        return 0; // Kampanya uygulanmadı
    }
}
