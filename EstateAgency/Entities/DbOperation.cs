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
                LastUsedKeys.Put("estateobject", key+1);
                tx.Commit();
            }
        }

        /// <summary>
        /// Put ClientWish entity into the cache. <br/>
        /// Referential integrity check is done on: <br/>
        /// ClientWish.ClientID - Person.key <br/>
        /// ClientWish.LocationID - Location.key 
        /// </summary>
        /// <param name="value">ClientWish to put into the cache.</param>
        public static void PutClientWish (ClientWish value)
        {
            using (var tx = Client.GetTransactions().TxStart())
            {
                // Error if Person with key = value.ClientID is not found.
                if (!(PersonCache.ContainsKey(value.ClientID)))
                {
                    tx.Commit();
                    throw new ReferentialException ("Can not put new entry into ClientWish cache.")
                    {
                        Operation = "put",
                        TableName = "ClientWish",
                        FieldName = "ClientID",
                        ReadableMessage = $"Can not put new entry into ClientWish cache because Person with key {value.ClientID} does not exist."
                    };
                }

                // Error if Location not found.
                if (!(LocationCache.ContainsKey(value.LocationID)))
                {
                    tx.Commit();
                    throw new ReferentialException ("Can not put new entry into ClientWish cache.")
                    {
                        Operation = "put",
                        TableName = "ClientWish",
                        FieldName = "LocationID",
                        ReadableMessage = $"Can not put new entry into ClientWish cache because Location with key {value.LocationID} does not exist."
                    };
                }

                // Normal operation.
                int key = LastUsedKeys.Get ("clientwish");
                ClientWishCache.Put (key, value);
                LastUsedKeys.Put ("clientwish", key+1);
                tx.Commit();
            }
        }

        /// <summary>
        /// Put bookmark entity into the cache. <br/>
        /// Referential integrity check is done on: <br/>
        /// Bookmark.PersonID - Person.key <br/>
        /// Bookmark.ObjectID - Object.key <br/>
        /// </summary>
        /// <param name="value">Bookmark to put into the cache.</param>
        public static void PutBookmark (Bookmark value)
        {
            using (var tx = Client.GetTransactions().TxStart())
            {
                // Error if Person with key = value.ClientID is not found.
                if (!(PersonCache.ContainsKey(value.PersonID)))
                {
                    tx.Commit();
                    throw new ReferentialException ("Can not put new entry into Bookmark cache.")
                    {
                        Operation = "put",
                        TableName = "Bookmark",
                        FieldName = "PersonID",
                        ReadableMessage = $"Can not put new entry into Bookmark cache because Person with key {value.PersonID} does not exist."
                    };
                }

                // Error if EstateObject with key = value.ObjectID is not found.
                if (!(ObjectCache.ContainsKey(value.ObjectID)))
                {
                    tx.Commit();
                    throw new ReferentialException ("Can not put new entry into Bookmark cache.")
                    {
                        Operation = "put",
                        TableName = "Bookmark",
                        FieldName = "ObjectID",
                        ReadableMessage = $"Can not put new entry into Bookmark cache because EstateObject with key {value.ObjectID} does not exist."
                    };
                }

                // Normal operation
                long key = (((long)value.PersonID)<<32) + value.ObjectID;
                BookmarkCache.Put (key, value);
                tx.Commit();
            }
        }

        /// <summary>
        /// Put match entity into the cache. <br/>
        /// Referential integrity check is done on: <br/>
        /// Match.WishID - ClientWish.key <br/>
        /// Match.ObjectID - Object.key <br/>
        /// </summary>
        /// <param name="value">Match to put into the cache.</param>
        public static void PutMatch (Match value)
        {
            using (var tx = Client.GetTransactions().TxStart())
            {
                // Error if ClientWish with key = value.WishID is not found.
                if (!(ClientWishCache.ContainsKey(value.WishID)))
                {
                    tx.Commit();
                    throw new ReferentialException ("Can not put new entry into Match cache.")
                    {
                        Operation = "put",
                        TableName = "Match",
                        FieldName = "WishID",
                        ReadableMessage = $"Can not put new entry into Match cache because ClientWish with key {value.WishID} does not exist."
                    };
                }

                // Error if EstateObject with key = value.ObjectID is not found.
                if (!(ObjectCache.ContainsKey(value.ObjectID)))
                {
                    tx.Commit();
                    throw new ReferentialException ("Can not put new entry into Match cache.")
                    {
                        Operation = "put",
                        TableName = "Match",
                        FieldName = "ObjectID",
                        ReadableMessage = $"Can not put new entry into Match cache because EstateObject with key {value.ObjectID} does not exist."
                    };
                }
                
                // Normal operation
                long key = (((long)value.WishID)<<32) + value.ObjectID;
                MatchCache.Put (key, value);
                tx.Commit();
            }
        }

        /// <summary>
        /// Put Order entity into the cache. <br/>
        /// NOTE: usually order is put with empty AgentID. <br/>
        /// Referential integrity check is done on: <br/>
        /// Order.ClientID - Person.key <br/>
        /// Order.ObjectID - EstateObject.key
        /// </summary>
        /// <param name="value"></param>
        public static void PutOrder (Order value)
        {
            using (var tx = Client.GetTransactions().TxStart())
            {
                // Error if Person with key = value.ClientID is not found.
                if (!(PersonCache.ContainsKey(value.ClientID)))
                {
                    tx.Commit();
                    throw new ReferentialException ("Can not put new entry into Order cache.")
                    {
                        Operation = "put",
                        TableName = "Order",
                        FieldName = "ClientID",
                        ReadableMessage = $"Can not put new entry into Order cache because Person with key {value.ClientID} does not exist."
                    };
                }

                // Error if EstateObject with key = value.ObjectID is not found.
                if (!(ObjectCache.ContainsKey(value.ObjectID)))
                {
                    tx.Commit();
                    throw new ReferentialException ("Can not put new entry into Order cache.")
                    {
                        Operation = "put",
                        TableName = "Order",
                        FieldName = "ObjectID",
                        ReadableMessage = $"Can not put new entry into Order cache because EstateObject with key {value.ObjectID} does not exist."
                    };
                }

                // Normal operation
                long key = (((long)value.ClientID)<<32) + value.ObjectID;
                OrderCache.Put (key, value);
                tx.Commit();
            }
        }

        /// <summary>
        /// Put deal entity into the cache. <br/>
        /// Referential integrity is checked on: <br/>
        /// Deal.key - EstateObject.key <br/>
        /// Deal.BuyerID - Person.key <br/>
        /// Deal.SellerID - Person.key <br/>
        /// Deal.AgentID - Agent.key <br/>
        /// </summary>
        /// <param name="value">Deal to put into the cache.</param>
        public static void PutDeal (int key, Deal value)
        {
            using (var tx = Client.GetTransactions().TxStart())
            {
                // Error if EstateObject with mentioned key is not found.
                if (!(ObjectCache.ContainsKey(key)))
                {
                    tx.Commit();
                    throw new ReferentialException ("Can not put new entry into Deal cache.")
                    {
                        Operation = "put",
                        TableName = "Deal",
                        FieldName = "key",
                        ReadableMessage = $"Can not put new entry into Deal cache because EstateObject with key {key} does not exist."
                    };
                }

                // Error if Person with key = value.BuyerID is not found.
                if (!(PersonCache.ContainsKey(value.BuyerID)))
                {
                    tx.Commit();
                    throw new ReferentialException ("Can not put new entry into Deal cache.")
                    {
                        Operation = "put",
                        TableName = "Deal",
                        FieldName = "BuyerID",
                        ReadableMessage = $"Can not put new entry into Deal cache because Person with key {value.BuyerID} does not exist."
                    };
                }

                // Error if Person with key = value.SellerID is not found.
                if (!(PersonCache.ContainsKey(value.SellerID)))
                {
                    tx.Commit();
                    throw new ReferentialException ("Can not put new entry into Deal cache.")
                    {
                        Operation = "put",
                        TableName = "Deal",
                        FieldName = "SellerID",
                        ReadableMessage = $"Can not put new entry into Deal cache because Person with key {value.SellerID} does not exist."
                    };
                }

                // Error if Agent with key = value.AgentID is not found.
                if (!(AgentCache.ContainsKey(value.AgentID)))
                {
                    tx.Commit();
                    throw new ReferentialException ("Can not put new entry into Deal cache.")
                    {
                        Operation = "put",
                        TableName = "Deal",
                        FieldName = "AgentID",
                        ReadableMessage = $"Can not put new entry into Deal cache because Agent with key {value.AgentID} does not exist."
                    };
                }

                // Normal operation
                DealCache.Put (key, value);
                tx.Commit();
            }
        }

        /// <summary>
        /// Delete an estate object.
        /// </summary>
        /// <param name="key">Key of object to delete.</param>
        /// <returns>True if object was successfully deleted.</returns>
        public static bool DeleteObject(int key)
        {
            if (DealCache.ContainsKey(key))
            {
                return false;
            }
            OrderCache.Query (new SqlFieldsQuery
                ($"delete from Orders where ObjectID={key};"));
            BookmarkCache.Query (new SqlFieldsQuery
                ($"delete from Bookmarks where ObjectID={key};"));
            MatchCache.Query (new SqlFieldsQuery
                ($"delete from Matches where ObjectID={key};"));
            ObjectCache.Remove(key);
            return true;
        }

    }
}
