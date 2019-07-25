using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Dapper.UnitOfWork.Example.Data.Commands
{
    public class AddPersonCommand : ICommand, IAsyncCommand
    {
        private const string Sql = @"
            INSERT INTO People(
				Name,
				Age,
                Address_Id)
			VALUES(
				@Name,
				@Age,
                @Address_Id);
		";

        private readonly PersonEntity _entity;

        public bool RequiresTransaction => true;

        public AddPersonCommand(PersonEntity entity)
            => _entity = entity;

        public void Execute(IDbConnection connection, IDbTransaction transaction)
            => connection.Execute(Sql, new { _entity.Name, _entity.Age, Address_Id = _entity.Address.Id }, transaction);

        public Task ExecuteAsync(IDbConnection connection, IDbTransaction transaction, CancellationToken cancellationToken = default)
            => connection.ExecuteAsync(Sql, new { _entity.Name, _entity.Age, Address_Id = _entity.Address.Id }, transaction);
    }
}
