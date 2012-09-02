using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace houser.Business
{
    public class List
    {
        #region Fields
        protected int _listID;
        protected string _name;
        #endregion

        #region Properties
        public int ListID { get { return _listID; } }
        public string Name { get { return _name; } set { _name = value; } }
        #endregion

        #region Constructors
        public List(string listID)
        {
                
        }
        
        #endregion
    }
}