using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FactoryManagement_Omri.Models
{
    public class ShiftEmployees
    {
        public ShiftEmployees()
        {
            Employees = new List<EmployeeInfo>();
        }
        public int ShiftId { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
        public List<EmployeeInfo> Employees { get; set; }
        
    }
     public class EmployeeInfo
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; }
    }
}