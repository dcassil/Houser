using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace houser.Data
{
    public class SheriffSalePropertyDB
    {
        public static void InsertProperty(Dictionary<string, string> data, string saleDate)
        {
            PropertyDataDataContext db = new PropertyDataDataContext();
            SSaleRecord ssp = new SSaleRecord();
            ssp.AccountNumber = data["8"].Substring(67);
            ssp.Price = data["Appraisal Value"];
            ssp.SaleDate = Convert.ToDateTime(Regex.Replace(saleDate, "%2f", "/"));
            db.SSaleRecords.InsertOnSubmit(ssp);
            db.SubmitChanges();
        }

        /// <summary>
        /// I think this is my favorite way to do an update method.  this way we can pass in just the data we want to update and ignore the rest.
        /// </summary>
        public static void UpdateProperty(string accountNumber, string price, string Note, string saleDate, string lastUpdate)
        {
            PropertyDataDataContext db = new PropertyDataDataContext();
            SSaleRecord record = db.SSaleRecords.First(r => r.AccountNumber == accountNumber);
            if (price != "")
                record.Price = price;
            if (Note != "")
                record.Note = Note;
            if (saleDate != "")
                record.SaleDate = Convert.ToDateTime(Regex.Replace(saleDate, "%2f", "/"));
            if (lastUpdate != "")
                record.LastUpdate = Convert.ToDateTime(lastUpdate);
            db.SubmitChanges();
        }
        public static DateTime? AccountNumberAlreadyInTable(string accountNumber)
        {
            PropertyDataDataContext db = new PropertyDataDataContext();
            SSaleRecord record = db.SSaleRecords.FirstOrDefault(r => r.AccountNumber == accountNumber);
            if (record != null)
                return record.SaleDate;
            else
                return null;
        }
        public static string GetMinimumBidByAccountNumberAndDate(string accountNumber, DateTime date)
        {
            PropertyDataDataContext db = new PropertyDataDataContext();
            var result = (from p in db.SSaleRecords
                              where p.AccountNumber == accountNumber && p.SaleDate == date
                              select p.Price).FirstOrDefault();
            return result;
        }
    }
}