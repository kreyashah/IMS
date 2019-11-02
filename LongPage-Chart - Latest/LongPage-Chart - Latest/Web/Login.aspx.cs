using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogIn_Click(object sender, EventArgs e)
        {
            DBMangement dbMan = new DBMangement();
            string userName = txtUserName.Text;
            string password = txtPwd.Text;
            List<UserItem> item = dbMan.GetValidUsers(userName, password);
            if (item.Count > 0)
            {
                Session["user"] = item[0];
                Response.Redirect("Default.aspx");
            }
        }
    }
}