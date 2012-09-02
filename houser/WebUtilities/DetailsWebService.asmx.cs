using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using houser.Business;
using System.Data;
using houser.utilities;

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
    }
}
