using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using houser.Data;
using System.Data;

namespace houser.Business
{
    public class PropertyComp
    {
        #region Static Methods

        /// <summary>
        /// return true if a property comp record exist.
        /// </summary>
        public static bool PropertCompExsits(string accountNumber, string cAccountNumber)
        {
            return PropertyCompDB.PropertyCompExists(accountNumber, cAccountNumber);
        }

        /// <summary>
        /// Insert new property comp.
        /// </summary>
        public static void InsertPropertyComp(string accountNumber, string cAccountNumber)
        {
            PropertyCompDB.InsertPropertyComp(accountNumber, cAccountNumber);
        }

        /// <summary>
        /// Return all comps for property.
        /// </summary>
        public static DataTable GetCompsForPropertyByAccountNumber(string accountNumber)
        {
            return PropertyCompDB.GetCompsForPropertyByAccountNumber(accountNumber);
        }
        #endregion
    }
}