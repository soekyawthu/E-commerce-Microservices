using Discount.Grpc;

namespace Basket.API.GrpcServices;

public class DiscountService
{
    private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoService;

    public DiscountService(DiscountProtoService.DiscountProtoServiceClient discountProtoService)
    {
        _discountProtoService = discountProtoService;
    }

    public async Task<CouponModel> GetDiscount(string productName)
    {
        return await _discountProtoService.GetDiscountAsync(new GetDiscountRequest { ProductName = productName });
    }
}