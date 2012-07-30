using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace houser
{
    public class PropertyCompareProperties
    {
        #region Properties
        protected string _accountNumber;
        protected string _cAccountNumber;
        
        bool isNew = true;

        #endregion

        #region Fields

        public string AccountNumber { get { return _accountNumber; } set { _accountNumber = value; } }
        public string CAccountNumber { get { return _cAccountNumber; } set { _cAccountNumber = value; } }
        
        #endregion

        #region Constructors
        public PropertyCompareProperties()
        {
        }

        public PropertyCompareProperties(string _accountNumber)
        {
        }

        #endregion
    }
}