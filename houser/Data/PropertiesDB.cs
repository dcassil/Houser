using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;

namespace houser.Data
{
    public class PropertiesDB
    {
        public static DateTime? AccountNumberAlreadyInTable(string accountNumber)
        {
            PropertyData db = new PropertyData();
            PropAccount property = db.PropAccounts.FirstOrDefault(r => r.AccountNumber == accountNumber);
            if (property != null)
                return property.DateModified;
            else
                return null;
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
            PropertyData db = new PropertyData();
            PropAccount property = new PropAccount();
            property.AccountNumber = accountNumber;
            property.Address = address;
            property.Sqft = sqft;
            property.Beds = beds;
            property.Baths = baths;
            property.YearBuilt = yearBuilt;
            property.GarageSize = garage;
            property.Exterior = exterior;
            property.LastSaleDate = saleDate;
            property.LastSalePrice = salePrice;
            property.DateModified = DateTime.Now;
            db.PropAccounts.InsertOnSubmit(property);
            db.SubmitChanges();
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
            PropertyData db = new PropertyData();
            PropAccount property = db.PropAccounts.First(p => p.AccountNumber == accountNumber);
            property.Address = address;
            property.Sqft = sqft;
            property.Beds = beds;
            property.Baths = baths;
            property.YearBuilt = yearBuilt;
            property.GarageSize = garage;
            property.Exterior = exterior;
            property.LastSaleDate = saleDate;
            property.LastSalePrice = salePrice;
            property.DateModified = DateTime.Now;
            db.SubmitChanges();
        }
    }
}