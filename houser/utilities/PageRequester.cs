using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using HtmlAgilityPack;

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


                using (StreamReader sr = new StreamReader(objResponse.GetResponseStream(), System.Text.Encoding.UTF8))
                {

                    var queryContent = sr.ReadToEnd();
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(queryContent);
                   
                    sr.Close();
                    return doc.DocumentNode.OuterHtml;
                }
            }
            catch { return ""; }
        }

        public static string HttpPost(string URI, string Parameters)
        {
            System.Net.WebRequest req = System.Net.WebRequest.Create(URI);
            //Add these, as we're doing a POST
            req.ContentType = "application/x-www-form-urlencoded";
            req.Method = "POST";
            //We need to count how many bytes we're sending. 
            //Post'ed Faked Forms should be name=value&
            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(Parameters);
            req.ContentLength = bytes.Length;
            System.IO.Stream os = req.GetRequestStream();
            os.Write(bytes, 0, bytes.Length); //Push it out there
            os.Close();
            System.Net.WebResponse resp = req.GetResponse();
            if (resp == null) return null;
            System.IO.StreamReader sr =
                  new System.IO.StreamReader(resp.GetResponseStream());
            return sr.ReadToEnd().Trim();
        }

    }
}