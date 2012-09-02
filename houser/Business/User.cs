using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using houser.Data;
using System.Data;

namespace houser.Business
{
    public class User
    {
        #region Fields
        protected int _userID;
        protected string _userName;
        protected string _firstName;
        protected string _lasteName;
        private bool isNew;
        #endregion

        #region Properties
        public int UserID { get { return _userID; } }
        public string UserName { get { return _userName; } set { _userName = value; } }
        public string FirstName { get { return _firstName; } set { _firstName = value; } }
        public string LastName { get { return _lasteName; } set { _lasteName = value; } }
        #endregion

        #region Constructors

        public User() { }

        public User(string userName, string password)
        {
            DataRow user = UserDB.GetUserByUserNameAndPassword(userName, password);
            if (user != null)
            {
                _userID = Convert.ToInt32(user["UserID"]);
                _userName = user["UserName"].ToString();
                _firstName = user["FirstName"].ToString();
                _lasteName = user["LastName"].ToString();
            }

        }
        #endregion
        #region Static methods
        public static bool ThisIsAUser(string userName, string password)
        {
            return UserDB.ThisIsAUser(userName, password);
        }
        #endregion
    }
}