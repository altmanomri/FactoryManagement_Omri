using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FactoryManagement_Omri.Models
{
    public class ShiftBL
    {
        FactoryManagementEntities2 db = new FactoryManagementEntities2();

        public List<Shift_Employee> GetShifts()
        {   //joining shifts, employee shits and employees tables with left outer join (the left outer join is for presenting if there is no value)
            // stagex is the joind table and combx is the tables name 
            var result = from Sft in db.Shift
                         join Emp_Sft in db.EmployeeShift on Sft.ID equals Emp_Sft.ShiftID into stage1
                         from comb1 in stage1.DefaultIfEmpty()
                         join Emp in db.Employee.DefaultIfEmpty() on comb1.EmployeeID equals Emp.ID into stage2
                         from comb2 in stage2.DefaultIfEmpty()
                         select new Shift_Employee
                         {
                             Date = (DateTime)Sft.Date,
                             StartTime = Sft.StartTime,
                             EndTime = Sft.EndTime,
                             ShiftId = Sft.ID,
                             EmployeeId = comb2.ID,
                             FirstName = comb2.FirstName,
                             LastName = comb2.LastName                            
                         };
          
            return result.ToList();
        }

        public List<Shift> GetAllShifts()
        {
           return db.Shift.ToList();
        }
        public void AddShift(Shift shf)
        {
            db.Shift.Add(shf);
            db.SaveChanges();
        }

    }



}