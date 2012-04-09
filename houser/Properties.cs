using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using houser.Data;

namespace houser
{
    public class Properties
    {
        public static DateTime? AccountNumberAlreadyInTable(string accountNumber)
        {
            return PropertiesDB.LastUpdateIfExists(accountNumber);
        }

        public static bool AccountSeedExist(string accountNumber)
        {
            return PropertiesDB.AccountSeedExist(accountNumber);
        }

        public static bool CompletePropAccountExist(string accountNumber)
        {
            return PropertiesDB.CompletePropAccountExist(accountNumber);
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
            PropertiesDB.InsertProperty(accountNumber, address, sqft, beds, baths, yearBuilt, garage, exterior, saleDate, salePrice, fullyLoaded, subjectProperty);
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
            PropertiesDB.UpdateProperty(accountNumber, address, sqft, beds, baths, yearBuilt, garage, exterior, saleDate, salePrice, fullyLoaded, subjectProperty);
        }
    }
}