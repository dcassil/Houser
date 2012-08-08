using System;
using System.Collections.Generic;
using System.Web;
using System.Text.RegularExpressions;
using System.Data;
using houser.Data;
using houser.Business;


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
                SaleRecord saleRecord = new SaleRecord(accountnuber, _saleDate);
                
                MatchCollection PropertyRow = Regex.Matches(pl.Groups[1].Value, "<tr valign=\"top\"*>(.*?)</tr>", RegexOptions.Singleline);
                string address = Regex.Matches(PropertyRow[2].Groups[1].Value, "<td><font class=\"featureFont\">\r\n(.*?)\r\n", RegexOptions.Singleline)[0].Groups[1].Value;
                
                saleRecord.AccountNumber =  accountnuber;
                saleRecord.SalePrice = Convert.ToDouble(Regex.Matches(PropertyRow[5].Groups[1].Value, "<td><font class=\"featureFont\">\r\n(.*?)\r\n", RegexOptions.Singleline)[0].Groups[1].Value);
                saleRecord.SaleDate = Convert.ToDateTime(saleDate.Replace("%2f", " "));
                saleRecord.Save();

                Property property = new Property(accountnuber);
                
                if (property.IsNew)
                    GetPropertyDataFromWeb(accountnuber, property);
            }
        }


        private static void GetPropertyDataFromWeb(string accountNumber, Property property)
        {
            string url = "http://www.oklahomacounty.org/assessor/Searches/AN-R.asp?ACCOUNTNO=" + accountNumber;
            string file = PageRequester.GetWebRequest(url);

            property.AccountNumber = accountNumber;

            string urlSimilarPropertiesSubSet = PopulatePropertySalesInfoAndGetCompsURL(property, file);
            
            //request the comps page file
            string cUrl = Regex.Match(urlSimilarPropertiesSubSet, "'(.*?)'", RegexOptions.Singleline).Groups[1].Value.Trim();
            file = PageRequester.GetWebRequest(cUrl);

            //scope in the regex text are
            string subjectPropertyTableSubSet = Regex.Match(file, "Property Information</font>(.*?)>Sales are pulled", RegexOptions.Singleline).Groups[1].Value.Trim();
            string subjectPropertyTable = Regex.Match(subjectPropertyTableSubSet, "<tbody>(.*?)</tbody>", RegexOptions.Singleline).Groups[1].Value.Trim();
            string regexHelpper = Regex.Match(subjectPropertyTable, "Built</font>(.*?)t></td>", RegexOptions.Singleline).Groups[1].Value.Trim();
            property.YearBuilt = Convert.ToInt32(Regex.Match(regexHelpper, "<font size=\\\"2\\\" color=\\\"#0000FF\\\">(.*?)</fon", RegexOptions.Singleline).Groups[1].Value.Trim());
            regexHelpper = Regex.Match(subjectPropertyTable, "bgcolor=\\\"#FFFFCC\\\">(.*?)t></td>", RegexOptions.Singleline).Groups[1].Value.Trim();
            property.Address = Regex.Match(regexHelpper, "<font size=\\\"2\\\" color=\\\"#0000FF\\\">(.*?)</fon", RegexOptions.Singleline).Groups[1].Value.Trim();
            regexHelpper = Regex.Match(subjectPropertyTable, "Total SQFT</font>(.*?)t></td>", RegexOptions.Singleline).Groups[1].Value.Trim();
            property.Sqft = Convert.ToInt32(Regex.Match(regexHelpper, "<font size=\\\"2\\\">(.*?)</fon", RegexOptions.Singleline).Groups[1].Value.Trim().Replace(",", ""));
            regexHelpper = Regex.Match(subjectPropertyTable, "Built As</font>(.*?)t></td>", RegexOptions.Singleline).Groups[1].Value.Trim();
            property.BuiltAs = Regex.Match(regexHelpper, "<font size=\\\"2\\\">(.*?)</fon", RegexOptions.Singleline).Groups[1].Value.Trim();
            regexHelpper = Regex.Match(subjectPropertyTable, "Bedrooms</font>(.*?)t></td>", RegexOptions.Singleline).Groups[1].Value.Trim();
            property.Beds = Convert.ToInt32(Regex.Match(regexHelpper, "<font size=\\\"2\\\">(.*?)</fon", RegexOptions.Singleline).Groups[1].Value.Trim());
            regexHelpper = Regex.Match(subjectPropertyTable, "Baths</font>(.*?)t></td>", RegexOptions.Singleline).Groups[1].Value.Trim();
            property.Baths = Convert.ToDouble(Regex.Match(regexHelpper, "<font size=\\\"2\\\">(.*?)</fon", RegexOptions.Singleline).Groups[1].Value.Trim());
            regexHelpper = Regex.Match(subjectPropertyTable, "Exterior</font>(.*?)t></td>", RegexOptions.Singleline).Groups[1].Value.Trim();
            property.Exterior = Regex.Match(regexHelpper, "<font size=\\\"2\\\">(.*?)</fon", RegexOptions.Singleline).Groups[1].Value.Trim();
            string comparePropertyGroup = Regex.Match(file, "width=\\\"703\\\" colspan=\\\"5(.*?)</body>", RegexOptions.Singleline).Groups[1].Value.Trim();

            PopulateComparableProps(accountNumber, comparePropertyGroup);
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
            property.LastSaleDate = Convert.ToDateTime(saleDates.Count > 0 ? saleDates[0].Groups[1].Value.Trim() : "No Sale Date Available");
            //get list of sales prices
            MatchCollection salePrices = Regex.Matches(salesDocsDataSet, "<p align=\\\"right\\\"><font size=\\\"2\\\">(.*?)</font></td>", RegexOptions.Singleline);
            string topSalePrice = salePrices[0].Groups[1].Value.Replace("\r\n\r\n", "").Replace("$", "").Replace(",", "");
            property.LastSalePrice = Convert.ToDecimal(!string.IsNullOrWhiteSpace(topSalePrice) ? topSalePrice : "0");
            
            string urlSimilarPropertiesSubSet = Regex.Match(file, "name=\\\"loc2\\\"(.*?)Click for sales of similar properties", RegexOptions.Singleline).Groups[1].Value.Trim();
            return urlSimilarPropertiesSubSet;
        }

        /// <summary>
        /// Creates or updates property entries for each comp property.
        /// </summary>
        private static void PopulateComparableProps(string accountNumber, string comparePropertyGroup)
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
                    property.Address = Regex.Match(regexHelpper, "<p align=\"center\"><font size=\"2\">(.*?)</font></td>", RegexOptions.Singleline).Groups[1].Value.Trim();
                    property.LastSaleDate = Convert.ToDateTime(Regex.Match(regexHelpper, "<font size=\"2\">(.*?)</font>", RegexOptions.Singleline).Groups[1].Value.Trim());
                    property.LastSalePrice = Convert.ToDecimal(Regex.Match(cp.Groups[1].Value.Trim(), "color=\"#FF0000\">(.*?)</font>", RegexOptions.Singleline).Groups[1].Value.Trim().Substring(1));
                    regexHelpper = Regex.Match(cp.Groups[1].Value.Trim(), "Built</font>(.*?)t></td>", RegexOptions.Singleline).Groups[1].Value.Trim();
                    property.YearBuilt = Convert.ToInt32(Regex.Match(regexHelpper, "<font size=\\\"2\\\">(.*?)</fon", RegexOptions.Singleline).Groups[1].Value.Trim());
                    regexHelpper = Regex.Match(cp.Groups[1].Value.Trim(), "Square Ft.</font>(.*?)t></td>", RegexOptions.Singleline).Groups[1].Value.Trim();
                    property.Sqft = Convert.ToInt32(Regex.Match(regexHelpper, "<font size=\\\"2\\\">(.*?)</fon", RegexOptions.Singleline).Groups[1].Value.Trim().Replace(",",""));
                    regexHelpper = Regex.Match(cp.Groups[1].Value.Trim(), "Built As</font>(.*?)t></td>", RegexOptions.Singleline).Groups[1].Value.Trim();
                    property.BuiltAs = Regex.Match(regexHelpper, "<font size=\\\"2\\\">(.*?)</fon", RegexOptions.Singleline).Groups[1].Value.Trim();
                    regexHelpper = Regex.Match(cp.Groups[1].Value.Trim(), "Bedrooms</font>(.*?)t></td>", RegexOptions.Singleline).Groups[1].Value.Trim();
                    property.Beds = Convert.ToInt32(Regex.Match(regexHelpper, "<font size=\\\"2\\\">(.*?)</fon", RegexOptions.Singleline).Groups[1].Value.Trim());
                    regexHelpper = Regex.Match(cp.Groups[1].Value.Trim(), "Baths</font>(.*?)t></td>", RegexOptions.Singleline).Groups[1].Value.Trim();
                    property.Baths = Convert.ToDouble(Regex.Match(regexHelpper, "<font size=\\\"2\\\">(.*?)</fon", RegexOptions.Singleline).Groups[1].Value.Trim());
                    // TODO  Fix regex for exterior.  curently targeting baths.
                    regexHelpper = Regex.Match(cp.Groups[1].Value.Trim(), "Exterior</font>(.*?)t></td>", RegexOptions.Singleline).Groups[1].Value.Trim();
                    property.Exterior = Regex.Match(regexHelpper, "<font size=\\\"2\\\">(.*?)</fon", RegexOptions.Singleline).Groups[1].Value.Trim();
                    regexHelpper = Regex.Match(cp.Groups[1].Value.Trim(), "Garage</font>(.*?)t></td>", RegexOptions.Singleline).Groups[1].Value.Trim();
                    property.GarageSize = Convert.ToInt32(Regex.Match(regexHelpper, "<font size=\\\"2\\\">(.*?)</fon", RegexOptions.Singleline).Groups[1].Value.Trim());

                    // need to check and see if comp is attached to prop in db and if not attach it.
                }
            }
            
        }
//        public static Dictionary<string, string> GetPropertyData(string file, string accountNumber)
//        {
            
//            Dictionary<string, string> propertyData = new Dictionary<string, string>();
//            if (file != "Error")
//            {
//                propertyData.Add("Type", Regex.Match(file, "Type:</font></b><font size=\\\"2\\\" color=\\\"#FF0000\\\">(.*?)</font", RegexOptions.Singleline).Groups[1].Value.Trim());
//                string salesDocsDataSet = Regex.Match(file, ">Sales Documents/Deed History(.*?)>Non Sales Documents/Deed History", RegexOptions.Singleline).Groups[1].Value.Trim();
//                MatchCollection saleDates = Regex.Matches(salesDocsDataSet, "&nbsp;</font><font size=\\\"2\\\">(.*?)</font></td>", RegexOptions.Singleline);
//                propertyData.Add("saleDate", saleDates.Count > 0 ? saleDates[0].Groups[1].Value.Trim() : "No Sale Date Available");
                
//                MatchCollection salePrices = Regex.Matches(salesDocsDataSet, "<p align=\\\"right\\\"><font size=\\\"2\\\">(.*?)</font></td>", RegexOptions.Singleline);
//                propertyData.Add("salePrice", salePrices.Count > 0 ? salePrices[0].Groups[1].Value.Trim() : "No Sale Price Available");
                
//                string urlSimilarPropertiesSubSet = Regex.Match(file, "name=\\\"loc2\\\"(.*?)Click for sales of similar properties", RegexOptions.Singleline).Groups[1].Value.Trim();
//                propertyData.Add("SimilarPropURL", Regex.Match(urlSimilarPropertiesSubSet, "'(.*?)'", RegexOptions.Singleline).Groups[1].Value.Trim());
//            }
//            else
//                propertyData.Add("Error", "Error");
//            return propertyData;
//        }

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

//        internal static Dictionary<int, Dictionary<string,string>> GetSimilarData(string file, string accountNumber, Dictionary<string, string> scrapedData)
//        {
//            Property property = new Property();
//            property.Load(accountNumber);
            
//            Dictionary<int, Dictionary<string, string>> propertyData = new Dictionary<int, Dictionary<string, string>>();
//            Dictionary<string, string> subjectPropertyFields = new Dictionary<string, string>();
//            if (file != "Error")
//            {
//                property.AccountNumber = accountNumber;
//                string subjectPropertyTableSubSet = Regex.Match(file, "Property Information</font>(.*?)>Sales are pulled", RegexOptions.Singleline).Groups[1].Value.Trim();
//                string subjectPropertyTable = Regex.Match(subjectPropertyTableSubSet, "<tbody>(.*?)</tbody>", RegexOptions.Singleline).Groups[1].Value.Trim();
//                string regexHelpper = Regex.Match(subjectPropertyTable, "Built</font>(.*?)t></td>", RegexOptions.Singleline).Groups[1].Value.Trim();
//                property.YearBuilt = Convert.ToInt32(Regex.Match(regexHelpper, "<font size=\\\"2\\\" color=\\\"#0000FF\\\">(.*?)</fon", RegexOptions.Singleline).Groups[1].Value.Trim());
//                //regexHelpper = Regex.Match(subjectPropertyTable, "bgcolor=\\\"#FFFFCC\\\">(.*?)t></td>", RegexOptions.Singleline).Groups[1].Value.Trim();
//                //string address = Regex.Match(regexHelpper, "<font size=\\\"2\\\" color=\\\"#0000FF\\\">(.*?)</fon", RegexOptions.Singleline).Groups[1].Value.Trim();
//                regexHelpper = Regex.Match(subjectPropertyTable, "Total SQFT</font>(.*?)t></td>", RegexOptions.Singleline).Groups[1].Value.Trim();
//                property.SQFT = Convert.ToInt32(Regex.Match(regexHelpper, "<font size=\\\"2\\\">(.*?)</fon", RegexOptions.Singleline).Groups[1].Value.Trim().Replace(",",""));
//                regexHelpper = Regex.Match(subjectPropertyTable, "Built As</font>(.*?)t></td>", RegexOptions.Singleline).Groups[1].Value.Trim();
//                string builtAs = Regex.Match(regexHelpper, "<font size=\\\"2\\\">(.*?)</fon", RegexOptions.Singleline).Groups[1].Value.Trim();
//                regexHelpper = Regex.Match(subjectPropertyTable, "Bedrooms</font>(.*?)t></td>", RegexOptions.Singleline).Groups[1].Value.Trim();
//                property.Beds = Convert.ToInt32(Regex.Match(regexHelpper, "<font size=\\\"2\\\">(.*?)</fon", RegexOptions.Singleline).Groups[1].Value.Trim());
//                regexHelpper = Regex.Match(subjectPropertyTable, "Baths</font>(.*?)t></td>", RegexOptions.Singleline).Groups[1].Value.Trim();
//                property.Baths = Convert.ToDouble(Regex.Match(regexHelpper, "<font size=\\\"2\\\">(.*?)</fon", RegexOptions.Singleline).Groups[1].Value.Trim());
//                regexHelpper = Regex.Match(subjectPropertyTable, "Exterior</font>(.*?)t></td>", RegexOptions.Singleline).Groups[1].Value.Trim();
//                property.Exterior = Regex.Match(regexHelpper, "<font size=\\\"2\\\">(.*?)</fon", RegexOptions.Singleline).Groups[1].Value.Trim();
//                string comparePropertyGroup = Regex.Match(file, "width=\\\"703\\\" colspan=\\\"5(.*?)</body>", RegexOptions.Singleline).Groups[1].Value.Trim();
//                propertyData.Add(0, subjectPropertyFields);
//                //property.LastSaleDate = Convert.ToDateTime(scrapedData["saleDate"]);
//                //property.LastSalePrice = Convert.ToInt32(scrapedData["salePrice"]);


//                if (!PropAccounts.CompletePropAccountExist(accountNumber) && PropAccounts.AccountSeedExist(accountNumber))
//                {
//                    property.FullyLoaded = true;
//                    property.SubjectProperty = true;
//                    property.Save();
//                }
//                    //PropAccounts.UpdateProperty(accountNumber, "", sqft, bedrooms, bathrooms, yearBuilt, "", exterior, saleDate, salePrice, "1", "1");

//                MatchCollection comparePropertyTable = Regex.Matches(comparePropertyGroup, "nt #</font>(.*?)>Accou", RegexOptions.Singleline);
//                int index = 1;
//                Dictionary<string, string> comparePropertyFields = new Dictionary<string, string>();
//                foreach (Match cp in comparePropertyTable)
//                {
//                    comparePropertyFields.Clear();
//                    // need to get the account number here so we add new prop records.
//                    regexHelpper = Regex.Match(cp.Groups[1].Value.Trim(), "href=\\\"AN-R.asp(.*?)#FF0000", RegexOptions.Singleline).Groups[1].Value.Trim();
//                    string C_AccountNumber = Regex.Match(regexHelpper, "ACCOUNTNO=(.*?)\\\">", RegexOptions.Singleline).Groups[1].Value.Trim();
//                    string C_Address=  Regex.Match(regexHelpper, "<p align=\"center\"><font size=\"2\">(.*?)</font></td>", RegexOptions.Singleline).Groups[1].Value.Trim();
//                    string C_SaleDate=  Regex.Match(regexHelpper, "<font size=\"2\">(.*?)</font>", RegexOptions.Singleline).Groups[1].Value.Trim();
//                    string C_SalePrice = Regex.Match(cp.Groups[1].Value.Trim(), "color=\"#FF0000\">(.*?)</font>", RegexOptions.Singleline).Groups[1].Value.Trim().Substring(1);
//                    regexHelpper = Regex.Match(cp.Groups[1].Value.Trim(), "Built</font>(.*?)t></td>", RegexOptions.Singleline).Groups[1].Value.Trim();
//                    string C_YearBuilt = Regex.Match(regexHelpper, "<font size=\\\"2\\\">(.*?)</fon", RegexOptions.Singleline).Groups[1].Value.Trim();
//                    regexHelpper = Regex.Match(cp.Groups[1].Value.Trim(), "Square Ft.</font>(.*?)t></td>", RegexOptions.Singleline).Groups[1].Value.Trim();
//                    string C_SQFT = Regex.Match(regexHelpper, "<font size=\\\"2\\\">(.*?)</fon", RegexOptions.Singleline).Groups[1].Value.Trim();
//                    regexHelpper = Regex.Match(cp.Groups[1].Value.Trim(), "Built As</font>(.*?)t></td>", RegexOptions.Singleline).Groups[1].Value.Trim();
//                    string C_BuiltAs = Regex.Match(regexHelpper, "<font size=\\\"2\\\">(.*?)</fon", RegexOptions.Singleline).Groups[1].Value.Trim();
//                    regexHelpper = Regex.Match(cp.Groups[1].Value.Trim(), "Bedrooms</font>(.*?)t></td>", RegexOptions.Singleline).Groups[1].Value.Trim();
//                    string C_Bedrooms = Regex.Match(regexHelpper, "<font size=\\\"2\\\">(.*?)</fon", RegexOptions.Singleline).Groups[1].Value.Trim();
//                    regexHelpper = Regex.Match(cp.Groups[1].Value.Trim(), "Baths</font>(.*?)t></td>", RegexOptions.Singleline).Groups[1].Value.Trim();
//                    string C_Bathrooms = Regex.Match(regexHelpper, "<font size=\\\"2\\\">(.*?)</fon", RegexOptions.Singleline).Groups[1].Value.Trim();
//                    // TODO  Fix regex for exterior.  curently targeting baths.
//                    regexHelpper = Regex.Match(cp.Groups[1].Value.Trim(), "Baths</font>(.*?)t></td>", RegexOptions.Singleline).Groups[1].Value.Trim();
//                    string C_Exterior = Regex.Match(regexHelpper, "<font size=\\\"2\\\">(.*?)</fon", RegexOptions.Singleline).Groups[1].Value.Trim();
//                    regexHelpper = Regex.Match(cp.Groups[1].Value.Trim(), "Garage</font>(.*?)t></td>", RegexOptions.Singleline).Groups[1].Value.Trim();
//                    string C_Garage = Regex.Match(regexHelpper, "<font size=\\\"2\\\">(.*?)</fon", RegexOptions.Singleline).Groups[1].Value.Trim();
//                    propertyData.Add(index, new Dictionary<string, string>(comparePropertyFields));
                    
//                    if (PropAccounts.AccountSeedExist(C_AccountNumber))
//                        PropAccounts.UpdateProperty(C_AccountNumber, C_Address, C_SQFT, C_Bedrooms, C_Bathrooms, C_YearBuilt, C_Garage, C_Exterior, C_SaleDate, C_SalePrice, "1", "");
//                    else
//                        PropAccounts.InsertProperty(C_AccountNumber, C_Address, C_SQFT, C_Bedrooms, C_Bathrooms, C_YearBuilt, C_Garage, C_Exterior, C_SaleDate, C_SalePrice, "1", "");

//                    bool propCompExist = PropertyComps.PropertyCompExists(accountNumber, C_AccountNumber);
//                    if (!propCompExist)
//                        PropertyComps.InsertPropertyComp(accountNumber, C_AccountNumber);
//                    index++;
//                }
//            }
//            else
//            {
//                subjectPropertyFields.Add("Error", "Error");
//                propertyData.Add(0, subjectPropertyFields);
//            }
            

            
//            return propertyData;
//        }
    }
}