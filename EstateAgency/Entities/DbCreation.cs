using System;
using System.Collections;
using System.Collections.Generic;
using Apache.Ignite.Core;
using Apache.Ignite.Core.Cache.Query;
using Apache.Ignite.Core.Client;
using Apache.Ignite.Core.Client.Cache;
using Apache.Ignite.Core.Cache.Configuration;
using EstateAgency.Entities;

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
            "deal"
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


        /// <summary>
        /// Create the database of estate agency. 
        /// Requires connection to the cluster.
        /// </summary>
        /// <returns>True if database was successfully created.</returns>
        public static bool CreateDatabase()
        {
            if (client==null) 
                return false;

            CacheClientConfiguration credentialCfg = new CacheClientConfiguration
            {
                GroupName = "estateagency",
                DataRegionName = "estateagency",
                Name = "credential",
                QueryEntities = new[]
                {
                    new QueryEntity
                    {
                        TableName = "Credentials",
                        KeyType = typeof(string),
                        ValueType = typeof(Credential)
                    }
                }
            };

            CacheClientConfiguration personCfg = new CacheClientConfiguration
            {
                GroupName = "estateagency",
                DataRegionName = "estateagency",
                Name = "person",
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
                DataRegionName = "estateagency",
                Name = "agent",
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
                DataRegionName = "estateagency",
                Name = "estateobject",
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
                DataRegionName = "estateagency",
                Name = "location",
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
                DataRegionName = "estateagency",
                Name = "clientwish",
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
                DataRegionName = "estateagency",
                Name = "bookmark",
                QueryEntities = new[]
                {
                    new QueryEntity
                    {
                        TableName = "Bookmarks",
                        KeyType = typeof(long),
                        ValueType = typeof(Bookmark)
                    }
                }
            };

            CacheClientConfiguration matchCfg = new CacheClientConfiguration
            {
                GroupName = "estateagency",
                DataRegionName = "estateagency",
                Name = "match",
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
                DataRegionName = "estateagency",
                Name = "match",
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
                DataRegionName = "estateagency",
                Name = "deal",
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
                client.DestroyCache (i);

            return true;
        }
    }
}