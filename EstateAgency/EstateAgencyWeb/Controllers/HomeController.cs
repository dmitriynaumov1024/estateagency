using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using EstateAgencyWeb.Models;
using EstateAgency.Entities;
using EstateAgency.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Session;
using System.IO;


namespace EstateAgencyWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public ActionResult Index()
        {
            return View("Index");
        }

        [HttpGet("/Login")]
        public IActionResult LoginGet()
        {
            return View("Login");
        }

        [HttpPost("/Login")]
        public IActionResult LoginPost(string phone, string password)
        {
            if (phone==null || password==null) 
            {
                ViewData["ErrorMessage"] = "Phone or password fields were empty.";
                return View ("Login");
            }

            switch (DbAdvanced.CheckCredentials(phone, password))
            {
                case 'n':
                    HttpContext.Session.SetString("Phone", phone);
                    Credential cr = DbClient.CredentialCache.Get(phone);
                    HttpContext.Session.SetInt32("PersonID", cr.PersonID);
                    HttpContext.Session.SetString("Privilegies", ((char)cr.Privilegies).ToString());
                    ViewData["LoggedIn"]=true;
                    return RedirectToAction ("Explore");
                
                case 'p':
                    ViewData["ErrorMessage"] = "Password is wrong. Try again.";
                    break;

                case 'a':
                    ViewData["ErrorMessage"] = $"Account with phone '{phone}' was not found.";
                    break;

                case 'b':
                    ViewData["ErrorMessage"] = $"Account with phone '{phone}' was permanently banned.";
                    break;

                case 'd':
                    ViewData["ErrorMessage"] = $"Account with phone '{phone}' was deactivated.";
                    break;

                default:
                    ViewData["ErrorMessage"] = "Unknown error occured. Try again.";
                    break;
            }

            return View("Login");
        }

        [HttpGet("/Signup")]
        public ActionResult SignupGet()
        {
            return View("Signup");
        }

        [HttpPost("/Signup")]
        public ActionResult Signup()
        {
            Person p = new Person{ 
                Phone = Request.Form["phone"],
                Email = Request.Form["email"],
                Name = Request.Form["name"], 
                Surname = Request.Form["surname"], 
                LocationID = int.Parse(Request.Form["location"]), 
                StreetName = Request.Form["street"],
                HouseNumber = Request.Form["housenumber"],
                FlatNumber = short.Parse(Request.Form["flatnumber"]),
                RegDate = DateTime.UtcNow 
            };
            var v = p.Validate;
            if(!v.isValid) 
            { 
                ViewData["ErrorMessage"] = v.Message;
                return View ("Signup");
            }
            try 
            {
                DbAdvanced.CreateAccount(p, Request.Form["password"]);
                ViewData["ErrorMessage"] = "You have successfully created an account. Now log in.";
                return View("Login");
            }
            catch (ReferentialException ee)
            {
                ViewData["ErrorMessage"] = ee.ReadableMessage;
                return View ("Signup");
            }
        }

        public ActionResult Explore(int district, string variant, string order, int maxprice)
        {
            if(HttpContext.Session.GetInt32("PersonID")!=null) ViewData["LoggedIn"]=true;
            if (variant == null)
            {
                return View ("Explore", DbAdvanced.GetEstateObjects());
            }
            Dictionary<int, EstateObject> result = new Dictionary<int, EstateObject>();
            switch (variant)
            {
                case "h":
                    foreach (var i in DbAdvanced.GetHouses(district, maxprice, order))
                        result[i.Key] = i.Value;
                    return View ("Explore", result);

                case "f":
                    foreach (var i in DbAdvanced.GetFlats(district, maxprice, order))
                        result[i.Key] = i.Value;
                    return View ("Explore", result);

                case "l":
                    foreach (var i in DbAdvanced.GetLandplots(district, maxprice, order))
                        result[i.Key] = i.Value;
                    return View ("Explore", result);

                default:
                    return View ("Explore", DbAdvanced.GetEstateObjects(district, maxprice, order));
            }
        }

        [HttpGet("/Post")]
        public ActionResult Post_Stage1(int district=-1, string variant="")
        {
            if(HttpContext.Session.GetInt32("PersonID")==null)
                return new UnauthorizedResult();

            if (district == -1)
            {
                return View ("Post");
            }
            else
            {
                ViewData["location"] = district;
                ViewData["variant"] = variant;
                switch (variant)
                {
                    case "h":
                        return View ("PostHouse");
                    case "f":
                        return View ("PostFlat");
                    case "l":
                        return View ("PostLandplot");
                    default:
                        return View ("PostObject");
                }
            }
        }

        [HttpPost]
        public ActionResult Post_Stage2()
        {
            if(HttpContext.Session.GetInt32("PersonID")==null)
                return new UnauthorizedResult();
            else 
                ViewData["LoggedIn"]=true;
            
            switch (Request.Form["variant"])
            {
                case "h":
                    House h = new House
                    {
                        PostDate = DateTime.UtcNow,
                        LocationID = int.Parse(Request.Form["location"]),
                        Variant = (byte)'h',
                        StreetName = Request.Form["streetname"],
                        HouseNumber = Request.Form["housenumber"],
                        Description = Request.Form["description"],
                        SellerID = (int)HttpContext.Session.GetInt32("PersonID"),
                        Price = int.Parse(Request.Form["price"]),
                        HomeArea = float.Parse(Request.Form["homearea"]),
                        LandArea = float.Parse(Request.Form["landarea"]),
                        FloorCount = short.Parse(Request.Form["floorcount"]),
                        RoomCount = short.Parse(Request.Form["roomcount"]),
                        isVisible = false,
                        isOpen = true
                    };
                    string t = Request.Form["tags"];
                    h.Tags = t.Split(' ');
                    var v = h.Validate;
                    if (!v.isValid && v.FieldName!="PhotoUrls") // wheelchair
                    {
                        ViewData["ErrorMessage"] = v.Message;
                        return View ("PostHouse", h);
                    }
                    else
                    {
                        h.PhotoUrls = new List<string>();
                        var photos = Request.Form.Files.ToList();
                        if (photos.Count < 1)
                        {
                            ViewData["ErrorMessage"]="There were no photos.";
                            return View("PostHouse");
                        }
                        for (int i=0; i<photos.Count & i<10; i++)
                        {
                            if (photos[i].Length < (2 * 1024 * 1024))
                            { 
                                string ext = photos[i].ContentType.Split('/')[1];
                                string filename = $"{h.PostDate.ToBinary():X}_{i}.{ext}";
                                Console.WriteLine ("File name: "+filename);
                                h.PhotoUrls.Add(filename);
                                using (FileStream s = new FileStream($"wwwroot/img/{filename}", FileMode.OpenOrCreate))
                                {
                                    photos[i].CopyTo (s);
                                }
                            }
                        }
                        try 
                        {
                            DbClient.PutEstateObject(h); 
                            ViewData["ErrorMessage"] = "";
                        } 
                        catch (ReferentialException ee)
                        {
                            ViewData["ErrorMessage"] = ee.ReadableMessage;
                            ViewData["location"] = Request.Form["location"];
                            ViewData["variant"] = "h";
                            return View ("PostHouse");
                        }
                    }
                    return View ("ViewObject", h);
                case "f":
                    return View ("PostFlat");
                case "l":
                    return View ("PostLandplot");
                default:
                    return View ("PostObject");
            }
        }

        [HttpGet]
        public ActionResult ViewObject (int id)
        {
            EstateObject h;
            if(DbClient.ObjectCache.TryGet(id, out h))
            {
                Location l = DbClient.LocationCache.Get(h.LocationID);
                Person p = DbClient.PersonCache.Get(h.SellerID);
                ViewData["ObjectID"] = id;
                ViewData["LocationFull"] = l.Region + " область, " + l.Town + ((l.District.Length > 0) ? (", " + l.District + " район") : "");
                ViewData["Seller"] = $"{p.Name} {p.Surname}, тел. {p.Phone}";
                int? personid = HttpContext.Session.GetInt32("PersonID");
                if (personid != null) { 
                    ViewData["LoggedIn"]=true;
                    if((int)personid==h.SellerID)
                        ViewData["isDeletable"] = true;
                    ViewData["isBookmarked"] = DbClient.BookmarkCache.ContainsKey(((long)personid<<32)+id);
                }
                return View("ViewObject", h);
            }
            return new NotFoundResult();
        }

        [HttpGet]
        public ActionResult Bookmarks()
        {
            int? personid = HttpContext.Session.GetInt32("PersonID");
            if (personid==null)
                return new UnauthorizedResult();

            ViewData["LoggedIn"]=true;
            Dictionary<int,string> d = DbAdvanced.GetBookmarks((int)personid);
            return View ("Bookmarks", d);
        }

        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        public ActionResult Account()
        {
            int? personid = HttpContext.Session.GetInt32("PersonID");
            if(personid==null) return new UnauthorizedResult();
            ViewData["LoggedIn"]=true;
            Person p; 
            if(DbClient.PersonCache.TryGet((int)personid, out p)) 
            {
                Location l = DbClient.LocationCache.Get(p.LocationID);
                ViewData["Location"] = $"{l.Region} область, {l.Town}, {(l.District.Length<2? l.District + " район, ": "")} {p.StreetName}, {p.HouseNumber}{(p.FlatNumber>0? ", кв. " + p.FlatNumber.ToString(): "")}";
                return View("Account", p);
            }
            else return new UnauthorizedResult();
        }

        public ActionResult DeleteObject(int id)
        {
            int? personid = HttpContext.Session.GetInt32("PersonID");
            if(personid==null) 
                return new UnauthorizedResult();
            else
            {
                EstateObject obj = new EstateObject();
                if(DbClient.ObjectCache.TryGet(id, out obj))
                {
                    if (obj.SellerID != personid)
                    {
                        return new UnauthorizedResult();
                    }
                    ViewData["objectid"] = id;
                    return View("DeleteObject");
                }
                return new NotFoundResult();
            }
        }

        public ActionResult DeleteObjectConfirm(int id)
        {
            DbClient.DeleteObject(id);
            return RedirectToAction("Explore");
        }

        // --- IDK what is this -----------------------------------------------
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
