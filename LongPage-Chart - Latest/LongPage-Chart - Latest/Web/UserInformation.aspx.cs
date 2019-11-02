using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web
{
    public partial class UserInformation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user"] == null)
                Response.Redirect("Login.aspx");
            UserItem item = (UserItem)Session["user"];
            txtEmail.Value = item.Email;
            txtFirstName.Value = item.FirstName;
            txtLastName.Value = item.LastName;
            txtUserName.Value = item.UserName;
            txtUsergroup.Value = item.UserGroup == 1 ? "Administrator" : "User";

            DBMangement dbMan = new DBMangement();
            List<OrganizationItem> lstOrgan = dbMan.GetOrganization();
            for(int i=0; i < lstOrgan.Count; i++)
                if(lstOrgan[i].ID == item.UserOrganization)
                    txtOraganization.Value = lstOrgan[i].Name;
        }
       

        protected void btnReset_Click1(object sender, EventArgs e)
        {
            txtOldPwd.Value = "";
            txtNewPwd.Value = "";
            txtConfirmPwd.Value = "";
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            UserItem item = (UserItem)Session["user"];
            DBMangement dbMan = new DBMangement();
            if (txtOldPwd.Value == item.Password)
            {
                if(txtNewPwd.Value == txtConfirmPwd.Value)
                {
                    item.Password = txtNewPwd.Value;
                    Session["user"] = item;
                    List<string> fieldList = new List<string>();
                    List<string> valueList = new List<string>();
                    fieldList.Add("password");
                    valueList.Add(item.Password);
                    dbMan.UpdateTable("users", item.ID.ToString(), fieldList, valueList);
                    lblMsg.Text = "Password successfully changed.";
                }
                else
                {
                    lblMsg.Text = "New Password and Confirm Password is Invalid.";
                }
            }
            else
            {
                lblMsg.Text = "Old Password Error.";
            }
        }
    }
}