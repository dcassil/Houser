using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using houser.Business;
using System.Data;

namespace houser
{
    public partial class print : System.Web.UI.Page
    {
        #region Private Variables.
        public int userID = 0;
        private bool logedIn;
        private User user;
        public DateTime saleDate;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            CheckLoginCookie();
            if (!logedIn)
            {
                Response.Redirect("/login.aspx");   
            }
            if (!IsPostBack)
            {
                string date = Request.QueryString["date"];
                string s_userID = Request.QueryString["userID"];
                if (date != null)
                {
                    saleDate = Convert.ToDateTime(date);
                }
                if (s_userID != null)
                {
                    userID = Convert.ToInt32(s_userID);
                }

            }
        }

        // If login cookie does not exist and the user and password are verified then create the login cookie.
        protected void btnSubmitLogOut_Click(object sender, EventArgs e)
        {
            logedIn = false;
            HttpCookie loginCookie = new HttpCookie("HouserLogin");
            loginCookie.Expires = DateTime.UtcNow.AddDays(-1);
            HttpContext.Current.Response.Cookies.Set(loginCookie);
            Response.Redirect("/login.aspx");
        }

        private void CheckLoginCookie()
        {
            if (Request.Cookies["HouserLogin"] != null)
            {
                string userName = Request.Cookies["HouserLogin"]["UserName"];
                string password = Request.Cookies["HouserLogin"]["Password"];
                user = new User(userName, password);
                if (user.UserID != 0)
                {
                    userID = user.UserID;
                    logedIn = true;
                }
            }
        }
    }
}