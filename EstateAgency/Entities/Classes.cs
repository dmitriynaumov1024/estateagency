using System;

namespace Entities
{
    public class Person
    {
        public string Surname;
        public string Name;
        public string Email;
        public string Phone;
        public int LocationID;
        public string Address;
        public DateTime RegDate;
    }

    public class Location
    {
        public string Region;
        public string Town;
        public string District;
    }

    public class Credential
    {
        public int PersonID;
        public string Password;
        public char Privilegies;
        public char Status;
    }
}
