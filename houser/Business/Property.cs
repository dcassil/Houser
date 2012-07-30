using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using houser.Data;

namespace houser.Business
{
    public class Property
    {
        #region Fields
        protected string _accountNumber;
        protected string _Address;
        protected int _sqft;
        protected double _baths;
        protected int _beds;
        protected string _exterior;
        protected DateTime _lastSaleDate;
        protected int _lastSalePrice;
        protected DateTime _dateModified;
        protected int _garageSize;
        #endregion

        #region Propertiese
        
        #endregion

        #region Static Methods

        public static DataSet GetPropertyByAccount(string accountNumber)
        {
            return PropertyDB.GetPropertyByAccount(accountNumber);
        }
        #endregion
    }
}