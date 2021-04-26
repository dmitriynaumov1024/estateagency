using System;
using System.Collections;
using System.Collections.Generic;
using Apache.Ignite.Core;
using Apache.Ignite.Core.Cache.Query;
using Apache.Ignite.Core.Client;
using Apache.Ignite.Core.Client.Cache;
using Apache.Ignite.Core.Cache.Configuration;
using EstateAgency.Entities;

namespace EstateAgencyConsole
{
    class Program
    {
        /// <summary>
        /// Method for creating a database of Estate agency.
        /// </summary>
        /// <param name="client">IIgniteClient instance for database connection.</param>
        /// <returns>true if caches were successfully created, false otherwise.</returns>
        public static bool CreateDatabase(IIgniteClient client)
        {
            CacheClientConfiguration CacheCfg = new CacheClientConfiguration
            {
                Name = "EstateAgency",
                QueryEntities = new[]
                {
                    new QueryEntity
                    {
                        TableName = "Persons",
                        KeyType = typeof(int),
                        ValueType = typeof(Person),
                        Fields = new QueryField[]
                        {
                            new QueryField ("Surname", typeof(string)),
                            new QueryField ("Name", typeof(string)),
                            new QueryField ("Phone", typeof(string)),
                            new QueryField ("Email", typeof(string)),
                            new QueryField ("LocationID", typeof(int)),
                            new QueryField ("Address", typeof(string)),
                            new QueryField ("RegDate", typeof(DateTime))
                        }
                    },
                    new QueryEntity
                    {
                        TableName = "Locations",
                        KeyType = typeof(int),
                        ValueType = typeof(Location),
                        Fields = new QueryField[]
                        {
                            new QueryField ("Region", typeof(string)),
                            new QueryField ("Town", typeof(string)),
                            new QueryField ("District", typeof(string))
                        }
                    },
                    new QueryEntity
                    {
                        TableName = "Credentials",
                        KeyType = typeof(string),
                        ValueType = typeof(Credential)
                    },
                    new QueryEntity
                    {
                        TableName = "Agents",
                        KeyType = typeof(int),
                        ValueType = typeof(Agent),
                        Fields = new QueryField[]
                        {
                            new QueryField ("TotalDeals", typeof(int)),
                            new QueryField ("MonthDeals", typeof(int)),
                            new QueryField ("MonthPayment", typeof(int))
                        }
                    },
                    new QueryEntity
                    {
                        TableName = "EstateObjects",
                        KeyType = typeof(int),
                        ValueType = typeof(EstateObject),
                        Fields = new QueryField[]
                        {
                            new QueryField ("SellerID", typeof(int)),
                            new QueryField ("PostDate", typeof(DateTime)),
                            new QueryField ("isOpen", typeof(bool)),
                            new QueryField ("isVisible", typeof(bool)),
                            new QueryField ("LocationID", typeof(int)),
                            new QueryField ("Address", typeof(string)),
                            new QueryField ("Variant", typeof(byte)),
                            new QueryField ("Price", typeof(int)),
                            new QueryField ("State", typeof(byte)),
                            new QueryField ("Description", typeof(string)),
                            new QueryField ("Tags", typeof (List<string>)),
                            new QueryField ("PhotoUrls", typeof (List<string>))
                        }
                    },
                    new QueryEntity
                    {
                        TableName = "Houses",
                        KeyType = typeof(int),
                        ValueType = typeof(House),
                        Fields = new QueryField[]
                        {
                            new QueryField ("LandArea", typeof(float)),
                            new QueryField ("HomeArea", typeof(float)),
                            new QueryField ("FloorCount", typeof(int)),
                            new QueryField ("RoomCount", typeof(int))
                        }
                    },
                    new QueryEntity
                    {
                        TableName = "Flats",
                        KeyType = typeof(int),
                        ValueType = typeof(Flat),
                        Fields = new QueryField[]
                        {
                            new QueryField ("Floor", typeof(int)),
                            new QueryField ("HomeArea", typeof(float)),
                            new QueryField ("RoomCount", typeof(int))
                        }
                    },
                    new QueryEntity
                    {
                        TableName = "LandPlots",
                        KeyType = typeof(int),
                        ValueType = typeof(Landplot),
                        Fields = new[]
                        {
                            new QueryField ("LandArea", typeof(float))
                        }
                    },
                    new QueryEntity
                    {
                        TableName = "Deals",
                        KeyType = typeof(int),
                        ValueType = typeof(Deal),
                        Fields = new[]
                        {
                            new QueryField ("BuyerID", typeof(int)),
                            new QueryField ("SellerID", typeof(int)),
                            new QueryField ("AgentID", typeof(int)),
                            new QueryField ("DealTime", typeof(DateTime)),
                            new QueryField ("Price", typeof(int))
                        }
                    },
                    new QueryEntity
                    {
                        TableName = "ClientWishes",
                        KeyType = typeof(int),
                        ValueType = typeof(ClientWish),
                        Fields = new[]
                        {
                            new QueryField ("ClientID", typeof(int)),
                            new QueryField ("isOpen", typeof(bool)),
                            new QueryField ("PostDate", typeof(DateTime)),
                            new QueryField ("LocationID", typeof(int)),
                            new QueryField ("Variant", typeof(byte)),
                            new QueryField ("Price", typeof(int)),
                            new QueryField ("Tags", typeof(List<string>)),
                            new QueryField ("NeededState", typeof(byte))
                        }
                    },
                    new QueryEntity
                    {
                        TableName = "Matches",
                        KeyType = typeof(int),
                        ValueType = typeof(Match),
                        Fields = new[]
                        {
                            new QueryField ("WishID", typeof(int)),
                            new QueryField ("ObjectID", typeof(int))
                        },
                        Indexes = new[]
                        {
                            new QueryIndex ("WishID"),
                            new QueryIndex ("ObjectID")
                        }
                    },
                    new QueryEntity
                    {
                        TableName = "Bookmarks",
                        KeyType = typeof(int),
                        ValueType = typeof(Bookmark),
                        Fields = new[]
                        {
                            new QueryField ("PersonID", typeof(int)),
                            new QueryField ("ObjectID", typeof(int))
                        }
                    },
                    new QueryEntity
                    {
                        TableName = "Orders",
                        KeyType = typeof(int),
                        ValueType = typeof(Order),
                        Fields = new[]
                        {
                            new QueryField ("ClientID", typeof(int)),
                            new QueryField ("ObjectID", typeof(int)),
                            new QueryField ("AgentID", typeof(int)),
                            new QueryField ("OrderTime", typeof(DateTime)),
                            new QueryField ("isOpen", typeof(bool))
                        }
                    }
                }
            };


            try 
            { 
                client.CreateCache<string, Credential>(CacheCfg);
                client.CreateCache<int, Person>(CacheCfg);
                client.CreateCache<int, Agent>(CacheCfg);
                client.CreateCache<int, Location>(CacheCfg);
                client.CreateCache<int, EstateObject>(CacheCfg);
                client.CreateCache<int, House>(CacheCfg);
                client.CreateCache<int, Flat>(CacheCfg);
                client.CreateCache<int, Landplot>(CacheCfg);
                client.CreateCache<int, Deal>(CacheCfg);
                client.CreateCache<int, Bookmark>(CacheCfg);
                client.CreateCache<int, Order>(CacheCfg);
                client.CreateCache<int, ClientWish>(CacheCfg);
                client.CreateCache<int, Match>(CacheCfg);

                // Return true if all tables/caches with given configurations are successfully created
                return true;
            } 
            catch (Exception e)
            {
                Console.WriteLine("An exception occured during cache creation. \n Details: \n");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                return false;
            }
            
        }

        /// <summary>
        /// Connect to Apache ignite cluster.
        /// </summary>
        /// <returns>Instance of IIgniteClient</returns>
        public static IIgniteClient GetClient()
        {
            var igniteCfg = new IgniteClientConfiguration
            {
                BinaryConfiguration = new Apache.Ignite.Core.Binary.BinaryConfiguration
                {
                    Serializer = new Apache.Ignite.Core.Binary.BinaryReflectiveSerializer
                    {
                        ForceTimestamp = true
                    }
                },
                Endpoints = new[] {"127.0.0.1:10800"}
            };
            return Ignition.StartClient (igniteCfg);
        }

        /// <summary>
        /// Main entry point for application. The main task 
        /// of the program is to create database of Estate agency.
        /// </summary>
        static void Main ()
        {
            IIgniteClient DbClient = GetClient();
            DbClient.GetCluster().SetActive(true);
            /*
            foreach (string i in DbClient.GetCacheNames())
                DbClient.DestroyCache(i);
            */
            CreateDatabase(DbClient);

            Console.WriteLine ("Caches:");
            foreach (string i in DbClient.GetCacheNames())
                Console.WriteLine (i);
            Console.WriteLine ("-----------------------------------------------------------");
            /*
            Credential c = new Credential
            {
                PersonID = 100,
                Password = "Ag7a0grspaz45!a",
                Privilegies = 'c',
                Status = 'n'
            };
            */
            ICacheClient<int, Person> PersonCache = DbClient.GetCache<int, Person> ("EstateAgency");
            ICacheClient<int, EstateObject> EstateObjectCache = DbClient.GetCache<int, EstateObject>("EstateAgency");

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
                Tags = new List<string>(),
                PhotoUrls = new List<string>()
            };

            for (int i=0; i<p.Length; i++)
                PersonCache.Put (i+5, p[i]);

            EstateObjectCache.Put (5, obj);

            foreach (var row in PersonCache.Query(new SqlFieldsQuery("select _key, * from Persons;")))
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

            foreach (var row in EstateObjectCache.Query(new SqlFieldsQuery("select _key, * from EstateObjects;")))
            {
                Console.WriteLine ("key         : {0}", row[0]);
                Console.WriteLine ("SellerID    : {0}", row[1]);
                Console.WriteLine ("PostDate    : {0}", row[2]);
                Console.WriteLine ("IsOpen      : {0}", row[3]);
                Console.WriteLine ("Address     : {0}", row[6]);
                Console.WriteLine ("-----------------------------------------------------------");
            }

            Console.Read();
            DbClient.Dispose();
        }


        // -------------- UNUSED ---------------------------------------------
        [Obsolete("The program is not using Ignite server any more. It should be started from command line using ignite.bat command.", true)]
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
    }
}
