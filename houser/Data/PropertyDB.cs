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
    public class PropertyDB
    {
        private static string CONNECTIONSTRING = ConfigurationManager.ConnectionStrings["HouserConnectionString1"].ConnectionString;
        #region static methods

        /// <summary>
        /// Example of sqlhelper execute dataset.
        /// </summary>
        /// <param name="accountNumber"></param>
        /// <returns></returns>
        public static DataRow GetPropertyByAccount(string accountNumber)
        {
            DataSet results =  SqlHelper.ExecuteDataset(CONNECTIONSTRING, CommandType.Text,
                @"SELECT TOP (1) * FROM Property WHERE AccountNumber = @AccountNumber",
                new SqlParameter("@AccountNumber", accountNumber));
            return results.Tables[0].Rows.Count > 0 ? results.Tables[0].Rows[0] : null;
        }


        #endregion
    }
}