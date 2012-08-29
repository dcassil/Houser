using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.ApplicationBlocks.Data;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


namespace houser.Data
{
    public class UserDB
    {
        private static string CONNECTIONSTRING = ConfigurationManager.ConnectionStrings["SQLSERVER_CONNECTION_STRING"].ConnectionString;

        #region Static methods
        public static bool ThisIsAUser(string userName, string password)
        {
            var result = SqlHelper.ExecuteScalar(CONNECTIONSTRING, CommandType.Text,
                "SELECT * FROM [User] WHERE UserName = @UserName AND Password = @Password",
                new SqlParameter("@UserName", userName),
                new SqlParameter("@Password", password));
            return result != null;
        }
        #endregion
    }
}