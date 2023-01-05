using Npgsql;

namespace Discount.Grpc.Extensions
{
	public static class HostExtensions
	{
		//we using retry because someties maybe there will be a chance when cpntainer is not ready and we have to retry the task till the container is ready for inserting the data
		public static IHost MigrateDatabase<TContext>(this IHost host,int ? retry=0)
		{
			int retryForavailbailty = retry.Value;
			//using "using word" here because we need to get the some services from the dependency injection
			using (var scope = host.Services.CreateScope())
			{
				//storing the scope service provider means we are getting dependency injection in here.

				var services = scope.ServiceProvider; 
				//get the service of IConfiguration from the dependency injection
				var configuration=services.GetRequiredService<IConfiguration>();
				var logger = services.GetRequiredService<ILogger<TContext>>();
				//try catch because migrating needs to be done try again and again
				try
				{
					logger.LogInformation("Migrating postgresssql database");
					//creating the conenction with the postgree so that we can perform the database level operations.
					using var connection = new NpgsqlConnection(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
					connection.Open();
					//created new command with the connection so that I will use that command to insert the data into the databse
					using var command = new NpgsqlCommand()
					{
						Connection = connection
					};
					//First time when our applciation will run it will delete the table if there is another table already existing
					command.CommandText = "DROP TABLE IF EXISTS Coupon";
					command.ExecuteNonQuery();
					command.CommandText = @"CREATE TABLE Coupon(Id SERIAL PRIMARY KEY, 
                                                                ProductName VARCHAR(24) NOT NULL,
                                                                Description TEXT,
                                                                Amount INT)";
					command.ExecuteNonQuery();

					command.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('IPhone X', 'IPhone Discount', 150);";
					command.ExecuteNonQuery();

					command.CommandText = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('Samsung 10', 'Samsung Discount', 100);";
					command.ExecuteNonQuery();

					logger.LogInformation("Migrated postresql database.");

				}
				catch (NpgsqlException ex)
				{

					logger.LogError(ex, "An error occurred while migrating the postresql database");

					if (retryForavailbailty < 50)
					{
						retryForavailbailty++;
						System.Threading.Thread.Sleep(2000);
						MigrateDatabase<TContext>(host, retryForavailbailty);
					}
				}
				return host;

			}

		}
	}
}
