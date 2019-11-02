using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string url = HttpContext.Current.Request.Url.AbsolutePath;
            bool flag = false;
            if (url.Contains("Default"))
            {
                aHomeLink.Attributes.Add("class", "current");
                flag = true;
            }
            else
                aHomeLink.Attributes.Remove("class");

            if (url.Contains("Cases"))
            {
                aCaseLink.Attributes.Add("class", "current");
                flag = true;
            }
            else
                aCaseLink.Attributes.Remove("class");

            if (url.Contains("Summery"))
            {
                aSummeryLink.Attributes.Add("class", "current");
                flag = true;
            }
            else
                aSummeryLink.Attributes.Remove("class");

            if (url.Contains("About"))
            {
                aAboutLink.Attributes.Add("class", "current");
                flag = true;
            }
            else
                aAboutLink.Attributes.Remove("class");

            if (url.Contains("Contact"))
            {
                aContactLink.Attributes.Add("class", "current");
                flag = true;
            }
            else
                aContactLink.Attributes.Remove("class");
            //if(flag == false)
            //    aHomeLink.Attributes.Add("class", "current");
            if (Session["user"] == null)
                Response.Redirect("Login.aspx");
            UserItem item = (UserItem)Session["user"];
            aUserInformation.InnerText = item.UserName;
        }
        
    }
}