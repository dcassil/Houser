using System.Web.Services;
using houser.utilities;
using System.Web;
using System.Collections.Generic;
using houser.objects;
using Newtonsoft.Json;


namespace houser.WebUtilities
{
    /// <summary>
    /// Summary description for DetailsWebService
    /// </summary>
    //[WebService(Namespace = "http://tempuri.org/")]
    //[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    //[System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class DetailsWebService : System.Web.Services.WebService
    {
        [WebMethod]
        public object GetSherifSaleDates()
        {
            // Request the sherifsale page so we can get the available sale dates.
            //string sheriffSaleDatePage = PageRequester.GetWebRequest("http://oklahomacounty.org/sheriff/SheriffSales/");
            string sheriffSaleDatePage = PageRequester.HttpPost("http://oklahomacounty.org/sheriff/SheriffSales/", "");
            // Create a list of dates
            List<string> dates = CountyScraper.GetSheriffSaleDates(sheriffSaleDatePage);
            return JsonConvert.SerializeObject(dates);
        }

        /// <summary>
        /// Get property list by sale date and list type and return as json.
        /// </summary>
        [WebMethod]
        //[ScriptMethod(UseHttpGet = true)]
        public object GetSherifSaleRecord(string sDate)
        {
            sDate = !string.IsNullOrEmpty(sDate) ? sDate : "10/08/2015";
            //string sherifSaleUrl = "http://oklahomacounty.org/sheriff/SheriffSales/saledetail.asp?SaleDates=" + saleDate;
            string sherifSaleUrl = "http://oklahomacounty.org/sheriff/SheriffSales/saledetail.asp";
            //string sherifSaleWebRequestData = PageRequester.GetWebRequest(sherifSaleUrl);
            string sherifSaleWebRequestData = PageRequester.HttpPost(sherifSaleUrl, "SaleDates=" + sDate);
            if (!string.IsNullOrWhiteSpace(sherifSaleWebRequestData))
            {
                SaleRecord saleRecord = CountyScraper.ScrapeSaleRecord(sherifSaleWebRequestData, sDate);
                return JsonConvert.SerializeObject(saleRecord.getWebObject());
            }
            else
            {
                return new { error = "Page not found" };
            }
        }
    }
}
