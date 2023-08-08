using System.Data;
using Dapper;
using Discount.API.Entities;

namespace Discount.API.Repositories;

public class DiscountRepository : IDiscountRepository
{
    private readonly IDbConnection _connection;

    public DiscountRepository(IDbConnection connection)
    {
        _connection = connection;
    }
    
    public async Task<Coupon> GetDiscount(string productName)
    {
        var coupon = await _connection.QueryFirstOrDefaultAsync<Coupon>
            ("SELECT * FROM Coupon WHERE ProductName = @ProductName", new { ProductName = productName });

        return coupon ?? new Coupon { ProductName = "No Discount", Amount = 0, Description = "No Discount Desc" };
    }

    public async Task<bool> CreateDiscount(Coupon coupon)
    {
        var affected = await _connection.ExecuteAsync
            ("INSERT INTO Coupon (ProductName, Description, Amount) VALUES (@ProductName, @Description, @Amount)",
                new { coupon.ProductName, coupon.Description, coupon.Amount });

        return affected != 0;
    }

    public async Task<bool> UpdateDiscount(Coupon coupon)
    {
        var affected = await _connection.ExecuteAsync
        ("UPDATE Coupon SET ProductName=@ProductName, Description = @Description, Amount = @Amount WHERE Id = @Id",
            new { coupon.ProductName, coupon.Description, coupon.Amount, coupon.Id });

        return affected != 0;
    }

    public async Task<bool> DeleteDiscount(string productName)
    {
        var affected = await _connection.ExecuteAsync("DELETE FROM Coupon WHERE ProductName = @ProductName",
            new { ProductName = productName });

        return affected != 0;
    }
}