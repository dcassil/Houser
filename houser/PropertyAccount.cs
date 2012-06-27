using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using houser.Data;

namespace houser
{
    public class PropertyAccount
    {
        #region Properties
        protected string _accountNumber;
        protected string _address;
        protected int _sqft;
        protected int _beds;
        protected int _baths;
        protected int _yearBuilt;
        protected string _exterior;
        protected DateTime _lastSaleDate;
        protected int _lastSalePrice;
        protected int _garageSize;
        protected bool _fullyLoaded;
        protected bool _subjectProperty;

        #endregion

        #region Fields

        public string AccountNumber { get { return this._accountNumber; } set { this._accountNumber = value; } }
        public string Address { get; set; }
        public int SQFT { get; set; }
        public int Beds { get; set; }
        public int Baths { get; set; }
        public int YearBuilt { get; set; }
        public string Exterior { get; set; }
        public DateTime LastSaleDate { get; set; }
        public int LastSalePrice { get; set; }
        public int GarageSize { get; set; }
        public bool FullyLoaded { get; set; }
        public bool SubjectProperty { get; set; }

        #endregion

        #region Constructors
        public PropertyAccount()
        {
        }

        public PropertyAccount(string accountID)
        {
        }

        #endregion
        
        #region private methods
               
        #endregion

        #region Persistance

        public void Load()
        {
            PropAccountsDB.GetProperty(_accountNumber);
        }

        public void Save()
        {
            // need to check to see if we should update or insert here.
            if (true)
                PropAccountsDB.InsertProperty(_accountNumber, _address, _sqft, _beds, _baths, _yearBuilt, _garageSize, _exterior, _lastSaleDate, _lastSalePrice, _fullyLoaded, _subjectProperty);   
            else
                PropAccountsDB.UpdateProperty(_accountNumber, _address, _sqft, _beds, _baths, _yearBuilt, _garageSize, _exterior, _lastSaleDate, _lastSalePrice, _fullyLoaded, _subjectProperty);
        }
        #endregion

    }
}
   
