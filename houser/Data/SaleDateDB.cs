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
    public class SaleDateDB
    {
        private static string CONNECTIONSTRING = ConfigurationManager.ConnectionStrings["SQLSERVER_CONNECTION_STRING"].ConnectionString;

        public static DataRow GetSaleDate(string saleDate)
        {
            var results = SqlHelper.ExecuteDataset(CONNECTIONSTRING, CommandType.Text,
                "SELECT * FROM SaleDate WHERE SaleDate = @saleDate",
                new SqlParameter("@saleDate", saleDate)).Tables[0];
            return results.Rows.Count > 0 ? results.Rows[0] : null;
        }

        public static void InsertSaleDate(string saleDate)
        {
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRING, CommandType.Text,
                "INSERT INTO SaleDate (SaleDate, LastIndexed) VALUES (@saleDate, @lastIndexed)",
                new SqlParameter("@saleDate", saleDate),
                new SqlParameter("lastIndexed", Convert.ToString(DateTime.Now)));
        }

        public static void UpdateSaleDate(string saleDate)
        {
            SqlHelper.ExecuteNonQuery(CONNECTIONSTRING, CommandType.Text,
                "UPDATE SaleDate SET LastIndexed = @lastIndexed WHERE SaleDate = @saleDate",
                new SqlParameter("@saleDate", saleDate),
                new SqlParameter("@lastIndexed", Convert.ToString(DateTime.Now)));
        }
    }
}