using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace houser
{
    public class SheriffSaleProperty
    {
        public static void InsertProperty(Dictionary<string, string> data, string saleDate)
        {
            Data.SheriffSalePropertyDB.InsertProperty(data, saleDate);
        }

        /// <summary>
        /// Check to see if this property has already been entered.
        /// </summary>
        /// <param name="accountNumber"></param>
        /// <returns></returns>
        public static DateTime? AccountNumberAlreadyInTable(string accountNumber)
        {
            return Data.SheriffSalePropertyDB.AccountNumberAlreadyInTable(accountNumber);
        }
    }
}