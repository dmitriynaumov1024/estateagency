using System;
using System.Collections;
using System.Collections.Generic;
using Apache.Ignite.Core;
using Apache.Ignite.Core.Cache.Query;
using Apache.Ignite.Core.Client;
using Apache.Ignite.Core.Client.Cache;
using Apache.Ignite.Core.Cache.Configuration;
using Apache.Ignite.Core.Client.Transactions;
using static EstateAgency.Database.DbClient;
using EstateAgency.Entities;

namespace EstateAgency.Database
{
    public static partial class DbAdvanced
    {
        /// <summary>
        /// Create new account, using Person entity, password, and optionally privilegies level.
        /// </summary>
        /// <param name="person"></param>
        /// <param name="password"></param>
        /// <param name="privilegies"></param>
        public static void CreateAccount(Person person, string password, char privilegies = 'c')
        {
            using (var tx = Client.GetTransactions().TxStart())
            { 
                // Referential integrity check
                if (CredentialCache.ContainsKey(person.Phone))
                {
                    tx.Commit();
                    throw new ReferentialException ("Can not create new account.")
                    {
                        ReadableMessage = $"Con not create account because account with phone {person.Phone} already exists."
                    };
                }
                if (!LocationCache.ContainsKey(person.LocationID))
                {
                    tx.Commit();
                    throw new ReferentialException ("Can not create new account.")
                    {
                        ReadableMessage = $"Con not create account because location with key {person.LocationID} does not exist."
                    };
                }

                // Normal operation
                int key = LastUsedKeys.Get("person");
                PersonCache.Put (key, person);
                LastUsedKeys.Put ("person", key+1);

                Credential c = new Credential
                {
                    Password = password,
                    Privilegies = (byte)privilegies,
                    Status = (byte)'n',
                    PersonID = key
                };

                CredentialCache.Put (person.Phone, c);
                tx.Commit();
            }
        }

        /// <summary>
        /// Check credentials.
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="password"></param>
        /// <returns>
        /// 'n' if normal <br/>
        /// 'a' if account was not found <br/>
        /// 'p' if wrong password <br/>
        /// 'b' if account is banned <br/>
        /// 'd' if account is deactivated.
        /// </returns>
        public static char CheckCredentials (string phone, string password)
        {
            Credential cr = new Credential();
            bool accountPresent = CredentialCache.TryGet(phone, out cr);
            if(!accountPresent) 
                return 'a';
            if(cr.Status==(byte)'b') 
                return 'b';
            if(cr.Password == password)
            {
                if (cr.Status==(byte)'d') 
                    return 'd';
                return 'n';
            }
            return 'p';
        }

        public static ICollection<EstateObject> GetEstateObjects()
        {
            List<EstateObject> result = new List<EstateObject>();
            foreach (var row in ObjectCache.Query(new SqlFieldsQuery("select _val from EstateObjects order by PostDate;")))
            {
                result.Add (row[0] as EstateObject);
            }
            foreach (var row in HouseCache.Query(new SqlFieldsQuery("select _val from Houses order by PostDate;")))
            {
                result.Add (row[0] as House);
            }
            foreach (var row in FlatCache.Query(new SqlFieldsQuery("select _val from Flats order by PostDate;")))
            {
                result.Add (row[0] as Flat);
            }
            foreach (var row in LandplotCache.Query(new SqlFieldsQuery("select _val from Landplots order by PostDate;")))
            {
                result.Add (row[0] as Landplot);
            }
            return result;
        }
    }
}