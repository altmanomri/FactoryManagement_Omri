using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FactoryManagement_Omri.Models;

namespace FactoryManagement_Omri.Controllers
{
    public class ShiftsController : Controller
    {
        ShiftBL shiftBL = new ShiftBL();

        // GET: Shifts
        #region varification and shift display
        public ActionResult Index()
        {    //if log in is autenticated proceed to showing shifts page
            if (Session["authenticated"] != null && (bool)Session["authenticated"] == true)
            {
                var ShiftsData = shiftBL.GetShifts();

                var shiftEmployees = new List<ShiftEmployees>();

                foreach (var shiftItem in ShiftsData)
                {
                    //find if shift already exsists in the list
                    var shiftEmployee = shiftEmployees.FirstOrDefault(x => x.ShiftId == shiftItem.ShiftId);
                    //if exsist add employee record to shift
                    if (shiftEmployee != null)
                    {
                        if (shiftItem.EmployeeId.HasValue)
                        {
                            shiftEmployee.Employees.Add(new EmployeeInfo
                            {
                                EmployeeId = shiftItem.EmployeeId.Value,
                                FullName = $"{shiftItem.FirstName} {shiftItem.LastName}"
                            });
                        }
                    }
                    //if not exsist create new shift with its employee
                    else
                    {
                        //firest create the base class
                        shiftEmployee = new ShiftEmployees
                        {
                            ShiftId = shiftItem.ShiftId,
                            Date = shiftItem.Date,
                            Time = $"{shiftItem.StartTime} to {shiftItem.EndTime}"
                        };
                        //Then add the first employee info
                        if (shiftItem.EmployeeId.HasValue)
                        {
                            shiftEmployee.Employees.Add(new EmployeeInfo
                            {
                                EmployeeId = shiftItem.EmployeeId.Value,
                                FullName = $"{shiftItem.FirstName} {shiftItem.LastName}"
                            });
                        }
                        shiftEmployees.Add(shiftEmployee);
                    }

                }

                ViewBag.Shifts = shiftEmployees;

                var UserFlName = Session["UserDisplay"];
                ViewBag.userFlName = UserFlName;

                #region activity count
                /*Activity section start*/
                var currentActivityCount = new UserActivityLogBL().AddUserActivityToLog();
                if (currentActivityCount > int.Parse(Session["ActivityLimit"].ToString()))
                {
                    return RedirectToAction("LogOut", "LogIn", new { IsOverLimit = true });
                }
                /*Activity section end*/
                #endregion

                return View("shifts");
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }
        #endregion

        #region add shift
        public ActionResult AddShift()
        {
            var UserFlName = Session["UserDisplay"];
            ViewBag.userFlName = UserFlName;
            #region activaty count
            /*Activity section start*/
            var currentActivityCount = new UserActivityLogBL().AddUserActivityToLog();
            if (currentActivityCount > int.Parse(Session["ActivityLimit"].ToString()))
            {
                return RedirectToAction("LogOut", "LogIn", new { IsOverLimit = true });
            }
            /*Activity section end*/
            #endregion
            return View("AddShift");
        }

        [HttpPost]
        public ActionResult GetNewShiftFromUser(Shift shf)
        {
            shiftBL.AddShift(shf);
            return RedirectToAction("Index");
        }
        #endregion
    }
}