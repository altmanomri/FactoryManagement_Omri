using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace FactoryManagement_Omri.Models
{
    public class EmployeeBL
    {
        FactoryManagementEntities2 db = new FactoryManagementEntities2();
        #region get employees
        public List<Employee> GettAllEmployees()
        {
            return db.Employee.ToList();
        }

        public List<EmployeeDep> GetEmployees()
        {
            //joining employees, departments, shits, and employee shifts tables with left outer join (the left puter join is to show if data is null)
            //for presenting them in the view (stagex is the joind table and combx is the tables name )
            var result = from Emp in db.Employee
                         join Dep in db.Department on Emp.DepartmentID equals Dep.ID into stage1
                         from comb1 in stage1.DefaultIfEmpty()
                         join Em_shf in db.EmployeeShift on Emp.ID equals Em_shf.EmployeeID into stage2
                         from comb2 in stage2.DefaultIfEmpty()
                         join Shi in db.Shift on comb2.ShiftID equals Shi.ID into stage3
                         from comb3 in stage3.DefaultIfEmpty()

                         select new EmployeeDep
                         {
                             ID = Emp.ID,
                             FirstName = Emp.FirstName,
                             LastName = Emp.LastName,
                             Name = Emp.FirstName + Emp.LastName,
                             StartWorkYear = Emp.StartWorkTear,
                             DepartmentID = comb1.ID,
                             DepName = comb1.Name,
                             EndShift = comb3.EndTime,
                             StartShift = comb3.StartTime,
                             ShiftDate = (DateTime)comb3.Date,
                             Shiftid = comb3.ID,
                             EmpShiftid = comb2.ID
                         };

            return result.ToList();
        }
        #endregion

        #region search employees
        public List<EmployeeDep> Search(string phrase)
        {
            var employees = GetEmployees();
            return employees.Where(x => x.FirstName.ToLower().Contains(phrase.ToLower()) || x.LastName.ToLower().Contains(phrase.ToLower()) 
            || (x.DepName !=null && x.DepName.ToLower().Contains(phrase.ToLower()))).ToList() ;
        }
        #endregion

        #region delete employee
        public void DeleteEmployee(int id)
        {
            Employee Emp = db.Employee.Where(x => x.ID == id).First();
            //removing employee's shifts befor delete 
            var em_sh = db.EmployeeShift.Where(x => x.EmployeeID == id);
            foreach (var item in em_sh)
            {
                db.EmployeeShift.Remove(item);
            }

            db.Employee.Remove(Emp);
            db.SaveChanges();
        }
        #endregion

        #region add employee
        public void AddEmployee(Employee emp)
        {
            db.Employee.Add(emp);
            db.SaveChanges();
        }
        #endregion

        #region edit employee
        public Employee GetEmployee(int id)
        {
            Employee emp = db.Employee.Where(x => x.ID == id).First();
            return emp;
        }
        public void EditEmployee(Employee emp)
        {
            Employee e = db.Employee.Where(x => x.ID == emp.ID).First();
            e.FirstName = emp.FirstName;
            e.LastName = emp.LastName;
            e.DepartmentID = emp.DepartmentID;

            db.SaveChanges();
        }
        #endregion

        #region add shift to employee
        public void AddShiftToEmployee(EmployeeShift employeeShift)
        {
            db.EmployeeShift.Add(employeeShift);
            db.SaveChanges();
        }
        #endregion

    }
}