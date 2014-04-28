using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using houser.Business;

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
    }
}