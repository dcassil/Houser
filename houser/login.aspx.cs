using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using houser.Business;
using houser.utilities;

namespace houser
{
    public partial class login : System.Web.UI.Page
    {
        private bool _logedIn;
        private User _user;
        public int _userID = 0;
        public string _notification;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsLogedIn())
            {
                RedirectToAlt();
            }
        }

        // If login cookie does not exist and the user and password are verified then create the login cookie.
        protected void btnSubmitLogin_Click(object sender, EventArgs e)
        {
            if (btnSubmitLogin.Text.Trim() == "Login")
            {
                _user = new User(txtUserName.Text.Trim(), txtPassword.Text.Trim());
                if (_user.UserID > 0)
                {
                    _userID = _user.UserID;
                    _logedIn = true;
                    if (Request.Cookies["HouserLogin"] == null)
                    {
                        CreateLoginCookie(txtUserName.Text.Trim(), txtPassword.Text.Trim());
                    }
                    btnSubmitLogin.Text = "Log out";
                    txtUserName.Enabled = false;
                    txtPassword.Enabled = false;
                    _notification = "You are logged in as: " + _user.FirstName;

                    RedirectToAlt();
                }
                else
                {
                    _notification = "The user / password does not match our records";
                }
            }
            else
            {
                _logedIn = false;
                HttpCookie loginCookie = new HttpCookie("HouserLogin");
                loginCookie.Expires = DateTime.UtcNow.AddDays(-1);
                HttpContext.Current.Response.Cookies.Set(loginCookie);
                btnSubmitLogin.Text = "Login";
                txtUserName.Enabled = true;
                txtPassword.Enabled = true;
                _notification = "You are now logged out.";
            }
        }

        private void RedirectToAlt()
        {
            // Request the sherifsale page so we can get the available sale dates.
            string sheriffSaleDatePage = PageRequester.GetWebRequest("http://oklahomacounty.org/sheriff/SheriffSales/");
            // Create a list of dates
            List<string> dates = PageScraper.GetSheriffSaleDates(sheriffSaleDatePage);
            if (dates.Count > 0)
                Response.Redirect("/alt.aspx?userID=" + _user.UserID + "&date=" + dates[0]);
        }
        /// <summary>
        /// Create the session cookie.
        /// </summary>
        private void CreateLoginCookie(string userID, string password)
        {
            HttpCookie loginCookie = new HttpCookie("HouserLogin");
            loginCookie.Values.Add("UserName", userID);
            loginCookie.Values.Add("Password", password);
            loginCookie.Expires = DateTime.Now.AddYears(2);
            Response.Cookies.Add(loginCookie);
        }

        /// <summary>
        /// Check to see if there is a login cookie and if it is valid.
        /// </summary>
        private bool IsLogedIn()
        {
            if (Request.Cookies["HouserLogin"] == null)
                return false;
            
            string userName = Request.Cookies["HouserLogin"]["UserName"];
            string password = Request.Cookies["HouserLogin"]["Password"];
            _user = new User(userName, password);
            if (_user != null && _user.UserID != 0)
                return true;
            return false;
        }
    }
}