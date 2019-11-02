using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Data.SqlClient;
using System.Configuration;
using inc;
using iom;

namespace Web
{
    public partial class referrals : System.Web.UI.Page
    {
        private const String FMTTEXTFIELD = "vsapo{0}yjdge";
        private const String FMTDROPLISTFIELD = "gwdlz{0}nkghw";
        private const String FMTINSTANCEMASK = "ybgldc{0}hdfkr";
        private const String CACHENAME = "referrals";

        private const String FMTDAYID = "fdsbe{0}opnte{1}nghdr";
        private const String FMTMONID = "ubdmh{0}tkzap{1}sqzeo";
        private const String FMTYRID = "wucsi{0}fdxew{1}gfvtp";

        private const String FMTAGEFROM = "bthfgr{0}mapske";
        private const String FMTAGETO = "dcqxza{0}injrcgs";

        protected void Page_Load(object sender, EventArgs e)
        {
            this.SetMessage("Use this page to make referrals on affected individuals to other organisational units");
            this.SetContextLabel("Referrals");

            this.PutReferralModes();
            this.SetSubMenu();

            this.ViewState["rec_org_unit"] = this.rec_org_unit.SelectedItem != null ? this.rec_org_unit.SelectedItem.Value : "0";
            if (this.ViewState["instance_id"] == null) this.ViewState["instance_id"] = this.NewInstanceID();

            this.ViewState["emailuser"] = this.emailuser.SelectedItem != null ? this.emailuser.SelectedItem.Value : "0";
            this.ViewState["emailuser2"] = this.emailuser2.SelectedItem != null ? this.emailuser2.SelectedItem.Value : "0";

            this.ViewState["drp_srch1"] = this.drp_srch1.SelectedItem != null ? this.drp_srch1.SelectedItem.Value : "0";
            this.ViewState["drp_srch2"] = this.drp_srch2.SelectedItem != null ? this.drp_srch2.SelectedItem.Value : "0";
            this.ViewState["drp_srch3"] = this.drp_srch3.SelectedItem != null ? this.drp_srch3.SelectedItem.Value : "0";
            this.ViewState["drp_srch4"] = this.drp_srch4.SelectedItem != null ? this.drp_srch4.SelectedItem.Value : "0";

            this.ViewState["count_search"] = "0";

            if (!this.IsPostBack)
            {
                if (this.IsReferral())
                {
                    Regex rgxp = new Regex("^\\d+$");
                    String sprmid = IncUtilxs.GetParameterName("id");

                    if (this.Request.QueryString.AllKeys.Contains(sprmid))
                        if (rgxp.IsMatch(this.Request.QueryString[sprmid]))
                            this.GetRecord(this.Request.QueryString[sprmid]);
                }

                this.ViewState["referrer"] = this.Request.UrlReferrer != null ? this.Request.UrlReferrer.AbsoluteUri : this.Request.Url.AbsoluteUri;
            }

            this.is_error.Value = "0";
        }

        private void SetContextLabel(string s)
        {
            if (this.Master != null)
            {
                Label lbl = (Label)this.Master.FindControl("lbl_context");
                if (lbl != null) lbl.Text = s;
            }
        }

        private void SetMessage(string s)
        {
            if (this.Master != null)
            {
                Label lbl = (Label)this.Master.FindControl("lbl_msg");
                if (lbl != null) lbl.Text = s;
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            IncUtilxs.FillOrgDropDown(this.rec_org_unit, Int16.Parse(this.ViewState["rec_org_unit"].ToString()));

            if (this.IsReferral())
            {
                IncUtilxs.FillReferralSearchFields(this.drp_srch1);
                this.ReSetSearchFieldsDrop(this.drp_srch1, "drp_srch1");
                IncUtilxs.FillReferralSearchFields(this.drp_srch2);
                this.ReSetSearchFieldsDrop(this.drp_srch2, "drp_srch2");
                IncUtilxs.FillReferralSearchFields(this.drp_srch3);
                this.ReSetSearchFieldsDrop(this.drp_srch3, "drp_srch3");
                IncUtilxs.FillReferralSearchFields(this.drp_srch4);
                this.ReSetSearchFieldsDrop(this.drp_srch4, "drp_srch4");
            }
            else
            {
                IncUtilxs.FillReferralSearchSearch(this.drp_srch1);
                this.ReSetSearchFieldsDrop(this.drp_srch1, "drp_srch1");
                IncUtilxs.FillReferralSearchSearch(this.drp_srch2);
                this.ReSetSearchFieldsDrop(this.drp_srch2, "drp_srch2");
                IncUtilxs.FillReferralSearchSearch(this.drp_srch3);
                this.ReSetSearchFieldsDrop(this.drp_srch3, "drp_srch3");
                IncUtilxs.FillReferralSearchSearch(this.drp_srch4);
                this.ReSetSearchFieldsDrop(this.drp_srch4, "drp_srch4");
            }

            this.MakeSearchField(ref this.plchldr1, this.ViewState["drp_srch1"] != null ? this.ViewState["drp_srch1"].ToString() : String.Empty);
            this.MakeSearchField(ref this.plchldr2, this.ViewState["drp_srch2"] != null ? this.ViewState["drp_srch2"].ToString() : String.Empty);
            this.MakeSearchField(ref this.plchldr3, this.ViewState["drp_srch3"] != null ? this.ViewState["drp_srch3"].ToString() : String.Empty);
            this.MakeSearchField(ref this.plchldr4, this.ViewState["drp_srch4"] != null ? this.ViewState["drp_srch4"].ToString() : String.Empty);

            if (!this.IsPostBack) this.FillDropDowns();
            this.IncInitSQL();

            this.lstsrch.DataBind();
            this.lstrefs.DataBind();
            this.lstpstrefs.DataBind();
            this.lst_srch_mode.DataBind();

            if (this.is_error.Value.Equals("1"))
            {
                Label lbl = (Label)this.Master.FindControl("lbl_msg");

                this.err_label.Visible = true;
                if (lbl != null) this.err_label.Text = lbl.Text;
            }
            else
                this.err_label.Visible = false;

            if (this.IsReferral())
            {
                this.pnl_referral_mode.Visible = true;
                this.pnl_search.Visible = false;

                if (this.ViewState["id"] != null)
                {
                    this.lbl_more.Visible = true;
                    this.lbl_more.Attributes.CssStyle.Add(HtmlTextWriterStyle.Cursor, "pointer");
                }
                else
                    this.lbl_more.Visible = false;
            }
            else
            {
                this.pnl_referral_mode.Visible = false;
                this.pnl_search.Visible = true;
                this.lbl_more.Visible = false;
            }
        }

        private void IncInitSQL()
        {
            if (this.IsReferral())
                this.ViewState["query"] = null;
            else
                this.ViewState["sql"] = null;
        }

        private void SetSubMenu()
        {
            PlaceHolder bttns = (PlaceHolder)this.Master.FindControl("plchdr_bttns");
            PlaceHolder lnks = (PlaceHolder)this.Master.FindControl("plchdr_links");

            Button btn;
            Literal ltrl;
            String url = String.Empty;

            if (this.Request.UrlReferrer != null)
                url = this.ViewState["referrer"] != null ? this.ViewState["referrer"].ToString() : this.Request.UrlReferrer.AbsoluteUri;
            else
                url = this.Request.Url.AbsoluteUri;

            ltrl = new Literal();
            ltrl.Text = String.Format("<input id=\"btnbk\" name=\"btnbk\" class=\"frmbttns\" type=\"button\" value=\"Back\" onclick=\"javascript: window.location.href='{0}'\" />", url);
            bttns.Controls.Add(ltrl);

            if (this.IsReferral())
            {
                ltrl = new Literal();
                ltrl.Text = "<input id=\"btnsv\" name=\"btnsv\" class=\"frmbttns\" type=\"button\" value=\"Save\" />";
                bttns.Controls.Add(ltrl);

                btn = new Button();
                btn.ID = "btnclr";
                btn.CausesValidation = false;
                btn.CssClass = "frmbttns";
                btn.Click += new EventHandler(this.NewReferral);
                btn.Text = "Clear";
                bttns.Controls.Add(btn);
            }
            else
            {
                btn = new Button();
                btn.ID = "btncsv";
                btn.CausesValidation = false;
                btn.CssClass = "frmbttns";
                btn.Click += new EventHandler(this.GenerateCSV);
                btn.Text = "CSV";
                bttns.Controls.Add(btn);

                btn = new Button();
                btn.ID = "btnsclr";
                btn.CausesValidation = false;
                btn.CssClass = "frmbttns";
                btn.Click += new EventHandler(this.NewSearch);
                btn.Text = "Clear";
                bttns.Controls.Add(btn);
            }

            this.PutSearchDropList(lnks);
        }

        private void PutSearchDropList(PlaceHolder plchldr)
        {
            DropDownList drp = new DropDownList();
            Button bttn = new Button();

            drp.ID = "drp_search";
            drp.CssClass = "drpdwnw";
            drp.CausesValidation = false;
            drp.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            drp.Items.Add(new ListItem("-- Search --", "0"));
            drp.Items.Add(new ListItem("Individual(s)", "srchindividual.aspx"));
            drp.Items.Add(new ListItem("Case(s)", "srchcase.aspx"));
            plchldr.Controls.Add(drp);

            bttn.ID = "btngosrch";
            bttn.Text = "GO";
            bttn.CssClass = "nwbttns";
            bttn.CausesValidation = false;
            bttn.ClientIDMode = System.Web.UI.ClientIDMode.Static;
            bttn.Click += new EventHandler(this.DoSearch);
            bttn.Attributes.CssStyle.Add(HtmlTextWriterStyle.MarginLeft, "5px");
            bttn.Attributes.CssStyle.Add(HtmlTextWriterStyle.Padding, "0px 3px");
            plchldr.Controls.Add(bttn);
        }

        protected void DoSearch(object sender, EventArgs e)
        {
            String vl = IncUtilxs.GetRequestParamValue(this.Request.Params, "drp_search");

            if (String.IsNullOrEmpty(vl)) return;
            if (vl.Equals("0")) return;

            this.Response.Redirect("~/" + vl);
        }

        private void PutReferralModes()
        {
            PlaceHolder nvbttns = (PlaceHolder)this.Master.FindControl("plchldr_navbttns");
            String s = IncUtilxs.GetDictionaryKeyVal("referral_modes");
            String vl = IncUtilxs.GetRequestParamValue(this.Request.Params, "drp_refmodes");

            List<Dictionary<String, String>> lst = IncUtilxs.DictKeyValString(s, ';', ',');

            Label lbl = new Label();
            lbl.Text = "Select Mode";
            lbl.Attributes.CssStyle.Add(HtmlTextWriterStyle.MarginRight, "5px");
            lbl.Attributes.CssStyle.Add(HtmlTextWriterStyle.FontWeight, "bold");

            DropDownList drp = new DropDownList();
            foreach (Dictionary<String, String> dct in lst) drp.Items.Add(new ListItem(dct["val"], dct["key"]));
            drp.ID = "drp_refmodes";
            drp.CssClass = "drpdwnw";
            drp.Width = Unit.Pixel(180);
            drp.AutoPostBack = true;
            drp.CausesValidation = false;
            if (drp.Items.FindByValue(vl) != null) drp.SelectedValue = vl;
            nvbttns.Controls.Add(lbl);
            nvbttns.Controls.Add(drp);
        }

        protected void NotAllowed(String msg, String cntxt)
        {
            IncPrompt prmpt = new IncPrompt();

            String js = String.Format("javascript: window.location.href='{0}';", this.Request.Url.PathAndQuery);
            prmpt.MessageButtons.Add(new MessageButton() { ButtonLabel = "OK", ButtonName = "btnok", JScriptCommand = js });

            prmpt.PromptItems.Add("message", msg);
            prmpt.PromptItems.Add("context", cntxt);

            this.Session["prompt"] = prmpt;
            this.Response.Redirect("~/prompt.aspx", true);
        }

        private void GetRecord(String sid)
        {
            IncAuthentication auth = (IncAuthentication)this.Session["incuser"];

            if (!auth.AnyReadPermission())
                this.NotAllowed("!!! Error : You don't have permission to carry out the action you requested", "! Permission Denied");

            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
            {
                VwReferral robj = db.VwReferrals.FirstOrDefault(obj => obj.id == Int32.Parse(sid));
                if (robj != null)
                {
                    this.ViewState["id"] = robj.id.ToString();
                    String eml1 = !String.IsNullOrEmpty(robj.emailuser) ? robj.emailuser : String.Empty;
                    String eml2 = !String.IsNullOrEmpty(robj.emailuser2) ? robj.emailuser2 : String.Empty;

                    if (!String.IsNullOrEmpty(eml1))
                    {
                        IncUser usr = db.IncUsers.FirstOrDefault(obj => obj.email.Equals(eml1));
                        if (usr != null) this.ViewState["emailuser"] = usr.id.ToString();
                    }

                    if (!String.IsNullOrEmpty(eml2))
                    {
                        IncUser usr = db.IncUsers.FirstOrDefault(obj => obj.email.Equals(eml2));
                        if (usr != null) this.ViewState["emailuser2"] = usr.id.ToString();
                    }

                    this.copyemail.Text = !String.IsNullOrEmpty(robj.copyemail) ? robj.copyemail : String.Empty;
                    this.subject.Text = !String.IsNullOrEmpty(robj.subject) ? robj.subject : String.Empty;
                    this.message.Text = !String.IsNullOrEmpty(robj.message) ? robj.message : String.Empty;
                    this.ViewState["rec_org_unit"] = robj.rec_org_unit_id.ToString();
                    this.lbl_reference_no.Text = robj.referral_no;
                }
            }

            if (this.ViewState["id"] != null) this.GrabIndivsToCache(this.ViewState["id"].ToString());
        }

        private List<VwReferral> GetPastReferrals()
        {
            List<VwReferral> lst = null;

            IncAuthentication auth = (IncAuthentication)this.Session["incuser"];

            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
                lst = db.VwReferrals.Where(obj => obj.updated_by.Equals(auth.UserName)).Take(100).ToList();

            if (lst == null) lst = new List<VwReferral>();

            return lst.OrderBy(obj => obj.individual_name).ToList();
        }

        protected void lnq_pstrefs_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            e.Result = this.GetPastReferrals();
        }

        private void MakeSearchField(ref PlaceHolder plchldr, String sfld)
        {
            DropDownList drp;
            TextBox txt;
            Literal ltrl;
            String vl = String.Empty, sindx = plchldr.ID[plchldr.ID.Length - 1].ToString();
            String cid = String.Empty;

            switch (sfld)
            {
                case "displacement_status_id":
                    cid = String.Format(FMTDROPLISTFIELD, sindx);
                    drp = new DropDownList();
                    drp.ID = cid;
                    drp.CssClass = "mddrpdwn";
                    IncUtilxs.FillDisplacementStatusDropList(drp, "0");
                    this.ReSetDropDownValue(drp, cid);
                    plchldr.Controls.Add(drp);
                    break;
                case "incident_type_id":
                    cid = String.Format(FMTDROPLISTFIELD, sindx);
                    drp = new DropDownList();
                    drp.ID = cid;
                    drp.CssClass = "mddrpdwn";
                    IncUtilxs.FillIncidentDropDown(drp, 0);
                    this.ReSetDropDownValue(drp, cid);
                    plchldr.Controls.Add(drp);
                    break;
                case "vulnerability_id":
                    cid = String.Format(FMTDROPLISTFIELD, sindx);
                    drp = new DropDownList();
                    drp.ID = cid;
                    drp.CssClass = "mddrpdwn";
                    IncUtilxs.FillVulnerabilityLevelDropList(drp, String.Empty);
                    this.ReSetDropDownValue(drp, cid);
                    plchldr.Controls.Add(drp);
                    break;
                case "incident_date":
                case "dob":
                case "date_reported":
                case "ref_date":
                    List<Int32> dflts = new List<Int32>() { DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year };
                    ltrl = new Literal();

                    ltrl.Text = "<br /><br />";

                    String svl = String.Empty;

                    String dyid = String.Format(FMTDAYID, sindx, "11");
                    String mnid = String.Format(FMTMONID, sindx, "11");
                    String yrid = String.Format(FMTYRID, sindx, "11");

                    svl = IncUtilxs.GetRequestParamValue(this.Request.Params, dyid);
                    if (!String.IsNullOrEmpty(svl)) dflts[0] = Int32.Parse(svl);

                    svl = IncUtilxs.GetRequestParamValue(this.Request.Params, mnid);
                    if (!String.IsNullOrEmpty(svl)) dflts[1] = Int32.Parse(svl);

                    svl = IncUtilxs.GetRequestParamValue(this.Request.Params, yrid);
                    if (!String.IsNullOrEmpty(svl)) dflts[2] = Int32.Parse(svl);

                    IncUtilxs.MakeDMYDropDowns(plchldr, "From", dyid, mnid, yrid, DateTime.Now.Year - 90, 91, dflts);
                    plchldr.Controls.Add(ltrl);

                    dyid = String.Format(FMTDAYID, sindx, "22");
                    mnid = String.Format(FMTMONID, sindx, "22");
                    yrid = String.Format(FMTYRID, sindx, "22");

                    svl = IncUtilxs.GetRequestParamValue(this.Request.Params, dyid);
                    if (!String.IsNullOrEmpty(svl)) dflts[0] = Int32.Parse(svl);

                    svl = IncUtilxs.GetRequestParamValue(this.Request.Params, mnid);
                    if (!String.IsNullOrEmpty(svl)) dflts[1] = Int32.Parse(svl);

                    svl = IncUtilxs.GetRequestParamValue(this.Request.Params, yrid);
                    if (!String.IsNullOrEmpty(svl)) dflts[2] = Int32.Parse(svl);

                    IncUtilxs.MakeDMYDropDowns(plchldr, "To", dyid, mnid, yrid, DateTime.Now.Year - 90, 91, dflts);
                    break;
                case "gender":
                    cid = String.Format(FMTDROPLISTFIELD, sindx);
                    drp = new DropDownList();
                    drp.ID = cid;
                    drp.CssClass = "mddrpdwn";
                    drp.Items.Add(new ListItem("Male", "Male"));
                    drp.Items.Add(new ListItem("Female", "Female"));
                    this.ReSetDropDownValue(drp, cid);
                    plchldr.Controls.Add(drp);
                    break;
                case "age":
                    cid = String.Format(FMTAGEFROM, sindx);
                    txt = new TextBox();
                    txt.ID = cid;
                    txt.CssClass = "atxtbx";
                    txt.Width = Unit.Pixel(85);
                    txt.Attributes.CssStyle.Add(HtmlTextWriterStyle.Padding, "3px 0px 3px 2px");
                    this.ReSetTextValue(txt, cid);
                    plchldr.Controls.Add(txt);

                    ltrl = new Literal();
                    ltrl.Text = "<span style=\"margin: 0px 5px 0px 20px\">To</span>";
                    plchldr.Controls.Add(ltrl);

                    cid = String.Format(FMTAGETO, sindx);
                    txt = new TextBox();
                    txt.ID = cid;
                    txt.CssClass = "atxtbx";
                    txt.Width = Unit.Pixel(85);
                    txt.Attributes.CssStyle.Add(HtmlTextWriterStyle.Padding, "3px 0px 3px 2px");
                    this.ReSetTextValue(txt, cid);
                    plchldr.Controls.Add(txt);
                    break;
                case "is_breadwinner":
                    cid = String.Format(FMTDROPLISTFIELD, sindx);
                    drp = new DropDownList();
                    drp.ID = cid;
                    drp.CssClass = "mddrpdwn";
                    drp.Items.Add(new ListItem("N/A", "2"));
                    drp.Items.Add(new ListItem("Yes", "1"));
                    drp.Items.Add(new ListItem("No", "0"));
                    this.ReSetDropDownValue(drp, cid);
                    plchldr.Controls.Add(drp);
                    break;
                case "marital_status":
                    cid = String.Format(FMTDROPLISTFIELD, sindx);
                    drp = new DropDownList();
                    drp.ID = cid;
                    drp.CssClass = "mddrpdwn";
                    IncUtilxs.FillMaritalStatus(drp, String.Empty);
                    this.ReSetDropDownValue(drp, cid);
                    plchldr.Controls.Add(drp);
                    break;
                case "rec_org_unit_id":
                case "user_org_unit_id":
                    cid = String.Format(FMTDROPLISTFIELD, sindx);
                    drp = new DropDownList();
                    drp.ID = cid;
                    drp.CssClass = "mddrpdwn";
                    IncUtilxs.FillOrgDropDown(drp, 0);
                    this.ReSetDropDownValue(drp, cid);
                    plchldr.Controls.Add(drp);
                    break;
                default:
                    cid = String.Format(FMTTEXTFIELD, sindx);
                    txt = new TextBox();
                    txt.ID = cid;
                    txt.CssClass = "mdtxtfields";
                    this.ReSetTextValue(txt, cid);
                    plchldr.Controls.Add(txt);
                    break;
            }
        }

        protected void lnq_srch_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            List<VwIndividual> lst = null;

            String sql = "select a.* from v_individuals a where a.id = 0";

            if (this.ViewState["sql"] != null) sql = this.ViewState["sql"].ToString();

            sql = "set dateformat dmy; " + sql;

            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
                lst = db.ExecuteQuery<VwIndividual>(sql).Take(100).ToList();

            if (lst == null) lst = new List<VwIndividual>();

            e.Result = lst.OrderBy(obj => obj.lname).ToList();
        }

        protected void lnq_refs_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            List<VwIndividual> lst = null;
            Dictionary<String, Object> dct = null;

            if (this.ViewState["instance_id"] != null)
            {
                dct = this.GetDictSessCachedObjects(CACHENAME, this.ViewState["instance_id"].ToString());
                if (dct != null)
                {
                    lst = new List<VwIndividual>();
                    foreach (KeyValuePair<String, Object> kv in dct)
                        lst.Add((VwIndividual)kv.Value);
                }
            }

            if (lst == null) lst = new List<VwIndividual>();

            e.Result = lst.OrderBy(obj => obj.lname).ToList();
        }

        protected Dictionary<String, Object> GetDictSessCachedObjects(String dctname, String sinstance)
        {
            Dictionary<String, Object> dct = null;
            Dictionary<String, Object> instdct = null;

            if (this.Session[dctname] != null) dct = (Dictionary<String, Object>)this.Session[dctname];
            if (dct != null)
                if (dct.Keys.Contains(sinstance)) instdct = (Dictionary<String, Object>)dct[sinstance];

            return instdct;
        }

        private String GetSearchCriteria()
        {
            List<String> whr = new List<string>();
            String s = String.Empty;
            String x = String.Empty;

            x = this.GetCriteriaExpression(this.drp_srch1, "1", "a");
            if (!String.IsNullOrEmpty(x)) whr.Add(x);

            x = this.GetCriteriaExpression(this.drp_srch2, "2", "a");
            if (!String.IsNullOrEmpty(x)) whr.Add(x);

            x = this.GetCriteriaExpression(this.drp_srch3, "3", "a");
            if (!String.IsNullOrEmpty(x)) whr.Add(x);

            x = this.GetCriteriaExpression(this.drp_srch4, "4", "a");
            if (!String.IsNullOrEmpty(x)) whr.Add(x);

            if (whr.Count == 0) whr.Add("a.id = 0");

            s = String.Join(" and ", whr.ToArray());

            return s;
        }

        private String GetCriteriaExpression(DropDownList drp, String sindx, String prfx)
        {
            Regex rgxp = new Regex("^\\d+$");
            List<String> dtlst = new List<string>() { "date_reported", "dob", "ref_date" };

            String s = String.Empty;
            String vl1 = String.Empty;
            String vl2 = String.Empty;
            String a = String.Empty;
            String b = String.Empty;

            if (drp.SelectedValue.Equals("0")) return String.Empty;

            if (dtlst.Contains(drp.SelectedValue))
            {
                String dyid = String.Format(FMTDAYID, sindx, "11");
                String mnid = String.Format(FMTMONID, sindx, "11");
                String yrid = String.Format(FMTYRID, sindx, "11");

                String sfrom = String.Empty;
                String sto = String.Empty;

                String sdy = IncUtilxs.GetRequestParamValue(this.Request.Params, dyid);
                String smn = IncUtilxs.GetRequestParamValue(this.Request.Params, mnid);
                String syr = IncUtilxs.GetRequestParamValue(this.Request.Params, yrid);

                sfrom = String.Format("{0}/{1}/{2}", sdy, smn, syr);

                dyid = String.Format(FMTDAYID, sindx, "22");
                mnid = String.Format(FMTMONID, sindx, "22");
                yrid = String.Format(FMTYRID, sindx, "22");

                sdy = IncUtilxs.GetRequestParamValue(this.Request.Params, dyid);
                smn = IncUtilxs.GetRequestParamValue(this.Request.Params, mnid);
                syr = IncUtilxs.GetRequestParamValue(this.Request.Params, yrid);

                sto = String.Format("{0}/{1}/{2}", sdy, smn, syr);

                if ((IncUtilxs.IsDate(sfrom, "dd/MM/yyyy") || IncUtilxs.IsDate(sfrom, "d/M/yyyy")) && (IncUtilxs.IsDate(sto, "dd/MM/yyyy") || IncUtilxs.IsDate(sto, "d/M/yyyy")))
                    s = String.Format("{0}.{1} between '{2}' and '{3}'", prfx, drp.SelectedValue, sfrom, sto);
            }
            else
                if (drp.SelectedValue.Equals("age"))
            {
                a = String.Format(FMTAGEFROM, sindx);
                b = String.Format(FMTAGETO, sindx);

                vl1 = IncUtilxs.GetRequestParamValue(this.Request.Params, a);
                vl2 = IncUtilxs.GetRequestParamValue(this.Request.Params, b);

                if (rgxp.IsMatch(vl1) && rgxp.IsMatch(vl2))
                    s = String.Format("{0}.{1} between {2} and {3}", prfx, drp.SelectedValue, vl1, vl2);
            }
            else
                    if (drp.SelectedValue.Equals("is_breadwinner"))
            {
                String ky = String.Format(FMTDROPLISTFIELD, sindx);
                String vl = IncUtilxs.GetRequestParamValue(this.Request.Params, ky);
                if (!String.IsNullOrEmpty(vl))
                    s = String.Format("{0}.{1} = {2}", prfx, drp.SelectedValue, vl);
            }
            else
            {
                a = String.Format(FMTTEXTFIELD, sindx);
                b = String.Format(FMTDROPLISTFIELD, sindx);

                vl1 = IncUtilxs.GetRequestParamValue(this.Request.Params, a);
                vl2 = IncUtilxs.GetRequestParamValue(this.Request.Params, b);

                if (!String.IsNullOrEmpty(vl1))
                    s = String.Format("{0}.{1} like '{2}'", prfx, drp.SelectedValue, IncUtilxs.CleanSQLExpression(vl1));
                else
                    if (!String.IsNullOrEmpty(vl2))
                    if (rgxp.IsMatch(vl2))
                        switch (drp.SelectedValue)
                        {
                            case "displacement_status_id":
                                s = String.Format("({0}.{1} = {2} or {0}.case_displacement_status_id = {2})", prfx, drp.SelectedValue, vl2);
                                s += String.Format(" and ((dbo.is_iom_displacement({0}.incident_type_id, {0}.displacement_status_id, {2}) = 1) or (dbo.is_iom_displacement({0}.incident_type_id, {0}.case_displacement_status_id, {2}) = 1))", prfx, drp.SelectedValue, vl2);
                                break;
                            case "vulnerability_id":
                                s = String.Format("({0}.{1} = {2} or {0}.case_vulnerability_id = {2})", prfx, drp.SelectedValue, vl2);
                                break;
                            default:
                                s = String.Format("{0}.{1} = {2}", prfx, drp.SelectedValue, vl2);
                                break;
                        }
                    else
                        s = String.Format("{0}.{1} = '{2}'", prfx, drp.SelectedValue, vl2);
            }

            return s;
        }

        private void CacheIndividuals(String sid)
        {
            if (String.IsNullOrEmpty(sid)) return;

            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
            {
                VwIndividual indv = db.VwIndividuals.FirstOrDefault(obj => obj.id == Int32.Parse(sid));
                if (indv != null)
                    if (!this.ObjectExists(CACHENAME, this.ViewState["instance_id"].ToString(), indv.id.ToString()))
                        this.SessCacheObject(indv, CACHENAME, this.ViewState["instance_id"].ToString(), indv.id.ToString());
            }
        }

        protected Boolean ObjectExists(String dctname, String sinstance, String skey)
        {
            Dictionary<String, Object> dct = null;
            Dictionary<String, Object> instdct = null;

            Boolean b = false;

            if (this.Session[dctname] != null) dct = (Dictionary<String, Object>)this.Session[dctname];
            if (dct != null)
                if (dct.Keys.Contains(sinstance))
                {
                    instdct = (Dictionary<String, Object>)dct[sinstance];
                    b = instdct.Keys.Contains(skey);
                }

            return b;
        }

        protected void SessCacheObject(Object obj, String dctname, String sinstance, String skey)
        {
            Dictionary<String, Object> dct = null;
            Dictionary<String, Object> instdct = null;

            if (this.Session[dctname] != null) dct = (Dictionary<String, Object>)this.Session[dctname];
            if (dct == null)
            {
                dct = new Dictionary<string, object>();
                this.Session[dctname] = dct;
            }

            if (dct.Keys.Contains(sinstance))
                instdct = (Dictionary<String, Object>)dct[sinstance];
            else
            {
                dct.Add(sinstance, new Dictionary<String, Object>());
                instdct = (Dictionary<String, Object>)dct[sinstance];
            }

            if (!instdct.Keys.Contains(skey)) instdct.Add(skey, obj);
        }

        protected void chkallsrch_CheckedChanged(object sender, EventArgs e)
        {
        }

        protected void chkallrefs_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void RemovedIndividuals(String sid)
        {
            if (String.IsNullOrEmpty(sid)) return;

            this.RemoveCachedObject(CACHENAME, this.ViewState["instance_id"].ToString(), sid);
        }

        protected void RemoveCachedObject(String dctname, String sinstance, String skey)
        {
            Dictionary<String, Object> dct = null;
            Dictionary<String, Object> instdct = null;

            if (this.Session[dctname] != null) dct = (Dictionary<String, Object>)this.Session[dctname];
            if (dct != null)
                if (dct.Keys.Contains(sinstance))
                {
                    instdct = (Dictionary<String, Object>)dct[sinstance];
                    if (instdct.Keys.Contains(skey)) instdct.Remove(skey);
                }
        }

        private void FillDropDowns()
        {
            IncAuthentication auth = (IncAuthentication)this.Session["incuser"];
            IncUtilxs.FillOrgDropDown(this.rec_org_unit, Int16.Parse(this.ViewState["rec_org_unit"].ToString()));

            String sql = "select distinct u.* from t_users u";
            sql += " inner join t_user_perms p on p.usr_id = u.id";
            sql += String.Format(" where (p.org_unit_id = {0} and p.can_read = 1 and u.enabled = 1 and u.is_locked = 0)", this.rec_org_unit.SelectedItem.Value);
            sql += " order by u.lname";

            this.emailuser.Items.Clear();
            this.emailuser2.Items.Clear();

            this.emailuser.Items.Add(new ListItem("...Select...", "0"));
            this.emailuser2.Items.Add(new ListItem("...Select...", "0"));

            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
                foreach (IncUser usr in db.ExecuteQuery<IncUser>(sql).ToList())
                {
                    if (usr.usr_name.Equals(auth.UserName)) continue;

                    this.emailuser.Items.Add(new ListItem(String.Format("{0}, {1}", usr.lname, usr.fnames), usr.id.ToString()));
                    this.emailuser2.Items.Add(new ListItem(String.Format("{0}, {1}", usr.lname, usr.fnames), usr.id.ToString()));
                }

            if (this.ViewState["emailuser"] != null)
                if (this.emailuser.Items.FindByValue(this.ViewState["emailuser"].ToString()) != null)
                    this.emailuser.SelectedValue = this.ViewState["emailuser"].ToString();

            if (this.ViewState["emailuser2"] != null)
                if (this.emailuser2.Items.FindByValue(this.ViewState["emailuser2"].ToString()) != null)
                    this.emailuser2.SelectedValue = this.ViewState["emailuser2"].ToString();
        }

        private void WriteToDb()
        {
            if (!this.IsValidated()) return;

            IncAuthentication auth = (IncAuthentication)this.Session["incuser"];
            Dictionary<String, Object> dct = this.GetDictSessCachedObjects(CACHENAME, this.ViewState["instance_id"].ToString());

            Int32? rid = 0, fid = 0;
            Int32 xid = 0;
            Int16 eml1 = 0, eml2 = 0, rcorg = 0;
            Boolean ok = false;

            if (this.ViewState["emailuser"] != null) eml1 = Int16.Parse(this.ViewState["emailuser"].ToString());
            if (this.ViewState["emailuser2"] != null) eml2 = Int16.Parse(this.ViewState["emailuser2"].ToString());
            if (this.ViewState["rec_org_unit"] != null) rcorg = Int16.Parse(this.ViewState["rec_org_unit"].ToString());
            if (this.ViewState["id"] != null) xid = Int32.Parse(this.ViewState["id"].ToString());

            rid = xid;

            try
            {
                using (IncDataClassesDataContext db = new IncDataClassesDataContext())
                {
                    Int32 rslt = db.p_write_referral(ref rid, auth.UserOrgUnitID, rcorg, eml1, eml2, this.message.Text, this.copyemail.Text, this.subject.Text, auth.UserName);
                    if (dct != null)
                        foreach (KeyValuePair<String, Object> kv in dct)
                        {
                            fid = 0;

                            VwIndividual vi = (VwIndividual)kv.Value;
                            RefIndividual iobj = db.RefIndividuals.FirstOrDefault(obj => obj.individual_id == vi.id && obj.ref_id == rid.Value);
                            if (iobj != null) fid = iobj.id;

                            rslt = db.p_write_ref_individual(ref fid, rid.Value, rcorg, vi.id, eml1, eml2, auth.UserName);
                        }

                    ok = true;
                    this.ViewState["id"] = rid.Value.ToString();

                    VwReferral objref = db.VwReferrals.FirstOrDefault(obj => obj.id == rid.Value);
                    if (objref != null) this.lbl_reference_no.Text = objref.referral_no;
                }
            }
            catch (Exception xcp)
            {
                this.SetMessage(String.Format("!!! Error : {0}", xcp.Message));
            }

            if (ok)
            {
                String emsg = this.GetNotificationMessage();
                this.SendReferralNotification(emsg);
            }
        }

        private Boolean IsValidated()
        {
            Boolean b = true;

            if (this.emailuser.SelectedValue.Equals("0"))
            {
                b = false;
                this.SetMessage("!!! Error : Select notification recipient in referral organisation");
            }
            else
                if (!String.IsNullOrEmpty(this.copyemail.Text))
            {
                if (!IncUtilxs.IsValidEmailFormat(this.copyemail.Text))
                {
                    b = false;
                    this.SetMessage("!!! Error : Copy email field has an invalid email address");
                }
            }

            if (b)
                if (String.IsNullOrEmpty(this.message.Text))
                {
                    b = false;
                    this.SetMessage("!!! Error : Include a short introductory message to notification recipients");
                }
                else
                    if (this.message.Text.Trim().Length == 0)
                {
                    b = false;
                    this.SetMessage("!!! Error : Include a short introductory message to notification recipients");
                }
                else
                        if (String.IsNullOrEmpty(this.subject.Text))
                {
                    b = false;
                    this.SetMessage("!!! Error : Email notification's subject required");
                }

            if (!b) this.is_error.Value = "1";

            if (b)
            {
                Dictionary<String, Object> dct = this.GetDictSessCachedObjects(CACHENAME, this.ViewState["instance_id"].ToString());
                if (dct == null) b = false;
                else
                    if (dct.Count == 0) b = false;

                if (!b)
                    this.SetMessage("!!! Error : No individual(s) choosen for referral");
                else
                    this.SetMessage("Referral saved and email notification sent successfully");
            }

            return b;
        }

        private void NewSearch(object sender, EventArgs e)
        {
            this.ClearSearchFields();
        }

        private void NewReferral(object sender, EventArgs e)
        {
            this.ClearFields();
        }

        private void ClearFields()
        {
            this.ViewState["id"] = null;
            this.ViewState["emailuser"] = "0";
            this.ViewState["emailuser2"] = "0";
            this.ViewState["rec_org_unit"] = "0";

            this.copyemail.Text = String.Empty;
            this.message.Text = String.Empty;
            this.lbl_reference_no.Text = String.Empty;

            this.ViewState["id"] = null;
            this.ViewState["sql"] = null;
            this.ViewState["query"] = null;

            this.RemoveAllCachedObjects(CACHENAME, this.ViewState["instance_id"].ToString());

            this.ViewState["instance_id"] = this.NewInstanceID();
            this.ClearSearchFields();
        }

        protected void RemoveAllCachedObjects(String dctname, String sinstance)
        {
            Dictionary<String, Object> dct = null;

            if (this.Session[dctname] != null) dct = (Dictionary<String, Object>)this.Session[dctname];
            if (dct != null)
                if (dct.Keys.Contains(sinstance)) dct.Remove(sinstance);
        }

        private void ClearSearchFields()
        {
            this.ViewState["drp_srch1"] = null;
            this.ViewState["drp_srch2"] = null;
            this.ViewState["drp_srch3"] = null;
            this.ViewState["drp_srch4"] = null;
        }

        private String GetNotificationMessage()
        {
            String shtml = String.Empty;

            Dictionary<String, Object> dct = this.GetDictSessCachedObjects(CACHENAME, this.ViewState["instance_id"].ToString());
            if (dct.Count == 0) return shtml;

            String prmid = IncUtilxs.GetParameterName("id");
            String prmindvid = IncUtilxs.GetParameterName("individual_id");
            String sappttl = IncUtilxs.GetDictionaryKeyVal("asp_web_title");
            Boolean toggle = true;

            shtml += "<!DOCTYPE html>";
            shtml += "<html>";
            shtml += "<head>";
            shtml += "<title>PIMSZ - Referrals</title>";
            shtml += "<style type=\"text/css\">";
            shtml += "body { font-family: Arial, Helvetica, sans-serif; font-size: 10px }";
            shtml += "#tbx { width: 900px; border: 1px solid #CCCCCC; margin-top: 10px }";
            shtml += "#tbx tr th { padding: 5px 0px; text-align: center; background-color: #003399; color: #FFFFFF }";
            shtml += "#tbx tr td { text-align: left; vertical-align: top }";
            shtml += ".col1 { width: 160px }";
            shtml += ".col2 { width: 160px }";
            shtml += ".col3 { width: 160px }";
            shtml += ".col4 { width: 60px }";
            shtml += ".col6 { width: 140px }";
            shtml += ".msg { font-size: 12px }";
            shtml += ".ttl { text-decoration: underline }";
            shtml += ".lbl { font-size: 11px; font-weight: bold }";
            shtml += ".dtrw { background-color: #FFFFCC }";
            shtml += "</style>";
            shtml += "</head>";
            shtml += "<body>";
            shtml += String.Format("<p class=\"msg\">{0}</p>", IncUtilxs.ConvertToHtml(this.message.Text));
            shtml += "<br /><br />";
            shtml += "<span class=\"lbl\">Individuals Referred</span><br />";
            shtml += String.Format("<span class=\"lbl\">Reference No.</span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a href=\"{0}{1}?{2}={3}\">{4}</a><br />", this.frnt_uri.Text, this.Request.Url.AbsolutePath, prmid, this.ViewState["id"].ToString(), this.lbl_reference_no.Text);
            shtml += "<table id=\"tbx\">";
            shtml += "<tr>";
            shtml += "<th class=\"col1\">Case No.</th><th class=\"col2\">First Name(s)</th><th class=\"col3\">Last Name</th><th class=\"col4\">Gender</th><th>Incident</th><th class=\"col6\">Status</th>";
            shtml += "</tr>";
            foreach (KeyValuePair<String, Object> kv in dct)
            {
                VwIndividual vi = (VwIndividual)kv.Value;

                if (toggle) shtml += "<tr>";
                else shtml += "<tr class=\"dtrw\">";
                shtml += String.Format("<td><a href=\"{0}/case.aspx?{1}={2}&{3}={4}#affected_groups\">{5}</a></td>", this.frnt_uri.Text, prmid, vi.case_id.ToString(), prmindvid, vi.id.ToString(), vi.case_no);
                shtml += String.Format("<td><a href=\"{0}/case.aspx?{1}={2}&{3}={4}#affected_groups\">{5}</a></td>", this.frnt_uri.Text, prmid, vi.case_id.ToString(), prmindvid, vi.id.ToString(), vi.fnames);
                shtml += String.Format("<td><a href=\"{0}/case.aspx?{1}={2}&{3}={4}#affected_groups\">{5}</a></td>", this.frnt_uri.Text, prmid, vi.case_id.ToString(), prmindvid, vi.id.ToString(), vi.lname);
                shtml += String.Format("<td>{0}</td>", vi.gender);
                shtml += String.Format("<td>{0}</td>", vi.incident_type);
                shtml += String.Format("<td>{0}</td>", vi.idisplacement_status);
                shtml += "</tr>";

                toggle = !toggle;
            }
            shtml += "</table>";
            shtml += "<br /><br />";
            shtml += String.Format("<p class=\"ttl\">{0}</p>", sappttl);
            shtml += "<br />";
            shtml += "</body>";
            shtml += "</html>";

            return shtml;
        }

        private void SendReferralNotification(String emlmsg)
        {
            String emlattribs = IncUtilxs.GetDictionaryKeyVal("emailing_attributes");
            String[] attriblst = emlattribs.Split(';');

            List<String> emlto = this.GetEmailAddresses();

            if (emlto.Count == 0)
            {
                this.SetMessage("!!! Error : Failed to resolve recipients email addresses");
                return;
            }

            try
            {
                MailMessage eml = new MailMessage();

                eml.Subject = this.subject.Text;
                foreach (String e in emlto)
                    eml.To.Add(new MailAddress(e));
                if (!String.IsNullOrEmpty(this.copyemail.Text))
                    eml.CC.Add(new MailAddress(this.copyemail.Text));
                eml.IsBodyHtml = true;
                eml.Body = emlmsg;
                eml.From = new MailAddress(attriblst[3]);

                SmtpClient client = new SmtpClient(attriblst[0]);
                client.Credentials = new System.Net.NetworkCredential(attriblst[2], attriblst[1]);
                client.Send(eml);

                this.SetMessage("Referral saved successfully and email notification sent to referral organisation");
            }
            catch (Exception xcp)
            {
                this.SetMessage(String.Format("!!! Error : {0}", xcp.Message));
            }
        }

        private List<String> GetEmailAddresses()
        {
            List<String> lst = new List<string>();

            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
            {
                IncUser usr = db.IncUsers.FirstOrDefault(obj => obj.id == Int16.Parse(this.ViewState["emailuser"].ToString()));
                if (usr != null) lst.Add(usr.email);

                if (!this.ViewState["emailuser2"].ToString().Equals("0"))
                {
                    usr = db.IncUsers.FirstOrDefault(obj => obj.id == Int16.Parse(this.ViewState["emailuser2"].ToString()));
                    if (usr != null) lst.Add(usr.email);
                }
            }

            return lst;
        }

        protected void fndbttn_Click(object sender, EventArgs e)
        {
            String whr = this.GetSearchCriteria();

            this.ViewState["sql"] = String.Format("select a.* from v_individuals a where ({0})", whr);
            this.ViewState["query"] = String.Format("select a.* from v_referral_search a where ({0})", whr);
        }

        private void ReSetDropDownValue(DropDownList drp, String cid)
        {
            String vl = IncUtilxs.GetRequestParamValue(this.Request.Params, cid);

            if (!String.IsNullOrEmpty(vl))
                if (drp.Items.FindByValue(vl) != null) drp.SelectedValue = vl;
        }

        private void ReSetSearchFieldsDrop(DropDownList drp, String cid)
        {
            String vl = String.Empty;

            if (this.ViewState[cid] != null)
            {
                vl = this.ViewState[cid].ToString();

                if (!String.IsNullOrEmpty(vl))
                    if (drp.Items.FindByValue(vl) != null) drp.SelectedValue = vl;
            }
        }

        private void ReSetTextValue(TextBox txt, String cid)
        {
            String vl = IncUtilxs.GetRequestParamValue(this.Request.Params, cid);
            if (!String.IsNullOrEmpty(vl)) txt.Text = vl;
        }

        protected void lstsrch_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("srchselected"))
                this.CacheIndividuals(e.CommandArgument.ToString());
        }

        protected void lstrefs_ItemCommand(object sender, ListViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("refselected"))
                this.RemovedIndividuals(e.CommandArgument.ToString());
        }

        protected void btnok_Click(object sender, EventArgs e)
        {
            this.WriteToDb();
        }

        private String NewInstanceID()
        {
            return String.Format(FMTINSTANCEMASK, DateTime.Now.ToString("dd-MM-yyyy:H:mm:ss"));
        }

        private void GrabIndivsToCache(String rid)
        {
            this.RemoveAllCachedObjects(CACHENAME, this.ViewState["instance_id"].ToString());

            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
            {
                List<Int32> inums = new List<int>();
                foreach (VwReferral robj in db.VwReferrals.Where(obj => obj.id == Int32.Parse(rid)).ToList()) inums.Add(robj.individual_id);

                if (inums.Count != 0)
                    foreach (VwIndividual indv in db.VwIndividuals.Where(obj => inums.Contains(obj.id)).ToList())
                        this.SessCacheObject(indv, CACHENAME, this.ViewState["instance_id"].ToString(), indv.id.ToString());
            }
        }

        protected void lnq_rfsrchsrch_Selecting(object sender, LinqDataSourceSelectEventArgs e)
        {
            List<VwReferralSearch> lst = null;

            String q = "set dateformat dmy; select a.* from v_referral_search a where a.id = 0";

            if (this.ViewState["query"] != null) q = String.Format("set dateformat dmy; {0}", this.ViewState["query"].ToString());

            using (IncDataClassesDataContext db = new IncDataClassesDataContext()) lst = db.ExecuteQuery<VwReferralSearch>(q).Take(100).ToList();

            if (lst == null) lst = new List<VwReferralSearch>();

            e.Result = lst;
        }

        private Boolean IsReferral()
        {
            String refvl = IncUtilxs.GetDictionaryKeyVal("referral");
            String smode = IncUtilxs.GetRequestParamValue(this.Request.Params, "drp_refmodes");
            Boolean b = false;

            if (String.IsNullOrEmpty(smode))
                b = true;
            else
                b = refvl.Equals(smode);

            return b;
        }

        private void GenerateCSV(object sender, EventArgs e)
        {
            this.WriteToCSV();
        }

        private void WriteToCSV()
        {

            List<List<String>> data = new List<List<string>>();
            List<String> cols = new List<string>();

            List<String> slct = new List<string>(), frm = new List<string>(), whr = new List<string>(), ordr = new List<string>();
            String sql = String.Empty, vl = String.Empty;
            String csvfl = IncUtilxs.GetDictionaryKeyVal("referral_csv_file");

            List<String> dts = new List<string>() { "Date Referral Made", "Date Case Reported" };
            List<String> nums = new List<string>() { "Age" };

            slct.Add("a.referral_no as [Referral Ref. No.]");
            slct.Add("a.ref_date as [Date Referral Made]");
            slct.Add("a.referred_by as [Referred By]");
            slct.Add("a.referred_to as [Referred To]");
            slct.Add("a.fnames as [First Names]");
            slct.Add("a.mdname as [Middle Name]");
            slct.Add("a.lname as [Last Name]");
            slct.Add("a.gender as Gender");
            slct.Add("a.physical_address as [Physical Address]");
            slct.Add("a.contact_phone as [Contact Phone]");
            slct.Add("a.email as Email");
            slct.Add("a.age as Age");
            slct.Add("a.remarks as Remarks");
            slct.Add("a.case_no as [Case No.]");
            slct.Add("a.date_reported as [Date Case Reported]");
            slct.Add("a.incident_type as Incident");
            slct.Add("a.ext_displacement_status as [Displacement Status]");
            slct.Add("a.ext_type_vulnerability as [Vulnerability Level]");

            frm.Add("v_referral_search a");

            vl = this.GetCriteriaExpression(this.drp_srch1, "1", "a"); if (!String.IsNullOrEmpty(vl)) whr.Add(vl);
            vl = this.GetCriteriaExpression(this.drp_srch2, "2", "a"); if (!String.IsNullOrEmpty(vl)) whr.Add(vl);
            vl = this.GetCriteriaExpression(this.drp_srch3, "3", "a"); if (!String.IsNullOrEmpty(vl)) whr.Add(vl);
            vl = this.GetCriteriaExpression(this.drp_srch4, "4", "a"); if (!String.IsNullOrEmpty(vl)) whr.Add(vl);

            if (whr.Count == 0) whr.Add("a.id = 0");

            ordr.Add("a.ref_date"); ordr.Add("a.fnames");

            sql = String.Format("set dateformat dmy; select {0}", String.Join(", ", slct.ToArray()));
            sql += String.Format(" from {0}", String.Join(" ", frm.ToArray()));
            if (whr.Count != 0) sql += String.Format(" where {0}", String.Join(" and ", whr.ToArray()));
            if (ordr.Count != 0) sql += String.Format(" order by {0}", String.Join(", ", ordr.ToArray()));

            using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["incAdoNet"].ConnectionString))
            {
                cnn.Open();

                SqlCommand cmd = new SqlCommand(sql, cnn);
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    List<String> rw = new List<string>();

                    if (cols.Count == 0) for (Int16 i = 0; i < rdr.FieldCount; i++) cols.Add(rdr.GetName(i));

                    for (Int16 i = 0; i < rdr.FieldCount; i++)
                        if (dts.Contains(rdr.GetName(i)))
                            rw.Add(DateTime.Parse(rdr[i].ToString()).ToString("dd/MM/yyyy"));
                        else
                            if (nums.Contains(rdr.GetName(i)))
                            rw.Add(IncUtilxs.SilenceZero(rdr[i].ToString()));
                        else
                                if (rdr[i] != null)
                            rw.Add(rdr[i].ToString());
                        else
                            rw.Add(String.Empty);

                    data.Add(rw);
                }

                rdr.Close();
            }

            if (cols.Count == 0 || data.Count == 0)
            {
                this.SetMessage("!!! No data. Your query yielded no data");
                return;
            }

            IncUtilxs.DumpCSV(this.Response, data, cols, csvfl);
        }
    }
}