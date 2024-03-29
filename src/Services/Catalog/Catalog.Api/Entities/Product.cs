﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.Api.Entities
{
	public class Product
	{
		//Cause Bason id is used in Mongodb so thats why we specify bson id
		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }
		[BsonElement]
		public string Name { get; set; }
		public string Category { get; set; }
		public string Summary{ get; set; }
		public string Description { get; set; }
		public string ImageFile { get; set; }
		public decimal Price { get; set; }
	}
}
