using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FactoryManagement_Omri.Models;
namespace FactoryManagement_Omri.Controllers
{
    public class HomePageController : Controller
    {
        HomePageBL homePageBL = new HomePageBL();
        // GET: HomePage
        public ActionResult HomePage()
        {   //if log in is autenticated proceed to home page
            if (Session["authenticated"] != null && (bool)Session["authenticated"] == true)
            {
                var UserFlName = Session["UserDisplay"];               
                ViewBag.userFlName = UserFlName;
                
                return View("HomePage");
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }




        }
    }
}