using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using houser.Data;
using System.Data;

namespace houser
{
    public class PropAccounts
    {
        public static DateTime? AccountNumberAlreadyInTable(string accountNumber)
        {
            return PropAccountsDB.LastUpdateIfExists(accountNumber);
        }

        public static bool AccountSeedExist(string accountNumber)
        {
            return PropAccountsDB.AccountSeedExist(accountNumber);
        }

        public static bool CompletePropAccountExist(string accountNumber)
        {
            return PropAccountsDB.CompletePropAccountExist(accountNumber);
        }

        public static List<PropAccount> GetSubjectPropertiesByDate(DateTime date)
        {
            return PropAccountsDB.GetSubjectPropertiesByDate(date);
        }

        public static void InsertProperty(  string accountNumber,
                                            string address,
                                            string sqft,
                                            string beds,
                                            string baths,
                                            string yearBuilt,
                                            string garage,
                                            string exterior,
                                            string saleDate,
                                            string salePrice,
                                            string fullyLoaded,
                                            string subjectProperty)
        {
            PropAccountsDB.InsertProperty(accountNumber, address, sqft, beds, baths, yearBuilt, garage, exterior, saleDate, salePrice, fullyLoaded, subjectProperty);
        }

        public static void UpdateProperty(string accountNumber,
                                            string address,
                                            string sqft,
                                            string beds,
                                            string baths,
                                            string yearBuilt,
                                            string garage,
                                            string exterior,
                                            string saleDate,
                                            string salePrice,
                                            string fullyLoaded,
                                            string subjectProperty)
        {
            PropAccountsDB.UpdateProperty(accountNumber, address, sqft, beds, baths, yearBuilt, garage, exterior, saleDate, salePrice, fullyLoaded, subjectProperty);
        }
    }
}