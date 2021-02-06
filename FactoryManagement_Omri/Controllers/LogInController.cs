using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FactoryManagement_Omri.Models;

namespace FactoryManagement_Omri.Controllers
{
    public class LogInController : Controller
    {
        // GET: LogIn
        LoginBL loginBL = new LoginBL();

        #region login
        public ActionResult Index()
        {
            return View("LogIn");
        }

        [HttpPost]
        public ActionResult GetLoginData(string username, string password)
        {   //authenticating user by user name and password
            var user = loginBL.IsAuthenticated(username, password);

            if (user != null)
            {
                Session["authenticated"] = true;

                var result = loginBL.GetUsers();
                ViewBag.userFlName = result;
                //saving loged user's data (full name and activity limit) for presenting 
                Session["UserFlName"] = user.FullName;
                Session["UserID"] = user.ID;
                Session["ActivityLimit"] = user.NumOfActions;
                //
                var activityCount = new UserActivityLogBL().GetUserActivityCount();
                if (activityCount > user.NumOfActions)
                {
                    return LogOut(true);
                }
                //saving user's activity count for presenting
                Session["ActivityCount"] = activityCount;
                // saving a summarised sentence for presenting
                Session["UserDisplay"] = $"{user.FullName} (Action made: {activityCount} out of {user.NumOfActions})";

                return RedirectToAction("HomePage", "HomePage");
            }
            else
            {   //if not authenticated - directed to login screen
                Session["authenticated"] = false;
                return RedirectToAction("Index", "LogIn");
            }
        }
        #endregion

        #region logout
        //logout of the system if actions made are over the limit - and "loged out" message 
        public ActionResult LogOut(bool isOverLimit = false)
        {
            if (isOverLimit)
            {
                ViewBag.IsOverLimit = true;
            }
            Session.Clear();
            return View("Logout");
        }
        //for loging out indepandetly by clicking the "logout" link
        public ActionResult VoluntarylogOut()
        {
            Session.Clear();
            return View("Login");
        }
        #endregion

    }
}