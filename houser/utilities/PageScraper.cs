using System;
using System.Collections.Generic;
using System.Web;
using System.Text.RegularExpressions;
using System.Data;
using houser.Data;
using houser.Business;
using System.Net;


namespace houser.utilities
{
    // look at this http://www.dotnetperls.com/scraping-html for some regex and to see what you were doing.
    public static class PageScraper
    {

        #region Fields
        
        #endregion

        // Scrapes the salerecord data and passes it to the property account scraper.
        public static void ScrapePropertyDatePiecesIntoDatabase(string file, string saleDate)
        {
            MatchCollection propertyListing = Regex.Matches(file, "<table cellpadding=\"0\" cellspacing=\"1\">\r\n\t(.*?)</table", RegexOptions.Singleline);
            
            foreach (Match pl in propertyListing)
            {
                string accountnuber = Regex.Matches(pl.Groups[1].Value, "ACCOUNTNO=(.*?)\"", RegexOptions.Singleline)[0].Groups[1].Value;
                DateTime _saleDate = Convert.ToDateTime(saleDate.Replace("%2f", " "));
                //Try to get an existing record or make a new one.
                SaleRecord saleRecord = new SaleRecord(accountnuber, _saleDate);

                if (saleRecord.DateModified.AddDays(1) < DateTime.Now)
                {
                    string regexhelper = Regex.Matches(pl.Groups[1].Value, "Case Number(.*?)</font>\r\n\t\t", RegexOptions.Singleline)[0].Groups[1].Value;
                    saleRecord.CaseNumber = Regex.Matches(regexhelper, "<td><font class=\"featureFont\">\r\n(.*?)\r\n", RegexOptions.Singleline)[0].Groups[1].Value.Trim();
                    MatchCollection PropertyRow = Regex.Matches(pl.Groups[1].Value, "<tr valign=\"top\"*>(.*?)</tr>", RegexOptions.Singleline);
                    saleRecord.Address = Regex.Matches(PropertyRow[2].Groups[1].Value, "<td><font class=\"featureFont\">\r\n(.*?)\r\n", RegexOptions.Singleline)[0].Groups[1].Value.Trim();
                    saleRecord.AccountNumber = accountnuber;
                    string salePriceS = Regex.Matches(PropertyRow[5].Groups[1].Value, "<td><font class=\"featureFont\">\r\n(.*?)\r\n", RegexOptions.Singleline)[0].Groups[1].Value;
                    double salePrice = 0;
                    double.TryParse(salePriceS, out salePrice);
                    saleRecord.SalePrice = salePrice;
                    saleRecord.SaleDate = Convert.ToDateTime(saleDate.Replace("%2f", " "));
                    saleRecord.Save();
                }
                GetPropertyDataFromWeb(accountnuber);
            }
        }


        private static void GetPropertyDataFromWeb(string accountNumber)
        {
            Property property = new Property(accountNumber);

            if (property.DateModified.AddDays(30) < DateTime.Now)
            {
                string url = "http://www.oklahomacounty.org/assessor/Searches/AN-R.asp?ACCOUNTNO=" + accountNumber;
                string file = PageRequester.GetWebRequest(url);

                if (!string.IsNullOrEmpty(file))
                {
                
                    // populate the property and return the url for the comps.
                    string urlSimilarPropertiesSubSet = PopulatePropertySalesInfoAndGetCompsURL(property, file);

                    //request the comps page file
                    string cUrl = Regex.Match(urlSimilarPropertiesSubSet, "'(.*?)'", RegexOptions.Singleline).Groups[1].Value.Trim();
                    file = PageRequester.GetWebRequest(cUrl);
                    // If we have a page then lets scrape it.
                    if (!string.IsNullOrEmpty(file))
                    {
                        //scope in the regex text area
                        string subjectPropertyTableSubSet = Regex.Match(file, "Property Information</font>(.*?)>Sales are pulled", RegexOptions.Singleline).Groups[1].Value.Trim();
                        string subjectPropertyTable = Regex.Match(subjectPropertyTableSubSet, "<tbody>(.*?)</tbody>", RegexOptions.Singleline).Groups[1].Value.Trim();
                        string regexHelpper = Regex.Match(subjectPropertyTable, "Built</font>(.*?)t></td>", RegexOptions.Singleline).Groups[1].Value.Trim();
                        property.YearBuilt = Convert.ToInt32(Regex.Match(regexHelpper, "<font size=\\\"2\\\" color=\\\"#0000FF\\\">(.*?)</fon", RegexOptions.Singleline).Groups[1].Value.Trim());
                        MatchCollection regexMatchHelper = Regex.Matches(subjectPropertyTable, "<p align=\\\"center\\\"><font size=\\\"2\\\">\\\r\\\n\\\t\\\t\\\t\\\t(.*?)</font></td>", RegexOptions.Singleline);
                        property.Address = regexMatchHelper[1].Groups[1].Value.Trim();
                        regexHelpper = Regex.Match(subjectPropertyTable, "Total SQFT</font>(.*?)t></td>", RegexOptions.Singleline).Groups[1].Value.Trim();
                        property.Sqft = Convert.ToInt32(Regex.Match(regexHelpper, "<font size=\\\"2\\\">(.*?)</fon", RegexOptions.Singleline).Groups[1].Value.Trim().Replace(",", ""));
                        regexHelpper = Regex.Match(subjectPropertyTable, "Built As</font>(.*?)t></td>", RegexOptions.Singleline).Groups[1].Value.Trim();
                        property.BuiltAs = WebUtility.HtmlDecode(Regex.Match(regexHelpper, "<font size=\\\"2\\\">(.*?)</fon", RegexOptions.Singleline).Groups[1].Value.Trim());
                        regexHelpper = Regex.Match(subjectPropertyTable, "Bedrooms</font>(.*?)t></td>", RegexOptions.Singleline).Groups[1].Value.Trim();
                        property.Beds = Convert.ToInt32(Regex.Match(regexHelpper, "<font size=\\\"2\\\">(.*?)</fon", RegexOptions.Singleline).Groups[1].Value.Trim());
                        regexHelpper = Regex.Match(subjectPropertyTable, "Baths</font>(.*?)t></td>", RegexOptions.Singleline).Groups[1].Value.Trim();
                        property.Baths = Convert.ToDouble(Regex.Match(regexHelpper, "<font size=\\\"2\\\">(.*?)</fon", RegexOptions.Singleline).Groups[1].Value.Trim());
                        regexHelpper = Regex.Match(subjectPropertyTable, "Exterior</font>(.*?)t></td>", RegexOptions.Singleline).Groups[1].Value.Trim();
                        property.Exterior = Regex.Match(regexHelpper, "<font size=\\\"2\\\">(.*?)</fon", RegexOptions.Singleline).Groups[1].Value.Trim();
                        property.Save();
                        if (!PropertyList.PropertyListExists(property.AccountNumber, 2))
                            PropertyList.AddPropertyToList(property.AccountNumber, 2, 0);
                        
                        // Get comp property chunk of page.
                        string comparePropertyGroup = Regex.Match(file, "width=\\\"703\\\" colspan=\\\"5(.*?)</body>", RegexOptions.Singleline).Groups[1].Value.Trim();
                        // scrape comparable properties.
                        PopulateComparableProps(accountNumber, comparePropertyGroup, property.Type);
                    }
                }
            }
        }

        /// <summary>
        /// Sets the last sale date and sale price of a property and returns the url for comps which is used to populate the remainder of the data.
        /// </summary>
        private static string PopulatePropertySalesInfoAndGetCompsURL(Property property, string file)
        {
            //set property type (residential or commercial)
            property.Type = Regex.Match(file, "Type:</font></b><font size=\\\"2\\\" color=\\\"#FF0000\\\">(.*?)</font", RegexOptions.Singleline).Groups[1].Value.Trim();
            //get list of sales dates
            string salesDocsDataSet = Regex.Match(file, ">Sales Documents/Deed History(.*?)>Non Sales Documents/Deed History", RegexOptions.Singleline).Groups[1].Value.Trim();
            MatchCollection saleDates = Regex.Matches(salesDocsDataSet, "&nbsp;</font><font size=\\\"2\\\">(.*?)</font></td>", RegexOptions.Singleline);
            property.LastSaleDate = saleDates.Count > 0 ? saleDates[0].Groups[1].Value.Trim() : null;
            //get list of sales prices
            MatchCollection salePrices = Regex.Matches(salesDocsDataSet, "<p align=\\\"right\\\"><font size=\\\"2\\\">(.*?)</font></td>", RegexOptions.Singleline);
            string topSalePrice = salePrices.Count > 0 ? salePrices[0].Groups[1].Value.Replace("\r\n\r\n", "").Replace("$", "").Replace(",", "") : null;
            property.LastSalePrice = Convert.ToDecimal(!string.IsNullOrWhiteSpace(topSalePrice) ? topSalePrice : "0");

            MatchCollection pics = Regex.Matches(file, "href=\\\"sketches/picfile/(.*?).jpg", RegexOptions.Singleline);
            foreach (Match pic in pics)
            {
                if(PicFileExists("http://www.oklahomacounty.org/assessor/Searches/sketches/picfile/" + pic.Groups[1].Value + ".jpg"))
                {
                    property.ImgPath = "http://www.oklahomacounty.org/assessor/Searches/sketches/picfile/" + pic.Groups[1].Value + ".jpg";
                    break;
                }
            }

            string urlSimilarPropertiesSubSet = Regex.Match(file, "name=\\\"loc2\\\"(.*?)Click for sales of similar properties", RegexOptions.Singleline).Groups[1].Value.Trim();
            return urlSimilarPropertiesSubSet;
        }

        private static bool PicFileExists(string url)
        {
            try
            {
                //Creating the HttpWebRequest
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                //Setting the Request method HEAD, you can also use GET too.
                request.Method = "HEAD";
                //Getting the Web Response.
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                //Returns TURE if the Status code == 200
                return (response.StatusCode == HttpStatusCode.OK);
            }
            catch
            {
                //Any exception will returns false.
                return false;
            }
        }

        /// <summary>
        /// Creates or updates property entries for each comp property.
        /// </summary>
        private static void PopulateComparableProps(string accountNumber, string comparePropertyGroup, string type)
        {
            MatchCollection comparePropertyTable = Regex.Matches(comparePropertyGroup, "nt #</font>(.*?)>Accou", RegexOptions.Singleline);
            foreach (Match cp in comparePropertyTable)
            {
                // need to get the account number here so we add new prop records.
                string regexHelpper = Regex.Match(cp.Groups[1].Value.Trim(), "href=\\\"AN-R.asp(.*?)#FF0000", RegexOptions.Singleline).Groups[1].Value.Trim();
                string cAccountNumber = Regex.Match(regexHelpper, "ACCOUNTNO=(.*?)\\\">", RegexOptions.Singleline).Groups[1].Value.Trim();
                Property property = new Property(cAccountNumber);
                if (property.DateModified.AddDays(30) < DateTime.Now)
                {
                    if (string.IsNullOrEmpty(property.AccountNumber))
                        property.AccountNumber = cAccountNumber;
                    property.Address = Regex.Match(regexHelpper, "<p align=\"center\"><font size=\"2\">(.*?)</font></td>", RegexOptions.Singleline).Groups[1].Value.Trim();
                    property.LastSaleDate = Regex.Match(regexHelpper, "<font size=\"2\">(.*?)</font>", RegexOptions.Singleline).Groups[1].Value.Trim();
                    property.LastSalePrice = Convert.ToDecimal(Regex.Match(cp.Groups[1].Value.Trim(), "color=\"#FF0000\">(.*?)</font>", RegexOptions.Singleline).Groups[1].Value.Trim().Substring(1));
                    regexHelpper = Regex.Match(cp.Groups[1].Value.Trim(), "Built</font>(.*?)t></td>", RegexOptions.Singleline).Groups[1].Value.Trim();
                    property.YearBuilt = Convert.ToInt32(Regex.Match(regexHelpper, "<font size=\\\"2\\\">(.*?)</fon", RegexOptions.Singleline).Groups[1].Value.Trim());
                    regexHelpper = Regex.Match(cp.Groups[1].Value.Trim(), "Square Ft.</font>(.*?)t></td>", RegexOptions.Singleline).Groups[1].Value.Trim();
                    string sqft = Regex.Match(regexHelpper, "<font size=\\\"2\\\">(.*?)</fon", RegexOptions.Singleline).Groups[1].Value.Trim().Replace(",", "");
                    property.Sqft = Convert.ToInt32(!string.IsNullOrWhiteSpace(sqft) ? sqft : "0");
                    regexHelpper = Regex.Match(cp.Groups[1].Value.Trim(), "Built As</font>(.*?)t></td>", RegexOptions.Singleline).Groups[1].Value.Trim();
                    property.BuiltAs = WebUtility.HtmlDecode(Regex.Match(regexHelpper, "<font size=\\\"2\\\">(.*?)</fon", RegexOptions.Singleline).Groups[1].Value.Trim());
                    regexHelpper = Regex.Match(cp.Groups[1].Value.Trim(), "Bedrooms</font>(.*?)t></td>", RegexOptions.Singleline).Groups[1].Value.Trim();
                    property.Beds = Convert.ToInt32(Regex.Match(regexHelpper, "<font size=\\\"2\\\">(.*?)</fon", RegexOptions.Singleline).Groups[1].Value.Trim());
                    regexHelpper = Regex.Match(cp.Groups[1].Value.Trim(), "Baths</font>(.*?)t></td>", RegexOptions.Singleline).Groups[1].Value.Trim();
                    property.Baths = Convert.ToDouble(Regex.Match(regexHelpper, "<font size=\\\"2\\\">(.*?)</fon", RegexOptions.Singleline).Groups[1].Value.Trim());
                    // TODO  Fix regex for exterior.  curently targeting baths.
                    regexHelpper = Regex.Match(cp.Groups[1].Value.Trim(), "Exterior</font>(.*?)t></td>", RegexOptions.Singleline).Groups[1].Value.Trim();
                    property.Exterior = Regex.Match(regexHelpper, "<font size=\\\"2\\\">(.*?)</fon", RegexOptions.Singleline).Groups[1].Value.Trim();
                    regexHelpper = Regex.Match(cp.Groups[1].Value.Trim(), "Garage</font>(.*?)t></td>", RegexOptions.Singleline).Groups[1].Value.Trim();
                    string garageSize = Regex.Match(regexHelpper, "<font size=\\\"2\\\">(.*?)</fon", RegexOptions.Singleline).Groups[1].Value.Trim();
                    int GARAGESIZE = 0;
                    Int32.TryParse(garageSize, out GARAGESIZE);
                    property.GarageSize = GARAGESIZE;
                    property.Type = type;
                    property.Save();
                    if(!PropertyCompDB.PropertyCompExists(accountNumber, cAccountNumber))
                        PropertyCompDB.InsertPropertyComp(accountNumber, cAccountNumber);
                }
            }
            
        }

        public static List<string> GetSheriffSaleDates(string file)
        {
            string dataSubSet;
            List<string> dates = new List<string>();


            if (!string.IsNullOrWhiteSpace(file))
            {
                dataSubSet = Regex.Match(file, "<form name=\"SheriffSale\"(.*?)</form>", RegexOptions.Singleline).Groups[1].Value.Trim();
                MatchCollection dateMatches = Regex.Matches(dataSubSet, "<option value=\"(.*?)\">", RegexOptions.Singleline);
                foreach (Match dt in dateMatches)
                {
                    dates.Add(dt.Groups[1].Value);
                }
            }
            else
                dates.Add("04/26/2012");
                
            return dates;
        }
    }
}