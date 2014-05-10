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
        protected Guid _token;
        protected DateTime _tokenExpirationDate;
        #endregion

        #region Properties
        public int UserID { get { return _userID; } }
        public string UserName { get { return _userName; } set { _userName = value; } }
        public string FirstName { get { return _firstName; } set { _firstName = value; } }
        public string LastName { get { return _lasteName; } set { _lasteName = value; } }
        public Guid Token { get { return _token; } set { _token = value; } }
        public DateTime TokenExpirationDate { get { return _tokenExpirationDate; } set { _tokenExpirationDate = value; } }
        
        #endregion

        #region Constructors

        public User(string userName, string password)
        {
            DataRow user = UserDB.GetUserByUserNameAndPassword(userName, password);
            if (user != null)
            {
                _userID = Convert.ToInt32(user["UserID"]);
                _userName = user["UserName"].ToString();
                _firstName = user["FirstName"].ToString();
                _lasteName = user["LastName"].ToString();
                _token = new Guid(user["Token"].ToString());
                _tokenExpirationDate = Convert.ToDateTime(user["TokenExpirationDate"]);
            }

        }
        public User(int userID)
        {
            DataRow user = UserDB.GetUserByUserID(userID);
            if (user != null)
            {
                _userID = Convert.ToInt32(user["UserID"]);
                _userName = user["UserName"].ToString();
                _firstName = user["FirstName"].ToString();
                _lasteName = user["LastName"].ToString();
                _token = new Guid(user["Token"].ToString());
                _tokenExpirationDate = Convert.ToDateTime(user["TokenExpirationDate"]);
            }
        }
        public User(Guid token)
        {
            DataRow user = UserDB.GetUserByToken(token);
            if (user != null)
            {
                _userID = Convert.ToInt32(user["UserID"]);
                _userName = user["UserName"].ToString();
                _firstName = user["FirstName"].ToString();
                _lasteName = user["LastName"].ToString();
                _token = new Guid(user["Token"].ToString());
                _tokenExpirationDate = Convert.ToDateTime(user["TokenExpirationDate"]);
            }
        }
        #endregion

        public void UpdateToken()
        {
            if (UserID != null)
            {
                Guid newToken = Guid.NewGuid();
                _tokenExpirationDate = UserDB.UpdateToken(_userID, newToken, _token);
                _token = newToken;
            }
            else
            {
                throw new Exception("Update was called before user was initialized");
            }
        }

        #region Static methods
        public static bool ThisIsAUser(string userName, string password)
        {
            return UserDB.ThisIsAUser(userName, password);
        }

        #endregion
    }
}