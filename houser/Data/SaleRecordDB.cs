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
    public class SaleRecordDB
    {
        private static string CONNECTIONSTRING = ConfigurationManager.ConnectionStrings["SQLSERVER_CONNECTION_STRING"].ConnectionString;

        // Insert new entry.
        public static int InsertSaleRecord(string accountNumber, string address, string caseNumber, DateTime saleDate, double salePrice)
        {
            var result = SqlHelper.ExecuteScalar(CONNECTIONSTRING, CommandType.Text,
                @"INSERT INTO SaleRecord (AccountNumber, Address, CaseNumber, SaleDate, SalePrice) VALUES (@AccountNumber, @Address, @CaseNumber, @SaleDate, @SalePrice) SELECT @@IDENTITY",
                new SqlParameter("@AccountNumber", accountNumber),
                new SqlParameter("@SaleDate", saleDate),
                new SqlParameter("@SalePrice", salePrice),
                new SqlParameter("@Address", address),
                new SqlParameter("@CaseNumber", caseNumber));
            return Convert.ToInt32(result);
        }

        //Update entry 
        public static void UpdateSaleRecord(string accountNumber, string address, string caseNumber, DateTime saleDate, double salePrice, int saleRecordID, DateTime dateModified)
        {
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRING, CommandType.Text,
                @"UPDATE SaleRecord SET AccountNumber = @AccountNumber, Address = @Address, CaseNumber = @CaseNumber, SaleDate = @SaleDate, SalePrice = @SalePrice, DateModified = @DateModified 
                WHERE SaleRecordID = @SaleRecordID",
                new SqlParameter("@AccountNumber", accountNumber),
                new SqlParameter("@Address", address),
                new SqlParameter("@CaseNumber", caseNumber),
                new SqlParameter("@SaleDate", saleDate),
                new SqlParameter("@SalePrice", salePrice),
                new SqlParameter("@SaleRecordID", saleRecordID),
                new SqlParameter("@DateModified", dateModified));
        }

        //  Get SaleRecord by accountNumber and saleDate.  if there are more than on get the most recent.
        public static DataRow GetMostRecentSaleRecordByAccountNumberAndSaleDate(string accountNumber, DateTime saleDate)
        {
            DataSet results = SqlHelper.ExecuteDataset(CONNECTIONSTRING, CommandType.Text,
                @"SELECT TOP (1) * FROM SaleRecord WHERE AccountNumber = @AccountNumber AND SaleDate = @SaleDate ORDER BY DateModified",
                new SqlParameter("@AccountNumber", accountNumber),
                new SqlParameter("@SaleDate", saleDate));
            return results.Tables[0].Rows.Count > 0 ? results.Tables[0].Rows[0] : null;
        }

        // Get records by date.  // limited to top 8 while testing.
        public static DataTable GetSaleProperitesByDate(DateTime saleDate, string orderBy, string listID, int userID)
        {
            DataSet results = SqlHelper.ExecuteDataset(CONNECTIONSTRING, CommandType.Text,
                @"SELECT *, (SELECT CASE (SELECT COUNT (*) FROM PropertyList 
                                WHERE ListID = 1 AND AccountNumber = s.AccountNumber AND UserID = @UserID) 
                                WHEN 0 THEN 'false' WHEN 1 THEN 'true' END) AS 'InReviewList'
                FROM SaleRecord s
                INNER JOIN Property p ON s.AccountNumber = p.AccountNumber
                INNER JOIN PropertyList pl ON s.AccountNumber = pl.AccountNumber
                LEFT OUTER JOIN Note n ON p.AccountNumber = n.AccountNumber AND n.UserID = @UserID
                WHERE s.SaleDate = @SaleDate AND pl.ListID = @ListID AND (pl.UserID = @UserID OR (pl.UserID = 0 AND @ListID = 2)) ORDER BY
                    CASE @OrderBy 
                        WHEN 'Address' THEN p.Address
                        END,
                    CASE @OrderBy
                        WHEN 'SalePrice' THEN s.SalePrice   
                        WHEN 'Beds' THEN p.Beds 
                        WHEN 'Baths' THEN p.Baths
                        WHEN 'LotSize' THEN p.LotSize
                        WHEN 'Sqft' THEN p.Sqft END",
                new SqlParameter("@SaleDate", saleDate),
                new SqlParameter("@ListID", listID),
                new SqlParameter("@OrderBy", orderBy),
                new SqlParameter("@UserID", userID));
            return results.Tables[0];
        }
    }
}