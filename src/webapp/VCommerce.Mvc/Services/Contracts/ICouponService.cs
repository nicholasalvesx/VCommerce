using VCommerce.Mvc.Models;

namespace VCommerce.Mvc.Services.Contracts;

public interface ICouponService
{
    Task<CouponViewModel?> GetDiscountCoupon(string couponCode, string token);
}
