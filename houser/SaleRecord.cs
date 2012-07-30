using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace houser
{
    public class SaleRecord
    {
        #region Properties
        protected string _accountNumber;
        protected DateTime _saleDate;
        protected double _salePrice;
        bool isNew = true;

        #endregion

        #region Fields

        public string AccountNumber { get { return _accountNumber; } set { _accountNumber = value; } }
        public DateTime SaleDate { get { return _saleDate; } set { _saleDate = value; } }
        public double SalePrice { get { return _salePrice; } set { _salePrice = value; } }

        #endregion

        #region Constructors
        public SaleRecord()
        {
        }

        public SaleRecord(string _accountNumber)
        {
        }
        #endregion

        #region Persistance

        public void Save()
        {

        }

        #endregion
    }
}