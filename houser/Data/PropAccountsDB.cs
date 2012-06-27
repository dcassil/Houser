using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using houser.Data;
using System.Web.UI.MobileControls;

namespace houser.Data
{
    public class PropAccountsDB
    {
        public static DateTime? LastUpdateIfExists(string accountNumber)
        {
            PropertyDataDataContext db = new PropertyDataDataContext();
            PropAccount property = db.PropAccounts.FirstOrDefault(r => r.AccountNumber == accountNumber);
            if (property != null)
                return property.DateModified;
            else
                return null;
        }

        public static bool AccountSeedExist(string accountNumber)
        {
            PropertyDataDataContext db = new PropertyDataDataContext();
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
            PropertyDataDataContext db = new PropertyDataDataContext();
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
            PropertyDataDataContext db = new PropertyDataDataContext();
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

        public static void InsertProperty(string accountNumber,
                                            string address,
                                            int sqft,
                                            int beds,
                                            int baths,
                                            int yearBuilt,
                                            int garage,
                                            string exterior,
                                            DateTime saleDate,
                                            int salePrice,
                                            bool fullyLoaded,
                                            bool subjectProperty)
        {
            PropertyDataDataContext db = new PropertyDataDataContext();
            PropertyAccount property = new PropertyAccount();
            property.AccountNumber = accountNumber;
            if (address != "")
                property.Address = address;
            if (sqft != 0)
                property.Sqft = sqft;
            if (beds != 0)
                property.Beds = beds;
            if (baths != 0)
                property.Baths = baths;
            if (yearBuilt != 0)
                property.YearBuilt = yearBuilt;
            if (garage != 0)
                property.GarageSize = garage;
            if (exterior != "")
                property.Exterior = exterior;
            if (saleDate != null)
                property.LastSaleDate = saleDate;
            if (salePrice != 0)
                property.LastSalePrice = salePrice;
            if (fullyLoaded != null)
                property.FullyLoaded = fullyLoaded;
            if (subjectProperty != null)
                property.SubjectProperty = subjectProperty;
            property.DateModified = DateTime.Now;
            db.PropertyAccounts.InsertOnSubmit(property);
            db.SubmitChanges();
        }

        public static List<PropAccount> GetSubjectPropertiesByDate(DateTime date)
        {
            PropertyDataDataContext db = new PropertyDataDataContext();
            List<PropAccount> props = (from p in db.PropAccounts
                         join s in db.SSaleRecords on p.AccountNumber equals s.AccountNumber
                         where s.SaleDate == date
                         where p.SubjectProperty == "1"
                         select p).ToList();
            
            return props;
        }

        public static PropertyAccount GetProperty(string accountNumber)
        {
            PropertyDataDataContext db = new PropertyDataDataContext();
            return db.PropertyAccounts.First(r => r.AccountNumber == accountNumber);
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
            PropertyDataDataContext db = new PropertyDataDataContext();
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

        public static void UpdateProperty(string accountNumber,
                                            string address,
                                            int sqft,
                                            int beds,
                                            int baths,
                                            int yearBuilt,
                                            int garage,
                                            string exterior,
                                            DateTime saleDate,
                                            int salePrice,
                                            bool fullyLoaded,
                                            bool subjectProperty)
        {
            PropertyDataDataContext db = new PropertyDataDataContext();
            PropertyAccount property = db.PropertyAccounts.First(p => p.AccountNumber == accountNumber);
            if (address != "")
                property.Address = address;
            if (sqft != null)
                property.Sqft = sqft;
            if (beds != null)
                property.Beds = beds;
            if (baths != null)
                property.Baths = baths;
            if (yearBuilt != null)
                property.YearBuilt = yearBuilt;
            if (garage != null)
                property.GarageSize = garage;
            if (exterior != "")
                property.Exterior = exterior;
            if (saleDate != null)
                property.LastSaleDate = saleDate;
            if (salePrice != null)
                property.LastSalePrice = salePrice;
            if (fullyLoaded != null)
                property.FullyLoaded = fullyLoaded;
            if (subjectProperty != null)
                property.SubjectProperty = subjectProperty;
            property.DateModified = DateTime.Now;
            db.SubmitChanges();
        }
    }
}