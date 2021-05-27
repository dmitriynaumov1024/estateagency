using System;
using System.Data.SQLite;

namespace Entities
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int LocationId { get; set; }
        public string Address { get; set; }
    }
    public class Location
    {
        public int Id { get; set; }
        public string Region { get; set; }
        public string Town { get; set; }
        public string District { get; set; }
    }
}
