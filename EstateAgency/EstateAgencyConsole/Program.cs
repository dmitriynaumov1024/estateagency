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
        static void Main()
        {
            Person p = new Person
            {
                Surname = "Іванов-Донцев",
                Name = "Іван Іванович",
                Phone = "09822899148",
                Email = "ivanov228@gmail.com",
                StreetName = "вулиця Героїв 93-ї бригади",
                HouseNumber = "1f",
                FlatNumber = 3,
                RegDate = DateTime.Parse("2021-04-20").ToUniversalTime()
            };

            ValidationResult vr = p.Validate;
            if (vr.isValid)
            {
                Console.WriteLine ("Success!");
            } 
            else
            {
                Console.WriteLine (vr.Message);
                Console.WriteLine (vr.FieldName);
            }
        }

        /*
        static void Main ()
        {
            DbClient.Connect();
            DbClient.SetClusterActive(true);

            //DbClient.CreateDatabase();
            DbClient.GetDatabase();

            Console.WriteLine ("Caches:");
            foreach (string i in DbClient.Client.GetCacheNames())
                Console.WriteLine (i);
            Console.WriteLine ("-----------------------------------------------------------");

            
            Credential c = new Credential
            {
                PersonID = 100,
                Password = "Ag7a0grspaz45!a",
                Privilegies = 'c',
                Status = 'n'
            };
            

            Person[] p = new Person[]
            {
                new Person {
                    Surname = "Ivanov",
                    Name = "Ivan",
                    Phone = "+380962281488",
                    Email = "ivan228@gmail.com",
                    LocationID = 4,
                    Address = "West st., 45",
                    RegDate = DateTime.Today.ToUniversalTime()
                },
                new Person {
                    Surname = "Petrov",
                    Name = "Petro",
                    Phone = "+380962281400",
                    Email = "petrov228@gmail.com",
                    LocationID = 4,
                    Address = "West st., 46",
                    RegDate = (DateTime.Parse("2021-04-20")).ToUniversalTime()
                },
                new Person {
                    Surname = "Petros",
                    Name = "Petro",
                    Phone = "+380962281400",
                    Email = "petrov228@gmail.com",
                    LocationID = 4,
                    Address = "West st., 46",
                    RegDate = (DateTime.Parse("2021-04-20")).ToUniversalTime()
                }
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
                Address = "West st., 46",
                RegDate = (DateTime.Parse("2021-04-20")).ToUniversalTime()
            };

            EstateObject obj = new EstateObject
            {
                SellerID = 0,
                PostDate = DateTime.Now.ToUniversalTime(),
                isOpen = true,
                isVisible = true,
                LocationID = 4,
                Address = "West st., 45",
                Variant = (byte)'h',
                Price = 10000,
                State = 5,
                Description = "House",
                Tags = new[] {"string1", "string2"},
                PhotoUrls = new[] {"url1", "url2", "url3"}
            };
            
            DbClient.LocationCache.Put (4, loc);

            DbClient.PutPerson (p);
            DbClient.ObjectCache.Put (0, obj);
            
            foreach (var row in DbClient.PersonCache.Query(new SqlFieldsQuery("select _key, Surname, Name, Phone, Email, LocationID, Address, RegDate from Persons;")))
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
            
            foreach (var row in DbClient.ObjectCache.Query(new SqlFieldsQuery("select _key, SellerID, PostDate, isOpen, Address, Tags from EstateObjects;")))
            {
                Console.WriteLine ("key         : {0}", row[0]);
                Console.WriteLine ("SellerID    : {0}", row[1]);
                Console.WriteLine ("PostDate    : {0}", row[2]);
                Console.WriteLine ("IsOpen      : {0}", row[3]);
                Console.WriteLine ("Address     : {0}", row[4]);
                Console.WriteLine ("Tags        : ");
                foreach (string i in (row[5] as string[]))
                Console.WriteLine ("              "+i);
                Console.WriteLine ("-----------------------------------------------------------");
            }

            Console.WriteLine("[Done. Press ENTER to continue.]");
            Console.Read();
            DbClient.Disconnect();
        }
        */
    }
}
