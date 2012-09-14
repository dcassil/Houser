using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using houser.Business;
using houser.utilities;
using System.Text.RegularExpressions;

namespace houser
{
    public partial class _Default : System.Web.UI.Page
    {
        #region Private Variables.
        public int userID = 0;
        public string notification = "";
        private string userName;
        private string password;
        private bool logedIn;
        private User user;
        private bool showNotification = false;
        #endregion

        #region UI events
        public void Page_Load(object sender, EventArgs e)
        {
            CheckLoginCookie();
            notification = "";
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

                // will need to be database call when list are dynamic.
                ListItem listItem = new ListItem();
                listItem.Text = "All";
                listItem.Value = "2";
                listItem.Selected = true;
                ddlList.Items.Add(listItem);
                ListItem listItem2 = new ListItem();
                listItem2.Text = "Review";
                listItem2.Value = "1";
                ddlList.Items.Add(listItem2);
                chkNonLive.Checked = true;
                notification = logedIn ? "Welcome back:  " + user.FirstName + " " + user.LastName : "You must log in to use this system";
            }
        }

        // populate properties.
        protected void btnPopulateData_Click(object sender, EventArgs e)
        {
            PopulateData();
        }
        
        // If login cookie does not exist and the user and password are verified then create the login cookie.
        protected void btnSubmitLogin_Click(object sender, EventArgs e)
        {
            if (btnSubmitLogin.Text.Trim() == "Login")
            {
                user = new User(txtUserName.Text.Trim(), txtPassword.Text.Trim());
                if (user.UserID > 0)
                {
                    userID = user.UserID;
                    logedIn = true;
                    if (Request.Cookies["HouserLogin"] == null)
                    {
                        CreateLoginCookie();
                    }
                    btnSubmitLogin.Text = "Log out";
                    txtUserName.Enabled = false;
                    txtPassword.Enabled = false;
                    notification = "You are logged in as: " + user.FirstName;
                }
                else
                {
                    notification = "The user / password does not match our records";
                }
            }
            else
            {
                logedIn = false;
                HttpCookie loginCookie = new HttpCookie("HouserLogin");
                loginCookie.Expires = DateTime.UtcNow.AddDays(-1);
                HttpContext.Current.Response.Cookies.Set(loginCookie);
                btnSubmitLogin.Text = "Login";
                txtUserName.Enabled = true;
                txtPassword.Enabled = true;
                notification = "Peace out";
            }
        }
        
        // needs to be replaced with jquery sort.
        protected void btnSortAddress_Click(object sender, EventArgs e)
        {
            BuildSheriffSalePropertyList("Address");
        }
        // needs to be replaced with jquery sort.
        protected void btnSortMinBid_Click(object sender, EventArgs e)
        {
            BuildSheriffSalePropertyList("SalePrice");
        }
        // needs to be replaced with jquery sort.
        protected void btnSortSQFT_Click(object sender, EventArgs e)
        {
            BuildSheriffSalePropertyList("Sqft");
        }
        // needs to be replaced with jquery sort.
        protected void btnSortBeds_Click(object sender, EventArgs e)
        {
            BuildSheriffSalePropertyList("Beds");
        }
        // needs to be replaced with jquery sort.
        protected void btnSortBaths_Click(object sender, EventArgs e)
        {
            BuildSheriffSalePropertyList("Baths");
        }

        // gets properties by list when selected list cahnges.
        protected void ddlList_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateData();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Creates the actual markup for the properties.
        /// </summary>
        private void BuildListingPanels(DateTime date, string orderBy)
        {
            //BindingFlags b = BindingFlags.Instance | BindingFlags.Public;
            DataTable subjectProperties = SaleRecord.GetSaleProperitesByDate(date, orderBy, ddlList.SelectedValue, user.UserID);
            string listingPnlClass;
            string hasNoteClass = "";
            string inReviewList;
            string addRemoveList;
            StringBuilder html = new StringBuilder();
            foreach (DataRow property in subjectProperties.Rows)
            {

                bool isInReviewList = false;
                bool.TryParse(property["InReviewList"].ToString(), out isInReviewList);
                // if the property is in the review list then add the class.
                if (isInReviewList)
                {
                    inReviewList = "inReviewList";
                    addRemoveList = "Remove from list";
                }
                else
                {
                    inReviewList = "";
                    addRemoveList = "Add to review list";
                }
                if (!string.IsNullOrEmpty(property["Note"].ToString()))
                    hasNoteClass = "hasNote";
                else
                    hasNoteClass = "";

                html.Clear();
                
                listingPnlClass = "listingPanel";

                
                string _accountNumber = property["AccountNumber"].ToString();
                string _address = property["Address"].ToString();
                string indicatorClass = Regex.IsMatch(_address, "RECALLED") ? "indicatorRed" : "indicatorGreen";
                string caseNumber = property["CaseNumber"].ToString();
                string caseURL = "http://www.oscn.net/applications/oscn/getcaseinformation.asp?query=true&srch=0&web=true&db=Oklahoma&number=" + caseNumber + "&iLAST=&iFIRST=&iMIDDLE=&iID=&iDOBL=&iDOBH=&SearchType=0&iDCPT=&iapcasetype=All&idccasetype=All&iDATEL=&iDATEH=&iCLOSEDL=&iCLOSEDH=&iDCType=0&iYear=&iNumber=&icitation=&submitted=true";
                int _salePrice = -1;
                Int32.TryParse(property["SalePrice"].ToString(), out _salePrice);
                int _sqft = -1;
                Int32.TryParse(property["Sqft"].ToString(), out _sqft);
                int _beds = -1;
                Int32.TryParse(property["Beds"].ToString(), out _beds);
                double _baths = -1;
                double.TryParse(property["baths"].ToString(), out _baths);


                html.Append("<div class=\"listingWrapper\">");
                html.Append("<div class=\"" + indicatorClass + "\"></div>");
                html.Append("<div id=\"" + _accountNumber + "\" class=\"" + listingPnlClass + "\">");
                html.Append("<span class=\"propertyData\">");
                html.Append("<div class=\"notes " + hasNoteClass + " \" id=\"" + _accountNumber + "\" > </div>");
                html.Append("<span class=\"address\">" + _address.Substring(0, _address.Length > 40 ? 40 : _address.Length) + "</span>");
                html.Append("<span class=\"minBidWrapper\">$" + Convert.ToString(_salePrice * .66) + "</span>");
                html.Append("<span class=\"sqft\">" + Convert.ToString(_sqft) + "</span>");
                html.Append("<span class=\"beds\">" + Convert.ToString(_beds) + "</span>");
                html.Append("<span class=\"baths\">" + Convert.ToString(_baths) + "</span>");
                html.Append("<span class=\"addToReview " + inReviewList + "\"></span>");
                html.Append("<span class=\"pricePerSqft\">$" + Convert.ToString(_salePrice / _sqft) + "</span>");
                html.Append("<span class=\"caseDocs\" onclick=\"window.open('" + caseURL + "\', 'case')\"></a></span>");
                html.Append("<input type=\"hidden\" class=\"propertyPhoto\" value=\"" + property["ImgPath"].ToString() + "\"/>");
                html.Append("</div>");
                html.Append("</div>");
            
                pnlListingPanel.Controls.Add(new LiteralControl(html.ToString()));
                // go here to see how to use scroll up down events http://api.jquery.com/scroll/  we will use it to make sure key down and wheel down move down one listing exactly.
            }
        }
        
        /// <summary>
        /// Builds the view of properties.
        /// </summary>
        private void BuildSheriffSalePropertyList(string orderBy)
        {
            if (logedIn)
            {
                if (!chkNonLive.Checked)
                {
                    string saleDate = ddlSaleDate.SelectedItem.Value.Replace("/", "%2f");
                    string finishedLoading = GetCompletePropertyList(saleDate, chkNonLive.Checked);
                }

                BuildListingPanels(Convert.ToDateTime(ddlSaleDate.SelectedItem.Value), orderBy);
            }
        }

        /// <summary>
        /// Check to see if there is a login cookie and if it is valid.
        /// </summary>
        private void CheckLoginCookie()
        {
            if (Request.Cookies["HouserLogin"] != null)
            {
                userName = Request.Cookies["HouserLogin"]["UserName"];
                password = Request.Cookies["HouserLogin"]["Password"];
                user = new User(userName, password);
                if (user.UserID != 0)
                {
                    userID = user.UserID;
                    logedIn = true;
                    txtUserName.Enabled = false;
                    txtUserName.Text = user.UserName;
                    txtPassword.Enabled = false;
                    btnSubmitLogin.Text = "Log out";
                }
            }
        }

        /// <summary>
        /// Create the session cookie.
        /// </summary>
        private void CreateLoginCookie()
        {
            HttpCookie loginCookie = new HttpCookie("HouserLogin");
            loginCookie.Values.Add("UserName", txtUserName.Text.Trim());
            loginCookie.Values.Add("Password", txtPassword.Text.Trim());
            loginCookie.Expires = DateTime.Now.AddYears(2);
            Response.Cookies.Add(loginCookie);
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
        
        //----The use of multiple strings to fill in for the order by on the query needs to go.
        //  Switch this to jquery sorts... there is no reason to get the data more than once.
        // which will make this method pointless.  it may already be pointless.
        private void PopulateData()
        {
            BuildSheriffSalePropertyList("[Address]");
        }
        #endregion





    }
}