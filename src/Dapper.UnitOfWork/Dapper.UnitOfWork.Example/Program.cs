using System;
using System.Linq;
using System.Threading.Tasks;
using Dapper.UnitOfWork.Example.Data;
using Dapper.UnitOfWork.Example.Data.Commands;
using Dapper.UnitOfWork.Example.Data.Queries;

namespace Dapper.UnitOfWork.Example
{
	class Program
    {
        private const string ConnectionString = @"Data Source=.\SQLExpress;database=NORTHWND;Integrated Security=true";

		static void Main(string[] args)
		{
			var factory = new UnitOfWorkFactory(ConnectionString);

			PrintCustomer("ALFKI");

			const string newCustomerId = "OUTM";

			// data manipulation with explicit transaction
			using (var uow = factory.Create(true))
			{
				uow.Execute(new DeleteCustomerCommand(newCustomerId));
				uow.Execute(new AddCustomerCommand(new CustomerEntity
				{
					CustomerId = newCustomerId,
					CompanyName = "Outmatic"
				}));

				uow.Commit(); // don't forget to explicitly commit the transaction, otherwise it will get rolled back
			}

			PrintCustomer(newCustomerId);

            Task.Run(async()=> await MainAsync(args));

			Console.WriteLine("Press any key to exit");
			Console.ReadKey();

			void PrintCustomer(string customerId)
			{
				// query
				using (var uow = factory.Create())
				{
                    // simple query
					var customer = uow.Query(new GetCustomerByIdQuery(customerId));

					Console.WriteLine($"Retrieved: {customer.CompanyName}");

                    // query with lazy loading
                    var person = uow.Query(new GetPersonByIdQuery(1));

                    Console.WriteLine($"Person: {person.Name} ({person.Address.Street})");
                }
			}
		}

        private static async Task MainAsync(string[] args)
        {
            var factory = new UnitOfWorkFactory(ConnectionString);

            using (var uow = await factory.CreateAsync(retryOptions:new RetryOptions(5, 100, new SqlTransientExceptionDetector())))
            {
                var customer = await uow.QueryAsync(new GetCustomerByIdQuery("ALFKI"));

                Console.WriteLine($"Retrieved asynchronously: {customer.CompanyName}");

                // query the table for one result
                var people = await uow.QueryAsyncList(new GetPersonByIdQuery(1));
                Console.WriteLine($"Person: {people.FirstOrDefault()?.Name} ({people.FirstOrDefault()?.Address?.Street})");
            }
        }
    }
}