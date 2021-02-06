using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FactoryManagement_Omri.Models
{
    public class Employees
    {
        public Employees()
        {
            List<Employee> Employees = new List<Employee>();
        }
        public Employees(int employeeId, string firstName, string lastName, string department, int? startWorkYear)
        {
            EmployeeId = employeeId;
            FullName = $"{firstName} {lastName}";
            Department = department;
            StartWorkYear = startWorkYear.HasValue ? startWorkYear.ToString() : "-";
            List<Employee> Employees = new List<Employee>();
        }
        public int EmployeeId { get; set; }
        public string FullName { get; set; }
        public string Department { get; set; }
        public string StartWorkYear { get; set; }
        public List<Employee> Shifts { get; set; }
    }
}