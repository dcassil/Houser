using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using houser.Data;
using System.Data;

namespace houser.Business
{
    public class User
    {
        #region Static methods
        public static bool ThisIsAUser(string userName, string password)
        {
            return UserDB.ThisIsAUser(userName, password);
        }
        #endregion
    }
}