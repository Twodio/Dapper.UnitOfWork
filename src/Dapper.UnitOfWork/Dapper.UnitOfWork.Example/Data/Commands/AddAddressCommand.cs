using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Dapper.UnitOfWork.Example.Data.Commands
{
    public class AddAddressCommand : ICommand, IAsyncCommand
    {
        private const string Sql = @"
            DECLARE @Ident TABLE(n INT);
			INSERT INTO Addresses(
				Street,
				Region)
            OUTPUT INSERTED.id INTO @Ident(n)
			VALUES(
				@Street,
				@Region); SELECT CAST(n as int) FROM @Ident;
		";

        private AddressEntity _entity;

        // Set this to true prevents invoking the command without an explicit transaction
        public bool RequiresTransaction => true;

        public AddAddressCommand(ref AddressEntity entity)
            => _entity = entity;

        // exeutes the query and assign the returned id to the object we passed in the constructor
        public void Execute(IDbConnection connection, IDbTransaction transaction)
            => _entity.Id = connection.ExecuteScalar<int>(Sql, _entity, transaction);

        public Task ExecuteAsync(IDbConnection connection, IDbTransaction transaction, CancellationToken cancellationToken = default)
            => connection.ExecuteAsync(Sql, _entity, transaction);

        public Task<int> ExecuteAsyncScalar(IDbConnection connection, IDbTransaction transaction, CancellationToken cancellationToken = default)
            => connection.ExecuteAsync(Sql, _entity, transaction);
    }
}
