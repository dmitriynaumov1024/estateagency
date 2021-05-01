using System;
using System.Collections;
using System.Collections.Generic;
using Apache.Ignite.Core.Cache.Configuration;
using System.Text.RegularExpressions;

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

        public ValidationResult Validate
        {
            get
            {
                if (Password == null || Password.Length < 8 || Password.Length > 32) 
                    return new ValidationResult (
                        "Password should be from 8 to 32 characters long.", 
                        "Password"
                    );

                if (Status!=(byte)'n' && Status!=(byte)'d' && Status!=(byte)'b')
                    return new ValidationResult (
                        "Status can be only 'n' - normal, 'd' - deactivated or 'b' - banned.", 
                        "Status"
                    );

                if (Privilegies!=(byte)'c' && Privilegies!=(byte)'a' && Privilegies!=(byte)'m' && Privilegies!=(byte)'r')
                    return new ValidationResult (
                        "Privilegies can be only 'c' - client, 'a' - agent, 'm' - moderator or 'r' - root.", 
                        "Credential"
                    );

                return ValidationResult.Success;
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
        /// Name of street.
        /// </summary>
        [QuerySqlField] public string StreetName;

        /// <summary>
        /// House number. Should contain a decimal number and optionally a letter at the end.
        /// </summary>
        [QuerySqlField] public string HouseNumber;

        /// <summary>
        /// Flat number.
        /// </summary>
        [QuerySqlField] public short FlatNumber;

        /// <summary>
        /// Date of registration.
        /// </summary>
        [QuerySqlField] public DateTime RegDate;

        public ValidationResult Validate
        {
            get
            {
                if(Surname == null || Surname.Length < 2 || Surname.Length > 64)
                    return new ValidationResult (
                        "Surname must have from 2 to 64 letters.", 
                        "Surname"
                    );

                if(Name == null || Name.Length < 2 || Name.Length > 64)
                    return new ValidationResult (
                        "Name must have from 2 to 64 letters.", 
                        "Name"
                    );

                if(Phone == null || Phone.Length < 10 || Phone.Length > 16)
                    return new ValidationResult (
                        "Phone number must be from 10 to 16 digits long.", 
                        "Phone"
                    );

                foreach (char i in Surname)
                {
                    if (!(char.IsLetter(i) || i=='-' || i==' '))
                        return new ValidationResult (
                            "Surname can only consist of alphabetic letters or dash.", 
                            "Surname"
                        );
                }

                foreach (char i in Name)
                {
                    if (!(char.IsLetter(i) || i=='-' || i==' '))
                        return new ValidationResult (
                            "Name can only consist of alphabetic letters or dash.", 
                            "Name"
                        );
                }

                for (int i=0; i<Phone.Length; i++)
                {
                    if (Phone[i]=='+' && i==0) continue;
                    if (!(char.IsDigit(Phone[i]))) 
                        return new ValidationResult (
                            "Phone must consist only of digits or '+' at the begin.", 
                            "Phone"
                        );
                }
                
                if (Email == null || Email.Length<8 || Email.Length>64) 
                    return new ValidationResult (
                        "Email must be from 8 to 64 characters long.", 
                        "Email"
                    );

                string mch = Regex.Match(Email, @"\b[a-zA-Z0-9._-]+@[a-zA-Z0-9][a-zA-Z0-9.-]{0,61}[a-zA-Z0-9]\.[a-zA-Z.]{2,6}\b").Value;
                if (mch != Email)
                    return new ValidationResult (
                        $"Regex: {mch} - Email must consist only of latin alphabetic letters, digits, '@' symbol, dot or dash. If your email is real and doesn't correspond to this criteria, we are sorry.",
                        "Email"
                    );

                if (StreetName == null)
                    return new ValidationResult (
                        "Street name must not be empty.",
                        "StreetName"
                    );

                foreach (char i in StreetName)
                {
                    if (!(char.IsLetter(i) || char.IsDigit(i) || i=='.' || i=='-' || i==' '))
                        return new ValidationResult (
                            "Street name must contain only alphabetic letters, digits, dot or dash.",
                            "Email"
                        );
                }

                if (HouseNumber.Length==0 || HouseNumber.Length>5 || HouseNumber[0]=='0' || 
                    ((HouseNumber.Length - Regex.Match(HouseNumber, "\\d+").Value.Length) > 1) || 
                    (Regex.Match(HouseNumber, "\\d+").Index!=0))
                    return new ValidationResult (
                        "Invalid house number.",
                        "HouseNumber"
                    );

                if (FlatNumber > 1000) 
                    return new ValidationResult (
                        "Invalid flat number.",
                        "FlatNumber"
                    );

                if(RegDate > DateTime.UtcNow)
                    return new ValidationResult (
                        "Signup date can not be after current date and time.",
                        "RegDate"
                    );

                return ValidationResult.Success;
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
        [QuerySqlField] public short TotalDeals;

        /// <summary>
        /// Amount of current month deals.
        /// </summary>
        [QuerySqlField] public short MonthDeals;

        /// <summary>
        /// Current month payment in USD.
        /// </summary>
        [QuerySqlField] public int MonthPayment;

        public ValidationResult Validate
        {
            get
            {
                if (TotalDeals < 0 || MonthDeals < 0 || MonthPayment < 0 || TotalDeals < MonthDeals) return ValidationResult.Fail; 
                return ValidationResult.Success;
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

        public ValidationResult Validate
        {
            get
            {
                if (Region == null || Region.Length < 5 || Region.Length > 30) 
                    return new ValidationResult(
                        "Region name length should be from 5 to 30 characters.",
                        "Region"
                    );

                if (Town == null || Town.Length < 2 || Town.Length > 30) 
                    return new ValidationResult(
                        "Town name length should be from 2 to 30 characters.",
                        "Region"
                    );

                if (District!=null && (District.Length > 30) || District.Length < 2)
                    return new ValidationResult(
                        "District name length should be from 2 to 30 characters.",
                        "Region"
                    );

                return ValidationResult.Success;
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
        /// Name of street.
        /// </summary>
        [QuerySqlField] public string StreetName;

        /// <summary>
        /// House number. Should contain a decimal number and optionally a letter at the end.
        /// </summary>
        [QuerySqlField] public string HouseNumber;

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

        public virtual ValidationResult Validate
        {
            get
            {
                if (StreetName == null)
                    return new ValidationResult (
                        "Street name must not be empty.",
                        "StreetName"
                    );

                foreach (char i in StreetName)
                {
                    if (!(char.IsLetter(i) || char.IsDigit(i) || i=='.' || i=='-' || i==' '))
                        return new ValidationResult (
                            "Street name must contain only alphabetic letters, digits, dot or dash.",
                            "Email"
                        );
                }

                if (HouseNumber.Length==0 || HouseNumber.Length>5 || HouseNumber[0]=='0' || 
                    ((HouseNumber.Length - Regex.Match(HouseNumber, "\\d+").Value.Length) > 1) || 
                    (Regex.Match(HouseNumber, "\\d+").Index!=0))
                    return new ValidationResult (
                        "Invalid house number.",
                        "HouseNumber"
                    );

                if (Variant!=(byte)'o') 
                    return new ValidationResult (
                        "Variant of basic Estate object can only be 'o' - other.",
                        "Variant"
                    );

                if (State < 0 || State > 5)
                    return new ValidationResult (
                        "State can be only from 0 to 5.",
                        "State"
                    );

                if (Description == null || Description.Length > 2000 || Description.Length < 10)
                    return new ValidationResult (
                        "Description of object must be from 10 to 2000 characters long.",
                        "Description"
                    );

                if (Tags!=null && Tags.Count > 20) 
                    return new ValidationResult (
                        "Only 20 tags are allowed.",
                        "Tags"
                    );

                if (PhotoUrls == null || PhotoUrls.Count < 1 || PhotoUrls.Count > 10)
                    return new ValidationResult (
                        "You should provide from 1 to 10 photos.",
                        "PhotoUrls"
                    );

                return ValidationResult.Success;
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

        public override ValidationResult Validate
        {
            get
            {
                var v = base.Validate;
                if (!(v.isValid) && v.FieldName!="Variant") return v;

                if (Variant!='h') 
                    return new ValidationResult ("Variant must be 'h' for a house.", "Variant");

                if (LandArea < 1 || LandArea > 1000F)
                    return new ValidationResult ("Land area must be in [1..1000] range.", "LandArea");

                if (HomeArea < 1 || HomeArea > 40000F)
                    return new ValidationResult ("Home area must be in [1..40000] range.", "HomeArea");

                if (FloorCount < 1 || FloorCount > 10)
                    return new ValidationResult ("Floor count must be in [1..10] range.", "FloorCount");

                if (RoomCount < 1 || RoomCount > 100) 
                    return new ValidationResult ("Room count must be in [1..100] range.", "RoomCount");

                return ValidationResult.Success;
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

        /// <summary>
        /// Flat number.
        /// </summary>
        [QuerySqlField] public short FlatNumber;

        public override ValidationResult Validate
        {
            get
            {
                var v = base.Validate;
                if (!(v.isValid) && v.FieldName!="Variant") return v;

                if (Variant!='f') 
                    return new ValidationResult ("Variant must be 'f' for a flat.", "Variant");

                if (HomeArea < 1 || HomeArea > 400F)
                    return new ValidationResult ("Home area must be in [1..40000] range.", "HomeArea");

                if (Floor < 0 || Floor > 100)
                    return new ValidationResult ("Floor number must be in [1..100] range.", "FloorCount");

                if (RoomCount < 1 || RoomCount > 10) 
                    return new ValidationResult ("Room count must be in [1..10] range.", "RoomCount");

                if (FlatNumber < 1 || FlatNumber > 1000)
                    return new ValidationResult ("Flat number must be in [1.1000] range.", "FlatNumber");

                return ValidationResult.Success;
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

        public override ValidationResult Validate
        {
            get
            {
                var v = base.Validate;
                if (!(v.isValid) && v.FieldName!="Variant") return v;

                if (Variant!='l') 
                    return new ValidationResult ("Variant must be 'l' for landplot.", "Variant");

                if (LandArea < 1 || LandArea > 1000F)
                    return new ValidationResult ("Land area must be in [1..1000] range.", "LandArea");

                return ValidationResult.Success;
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

        public ValidationResult Validate
        {
            get
            {
                if (PostDate > DateTime.UtcNow) 
                    return new ValidationResult ("Post date must not be after now.", "PostDate");

                if (!(Variant==(byte)'o' || Variant==(byte)'h' || Variant==(byte)'f' || Variant==(byte)'l'))
                    return new ValidationResult ("Variant can be only 'o' - other, 'h' - house, 'f' - flat or 'l' - landplot.", "Variant");

                if (Price < 1) 
                    return new ValidationResult ("Price must be greater than 0.", "Price");
                
                if (Tags!=null && Tags.Count > 20)
                    return new ValidationResult ("No more than 20 tags are allowed.", "Tags");

                if (NeededState < 0 || NeededState > 5)
                    return new ValidationResult ("Needed state must be from 0 to 5.", "Needed state");

                return ValidationResult.Success;
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
