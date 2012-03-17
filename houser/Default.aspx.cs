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

namespace houser
{
    public partial class _Default : System.Web.UI.Page
    {
        public void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bool nonLiveData = chkNonLive.Checked;
                string sheriffSaleDatePage = GetWebRequest("http://oklahomacounty.org/sheriff/SheriffSales/", "SheriffSales", nonLiveData);
                List<string> dates = PageScraper.GetSheriffSaleDates(sheriffSaleDatePage);
                foreach (var date in dates)
                {
                    ddlSaleDate.Items.Add(date);
                }
            }
        }

        /// <summary>
        /// Build the entire data structure for all properties.  Includes comparable data, property specs, ect.  03%2f15%2f2012
        /// </summary>
        private static Dictionary<string, Dictionary<int, Dictionary<string, string>>> GetCompletePropertyList(string saleDate, bool nonLiveDataOnly)
        {
            Dictionary<string, Dictionary<int, Dictionary<string,string>>> allPropertyData = new Dictionary<string, Dictionary<int, Dictionary<string,string>>>();
            Dictionary<int, Dictionary<string, string>> allCoreDataTMP = new Dictionary<int, Dictionary<string, string>>();
            Dictionary<string, string> allFieldDataTMP = new Dictionary<string, string>();
            string sherifSaleUrl = "http://oklahomacounty.org/sheriff/SheriffSales/saledetail.asp?SaleDates="+saleDate;
            string sherifSaleWebRequestData = GetWebRequest(sherifSaleUrl, saleDate, nonLiveDataOnly);
            string currentPropertyAddress = "No Address Found";
            Dictionary<int, Dictionary<string, string>> SheriffSaleProperties = PageScraper.Find(sherifSaleWebRequestData);
            
            foreach (var property in SheriffSaleProperties)
            {
                
                currentPropertyAddress = property.Value["Address"];
                foreach (var pItem in property.Value)
                {
                    allFieldDataTMP.Add(pItem.Key, pItem.Value);
                }
                string propertyAccountURL = property.Value["8"];
                string fileName = propertyAccountURL.Substring(67);
                string propertyAssessorData = propertyAccountURL != "" ? GetWebRequest(propertyAccountURL, fileName, nonLiveDataOnly) : "Error";
                Dictionary<string, string> scrapedData = new Dictionary<string, string>(PageScraper.GetPropertyData(propertyAssessorData));
                foreach (var sdItem in scrapedData)
                {
                    allFieldDataTMP.Add(sdItem.Key, sdItem.Value);
                }
                string similarPropertyData = scrapedData["SimilarPropURL"] != "" ? GetWebRequest(scrapedData["SimilarPropURL"], "C"+fileName, nonLiveDataOnly) : "Error";
                Dictionary<int, Dictionary<string, string>> scrapedCoreData = new Dictionary<int, Dictionary<string, string>>(PageScraper.GetSimilarData(similarPropertyData));
                int i = 0;
                foreach (var scdItem in scrapedCoreData)
                {
                    foreach (var scdSubItem in scdItem.Value)
                    {
                        allFieldDataTMP.Add(scdSubItem.Key, scdSubItem.Value);
                    }
                    //if (scdItem.Key == 0)
                    //    allCoreDataTMP.Add(0, allFieldDataTMP);
                    //else
                    allCoreDataTMP.Add(i, new Dictionary<string, string>(allFieldDataTMP));
                    allFieldDataTMP.Clear();
                    i++;
                }
                allPropertyData.Add(currentPropertyAddress, new Dictionary<int, Dictionary<string, string>>(allCoreDataTMP));
                allCoreDataTMP.Clear();
            }
            return allPropertyData;
        }


        /// <summary>
        /// Request the webpage as a string.
        /// </summary>
        public static string GetWebRequest(string url, string fileName, bool nonLiveDataOnly)
        {
            string workPath = @"C:\Users\Daniel\GitProjectsPersonal\Houser\houser\webCache\";
            string homePath = @"F:\houser\houser\webCache\";
            string pathToUse = System.Environment.MachineName == "RYAN-PC"? workPath : homePath;

            if (!File.Exists(pathToUse + fileName + ".txt") || fileName == "xxSheriffSales")
            if(!nonLiveDataOnly)
            {
                string strResults = "";
                WebResponse objResponse;
                WebRequest objRequest = System.Net.HttpWebRequest.Create(url);

                try
                {
                    objResponse = objRequest.GetResponse();


                    using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
                    {
                        strResults = sr.ReadToEnd();
                        sr.Close();
                        System.IO.File.WriteAllText(pathToUse + fileName + ".txt", strResults);
                        return strResults;
                    }
                }
                catch { return ""; }
            }
            else
            {
                if (File.Exists(pathToUse + fileName + ".txt"))
                    return System.IO.File.ReadAllText(pathToUse + fileName + ".txt");
                else
                    return "";

            }
            else
            {
                return System.IO.File.ReadAllText(pathToUse + fileName + ".txt");
            }
            }
        
        /// <summary>
        /// Builds the view of properties.
        /// </summary>
        private void BuildSheriffSalePropertyList()
        {
            string saleDate = ddlSaleDate.SelectedItem.Value.Replace("/", "%2f");
            Dictionary<string, Dictionary<int, Dictionary<string, string>>> CompletePropertyList = new Dictionary<string, Dictionary<int, Dictionary<string, string>>>(GetCompletePropertyList(saleDate, chkNonLive.Checked));
            string red = "red";
            string green = "green";
            string orange = "orange";
            string blue = "blue";
            string yellow = "yellow";
            string black = "black";
            string reliabilityColor = black;
            double avgSalePrice = -1;
            double avgReliability = -1;
            string sDateTime;
            string sPrice;
            double[,] avgSalePriceAndReliability = new double[1,2];
            foreach (KeyValuePair<string, Dictionary<int, Dictionary<string, string>>> property in CompletePropertyList)
            {
                try
                {
                    reliabilityColor = black;
                    //ppsftDiff = RateProps.CompareProp(property.Value);
                    try
                    {
                        avgSalePriceAndReliability[0, 0] = 0;
                        avgSalePriceAndReliability[0, 1] = 0;
                        avgSalePrice = 0;
                        avgReliability = 0;
                        avgSalePriceAndReliability = RateProps.GetProprtyValueByComps(property.Value);
                        avgSalePrice = avgSalePriceAndReliability[0, 1];
                        avgReliability = avgSalePriceAndReliability[0, 0];
                        if (avgReliability > 7)
                            reliabilityColor = yellow;
                        if (avgReliability > 8)
                            reliabilityColor = orange;
                        if (avgReliability > 9)
                            reliabilityColor = green;
                        
                        
                        
                    }
                    catch { avgSalePrice = 0; }
                    sDateTime = property.Value[0].ContainsKey("SaleDate") ? property.Value[0]["SaleDate"] : "NA";
                    sPrice = property.Value[0].ContainsKey("SaleDate") ? property.Value[0]["SalePrice"] : "NA";

                    displayPanel.Controls.Add(new LiteralControl("<table class=\"Address\">"));
                    displayPanel.Controls.Add(new LiteralControl("<tr class=\"subjectProperty\">"));
                    displayPanel.Controls.Add(new LiteralControl("<td class=\"" + blue + "\">" + property.Key + "</td><td class=\"priceRank " 
                        + reliabilityColor + "\">Avg Sale Price  " + avgSalePrice + "</td><td class=\"reliability " + reliabilityColor + "\">Reliability of comps  " 
                        + avgReliability + "</td><td>Minimum Bid = " + Convert.ToString(Convert.ToDouble((property.Value[0]["Appraisal Value"])) * .66) 
                        + "</td><td>Last Sale Date = " + sDateTime
                        + "</td><td>Last Sale Price = " + sPrice
                        + "</tr><tr class=\"property\">"));
                    foreach (var field in property.Value)
                    {

                        if (field.Key < 2)
                        {
                            displayPanel.Controls.Add(new LiteralControl("<td>" + Convert.ToString(field.Key == 0 ? "Subject Property" : "Compare Property") + "</td></tr><tr class=\"fieldTitle\">"));
                            foreach (var f in field.Value)
                            {
                                if (f.Key != "Plantiff" && f.Key != "Defendant" && f.Key != "Attorney" && f.Key != "Attorney Phone" && f.Key != "Address")
                                {
                                    if (f.Key == "SaleDate" || f.Key == "SalePrice")
                                        displayPanel.Controls.Add(new LiteralControl("<td bgcolor=\"#FFB691\">" + f.Key + "</td>"));
                                    else if (f.Key == "SaleDate1" || f.Key == "SalePrice1")
                                        displayPanel.Controls.Add(new LiteralControl("<td bgcolor=\"#A5D4A8\">" + f.Key + "</td>"));
                                    else if (f.Key == "SaleDate2" || f.Key == "SalePrice2")
                                        displayPanel.Controls.Add(new LiteralControl("<td bgcolor=\"#A5B6E8\">" + f.Key + "</td>"));
                                    else
                                        displayPanel.Controls.Add(new LiteralControl("<td>" + f.Key + "</td>"));
                                }
                            }
                        }
                        displayPanel.Controls.Add(new LiteralControl("</tr><tr class=\"fieldValue\">"));
                        foreach (var f in field.Value)
                        {
                            if (f.Key == "8")
                                displayPanel.Controls.Add(new LiteralControl("<td><a href=\"" + f.Value + "\" target=\"_blank\">See Assessors Page</a></td>"));
                            else if (f.Key == "9")
                                displayPanel.Controls.Add(new LiteralControl("<td><a href=\"" + f.Value + "\" target=\"_blank\">See Tax Info</a></td>"));
                            else if (f.Key == "SimilarPropURL")
                                displayPanel.Controls.Add(new LiteralControl("<td><a href=\"" + f.Value + "\">See Comps</a></td>"));
                            else if (f.Key != "Plantiff" && f.Key != "Defendant" && f.Key != "Attorney" && f.Key != "Attorney Phone" && f.Key != "Address")
                                displayPanel.Controls.Add(new LiteralControl("<td>" + f.Value + "</td>"));
                        }
                        displayPanel.Controls.Add(new LiteralControl("</tr>"));
                    }
                    displayPanel.Controls.Add(new LiteralControl("</tr>"));
                    displayPanel.Controls.Add(new LiteralControl("</table>"));
                }
                catch { }

            }
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