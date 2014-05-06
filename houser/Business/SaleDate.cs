using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using houser.Data;
using System.Data;

namespace houser.Business
{
    public class SaleDate
    {
        public DateTime Date;
        public DateTime LastIndexed;

        private bool _isNew = true;

        public SaleDate(DateTime date)
        {
            Date = date;
            LastIndexed = new DateTime();

            DataRow saleDate = SaleDateDB.GetSaleDate(Convert.ToString(date));

            if (saleDate != null)
            {
                LastIndexed = Convert.ToDateTime(saleDate["LastIndexed"]);
                _isNew = false;
            }
        }

        public void save()
        {
            if (_isNew)
                SaleDateDB.InsertSaleDate(Convert.ToString(Date));
            else
                SaleDateDB.UpdateSaleDate(Convert.ToString(Date));
        }
    }
}