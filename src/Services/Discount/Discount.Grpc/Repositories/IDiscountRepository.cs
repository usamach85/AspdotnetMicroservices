using Discount.Grpc.Entities;

namespace Discount.Grpc.Repositories
{
	public interface IDiscountRepository
	{
		//Return the Coupon information of product from the product table
		Task<Coupon> GetDiscount(string productName);
		Task<bool> CreateDiscount(Coupon coupon);
		Task<bool> UpdateDiscount(Coupon coupon);
		Task<bool> DeleteDiscount(string productName);
	}
}
