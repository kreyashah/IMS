using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web
{
    public partial class Admin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user"] == null)
                Response.Redirect("Login.aspx");
            UserItem item = (UserItem)Session["user"];
            if(item.UserGroup != 1)
                Response.Redirect("Default.aspx");

            DBMangement dbMan = new DBMangement();
            List<OrganizationItem> lstOrganization = dbMan.GetOrganization();
            if (cmbOrganization.Items.Count == 0)
            {
                for (int i = 0; i < lstOrganization.Count; i++)
                {
                    cmbOrganization.Items.Add(lstOrganization[i].Name.ToString());
                    cmbOrganization.Items[i].Value = lstOrganization[i].ID.ToString();
                }
            }

        }

        protected void grdUser_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] =
                     Page.ClientScript.GetPostBackClientHyperlink(grdUser, "Select$" + e.Row.RowIndex);
            }
        }

        protected void grdUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            //grdIndividuals.SelectRow(grdIndividuals.SelectedIndex);
            DBMangement dbMan = new DBMangement();
            List<UserItem> items = dbMan.GetUserByID(Convert.ToInt32(grdUser.SelectedValue));
            if (items.Count == 0)
                return;
            UserItem item = items[0];

            LoadUserInformation(item);
        }

        private void LoadUserInformation(UserItem item)
        {
            txtFirstName.Text = item.FirstName;
            txtLastName.Text = item.LastName;
            txtUserName.Text = item.UserName;
            txtEmail.Text = item.Email;
            cmbUserGroup.SelectedValue = item.UserGroup.ToString();
            cmbOrganization.SelectedValue = item.UserOrganization.ToString();
            chkEnabled.Checked = item.Enabled == 1;
            chkLocked.Checked = item.Locked == 1;
            chkNotifyNewCase.Checked = item.NotifyNewCase == 1;
        }

        protected void grdUser_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (ViewState["PreviousRowIndex"] != null)
            {
                var previousRowIndex = (int)ViewState["PreviousRowIndex"];
                GridViewRow PreviousRow = grdUser.Rows[previousRowIndex];
                PreviousRow.ForeColor = System.Drawing.Color.Black;
            }
            int currentRowIndex = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow row = grdUser.Rows[currentRowIndex];
            row.ForeColor = System.Drawing.Color.Blue;
            ViewState["PreviousRowIndex"] = currentRowIndex;
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            if (txtFilter.Value == "")
                txtFilter.Value = " ";
            SqlDatabase.SelectParameters["filter"].DefaultValue = txtFilter.Value;
            SqlDatabase.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            DBMangement dbMan = new DBMangement();
            List<string> fieldList = new List<string>();
            List<string> valueList = new List<string>();
            if (grdUser.SelectedIndex == -1)
            {
                fieldList.Add("id");
                fieldList.Add("first_name");
                fieldList.Add("last_name");
                fieldList.Add("user_name");
                fieldList.Add("user_group");
                fieldList.Add("user_org");
                fieldList.Add("email");
                fieldList.Add("notify_new_case");
                fieldList.Add("enabled");
                fieldList.Add("locked");
                fieldList.Add("password");

                valueList.Add((dbMan.GetMaxIDFromTable("users") + 1).ToString());
                valueList.Add(txtFirstName.Text);
                valueList.Add(txtLastName.Text);
                valueList.Add(txtUserName.Text);
                valueList.Add(cmbUserGroup.SelectedValue);
                valueList.Add(cmbOrganization.SelectedValue);
                valueList.Add(txtEmail.Text);
                valueList.Add(chkNotifyNewCase.Checked ? "1" : "0");
                valueList.Add(chkEnabled.Checked ? "1" : "0");
                valueList.Add(chkLocked.Checked ? "1" : "0");
                valueList.Add("");

                dbMan.InsertIntoTable("users", fieldList, valueList);
            }
            else
            {
                fieldList.Add("first_name");
                fieldList.Add("last_name");
                fieldList.Add("user_name");
                fieldList.Add("user_group");
                fieldList.Add("user_org");
                fieldList.Add("email");
                fieldList.Add("notify_new_case");
                fieldList.Add("enabled");
                fieldList.Add("locked");
                
                valueList.Add(txtFirstName.Text);
                valueList.Add(txtLastName.Text);
                valueList.Add(txtUserName.Text);
                valueList.Add(cmbUserGroup.SelectedValue);
                valueList.Add(cmbOrganization.SelectedValue);
                valueList.Add(txtEmail.Text);
                valueList.Add(chkNotifyNewCase.Checked ? "1" : "0");
                valueList.Add(chkEnabled.Checked ? "1" : "0");
                valueList.Add(chkLocked.Checked ? "1" : "0");

                dbMan.UpdateTable("users", grdUser.SelectedValue.ToString(), fieldList, valueList);
            }
            SqlDatabase.EnableCaching = false;
            grdUser.DataBind();
            SqlDatabase.EnableCaching = true;
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string individual_id = grdUser.SelectedValue.ToString();
            DBMangement dbMan = new DBMangement();
            dbMan.DeleteItem("users", individual_id);

            SqlDatabase.EnableCaching = false;
            grdUser.DataBind();
            SqlDatabase.EnableCaching = true;

            InitializeControls();
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            InitializeControls();
        }

        private void InitializeControls()
        {
            grdUser.SelectedIndex = -1;
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtUserName.Text = "";
            txtEmail.Text = "";
            chkEnabled.Checked = false;
            chkLocked.Checked = false;
            chkNotifyNewCase.Checked = false;
            cmbOrganization.SelectedIndex = 0;
            cmbUserGroup.SelectedIndex = 0;

            if (ViewState["PreviousRowIndex"] != null && grdUser.Rows.Count > 0 )
            {
                var previousRowIndex = (int)ViewState["PreviousRowIndex"];
                if (previousRowIndex < grdUser.Rows.Count)
                {
                    GridViewRow PreviousRow = grdUser.Rows[previousRowIndex];
                    PreviousRow.ForeColor = System.Drawing.Color.Black;
                }
            }
        }
    }

}
