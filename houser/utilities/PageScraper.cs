using System;
using System.Collections.Generic;
using System.Web;
using System.Text.RegularExpressions;
using System.Data;
using houser.Data;


namespace houser.utilities
{
    // look at this http://www.dotnetperls.com/scraping-html for some regex and to see what you were doing.
    public static class PageScraper
    {
        public static Dictionary<int, Dictionary<string, string>> Find(string file, string saleDate)
        {
            Dictionary<int, Dictionary<string, string>> propertyGroup = new Dictionary<int, Dictionary<string, string>>();
            Dictionary<string, string> field = new Dictionary<string, string>();
            MatchCollection propertyListing = Regex.Matches(file, "<table cellpadding=\"0\" cellspacing=\"1\">\r\n\t(.*?)</table", RegexOptions.Singleline);
            int propertyID = 0;
            foreach (Match pl in propertyListing)
            {
                string property = pl.Groups[1].Value;
                List<string> fieldTrackerC1 = new List<string>();
                List<string> fieldTrackerC2 = new List<string>();
                MatchCollection PropertyRow = Regex.Matches(property, "<tr valign=\"top\"*>(.*?)</tr>",
                RegexOptions.Singleline);
                int fieldID = 0;
                foreach (Match m in PropertyRow)
                {
                    string propertyItem = m.Groups[1].Value.Trim();
                    fieldID++;
                    MatchCollection propertyItemKey = Regex.Matches(propertyItem, "<td><strong><font class=\"featureFont\">(.*?)</font>", RegexOptions.Singleline);
                    MatchCollection propertyItemValue = Regex.Matches(propertyItem, "<td><font class=\"featureFont\">\r\n(.*?)\r\n", RegexOptions.Singleline);
                    MatchCollection propertyRlink = Regex.Matches(propertyItem, "<td colspan=\"2\"><strong><font class=\"featureFont\"><a href=\"(.*?)\" target=\"_blank\">", RegexOptions.Singleline);
                    foreach (Match pf in propertyItemKey)
                    {
                        fieldTrackerC1.Add(pf.Groups[1].Value.Trim());
                    }
                    foreach (Match pf in propertyItemValue)
                    {
                        fieldTrackerC2.Add(pf.Groups[1].Value.Trim());
                    }
                    foreach (Match pf in propertyRlink)
                    {
                        field.Add(fieldID.ToString(), pf.Groups[1].Value.Trim());
                    }
                }

                foreach (var ft in fieldTrackerC1)
                {
                    int fieldIndex = fieldTrackerC1.IndexOf(ft);
                    field.Add(fieldTrackerC1[fieldIndex], fieldTrackerC2[fieldIndex]);
                }
                propertyGroup.Add(propertyID, new Dictionary<string, string>(field));
                // field contains the bulk of the data we need.  first time through we have the subject prop, aftert that we have the comps.
                DateTime? lastUpdate = SheriffSaleProperty.AccountNumberAlreadyInTable(field["8"].Substring(67));
                if (lastUpdate == null) // new record
                    SheriffSaleProperty.InsertProperty(field, saleDate);
                else if (lastUpdate != Convert.ToDateTime(Regex.Replace(saleDate, "%2f", "/")))
                    SheriffSaleProperty.UpdateProperty(field["8"].Substring(67), "", "", saleDate.ToString(), "");
                // need to insert into prop account table address and account number.  insert only.
                if (!Properties.AccountSeedExist(field["8"].Substring(67)))
                    Properties.InsertProperty(field["8"].Substring(67), field["Address"], "", "", "", "", "", "", "", "", "", "");

                propertyID++;
                field.Clear();
            }
            return propertyGroup;
        }

        public static Dictionary<string, string> GetPropertyData(string file, string accountNumber)
        {
            
            Dictionary<string, string> propertyData = new Dictionary<string, string>();
            if (file != "Error")
            {
                propertyData.Add("Type", Regex.Match(file, "Type:</font></b><font size=\\\"2\\\" color=\\\"#FF0000\\\">(.*?)</font", RegexOptions.Singleline).Groups[1].Value.Trim());
                string salesDocsDataSet = Regex.Match(file, ">Sales Documents/Deed History(.*?)>Non Sales Documents/Deed History", RegexOptions.Singleline).Groups[1].Value.Trim();
                MatchCollection saleDates = Regex.Matches(salesDocsDataSet, "&nbsp;</font><font size=\\\"2\\\">(.*?)</font></td>", RegexOptions.Singleline);
                propertyData.Add("saleDate", saleDates.Count > 0 ? saleDates[0].Groups[1].Value.Trim() : "No Sale Date Available");
                
                MatchCollection salePrices = Regex.Matches(salesDocsDataSet, "<p align=\\\"right\\\"><font size=\\\"2\\\">(.*?)</font></td>", RegexOptions.Singleline);
                propertyData.Add("salePrice", salePrices.Count > 0 ? salePrices[0].Groups[1].Value.Trim() : "No Sale Price Available");
                
                string urlSimilarPropertiesSubSet = Regex.Match(file, "name=\\\"loc2\\\"(.*?)Click for sales of similar properties", RegexOptions.Singleline).Groups[1].Value.Trim();
                propertyData.Add("SimilarPropURL", Regex.Match(urlSimilarPropertiesSubSet, "'(.*?)'", RegexOptions.Singleline).Groups[1].Value.Trim());
            }
            else
                propertyData.Add("Error", "Error");
            return propertyData;
        }

        public static List<string> GetSheriffSaleDates(string file)
        {
            string dataSubSet = Regex.Match(file, "<form name=\"SheriffSale\"(.*?)</form>", RegexOptions.Singleline).Groups[1].Value.Trim();
            MatchCollection dateMatches = Regex.Matches(dataSubSet, "<option value=\"(.*?)\">", RegexOptions.Singleline);
            List<string> dates = new List<string>();
            foreach (Match dt in dateMatches)
            {
                dates.Add(dt.Groups[1].Value);
            }
            
            return dates;
        }

        internal static Dictionary<int, Dictionary<string,string>> GetSimilarData(string file, string accountNumber, Dictionary<string, string> scrapedData)
        {
            Dictionary<int, Dictionary<string, string>> propertyData = new Dictionary<int, Dictionary<string, string>>();
            Dictionary<string, string> subjectPropertyFields = new Dictionary<string, string>();
            if (file != "Error")
            {
                string subjectPropertyTableSubSet = Regex.Match(file, "Property Information</font>(.*?)>Sales are pulled", RegexOptions.Singleline).Groups[1].Value.Trim();
                string subjectPropertyTable = Regex.Match(subjectPropertyTableSubSet, "<tbody>(.*?)</tbody>", RegexOptions.Singleline).Groups[1].Value.Trim();
                string regexHelpper = Regex.Match(subjectPropertyTable, "Built</font>(.*?)t></td>", RegexOptions.Singleline).Groups[1].Value.Trim();
                string yearBuilt = Regex.Match(regexHelpper, "<font size=\\\"2\\\" color=\\\"#0000FF\\\">(.*?)</fon", RegexOptions.Singleline).Groups[1].Value.Trim();
                //regexHelpper = Regex.Match(subjectPropertyTable, "bgcolor=\\\"#FFFFCC\\\">(.*?)t></td>", RegexOptions.Singleline).Groups[1].Value.Trim();
                //string address = Regex.Match(regexHelpper, "<font size=\\\"2\\\" color=\\\"#0000FF\\\">(.*?)</fon", RegexOptions.Singleline).Groups[1].Value.Trim();
                regexHelpper = Regex.Match(subjectPropertyTable, "Total SQFT</font>(.*?)t></td>", RegexOptions.Singleline).Groups[1].Value.Trim();
                string sqft = Regex.Match(regexHelpper, "<font size=\\\"2\\\">(.*?)</fon", RegexOptions.Singleline).Groups[1].Value.Trim();
                regexHelpper = Regex.Match(subjectPropertyTable, "Built As</font>(.*?)t></td>", RegexOptions.Singleline).Groups[1].Value.Trim();
                string builtAs = Regex.Match(regexHelpper, "<font size=\\\"2\\\">(.*?)</fon", RegexOptions.Singleline).Groups[1].Value.Trim();
                regexHelpper = Regex.Match(subjectPropertyTable, "Bedrooms</font>(.*?)t></td>", RegexOptions.Singleline).Groups[1].Value.Trim();
                string bedrooms = Regex.Match(regexHelpper, "<font size=\\\"2\\\">(.*?)</fon", RegexOptions.Singleline).Groups[1].Value.Trim();
                regexHelpper = Regex.Match(subjectPropertyTable, "Baths</font>(.*?)t></td>", RegexOptions.Singleline).Groups[1].Value.Trim();
                string bathrooms = Regex.Match(regexHelpper, "<font size=\\\"2\\\">(.*?)</fon", RegexOptions.Singleline).Groups[1].Value.Trim();
                regexHelpper = Regex.Match(subjectPropertyTable, "Exterior</font>(.*?)t></td>", RegexOptions.Singleline).Groups[1].Value.Trim();
                string exterior = Regex.Match(regexHelpper, "<font size=\\\"2\\\">(.*?)</fon", RegexOptions.Singleline).Groups[1].Value.Trim();
                string comparePropertyGroup = Regex.Match(file, "width=\\\"703\\\" colspan=\\\"5(.*?)</body>", RegexOptions.Singleline).Groups[1].Value.Trim();
                propertyData.Add(0, subjectPropertyFields);
                string saleDate = scrapedData["saleDate"];
                string salePrice = scrapedData["salePrice"];
                if (!Properties.CompletePropAccountExist(accountNumber) && Properties.AccountSeedExist(accountNumber))
                    Properties.UpdateProperty(accountNumber, "", sqft, bedrooms, bathrooms, yearBuilt, "", exterior, saleDate, salePrice, "1", "1");

                MatchCollection comparePropertyTable = Regex.Matches(comparePropertyGroup, "nt #</font>(.*?)>Accou", RegexOptions.Singleline);
                int index = 1;
                Dictionary<string, string> comparePropertyFields = new Dictionary<string, string>();
                foreach (Match cp in comparePropertyTable)
                {
                    comparePropertyFields.Clear();
                    // need to get the account number here so we add new prop records.
                    regexHelpper = Regex.Match(cp.Groups[1].Value.Trim(), "href=\\\"AN-R.asp(.*?)#FF0000", RegexOptions.Singleline).Groups[1].Value.Trim();
                    string C_AccountNumber = Regex.Match(regexHelpper, "ACCOUNTNO=(.*?)\\\">", RegexOptions.Singleline).Groups[1].Value.Trim();
                    string C_Address=  Regex.Match(regexHelpper, "<p align=\"center\"><font size=\"2\">(.*?)</font></td>", RegexOptions.Singleline).Groups[1].Value.Trim();
                    string C_SaleDate=  Regex.Match(regexHelpper, "<font size=\"2\">(.*?)</font>", RegexOptions.Singleline).Groups[1].Value.Trim();
                    string C_SalePrice = Regex.Match(cp.Groups[1].Value.Trim(), "color=\"#FF0000\">(.*?)</font>", RegexOptions.Singleline).Groups[1].Value.Trim().Substring(1);
                    regexHelpper = Regex.Match(cp.Groups[1].Value.Trim(), "Built</font>(.*?)t></td>", RegexOptions.Singleline).Groups[1].Value.Trim();
                    string C_YearBuilt = Regex.Match(regexHelpper, "<font size=\\\"2\\\">(.*?)</fon", RegexOptions.Singleline).Groups[1].Value.Trim();
                    regexHelpper = Regex.Match(cp.Groups[1].Value.Trim(), "Square Ft.</font>(.*?)t></td>", RegexOptions.Singleline).Groups[1].Value.Trim();
                    string C_SQFT = Regex.Match(regexHelpper, "<font size=\\\"2\\\">(.*?)</fon", RegexOptions.Singleline).Groups[1].Value.Trim();
                    regexHelpper = Regex.Match(cp.Groups[1].Value.Trim(), "Built As</font>(.*?)t></td>", RegexOptions.Singleline).Groups[1].Value.Trim();
                    string C_BuiltAs = Regex.Match(regexHelpper, "<font size=\\\"2\\\">(.*?)</fon", RegexOptions.Singleline).Groups[1].Value.Trim();
                    regexHelpper = Regex.Match(cp.Groups[1].Value.Trim(), "Bedrooms</font>(.*?)t></td>", RegexOptions.Singleline).Groups[1].Value.Trim();
                    string C_Bedrooms = Regex.Match(regexHelpper, "<font size=\\\"2\\\">(.*?)</fon", RegexOptions.Singleline).Groups[1].Value.Trim();
                    regexHelpper = Regex.Match(cp.Groups[1].Value.Trim(), "Baths</font>(.*?)t></td>", RegexOptions.Singleline).Groups[1].Value.Trim();
                    string C_Bathrooms = Regex.Match(regexHelpper, "<font size=\\\"2\\\">(.*?)</fon", RegexOptions.Singleline).Groups[1].Value.Trim();
                    // TODO  Fix regex for exterior.  curently targeting baths.
                    regexHelpper = Regex.Match(cp.Groups[1].Value.Trim(), "Baths</font>(.*?)t></td>", RegexOptions.Singleline).Groups[1].Value.Trim();
                    string C_Exterior = Regex.Match(regexHelpper, "<font size=\\\"2\\\">(.*?)</fon", RegexOptions.Singleline).Groups[1].Value.Trim();
                    regexHelpper = Regex.Match(cp.Groups[1].Value.Trim(), "Garage</font>(.*?)t></td>", RegexOptions.Singleline).Groups[1].Value.Trim();
                    string C_Garage = Regex.Match(regexHelpper, "<font size=\\\"2\\\">(.*?)</fon", RegexOptions.Singleline).Groups[1].Value.Trim();
                    propertyData.Add(index, new Dictionary<string, string>(comparePropertyFields));
                    
                    if (Properties.AccountSeedExist(C_AccountNumber))
                        Properties.UpdateProperty(C_AccountNumber, C_Address, C_SQFT, C_Bedrooms, C_Bathrooms, C_YearBuilt, C_Garage, C_Exterior, C_SaleDate, C_SalePrice, "1", "");
                    else
                        Properties.InsertProperty(C_AccountNumber, C_Address, C_SQFT, C_Bedrooms, C_Bathrooms, C_YearBuilt, C_Garage, C_Exterior, C_SaleDate, C_SalePrice, "1", "");

                    bool propCompExist = PropertyComps.PropertyCompExists(accountNumber, C_AccountNumber);
                    if (!propCompExist)
                        PropertyComps.InsertPropertyComp(accountNumber, C_AccountNumber);
                    index++;
                }
            }
            else
            {
                subjectPropertyFields.Add("Error", "Error");
                propertyData.Add(0, subjectPropertyFields);
            }
            

            
            return propertyData;
        }
    }
}