using System;
using System.Collections;
using System.Collections.Generic;
using Apache.Ignite.Core;
using Apache.Ignite.Core.Cache.Query;
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
                        ReadableMessage = $"Can not create account because account with phone {person.Phone} already exists."
                    };
                }
                if (!LocationCache.ContainsKey(person.LocationID))
                {
                    tx.Commit();
                    throw new ReferentialException ("Can not create new account.")
                    {
                        ReadableMessage = $"Can not create account because location with key {person.LocationID} does not exist."
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

        /// <summary>
        /// Simply get all estate objects
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, EstateObject> GetEstateObjects()
        {
            Dictionary<int, EstateObject> result = new Dictionary<int, EstateObject>();
            foreach (var row in ObjectCache.Query(new SqlFieldsQuery("select _key, _val from EstateObjects order by PostDate;")))
            {
                result[(int)row[0]] = (row[1] as EstateObject);
            }
            foreach (var row in HouseCache.Query(new SqlFieldsQuery("select _key, _val from Houses order by PostDate;")))
            {
                result[(int)row[0]] = (row[1] as House);
            }
            foreach (var row in FlatCache.Query(new SqlFieldsQuery("select _key, _val from Flats order by PostDate;")))
            {
                result[(int)row[0]] = (row[1] as Flat);
            }
            foreach (var row in LandplotCache.Query(new SqlFieldsQuery("select _key, _val from Landplots order by PostDate;")))
            {
                result[(int)row[0]] = (row[1] as Landplot);
            }
            return result;
        }

        /// <summary>
        /// Get houses in mentioned location, optionally order and set max price.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="price"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static Dictionary<int, House> GetHouses(int location, int price=0, string order="")
        {
            string querystring1 = "", querystring2 = "";
            switch (order)
            {
                case "new":
                    querystring1 = $"select _key, _val, PostDate, Price, LocationID, isOpen from Houses where LocationID={location} and isOpen=true ";
                    querystring2 = " order by PostDate desc;";
                    break;
                case "price":
                    querystring1 = $"select _key, _val, Price, LocationID, isOpen from Houses where LocationID={location} and isOpen=true ";
                    querystring2 = " order by Price;";
                    break;
                case "prisqm":
                    querystring1 = $"select _key, _val, Price, HomeArea, (Price/HomeArea) as SqmPrice, LocationID, isOpen from Houses where LocationID={location} and isOpen=true ";
                    querystring2 = " order by SqmPrice;";
                    break;
                case "pop":
                    querystring1 = $"select A._key, A._val, A.Price, B.Cnt, A.LocationID, A.isOpen from \"estateobject\".HOUSES as A left join (select ObjectID, count(PersonID) as Cnt from \"bookmark\".BOOKMARKS group by ObjectID) as B on A._key = B.ObjectID where A.LocationID={location} and A.isOpen=true ";
                    querystring2 = " order by B.Cnt desc;";
                    break;
                case "state":
                    querystring1 = $"select _key, _val, Price, LocationID, State, isOpen from Houses where LocationID={location} and isOpen=true ";
                    querystring2 = " order by State desc;";
                    break;

                default:
                    querystring1 = $"select _key, _val, Price, LocationID, isOpen from Houses where LocationID={location} and isOpen=true ";
                    querystring2 = ";";
                    break;
            }

            if (price>0) querystring1 += $" and Price<={price} " + querystring2;
            else querystring1 += querystring2;
            Dictionary<int, House> result = new Dictionary<int, House>();
            foreach (var row in HouseCache.Query(new SqlFieldsQuery(querystring1)))
            {
                result[(int)row[0]] = row[1] as House;
            }
            return result;
        }

        /// <summary>
        /// Get flats in mentioned location, optionally order and set max price.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="price"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static Dictionary<int, Flat> GetFlats(int location, int price=0, string order="")
        {
            string querystring1 = "", querystring2 = "";
            switch (order)
            {
                case "new":
                    querystring1 = $"select _key, _val, PostDate, Price, LocationID, isOpen from Flats where LocationID={location} and isOpen=true ";
                    querystring2 = " order by PostDate desc;";
                    break;
                case "price":
                    querystring1 = $"select _key, _val, Price, LocationID, isOpen from Flats where LocationID={location} and isOpen=true ";
                    querystring2 = " order by Price;";
                    break;
                case "prisqm":
                    querystring1 = $"select _key, _val, Price, HomeArea, (Price/HomeArea) as SqmPrice, LocationID, isOpen from Flats where LocationID={location} and isOpen=true ";
                    querystring2 = " order by SqmPrice;";
                    break;
                case "pop":
                    querystring1 = $"select _key, _val, Price, Cnt, LocationID, isOpen from \"estateobject\".Flats as A left join (select ObjectID, count(PersonID) as Cnt from \"bookmark\".Bookmarks group by ObjectID) as B on A._key = B.ObjectID where LocationID={location} and isOpen=true ";
                    querystring2 = " order by Cnt desc;";
                    break;
                case "state":
                    querystring1 = $"select _key, _val, Price, State, LocationID, isOpen from Flats where LocationID={location} and isOpen=true ";
                    querystring2 = " order by State desc;";
                    break;

                default:
                    querystring1 = $"select _key, _val, Price, LocationID, isOpen from Flats where LocationID={location} and isOpen=true ";
                    querystring2 = ";";
                    break;
            }

            if (price>0) querystring1 += $" and Price<={price} ";
            querystring1 += querystring2;
            Dictionary<int, Flat> result = new Dictionary<int, Flat>();
            foreach (var row in FlatCache.Query(new SqlFieldsQuery(querystring1)))
            {
                result[(int)row[0]] = row[1] as Flat;
            }
            return result;
        }

        /// <summary>
        /// Get landplots in mentioned location, optionally order and set max price.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="price"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static Dictionary<int, Landplot> GetLandplots(int location, int price=0, string order="")
        {
            string querystring1 = "", querystring2 = "";
            switch (order)
            {
                case "new":
                    querystring1 = $"select _key, _val, PostDate, Price, LocationID, isOpen from Landplots where LocationID={location} and isOpen=true ";
                    querystring2 = " order by PostDate desc;";
                    break;
                case "price":
                    querystring1 = $"select _key, _val, Price, LocationID, isOpen from Landplots where LocationID={location} and isOpen=true ";
                    querystring2 = " order by Price;";
                    break;
                case "prisqm":
                    querystring1 = $"select _key, _val, Price, LandArea, (Price/LandArea) as SqmPrice, LocationID, isOpen from Landplots where LocationID={location} and isOpen=true ";
                    querystring2 = " order by SqmPrice;";
                    break;
                case "pop":
                    querystring1 = $"select _key, _val, Price, Cnt, LocationID, isOpen from \"estateobject\".Landplots as A left join (select ObjectID, count(*) as Cnt from \"bookmark\".Bookmarks group by ObjectID) as B on A._key = B.ObjectID where LocationID={location} and isOpen=true ";
                    querystring2 = " order by Cnt desc;";
                    break;
                case "state":
                    querystring1 = $"select _key, _val, Price, State, LocationID, isOpen from Landplots where LocationID={location} and isOpen=true ";
                    querystring2 = " order by State desc;";
                    break;

                default:
                    querystring1 = $"select _key, _val, Price, LocationID, isOpen from Landplots where LocationID={location} and isOpen=true ";
                    querystring2 = ";";
                    break;
            }

            if (price>0) querystring1 += $" and Price<={price} ";
            querystring1 += querystring2;
            Dictionary<int, Landplot> result = new Dictionary<int, Landplot>();
            foreach (var row in LandplotCache.Query(new SqlFieldsQuery(querystring1)))
            {
                result[(int)row[0]] = row[1] as Landplot;
            }
            return result;
        }

        /// <summary>
        /// Get other type bjects in mentioned location, optionally order and set max price.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="price"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public static Dictionary<int, EstateObject> GetEstateObjects(int location, int price=0, string order="")
        {
            string querystring1 = "", querystring2 = "";
            switch (order)
            {
                case "new":
                    querystring1 = $"select _key, _val, PostDate, Price, LocationID, isOpen from EstateObjects where LocationID={location} and isOpen=true ";
                    querystring2 = " order by PostDate desc;";
                    break;
                case "price":
                    querystring1 = $"select _key, _val, Price, LocationID, isOpen from EstateObjects where LocationID={location} and isOpen=true ";
                    querystring2 = " order by Price;";
                    break;
                case "pop":
                    querystring1 = $"select _key, _val, Price, Cnt, LocationID, isOpen from \"estateobject\".EstateObjects as A left join (select ObjectID, count(*) as Cnt from \"bookmark\".Bookmarks group by ObjectID) as B on A._key = B.ObjectID where LocationID={location} and isOpen=true ";
                    querystring2 = " order by Cnt desc;";
                    break;
                case "state":
                    querystring1 = $"select _key, _val, Price, State, LocationID, isOpen from EstateObjects where LocationID={location} and isOpen=true ";
                    querystring2 = " order by State desc;";
                    break;

                default:
                    querystring1 = $"select _key, _val, Price, LocationID, isOpen from EstateObjects where LocationID={location} and isOpen=true ";
                    querystring2 = ";";
                    break;
            }

            if (price>0) querystring1 += $" and Price<={price} " + querystring2;
            else querystring1 += querystring2;
            Dictionary<int, EstateObject> result = new Dictionary<int, EstateObject>();
            foreach (var row in ObjectCache.Query(new SqlFieldsQuery(querystring1)))
            {
                result[(int)row[0]] = row[1] as EstateObject;
            }
            return result;
        }

        /// <summary>
        /// Get list of towns of metioned region. Is used for dropdowns.
        /// </summary>
        public static ICollection<string> GetTownsOfRegion(string region)
        {
            List<string> result = new List<string>();
            foreach (var row in LocationCache.Query(new SqlFieldsQuery($"select distinct Town from Locations where Region='{region}';")))
            {
                result.Add (row[0] as string);
            }
            return result;
        }

        /// <summary>
        /// Get a dictionary where key is location id and value is district name. Is used for dropdowns.
        /// </summary>
        public static Dictionary<string, string> GetDistrictsOfTown(string town)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach (var row in LocationCache.Query(new SqlFieldsQuery($"select _key, District from Locations where Town='{town}';")))
            {
                result[row[0].ToString()] = row[1] as string;
            }
            return result;
        }

        /// <summary>
        /// Get bookmarked objects.
        /// </summary>
        /// <param name="personid"></param>
        /// <returns>Key-value pairs of bookmarked objects' short descriptions.</returns>
        public static Dictionary<int, string> GetBookmarks(int personid)
        {
            List<int> idlist = new List<int>();
            Dictionary<int, string> result = new Dictionary<int, string>();
            foreach (var row in BookmarkCache.Query(new SqlFieldsQuery($"select ObjectID from Bookmarks where PersonID={personid}")))
            {
                idlist.Add ((int)row[0]);
            }
            foreach (var entry in ObjectCache.GetAll(idlist))
            {
                var obj = entry.Value;
                if (obj.Description.Length>200) obj.Description = obj.Description.Substring(0, 180);
                result.Add (entry.Key, $"{obj.PostDate.ToLocalTime()} | {obj.Price} USD | {obj.Description}");
            }
            return result;
        }

        public static IList<IList<object>> GetHouses (int location, int maxprice)
        {
            string query = $"select _key, Price, LocationID, PostDate, Description from \"estateobject\".Houses where LocationID={location} and Price<={maxprice} order by PostDate desc;";
            return ObjectCache.Query(new SqlFieldsQuery(query)).GetAll();
        }

        public static Dictionary<int, EstateObject> FindMatches (ClientWish wish, int limit)
        {
            Dictionary<int, EstateObject> result = new Dictionary<int, EstateObject>();
            string objectvariant;
            switch ((char)(wish.Variant))
            {
                case 'h': objectvariant = "Houses"; break;
                case 'f': objectvariant = "Flats"; break;
                case 'l': objectvariant = "Landplots"; break;
                default : objectvariant = "EstateObjects"; break;
            }
            string query = $"select _key, _val, LocationID, Price, State from \"estateobject\".{objectvariant} where LocationID={wish.LocationID} and Price<={wish.Price} and State>={wish.NeededState} limit {limit};";
            foreach(var row in ObjectCache.Query(new SqlFieldsQuery(query))){
                result [(int)row[0]] = (EstateObject)(row[1]);
            }
            return result;
        }

        public static Dictionary<int, ClientWish> GetWishesByPerson (int id)
        {
            Dictionary<int, ClientWish> result = new Dictionary<int, ClientWish>();
            foreach(var row in ClientWishCache.Query(new SqlFieldsQuery($"select _key, _val, ClientID from ClientWishes where ClientID={id};")))
            {
                result[(int)(row[0])] = (ClientWish)(row[1]);
            }
            return result;
        }

        public static string[] Regions =
        {
            "Вінницька",
            "Волинська",
            "Дніпропетровська",
            "Донецька",
            "Житомирська",
            "Закарпатська",
            "Запорізька",
            "Івано-Франківська",
            "Київська",
            "Кіровоградська",
            "Луганська",
            "Львівська",
            "Миколаївська",
            "Одеська",
            "Полтавська",
            "Рівненська",
            "Сумська",
            "Тернопільська",
            "Харківська",
            "Херсонська",
            "Хмельницька",
            "Черкаська",
            "Чернівецька",
            "Чернігівська",
            "АР Крим",
        };
    }
}