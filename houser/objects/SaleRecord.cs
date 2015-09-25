using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace houser.objects
{
    public class SaleRecord
    {
        protected List<SaleRecordItem> _saleRecordList = new List<SaleRecordItem>();
        protected DateTime _saleDate;

        public List<SaleRecordItem> Items { get { return _saleRecordList; } set { _saleRecordList = value; } }
        public DateTime SaleDate { get { return _saleDate; } set { _saleDate = value; } }

        public SaleRecord(DateTime saleDate)
        {
            _saleDate = saleDate;
        }

        public void Add(SaleRecordItem saleRecordItem) 
        {
            _saleRecordList.Add(saleRecordItem);
        }

        public Object getWebObject()
        {
            return new { SaleDate = _saleDate, Properties = _saleRecordList };
        }
    }
}