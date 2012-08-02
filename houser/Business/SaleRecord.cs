using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using houser.Data;
using System.Data;

namespace houser
{
    public class SaleRecord
    {
        #region Properties
        protected int _saleRecordID;
        protected string _accountNumber;
        protected DateTime _saleDate;
        protected double _salePrice;
        bool isNew = true;

        #endregion

        #region Fields

        public int SaleRecordID { get { return _saleRecordID; } set { _saleRecordID = value; } }
        public string AccountNumber { get { return _accountNumber; } set { _accountNumber = value; } }
        public DateTime SaleDate { get { return _saleDate; } set { _saleDate = value; } }
        public double SalePrice { get { return _salePrice; } set { _salePrice = value; } }
        public bool IsNew { get { return isNew; } private set { isNew = value; } }

        #endregion

        #region Constructors
        public SaleRecord()
        {
        }

        public SaleRecord(string accountNumber, DateTime saleDate)
        {
            DataRow saleRecord = SaleRecordDB.GetMostRecentSaleRecordByAccountNumberAndSaleDate(accountNumber, saleDate);
            if (saleRecord == null)
                isNew = true;
            else
            {
                isNew = false;
                _accountNumber = accountNumber;
                _saleDate = saleDate;
                _salePrice = Convert.ToDouble(saleRecord["SalePrice"]);
                _saleRecordID = Convert.ToInt32(saleRecord["SaleRecordID"]);
            }
        }
        #endregion

        #region Persistance

        public void Save()
        {
            if (IsNew)
                SaleRecordDB.InsertSaleRecord(_accountNumber, _saleDate, _salePrice);
            else
                SaleRecordDB.UpdateSaleRecord(_accountNumber, _saleDate, _salePrice, _saleRecordID, DateTime.Now);
        }

        
        #endregion
    }
}