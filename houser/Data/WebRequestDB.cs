using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace houser.Data
{
    public class WebRequestDB
    {
        public static void WritePropertyRecord(string accountNumber, string data)
        {
            WebRequestDBMLDataContext db = new WebRequestDBMLDataContext();
            PropertyRecord record = new PropertyRecord();
            record.AccountNumber = accountNumber;
            record.Data = data;
            record.DateCreated = DateTime.Now;
            db.PropertyRecords.InsertOnSubmit(record);
            db.SubmitChanges();
        }

        public static void UpdatePropertyRecord(string accountNumber, string data)
        {
            WebRequestDBMLDataContext db = new WebRequestDBMLDataContext();
            var updateRecord = (from c in db.PropertyRecords where c.AccountNumber == accountNumber select c).First();
            updateRecord.Data = data;
            updateRecord.DateCreated = DateTime.Now;

        }

        public static string GetPropertyRecord(string accountNumber)
        {
            WebRequestDBMLDataContext db = new WebRequestDBMLDataContext();
            PropertyRecord record = db.PropertyRecords.FirstOrDefault(r => r.AccountNumber == accountNumber);
            if (record != null)
            {
                return record.Data.ToString();
            }
            else
                return "No_DATA_29454";
            

        }

    }
} 