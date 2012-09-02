using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace houser.Data
{
    public class PropertyCompDB
    {
        private static string CONNECTIONSTRING = ConfigurationManager.ConnectionStrings["SQLSERVER_CONNECTION_STRING"].ConnectionString;
        #region Static Methods

        /// <summary>
        /// If property comp already exist return true.
        /// </summary>
        public static bool PropertyCompExists(string accountNumber, string cAccountNumber)
        {
            var results = SqlHelper.ExecuteScalar(CONNECTIONSTRING, CommandType.Text,
                "SELECT AccountNumber FROM PropertyComp WHERE AccountNumber = @AccountNumber AND CAccountNumber = @CAccountNumber",
                new SqlParameter("@AccountNumber", accountNumber),
                new SqlParameter("CAccountNumber", cAccountNumber));
            return (string)results == accountNumber;
        }

        /// <summary>
        /// Insert new property comp record.
        /// </summary>
        public static void InsertPropertyComp(string accountNumber, string cAccountNumber)
        {
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRING, CommandType.Text,
                "INSERT INTO PropertyComp (AccountNumber, CAccountNumber) VALUES (@AccountNumber, @CAccountNumber)",
                new SqlParameter("@AccountNumber", accountNumber),
                new SqlParameter("@CAccountNumber", cAccountNumber));
        }

        /// <summary>
        /// Get all of the comps for a property.
        /// </summary>
        /// <param name="accountNumber"></param>
        public static DataTable GetCompsForPropertyByAccountNumber(string accountNumber)
        {
            return SqlHelper.ExecuteDataset(CONNECTIONSTRING, CommandType.Text,
                @"SELECT p.[AccountNumber],[Address],[Sqft],[Baths],[Beds],[Exterior],[LastSaleDate],[LastSalePrice],[DateModified]
                    ,[GarageSize],[YearBuilt],[Type],[BuiltAs], ([LastSalePrice]/[Sqft]) AS 'PricePerSqft'
                    FROM [Property] p 
                    INNER JOIN PropertyComp pc ON p.AccountNumber = pc.CAccountNumber 
                    WHERE pc.AccountNumber = @AccountNumber",
                new SqlParameter("@AccountNumber", accountNumber)).Tables[0];
        }
        #endregion
    }
}