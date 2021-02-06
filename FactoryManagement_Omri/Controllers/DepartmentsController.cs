using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FactoryManagement_Omri.Models;


namespace FactoryManagement_Omri.Controllers
{

    public class DepartmentsController : Controller
    {

        DepartmentsBL departmentsBL = new DepartmentsBL();
        EmployeeBL employeeBL = new EmployeeBL();
        // GET: Departments

        #region login verification and department display
        public ActionResult Index(bool success = true)
        {
            //if log in is autenticated proceed to department page
            if (Session["authenticated"] != null && (bool)Session["authenticated"] == true)
            {
                var UserFlName = Session["UserDisplay"];
                ViewBag.userFlName = UserFlName;
                
                if (!success)
                {
                    ViewBag.status = false;
                }
                else
                {
                    ViewBag.status = true;
                }

                var DepartmentData = departmentsBL.GetDepartments();

                var departmentemployeesList = new List<Employees>();
                // run all records from DB
                foreach (var depItem in DepartmentData)
                {
                    //find if emplyee exsist in my list
                    var currDep = departmentemployeesList.FirstOrDefault(x => x.Department == depItem.Name);  

                }
                ViewBag.department = DepartmentData;

                #region activity count
                /*Activity section start*/
                var currentActivityCount = new UserActivityLogBL().AddUserActivityToLog();
                if (currentActivityCount > int.Parse(Session["ActivityLimit"].ToString()))
                {
                    return RedirectToAction("LogOut", "LogIn", new { IsOverLimit = true });
                }
                /*Activity section end*/
                #endregion

                return View("Departments");
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
        }
        #endregion
        
        #region adding new department
        public ActionResult AddDepartment()
        {
            var UserFlName = Session["UserDisplay"];
            ViewBag.userFlName = UserFlName;
            //geting employees info for chusing manager
            var EmployeeData = employeeBL.GetEmployees();
            ViewBag.Employees = EmployeeData;

            var DepartmentData = departmentsBL.GetDepartments();
            ViewBag.Departments = DepartmentData;

            return View("NewDepartment");
        }
        [HttpPost]
        public ActionResult GetNewDepartmentFromUser(Department dep)
        {
            var UserFlName = Session["UserDisplay"];
            ViewBag.userFlName = UserFlName;

            departmentsBL.AddDepartment(dep);
            #region activaty count
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

        #region delete department
        public ActionResult DeleteDepartment(int id)
        {
            //restriction for not deleting dep with emoloyees in it 
            var count = departmentsBL.GetDepartmentEmployeeCount(id);
            if (count > 0)
            {    // if departmant has employee user will be redirected to index action with "Unable to delete..." message 
                return RedirectToAction("Index", new { success = false });
            }    //if not contaning employees will proceed to deleting
            departmentsBL.DeleteDepartment(id);
            #region activaty count
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

        #region edit departmant

        public ActionResult EditDepartment(int id)
        {
            var UserFlName = Session["UserDisplay"];
            ViewBag.userFlName = UserFlName;
            //geting employees info for editing manager
            var EmployeeData = employeeBL.GettAllEmployees();
            ViewBag.Employees = EmployeeData;

            var DepartmentData = departmentsBL.GetDepartments();
            ViewBag.Departments = DepartmentData;

            Department dep = departmentsBL.GetDepartmant(id);
            return View("EditDepartment", dep);
        }

        [HttpPost]
        public ActionResult GetEditedDepartmentFromUser(Department dep)
        {
            var UserFlName = Session["UserDisplay"];
            ViewBag.userFlName = UserFlName;

            departmentsBL.UpdateDepartment(dep);

            #region activaty count
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

    }
}