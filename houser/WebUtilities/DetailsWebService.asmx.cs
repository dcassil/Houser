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
        [WebMethod]
        public string GetPropertyCompsByAccountNumber(string accountNumber)
        {
            DataTable results =  PropertyComp.GetCompsForPropertyByAccountNumber(accountNumber);
            if (results.Rows.Count > 0)
                return jsonHelper.GetJSONString(results);
            else
                return "";
        }

        [WebMethod]
        public void MovePropertyToList(string accountNumber, int list)
        {
            PropertyList.UpdatePropertyList(accountNumber, list);
        }
    }
}
