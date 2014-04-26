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
        public string notification = "";
        private string userName;
        private string password;
        private bool logedIn;
        private User user;
        private bool showNotification = false;
        public DateTime saleDate;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CheckLoginCookie();
                string date = Request.QueryString["date"];
                if (date != null)
                {
                    saleDate = Convert.ToDateTime(date);
                }

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
                }
            }
        }
    }
}