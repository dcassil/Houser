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
            PropertyData db = new PropertyData();
            SSaleRecord ssp = new SSaleRecord();
            ssp.AccountNumber = data["8"].Substring(67);
            ssp.Price = data["Appraisal Value"];
            ssp.SaleDate = Convert.ToDateTime(Regex.Replace(saleDate, "%2f", "/"));
            db.SSaleRecords.InsertOnSubmit(ssp);
            db.SubmitChanges();
        }

        public static void UpdateProperty(Dictionary<string, string> data, string saleDate)
        {

        }
        public static DateTime? AccountNumberAlreadyInTable(string accountNumber)
        {
            PropertyData db = new PropertyData();
            SSaleRecord record = db.SSaleRecords.FirstOrDefault(r => r.AccountNumber == accountNumber);
            if (record != null)
                return record.SaleDate;
            else
                return null;
        }
    }
}