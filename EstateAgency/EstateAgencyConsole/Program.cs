using System;
using System.Collections;
using System.Collections.Generic;
using Apache.Ignite.Core;
using Apache.Ignite.Core.Cache.Query;
using Apache.Ignite.Core.Client;
using Apache.Ignite.Core.Client.Cache;
using EstateAgency.Entities;
using EstateAgency.Database;

namespace EstateAgencyConsole
{
    class Program
    {
        
        /// <summary>
        /// Main entry point for application. The main task 
        /// of the program is to create database of Estate agency.
        /// </summary>

        static void Main ()
        {
            DbClient.Connect();
            DbClient.SetClusterActive(true);

            DbClient.DeleteDatabase();
            DbClient.CreateDatabase();
            //DbClient.GetDatabase();

            Console.WriteLine ("Caches:");
            foreach (string i in DbClient.Client.GetCacheNames())
                Console.WriteLine (i);
            Console.WriteLine ("-----------------------------------------------------------");

            
            Credential c = new Credential
            {
                PersonID = 0,
                Password = "Ag7a0grspaz45!a",
                Privilegies = (byte)'c',
                Status = (byte)'n'
            };
            
            Location loc = new Location
            {
                Region = "Запорізька",
                Town = "Запоріжжя",
                District = "Вознесенівський"
            };

            Person p = new Person
            {
                Surname = "Petrov",
                Name = "Petro",
                Phone = "+380962281400",
                Email = "petrov228@gmail.com",
                LocationID = 4,
                StreetName = "вулиця Героїв 93 бригади",
                HouseNumber = "14",
                FlatNumber = 12,
                RegDate = (DateTime.Parse("2021-04-20")).ToUniversalTime()
            };
            
            EstateObject obj = new House
            {
                SellerID = 0,
                PostDate = (DateTime.Parse("2021-04-20 18:05:00")).ToUniversalTime(),
                isOpen = true,
                isVisible = true,
                LocationID = 4,
                StreetName = "вулиця Барикадна",
                HouseNumber = "45а",
                Variant = (byte)'h',
                Price = 23000,
                State = 5,
                Description = "Hello hello hell to world... na na na na na na no.",
                Tags = new[] {"ele", "wat", "gas"},
                PhotoUrls = new[] {"photo1.jpg", "photo2.jpg"},
                LandArea = 12,
                HomeArea = 89,
                FloorCount = 1,
                RoomCount = 5
            };

            DbClient.LocationCache.Put (4, loc);

            DbClient.PutPerson (p);
            DbClient.PutCredential (c);
            
            var v = obj.Validate;
            if (v.isValid)
            {
                Console.WriteLine ("Succesful validation.");
                DbClient.PutEstateObject(obj);
            }
            else
            {
                Console.WriteLine ("Validation failed: " + v.Message);
            }

            foreach (var row in DbClient.PersonCache.Query(new SqlFieldsQuery("select _key, Surname, Name, Phone, Email, LocationID, concat (StreetName, ' ', HouseNumber, ' ', FlatNumber) as Address, RegDate from Persons;")))
            {
                Console.WriteLine ("key         : {0}", row[0]);
                Console.WriteLine ("Surname     : {0}", row[1]);
                Console.WriteLine ("Name        : {0}", row[2]);
                Console.WriteLine ("Phone       : {0}", row[3]);
                Console.WriteLine ("Email       : {0}", row[4]);
                Console.WriteLine ("LocationID  : {0}", row[5]);
                Console.WriteLine ("Address     : {0}", row[6]);
                Console.WriteLine ("RegDate     : {0:yyyy-MM-dd}", ((DateTime)row[7]).ToLocalTime());
                Console.WriteLine ("-----------------------------------------------------------");
            }
            Console.WriteLine($"EstateObject cache contains Key: {DbClient.ObjectCache.ContainsKey(0)}");
            House hh = (House) DbClient.ObjectCache.Get(0);
            Console.WriteLine ((char)hh.Variant);
            Console.WriteLine (hh.RoomCount);
            Console.WriteLine ("-----------------------------------------------------------");
            foreach (var row in DbClient.ObjectCache.Query(new SqlFieldsQuery("select _key, SellerID, PostDate, isOpen, Tags from Houses;")))
            {
                Console.WriteLine ("key         : {0}", row[0]);
                Console.WriteLine ("SellerID    : {0}", row[1]);
                Console.WriteLine ("PostDate    : {0}", row[2]);
                Console.WriteLine ("IsOpen      : {0}", row[3]);
                Console.WriteLine ("Tags        : ");
                foreach (string i in (row[4] as string[]))
                Console.WriteLine ("              "+i);
                Console.WriteLine ("-----------------------------------------------------------");
            }

            Console.WriteLine("[Done. Press ENTER to continue.]");
            Console.Read();
            DbClient.Disconnect();
        }
    }
}
