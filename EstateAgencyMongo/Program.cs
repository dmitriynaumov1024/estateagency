using System;
using MongoDB.Driver;
using MongoDB.Bson;

namespace EstateAgencyMongo
{
    class Program
    {
        static void Main ()
        {
            Console.WriteLine("Hell to world!");
            MongoClient client = new MongoClient("mongodb://127.0.0.1:27017");

        }
    }
}
