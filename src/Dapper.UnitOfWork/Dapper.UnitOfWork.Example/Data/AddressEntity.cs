using System;
using System.Collections.Generic;
using System.Text;

namespace Dapper.UnitOfWork.Example.Data
{
    public class AddressEntity
    {
        public int Id { get; set; }
        public string Street { get; set; }
        public string Region { get; set; }
    }
}
