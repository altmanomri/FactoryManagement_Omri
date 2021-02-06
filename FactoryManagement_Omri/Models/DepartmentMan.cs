using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FactoryManagement_Omri.Models
{
    public class DepartmentMan
    {
        public int? ID { get; set; }
        public string Name { get; set; }
        public string Manager { get; set; }
        public int NumOfEmpInDep { get; set; } // for DeleteDpartment option - if > 0 not able to delete
        


    }
}