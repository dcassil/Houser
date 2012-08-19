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
using System.Linq;
using System.Reflection;
using System.Text;
using houser.Business;

namespace houser
{
    public partial class _Default : System.Web.UI.Page
    {
        public void Page_Load(object sender, EventArgs e)
        {
            
           //If this is not a post back (first page load)
            if (!IsPostBack)
            {
                // See if we have checked to only get cached data...........This will probably go away.
                bool nonLiveData = chkNonLive.Checked;
                // Request the sherifsale page so we can get the available sale dates.
                string sheriffSaleDatePage = PageRequester.GetWebRequest("http://oklahomacounty.org/sheriff/SheriffSales/");
                // Create a list of dates
                List<string> dates = PageScraper.GetSheriffSaleDates(sheriffSaleDatePage);
                foreach (var date in dates)
                {
                    // Add dates to our drop down list.
                    ddlSaleDate.Items.Add(date);
                }

            }
        }

        public void BuildListingPanels(DateTime date)
        {
            //BindingFlags b = BindingFlags.Instance | BindingFlags.Public;
            DataTable subjectProperties = SaleRecord.GetSaleProperitesByDate(date);
            int i = 0;
            string listingPnlClass;
            StringBuilder html = new StringBuilder();
            foreach (DataRow property in subjectProperties.Rows)
            {
                html.Clear();
                if (i == 0)
                {
                    listingPnlClass = "listingPanel";
                    i = 1;
                }
                else
                {
                    listingPnlClass = "listingPanelx";
                    i = 0;
                }
                html.Append("<div class=\"listingWrapper\">");
                html.Append("<div class=\"indicator\"></div>");
                html.Append("<div id=\"" + property["AccountNumber"].ToString() + "\" class=\"" + listingPnlClass + "\">");
                html.Append("<span class=\"propertyData\">");
                html.Append("<span class=\"address\">" + property["Address"].ToString() + "</span>");
                html.Append("<span class=\"minBidWrapper\">$" + Convert.ToString(Convert.ToInt32(property["SalePrice"]) * .66) + "</span>");
                html.Append("<span class=\"sqft\">" + property["Sqft"].ToString() + "</span>");
                html.Append("<span class=\"beds\">" + property["Beds"].ToString() + "</span>");
                html.Append("<span class=\"baths\">" + property["baths"].ToString() + "</span>");
                html.Append("</span>");
                html.Append("</div>");
                html.Append("</div>");

                pnlListingPanel.Controls.Add(new LiteralControl(html.ToString()));
                // go here to see how to use scroll up down events http://api.jquery.com/scroll/  we will use it to make sure key down and wheel down move down one listing exactly.
            }
        }

        /// <summary>
        /// Build the entire data structure for all properties.  Includes comparable data, property specs, ect.  03%2f15%2f2012
        /// </summary>
        private static string GetCompletePropertyList(string saleDate, bool nonLiveDataOnly)
        {
            string sherifSaleUrl = "http://oklahomacounty.org/sheriff/SheriffSales/saledetail.asp?SaleDates=" + saleDate;
            string sherifSaleWebRequestData = PageRequester.GetWebRequest(sherifSaleUrl);
            if (!string.IsNullOrWhiteSpace(sherifSaleWebRequestData))
                PageScraper.ScrapePropertyDatePiecesIntoDatabase(sherifSaleWebRequestData, saleDate);
            return "Finished Loading";
        }

        /// <summary>
        /// Builds the view of properties.
        /// </summary>
        private void BuildSheriffSalePropertyList()
        {
            if (!chkNonLive.Checked)
            {
                string saleDate = ddlSaleDate.SelectedItem.Value.Replace("/", "%2f");
                string finishedLoading = GetCompletePropertyList(saleDate, chkNonLive.Checked);
            }
            try
            {
                BuildListingPanels(Convert.ToDateTime(ddlSaleDate.SelectedItem.Value));
            }
            catch { }
        }


        #region UI events

        protected void btnPopulateData_Click(object sender, EventArgs e)
        {
            if (!chkTestMode.Checked)
                BuildSheriffSalePropertyList();
            else
                BuildTestProperties();
        }

        private void BuildTestProperties()
        {

            BuildListingPanels(Convert.ToDateTime("2001/01/01"));
            displayPanel.Controls.Add(new LiteralControl("<p>" + "Test Data" + "</p>"));
        }

        protected void ddlSaleDate_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        #endregion



    }
}