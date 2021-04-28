using System;
using System.Collections;
using System.Collections.Generic;
using Apache.Ignite.Core.Cache.Configuration;

/// <summary>
/// Represents a set of Entities of Estate agency.
/// From now, 'Entities' is a database adapter with built-in types.
/// Apache.Ignite is referenced.
/// Each entity class should contain methods for data validation.
/// </summary>
namespace EstateAgency.Entities
{
    /// <summary>
    /// Credential. Contains back link to Person, 
    /// information about Password, privilegies and status.
    /// </summary>
    public class Credential: IValidable
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

        public bool isValid
        {
            get
            {
                return true;
            }
        }
    }

    /// <summary>
    /// Contains information about person.
    /// </summary>
    public class Person: IValidable
    {
        /// <summary>
        /// Surname
        /// </summary>
        [QuerySqlField] public string Surname;

        /// <summary>
        /// Name
        /// </summary>
        [QuerySqlField] public string Name;

        /// <summary>
        /// Phone number. Should consist of numerical characters, starting with '+'.
        /// </summary>
        [QuerySqlField] public string Phone;

        /// <summary>
        /// Email address.
        /// </summary>
        [QuerySqlField] public string Email;

        /// <summary>
        /// ID of location.
        /// </summary>
        [QuerySqlField] public int LocationID;

        /// <summary>
        /// Address. Should consist of street name, home number and optionally flat number.
        /// </summary>
        [QuerySqlField] public string Address;

        /// <summary>
        /// Date of registration.
        /// </summary>
        [QuerySqlField] public DateTime RegDate;

        public bool isValid
        {
            get
            {
                return true;
            }
        }
    }

    /// <summary>
    /// Descriptive entity. Contains information of agent.
    /// </summary>
    public class Agent: IValidable
    {
        /// <summary>
        /// Total amount of deals.
        /// </summary>
        [QuerySqlField] public int TotalDeals;

        /// <summary>
        /// Amount of current month deals.
        /// </summary>
        [QuerySqlField] public int MonthDeals;

        /// <summary>
        /// Current month payment in USD.
        /// </summary>
        [QuerySqlField] public int MonthPayment;

        public bool isValid
        {
            get
            {
                return true;
            }
        }
    }

    /// <summary>
    /// Contains info about location: region, town/settlement name, district name.
    /// </summary>
    public class Location: IValidable
    {
        /// <summary>
        /// Name of region.
        /// </summary>
        [QuerySqlField] public string Region;

        /// <summary>
        /// Name of town or village.
        /// </summary>
        [QuerySqlField] public string Town;

        /// <summary>
        /// Name of district, if it is present.
        /// </summary>
        [QuerySqlField] public string District;

        public bool isValid
        {
            get
            {
                return true;
            }
        }
    }

    /// <summary>
    /// Base estate object entity. Contains common fields.
    /// </summary>
    public class EstateObject: IValidable
    {
        /// <summary>
        /// ID of seller.
        /// </summary>
        [QuerySqlField] public int SellerID;

        /// <summary>
        /// Post date.
        /// </summary>
        [QuerySqlField] public DateTime PostDate;

        /// <summary>
        /// Indicates whether this object can be bought.
        /// </summary>
        [QuerySqlField] public bool isOpen;

        /// <summary>
        /// Indicates whether this object is visible to clients.
        /// </summary>
        [QuerySqlField] public bool isVisible;

        /// <summary>
        /// ID of location.
        /// </summary>
        [QuerySqlField] public int LocationID;

        /// <summary>
        /// Address of object. Should include street name, home number and flat number, if available.
        /// </summary>
        [QuerySqlField] public string Address;

        /// <summary>
        /// Variant. Possible values: <br/>
        /// 'h' - house <br/>
        /// 'f' - flat <br/>
        /// 'l' - landplot <br/>
        /// 'o' - other.
        /// </summary>
        [QuerySqlField] public byte Variant;

        /// <summary>
        /// Price of the object measured in USD.
        /// </summary>
        [QuerySqlField] public int Price;

        /// <summary>
        /// Overall state of object. Can be from 0 to 5.
        /// </summary>
        [QuerySqlField] public byte State;

        /// <summary>
        /// Description of the object.
        /// </summary>
        [QuerySqlField] public string Description;

        /// <summary>
        /// Collection of tags.
        /// </summary>
        [QuerySqlField] public ICollection<string> Tags;

        /// <summary>
        /// Collection of photo Urls.
        /// </summary>
        [QuerySqlField] public ICollection<string> PhotoUrls;

        public virtual bool isValid
        {
            get
            {
                return true;
            }
        }
    }

    /// <summary>
    /// House inherits basic estate object entity, contains specific information about house.
    /// </summary>
    public class House: EstateObject, IValidable
    {
        /// <summary>
        /// Land area measured in a (1 a = 100 sq.m)
        /// </summary>
        [QuerySqlField] public float LandArea;

        /// <summary>
        /// Home area measured in sq.m.
        /// </summary>
        [QuerySqlField] public float HomeArea;

        /// <summary>
        /// Floor count.
        /// </summary>
        [QuerySqlField] public short FloorCount;

        /// <summary>
        /// Room count.
        /// </summary>
        [QuerySqlField] public short RoomCount;

        public override bool isValid
        {
            get
            {
                return true;
            }
        }
    }

    /// <summary>
    /// Inherits basic estate object entity, contains specific information about flat.
    /// </summary>
    public class Flat: EstateObject, IValidable
    {
        /// <summary>
        /// Home area measured in sq.m.
        /// </summary>
        [QuerySqlField] public float HomeArea;

        /// <summary>
        /// Floor number.
        /// </summary>
        [QuerySqlField] public short Floor;

        /// <summary>
        /// Count of rooms.
        /// </summary>
        [QuerySqlField] public short RoomCount;

        public override bool isValid
        {
            get
            {
                return true;
            }
        }
    }

    /// <summary>
    /// Inherits basic estate object entity, contains specific information about land plot.
    /// </summary>
    public class Landplot: EstateObject, IValidable
    {
        /// <summary>
        /// Land area measured in a (1 a = 100 sq.m)
        /// </summary>
        [QuerySqlField] public float LandArea;

        public override bool isValid
        {
            get
            {
                return true;
            }
        }
    }

    /// <summary>
    /// Contains description of client wish.
    /// </summary>
    public class ClientWish: IValidable
    {
        /// <summary>
        /// ID of client.
        /// </summary>
        [QuerySqlField] public int ClientID;

        /// <summary>
        /// Date of posting the wish.
        /// </summary>
        [QuerySqlField] public DateTime PostDate;

        /// <summary>
        /// Indicates whether this wish is still actual.
        /// </summary>
        [QuerySqlField] public bool isOpen;

        /// <summary>
        /// Variant. Possible values: <br/>
        /// 'h' - house <br/>
        /// 'f' - flat <br/>
        /// 'l' - landplot <br/>
        /// 'o' - other.
        /// </summary>
        [QuerySqlField] public byte Variant;

        /// <summary>
        /// ID of location.
        /// </summary>
        [QuerySqlField] public int LocationID;

        /// <summary>
        /// Max price client is able to pay in USD.
        /// </summary>
        [QuerySqlField] public int Price;

        /// <summary>
        /// Collection of tags.
        /// </summary>
        [QuerySqlField] public ICollection<string> Tags;

        /// <summary>
        /// Needed state of object. Should be in [0...5] range.
        /// </summary>
        [QuerySqlField] public byte NeededState;

        public bool isValid
        {
            get
            {
                return true;
            }
        }
    }

    /// <summary>
    /// Contains information about bookmark.
    /// </summary>
    public class Bookmark
    {
        /// <summary>
        /// ID of user.
        /// </summary>
        [QuerySqlField] public int PersonID;

        /// <summary>
        /// ID of object.
        /// </summary>
        [QuerySqlField] public int ObjectID;
    }

    /// <summary>
    /// Contains information about wish-object match.
    /// </summary>
    public class Match
    {
        /// <summary>
        /// ID of wish.
        /// </summary>
        [QuerySqlField] public int WishID;

        /// <summary>
        /// ID of object.
        /// </summary>
        [QuerySqlField] public int ObjectID;
    }

    /// <summary>
    /// Contains information about order.
    /// </summary>
    public class Order
    {
        /// <summary>
        /// ID of client.
        /// </summary>
        [QuerySqlField] public int ClientID;

        /// <summary>
        /// ID of object.
        /// </summary>
        [QuerySqlField] public int ObjectID;

        /// <summary>
        /// ID of agent.
        /// </summary>
        [QuerySqlField] public int AgentID;

        /// <summary>
        /// Date and time of order.
        /// </summary>
        [QuerySqlField] public DateTime OrderTime;

        /// <summary>
        /// Indicates whether the order is still actual.
        /// </summary>
        [QuerySqlField] public bool isOpen;
    }

    /// <summary>
    /// Contains information about deal.
    /// </summary>
    public class Deal
    {
        /// <summary>
        /// ID of buyer.
        /// </summary>
        [QuerySqlField] public int BuyerID;

        /// <summary>
        /// ID of seller.
        /// </summary>
        [QuerySqlField] public int SellerID;

        /// <summary>
        /// ID of agent.
        /// </summary>
        [QuerySqlField] public int AgentID;

        /// <summary>
        /// Date and time of deal.
        /// </summary>
        [QuerySqlField] public DateTime DealTime;

        /// <summary>
        /// Final price of deal measured in USD.
        /// </summary>
        [QuerySqlField] public int Price;
    }
}
