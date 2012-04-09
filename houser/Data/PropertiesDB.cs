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
        public static DateTime? LastUpdateIfExists(string accountNumber)
        {
            PropertyData db = new PropertyData();
            PropAccount property = db.PropAccounts.FirstOrDefault(r => r.AccountNumber == accountNumber);
            if (property != null)
                return property.DateModified;
            else
                return null;
        }

        public static bool AccountSeedExist(string accountNumber)
        {
            PropertyData db = new PropertyData();
            var propAccount = from p in db.PropAccounts
                              where p.AccountNumber == accountNumber
                              select p;
            if (propAccount.Any())
                return true;
            else
                return false;
        }

        public static bool CompletePropAccountExist(string accountNumber)
        {
            PropertyData db = new PropertyData();
            var propAccount = from p in db.PropAccounts
                           where p.AccountNumber == accountNumber && p.FullyLoaded == "1"
                           select p;
            if (propAccount.Any())
                return true;
            else
                return false;
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
            PropertyData db = new PropertyData();
            PropAccount property = new PropAccount();
            property.AccountNumber = accountNumber;
            if (address != "")
                property.Address = address;
            if (sqft != "")
                property.Sqft = sqft;
            if (beds != "")    
                property.Beds = beds;
            if (baths != "")
                property.Baths = baths;
            if (yearBuilt != "")
                property.YearBuilt = yearBuilt;
            if (garage != "")
                property.GarageSize = garage;
            if (exterior != "")
                property.Exterior = exterior;
            if (saleDate != "")
                property.LastSaleDate = saleDate;
            if (salePrice != "")
                property.LastSalePrice = salePrice;
            if (fullyLoaded != "")
                property.FullyLoaded = fullyLoaded;
            if (subjectProperty != "")
                property.SubjectProperty = subjectProperty;
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
                                            string salePrice,
                                            string fullyLoaded,
                                            string subjectProperty)
        {
            PropertyData db = new PropertyData();
            PropAccount property = db.PropAccounts.First(p => p.AccountNumber == accountNumber);
            if (address != "")
                property.Address = address;
            if (sqft != "")
                property.Sqft = sqft;
            if (beds != "")
                property.Beds = beds;
            if (baths != "")
                property.Baths = baths;
            if (yearBuilt != "")
                property.YearBuilt = yearBuilt;
            if (garage != "")
                property.GarageSize = garage;
            if (exterior != "")
                property.Exterior = exterior;
            if (saleDate != "")
                property.LastSaleDate = saleDate;
            if (salePrice != "")
                property.LastSalePrice = salePrice;
            if (fullyLoaded != "")
                property.FullyLoaded = fullyLoaded;
            if (subjectProperty != "")
                property.SubjectProperty = subjectProperty;
            property.DateModified = DateTime.Now;
            db.SubmitChanges();
        }
    }
}