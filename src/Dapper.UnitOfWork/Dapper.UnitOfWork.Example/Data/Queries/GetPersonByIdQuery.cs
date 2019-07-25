using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Dapper.UnitOfWork.Example.Data.Queries
{
    public class GetPersonByIdQuery : IQuery<PersonEntity>, IQueryList<PersonEntity>, IAsyncQueryList<PersonEntity>
    {
        private const string Sql = "" +
            "SELECT p.*, a.* FROM People p " +
                "JOIN Addresses a " +
                    "ON a.Id = p.Address_Id " +
            "WHERE p.Id = @Id";

        private int _personId { get; }

        public GetPersonByIdQuery(int personId)
            => _personId = personId;

        public PersonEntity Execute(IDbConnection connection, IDbTransaction transaction)
            => connection.Query<PersonEntity, AddressEntity, PersonEntity>(Sql, (person, address) => {
                person.Address = address;
                return person;
            }, new { Id = _personId }, transaction, splitOn: "Id").FirstOrDefault();

        public IEnumerable<PersonEntity> ExecuteList(IDbConnection connection, IDbTransaction transaction)
            => connection.Query<PersonEntity, AddressEntity, PersonEntity>(Sql, (person, address) => {
                person.Address = address;
                return person;
            }, new { Id = _personId }, transaction, splitOn: "Id");

        public Task<IEnumerable<PersonEntity>> ExecuteAsyncList(IDbConnection connection, IDbTransaction transaction, CancellationToken cancellationToken = default)
            => connection.QueryAsync<PersonEntity, AddressEntity, PersonEntity>(Sql, (person, address) => {
                person.Address = address;
                return person;
            }, new { Id = _personId }, transaction, splitOn: "Id");
    }
}
