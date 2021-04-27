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
        public static ICacheClient<int, Bookmark>       BookmarkCache;
        public static ICacheClient<int, Order>          OrderCache;
        public static ICacheClient<int, Match>          MatchCache;
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