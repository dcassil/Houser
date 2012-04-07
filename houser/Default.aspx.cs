using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using houser.utilities;
using System.Text.RegularExpressions;
using System.Data;
using System.Web.UI.HtmlControls;
using houser.Data;

namespace houser
{
    public partial class _Default : System.Web.UI.Page
    {
        public void Page_Load(object sender, EventArgs e)
        {
            // If this is not a post back (first page load)
            if (!IsPostBack)
            {
                // See if we have checked to only get cached data...........This will probably go away.
                bool nonLiveData = chkNonLive.Checked;
                // Request the sherifsale page so we can get the available sale dates.
                string sheriffSaleDatePage = GetWebRequest("http://oklahomacounty.org/sheriff/SheriffSales/", "SheriffSales", nonLiveData);
                // Create a list of dates
                List<string> dates = PageScraper.GetSheriffSaleDates(sheriffSaleDatePage);
                foreach (var date in dates)
                {
                    // Add dates to our drop down list.
                    ddlSaleDate.Items.Add(date);
                }
            }
        }

        /// <summary>
        /// Build the entire data structure for all properties.  Includes comparable data, property specs, ect.  03%2f15%2f2012
        /// </summary>
        private static string GetCompletePropertyList(string saleDate, bool nonLiveDataOnly)
        {
            //Dictionary<string, Dictionary<int, Dictionary<string,string>>> allPropertyData = new Dictionary<string, Dictionary<int, Dictionary<string,string>>>();
            //Dictionary<int, Dictionary<string, string>> allCoreDataTMP = new Dictionary<int, Dictionary<string, string>>();
            //Dictionary<string, string> allFieldDataTMP = new Dictionary<string, string>();
            string sherifSaleUrl = "http://oklahomacounty.org/sheriff/SheriffSales/saledetail.asp?SaleDates="+saleDate;
            string sherifSaleWebRequestData = GetWebRequest(sherifSaleUrl, saleDate, nonLiveDataOnly);
            // Get the list of all properties for this sherif sale dat.
            Dictionary<int, Dictionary<string, string>> SheriffSaleProperties = PageScraper.Find(sherifSaleWebRequestData, saleDate);
            
            foreach (var property in SheriffSaleProperties)
            {
                // get the accoutn url.
                string propertyAccountURL = property.Value["8"];
                // get the account number.
                string accountNumber = propertyAccountURL.Substring(67);
                DateTime? lastUpadteOrNull = Properties.AccountNumberAlreadyInTable(accountNumber);
                bool dataHasExpired = (lastUpadteOrNull != null ? Convert.ToDateTime(lastUpadteOrNull) : DateTime.Now).AddDays(60) < DateTime.Now;
                // If we have a subject property result from this account number already.  then dont scrape the page.
                //  Find a better way to do this.  maybe add a is subject column to properties.  Otherwise we are now hitting the live data several dozen times per run.

                if (!PropertyComps.PropertyHasComps(accountNumber) || dataHasExpired)
                {
                    // Get the webrequest data for this property account.
                    string propertyAssessorData = propertyAccountURL != "" ? GetWebRequest(propertyAccountURL, accountNumber, nonLiveDataOnly) : "Error";
                    // Scrape this webrequest data into a list of useable data for this property.
                    Dictionary<string, string> scrapedData = new Dictionary<string, string>(PageScraper.GetPropertyData(propertyAssessorData));
                    // Get the webrequest data for the similar properties if they exist.
                    string similarPropertyData = scrapedData["SimilarPropURL"] != "" ? GetWebRequest(scrapedData["SimilarPropURL"], "C"+accountNumber, nonLiveDataOnly) : "Error";
                    // Scrape this webrequest into a list of useable data for the similar properties--- also called comps.  need to stick to one convention.
                    Dictionary<int, Dictionary<string, string>> scrapedCoreData = new Dictionary<int, Dictionary<string, string>>(PageScraper.GetSimilarData(similarPropertyData, accountNumber, scrapedData));
                }            
            }
            return "Finished Loading";
        }


        /// <summary>
        /// Request the webpage as a string.
        /// </summary>
        public static string GetWebRequest(string url, string accountNumber, bool nonLiveDataOnly)
        {
            // Try to get a property record by account number.  or return No_DATA_29454.  this needs to be replaced by soemthing not dumb.
            //string propertyRecord = WebRequestDB.GetPropertyRecord(accountNumber);

            //if (propertyRecord == "No_DATA_29454" || accountNumber == "SheriffSales")
            //if(!nonLiveDataOnly)
            //{
            string strResults = "";
            WebResponse objResponse;
            // request a url.
            WebRequest objRequest = System.Net.HttpWebRequest.Create(url);

            try
            {
                // get the data from our url
                objResponse = objRequest.GetResponse();


                using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
                {
                    // create a string of the entire page we just requested.
                    strResults = sr.ReadToEnd();
                    sr.Close();
                    // if this is not a sherifsale request and not a date request then its a account number request.  this is gay too.  should be less cryptic.
                    //if (accountNumber != "SheriffSales" && !accountNumber.Contains(@"%"))
                    //    // write the entire request string to the db.  this is also dumb and needs to go away.
                    //    WebRequestDB.WritePropertyRecord(accountNumber, strResults);

                    return strResults;
                }
            }
            catch { return ""; }
        }
            //}
            //else
            //{
            //    return propertyRecord;
            //    else
            //        return "";

            //}
            //else
            //{
            //    return propertyRecord;
            //}
            //}
        
        /// <summary>
        /// Builds the view of properties.
        /// </summary>
        private void BuildSheriffSalePropertyList()
        {
            string saleDate = ddlSaleDate.SelectedItem.Value.Replace("/", "%2f");
            string finishedLoading = GetCompletePropertyList(saleDate, chkNonLive.Checked);
            try
            {
                displayPanel.Controls.Add(new LiteralControl("<p>" + finishedLoading + "</p>"));
            }
            catch {}
            
        }
        

        #region UI events

        protected void btnPopulateData_Click(object sender, EventArgs e)
        {
            BuildSheriffSalePropertyList();
        }

        protected void ddlSaleDate_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        #endregion

        

    }
}