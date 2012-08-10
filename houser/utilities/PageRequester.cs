using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;

namespace houser.utilities
{
    public class PageRequester
    {
        /// <summary>
        /// Request the webpage as a string.
        /// </summary>
        public static string GetWebRequest(string url)
        {
            string strResults = "";
            WebResponse objResponse;
            // request a url.
            try
            {
                WebRequest objRequest = System.Net.HttpWebRequest.Create(url);
                // get the data from our url
                objResponse = objRequest.GetResponse();


                using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
                {
                    // create a string of the entire page we just requested.
                    strResults = sr.ReadToEnd();
                    sr.Close();
                    return strResults;
                }
            }
            catch { return ""; }
        }
    }
}