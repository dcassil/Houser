using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace houser
{
    public class Properties
    {
        public static DateTime? AccountNumberAlreadyInTable(string accountNumber)
        {
            return Data.PropertiesDB.AccountNumberAlreadyInTable(accountNumber);
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
                                            string salePrice)
        {
            Data.PropertiesDB.InsertProperty(accountNumber, address, sqft, beds, baths, yearBuilt, garage, exterior, saleDate, salePrice);
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
                                            string salePrice)
        {
            Data.PropertiesDB.UpdateProperty(accountNumber, address, sqft, beds, baths, yearBuilt, garage, exterior, saleDate, salePrice);
        }
    }
}