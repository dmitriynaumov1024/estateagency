using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Data.SQLite;
using Entities;


namespace ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.Write ("Hell to world!\n\n");
            DbClient client = new DbClient("URI=file:database.db");
            client.Connect();
            client.Execute("pragma foreign_keys=ON;");
            /*
            Location l = new Location
            {
                Id = 0,
                Region = "Запорізька",
                Town = "Запоріжжя",
                District = "Вознесенівський"
            };
            */

            foreach (DbDataRecord row in client.Query ("select Id, Region, Town, District from Location;"))
            {
                Console.WriteLine ($" Id       : {row[0]}");
                Console.WriteLine ($" Region   : {row[1]}");
                Console.WriteLine ($" Town     : {row[2]}");
                Console.WriteLine ($" District : {row[3]}");
                Console.WriteLine ("----------------------------------------");
            }

            Console.WriteLine("Table names: ");
            foreach (string i in client.GetTableNames())
            {
                Console.WriteLine(i);
            }
            Console.WriteLine ("----------------------------------------");

            client.Disconnect();
        }
    }
}
