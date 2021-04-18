using System;
using Apache.Ignite;
using Apache.Ignite.Core;
using Apache.Ignite.Core.Cache;
using Entities;

namespace EstateAgencyConsole
{
    class Program
    {
        public static IIgnite StartIgnitePersistent()
        {
            IgniteConfiguration cfg = new IgniteConfiguration 
            { 
                RedirectJavaConsoleOutput = false,
                DataStorageConfiguration = 
                new Apache.Ignite.Core.Configuration.DataStorageConfiguration
                {
                    StoragePath = "/storage",
                    WalMode = Apache.Ignite.Core.Configuration.WalMode.Background,
                    WalSegments = 2,
                    WalHistorySize = 2,
                    DefaultDataRegionConfiguration = 
                    new Apache.Ignite.Core.Configuration.DataRegionConfiguration
                    {
                        PersistenceEnabled = true,
                        Name = "defaultregion"
                    }
                }
            };
            IIgnite ignite = Ignition.Start(cfg);
            ignite.GetCluster().SetActive(true);
            return ignite;
        }

        static void Main ()
        {
            IIgnite ignite = StartIgnitePersistent();
            ICache<int, Person> Persons = ignite.GetOrCreateCache<int, Person>("persons");
            Person p = null;

            
            Person p0 = new Person
            {
                Surname = "Іванов",
                Name = "Іван",
                Email = "ivanov.ivan@ukr.net",
                Phone = "+380964449810",
                LocationID = 1,
                Address = "вул. Житомирська, буд. 6",
                RegDate = DateTime.Today
            };
            ignite.GetCluster().DisableWal("persons");
            for (int i=-10001; i<20000; i++)
            Persons.Put (i, p0);
            ignite.GetCluster().EnableWal("persons");

            if (Persons.TryGet (0, out p) && p!=null)
            {
                Console.WriteLine ($"Surname: {p.Surname} \nName: {p.Name} \nSignup date: {p.RegDate}");
            } 
            else
            {
                Console.WriteLine ("Key not found.");
            }
            Console.Read();
            ignite.Dispose();
        }
    }
}
