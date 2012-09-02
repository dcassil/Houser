using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Microsoft.ApplicationBlocks.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace houser.Data
{
    public class NoteDB
    {
        private static string CONNECTIONSTRING = ConfigurationManager.ConnectionStrings["SQLSERVER_CONNECTION_STRING"].ConnectionString;

        #region Static Methods
        public static DataRow GetNotesByAccountNumber(string accountNumber, int userID)
        {
            var results = SqlHelper.ExecuteDataset(CONNECTIONSTRING, CommandType.Text,
                "SELECT * FROM Note WHERE AccountNumber = @AccountNumber AND UserID = @UserID ORDER BY DateCreated",
                new SqlParameter("@AccountNumber", accountNumber),
                new SqlParameter("@UserID", userID)).Tables[0];
            return results.Rows.Count > 0 ? results.Rows[0] : null;
        }

        public static void InsertNote(string accountNumber, string note, int userID)
        {
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRING, CommandType.Text,
                "INSERT INTO Note (AccountNumber, Note, UserID) VALUES (@AccountNumber, @Note, @UserID)",
                new SqlParameter("@AccountNumber", accountNumber),
                new SqlParameter("@Note", note),
                new SqlParameter("@UserID", userID));
        }

        public static void UpdateNote(string accountNumber, string note, int userID)
        {
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRING, CommandType.Text,
                "UPDATE Note SET Note = @Note WHERE AccountNumber = @AccountNumber AND UserID = @UserID",
                new SqlParameter("@AccountNumber", accountNumber),
                new SqlParameter("@Note", note),
                new SqlParameter("@UserID", userID));
        }
        #endregion
    }
}