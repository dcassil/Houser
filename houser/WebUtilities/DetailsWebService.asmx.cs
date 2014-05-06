using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using houser.Business;
using System.Data;
using houser.utilities;
using System.Web.Script.Serialization;
using System.Web.Script.Services;

namespace houser.WebUtilities
{
    /// <summary>
    /// Summary description for DetailsWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class DetailsWebService : System.Web.Services.WebService
    {
        /// <summary>
        /// Get property comps by subject property account number and return as json.
        /// </summary>
        [WebMethod]
        public string GetPropertyCompsByAccountNumber(string accountNumber)
        {
            DataTable results =  PropertyComp.GetCompsForPropertyByAccountNumber(accountNumber);
            if (results.Rows.Count > 0)
                return jsonHelper.GetJSONString(results);
            else
                return "";
        }

        /// <summary>
        /// Add property to property list.
        /// </summary>
        /// <param name="accountNumber"></param>
        [WebMethod]
        public void AddToReviewList(string accountNumber, int listID, int userID)
        {
            PropertyList.RemoveFromFromList(accountNumber, listID, userID);
            PropertyList.AddPropertyToList(accountNumber, listID, userID);
        }

        /// <summary>
        /// Remove property from review list.
        /// </summary>
        /// <param name="accountNumber"></param>
        [WebMethod]
        public void RemovePropertyFromList(string accountNumber, int listID, int userID)
        {
            PropertyList.RemoveFromFromList(accountNumber, listID, userID);
        }

        /// <summary>
        /// Get property list by sale date and list type and return as json.
        /// </summary>
        [WebMethod]
        //[ScriptMethod(UseHttpGet = true)]
        public string GetPropertiesBySaleDate(string sDate, string list, string sUserID)
        {
            DateTime date = Convert.ToDateTime(sDate);
            int userID = Convert.ToInt32(sUserID);
            User user = new User(userID);
            if (date != null && user != null)
            {
                SaleDate saleDate = new SaleDate(date);
                if (saleDate.LastIndexed.AddDays(1) < DateTime.Now && list == "2")
                {
                    PageScraper.GetCompletePropertyList(sDate);
                    saleDate.save();
                }

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                DataTable subjectProperties = SaleRecord.GetSaleProperitesByDate(Convert.ToDateTime(date), "Address", list, Convert.ToInt32(userID));
                if (subjectProperties.Rows.Count > 0)
                    return jsonHelper.GetJson(subjectProperties);
                else
                    return "";
            }
            else return "";
        }

        [WebMethod]
        public object GetSherifSaleDates()
        {
            // Request the sherifsale page so we can get the available sale dates.
            string sheriffSaleDatePage = PageRequester.GetWebRequest("http://oklahomacounty.org/sheriff/SheriffSales/");
            // Create a list of dates
            List<string> dates = PageScraper.GetSheriffSaleDates(sheriffSaleDatePage);
            return dates;
        }
    }
}

