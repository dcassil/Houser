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

        public static DataRow GetUserByUserNameAndPassword(string userName, string password)
        {
            DataSet result = SqlHelper.ExecuteDataset(CONNECTIONSTRING, CommandType.Text,
                "SELECT * FROM [User] WHERE UserName = @UserName AND Password = @Password",
                new SqlParameter("@UserName", userName),
                new SqlParameter("@Password", password));
            return result.Tables[0].Rows.Count > 0 ? result.Tables[0].Rows[0] : null;
        }

        public static DataRow GetUserByUserID(int userID)
        {
            DataSet result = SqlHelper.ExecuteDataset(CONNECTIONSTRING, CommandType.Text,
                "SELECT * FROM [User] WHERE UserID = @userID",
                new SqlParameter("@userID", userID));
            return result.Tables[0].Rows.Count > 0 ? result.Tables[0].Rows[0] : null;
        }

        public static DataRow GetUserByToken(Guid token)
        {
            DataSet result = SqlHelper.ExecuteDataset(CONNECTIONSTRING, CommandType.Text,
                "SELECT * FROM [User] WHERE Token = @Token",
                new SqlParameter("@Token", token));
            return result.Tables[0].Rows.Count > 0 ? result.Tables[0].Rows[0] : null;
        }

        public static DateTime UpdateToken(int userID, Guid token, Guid oldToken)
        {
            DateTime tokenExp = DateTime.Now.AddDays(30);
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRING, CommandType.Text,
                "UPDATE [User] SET Token = @Token, TokenExp = @TokenExp WHERE UserID = @userID AND Token = @OldToken",
                new SqlParameter("@Token", token),
                new SqlParameter("@OldToken", oldToken),
                new SqlParameter("@TokenExp", tokenExp),
                new SqlParameter("@userID", userID));
            return tokenExp;
        }

        public static int InsertUser(string accountNumber, string password)
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(CONNECTIONSTRING, CommandType.Text,
                "INSERT INTO [User] (AccountNumber, Password) VALUSE (@AccountNumber, @Password); SELECT @id = SCOPE_IDENTITY()",
                new SqlParameter("@accountNumber", accountNumber),
                new SqlParameter("@Password", password)));
        }
        
        #endregion
    }
}