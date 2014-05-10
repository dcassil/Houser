using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using houser;
using houser.Business;
using houser.utilities;

namespace houser.WebUtilities
{
    /// <summary>
    /// Summary description for UserService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class UserService : System.Web.Services.WebService
    {

        [WebMethod]
        public object SubmitLogin(string userName, string password)
        {
            User user = new User(userName, password);
            if (user != null)
            {
                if (user.TokenExpirationDate > DateTime.Now)
                {
                    return new { authorized = true, token = user.Token.ToString() };
                }
                else
                {
                    user.UpdateToken();
                    return new { authorized = true, token = user.Token.ToString() };
                }
            }
            else
            {
                return new { authorized = false, message = "Not Authorized" };
            }
        }

        [WebMethod]
        public object SubmitTokenLogin(string token)
        {
            Guid gToken = new Guid(token);
            User user = new User(gToken);
            if (user != null)
            {
                if (user.TokenExpirationDate > DateTime.Now)
                {
                    return new { authorized = true, token = user.Token.ToString() };
                }
                else
                {
                    return new { authorized = false, message = "Token has expired" };
                }
            }
            return new { authorized = false, message = "Not Authorized" };
        }
    }
}
