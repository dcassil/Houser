using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using houser.Data;

namespace houser.Business
{
    public class PropertyList
    {
        #region Static MEthods

        public static void RemoveFromFromList(string accountNumber, int list, int userID)
        {
            PropertyListDB.RemoveFromFromList(accountNumber, list, userID);
        }

        public static void AddPropertyToList(string accountNumber, int list, int userID)
        {
            PropertyListDB.AddPropertyToList(accountNumber, list, userID);
        }

        public static bool PropertyListExists(string accountNumber, int list)
        {
            return PropertyListDB.PropertyListExist(accountNumber, list);
        }

        #endregion
    }
}