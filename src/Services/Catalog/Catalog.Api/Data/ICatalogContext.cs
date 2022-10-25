using Catalog.Api.Entities;
using MongoDB.Driver;

namespace Catalog.Api.Data
{
	//so this is a data access layer in which we define regarding the context class what function will be implemented just like 
	//we will define list here so that any calss will get it
	public interface ICatalogContext
	{
		IMongoCollection<Product> Products { get; }
	}
}
