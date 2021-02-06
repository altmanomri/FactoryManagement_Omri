using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FactoryManagement_Omri.Models
{
    public class EmployeeDep
    {
        public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? StartWorkYear { get; set; }
        public int? DepartmentID { get; set; }
        public string DepName { get; set; }
        public int? StartShift { get; set; }
        public int? EndShift { get; set; }
        public DateTime? ShiftDate { get; set; }
        public string Name { get; set; }
        public int? Shiftid { get; set; }
        public int? EmpShiftid { get; set; }

    }
}