using Catalog.Api.Entities;
using MongoDB.Driver;

namespace Catalog.Api.Data
{
	//it will inherit the interface. actual busniess logic will be here
	public class CatalogContext: ICatalogContext
	{
		//will initlize the connection with mongodb database
		//configuration will used to inject configuration to any class
		public CatalogContext(IConfiguration configuration)
		{
			//driver will provide the mongoclient to connect to mongodb
			var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
			//get database will create new database of name which we are passing if there is no existing databse
			var database = client.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName"));

			//acessing my products list from interface and filling it

			Products = database.GetCollection<Product>(configuration.GetValue<string>("DatabaseSettings:CollectionName"));
			CatalogContextSeed.SeedData(Products);
		}

		public IMongoCollection<Product> Products { get; }
	}
}
