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
                    HttpContext.Session.SetString("phone", phone);
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

        public ActionResult Signup()
        {
            return View("Signup");
        }

        public ActionResult Explore()
        {
            return View ("Explore");
        }



        // --- IDK what is this -----------------------------------------------
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
