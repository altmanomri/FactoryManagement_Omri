using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FactoryManagement_Omri.Models
{
    public class DepartmentsBL
    {
        FactoryManagementEntities2 db = new FactoryManagementEntities2();
        #region get departments
        public List<DepartmentMan> GetDepartments()
        {      //for presenting managers name in the table (and not only the id number)
            var result = from Dep in db.Department
                         join Emp in db.Employee on Dep.Manager equals Emp.ID
                         
                         select new DepartmentMan
                         {
                             Manager = Emp.FirstName + Emp.LastName,
                             ID = Dep.ID,
                             Name = Dep.Name,
                             NumOfEmpInDep = (int)Emp.DepartmentID
                             
                         };

            return result.ToList();
        }
        #endregion

        #region add department
        public void AddDepartment(Department dep)
        {
            db.Department.Add(dep);
            db.SaveChanges();
        }
        #endregion

        #region delete department
        public void DeleteDepartment(int id)
        {
            Department dep = db.Department.Where(x => x.ID == id).First();
            
            db.Department.Remove(dep);
            db.SaveChanges();
        }

        public int GetDepartmentEmployeeCount(int depratmentId)
        {        // checking if departmant has employees in it befor moving on to deleting
            var count = (from Dep in db.Department
                         join Emp in db.Employee on Dep.ID equals Emp.DepartmentID
                         where Dep.ID == depratmentId
                         select Emp).Count();

            return count;
        }
        #endregion

        #region edit departmant
        public Department GetDepartmant(int id)
        {
            Department dep = db.Department.Where(x => x.ID == id).First();
            return dep;
        }

        public void UpdateDepartment(Department dep)
        {
            Department d = db.Department.Where(x => x.ID == dep.ID).First();
            d.Name = dep.Name;
            d.Manager = dep.Manager;            

            db.SaveChanges();
        }
        #endregion
    }

}

