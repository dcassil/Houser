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

        public static void RemoveFromFromList(string accountNumber, int list)
        {
            PropertyListDB.RemoveFromFromList(accountNumber, list);
        }

        public static void UpdatePropertyList(string accountNumber, int list)
        {
            PropertyListDB.UpdatePropertyList(accountNumber, list);
        }

        public static void AddPropertyToList(string accountNumber, int list)
        {
            PropertyListDB.AddPropertyToList(accountNumber, list);
        }

        public static bool PropertyListExists(string accountNumber, int list)
        {
            return PropertyListDB.PropertyListExist(accountNumber, list);
        }

        public static bool PropertyListExistByAccountAndList(string accountNumber, int list)
        {
            return PropertyListDB.PropertyListExistByAccountAndList(accountNumber, list);
        }
        #endregion
    }
}