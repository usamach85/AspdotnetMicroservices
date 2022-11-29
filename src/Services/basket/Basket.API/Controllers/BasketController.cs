using Basket.API.Entities;
using Basket.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers
{
	[ApiController]
	[Route("api/v1/[controller]")]
	public class BasketController : ControllerBase
	{
		private readonly IBasketRepository _basketRepository;

		public BasketController(IBasketRepository basketRepository)
		{
			_basketRepository = basketRepository ?? throw new ArgumentNullException(nameof(basketRepository));
		}
		[HttpGet("{userName}",Name ="GetBasket")]
		[ProducesResponseType(typeof(ShoppingCart),(int)HttpStatusCode.OK)]
		//The above line to explicity tell that it will always resturn now ok Response.
		public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
		{
			var basket = await _basketRepository.GetBasket(userName); 
			//if the basekt is null new shopping cart will be created with the user name
			//when client tries to add first item in basket then new basket will be created. 
			return Ok(basket ?? new ShoppingCart(userName));
		}
		[HttpPost]
		[ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
		public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart basket)
		{

			return Ok(await _basketRepository.UpdateBasket(basket));
		}

		[HttpDelete("{userName}", Name = "DeleteBasket")]
		[ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
		public async Task<IActionResult> DeleteBasket(string userName)
		{
			await _basketRepository.DeleteBasket(userName);
			return Ok();
		}
	}
}
