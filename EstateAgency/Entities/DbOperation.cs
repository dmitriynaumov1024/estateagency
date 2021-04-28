using System;
using System.Collections;
using System.Collections.Generic;
using Apache.Ignite.Core;
using Apache.Ignite.Core.Cache.Query;
using Apache.Ignite.Core.Client;
using Apache.Ignite.Core.Client.Cache;
using Apache.Ignite.Core.Cache.Configuration;
using Apache.Ignite.Core.Client.Transactions;
using EstateAgency.Entities;

namespace EstateAgency.Database
{
    public static partial class DbClient
    {
        /// <summary>
        /// Put a person into the cache.<br/>
        /// This method checks referential integrity on field Person.LocationID: <br/>
        /// Location with _key = Person.LocationID has to exist. <br/>
        /// Otherwise, method throws ReferentialException. <br/>
        /// This and other put methods DO NOT validate the entity.
        /// </summary>
        /// <param name="p">Person to put into the cache.</param>
        public static void PutPerson (Person p)
        {
            if (LocationCache.ContainsKey(p.LocationID))
            {
                using (var tx = Client.GetTransactions().TxStart())
                {
                    int key = LastUsedKeys.Get("person");
                    PersonCache.Put(key, p);
                    key++;
                    LastUsedKeys.Put ("person", key);
                    tx.Commit();
                }
            } 
            else
            {
                throw new ReferentialException ("Can not put new entry into Persons")
                {
                    Operation = "put",
                    TableName = "Persons",
                    FieldName = "LocationID",
                    ReadableMessage = $"Can not put new entry into Persons because Location with key {p.LocationID} doest not exist."
                };
            }
        }
    }
}