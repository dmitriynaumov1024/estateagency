using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EstateAgency.Database;
using Microsoft.AspNetCore.Http;

namespace EstateAgencyWeb.Controllers
{
    public class AjaxController : Controller
    {
        [HttpGet("/ajax/gettownsofregion")]
        public ICollection<string> GetTownsOfRegion(string region)
        {
            return DbAdvanced.GetTownsOfRegion(region);
        }

        [HttpGet("/ajax/getdistrictsoftown")]
        public Dictionary<string, string> GetDistrictsOfTown(string town)
        {
            return DbAdvanced.GetDistrictsOfTown(town);
        }

        [HttpPost("/ajax/addbookmark")]
        public bool AddBookmark(int objectid)
        {
            int? personid = HttpContext.Session.GetInt32("PersonID");
            if(personid==null) return false;
            try 
            { 
                DbClient.PutBookmark (new EstateAgency.Entities.Bookmark { ObjectID = objectid, PersonID = (int)personid });
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [HttpPost("/ajax/delbookmark")]
        public bool DelBookmark(int objectid)
        {
            int? personid = HttpContext.Session.GetInt32("PersonID");
            if(personid==null) return false;
            try 
            { 
                return DbClient.BookmarkCache.Remove((long)personid<<32+objectid);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
