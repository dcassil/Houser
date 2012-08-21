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
        private static string CONNECTIONSTRING = ConfigurationManager.ConnectionStrings["HouserConnectionString1"].ConnectionString;

        #region Static Methods
        public static DataRow GetNotesByAccountNumber(string accountNumber)
        {
            var results = SqlHelper.ExecuteDataset(CONNECTIONSTRING, CommandType.Text,
                "SELECT * FROM Note WHERE AccountNumber = @AccountNumber ORDER BY DateCreated",
                new SqlParameter("@AccountNumber", accountNumber)).Tables[0];
            return results.Rows.Count > 0 ? results.Rows[0] : null;
        }

        public static void InsertNote(string accountNumber, string note)
        {
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRING, CommandType.Text,
                "INSERT INTO Note (AccountNumber, Note) VALUES (@AccountNumber, @Note)",
                new SqlParameter("@AccountNumber", accountNumber),
                new SqlParameter("@Note", note));
        }

        public static void UpdateNote(string accountNumber, string note)
        {
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRING, CommandType.Text,
                "UPDATE Note SET Note = @Note WHERE AccountNumber = @AccountNumber",
                new SqlParameter("@AccountNumber", accountNumber),
                new SqlParameter("@Note", note));
        }
        #endregion
    }
}