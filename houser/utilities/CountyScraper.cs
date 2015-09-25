using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using houser.objects;

namespace houser.utilities
{
    public class CountyScraper
    {
        // Scrapes the salerecord data and passes it to the property account scraper.
        public static SaleRecord ScrapeSaleRecord(string file, string saleDate)
        {

            DateTime _saleDate = Convert.ToDateTime(saleDate.Replace("%2f", " "));

            SaleRecord saleRecord = new SaleRecord(_saleDate);
            
            MatchCollection propertyListing = Regex.Matches(file, "<table cellpadding=\"0\" cellspacing=\"1\">\r\n\t(.*?)</table", RegexOptions.Singleline);

            foreach (Match pl in propertyListing)
            {
                SaleRecordItem saleRecordItem = new SaleRecordItem();

                string regexhelper = Regex.Matches(pl.Groups[1].Value, "Case Number(.*?)</font>\r\n\t\t", RegexOptions.Singleline)[0].Groups[1].Value;
                saleRecordItem.CaseNumber = Regex.Matches(regexhelper, "<td><font class=\"featureFont\">\r\n(.*?)\r\n", RegexOptions.Singleline)[0].Groups[1].Value.Trim();
                MatchCollection PropertyRow = Regex.Matches(pl.Groups[1].Value, "<tr valign=\"top\"*>(.*?)</tr>", RegexOptions.Singleline);
                saleRecordItem.Address = Regex.Matches(PropertyRow[2].Groups[1].Value, "<td><font class=\"featureFont\">\r\n(.*?)\r\n", RegexOptions.Singleline)[0].Groups[1].Value.Trim();
                saleRecordItem.AccountNumber = Regex.Matches(pl.Groups[1].Value, "ACCOUNTNO=(.*?)\"", RegexOptions.Singleline)[0].Groups[1].Value;
                string salePriceS = Regex.Matches(PropertyRow[5].Groups[1].Value, "<td><font class=\"featureFont\">\r\n(.*?)\r\n", RegexOptions.Singleline)[0].Groups[1].Value;
                double salePrice = 0;
                double.TryParse(salePriceS, out salePrice);
                saleRecordItem.SalePrice = salePrice;
                saleRecordItem.SaleDate = Convert.ToDateTime(saleDate.Replace("%2f", " "));
                saleRecord.Add(saleRecordItem);
            }
            return saleRecord;
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