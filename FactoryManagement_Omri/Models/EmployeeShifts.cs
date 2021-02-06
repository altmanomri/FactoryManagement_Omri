
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FactoryManagement_Omri.Models
{
    public class EmployeeShifts
    {
        public EmployeeShifts()
        {
            Shifts = new List<Shift>();
        }
        public EmployeeShifts(int employeeId, string firstName, string lastName, string department, int? startWorkYear)
        {
            EmployeeId = employeeId;
            FullName = $"{firstName} {lastName}";
            Department = department;
            StartWorkYear = startWorkYear.HasValue ? startWorkYear.ToString() : "";
            Shifts = new List<Shift>();
        }
        public int EmployeeId { get; set; }
        public string FullName { get; set; }
        public string Department { get; set; }
        public string StartWorkYear { get; set; }
        public List<Shift> Shifts { get; set; }
    }
}