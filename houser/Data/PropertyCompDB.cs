using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
using System.Configuration;

namespace houser.Data
{
    public class PropertyCompDB
    {
        private static string CONNECTIONSTRING = ConfigurationManager.ConnectionStrings["HouserConnectionString1"].ConnectionString;
        #region Static Methods

        public static bool PropertyCompExists(string accountNumber, string cAccountNumber)
        {
            var results = SqlHelper.ExecuteScalar(CONNECTIONSTRING, CommandType.Text,
                "SELECT AccountNumber FROM PropertyComp WHERE AccountNumber = @AccountNumber AND CAccountNumber = @CAccountNumber",
                new SqlParameter("@AccountNumber", accountNumber),
                new SqlParameter("CAccountNumber", cAccountNumber));
            return (string)results == accountNumber;
        }

        public static void InsertPropertyComp(string accountNumber, string cAccountNumber)
        {
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRING, CommandType.Text,
                "INSERT INTO PropertyComp (AccountNumber, CAccountNumber) VALUES (@AccountNumber, @CAccountNumber)",
                new SqlParameter("@AccountNumber", accountNumber),
                new SqlParameter("@CAccountNumber", cAccountNumber));
        }

        public static DataTable GetCompsForPropertyByAccountNumber(string accountNumber)
        {
            return SqlHelper.ExecuteDataset(CONNECTIONSTRING, CommandType.Text,
                @"SELECT p.[AccountNumber],[Address],[Sqft],[Baths],[Beds],[Exterior],[LastSaleDate],[LastSalePrice],[DateModified]
                    ,[GarageSize],[YearBuilt],[Type],[BuiltAs]
                    FROM [Houser].[dbo].[Property] p 
                    INNER JOIN Houser.dbo.PropertyComp pc ON p.AccountNumber = pc.CAccountNumber 
                    WHERE pc.AccountNumber = @AccountNumber",
                new SqlParameter("@AccountNumber", accountNumber)).Tables[0];
        }
        #endregion
    }
}