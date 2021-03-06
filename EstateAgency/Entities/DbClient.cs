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
    /// <summary>
    /// Static client wrap class for interaction with database.
    /// </summary>
    public static partial class DbClient
    {
        /// <summary>
        /// IIgniteClient for interaction with database.
        /// </summary>
        static IIgniteClient client = null;

        /// <summary>
        /// Readonly instance of inner IIgniteClient.
        /// </summary>
        public static IIgniteClient Client { get => client; }

        /// <summary>
        /// Connect to database.
        /// </summary>
        public static void Connect()
        {
            client = Ignition.StartClient (new IgniteClientConfiguration 
                {
                    Endpoints = new[] {"127.0.0.1:10800"},
                    BinaryConfiguration = new Apache.Ignite.Core.Binary.BinaryConfiguration (typeof(Entities.Matcher), typeof(Int32)),
                }
            );
        }

        /// <summary>
        /// Disconnect from database.
        /// </summary>
        public static void Disconnect()
        {
            client.Dispose();
        }

        /// <summary>
        /// Change cruster state.
        /// </summary>
        /// <param name="isActive">Parameter to pass to inner 
        /// function, indicates whether the cluster should be active.
        /// </param>
        public static void SetClusterActive(bool isActive)
        {
            client.GetCluster().SetActive(isActive);
        }
    }
}