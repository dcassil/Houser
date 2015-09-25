using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace houser.objects
{
    public class SaleRecordItem
    {
        #region Properties
        protected int _saleRecordID;
        protected string _accountNumber;
        protected string _address;
        protected string _caseNumber;
        protected DateTime _saleDate;
        protected double _salePrice;
        protected DateTime _dateModified;
        bool isNew = true;
        #endregion

        #region Fields
        public int SaleRecordID { get { return _saleRecordID; } set { _saleRecordID = value; } }
        public string AccountNumber { get { return _accountNumber; } set { _accountNumber = value; } }
        public string Address { get { return _address; } set { _address = value; } }
        public string CaseNumber { get { return _caseNumber; } set { _caseNumber = value; } }
        public DateTime SaleDate { get { return _saleDate; } set { _saleDate = value; } }
        public double SalePrice { get { return _salePrice; } set { _salePrice = value; } }
        public DateTime DateModified { get { return _dateModified; } set { _dateModified = value; } }
        public bool IsNew { get { return isNew; } private set { isNew = value; } }

        #endregion

        #region Constructors
        public SaleRecordItem()
        {
        }

        #endregion
    }
}