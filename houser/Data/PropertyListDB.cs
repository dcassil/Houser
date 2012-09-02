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
    public class PropertyListDB
    {
        #region Static MEthods
        private static string CONNECTIONSTRING = ConfigurationManager.ConnectionStrings["SQLSERVER_CONNECTION_STRING"].ConnectionString;

        /// <summary>
        /// Remove property by accountnumber and listid
        /// </summary>
        public static void RemoveFromFromList(string accountNumber, int list, int userID)
        {
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRING, CommandType.Text,
                "DELETE FROM PropertyList WHERE AccountNumber = @AccountNumber AND ListID = @ListID AND UserID = @UserID",
                new SqlParameter("@ListID", list),
                new SqlParameter("@AccountNumber", accountNumber),
                new SqlParameter("@UserID", userID));
        }

        public static void AddPropertyToList(string accountNumber, int list, int userID)
        {
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRING, CommandType.Text,
                "INSERT INTO PropertyList (ListID, AccountNumber, UserID) VALUES (@ListID, @AccountNumber, @UserID)",
                new SqlParameter("@ListID", list),
                new SqlParameter("@AccountNumber", accountNumber),
                new SqlParameter("@UserID", userID));
        }

        public static bool PropertyListExist(string accountNumber, int list)
        {
            var result = SqlHelper.ExecuteScalar(CONNECTIONSTRING, CommandType.Text,
                "SELECT * FROM PropertyList WHERE AccountNumber = @AccountNumber",
                new SqlParameter("@AccountNumber", accountNumber));
            return result != null;
        }

        public static bool PropertyListExistByAccountAndList(string accountNumber, int list, int userID)
        {
            var result = SqlHelper.ExecuteScalar(CONNECTIONSTRING, CommandType.Text,
                "SELECT ListID FROM PropertyList WHERE ListID = @ListID AND AccountNumber = @AccountNumber AND UserID = @UserID",
                new SqlParameter("@AccountNumber", accountNumber),
                new SqlParameter("@ListID", list),
                new SqlParameter("@UserID", userID));
            return result != null;
        }
        #endregion
    }
}