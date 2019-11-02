using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using iom;

namespace inc
{
    public static partial class IncUtilxs
    {
        public static String StripOddChars(String s)
        {
            s = s.Replace('\t', ' ');
            s = s.Replace('\r', ' ');
            s = s.Replace('\n', ' ');

            return s.Trim();
        }

        public static void DumpCSV(HttpResponse resp, List<Dictionary<String, String>> data, Dictionary<String, String> cols, String filenm)
        {
            List<String> line = new List<string>();

            resp.Clear();
            resp.ClearContent();
            resp.ClearHeaders();
            resp.ContentType = "text/csv";
            resp.ContentEncoding = System.Text.Encoding.UTF8;
            resp.AddHeader("Content-Disposition", "attachment; filename=" + filenm);
            resp.AddHeader("Pragma", "public");

            foreach (String vl in cols.Values)
            {
                String s = StripOddChars(vl);
                s = CSVString(s);
                line.Add(s.Trim());
            }
            resp.Write(String.Format("{0}\r\n", String.Join(",", line.ToArray())));

            foreach (Dictionary<String, String> dct in data)
            {
                line = new List<string>();
                foreach (KeyValuePair<String, String> kv in dct)
                    if (cols.Keys.Contains(kv.Key))
                    {
                        String s = StripOddChars(kv.Value);
                        s = CSVString(s);
                        line.Add(s.Trim());
                    }

                resp.Write(String.Format("{0}\r\n", String.Join(",", line.ToArray())));
            }

            resp.End();
        }

        public static void DumpCSV(HttpResponse resp, List<List<String>> data, List<String> cols, String filenm)
        {            
            resp.Clear();
            resp.ClearContent();
            resp.ClearHeaders();
            resp.ContentType = "text/csv";
            resp.ContentEncoding = System.Text.Encoding.UTF8;
            resp.AddHeader("Content-Disposition", "attachment; filename=" + filenm);
            resp.AddHeader("Pragma", "public");

            List<String> line = new List<string>();

            foreach (String cl in cols)
            {
                String s = StripOddChars(cl);
                s = CSVString(s);
                line.Add(s.Trim());
            }
            resp.Write(String.Format("{0}\r\n", String.Join(",", line.ToArray())));

            foreach (List<String> lst in data)
            {
                line = new List<string>();
                foreach (String vl in lst)                    
                {
                    String s = StripOddChars(vl);
                    s = CSVString(s);
                    line.Add(s.Trim());
                }
                resp.Write(String.Format("{0}\r\n", String.Join(",", line.ToArray())));
            }

            resp.End();
        }

        public static void DumpCSV(HttpResponse resp, List<List<String>> data, String filenm)
        {
            resp.Clear();
            resp.ClearContent();
            resp.ClearHeaders();
            resp.ContentType = "text/csv";
            resp.ContentEncoding = System.Text.Encoding.UTF8;
            resp.AddHeader("Content-Disposition", "attachment; filename=" + filenm);
            resp.AddHeader("Pragma", "public");

            List<String> line = new List<string>();

            foreach (List<String> lst in data)
            {
                line = new List<string>();
                foreach (String vl in lst)
                {
                    String s = StripOddChars(vl);
                    s = CSVString(s);
                    line.Add(s.Trim());
                }
                resp.Write(String.Format("{0}\r\n", String.Join(",", line.ToArray())));
            }

            resp.End();
        }

        public static void FillAreaCasesDropDown(DropDownList drpdwn, Int16 prv, Int16 dst, Int32 csid)
        {
            drpdwn.Items.Clear();
            drpdwn.Items.Add(new ListItem("UnSpecified", "0"));

            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
            {
                var csobj = from cs in db.Cases
                            join cn in db.CaseNumbers on cs.id equals cn.case_id
                            join ic in db.TypeIncidents on cs.incident_type_id.Value equals ic.id
                            where cs.province_id == prv && cs.district_id == dst
                            select new { cn.case_id, cn.case_no, ic.incident_type };

                foreach (var obj in csobj) drpdwn.Items.Add(new ListItem(String.Format("{0}: {1}", obj.case_no, obj.incident_type), obj.case_id.ToString()));
            }

            if (drpdwn.Items.FindByValue(csid.ToString()) != null) drpdwn.SelectedValue = csid.ToString();
        }

        public static void FillGenderDropList(DropDownList drpdwn, String vl)
        {
            drpdwn.Items.Clear();

            drpdwn.Items.Add(new ListItem("Male", "Male"));
            drpdwn.Items.Add(new ListItem("Female", "Female"));

            if (!String.IsNullOrEmpty(vl))
                if (drpdwn.Items.FindByValue(vl) != null) drpdwn.SelectedValue = vl;
        }

        public static void FillEmergencyLevelDropList(DropDownList drpdwn, String vl)
        {
            drpdwn.Items.Clear();

            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
                foreach (TypeEmergencyLevel emrg in db.TypeEmergencyLevels.ToList())
                    drpdwn.Items.Add(new ListItem(emrg.emergency_level, emrg.id.ToString()));

            if (!String.IsNullOrEmpty(vl))
                if (drpdwn.Items.FindByValue(vl) != null) drpdwn.SelectedValue = vl;
        }

        public static void FillDisplacementStatusDropList(DropDownList drpdwn, String vl)
        {
            drpdwn.Items.Clear();

            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
                foreach (TypeDisplacement dis in db.TypeDisplacements.ToList())
                    drpdwn.Items.Add(new ListItem(dis.displacement_status, dis.id.ToString()));

            if (!String.IsNullOrEmpty(vl))
                if (drpdwn.Items.FindByValue(vl) != null) drpdwn.SelectedValue = vl;
        }

        public static void FillVulnerabilityLevelDropList(DropDownList drpdwn, String vl)
        {
            drpdwn.Items.Clear();

            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
                foreach (TypeVulnerability vul in db.TypeVulnerabilities.ToList())
                    drpdwn.Items.Add(new ListItem(vul.type_vulnerability, vul.id.ToString()));

            if (!String.IsNullOrEmpty(vl))
                if (drpdwn.Items.FindByValue(vl) != null) drpdwn.SelectedValue = vl;
        }

        public static void FillSeverityLevel(DropDownList drpdwn, String vl)
        {
            drpdwn.Items.Clear();

            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
                foreach (TypeIncSeverity svrty in db.TypeIncSeverities.ToList())
                    drpdwn.Items.Add(new ListItem(svrty.incident_severity, svrty.id.ToString()));

            if (!String.IsNullOrEmpty(vl))
                if (drpdwn.Items.FindByValue(vl) != null) drpdwn.SelectedValue = vl;
        }

        public static String IndividualEntityName(String fnm, String mdnm, String lnm)
        {
            String fullnm = String.Empty;

            if (!String.IsNullOrEmpty(lnm) && !String.IsNullOrEmpty(fnm))
                if (!String.IsNullOrEmpty(mdnm))
                    fullnm += String.Format("{0}, {1} {2}", lnm, mdnm, fnm);
                else
                    fullnm = String.Format("{0}, {1}", lnm, fnm);

            return fullnm;
        }

        public static Dictionary<String, String> UrlQueryParams(String surl)
        {
            Dictionary<String, String> dct = new Dictionary<string, string>();

            Uri nwuri = new Uri(surl);

            if (!String.IsNullOrEmpty(nwuri.Query))
            {
                String s = nwuri.Query;

                if (s[0].ToString().Equals("?")) s = s.Substring(1, s.Length - 1);

                List<String> lst = s.Split('&').ToList();
                foreach (String x in lst)
                {
                    List<String> kyvl = x.Split('=').ToList();
                    if (kyvl.Count > 1) dct.Add(kyvl[0], kyvl[1]);
                }
            }

            return dct;
        }

        public static void FillOtherTypeCauseList(DropDownList drpdwn, String vl)
        {
            drpdwn.Items.Clear();

            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
                foreach (TypeOtherCause toc in db.TypeOtherCauses.Where(obj => obj.enabled == true).ToList())
                    drpdwn.Items.Add(new ListItem(toc.type_other_cause, toc.id.ToString()));

            if (!String.IsNullOrEmpty(vl))
                if (drpdwn.Items.FindByValue(vl) != null) drpdwn.SelectedValue = vl;
        }

        public static String GetHtmlCheckBoxVal(System.Collections.Specialized.NameValueCollection prms, Object oid, Object omsk, Object ochkall)
        {
            String svl = String.Empty;
            String schkall = String.Empty;

            if (ochkall != null)
            {                
                Boolean chkd = Boolean.Parse(ochkall.ToString());
                
                if (chkd) svl = "checked=checked";
            }
            else                
                if (oid != null && omsk != null)
                {
                    String sky = String.Format(omsk.ToString(), oid.ToString());

                    foreach (String s in prms.AllKeys)
                        if (s.Contains(sky))
                        {
                            svl = "checked=checked";
                            break;
                        }
                }

            return svl;
        }

        public static void FillReferralSearchFields(DropDownList drp)
        {
            List<String> ilst = new List<string>() { "case_no", "date_reported", "displacement_status_id", "incident_type_id", "vulnerability_id" };
            List<String> xlst = new List<string>() { "assist_date", "provider_id", "project_id", "type_assistance_id" };
            List<String> slst = new List<string>();

            foreach (String x in ilst) slst.Add(String.Format("'{0}'", x));

            drp.Items.Clear();
            drp.Items.Add(new ListItem("-- Select --", "0"));

            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
            {
                String s = String.Format("select * from t_search_fields where class_name in ('individual') or field_name in ({0}) order by field_value", String.Join(", ", slst.ToArray()));
                foreach (SearchField srch in db.ExecuteQuery<SearchField>(s).ToList())
                    if (xlst.Contains(srch.field_name)) continue;
                    else
                        if (drp.Items.FindByValue(srch.field_name) == null)
                            drp.Items.Add(new ListItem(srch.field_value, srch.field_name));
            }
        }

        public static void FillReferralSearchSearch(DropDownList drp)
        {
            List<String> ilst = new List<string>() { "case_no", "date_reported", "displacement_status_id", "incident_type_id", "vulnerability_id" };
            List<String> xlst = new List<string>() { "assist_date", "provider_id", "project_id", "type_assistance_id", "assist_date" };
            List<String> slst = new List<string>();

            foreach (String x in ilst) slst.Add(String.Format("'{0}'", x));

            drp.Items.Clear();
            drp.Items.Add(new ListItem("-- Select --", "0"));

            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
            {
                String s = String.Format("select * from t_search_fields where class_name in ('individual', 'referral') or field_name in ({0}) order by field_value", String.Join(", ", slst.ToArray()));
                foreach (SearchField srch in db.ExecuteQuery<SearchField>(s).ToList())
                    if (xlst.Contains(srch.field_name)) continue;
                    else drp.Items.Add(new ListItem(srch.field_value, srch.field_name));
            }
        }

        public static void FillIndividualSearchFields(DropDownList drp)
        {
            List<String> ilst = new List<string>() { "date_reported" };
            List<String> xlst = new List<string>() { "hal", "province_id", "org_unit_id", "type_cause_id", "type_perpetrator_id", "severity_level" };
            List<String> slst = new List<string>();

            foreach (String x in ilst) slst.Add(String.Format("'{0}'", x));

            drp.Items.Clear();
            drp.Items.Add(new ListItem("-- Select --", "0"));

            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
            {
                String s = String.Format("select * from t_search_fields where class_name in ('case_individual', 'individual') or field_name in ({0}) order by field_value", String.Join(", ", slst.ToArray()));
                foreach (SearchField srch in db.ExecuteQuery<SearchField>(s).ToList())
                    if (xlst.Contains(srch.field_name)) continue;
                    else
                        drp.Items.Add(new ListItem(srch.field_value, srch.field_name));
            }
        }

        public static void FillCaseSearchFields(DropDownList drp)
        {
            List<String> xlst = new List<string>();

            drp.Items.Clear();
            drp.Items.Add(new ListItem("-- Select --", "0"));

            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
            {
                String s = "select * from t_search_fields where class_name in ('case', 'case_individual') order by field_value";
                foreach (SearchField srch in db.ExecuteQuery<SearchField>(s).ToList())
                    if (xlst.Contains(srch.field_name)) continue;
                    else
                        drp.Items.Add(new ListItem(srch.field_value, srch.field_name));
            }            
        }

        public static String GetSearchFieldValue(String clsnm, String fldnm)
        {
            SearchField srch = null;
            String s = String.Empty;

            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
                if (!String.IsNullOrEmpty(clsnm) && !String.IsNullOrEmpty(fldnm))
                {
                    srch = db.SearchFields.FirstOrDefault(obj => obj.class_name.Equals(clsnm) && obj.field_name.Equals(fldnm));
                    if (srch != null) s = srch.field_value;
                }
                else
                    if (!String.IsNullOrEmpty(fldnm))
                    {
                        srch = db.SearchFields.FirstOrDefault(obj => obj.field_name.Equals(fldnm));
                        if (srch != null) s = srch.field_value;
                    }

            return s;
        }

        public static void MakeDMYDropDowns(PlaceHolder plchldr, String sttl, String dyid, String mnid, String yrid, Int32 styr, Byte cntyr, List<Int32> dflts)
        {
            DropDownList dy = new DropDownList();
            DropDownList mon = new DropDownList();
            DropDownList yr = new DropDownList();

            Label fwdslsh1 = new Label();
            Label fwdslsh2 = new Label();

            Label ttl = new Label();            
            ttl.Text = sttl;
            ttl.Attributes.CssStyle.Add(System.Web.UI.HtmlTextWriterStyle.MarginLeft, "15px");

            dy.CssClass = "drpdwnw";
            mon.CssClass = "drpdwnw";
            yr.CssClass = "drpdwnw";

            dy.ID = dyid;
            mon.ID = mnid;
            yr.ID = yrid;

            dy.CausesValidation = false;
            mon.CausesValidation = false;
            yr.CausesValidation = false;

            dy.AutoPostBack = false;
            mon.AutoPostBack = false;
            yr.AutoPostBack = false;

            foreach (Byte i in Enumerable.Range(1, 31))
                if (i < 10) dy.Items.Add(new ListItem("0" + i.ToString(), "0" + i.ToString()));
                else dy.Items.Add(new ListItem(i.ToString(), i.ToString()));

            foreach (Byte i in Enumerable.Range(1, 12))
                if (i < 10) mon.Items.Add(new ListItem(System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(i), "0" + i.ToString()));
                else mon.Items.Add(new ListItem(System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(i), i.ToString()));
            
            //foreach (Int16 i in Enumerable.Range(styr, cntyr)) yr.Items.Add(new ListItem(i.ToString(), i.ToString()));
            for (Int16 i = (Int16)(DateTime.Now.Year + 1); i > (DateTime.Now.Year - 100); i--)
                yr.Items.Add(new ListItem(i.ToString(), i.ToString()));

            if (dy.Items.FindByValue(dflts[0].ToString()) != null)
                dy.SelectedValue = dflts[0].ToString();
            else
                if (dy.Items.FindByValue(String.Format("0{0}", dflts[0].ToString())) != null)
                    dy.SelectedValue = String.Format("0{0}", dflts[0].ToString());

            if (mon.Items.FindByValue(dflts[1].ToString()) != null)
                mon.SelectedValue = dflts[1].ToString();
            else
                if (mon.Items.FindByValue(String.Format("0{0}", dflts[1].ToString())) != null)
                    mon.SelectedValue = String.Format("0{0}", dflts[1].ToString());

            if (yr.Items.FindByValue(dflts[2].ToString()) != null) yr.SelectedValue = dflts[2].ToString();

            fwdslsh1.Text = "/";
            fwdslsh2.Text = "/";

            fwdslsh1.Attributes.CssStyle.Add(System.Web.UI.HtmlTextWriterStyle.Margin, "0px 5px");
            fwdslsh2.Attributes.CssStyle.Add(System.Web.UI.HtmlTextWriterStyle.Margin, "0px 5px");

            plchldr.Controls.Add(dy);
            plchldr.Controls.Add(fwdslsh1);
            plchldr.Controls.Add(mon);
            plchldr.Controls.Add(fwdslsh2);
            plchldr.Controls.Add(yr);
            plchldr.Controls.Add(ttl);
        }

        public static String ConvertToHtml(String msg)
        {
            String s = msg.Replace(Convert.ToChar(10).ToString(), "<br />");

            s = msg.Replace(Convert.ToChar(13).ToString(), "<br />");

            return s;
        }

        public static void FillMaritalStatus(DropDownList drp, String vl)
        {
            drp.Items.Clear();

            String sitems = IncUtilxs.GetDictionaryKeyVal("marital_status");
            List<String> lst = sitems.Split(',').ToList();

            if (lst != null)
                foreach (String s in lst) drp.Items.Add(new ListItem(s.Trim(), s.Trim()));

            if (!String.IsNullOrEmpty(vl))
                if (drp.Items.FindByValue(vl) != null)
                    drp.SelectedValue = vl.Trim();
        }

        public static void RestoreDropDownVal(DropDownList drp, System.Collections.Specialized.NameValueCollection prms, String ky)
        {
            String s = IncUtilxs.GetRequestParamValue(prms, ky);
            if (!String.IsNullOrEmpty(s))
                if (drp.Items.FindByValue(s) != null) drp.SelectedValue = s;
        }

        public static void RestoreTextVal(TextBox txt, System.Collections.Specialized.NameValueCollection prms, String ky)
        {
            String s = IncUtilxs.GetRequestParamValue(prms, ky);
            txt.Text = s;                
        }

        public static String CriteriaSearchVal(String sfld, String vl)
        {
            List<String> boolst = new List<string>() { "is_breadwinner", "is_verified" };
            String s = vl;

            if (boolst.Contains(sfld)) s = vl.Equals("1") ? "Yes" : "No";

            return s;
        }

        public static void FillQuestionnaires(DropDownList drp, String vl)
        {
            drp.Items.Clear();

            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
                foreach (Questionnaire qs in db.Questionnaires.ToList())
                    drp.Items.Add(new ListItem(qs.qname, qs.id.ToString()));

            if (drp.Items.FindByValue(vl) != null) drp.SelectedValue = vl;
        }

        public static void FillQsSections(DropDownList drp, String qs, String vl)
        {
            drp.Items.Clear();

            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
                foreach (QSection sc in db.QSections.Where(obj => obj.questionnaire_id == Int32.Parse(qs)).ToList())
                    drp.Items.Add(new ListItem(sc.section, sc.id.ToString()));

            if (drp.Items.FindByValue(vl) != null) drp.SelectedValue = vl;
        }

        public static void FillSurveyEvaluations(DropDownList drp, String vl)
        {
            drp.Items.Clear();

            String sevals = IncUtilxs.GetDictionaryKeyVal("evaluation_types");
            List<Dictionary<String, String>> lst = IncUtilxs.DictKeyValString(sevals, ';', ',');
            foreach (Dictionary<String, String> dct in lst) drp.Items.Add(new ListItem(dct["val"], dct["key"]));
        }
    }
}