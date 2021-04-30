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
            using (var tx = Client.GetTransactions().TxStart())
            {
                // Check if Location cache contains Location with key p.LocationID, throw error if not.
                if (!(LocationCache.ContainsKey(p.LocationID)))
                {
                    tx.Commit();
                    throw new ReferentialException ("Can not put new entry into Person cache.")
                    {
                        Operation = "put",
                        TableName = "Person",
                        FieldName = "LocationID",
                        ReadableMessage = $"Can not put new entry into Person cache because Location with key {p.LocationID} does not exist."
                    };
                }

                // Check if Credential cache contains Credential with key p.Phone, throw error if yes.
                if (CredentialCache.ContainsKey(p.Phone))
                {
                    tx.Commit();
                    throw new ReferentialException ("Can not put new entry into Person cache.")
                    {
                        Operation = "put",
                        TableName = "Person",
                        FieldName = "Phone",
                        ReadableMessage = $"Can not put new entry into Person cache because Person with Phone '{p.Phone}' and Credential with the same key already exist."
                    };
                }

                // Normal operation.
                int key = LastUsedKeys.Get("person");
                PersonCache.Put(key, p);
                key++;
                LastUsedKeys.Put ("person", key);
                tx.Commit();
            }
        }

        /// <summary>
        /// Put credential in cache and assign string key which is the phone number of the referenced person.
        /// </summary>
        /// <param name="value">Credential to put in the cache.</param>
        public static void PutCredential (Credential value)
        {
            using (var tx = Client.GetTransactions().TxStart())
            {
                if (PersonCache.ContainsKey(value.PersonID)) 
                {
                    CredentialCache.Put (PersonCache.Get(value.PersonID).Phone, value);
                    tx.Commit();
                }
                else 
                { 
                    tx.Commit();
                    throw new ReferentialException ("Can not put new entry into Credential cache.")
                    {
                        Operation = "put",
                        TableName = "Credential",
                        FieldName = "PersonID",
                        ReadableMessage = $"Can not put new entry into Credential cache because Person with key {value.PersonID} does not exist."
                    };
                }
            }
        }

        /// <summary>
        /// Put agent entity into the cache and assign a key.
        /// </summary>
        /// <param name="key">Key to use. Is additionally a back link to person.</param>
        /// <param name="value">Agent entity to put into the cache.</param>
        public static void PutAgent (int key, Agent value)
        {
            using (var tx = Client.GetTransactions().TxStart())
            {
                // Check whether Person cache contains Person with mentioned key. Throw an error if not.
                if (!(PersonCache.ContainsKey(key)))
                {
                    tx.Commit();
                    throw new ReferentialException ("Can not put new entry into Agent cache.")
                    {
                        Operation = "put",
                        TableName = "Agent",
                        FieldName = "key",
                        ReadableMessage = $"Can not put new entry into Agent cache because Person with key {key} does not exist."
                    };
                }

                // Normal operation.
                AgentCache.Put (key, value);
                tx.Commit();
            }
        }

        /// <summary>
        /// Put location entity into Location cache.
        /// </summary>
        /// <param name="value">Location entity to put into cache.</param>
        public static void PutLocation (Location value)
        {
            using (var tx = Client.GetTransactions().TxStart())
            {
                int key = LastUsedKeys.Get("location");
                LocationCache.Put(key, value);
                key++;
                LastUsedKeys.Put("location", key);
                tx.Commit();
            }
        }

        /// <summary>
        /// The one method to put different types of estate objects into caches. <br/>
        /// Key is chosen automatically. <br/>
        /// Method checks referential integrity on: <br/>
        /// EstateObject.SellerID - Person.key. <br/>
        /// EstateObject.LocationID - Location.key.
        /// </summary>
        /// <param name="value">Estate object to put into cache.</param>
        public static void PutEstateObject (EstateObject value)
        {
            using (var tx = Client.GetTransactions().TxStart())
            {
                // Error if Person with key = value.SellerID is not found.
                if (!(PersonCache.ContainsKey(value.SellerID)))
                {
                    tx.Commit();
                    throw new ReferentialException ("Can not put new entry into EstateObject cache.")
                    {
                        Operation = "put",
                        TableName = "EstateObject",
                        FieldName = "SellerID",
                        ReadableMessage = $"Can not put new entry into EstateObject cache because Person with key {value.SellerID} does not exist."
                    };
                }

                // Error if Location not found.
                if (!(LocationCache.ContainsKey(value.LocationID)))
                {
                    tx.Commit();
                    throw new ReferentialException ("Can not put new entry into EstateObject cache.")
                    {
                        Operation = "put",
                        TableName = "EstateObject",
                        FieldName = "LocationID",
                        ReadableMessage = $"Can not put new entry into EstateObject cache because Location with key {value.LocationID} does not exist."
                    };
                }

                // Normal operation.
                int key = LastUsedKeys.Get("estateobject");
                switch ((char)value.Variant)
                {
                    case 'o':
                    ObjectCache.Put (key, value);
                    break;

                    case 'h':
                    HouseCache.Put (key, (House)value);
                    break;
                    
                    case 'f':
                    FlatCache.Put (key, (Flat)value);
                    break;

                    case 'l':
                    LandplotCache.Put (key, (Landplot)value);
                    break;

                    default: 
                    break;
                }
                key++;
                LastUsedKeys.Put("estateobject", key);
                tx.Commit();
            }
        }


    }
}
