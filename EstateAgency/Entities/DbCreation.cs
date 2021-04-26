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
        public static bool CreateDatabase()
        {
            if (client==null) { return false; }

            return true;
        }
    }
}