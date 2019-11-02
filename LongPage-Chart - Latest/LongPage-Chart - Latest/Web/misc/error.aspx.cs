using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using inc;

namespace iom
{
    public partial class error : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            base.SetContextLabel("Error !");
            this.SetSubMenu();
            base.SetMessage("!!! Oopsy daisy, how embarrasing! A alert mail message has been sent to system administrators");
            if (!this.IsPostBack) this.SendErrorEmail();            
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            IncUtilxs.HideMapIcon(this.Master);
        }

        protected void SetSubMenu()
        {
            PlaceHolder plchldr;
            Literal ltrl;
            Label lbl;            
            Panel pnl;

            pnl = (Panel)this.Master.FindControl("pnl_topbar");
            if (pnl != null) pnl.Height = 28;

            lbl = (Label)this.Master.FindControl("lbl_main");
            if (lbl != null) lbl.Visible = false;

            plchldr = (PlaceHolder)this.Master.FindControl("plchldr_lgnstatus");
            if (plchldr != null) plchldr.Visible = false;

            plchldr = (PlaceHolder)this.Master.FindControl("plchldr_lgnstatus");
            if (plchldr != null) plchldr.Visible = false;

            pnl = (Panel)this.Master.FindControl("pnl_menubar");
            if (pnl != null) pnl.Height = 16;

            plchldr = (PlaceHolder)this.Master.FindControl("plchdr_bttns");
            if (plchldr != null)
            {
                String surl = String.Format("{0}{1}", this.GetClientUrlAuthority("txt_clnturl"), VirtualPathUtility.ToAbsolute("~/default.aspx"));
                String s = String.Format("<input id=\"btnok\" name=\"btnok\" type=\"button\" class=\"frmbttns\" onclick=\"{0}\" value=\"OK\" />", surl);

                ltrl = new Literal();
                ltrl.Text = s;
                plchldr.Controls.Add(ltrl);                
            }
        }

        private void SendErrorEmail()
        {
            String rply = String.Empty, msg = String.Empty;
            String sprmtkn = IncUtilxs.GetParameterName("error_token");
            String stknval = IncUtilxs.GetRequestParamValue(this.Request.Params, sprmtkn);

            if (String.IsNullOrEmpty(stknval))
            {
                this.SetMessage("!!! Error : An error occurred that prevented the program from carrying out the action you requested");
                return;
            }

            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
            {
                IncError rw = db.IncErrors.FirstOrDefault(obj => obj.token.Equals(stknval));
                if (rw != null) msg = rw.message;
            }

            if (String.IsNullOrEmpty(msg))
            {
                this.SetMessage("!!! Error : An error occurred that prevented the program from carrying out the action you requested");
                return;
            }

            try
            {                
                if (this.Session["incuser"] != null)
                {
                    IncAuthentication auth = (IncAuthentication)this.Session["incuser"];
                    if (auth.IsUserValid)
                    {
                        rply = auth.UserEmail;
                        msg = String.Format("User {0} encountered the following error:<br /><br />{1}<br /><br />", auth.UserName, msg);
                    }
                }

                String[] sndto = IncUtilxs.GetDictionaryKeyVal("admin_email").Split(';');

                if (sndto.Length == 0)
                    throw new Exception("!!! Error : Failed to send email. Distribution list is empty");

                IncUtilxs.DispatchEmail(sndto.ToList(), msg + "<br /><br /><br />", "!!! Error : PMISZ Management", new List<string>() { rply });

                this.SetMessage("!!! Oopsy daisy, how embarrasing. A alert mail message has been sent to system administrators");                
            }
            catch (Exception xcp)
            {
                this.SetMessage(String.Format("!!! Error : {0}", xcp.Message.ToString()));
            }
        }
    }
}