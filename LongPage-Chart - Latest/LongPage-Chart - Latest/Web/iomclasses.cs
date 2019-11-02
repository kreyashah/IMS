using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Net.Mail;
using System.Data.SqlClient;
using System.Configuration;
using System.Text.RegularExpressions;
using iom;
using inc;

namespace iom
{    
    public class PageBase : System.Web.UI.Page
    {
        protected override void OnLoad(EventArgs e)
        {           
            base.OnLoad(e);            
        }

        internal void IsCaseRecord()
        {
            throw new NotImplementedException();
        }

        internal void DoesCaseExists()
        {
            throw new NotImplementedException();
        }

        protected void SetContextLabel(String s)
        {
            if (this.Master != null)
            {
                Label lbl = (Label)this.Master.FindControl("lbl_context");
                if (lbl != null) lbl.Text = s;
            }
        }

        protected void SetMessage(String s)
        {
            if (this.Master != null)
            {
                Label lbl = (Label)this.Master.FindControl("lbl_msg");
                if (lbl != null) lbl.Text = s;
            }
        }

        protected void IsActionPermitted(Object csid, String msg, Actions act)
        {
            CaseRecord csrw = null;

            if (this.CheckCase(csid))
                csrw = this.GetCachedCase(csid);
            else
                this.NotAllowed(msg, "Permission Denied !");            
        
            IncAuthentication auth = (IncAuthentication)this.Session["incuser"];
            if (!auth.IsActionAllowed(Int16.Parse(csrw.CaseRow["org_unit_id"].ToString()), act))
                this.NotAllowed(msg, "Permission Denied !");
        }

        protected void NotAllowed(String msg, String cntxt)
        {
            IncPrompt prmpt = new IncPrompt();

            String js = String.Format("javascript: document.location.href='{0}';", this.Request.Url.PathAndQuery);
            prmpt.MessageButtons.Add(new MessageButton() { ButtonLabel = "OK", ButtonName = "btnok", JScriptCommand = js });

            prmpt.PromptItems.Add("message", msg);
            prmpt.PromptItems.Add("context", cntxt);

            this.Session["prompt"] = prmpt;
            this.Response.Redirect("prompt.aspx", true);
        }

        

        protected Boolean IsPermittedOrgUnit(Int16 org, String objkwd)
        {
            Boolean ok = false;

            IncAuthentication auth = (IncAuthentication)this.Session["incuser"];
            ok = auth.CanReadOrgUnit(org, objkwd);

            return ok;
        }

        protected String GetClientJsUrl(String hdnfld)
        {
            String jsurl = String.Empty;

            jsurl = IncUtilxs.GetRequestParamValue(this.Request.Params, hdnfld);

            if (String.IsNullOrEmpty(jsurl)) jsurl = this.Request.Url.AbsoluteUri.ToString();

            return jsurl;
        }

        private String GetMakeCaseUID(Object cid)
        {
            Regex rgxp = new Regex("^\\d+$");
            String sfmt = "CaS-{0}";
            String s = String.Empty;
            String cno = String.Empty;

            if (cid != null)
                if (rgxp.IsMatch(cid.ToString())) cno = cid.ToString();

            if (!String.IsNullOrEmpty(cno)) s = String.Format(sfmt, cno);
            else s = String.Format(sfmt, "0");

            return s;
        }

        protected Boolean CheckCase(Object csid)
        {
            Regex rgxp = new Regex("^\\d+$");
            Boolean b = false;

            if (csid != null)
                if (rgxp.IsMatch(csid.ToString()))
                    if (this.Session["cases_list"] != null)
                    {
                        Hashtable hsh = (Hashtable)this.Session["cases_list"];
                        String sky = this.GetMakeCaseUID(csid.ToString());
                        if (!String.IsNullOrEmpty(sky))
                            b = hsh.ContainsKey(sky);
                    }

            return b;
        }

        protected String GetCaseNo(Object csid)
        {
            Regex rgxp = new Regex("^\\d+$");
            String s = String.Empty;

            if (csid != null)
                if (rgxp.IsMatch(csid.ToString()))
                {
                    Hashtable hsh = (Hashtable)this.Session["cases_list"];
                    String ky = this.GetMakeCaseUID(csid);
                    if (hsh.ContainsKey(ky))
                    {
                        CaseRecord csrcd = (CaseRecord)hsh[ky];
                        if (csrcd.CaseRow["case_no"] != null) s = csrcd.CaseRow["case_no"].ToString();
                    }
                }

            return s;
        }

        protected void DoesCaseExists(Object csid)
        {
            if (!this.CheckCase(csid))
                this.Response.Redirect("case.aspx");
        }

        protected void SetCacheCase(CaseRecord csrcd)
        {
            const Byte MAXLIST = 10;            
         
            if (this.Session["cases_list"] == null) this.Session["cases_list"] = new Hashtable();

            Hashtable hsh = (Hashtable)this.Session["cases_list"];
            String csky = this.GetMakeCaseUID(csrcd.CaseRow["id"].ToString());

            if (hsh.Count > (MAXLIST - 1)) hsh.Remove(hsh[hsh.Count - 1]);

            if (hsh.ContainsKey(csky)) hsh[csky] = csrcd;
            else hsh.Add(csky, csrcd);
        }

        protected CaseRecord GetCachedCase(Object cid)
        {
            CaseRecord csrcd = null;            

            String csky = this.GetMakeCaseUID(cid);

            if (this.Session["cases_list"] != null)
            {
                Hashtable hsh = (Hashtable)this.Session["cases_list"];
                if (hsh.ContainsKey(csky)) csrcd = (CaseRecord)hsh[csky];
            }

            return csrcd;
        }

        protected void DeleteCachedCase(Object csid)
        {
            Regex rgxp = new Regex("^\\d+$");

            if (csid != null)
                if (this.CheckCase(csid))
                {
                    Hashtable hsh = (Hashtable)this.Session["cases_list"];
                    String sky = this.GetMakeCaseUID(csid);
                    if (hsh.ContainsKey(sky)) hsh.Remove(sky);
                }
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

        protected void RemoveAllCachedObjects(String dctname, String sinstance)
        {
            Dictionary<String, Object> dct = null;            

            if (this.Session[dctname] != null) dct = (Dictionary<String, Object>)this.Session[dctname];
            if (dct != null)
                if (dct.Keys.Contains(sinstance)) dct.Remove(sinstance);                                                    
        }

        protected Object GetSessCachedObject(String dctname, String sinstance, String skey)
        {            
            Dictionary<String, Object> dct = null;
            Dictionary<String, Object> instdct = null;

            Object obj = null;

            if (this.Session[dctname] != null) dct = (Dictionary<String, Object>)this.Session[dctname];
            if (dct != null)
                if (dct.Keys.Contains(sinstance))                    
                {
                    instdct = (Dictionary<String, Object>)dct[sinstance];
                    if (instdct.Keys.Contains(skey)) obj = instdct[skey];
                }

            return obj;
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

        protected void DoSearch(object sender, EventArgs e)
        {
            String vl = IncUtilxs.GetRequestParamValue(this.Request.Params, "drp_search");

            if (String.IsNullOrEmpty(vl)) return;
            if (vl.Equals("0")) return;

            this.Response.Redirect("~/" + vl);
        }

        protected void PutSearchDropList(PlaceHolder plchldr)
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

        protected String GetClientUrlAuthority(String hdnfld)
        {
            String s = String.Empty;
            String surl = this.GetClientJsUrl(hdnfld);

            Uri ubj = new Uri(surl);
            s = ubj.GetLeftPart(UriPartial.Authority);

            return s;
        }
    }

    public class MasterPageBase : System.Web.UI.MasterPage
    {
        protected override void OnLoad(EventArgs e)
        {            
            base.OnLoad(e);            
        }

        protected void SetContextLabel(String s)
        {
            if (this.Master != null)
            {
                Label lbl = (Label)this.Master.FindControl("lbl_context");
                if (lbl != null) lbl.Text = s;
            }
        }

        protected void SetMessage(String s)
        {
            if (this.Master != null)
            {
                Label lbl = (Label)this.Master.FindControl("lbl_msg");
                if (lbl != null) lbl.Text = s;
            }
        }

        protected void IsActionPermitted(String csid, String msg, Actions act)
        {
            CaseRecord csrw = null;

            if (this.CheckCase(csid))
                csrw = this.GetCachedCase(csid);
            else
                this.NotAllowed(msg, "Permission Denied !");

            IncAuthentication auth = (IncAuthentication)this.Session["incuser"];
            if (!auth.IsActionAllowed(Int16.Parse(csrw.CaseRow["org_unit_id"].ToString()), act))
                this.NotAllowed(msg, "Permission Denied !");
        }

        protected void NotAllowed(String msg, String cntxt)
        {
            IncPrompt prmpt = new IncPrompt();

            String js = String.Format("javascript: document.location.href='{0}';", this.Request.Url.PathAndQuery);
            prmpt.MessageButtons.Add(new MessageButton() { ButtonLabel = "OK", ButtonName = "btnok", JScriptCommand = js });

            prmpt.PromptItems.Add("message", msg);
            prmpt.PromptItems.Add("context", cntxt);

            this.Session["prompt"] = prmpt;
            this.Response.Redirect("prompt.aspx", true);
        }

        protected String GetClientJsUrl(String hdnfld)
        {
            String jsurl = String.Empty;

            jsurl = IncUtilxs.GetRequestParamValue(this.Request.Params, hdnfld);
            
            if (String.IsNullOrEmpty(jsurl)) jsurl = this.Request.Url.AbsoluteUri.ToString();

            return jsurl;
        }

        private String GetMakeCaseUID(Object cid)
        {
            Regex rgxp = new Regex("^\\d+$");
            String sfmt = "CaS-{0}";
            String s = String.Empty;
            String cno = String.Empty;

            if (cid != null)
                if (rgxp.IsMatch(cid.ToString())) cno = cid.ToString();

            if (!String.IsNullOrEmpty(cno)) s = String.Format(sfmt, cno);
            else s = String.Format(sfmt, "0");

            return s;
        }

        private Boolean CheckCase(Object csid)
        {
            Regex rgxp = new Regex("^\\d+$");
            Boolean b = false;

            if (csid != null)
                if (rgxp.IsMatch(csid.ToString()))
                    if (this.Session["cases_list"] != null)
                    {
                        Hashtable hsh = (Hashtable)this.Session["cases_list"];
                        String sky = this.GetMakeCaseUID(csid.ToString());
                        if (!String.IsNullOrEmpty(sky))
                            b = hsh.ContainsKey(sky);
                    }

            return b;
        }

        protected String GetCaseNo(Object csid)
        {
            Regex rgxp = new Regex("^\\d+$");
            String s = String.Empty;

            if (csid != null)
                if (rgxp.IsMatch(csid.ToString()))
                {
                    Hashtable hsh = (Hashtable)this.Session["cases_list"];
                    String ky = this.GetMakeCaseUID(csid);
                    if (hsh.ContainsKey(ky))
                    {
                        CaseRecord csrcd = (CaseRecord)hsh[ky];
                        if (csrcd.CaseRow["case_no"] != null) s = csrcd.CaseRow["case_no"].ToString();
                    }
                }

            return s;
        }

        protected void DoesCaseExists(Object csid)
        {
            if (!this.CheckCase(csid))
                this.Response.Redirect("case.aspx");
        }

        protected void SetCacheCase(CaseRecord csrcd)
        {
            const Byte MAXLIST = 10;

            if (this.Session["cases_list"] == null) this.Session["cases_list"] = new Hashtable();

            Hashtable hsh = (Hashtable)this.Session["cases_list"];
            String csky = this.GetMakeCaseUID(csrcd.CaseRow["id"].ToString());

            if (hsh.Count > (MAXLIST - 1)) hsh.Remove(hsh[hsh.Count - 1]);

            if (hsh.ContainsKey(csky)) hsh[csky] = csrcd;
            else hsh.Add(csky, csrcd);
        }

        protected CaseRecord GetCachedCase(Object cid)
        {
            CaseRecord csrcd = null;

            String csky = this.GetMakeCaseUID(cid);

            if (this.Session["cases_list"] != null)
            {
                Hashtable hsh = (Hashtable)this.Session["cases_list"];
                if (hsh.ContainsKey(csky)) csrcd = (CaseRecord)hsh[csky];
            }

            return csrcd;
        }

        protected void DeleteCachedCase(Object csid)
        {
            Regex rgxp = new Regex("^\\d+$");

            if (csid != null)
                if (this.CheckCase(csid))
                {
                    Hashtable hsh = (Hashtable)this.Session["cases_list"];
                    String sky = this.GetMakeCaseUID(csid);
                    if (hsh.ContainsKey(sky)) hsh.Remove(sky);
                }
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

        protected void RemoveAllCachedObjects(String dctname, String sinstance)
        {
            Dictionary<String, Object> dct = null;

            if (this.Session[dctname] != null) dct = (Dictionary<String, Object>)this.Session[dctname];
            if (dct != null)
                if (dct.Keys.Contains(sinstance)) dct.Remove(sinstance);
        }

        protected Object GetSessCachedObject(String dctname, String sinstance, String skey)
        {
            Dictionary<String, Object> dct = null;
            Dictionary<String, Object> instdct = null;

            Object obj = null;

            if (this.Session[dctname] != null) dct = (Dictionary<String, Object>)this.Session[dctname];
            if (dct != null)
                if (dct.Keys.Contains(sinstance))
                {
                    instdct = (Dictionary<String, Object>)dct[sinstance];
                    if (instdct.Keys.Contains(skey)) obj = instdct[skey];
                }

            return obj;
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

        protected void DoSearch(object sender, EventArgs e)
        {
            String vl = IncUtilxs.GetRequestParamValue(this.Request.Params, "drp_search");

            if (String.IsNullOrEmpty(vl)) return;
            if (vl.Equals("0")) return;

            this.Response.Redirect("~/" + vl);
        }

        protected void PutSearchDropList(PlaceHolder plchldr)
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

        protected String GetClientUrlAuthority(String hdnfld)
        {
            String s = String.Empty;
            String surl = this.GetClientJsUrl(hdnfld);

            Uri ubj = new Uri(surl);
            s = ubj.GetLeftPart(UriPartial.Authority);

            return s;
        }
    }
}

namespace inc
{
    public static partial class IncUtilxs
    {
        public static void FillOrgDropDown(DropDownList drpdwn, Int16 i)
        {
            drpdwn.Items.Clear();
            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
                foreach (VwOrgUnit org in db.VwOrgUnits.ToList())
                    drpdwn.Items.Add(new ListItem(org.alias, org.id.ToString()));

            if (drpdwn.Items.FindByValue(i.ToString()) != null)
                drpdwn.SelectedValue = i.ToString();
        }

        public static void FillOrgDropDown(DropDownList drpdwn, List<Int16> usrorgs, Int16 i)
        {
            drpdwn.Items.Clear();
            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
                foreach (VwOrgUnit org in db.VwOrgUnits.ToList())
                    if (usrorgs.Contains(org.id))
                        drpdwn.Items.Add(new ListItem(org.alias, org.id.ToString()));

            if (drpdwn.Items.FindByValue(i.ToString()) != null)
                drpdwn.SelectedValue = i.ToString();
        }

        public static void FillIncidentDropDown(DropDownList drpdwn, Int16 i)
        {
            drpdwn.Items.Clear();
            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
                foreach (VwTypeIncident inc in db.VwTypeIncidents.ToList())
                    drpdwn.Items.Add(new ListItem(inc.incident_type, inc.id.ToString()));

            if (drpdwn.Items.FindByValue(i.ToString()) != null) drpdwn.SelectedValue = i.ToString();
        }

        public static void FillDisplacementDropDown(DropDownList drpdwn, Int16 i)
        {
            drpdwn.Items.Clear();
            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
                foreach (TypeDisplacement dis in db.TypeDisplacements.ToList())
                    drpdwn.Items.Add(new ListItem(dis.displacement_status, dis.id.ToString()));

            if (drpdwn.Items.FindByValue(i.ToString()) != null) drpdwn.SelectedValue = i.ToString();
        }

        public static void PopulateNavigator(Dictionary<String, String> mnus, PlaceHolder plchldr, String btnnm)
        {
            Button btn;            
            DropDownList drpdwn;

            plchldr.Controls.Clear();

            String nvgtrid = IncUtilxs.GetDictionaryKeyVal("navigator_id");

            drpdwn = new DropDownList();
            drpdwn.ID = nvgtrid;
            drpdwn.CssClass = "nvmnu";
            drpdwn.ClientIDMode = ClientIDMode.Static;
            drpdwn.Attributes.CssStyle.Add(HtmlTextWriterStyle.MarginRight, "5px");

            foreach (KeyValuePair<String, String> kvp in mnus)
                drpdwn.Items.Add(new ListItem(kvp.Key, kvp.Value));

            plchldr.Controls.Add(drpdwn);

            btn = new Button();
            btn.ID = btnnm;
            btn.CssClass = "smlbttns";
            btn.Text = "GO";
            btn.CausesValidation = false;
            plchldr.Controls.Add(btn);
        }

        public static void DispatchEmail(List<String> rcpnts, String msg, String sbjct, List<String> rplyt)
        {            
            String s = IncUtilxs.GetDictionaryKeyVal("emailing_attributes");
            String[] attribs = s.Split(';');

            MailMessage mlmsg = new MailMessage();
            mlmsg.From = new MailAddress(attribs[3]);
            foreach (String t in rcpnts) mlmsg.To.Add(t);
            mlmsg.Subject = sbjct;
            mlmsg.IsBodyHtml = true;
            mlmsg.Body = msg;
            
            if (rplyt.Count != 0)
                foreach (String r in rplyt) mlmsg.ReplyToList.Add(r);

            SmtpClient clnt = new SmtpClient(attribs[0]);
            
            clnt.Credentials = new NetworkCredential(attribs[2], attribs[1]);
            clnt.Send(mlmsg);
        }

        public static void InitDownloadColumns(ref Dictionary<string, String> cols, String clsnm)
        {
            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
            {
                List<CsvOrder> rwo = db.CsvOrders.Where(obj => obj.class_name.Equals(clsnm)).ToList();                

                if (rwo != null)
                    foreach (CsvOrder ro in rwo)
                    {
                        String ky = ro.field_name;
                        String vl = ro.field_name;

                        CsvHeader rw = db.CsvHeaders.FirstOrDefault(obj => obj.field_name.Equals(ky));
                        if (rw != null) vl = rw.field_label;

                        cols.Add(ky, vl);
                    }
            }
        }

        public static void GetLatLongItems(List<String> whr, ref Hashtable prms)
        {
            List<Dictionary<String, String>> pgitms = new List<Dictionary<string, string>>();        
            List<Dictionary<String, String>> itms = new List<Dictionary<string, string>>();
            String sql = String.Empty;

            sql = GetCoordSQL(whr);
            MakeLatLong(ref itms, sql);
            ProceessLatLong(ref itms);
            SetListPagination(itms, ref pgitms, ref prms);            

            prms.Add("data_rows", pgitms);
            prms.Add("row_count", itms.Count.ToString());                        
        }

        private static void MakeLatLong(ref List<Dictionary<String, String>> itms, String sql)
        {            
            using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["incAdoNet"].ConnectionString))
            {
                cnn.Open();

                SqlCommand cmd = new SqlCommand(sql, cnn);

                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Dictionary<String, String> dct = new Dictionary<string, string>();

                    for (Int16 i = 0; i < rdr.FieldCount; i++)
                        dct.Add(rdr.GetName(i), rdr.GetValue(i).ToString());

                    itms.Add(dct);
                }                    
            }            
        }

        private static void ProceessLatLong(ref List<Dictionary<String, String>> lst)
        {
            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
            {
                foreach (Dictionary<String, String> dct in lst)
                {
                    String sql = String.Empty;
                    Boolean found = false;

                    if (!String.IsNullOrEmpty(dct["gps_east"]) && !String.IsNullOrEmpty(dct["gps_south"]))
                    {
                        found = true;

                        if (dct["gps_south"][0] != '-') dct["gps_south"] = "-" + dct["gps_south"];
                    }
                    
                    if (found) continue;
                    if (!String.IsNullOrEmpty(dct["place_name"]))
                    {
                        sql = String.Format("select top 1 * from t_zim_places where place like '{0}%'", dct["place_name"].Replace("'", "''"));
                        ZimPlace plc = db.ExecuteQuery<ZimPlace>(sql).FirstOrDefault();
                        if (plc != null)
                        {
                            dct["gps_south"] = plc.latitude;
                            dct["gps_east"] = plc.longitude;

                            found = true;
                        }
                    }

                    if (found) continue;
                    if (!String.IsNullOrEmpty(dct["district"]))
                    {
                        sql = String.Format("select top 1 * from t_zim_places where place like '{0}%'", dct["district"].Replace("'", "''"));
                        ZimPlace plc = db.ExecuteQuery<ZimPlace>(sql).FirstOrDefault();              
                        if (plc != null)
                        {
                            dct["gps_south"] = plc.latitude;
                            dct["gps_east"] = plc.longitude;

                            found = true;
                        }
                    }

                    if (found) continue;
                    if (!String.IsNullOrEmpty(dct["province"]))
                    {
                        sql = String.Format("select top 1 * from t_zim_places where place like '{0}%'", dct["province"].Replace("'", "''"));
                        ZimPlace plc = db.ExecuteQuery<ZimPlace>(sql).FirstOrDefault();
                        if (plc != null)
                        {
                            dct["gps_south"] = plc.latitude;
                            dct["gps_east"] = plc.longitude;
                        }
                    }
                }

                lst.RemoveAll(obj => String.IsNullOrEmpty(obj["gps_east"]) || String.IsNullOrEmpty(obj["gps_south"]));
            }
        }

        private static String GetCoordSQL(List<String> whr)
        {
            List<String> slct = new List<string>(); List<String> frm = new List<string>();            

            String sql = String.Empty;

            slct.Add("cs.id"); slct.Add("cs.date_reported"); slct.Add("cs.incident_date"); slct.Add("cs.ref_no"); slct.Add("cs.place_name");
            slct.Add("case when cs.is_verified=1 then 'YES' else 'NO' end as verified_remark"); slct.Add("cs.alias");  
            slct.Add("cs.num_individuals"); slct.Add("cs.num_households"); slct.Add("cs.num_communities");
            slct.Add("cs.case_no"); slct.Add("cs.province"); slct.Add("cs.district"); slct.Add("cs.ext_incident_type"); slct.Add("cs.gps_east");
            slct.Add("cs.gps_south"); slct.Add("cs.ext_displacement_status");            

            frm.Add("v_case_row cs");                                                
            frm.Add("left join v_primary_peps pp on pp.case_id = cs.id");
            frm.Add("left join v_primary_causes ca on ca.case_id = cs.id");

            sql = String.Format("set dateformat dmy; select distinct {0}", String.Join(", ", slct.ToArray()));
            sql += String.Format(" from {0}", String.Join(" ", frm.ToArray()));
            if (whr.Count != 0) sql += String.Format(" where ({0})", String.Join(" and ", whr.ToArray()));

            return sql;
        }

        public static List<Dictionary<String, String>> GetDataRows(String sql)
        {
            List<Dictionary<String, String>> data = null;

            using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["IncAdoNet"].ConnectionString))
            {
                cnn.Open();

                SqlCommand cmd = new SqlCommand(sql, cnn);
                SqlDataReader rdr = cmd.ExecuteReader();                
                while (rdr.Read())
                {
                    Dictionary<String, String> dct = new Dictionary<string, string>();

                    for (Int16 i = 0; i < rdr.FieldCount; i++)
                        dct.Add(rdr.GetName(i), rdr.GetValue(i) == null ? String.Empty : rdr.GetValue(i).ToString());

                    data.Add(dct);
                }
            }

            return data;
        }

        public static void SetListPagination(List<Dictionary<String, String>> lst, ref List<Dictionary<String, String>> pglst, ref Hashtable prms)
        {            
            String rwsppg = GetDictionaryKeyVal("map_rows");

            Int32 pg = Int32.Parse(prms["page"].ToString()), rws = Int32.Parse(rwsppg);
            Int32 skp = 0;

            prms["pages"] = 1;
            if (pg < 1) pg = 1;            

            if (lst.Count > rws)
            {
                Int32 nopgs = lst.Count / rws;
                Int32 rmdr = lst.Count % rws;

                if (rmdr != 0) ++nopgs;

                prms["pages"] = nopgs;

                if (pg > nopgs) pg = nopgs;

                prms["page"] = pg;

                skp = (pg - 1) * rws;
            }
            
            pglst = lst.Skip(skp).Take(rws).ToList();
        }

        public static String GetSelectHtml(List<Dictionary<String, String>> data, Dictionary<String, String> itms)
        {
            String s = String.Empty;
            String opt = String.Empty;

            foreach (Dictionary<String, String> dct in data)
                if (itms["selected"].Equals(dct["ky"]))
                    opt += String.Format("<option value=\"{0}\" selected=\"selected\">{1}</option>", dct["ky"], dct["vl"]);
                else
                    opt += String.Format("<option value=\"{0}\">{1}</option>", dct["ky"], dct["vl"]);

            if (itms.Keys.Contains("class"))
                s = String.Format("<select id=\"{0}\" name=\"{1}\" class=\"{2}\">{3}</select>", itms["id"], itms["name"], itms["class"], opt);
            else
                s = String.Format("<select id=\"{0}\" name=\"{1}\">{2}</select>", itms["id"], itms["name"], itms["class"]);

            return s;
        }
        
        public static Boolean IsUserAllowed(String usr, Actions act)
        {
            Boolean rslt = false;

            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
            {
                VwUserPermission upm = db.VwUserPermissions.FirstOrDefault(obj=>obj.usr_name.Equals(usr));
                if (upm != null)
                    switch (act)
                    {
                        case Actions.CanRead:
                            rslt = upm.can_read;
                            break;
                        case Actions.CanWrite:
                            rslt = upm.can_write;
                            break;
                        case Actions.CanDelete:
                            rslt = upm.can_delete;
                            break;                        
                    }
            }

            return rslt;
        }

        public static List<Dictionary<String, String>> DictKeyValString(String sin, Char itmsep, Char lstsep)
        {
            List<Dictionary<String, String>> itms = new List<Dictionary<string,string>>();

            List<String> lst = sin.Split(itmsep).ToList();
            if (lst.Count != 0)
                foreach (String s in lst)
                {
                    String[] dmp = s.Split(lstsep);
                    if (dmp.Length > 1)
                        if (!String.IsNullOrEmpty(dmp[0].Trim()) && !String.IsNullOrEmpty(dmp[1].Trim()))
                            itms.Add(new Dictionary<string, string>() { { "key", dmp[0].Trim() }, { "val", dmp[1].Trim() } });                                                    
                }

            return itms;
        }

        public static void FillDonorDropDown(DropDownList drpdwn, String sval)
        {
            drpdwn.Items.Clear();
            drpdwn.Items.Add(new ListItem("UnSpecified", "0"));

            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
                foreach (VwDonor dnr in db.VwDonors.ToList())
                    drpdwn.Items.Add(new ListItem(dnr.donor, dnr.id.ToString()));

            if (drpdwn.Items.FindByValue(sval) != null)
                drpdwn.SelectedValue = sval;
        }

        public static void FillProjectDropDown(DropDownList drpdwn, String dnr, String sval)
        {
            drpdwn.Items.Clear();
            drpdwn.Items.Add(new ListItem("UnSpecified", "0"));
            Int16 did = 0;
            Int16.TryParse(dnr, out did);

            using (IncDataClassesDataContext db = new IncDataClassesDataContext())                            
                foreach (VwProject p in db.VwProjects.Where(obj => obj.donor_id == did).ToList())
                    drpdwn.Items.Add(new ListItem(p.project_code, p.id.ToString()));            

            if (drpdwn.Items.FindByValue(sval) != null)
                drpdwn.SelectedValue = sval;
        }

        public static void FillProjectDropDown(DropDownList drpdwn, String sval)
        {
            drpdwn.Items.Clear();
            drpdwn.Items.Add(new ListItem("UnSpecified", "0"));            

            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
                foreach (VwProject p in db.VwProjects.OrderBy(obj => obj.project_code).ToList())
                    drpdwn.Items.Add(new ListItem(String.Format("{0}: {1}", p.project_code, p.project_name), p.id.ToString()));

            if (drpdwn.Items.FindByValue(sval) != null)
                drpdwn.SelectedValue = sval;
        }

        public static void FillProjectCodeDropDown(DropDownList drpdwn, String dnr, String sval)
        {
            drpdwn.Items.Clear();
            drpdwn.Items.Add(new ListItem("UnSpecified", "0"));
            Int16 did = 0;
            Int16.TryParse(dnr, out did);

            using (IncDataClassesDataContext db = new IncDataClassesDataContext())                            
                foreach (VwProject p in db.VwProjects.Where(obj => obj.donor_id == did).ToList())
                    drpdwn.Items.Add(new ListItem(p.project_code, p.project_code));            

            if (drpdwn.Items.FindByValue(sval) != null)
                drpdwn.SelectedValue = sval;
        }

        public static void FillAssistanceDropDown(DropDownList drpdwn, String sval)
        {
            drpdwn.Items.Clear();
            drpdwn.Items.Add(new ListItem("UnSpecified", "0"));

            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
                foreach (TypeAssistance ta in db.TypeAssistances.ToList())
                    if (drpdwn.Items.FindByValue(ta.id.ToString()) != null) continue;
                    else
                        drpdwn.Items.Add(new ListItem(ta.type_assistance, ta.id.ToString()));

            if (drpdwn.Items.FindByValue(sval) != null)
                drpdwn.SelectedValue = sval;
        }

        public static void FillAnswerTypes(DropDownList drpdwn, String svl)
        {
            drpdwn.Items.Clear();

            String ansopts = IncUtilxs.GetDictionaryKeyVal("answer_types");
            List<Dictionary<String, String>> lst = DictKeyValString(ansopts, ';', ',');

            foreach (Dictionary<String, String> dct in lst)
                drpdwn.Items.Add(new ListItem(dct["val"], dct["key"]));

            if (drpdwn.Items.FindByValue(svl) != null) drpdwn.SelectedValue = svl;
        }

        public static void FillOptionAnswerType(DropDownList drpdwn, String svl)
        {
            drpdwn.Items.Clear();

            String s = IncUtilxs.GetDictionaryKeyVal("option_types");
            List<Dictionary<String, String>> itms = IncUtilxs.DictKeyValString(s, ';', ',');

            if (itms.Count != 0)
                foreach (Dictionary<String, String> dct in itms)
                    drpdwn.Items.Add(new ListItem(dct["val"], dct["key"]));            

            if (drpdwn.Items.Count == 0) drpdwn.Items.Add(new ListItem("N/A", "0"));

            if (drpdwn.Items.FindByValue(svl) != null) drpdwn.SelectedValue = svl;
        }

        public static void FillGridDataType(DropDownList drpdwn, String svl)
        {
            drpdwn.Items.Clear();

            String s = IncUtilxs.GetDictionaryKeyVal("grid_data_types");
            List<Dictionary<String, String>> itms = IncUtilxs.DictKeyValString(s, ';', ',');

            if (itms.Count != 0)
                foreach (Dictionary<String, String> dct in itms)
                    drpdwn.Items.Add(new ListItem(dct["val"], dct["key"]));            

            if (drpdwn.Items.Count == 0) drpdwn.Items.Add(new ListItem("N/A", "0"));

            if (drpdwn.Items.FindByValue(svl) != null) drpdwn.SelectedValue = svl;
        }

        public static void NotifyCaseCommentItems(ref List<Dictionary<String, String>> cmnt, Int32 csid)
        {
            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
            {
                VwCaseRow csrw = db.VwCaseRows.FirstOrDefault(obj => obj.id == csid);
                if (csrw != null)
                {
                    cmnt.Add(new Dictionary<string, string>() { { "key", "Case No." }, { "val", csrw.case_no != null ? csrw.case_no : String.Empty } });
                    cmnt.Add(new Dictionary<string, string>() { { "key", "Incident" }, { "val", csrw.ext_incident_type != null ? csrw.ext_incident_type : String.Empty } });
                    cmnt.Add(new Dictionary<string, string>() { { "key", "Date Reported" }, { "val", csrw.date_reported.ToString("dd/MM/yyyy") } });
                    cmnt.Add(new Dictionary<string, string>() { { "key", "Province" }, { "val", csrw.province != null ? csrw.province : String.Empty } });
                    cmnt.Add(new Dictionary<string, string>() { { "key", "District" }, { "val", csrw.district != null ? csrw.district : String.Empty } });
                    cmnt.Add(new Dictionary<string, string>() { { "key", "Ward" }, { "val", IncUtilxs.SilenceZero(csrw.ward_no.ToString()) } });
                    cmnt.Add(new Dictionary<string, string>() { { "key", "Place name" }, { "val", csrw.place_name != null ? csrw.place_name : "UnKnown" } });
                    cmnt.Add(new Dictionary<string, string>() { { "key", "Individuals affected" }, { "val", csrw.num_individuals.HasValue ? IncUtilxs.SilenceZero(csrw.num_individuals.Value.ToString()) : String.Empty } });
                    cmnt.Add(new Dictionary<string, string>() { { "key", "Households affected" }, { "val", csrw.num_households.HasValue ? IncUtilxs.SilenceZero(csrw.num_households.Value.ToString()) : String.Empty } });
                    cmnt.Add(new Dictionary<string, string>() { { "key", "Communities affected" }, { "val", csrw.num_communities.HasValue ? IncUtilxs.SilenceZero(csrw.num_communities.Value.ToString()) : String.Empty } });
                }
            }
        }

        public static void MakeHtmlQuestion(ref Panel div, Int32 qid, Int32 aqid)
        {            
            Question qs = null;
            VwSurveyAnsweredQuestion svans = null;

            List<Dictionary<String, String>> lstopts = new List<Dictionary<string, string>>();
            List<Dictionary<String, String>> grdcols = new List<Dictionary<string, string>>();
            List<Dictionary<String, String>> grdrows = new List<Dictionary<string, string>>();
            List<Dictionary<String, String>> anscolrow = new List<Dictionary<string, string>>();

            Dictionary<String, String> dct = new Dictionary<string, string>();
            Dictionary<String, String> adct = null;

            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
            {
                foreach (IncDictionary rw in db.IncDictionaries.ToList()) dct.Add(rw.dict_key, rw.dict_value);

                qs = db.Questions.OrderBy(obj => obj.numbering).FirstOrDefault(obj => obj.id == qid && obj.enabled == true);
                if (qs == null) return;
                svans = db.VwSurveyAnsweredQuestions.FirstOrDefault(obj => obj.aq_id == aqid && obj.question_id == qid && obj.que_enabled == true && obj.section_enabled == true);

                foreach (QListOption opt in db.QListOptions.Where(obj => obj.question_id == qid).ToList())
                {
                    adct = new Dictionary<string, string>();
                    adct.Add("id", opt.id.ToString());
                    adct.Add("answer_option", opt.answer_option);
                    adct.Add("selected", false.ToString());
                    adct.Add("question_id", opt.question_id.ToString());                    
                    adct.Add("answer_id", String.Empty);
                    adct.Add("answer_txt", String.Empty);
                    adct.Add("is_other", false.ToString());
                    lstopts.Add(adct);
                }
                foreach (VwSurveyOptionAnswer svopt in db.VwSurveyOptionAnswers.Where(obj => obj.question_id == qid && obj.aq_id == aqid).ToList())
                    foreach (Dictionary<String, String> d in lstopts)
                        if (Int32.Parse(d["question_id"]) == svopt.question_id && svopt.answer_id == Int32.Parse(d["id"]))
                        {
                            d["selected"] = true.ToString();
                            d["answer_txt"] = !String.IsNullOrEmpty(svopt.answer_txt) ? svopt.answer_txt : String.Empty;
                        }

                if (Byte.Parse(dct["option_list_answer"]) == qs.answer_type || qs.answer_type == Byte.Parse(dct["multi_option_list_answer"]))
                    if (Byte.Parse(dct["option_type_other"]) == qs.option_type)
                    {
                        String atxt = String.Empty;

                        VwSurveyOptionAnswer aopt = db.VwSurveyOptionAnswers.FirstOrDefault(obj => obj.question_id == qs.id && obj.answer_id == Int32.Parse(dct["other_flag_id"]) && obj.aq_id == aqid);
                        if (aopt != null) atxt = !String.IsNullOrEmpty(aopt.answer_txt) ? aopt.answer_txt : String.Empty;

                        adct = new Dictionary<string, string>();
                        adct.Add("id", dct["other_flag_id"]);
                        adct.Add("answer_option", "Other");
                        adct.Add("selected", (!String.IsNullOrEmpty(atxt)).ToString());
                        adct.Add("question_id", qs.id.ToString());                        
                        adct.Add("answer_id", String.Empty);
                        adct.Add("answer_txt", atxt);
                        adct.Add("is_other", true.ToString());
                        lstopts.Add(adct);                        
                    }

                foreach (QGridColumn gcol in db.QGridColumns.Where(obj => obj.question_id == qid).ToList())
                {
                    adct = new Dictionary<string, string>();
                    adct.Add("column_name", gcol.column_name);
                    adct.Add("question_id", gcol.question_id.ToString());                    
                    adct.Add("column_data_type", gcol.column_data_type.Value.ToString());
                    adct.Add("id", gcol.id.ToString());
                    grdcols.Add(adct);
                }

                foreach (QGridRow grow in db.QGridRows.Where(obj => obj.question_id == qid).ToList())
                {
                    adct = new Dictionary<string, string>();
                    adct.Add("id", grow.id.ToString());
                    adct.Add("question_id", grow.question_id.ToString());
                    adct.Add("is_other", grow.is_other.Value.ToString());
                    adct.Add("row_name", grow.row_name);
                    grdrows.Add(adct);
                }
                if (grdrows.Count == 0)
                {
                    adct = new Dictionary<string, string>();
                    adct.Add("id", "0");
                    adct.Add("question_id", qs.id.ToString());
                    adct.Add("is_other", false.ToString());
                    adct.Add("row_name", dct["no_rows"]);
                    grdrows.Add(adct);
                }

                foreach (VwSurveyGridAnswer grdans in db.VwSurveyGridAnswers.Where(obj => obj.question_id == qid && obj.aq_id == aqid).ToList())
                {
                    adct = new Dictionary<string, string>();
                    adct.Add("id", grdans.id.ToString());
                    adct.Add("qgrid_col_id", grdans.qgrid_col_id.ToString());
                    adct.Add("qgrid_row_id", grdans.qgrid_row_id.ToString());
                    adct.Add("question_id", grdans.question_id.ToString());                    
                    adct.Add("answer_dec", grdans.answer_dec != null ? grdans.answer_dec.ToString() : String.Empty);
                    adct.Add("answer_num", grdans.answer_num != 0 ? grdans.answer_num.ToString() : String.Empty);
                    adct.Add("answer_txt", grdans.answer_txt != null ? grdans.answer_txt.ToString() : String.Empty);
                    adct.Add("sva_answer_txt", grdans.sva_answer_txt != null ? grdans.sva_answer_txt : String.Empty);
                    adct.Add("sva_answer_dec", grdans.sva_answer_dec != null ? grdans.sva_answer_dec.ToString() : String.Empty);
                    adct.Add("sva_answer_num", grdans.sva_answer_num != 0 ? grdans.sva_answer_num.ToString() : String.Empty);
                    anscolrow.Add(adct);
                }                
            }

            div.Controls.Clear();
            if (qs == null) return;

            List<String> bxans = new List<string>() { dct["real_answer"], dct["integer_answer"], dct["text_answer"] };
            
            Literal ltrl;
            String sid = String.Empty;

            ltrl = new Literal();
            ltrl.Text = String.Format("<p class=\"qstntxt\">{0} {1}</p>", qs.numbering, qs.question);
            div.Controls.Add(ltrl);

            if (qs.help_comment != null)
            {
                ltrl = new Literal();
                ltrl.Text = String.Format("<p class=\"qhlptxt\">{0}</p>", qs.help_comment);
                div.Controls.Add(ltrl);
            }

            if (Byte.Parse(dct["boolean_answer"]) == qs.answer_type.Value)
            {
                String cid = String.Format(dct["bool_ans_control"], qs.id);
                String noopt = String.Format(dct["bool_ans_mask"], qs.id.ToString(), dct["nooption"]);
                String ysopt = String.Format(dct["bool_ans_mask"], qs.id.ToString(), dct["yesoption"]);
                String yschkd = String.Empty, nochkd = String.Empty;

                ltrl = new Literal();
                ltrl.Text = "<ul class=\"vtcllst\">";
                div.Controls.Add(ltrl);
                if (svans != null)
                    if (svans.answer_num == 1) yschkd = " checked=\"checked\"";

                if (String.IsNullOrEmpty(yschkd)) nochkd = " checked=\"checked\"";

                ltrl = new Literal();
                ltrl.Text = String.Format("<li><input name=\"{0}\" type=\"radio\" value=\"{1}\"{2} />No</li>", cid, noopt, nochkd);
                div.Controls.Add(ltrl);

                ltrl = new Literal();
                ltrl.Text = String.Format("<li><input name=\"{0}\" type=\"radio\" value=\"{1}\"{2} />Yes</li>", cid, ysopt, yschkd);
                div.Controls.Add(ltrl);

                ltrl = new Literal();
                ltrl.Text = "</ul>";
                div.Controls.Add(ltrl);
            }
            else
                if (bxans.Contains(qs.answer_type.Value.ToString()))
                {
                    String bxvl = String.Empty;
                                        
                    if (Byte.Parse(dct["integer_answer"]) == qs.answer_type.Value)
                    {
                        sid = String.Format(dct["int_ans_mask"], qs.id.ToString());
                        if (svans != null)
                            bxvl = svans.answer_num.HasValue ? svans.answer_num.Value.ToString() : String.Empty;
                        if (!String.IsNullOrEmpty(bxvl)) bxvl = IncUtilxs.SilenceZero(bxvl);
                    }
                    else
                        if (Byte.Parse(dct["real_answer"]) == qs.answer_type.Value)
                        {
                            sid = String.Format(dct["real_ans_mask"], qs.id.ToString());
                            if (svans != null)
                                bxvl = svans.answer_dec.HasValue ? svans.answer_dec.Value.ToString() : String.Empty;
                            if (!String.IsNullOrEmpty(bxvl)) bxvl = IncUtilxs.SilenceZero(bxvl);
                        }
                        else
                            if (Byte.Parse(dct["text_answer"]) == qs.answer_type.Value)
                            {
                                sid = String.Format(dct["txt_ans_mask"], qs.id.ToString());
                                if (svans != null)
                                    bxvl = !String.IsNullOrEmpty(svans.answer_txt) ? svans.answer_txt.ToString() : String.Empty;
                            }
                    
                    TextBox txtbx = new TextBox();
                    txtbx.ID = sid;
                    txtbx.ClientIDMode = ClientIDMode.Static;
                    txtbx.CssClass = "txtfields";
                    txtbx.Text = bxvl;
                    div.Controls.Add(txtbx);
                }
                else
                    if (Byte.Parse(dct["option_list_answer"]) == qs.answer_type.Value)
                    {                        
                        Table tb = GetAnswerListOptionsTable(lstopts, dct, qs);
                        div.Controls.Add(tb);
                    }
                    else
                        if (Byte.Parse(dct["multi_option_list_answer"]) == qs.answer_type.Value)
                        {
                            Table tb = GetAnswerListOptionsTable(lstopts, dct, qs);
                            div.Controls.Add(tb);
                        }
                        else
                            if (Byte.Parse(dct["grid_answer"]) == qs.answer_type.Value)
                            {                                
                                Table tb = GetAnswerGridTable(grdcols, grdrows, anscolrow, dct);
                                div.Controls.Add(tb);
                            }
        }

        public static Table GetAnswerListOptionsTable(List<Dictionary<String, String>> lstopts, Dictionary<String, String> dct, Question que)
        {
            Table tbl = new Table();
            TableRow trw = new TableRow();
            TableCell cl = new TableCell();

            tbl.CssClass = "tbqstn";
            tbl.Rows.Add(trw);
            trw.Cells.Add(cl);

            cl.VerticalAlign = VerticalAlign.Top;
            cl.HorizontalAlign = HorizontalAlign.Left;

            Literal ltrl;

            Boolean ismulti = (que.answer_type == Byte.Parse(dct["multi_option_list_answer"]));

            String shtm = "<li><input name=\"{0}\" type=\"radio\" value=\"{1}\"{2} />{3}</li>";
            String sohtm = "<li><input name=\"{0}\" type=\"radio\" value=\"{1}\"{2} />{3}<input id=\"{4}\" name=\"{5}\" type=\"text\" class=\"mdtxtfields\"{6} style=\"margin-left: 10px\" /></li>";
            if (ismulti)
            {
                shtm = "<li><input id=\"{0}\" name=\"{1}\" type=\"checkbox\"{2} />{3}</li>";
                sohtm = "<li><input id=\"{0}\" name=\"{1}\" type=\"checkbox\"{2} />{3}<input id=\"{4}\" name=\"{5}\" type=\"text\" class=\"mdtxtfields\"{6} style=\"margin-left: 10px\" /></li>";
            }

            String sid = String.Empty;
            String sli = String.Empty;
            String xid = String.Empty;
            String schkd = String.Empty;
            String srvl = String.Empty;
            String txtvl = String.Empty;

            ltrl = new Literal();
            ltrl.Text = "<ul class=\"qalst\">";
            cl.Controls.Add(ltrl);

            foreach (Dictionary<String, String> opt in lstopts)
            {
                srvl = String.Format(dct["option_valans_mask"], que.id, opt["id"]);                

                if (Boolean.Parse(opt["is_other"]))
                {
                    txtvl = !String.IsNullOrEmpty(opt["answer_txt"]) ? String.Format(" value=\"{0}\"", opt["answer_txt"]) : String.Empty;

                    xid = String.Format(dct["option_ansoth_mask"], opt["question_id"], dct["other_option_id"]);
                    if (ismulti)
                    {
                        schkd = Boolean.Parse(opt["selected"]) ? " checked=\"checked\"" : String.Empty;
                        sid = String.Format(dct["option_ans_mask"], opt["question_id"], opt["id"]);
                        sli = String.Format(sohtm, sid, sid, schkd, "Other", xid, xid, txtvl);
                    }
                    else
                    {
                        schkd = !String.IsNullOrEmpty(opt["answer_txt"]) ? " checked=\"checked\"" : String.Empty;
                        sid = String.Format(dct["option_ans_mask"], opt["question_id"], opt["question_id"]);
                        sli = String.Format(sohtm, sid, srvl, schkd, "Other", xid, xid, txtvl);
                    }
                }
                else
                {
                    if (ismulti)
                    {
                        schkd = Boolean.Parse(opt["selected"]) ? " checked=\"checked\"" : String.Empty;
                        sid = String.Format(dct["option_ans_mask"], opt["question_id"], opt["id"]);
                        sli = String.Format(shtm, sid, sid, schkd, opt["answer_option"]);
                    }
                    else
                    {
                        schkd = opt["answer_option"].Equals(opt["answer_txt"]) ? " checked=\"checked\"" : String.Empty;
                        sid = String.Format(dct["option_ans_mask"], opt["question_id"], opt["question_id"]);
                        sli = String.Format(shtm, sid, srvl, schkd, opt["answer_option"]);
                    }
                }

                ltrl = new Literal();
                ltrl.Text = sli;

                cl.Controls.Add(ltrl);
            }

            ltrl = new Literal();
            ltrl.Text = "</ul>";
            cl.Controls.Add(ltrl);

            return tbl;
        }

        public static Table GetAnswerGridTable(List<Dictionary<String, String>> grdcols, List<Dictionary<String, String>> grdrows, List<Dictionary<String, String>> ansgrd, Dictionary<String, String> dct)
        {
            Table tbl = new Table();
            TableHeaderRow tbhdr;
            TableHeaderCell tbhcl;

            tbl.CssClass = "qagrd";            

            tbhdr = new TableHeaderRow();
            tbhdr.CssClass = "qagrdhdr";
            tbl.Rows.Add(tbhdr);

            tbhcl = new TableHeaderCell();
            tbhcl.Width = Unit.Pixel(250);
            tbhdr.Cells.Add(tbhcl);

            foreach (Dictionary<String, String> hdct in grdcols)
            {
                tbhcl = new TableHeaderCell();
                tbhcl.Text = hdct["column_name"];
                tbhcl.VerticalAlign = VerticalAlign.Middle;
                tbhcl.HorizontalAlign = HorizontalAlign.Center;
                tbhdr.Cells.Add(tbhcl);
            }

            TableRow trw;
            TableCell cl;
            Literal ltrl;
            String schk = String.Empty;

            foreach (Dictionary<String, String> rdct in grdrows)
            {
                trw = new TableRow();

                cl = new TableCell();
                ltrl = new Literal();
                ltrl.Text = String.Format("<span style=\"margin-right: 4px\">{0}</span>", rdct["row_name"]);
                cl.Controls.Add(ltrl);
                if (Boolean.Parse(rdct["is_other"]))
                {
                    String sothrid = String.Format(dct["other_grid_row_mask"], rdct["question_id"], rdct["id"], dct["other_option_id"]);
                    String sothrvl = ansgrd.Count != 0 ? ansgrd[0]["sva_answer_txt"] : String.Empty;
                    ltrl = new Literal();
                    ltrl.Text = String.Format("<input id=\"{0}\" name=\"{1}\" type=\"text\" class=\"mdhftxtflds\" value=\"{2}\" />", sothrid, sothrid, sothrvl);
                    cl.Controls.Add(ltrl);
                }
                cl.HorizontalAlign = HorizontalAlign.Left;
                cl.VerticalAlign = VerticalAlign.Top;                
                trw.Cells.Add(cl);

                foreach (Dictionary<String, String> cdct in grdcols)
                {
                    cl = new TableCell();
                    cl.HorizontalAlign = HorizontalAlign.Left;
                    cl.VerticalAlign = VerticalAlign.Top;

                    ltrl = new Literal();

                    Dictionary<String, String> dobj = ansgrd.FirstOrDefault(obj => obj["qgrid_col_id"].Equals(cdct["id"]) && obj["qgrid_row_id"].Equals(rdct["id"]));
                    String sid = String.Format(dct["grd_ans_mask"], cdct["question_id"], rdct["id"], cdct["id"]);

                    if (cdct["column_data_type"].ToString().Equals(dct["boolean_answer"]))
                    {
                        schk = String.Empty;
                        
                        if (dobj != null)
                            schk = Int32.Parse(dobj["answer_num"]) == 1 ? " checked=\"checked\"" : String.Empty;

                        ltrl.Text = String.Format("<input id=\"{0}\" name=\"{1}\" type=\"checkbox\"{2} />", sid, sid, schk);
                        cl.HorizontalAlign = HorizontalAlign.Center;
                        cl.VerticalAlign = VerticalAlign.Middle;
                    }
                    else
                    {
                        String txtvl = String.Empty;
                        if (dobj != null)
                        {
                            if (!String.IsNullOrEmpty(IncUtilxs.SilenceDecZero(dobj["answer_dec"])))
                                txtvl = IncUtilxs.SilenceDecZero(dobj["answer_dec"]);
                            else
                                if (!String.IsNullOrEmpty(IncUtilxs.SilenceZero(dobj["answer_num"])))
                                    txtvl = IncUtilxs.SilenceZero(dobj["answer_num"]);
                                else
                                    if (!String.IsNullOrEmpty(dobj["answer_txt"]))
                                        txtvl = dobj["answer_txt"];
                        }

                        ltrl.Text = String.Format("<input id=\"{0}\" name=\"{1}\" type=\"text\" class=\"tnytxtflds\" value=\"{2}\" />", sid, sid, txtvl);
                    }

                    cl.Controls.Add(ltrl);

                    trw.Cells.Add(cl);
                }

                tbl.Rows.Add(trw);
            }

            return tbl;
        }

        public static void LogNameValueCollection(System.Collections.Specialized.NameValueCollection nvc, String src)
        {
            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
            {
                String q = "insert into t_debug_dictionary (dict_key, dict_val, item_source) values ('{0}', '{1}', '{2}')";
                String s = String.Empty;
                Int32 rslt = 0;

                foreach (String sky in nvc.AllKeys)
                {
                    s = String.Format(q, sky.Replace("'", "").Replace("\"", ""), nvc[sky].ToString().Replace("'", "").Replace("\"", ""), src);
                    rslt = db.ExecuteCommand(s);
                }                
            }
        }

        public static void PlaceMapIcon(PlaceHolder plchldr)
        {
            HyperLink hypr;
            ImageMap img;

            String dflt = "mapmarkermarkeroutsidepinkicon.png";
            String imgurl = IncUtilxs.GetDictionaryKeyVal("app_map_icon");

            if (String.IsNullOrEmpty(imgurl)) imgurl = dflt;

            img = new ImageMap();
            img.ID = "imgmp";
            img.ToolTip = "See Map";
            img.ImageUrl = String.Format("mapicons/{0}", imgurl);
            img.AlternateText = img.ToolTip;

            hypr = new HyperLink();
            hypr.ID = "lnkmp";
            hypr.NavigateUrl = "openmap.aspx";
            hypr.Controls.Add(img);            
            plchldr.Controls.Add(hypr);            
        }

        public static void PlaceHelpIcon(PlaceHolder plchldr)
        {
            ImageMap img;
            String simg = "helpicon.png";

            img = new ImageMap();
            img.ID = "imghlp";
            img.ClientIDMode = ClientIDMode.Static;
            img.ToolTip = "Help";
            img.ImageUrl = String.Format("incimages/{0}", simg);
            img.AlternateText = img.ToolTip;
            plchldr.Controls.Add(img);
        }

        public static void SetMapIconAttribute(PlaceHolder plchldr)
        {
            Label lbl = new Label();
            HyperLink hypr = new HyperLink();

            String sattrib = IncUtilxs.GetDictionaryKeyVal("icon_attribute");
            String icnorg = IncUtilxs.GetDictionaryKeyVal("icon_site_name");
            String icnsite = IncUtilxs.GetDictionaryKeyVal("icon_site_url");

            lbl.ID = "icnattrib";
            lbl.Text = sattrib;
            lbl.Attributes.CssStyle.Add("font-size", "7pt");
            lbl.Attributes.CssStyle.Add("margin-right", "4px");
            lbl.Attributes.CssStyle.Add("color", "#0000FF");
            
            hypr.ID = "lnkicnattr";
            hypr.Text = icnorg;
            hypr.NavigateUrl = icnsite;
            hypr.Attributes.CssStyle.Add("color", "#990000");
            hypr.Attributes.CssStyle.Add("font-size", "7pt");

            plchldr.Controls.Clear();
            plchldr.Controls.Add(lbl);
            plchldr.Controls.Add(hypr);
        }

        public static void HideMapIcon(MasterPage mstr)
        {
            Panel pnl = (Panel)mstr.FindControl("pnl_mapicon_inner");

            if (pnl != null)
            {
                pnl.Controls.Clear();
                pnl.Attributes.CssStyle.Clear();
                pnl.Attributes.CssStyle.Add("width", "100%");
                pnl.Attributes.CssStyle.Add("height", "30px");
            }
        }

        public static String ColumnDataType(Byte dttyp)
        {
            String s = String.Empty;
            String svl = IncUtilxs.GetDictionaryKeyVal("grid_data_types");

            List<Dictionary<String, String>> lst = DictKeyValString(svl, ';', ',');
            if (lst.Count != 0)
                foreach (Dictionary<String, String> dct in lst)
                    if (dct["key"].Equals(dttyp.ToString())) { s = dct["val"]; break; }
            
            return s;
        }

        public static String CSVString(String s)
        {
            s = s.Replace("\"", "'");
            if (s.Contains(",")) s = s.Replace(",", String.Empty);
            if (s.Contains(";")) s = s.Replace(";", String.Empty);

            return s;
        }

        public static String TransformGroupPath(System.Collections.Specialized.NameValueCollection prms, System.Collections.Specialized.NameValueCollection qry, String surl)
        {
            Uri nwuri = new Uri(surl);

            List<String> lst = new List<string>();
            lst.Add(IncUtilxs.GetParameterName("community_id"));
            lst.Add(IncUtilxs.GetParameterName("household_id"));
            lst.Add(IncUtilxs.GetParameterName("individual_id"));

            String sprmindv = lst[2];
            String sval = String.Empty, sprms = String.Empty;

            Dictionary<String, String> dct = new Dictionary<string, string>();            
            Dictionary<String, String> rprms = IncUtilxs.UrlQueryParams(surl);

            if (prms.Count != 0)
                foreach(String s in lst)
                    if (prms.AllKeys.Contains(s))          
                        dct.Add(s, prms[s].ToString());            

            if (rprms.Count != 0)
                foreach (KeyValuePair<String, String> kv in rprms)
                    if (!dct.Keys.Contains(kv.Key)) dct.Add(kv.Key, kv.Value);

            if (qry.Count != 0)
                foreach (String ky in qry.AllKeys)
                    if (!dct.Keys.Contains(ky)) dct.Add(ky, qry[ky].ToString());

            if (dct.Count != 0) sprms = String.Join("&", from d in dct select String.Format("{0}={1}", d.Key, d.Value));

            if (dct.Keys.Contains(sprmindv)) sprms += "#affected_groups";

            sval = nwuri.AbsolutePath;            
            if (!String.IsNullOrEmpty(sprms)) sval += String.Format("?{0}", sprms);

            return sval;
        }

        public static void SetHtmlCheckBox(ref CheckBox chk, System.Collections.Specialized.NameValueCollection prms, Boolean is_saved, Boolean is_post)
        {
            if (!is_saved && is_post)
            {
                String vl = GetRequestParamValue(prms, chk.ID.ToString());
                chk.Checked = !String.IsNullOrEmpty(vl);
            }
        }

        public static void SetHtmlTextBox(ref TextBox txt, System.Collections.Specialized.NameValueCollection prms, Boolean is_saved, Boolean is_post)
        {
            if (!is_saved && is_post)
            {
                String vl = GetRequestParamValue(prms, txt.ID.ToString());
                if (!vl.Equals(txt.Text.ToString())) txt.Text = vl;
            }
        }

        public static String GetHelpObjectName(String pathnm)
        {
            String s = String.Empty;

            try
            {
                String fnm = System.IO.Path.GetFileName(pathnm);
                String[] arry = fnm.Split('.');
                s = arry[0];
            }
            catch
            {
                s = String.Empty;
            }

            return s;
        }
    }
}