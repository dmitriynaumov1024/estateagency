using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Represents a set of Entities of Estate agency.
/// </summary>
namespace Entities
{
    /// <summary>
    /// Credential. Contains back link to Person, 
    /// information about Password, privilegies and status.
    /// </summary>
    public class Credential
    {
        /// <summary>
        /// Back link to Person. Mandatory field. Credential can not exist without a Person.
        /// </summary>
        public int PersonID;

        /// <summary>
        /// Password.
        /// </summary>
        public string Password;

        /// <summary>
        /// Privilegies. Currently possible values are: <br/>
        /// 'c' - client <br/>
        /// 'a' - agent  <br/>
        /// 'm' - moderator <br/>
        /// 'r' - root. 
        /// </summary>
        public byte Privilegies;

        /// <summary>
        /// Status of account. Currently possible values are: <br/>
        /// 'n' - normal <br/>
        /// 'd' - deactivated <br/>
        /// 'b' - banned.
        /// </summary>
        public byte Status;
    }

    public class Person
    {
        public string Surname;
        public string Name;
        public string Phone;
        public string Email;
        public int LocationID;
        public string Address;
        public DateTime RegDate;
    }

    public class Agent
    {
        public int TotalDeals;
        public int MonthDeals;
        public int MonthPayment;
    }

    public class Location
    {
        public string Region;
        public string Town;
        public string District;
    }

    public class EstateObject
    {
        public int SellerID;
        public DateTime PostDate;
        public bool isOpen;
        public bool isVisible;
        public int LocationID;
        public string Address;
        public byte Variant;
        public int Price;
        public byte State;
        public string Description;
        public List<string> Tags;
        public List<string> PhotoUrls;
    }

    public class House
    {
        public float LandArea;
        public float HomeArea;
        public int FloorCount;
        public int RoomCount;
    }

    public class Flat
    {
        public int Floor;
        public float HomeArea;
        public int RoomCount;
    }

    public class Landplot
    {
        public float LandArea;
    }

    public class ClientWish
    {
        public int ClientID;
        public bool isOpen;
        public DateTime PostDate;
        public int LocationID;
        public char Variant;
        public int Price;
        public List<string> Tags;
        public byte NeededState;
    }

    public class Bookmark
    {
        public int PersonID;
        public int ObjectID;
    }

    public class Match
    {
        public int WishID;
        public int ObjectID;
    }

    public class Order
    {
        public int ClientID;
        public int ObjectID;
        public int AgentID;
        public DateTime OrderTime;
        public bool isOpen;
    }

    public class Deal
    {
        public int BuyerID;
        public int SellerID;
        public int AgentID;
        public DateTime DealTime;
        public int Price;
    }
}
