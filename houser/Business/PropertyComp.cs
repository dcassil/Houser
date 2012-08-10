using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using houser.Data;

namespace houser.Business
{
    public class PropertyComp
    {
        #region Static Methods

        public static bool PropertCompExsits(string accountNumber, string cAccountNumber)
        {
            return PropertyCompDB.PropertyCompExists(accountNumber, cAccountNumber);
        }

        public static void InsertPropertyComp(string accountNumber, string cAccountNumber)
        {
            PropertyCompDB.InsertPropertyComp(accountNumber, cAccountNumber);
        }
        #endregion
    }
}