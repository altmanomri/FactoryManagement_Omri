using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FactoryManagement_Omri.Models
{
    public class UserActivityLogBL
    {
        FactoryManagementEntities2 db = new FactoryManagementEntities2();

        public int AddUserActivityToLog()
        {
            UserActivityLog activity = new UserActivityLog
            {
                UserID = int.Parse(HttpContext.Current.Session["UserID"].ToString()),
                Date = DateTime.Now
            };
            db.UserActivityLog.Add(activity);
            db.SaveChanges();

            return GetUserActivityCount();
        }
        //gets user activity count for the actions limitation
        public int GetUserActivityCount()
        {
            var startDate = DateTime.Now.AddDays(-1);
            var userid = int.Parse(HttpContext.Current.Session["UserID"].ToString());
            return db.UserActivityLog.Where(x => x.UserID == userid && x.Date >= startDate).Count();
        }
    }
}