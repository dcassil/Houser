using System.Web.Services;
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
        public string GetSaleDates()
        {
            return PageRequester.GetWebRequest("http://oklahomacounty.org/sheriff/SheriffSales/");
        }

        [WebMethod]
        public string GetWebRequest(string url)
        {
            return PageRequester.GetWebRequest(url);
        }
    }
}
