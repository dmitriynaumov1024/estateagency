using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EstateAgency.Database;

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
    }
}
