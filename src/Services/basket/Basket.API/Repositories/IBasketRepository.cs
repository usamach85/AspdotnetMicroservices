using Basket.API.Entities;

namespace Basket.API.Repositories
{
	public interface IBasketRepository
	{
		//user name is for key value pair in redis storage
		Task<ShoppingCart> GetBasket(string userName);
		Task<ShoppingCart> UpdateBasket(ShoppingCart basekt);
		Task DeleteBasket(string userName);
	}
}
