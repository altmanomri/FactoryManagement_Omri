using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FactoryManagement_Omri.Models;

namespace FactoryManagement_Omri.Controllers
{
    public class EmployeesController : Controller
    {
        EmployeeBL employeeBL = new EmployeeBL();
        ShiftBL shiftBL = new ShiftBL();
        DepartmentsBL departmentsBL = new DepartmentsBL();
        // GET: Employees
        #region login verification and employees display
        public ActionResult Index()
        {
            //if log in is autenticated proceed to showing employee page
            if (Session["authenticated"] != null && (bool)Session["authenticated"] == true)
            {
                var EmployeeData = employeeBL.GetEmployees();

                var UserFlName = Session["UserDisplay"];
                ViewBag.userFlName = UserFlName;
                
                List<EmployeeShifts> employeeShiftList = ConvertToDataModel(EmployeeData);

                ViewBag.employee = employeeShiftList;

                #region activity count
                /*Activity section start*/
                var currentActivityCount = new UserActivityLogBL().AddUserActivityToLog();
                if (currentActivityCount > int.Parse(Session["ActivityLimit"].ToString()))
                {
                    return RedirectToAction("LogOut", "LogIn", new { IsOverLimit = true });
                }
                /*Activity section end*/
                #endregion

                return View("Employees");
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }

        private static List<EmployeeShifts> ConvertToDataModel(List<EmployeeDep> EmployeeData)
        {
            var employeeShiftList = new List<EmployeeShifts>();
            //run on all records from DB
            foreach (var empItem in EmployeeData)
            {
                //find if emplyee's shift exsist in my list
                var currEmp = employeeShiftList.FirstOrDefault(x => x.EmployeeId == empItem.ID);

                if (currEmp != null)
                {    //if not - add it to the list
                    currEmp.Shifts.Add(new Shift
                    {
                        ID = empItem.Shiftid.Value,
                        Date = empItem.ShiftDate.Value,
                        EndTime = empItem.EndShift.Value,
                        StartTime = empItem.StartShift.Value
                    });
                }
                else
                {
                    var empShifts = new EmployeeShifts(empItem.ID, empItem.FirstName,
                                    empItem.LastName, empItem.DepName, empItem.StartWorkYear);

                    if (empItem.Shiftid.HasValue)
                    {
                        empShifts.Shifts.Add(new Shift
                        {
                            ID = empItem.Shiftid.Value,
                            Date = empItem.ShiftDate.Value,
                            EndTime = empItem.EndShift.Value,
                            StartTime = empItem.StartShift.Value
                        });
                    }
                    employeeShiftList.Add(empShifts);
                }

            }

            return employeeShiftList;
        }
        #endregion

        #region employee serch
        [HttpPost]
        public ActionResult EmployeeSearchResult(string phrase)
        {    //getting a phrase and checking if it's or a part of it is equal to name/last name/department name of employees 
            var result = employeeBL.Search(phrase);
            List<EmployeeShifts> employeeShiftList = ConvertToDataModel(result);

            ViewBag.employee = employeeShiftList;

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
            return View("Employees");
        }
        #endregion

        #region delete employee
        public ActionResult DeleteEmployee(int id)
        {
            employeeBL.DeleteEmployee(id);
            #region activity count
            /*Activity section start*/
            var currentActivityCount = new UserActivityLogBL().AddUserActivityToLog();
            if (currentActivityCount > int.Parse(Session["ActivityLimit"].ToString()))
            {
                return RedirectToAction("LogOut", "LogIn", new { IsOverLimit = true });
            }
            /*Activity section end*/
            #endregion
            return RedirectToAction("Index");
        }
        #endregion

        #region add employee
        public ActionResult AddEmployee()
        {   //geting departmants info for the drop down selection of departmant
            var DepartmentData = departmentsBL.GetDepartments();
            ViewBag.Departments = DepartmentData;

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
            return View("NewEmployee");
        }

        [HttpPost]
        public ActionResult GetEmployeeFromUser(Employee emp)
        {
            employeeBL.AddEmployee(emp);

            var UserFlName = Session["UserDisplay"];
            ViewBag.userFlName = UserFlName;

            return RedirectToAction("Index");
        }
        #endregion

        #region edit employee
        public ActionResult EditEmployee(int id)
        {   //geting departmants info for the drop down selection of departmant
            var DepartmentData = departmentsBL.GetDepartments();
            ViewBag.Departments = DepartmentData;

            Employee emp = employeeBL.GetEmployee(id);

            var UserFlName = Session["UserDisplay"];
            ViewBag.userFlName = UserFlName;

            return View("EditEmployee", emp);
        }
        [HttpPost]
        public ActionResult GetEditedEmployeeFromUser(Employee emp)
        {
            employeeBL.EditEmployee(emp);
            #region activity count
            /*Activity section start*/
            var currentActivityCount = new UserActivityLogBL().AddUserActivityToLog();
            if (currentActivityCount > int.Parse(Session["ActivityLimit"].ToString()))
            {
                return RedirectToAction("LogOut", "LogIn", new { IsOverLimit = true });
            }
            /*Activity section end*/
            #endregion
            var UserFlName = Session["UserDisplay"];
            ViewBag.userFlName = UserFlName;

            return RedirectToAction("Index");
        }
        #endregion

        #region add shift to specific employee
        public ActionResult AddShiftToEmployee(int ID)
        {
            var UserFlName = Session["UserDisplay"];
            ViewBag.userFlName = UserFlName;
            //geting all shift for showing options in choosing shift for employee
            var shifts = shiftBL.GetAllShifts();
            ViewBag.shifts = shifts;
            return View("AddShiftToEmployee", new EmployeeShift { ID = ID });
        }

        [HttpPost]
        public ActionResult SetEmployeeShift(EmployeeShift employeeShift)
        {
            #region activity count
            /*Activity section start*/
            var currentActivityCount = new UserActivityLogBL().AddUserActivityToLog();
            if (currentActivityCount > int.Parse(Session["ActivityLimit"].ToString()))
            {
                return RedirectToAction("LogOut", "LogIn", new { IsOverLimit = true });
            }
            /*Activity section end*/
            #endregion

            employeeBL.AddShiftToEmployee(employeeShift);

            return RedirectToAction("Index");
        }
        #endregion
    }
}