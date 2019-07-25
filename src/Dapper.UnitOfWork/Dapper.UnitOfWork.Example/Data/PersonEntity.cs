using System;
using System.Collections.Generic;
using System.Text;

namespace Dapper.UnitOfWork.Example.Data
{
    public class PersonEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int Address_Id { get; set; }
        public AddressEntity Address { get; set; }
    }
}
