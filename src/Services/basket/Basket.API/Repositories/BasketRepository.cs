using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Basket.API.Repositories
{
	public class BasketRepository:IBasketRepository
	{
		private readonly IDistributedCache _redisCache;

		public BasketRepository(IDistributedCache redisCache)
		{
			_redisCache = redisCache ?? throw new ArgumentNullException(nameof(redisCache));
		}

		public async Task DeleteBasket(string userName)
		{
			await _redisCache.RemoveAsync(userName);
		}

		public async Task<ShoppingCart> GetBasket(string userName)
		{
			var basket = await _redisCache.GetStringAsync(userName);
			if (string.IsNullOrEmpty(basket))
				return null;
			return JsonConvert.DeserializeObject<ShoppingCart>(basket);
		}

		public async Task<ShoppingCart> UpdateBasket(ShoppingCart basket)
		{
			//we are using key value pair in redis cache so thats why add and update is using in same method we will use set async with
			//key value pair means that we are using username for a whole shopping cart as a key so thats why for key value of user we will set a whole json object basket
			// this method will override all the methods like for replacing updating and adding depending upon the key 
			await _redisCache.SetStringAsync(basket.UserName, JsonConvert.SerializeObject(basket));
			return await GetBasket(basket.UserName);
		}
	}
}
