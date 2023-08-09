using Discount.Grpc.Entities;
using Discount.Grpc.Repositories;
using Grpc.Core;

namespace Discount.Grpc.Services;

public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
{
    private readonly ILogger<DiscountService> _logger;
    private readonly IDiscountRepository _repository;

    public DiscountService(ILogger<DiscountService> logger, IDiscountRepository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var coupon = await _repository.GetDiscount(request.ProductName);
        _logger.LogInformation("Discount is retrieved for ProductName : {ProductName}, Amount : {Amount}", coupon.ProductName, coupon.Amount);
        
        return new CouponModel
        {
            Id = coupon.Id,
            ProductName = coupon.ProductName,
            Description = coupon.Description,
            Amount = coupon.Amount
        };
    }

    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        var coupon = new Coupon
        {
            ProductName = request.Coupon.ProductName,
            Description = request.Coupon.Description,
            Amount = request.Coupon.Amount
        };
        
        await _repository.CreateDiscount(coupon);
        _logger.LogInformation("Discount is successfully created. ProductName : {ProductName}", coupon.ProductName);

        return new CouponModel
        {
            Id = coupon.Id,
            ProductName = coupon.ProductName,
            Description = coupon.Description,
            Amount = coupon.Amount
        };
    }

    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var coupon = new Coupon
        {
            Id = request.Coupon.Id,
            ProductName = request.Coupon.ProductName,
            Description = request.Coupon.Description,
            Amount = request.Coupon.Amount
        };
        
        await _repository.UpdateDiscount(coupon);
        _logger.LogInformation("Discount is successfully updated. ProductName : {ProductName}", coupon.ProductName);

        return new CouponModel
        {
            Id = coupon.Id,
            ProductName = coupon.ProductName,
            Description = coupon.Description,
            Amount = coupon.Amount
        };
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        var isSuccess = await _repository.DeleteDiscount(request.ProductName);
        _logger.LogInformation("Discount is successfully deleted. ProductName : {ProductName}", request.ProductName);

        return new DeleteDiscountResponse { Success = isSuccess };
    }
}