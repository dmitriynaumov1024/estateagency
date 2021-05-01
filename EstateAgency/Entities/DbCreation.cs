using System;
using System.Collections;
using System.Collections.Generic;
using Apache.Ignite.Core;
using Apache.Ignite.Core.Cache.Query;
using Apache.Ignite.Core.Client;
using Apache.Ignite.Core.Client.Cache;
using Apache.Ignite.Core.Cache.Configuration;
using EstateAgency.Entities;

// Please obey single responsibility rule.

namespace EstateAgency.Database
{
    public static partial class DbClient
    {
        /// <summary>
        /// Current cache name collection.
        /// </summary>
        public static readonly string[] CacheNames = 
        {
            "credential",
            "person",
            "agent",
            "location",
            "clientwish",
            "estateobject",
            "bookmark",
            "order",
            "match",
            "deal",
            "lastusedkey"
        };

        /// <summary>
        /// Collection of names of caches that use independent integer keys. 
        /// </summary>
        public static readonly string[] CacheIndependentIntKeyNames =
        {
            "person",
            "location",
            "clientwish",
            "estateobject"
        };

        public static ICacheClient<string, Credential>  CredentialCache;
        public static ICacheClient<int, Person>         PersonCache;
        public static ICacheClient<int, Agent>          AgentCache;
        public static ICacheClient<int, Location>       LocationCache;
        public static ICacheClient<int, ClientWish>     ClientWishCache;
        public static ICacheClient<int, EstateObject>   ObjectCache;
        public static ICacheClient<int, House>          HouseCache;
        public static ICacheClient<int, Flat>           FlatCache;
        public static ICacheClient<int, Landplot>       LandplotCache;
        public static ICacheClient<long, Bookmark>      BookmarkCache;
        public static ICacheClient<long, Order>         OrderCache;
        public static ICacheClient<long, Match>         MatchCache;
        public static ICacheClient<int, Deal>           DealCache;

        public static ICacheClient<string, int> LastUsedKeys;

        /// <summary>
        /// Create the database of estate agency. 
        /// Requires connection to the cluster.
        /// </summary>
        /// <returns>True if database was successfully created.</returns>
        public static bool CreateDatabase()
        {
            if (client==null) 
                return false;

            // Cache configuration goes here ----------------------------------

            CacheClientConfiguration credentialCfg = new CacheClientConfiguration
            {
                GroupName = "estateagency",
                Name = "credential",
                AtomicityMode = CacheAtomicityMode.Transactional
                /*
                QueryEntities = new[]
                {
                    new QueryEntity
                    {
                        TableName = "Credentials",
                        KeyType = typeof(string),
                        ValueType = typeof(Credential)
                    }
                }
                */
            };

            CacheClientConfiguration personCfg = new CacheClientConfiguration
            {
                GroupName = "estateagency",
                Name = "person",
                AtomicityMode = CacheAtomicityMode.Transactional,
                QueryEntities = new[]
                {
                    new QueryEntity
                    {
                        TableName = "Persons",
                        KeyType = typeof(int),
                        ValueType = typeof(Person)
                    }
                }
            };

            CacheClientConfiguration agentCfg = new CacheClientConfiguration
            {
                GroupName = "estateagency",
                Name = "agent",
                AtomicityMode = CacheAtomicityMode.Transactional,
                QueryEntities = new[]
                {
                    new QueryEntity
                    {
                        TableName = "Agents",
                        KeyType = typeof(int),
                        ValueType = typeof(Agent)
                    }
                }
            };

            CacheClientConfiguration estateobjectCfg = new CacheClientConfiguration
            {
                GroupName = "estateagency",
                Name = "estateobject",
                AtomicityMode = CacheAtomicityMode.Transactional,
                QueryEntities = new[]
                {
                    new QueryEntity
                    {
                        TableName = "EstateObjects",
                        KeyType = typeof(int),
                        ValueType = typeof(EstateObject)
                    },
                    new QueryEntity
                    {
                        TableName = "Houses",
                        KeyType = typeof(int),
                        ValueType = typeof(House)
                    },
                    new QueryEntity
                    {
                        TableName = "Flats",
                        KeyType = typeof(int),
                        ValueType = typeof(Flat)
                    },
                    new QueryEntity
                    {
                        TableName = "Landplots",
                        KeyType = typeof(int),
                        ValueType = typeof(Landplot)
                    },
                }
            };

            CacheClientConfiguration locationCfg = new CacheClientConfiguration
            {
                GroupName = "estateagency",
                Name = "location",
                AtomicityMode = CacheAtomicityMode.Transactional,
                QueryEntities = new[]
                {
                    new QueryEntity
                    {
                        TableName = "Locations",
                        KeyType = typeof(int),
                        ValueType = typeof(Location)
                    }
                }
            };

            CacheClientConfiguration clientwishCfg = new CacheClientConfiguration
            {
                GroupName = "estateagency",
                Name = "clientwish",
                AtomicityMode = CacheAtomicityMode.Transactional,
                QueryEntities = new[]
                {
                    new QueryEntity
                    {
                        TableName = "ClientWishes",
                        KeyType = typeof(int),
                        ValueType = typeof(ClientWish)
                    }
                }
            };

            CacheClientConfiguration bookmarkCfg = new CacheClientConfiguration
            {
                GroupName = "estateagency",
                Name = "bookmark",
                AtomicityMode = CacheAtomicityMode.Transactional,
                QueryEntities = new[]
                {
                    new QueryEntity
                    {
                        TableName = "Bookmarks",
                        KeyType = typeof(long),
                        ValueType = typeof(Bookmark),
                        Indexes = new QueryIndex[]
                        {
                            new QueryIndex("PersonID"),
                            new QueryIndex("ObjectID")
                        }
                    }
                }
            };

            CacheClientConfiguration matchCfg = new CacheClientConfiguration
            {
                GroupName = "estateagency",
                Name = "match",
                AtomicityMode = CacheAtomicityMode.Transactional,
                QueryEntities = new[]
                {
                    new QueryEntity
                    {
                        TableName = "Matches",
                        KeyType = typeof(long),
                        ValueType = typeof(Match)
                    }
                }
            };

            CacheClientConfiguration orderCfg = new CacheClientConfiguration
            {
                GroupName = "estateagency",
                Name = "order",
                AtomicityMode = CacheAtomicityMode.Transactional,
                QueryEntities = new[]
                {
                    new QueryEntity
                    {
                        TableName = "Orders",
                        KeyType = typeof(long),
                        ValueType = typeof(Match)
                    }
                }
            };

            CacheClientConfiguration dealCfg = new CacheClientConfiguration
            {
                GroupName = "estateagency",
                Name = "deal",
                AtomicityMode = CacheAtomicityMode.Transactional,
                QueryEntities = new[]
                {
                    new QueryEntity
                    {
                        TableName = "Deals",
                        KeyType = typeof(int),
                        ValueType = typeof(Deal)
                    }
                }
            };

            CacheClientConfiguration lastusedkeyCfg = new CacheClientConfiguration
            {
                GroupName = "estateagency",
                Name = "lastusedkey",
                AtomicityMode = CacheAtomicityMode.Transactional
            };

            // Cache creation goes here ---------------------------------------
            LocationCache     = client.CreateCache <int, Location> (locationCfg);
            CredentialCache   = client.CreateCache <string, Credential> (credentialCfg);
            PersonCache       = client.CreateCache <int, Person> (personCfg);
            AgentCache        = client.CreateCache <int, Agent> (agentCfg);
            ObjectCache       = client.CreateCache <int, EstateObject> (estateobjectCfg);
            HouseCache        = client.GetCache <int, House> ("estateobject");
            FlatCache         = client.GetCache <int, Flat> ("estateobject");
            LandplotCache     = client.GetCache <int, Landplot> ("estateobject");
            ClientWishCache   = client.CreateCache <int, ClientWish> (clientwishCfg);
            MatchCache        = client.CreateCache <long, Match> (matchCfg);
            BookmarkCache     = client.CreateCache <long, Bookmark> (bookmarkCfg);
            OrderCache        = client.CreateCache <long, Order> (orderCfg);
            DealCache         = client.CreateCache <int, Deal> (dealCfg);

            LastUsedKeys      = Client.CreateCache <string, int> (lastusedkeyCfg);
            foreach (string i in CacheIndependentIntKeyNames)
                LastUsedKeys.Put (i, 0);

            return true;
        }
        
        /// <summary>
        /// Retrieve all used caches and assign <b>ICacheClient</b> variables for access to caches.
        /// </summary>
        /// <returns>True if all caches were successfully retrieved.</returns>
        public static bool GetDatabase()
        {
            if (client==null) 
                return false;

            LocationCache     = client.GetCache <int, Location>      ("location");
            CredentialCache   = client.GetCache <string, Credential> ("credential");
            PersonCache       = client.GetCache <int, Person>        ("person");
            AgentCache        = client.GetCache <int, Agent>         ("agent");
            ObjectCache       = client.GetCache <int, EstateObject>  ("estateobject");
            HouseCache        = client.GetCache <int, House>         ("estateobject");
            FlatCache         = client.GetCache <int, Flat>          ("estateobject");
            LandplotCache     = client.GetCache <int, Landplot>      ("estateobject");
            ClientWishCache   = client.GetCache <int, ClientWish>    ("clientwish");
            MatchCache        = client.GetCache <long, Match>        ("match");
            BookmarkCache     = client.GetCache <long, Bookmark>     ("bookmark");
            OrderCache        = client.GetCache <long, Order>        ("order");
            DealCache         = client.GetCache <int, Deal>          ("deal");

            LastUsedKeys      = client.GetCache <string, int>        ("lastusedkey");

            return true;
        }

        /// <summary>
        /// Delete the database of estate agency.
        /// Requires connection to the cluster.
        /// </summary>
        /// <returns>True if database was successfully deleted.</returns>
        public static bool DeleteDatabase()
        {
            if (client==null) 
                return false;

            foreach (string i in CacheNames) 
            {
                try { 
                    client.DestroyCache (i);
                }
                catch (Exception ee)
                {
                    Console.WriteLine ($"Cache with name {i} does not exist.");
                }
            }

            return true;
        }
    }
}
