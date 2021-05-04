using System;
using System.Collections;
using System.Collections.Generic;
using Apache.Ignite.Core;
using Apache.Ignite.Core.Cache.Query;
using Apache.Ignite.Core.Client;
using Apache.Ignite.Core.Client.Cache;
using EstateAgency.Entities;
using EstateAgency.Database;

namespace EstateAgencyConsole
{
    class Program
    {
        
        /// <summary>
        /// Main entry point for application. The main task 
        /// of the program is to create database of Estate agency.
        /// </summary>

        static void Main ()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.Unicode;

            DbClient.Connect();
            DbClient.SetClusterActive(true);
            DbClient.GetDatabase();

            Console.WriteLine ("Caches:");
            foreach (string i in DbClient.Client.GetCacheNames())
                Console.WriteLine (i);
            Console.WriteLine ("-----------------------------------------------------------");

            char c = '0';

            do
            {
                Console.WriteLine("Select variant: \n 1 - put Location \n 2 - get all locations \n 3 - reset counter \n 4 - delete all \n ~ - exit.");
                c = Console.ReadLine()[0];
                switch (c)
                {
                    case '1':
                        Location loc = new Location();
                        Console.Write ("[Region  ]: ");
                        loc.Region = Console.ReadLine();
                        Console.Write ("[Town    ]: ");
                        loc.Town = Console.ReadLine();
                        Console.Write ("[District]: ");
                        loc.District = Console.ReadLine();
                        var v = loc.Validate;
                        if(v.isValid) 
                        { 
                            Console.WriteLine("Success!");
                            DbClient.PutLocation(loc);
                        }
                        else
                        {
                            Console.WriteLine("Validation failed.");
                            Console.WriteLine(v.Message);
                            Console.WriteLine(loc.Region);
                            Console.WriteLine(loc.Town);
                            Console.WriteLine(loc.District);
                        }
                        break;

                    case '2':
                        int i = 0;
                        foreach (var row in DbClient.LocationCache.Query(new SqlFieldsQuery("select _key, Region, Town, District from Locations;")))
                        {
                            Console.WriteLine ($"      key : {row[0]}");
                            Console.WriteLine ($"   Region : {row[1]}");
                            Console.WriteLine ($"     Town : {row[2]}");
                            Console.WriteLine ($" District : {row[3]}");
                            Console.WriteLine ();
                            i++;
                        }
                            Console.WriteLine ($"    Total : {i} rows");
                        break;

                    case '3':
                        Console.Write ("Sure? [y/n] ");
                        c = Console.ReadLine()[0];
                        if (c == 'y')
                        { 
                            DbClient.LastUsedKeys.Put("location", 0);
                            Console.WriteLine ("Counter was reset to 0!");
                        }
                        else
                        {
                            Console.WriteLine ("Okay, cancelling.");
                        }
                        break;

                    case '4':
                        Console.Write ("Sure? [y/n] ");
                        c = Console.ReadLine()[0];
                        if (c == 'y')
                        { 
                            DbClient.LocationCache.RemoveAll();
                            DbClient.LastUsedKeys.Put("location", 0);
                            Console.WriteLine ("Everything was deleted!");
                        }
                        else
                        {
                            Console.WriteLine ("Okay, cancelling.");
                        }
                        break;

                    default: 
                        break;
                }
                Console.WriteLine("-----------------------------------------------------------");
            } while (c!='~');
            
            Console.WriteLine("[Done. Press ENTER to continue.]");
            Console.Read();
            DbClient.Disconnect();
        }
    }
}
