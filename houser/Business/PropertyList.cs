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

        public static void UpdatePropertyList(string accountNumber, int list)
        {
            PropertyListDB.UpdatePropertyList(accountNumber, list);
        }

        public static void InsertPropertyList(string accountNumber, int list)
        {
            PropertyListDB.InsertPropertyList(accountNumber, list);
        }

        public static bool PropertyListExists(string accountNumber, int list)
        {
            return PropertyListDB.PropertyListExist(accountNumber, list);
        }
        #endregion
    }
}