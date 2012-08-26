﻿using System;
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
        private static string CONNECTIONSTRING = ConfigurationManager.ConnectionStrings["HouserConnectionString1"].ConnectionString;

        // Insert new entry.
        public static int InsertSaleRecord(string accountNumber, DateTime saleDate, double salePrice)
        {
            var result = SqlHelper.ExecuteScalar(CONNECTIONSTRING, CommandType.Text,
                @"INSERT INTO SaleRecord (AccountNumber, SaleDate, SalePrice) VALUES (@AccountNumber, @SaleDate, @SalePrice) SELECT @@IDENTITY",
                new SqlParameter("@AccountNumber", accountNumber),
                new SqlParameter("@SaleDate", saleDate),
                new SqlParameter("@SalePrice", salePrice));
            return Convert.ToInt32(result);
        }

        //Update entry 
        public static void UpdateSaleRecord(string accountNumber, DateTime saleDate, double salePrice, int saleRecordID, DateTime dateModified)
        {
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRING, CommandType.Text,
                @"UPDATE SaleRecord SET AccountNumber =@AccountNumber, SaleDate = @SaleDate, SalePrice = @SalePrice, DateModified = @DateModified 
                WHERE SaleRecordID = @SaleRecordID",
                new SqlParameter("@AccountNumber", accountNumber),
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
        public static DataTable GetSaleProperitesByDate(DateTime saleDate, string orderBy, string listID)
        {
            DataSet results = SqlHelper.ExecuteDataset(CONNECTIONSTRING, CommandType.Text,
                @"SELECT * FROM SaleRecord s
                INNER JOIN Property p ON s.AccountNumber = p.AccountNumber
                INNER JOIN AccountNumberList anl ON s.AccountNumber = anl.AccountNumber
                LEFT OUTER JOIN Note n ON p.AccountNumber = n.AccountNumber
                WHERE s.SaleDate = @SaleDate AND anl.ListID = @ListID ORDER BY
                    CASE @OrderBy 
                        WHEN 'Address' THEN p.Address
                        END,
                    CASE @OrderBy
                        WHEN 'SalePrice' THEN s.SalePrice   
                        WHEN 'Beds' THEN p.Beds 
                        WHEN 'Baths' THEN p.Baths
                        WHEN 'Sqft' THEN p.Sqft END",
                new SqlParameter("@SaleDate", saleDate),
                new SqlParameter("@ListID", listID),
                new SqlParameter("@OrderBy", orderBy));
            return results.Tables[0];
        }
    }
}