using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FactoryManagement_Omri.Models
{
    public class LoginBL
    {
        FactoryManagementEntities2 db = new FactoryManagementEntities2();

        public User IsAuthenticated(string username, string password)
        {
            var result = db.User.Where(x => x.UserName == username && x.Password == password);
            return result.FirstOrDefault();           
        }  

        public List<User> GetUsers()
        {
            return db.User.ToList();
        }
    }
}
