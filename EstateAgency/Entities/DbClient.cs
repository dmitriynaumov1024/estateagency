using System;
using System.Collections;
using System.Collections.Generic;
using Apache.Ignite.Core;
using Apache.Ignite.Core.Cache.Query;
using Apache.Ignite.Core.Client;
using Apache.Ignite.Core.Client.Cache;
using Apache.Ignite.Core.Cache.Configuration;

namespace EstateAgency.Database
{
    public static partial class DbClient
    {
        /// <summary>
        /// IIgniteClient for interaction with database.
        /// </summary>
        static IIgniteClient client = null;

        /// <summary>
        /// Connect to database.
        /// </summary>
        public static void Connect()
        {
            client = Ignition.StartClient (new IgniteClientConfiguration 
                {Endpoints = new[] {"127.0.0.1:10800"}}
            );
        }

        /// <summary>
        /// Disconnect from database.
        /// </summary>
        public static void Disconnect()
        {
            client.Dispose();
        }
    }
}