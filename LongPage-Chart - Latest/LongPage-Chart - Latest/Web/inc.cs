using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.Text;
using System.Data.SqlClient;
using iom;

namespace inc
{
    public enum Actions
    {
        CanRead = 1,
        CanWrite = 2,
        CanDelete = 3,
        CanDeleteCase = 4
    }

    public enum UserGroups
    {
        Administrators = 1,
        Users = 2
    }

    public class MessageButton
    {
        public String ButtonName = String.Empty;
        public String ButtonLabel = String.Empty;
        public String JScriptCommand = String.Empty;
    }

    public class IncAuthentication
    {
        private Int16 usrid = 0;
        private Int16 usrorgunit = 0;
        private Boolean user_ok = false;
        private VwUser usritms;
        private UserGroups usrgrp;
        private List<VwUserPermission> perms;
        private String usrnm = String.Empty;
        private String fnms = String.Empty;
        private String lnm = String.Empty;
        private String eml = String.Empty;
        private String usrgrpnm = String.Empty;
        private List<Int16> xtraorgs = null;

        public IncAuthentication() { }

        public IncAuthentication(String usrnm, String pwd)
        {
            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
            {
                this.usritms = db.p_fetch_login_user(usrnm, pwd).FirstOrDefault();
                if (this.usritms != null)
                {
                    this.xtraorgs = new List<short>();

                    this.user_ok = true;
                    this.usrid = this.usritms.id;
                    this.usrnm = this.usritms.usr_name;
                    this.fnms = this.usritms.fnames;
                    this.lnm = this.usritms.lname;
                    this.eml = this.usritms.email;
                    this.usrorgunit = this.usritms.org_unit_id.Value;
                    this.usrgrp = (UserGroups)this.usritms.user_group_id;
                    this.perms = db.p_fetch_user_perms((short)this.usrid).ToList();
                    this.usrgrpnm = this.usritms.user_group;

                    this.xtraorgs = (from obj in db.ObjectPermissions where obj.granted_to == this.usrid select obj.org_unit_id).ToList();
                }
            }
        }

        public void AuthenticateFromCookie(String ckyusr, String ckynm, String ckyvl, ref Boolean ok)
        {
            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
            {
                IncCooky rw = (from obj in db.IncCookies where ckyusr.Equals(obj.usr_name) && ckynm.Equals(obj.cookie_name) && ckyvl.Equals(obj.cookie_value) && obj.inserted_on.Value.Day == DateTime.Now.Day && obj.inserted_on.Value.Month == DateTime.Now.Month && obj.inserted_on.Value.Year == DateTime.Now.Year select obj).FirstOrDefault();
                if (rw != null)
                {
                    this.usritms = db.VwUsers.FirstOrDefault(obj => obj.usr_name == rw.usr_name);
                    if (this.usritms != null)
                    {
                        ok = true;
                        this.user_ok = true;
                        this.usrid = this.usritms.id;
                        this.usrnm = this.usritms.usr_name;
                        this.fnms = this.usritms.fnames;
                        this.lnm = this.usritms.lname;
                        this.eml = this.usritms.email;
                        this.usrgrp = (UserGroups)this.usritms.user_group_id;
                        this.perms = db.p_fetch_user_perms((short)this.usrid).ToList();
                        this.usrgrpnm = this.usritms.user_group;
                    }
                }
            }
        }

        public Int16 UserID
        {
            get { return this.usrid; }
        }

        public String UserName
        {
            get { return this.usrnm; }
        }

        public Boolean IsUserValid
        {
            get { return this.user_ok; }
        }

        public UserGroups GroupUser
        {
            get { return this.usrgrp; }
        }

        public String UserGroupName
        {
            get { return this.usrgrpnm; }
        }

        public String FirstNames
        {
            get { return this.fnms; }
        }

        public String LastName
        {
            get { return this.lnm; }
        }

        public String UserEmail
        {
            get { return this.eml; }
        }

        public Int16 UserOrgUnitID
        {
            get { return this.usrorgunit; }
        }

        public Boolean IsActionAllowed(Int16 orguntid, Actions act, object v)
        {
            Boolean ans = false;

            if (this.perms.Count > 0)
                foreach (VwUserPermission obj in this.perms)
                    if (obj.org_unit_id == orguntid)
                    {
                        switch (act)
                        {
                            case Actions.CanRead:
                                ans = obj.can_read;
                                break;
                            case Actions.CanWrite:
                                ans = obj.can_write;
                                break;
                            case Actions.CanDelete:
                                ans = obj.can_delete;
                                break;
                            case Actions.CanDeleteCase:
                                ans = obj.can_delete_case.HasValue ? obj.can_delete_case.Value : false;
                                break;
                        }
                        break;
                    }

            return ans;
        }

        public Boolean AnyReadPermission()
        {
            Boolean ok = false;

            ok = this.perms.Exists(obj => obj.can_read == true);

            return ok;
        }

        public Boolean AnyWritePermission()
        {
            Boolean ok = false;

            ok = this.perms.Exists(obj => obj.can_write == true);

            return ok;
        }

        public Boolean CanReadOrgUnit(Int16 orgid, String objkwd)
        {
            Boolean ans = false;

            ans = this.perms.Exists(obj => (obj.can_read && (obj.org_unit_id == orgid)));

            return ans;
        }

        public Boolean IsAdmin()
        {
            return (this.usrgrp == UserGroups.Administrators);
        }

        public List<Int16> GetPermittedOrgUnits()
        {
            List<Int16> lst = new List<short>();

            if (this.perms != null)
                if (this.perms.Count != 0)
                    lst = (from obj in this.perms where obj.can_read select obj.org_unit_id).ToList();

            if (this.xtraorgs != null)
                foreach (Int16 i in this.xtraorgs)
                    if (!lst.Exists(obj => obj == i)) lst.Add(i);

            return lst;
        }

        internal bool IsActionAllowed(short v, Actions canWrite)
        {
            throw new NotImplementedException();
        }

        internal bool CanReadOrgUnit(short org_unit_id, object v)
        {
            throw new NotImplementedException();
        }
    }

    public class IncPrompt
    {
        private List<MessageButton> bttns = new List<MessageButton>();
        private Hashtable itms = new Hashtable();

        public IncPrompt() { ; }
        public IncPrompt(String btnm, String btnlbl, String jscrpt, String msg)
        {
            itms.Add("message", msg);
            this.bttns.Add(new MessageButton() { ButtonLabel = btnlbl, ButtonName = btnm, JScriptCommand = jscrpt });
        }

        public void PutButton(MessageButton btn)
        {
            bttns.Add(btn);
        }

        public List<MessageButton> MessageButtons
        {
            get { return bttns; }
            set { this.MessageButtons = value; }
        }

        public Hashtable PromptItems
        {
            get { return this.itms; }
            set { this.itms = value; }
        }
    }

    public static partial class IncUtilxs
    {
        public const string CRYSTALDATASET = "CrystalDataSet";
        public const string REPORTHASHOBJECT = "report_attributes";

        public static IncParameter GetIncParameter(String prmid)
        {
            IncParameter obj = null;

            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
            {
                obj = db.IncParameters.FirstOrDefault(x => x.param_id == prmid);
            }
            return obj;
        }

        public static String GetParameterNameOnly(String prmid)
        {
            String ans = String.Empty;

            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
            {
                IncParameter rw = db.IncParameters.FirstOrDefault(obj => obj.param_id.Equals(prmid));
                if (rw != null) ans = rw.param_name;
            }

            return ans;
        }

        public static String GetParameterName(String prmid)
        {
            IncParameter prm = IncUtilxs.GetIncParameter(prmid);
            String str = String.Empty;

            if (prm != null)
                str = prm.param_name;

            return str;
        }

        public static Boolean IsGetRecord(String q)
        {
            Boolean ans = false;

            if (!String.IsNullOrEmpty(q))
            {
                Regex rgxp = new Regex("^\\d{1,}$");
                Match mtchqry = rgxp.Match(q);
                ans = mtchqry.Success;
            }

            return ans;
        }

        public static String GetRequestParamValue(NameValueCollection prms, String ky)
        {
            String vl = String.Empty;

            foreach (String s in prms.AllKeys)
            {
                if (String.IsNullOrEmpty(s)) continue;

                if (s.Contains(ky))
                {
                    vl = prms[s].ToString();
                    break;
                }
            }

            return vl;
        }

        public static Boolean IsValidEmailFormat(String eml)
        {
            Boolean ans = false;

            String xpr = "^[\\w\\d\\.\\']*@[\\w\\d\\.]*[\\w]{2,3}$";

            Regex rgx = new Regex(xpr);
            Match mtch = rgx.Match(eml);
            ans = mtch.Success;

            return ans;
        }

        public static String GetDictionaryKeyVal(String ky)
        {
            String answer = String.Empty;

            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
            {
                IncDictionary rws = db.IncDictionaries.FirstOrDefault(obj => obj.dict_key.Equals(ky) && obj.enabled);
                if (rws != null) answer = rws.dict_value.ToString();
            }

            return answer;
        }

        public static String CleanSQLExpression(String s)
        {
            s.Replace("'", "''");
            s.Replace("\"", "\"\"");
            if (!s.Contains("%")) s  = String.Format("%{0}%", s);

            return s.Trim();
        }

        public static String Escape(String s)
        {
            String x = s.Replace("'", "''").Replace("\"", "\"\"").Replace("\\", "\\\\");

            return x;
        }

        public static void CaseToolBar(PlaceHolder bttns, PlaceHolder nvlnks, PlaceHolder mnulnks)
        {
            Button btn;
            HyperLink hypr;
            LinkButton lnkbtn;

            btn = new Button();
            btn.ID = "btnsv";
            btn.Text = "Save";
            btn.CssClass = "frmbttns";
            bttns.Controls.Add(btn);

            btn = new Button();
            btn.ID = "btndlt";
            btn.Text = "Delete";
            btn.CssClass = "frmbttns";
            bttns.Controls.Add(btn);

            btn = new Button();
            btn.ID = "btnw";
            btn.Text = "New";
            btn.CssClass = "frmbttns";
            bttns.Controls.Add(btn);

            lnkbtn = new LinkButton();
            lnkbtn.ID = "lnkbtnprev";
            lnkbtn.Text = "Previous";
            lnkbtn.CssClass = "sbmnulnks";
            nvlnks.Controls.Add(lnkbtn);

            lnkbtn = new LinkButton();
            lnkbtn.ID = "lnkbtnothr";
            lnkbtn.Text = "Other";
            lnkbtn.CssClass = "sbmnulnks";
            nvlnks.Controls.Add(lnkbtn);

            lnkbtn = new LinkButton();
            lnkbtn.ID = "lnkbtnxt";
            lnkbtn.Text = "Next";
            lnkbtn.CssClass = "sbmnulnks";
            nvlnks.Controls.Add(lnkbtn);

            hypr = new HyperLink();
            hypr.ID = "hyprsmmry";
            hypr.Text = "Summary";
            hypr.CssClass = "sbmnulnks";
            nvlnks.Controls.Add(hypr);

            Label lbl;
            lbl = new Label();
            lbl.ID = "lblpgindx";
            lbl.Text = "1/3";
            lbl.CssClass = "indxlbl";
            nvlnks.Controls.Add(lbl);

            hypr = new HyperLink();
            hypr.ID = "lnksrch";
            hypr.Text = "Search";
            hypr.CssClass = "nvlnks";
            hypr.NavigateUrl = "srchcase.aspx";
            mnulnks.Controls.Add(hypr);
        }

        public static void FillProvinceDropDown(DropDownList prvnc, Int16 i)
        {
            prvnc.Items.Clear();
            prvnc.Items.Add(new ListItem("UnSpecified", "0"));

            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
            {
                List<VwProvince> rws = db.VwProvinces.ToList();
                foreach (VwProvince rw in rws)
                    prvnc.Items.Add(new ListItem(rw.province, rw.id.ToString()));
            }

            if (prvnc.Items.FindByValue(i.ToString()) != null)
                prvnc.SelectedValue = i.ToString();
        }

        public static void FillDistrictsDropDown(DropDownList dstrct, Int16 i)
        {            
            dstrct.Items.Clear();
            dstrct.Items.Add(new ListItem("UnSpecified", "0"));

            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
            {
                if (i == 0)
                {
                    VwProvince prvnc = db.VwProvinces.OrderBy(obj => obj.province).FirstOrDefault();
                    if (prvnc != null) i = prvnc.id;
                }

                List<VwDistrict> rws = (from rw in db.VwDistricts where rw.province_id == i select rw).ToList();
                if (rws.Count != 0)
                    foreach (VwDistrict d in rws)
                        dstrct.Items.Add(new ListItem(d.district, d.id.ToString()));
            }
        }

        public static void FillDistrictsDropDown2(DropDownList dstrct, Int16 p)
        {            
            dstrct.Items.Clear();
            dstrct.Items.Add(new ListItem("UnSpecified", "0"));

            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
            {
                List<VwDistrict> rws = (from rw in db.VwDistricts where rw.province_id == p select rw).ToList();
                if (rws.Count != 0)
                    foreach (VwDistrict d in rws)
                        dstrct.Items.Add(new ListItem(d.district, d.id.ToString()));
            }
        }

        public static void FillDistrictsDropDown3(DropDownList dstrct, Int16 p, Int16 d)
        {            
            dstrct.Items.Clear();            
            dstrct.Items.Add(new ListItem("UnSpecified", "0"));

            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
            {
                List<VwDistrict> rws = (from rw in db.VwDistricts where rw.province_id == p select rw).ToList();
                if (rws.Count != 0)
                    foreach (VwDistrict x in rws)
                        dstrct.Items.Add(new ListItem(x.district, x.id.ToString()));
            }

            if (dstrct.Items.FindByValue(d.ToString()) != null) dstrct.SelectedValue = d.ToString();
        }

        public static void FillWardsDropDown(DropDownList wd, Int16 prv, Int16 dstrct, String wdno)
        {
            wd.Items.Clear();
            wd.Items.Add(new ListItem("UnSpecified", "0"));

            if (dstrct != 0 && prv != 0)
                using (IncDataClassesDataContext db = new IncDataClassesDataContext())
                {
                    List<Ward> rws = (from rw in db.Wards where rw.province_pcode == prv && rw.district_pcode == dstrct orderby rw.ward_no select rw).Distinct().ToList();
                    if (rws.Count != 0)
                        foreach (Ward obj in rws)
                            wd.Items.Add(new ListItem(obj.ward_no.ToString(), obj.ward_no.ToString()));
                }

            if (wd.Items.FindByValue(wdno) != null) wd.SelectedValue = wdno;
        }

        public static void FillWardsDropDown(DropDownList wd, Int16 dstrct)
        {
            wd.Items.Clear();
            wd.Items.Add(new ListItem("UnSpecified", "0"));

            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
                foreach (Ward obj in db.Wards.Where(obj => obj.district_pcode == dstrct).ToList())
                    if (!obj.ward_no.ToString().Equals("0"))
                        wd.Items.Add(new ListItem(obj.ward_no.ToString(), obj.ward_no.ToString()));                   
        }

        public static void FillPerpetratorsDropDown(DropDownList drp, String vl)
        {
            drp.Items.Clear();            

            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
                foreach (TypePerpetrator pep in db.TypePerpetrators.ToList())
                    drp.Items.Add(new ListItem(pep.type_perpetrator, pep.id.ToString()));

            if (drp.Items.FindByValue(vl) != null) drp.SelectedValue = vl;
        }

        public static void FillCausesDropDown(DropDownList drp, String vl)
        {
            drp.Items.Clear();            

            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
                foreach (TypeCause cas in db.TypeCauses.ToList())
                    drp.Items.Add(new ListItem(cas.type_cause, cas.id.ToString()));

            if (drp.Items.FindByValue(vl) != null) drp.SelectedValue = vl;
        }

        public static void FillHalDropDown(DropDownList drp, String vl)
        {
            drp.Items.Clear();
            drp.Items.Add(new ListItem("N/A", "0"));
            String shal = IncUtilxs.GetDictionaryKeyVal("hal");
            List<Dictionary<String, String>> hals = IncUtilxs.DictKeyValString(shal, ';', ',');
            if (hals.Count != 0)
                foreach (Dictionary<String, String> dct in hals) drp.Items.Add(new ListItem(dct["val"], dct["key"]));

            if (drp.Items.FindByValue(vl) != null) drp.SelectedValue = vl;
        }

        public static List<Dictionary<String, String>> GetHalDictList()
        {
            String shal = IncUtilxs.GetDictionaryKeyVal("hal");
            List<Dictionary<String, String>> hals = IncUtilxs.DictKeyValString(shal, ';', ',');

            if (hals.Count == 0) hals = null;

            return hals;
        }

        public static void FillCountryDropDown(DropDownList cntry, String s)
        {
            cntry.Items.Clear();

            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
            {
                List<Country> rws = db.Countries.ToList();
                foreach (Country rw in rws)
                    cntry.Items.Add(new ListItem(rw.country, rw.country_code));
            }

            if (cntry.Items.Count == 0)
                cntry.Items.Add(new ListItem("N/A", "None"));

            if (cntry.Items.FindByValue(s) != null)
                cntry.SelectedValue = s;
        }

        public static String GetParentLabel(Int32 p, Int32 c)
        {
            String s = String.Empty;

            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
            {
                VwCigParent rw = db.VwCigParents.FirstOrDefault(obj => obj.id == c);
                if (rw != null)
                {
                    if (!String.IsNullOrEmpty(rw.individual_name) && !String.IsNullOrEmpty(rw.cig_label) && !String.IsNullOrEmpty(rw.parent_population_group))
                        s = rw.individual_name + ": " + rw.cig_label + ": " + rw.parent_population_group;
                    else
                        if (rw.parent_id == 0)
                        {
                            s = "Root";
                            if (!String.IsNullOrEmpty(rw.individual_name))
                                s += ": " + rw.individual_name;
                            else
                                if (!String.IsNullOrEmpty(rw.child_cig_label))
                                    s += ": " + rw.child_cig_label;

                            if (!String.IsNullOrEmpty(rw.cig_label))
                                s += ": " + rw.cig_label;

                            if (!String.IsNullOrEmpty(rw.parent_population_group))
                                s += ": " + rw.parent_population_group;
                            else
                                if (!String.IsNullOrEmpty(rw.population_group))
                                    s += ": " + rw.population_group;
                        }
                        else
                            if (!String.IsNullOrEmpty(rw.child_cig_label) && !String.IsNullOrEmpty(rw.cig_label) && String.IsNullOrEmpty(rw.individual_name))
                                s = rw.child_cig_label + ": " + rw.population_group + ": " + rw.cig_label;
                }

                if (String.IsNullOrEmpty(s))
                {
                    rw = db.VwCigParents.FirstOrDefault(obj => obj.id == p);
                    if (rw != null)
                    {
                        if (!String.IsNullOrEmpty(rw.child_cig_label))
                            s = rw.child_cig_label;

                        if (!String.IsNullOrEmpty(rw.population_group))
                            s = !String.IsNullOrEmpty(s) ? s + ": " + rw.population_group : rw.population_group;
                        else
                            if (!String.IsNullOrEmpty(rw.parent_population_group))
                                s = !String.IsNullOrEmpty(s) ? s + ": " + rw.parent_population_group : rw.parent_population_group;

                        if (!String.IsNullOrEmpty(rw.cig_label))
                            s = !String.IsNullOrEmpty(s) ? s + ": " + rw.cig_label : rw.cig_label;
                    }
                }
            }

            if (String.IsNullOrEmpty(s)) s = "XXXXXXXXXX-XXXXXXXXXX";

            return s;
        }

        public static string GetMd5Hash(MD5 md5Hash, string input)
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
                sBuilder.Append(data[i].ToString("x2"));

            return sBuilder.ToString();
        }

        public static bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
        {
            string hashOfInput = GetMd5Hash(md5Hash, input);

            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static String MystifyUser(String u)
        {
            String e = String.Empty, s = String.Empty;

            e = String.Format("{0}{1}{2}", DateTime.Now.ToString("HHMMyyyydd"), u.Reverse(), DateTime.Now.ToString("ssHHmm"));
            using (MD5 md5hash = MD5.Create()) s = GetMd5Hash(md5hash, e);                    

            return s;
        }

        public static List<String> GetAspExtensions()
        {
            List<String> ext = new List<string>();

            ext.Add(".aspx");

            return ext;
        }

        public static String SilenceZero(Object vl)
        {
            String s = String.Empty;

            if (vl != null)                            
            {
                Int32 i = 0;
                Int32.TryParse(vl.ToString(), out i);
                if (i != 0) s = i.ToString();
            }

            return s;
        }

        public static String SilenceDecZero(Object vl)
        {
            List<String> lst = new List<string>() { "0", "0.00", "0.0" };
            String s = String.Empty;

            if (vl != null)
                if (!lst.Contains(vl.ToString())) s = vl.ToString();

            return s;
        }

        public static String SQLDate(String sdte)
        {
            String s = String.Empty;

            if (IsDate(sdte))
                s = String.Format("{0}-{1}-{2}", sdte.Substring(6, 4), sdte.Substring(3, 2), sdte.Substring(0, 2));

            if (s.Length != 0) s = s.Replace('/', '-');

            return s;
        }

        public static Boolean IsDate(String s)
        {
            Boolean b = true;

            if (!String.IsNullOrEmpty(s))
                try
                {
                    DateTime dt = DateTime.ParseExact(s, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                }
                catch
                {
                    b = false;
                }
            else
                b = false;

            return b;
        }

        public static Boolean IsDate(String s, String sfmt)
        {
            Boolean b = true;

            if (!String.IsNullOrEmpty(s))
                try
                {
                    DateTime dt = DateTime.ParseExact(s, sfmt, System.Globalization.CultureInfo.InvariantCulture);
                }
                catch
                {
                    b = false;
                }
            else
                b = false;

            return b;
        }
    }

    public class CaseRecord
    {
        private Hashtable cs = null;
        private List<Hashtable> cig = null;
        private List<Hashtable> cas = null;
        private List<Hashtable> pep = null;
        private List<Hashtable> ass = null;

        private List<PrimaryPerpetrator> prm_pem = null;
        private List<PrimaryCause> prm_cau = null;

        private List<Hashtable> cidlt = null;
        private List<Hashtable> cigdlt = null;
        private List<Hashtable> assdlt = null;

        private String ci_mask = String.Empty;
        private String svrty_drp_mask = String.Empty;
        private String cigrp_mask = String.Empty;
        private String cau_chk_mask = String.Empty;
        private String pep_chk_mask = String.Empty;
        private String pep_nm_mask = String.Empty;        

        private String othrinfsrcfld = String.Empty;
        private String othremrgncyfld = String.Empty;

        private String othrcimask = String.Empty;
        private String othrcisvrtymsk = String.Empty;

        private String othrcigmask = String.Empty;
        private String othrpepmask = String.Empty;
        private String othrcaumask = String.Empty;
        private String othrassmask = String.Empty;

        private String chkprmpep_mask = String.Empty;
        private String chkrprmcau_mask = String.Empty;
        private String othrprmpep_mask = String.Empty;
        private String othrprmcau_mask = String.Empty;

        private Byte othrinfsrcid = 0;
        private Byte othremrgncyid = 0;
        private Byte othrsvrtyid = 1;
        private Int16 otheroptionid = 1000;

        private void InitItems()
        {
            this.cs = new Hashtable();
            this.cig = new List<Hashtable>();
            this.cas = new List<Hashtable>();
            this.pep = new List<Hashtable>();
            this.ass = new List<Hashtable>();

            this.cidlt = new List<Hashtable>();
            this.cigdlt = new List<Hashtable>();
            this.assdlt = new List<Hashtable>();

            this.ci_mask = IncUtilxs.GetDictionaryKeyVal("incident_mask");
            this.cigrp_mask = IncUtilxs.GetDictionaryKeyVal("incident_group_mask");
            this.svrty_drp_mask = IncUtilxs.GetDictionaryKeyVal("incident_severity_mask");
            this.cau_chk_mask = IncUtilxs.GetDictionaryKeyVal("cause_checkbox_mask");
            this.pep_chk_mask = IncUtilxs.GetDictionaryKeyVal("perpetrator_checkbox_mask");
            this.pep_nm_mask = IncUtilxs.GetDictionaryKeyVal("number_peps_mask");            

            this.othrinfsrcfld = IncUtilxs.GetDictionaryKeyVal("other_infosource_textbox");
            this.othremrgncyfld = IncUtilxs.GetDictionaryKeyVal("other_emergency_textbox");

            this.othrcimask = IncUtilxs.GetDictionaryKeyVal("other_incident_textbox_mask");
            this.othrcisvrtymsk = IncUtilxs.GetDictionaryKeyVal("other_incident_severity_mask");
            this.othrcigmask = IncUtilxs.GetDictionaryKeyVal("other_incident_group_textbox_mask");
            this.othrpepmask = IncUtilxs.GetDictionaryKeyVal("other_perpetrator_textbox_mask");
            this.othrcaumask = IncUtilxs.GetDictionaryKeyVal("other_cause_textbox_mask");
            this.othrassmask = IncUtilxs.GetDictionaryKeyVal("other_assist_mask");

            this.chkprmpep_mask = IncUtilxs.GetDictionaryKeyVal("primary_perpetrator_mask");
            this.chkrprmcau_mask = IncUtilxs.GetDictionaryKeyVal("primary_causes_mask");
            this.othrprmpep_mask = IncUtilxs.GetDictionaryKeyVal("other_primary_perpetrator_mask");
            this.othrprmcau_mask = IncUtilxs.GetDictionaryKeyVal("other_primary_causes_mask");

            Byte.TryParse(IncUtilxs.GetDictionaryKeyVal("case_info_source_other_id"), out this.othrinfsrcid);
            Byte.TryParse(IncUtilxs.GetDictionaryKeyVal("case_emergency_level_other_id"), out this.othremrgncyid);
            Byte.TryParse(IncUtilxs.GetDictionaryKeyVal("case_incident_severity_other_id"), out this.othrsvrtyid);
            Int16.TryParse(IncUtilxs.GetDictionaryKeyVal("other_option_id"), out this.otheroptionid);

            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
            {
                List<VwTableColumn> rws = (from rw in db.VwTableColumns where rw.TABLE_NAME.Equals("v_case_row") select rw).ToList();
                if (rws.Count != 0)
                {
                    String[] dtyps = { "tinyint", "smallint", "bit", "int" };

                    foreach (VwTableColumn rw in rws)
                    {
                        Object vl = String.Empty;

                        if (dtyps.Contains<String>(rw.DATA_TYPE)) vl = 0;
                        this.cs.Add(rw.COLUMN_NAME, vl);
                    }

                    this.cs.Add("other_info_source_comment", String.Empty);
                    this.cs.Add("other_emergency_level_comment", String.Empty);
                    this.cs.Add("other_table_name", "t_cases");
                }
            }
        }

        public CaseRecord()
        {
            this.InitItems();
        }

        public CaseRecord(Int32 csid, List<Int16> orgs)
        {
            this.InitItems();

            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
            {
                VwCaseRow rw = db.VwCaseRows.FirstOrDefault(obj => obj.id == csid && orgs.Contains(obj.org_unit_id));
                if (rw != null)
                {
                    this.cs["id"] = rw.id; this.cs["case_no"] = rw.case_no; this.cs["comments"] = rw.comments; this.cs["country"] = rw.country; this.cs["country_code"] = rw.country_code; this.cs["country_name"] = rw.country_name;
                    this.cs["date_reported"] = rw.date_reported; this.cs["district_id"] = rw.district_id; this.cs["district"] = rw.district; this.cs["emergency_level"] = rw.emergency_level; this.cs["emergency_level_id"] = rw.emergency_level_id;
                    this.cs["info_source"] = rw.info_source; this.cs["info_source_id"] = rw.info_source_id; this.cs["info_verify_scale"] = rw.info_verify_scale; this.cs["inserted_by"] = rw.inserted_by; this.cs["inserted_on"] = rw.inserted_on;
                    this.cs["is_verified"] = rw.is_verified; this.cs["num_affected"] = rw.num_affected; this.cs["org_unit"] = rw.org_unit; this.cs["org_unit_id"] = rw.org_unit_id; this.cs["place_name"] = rw.place_name;
                    this.cs["org_unit_prefix"] = rw.org_unit_prefix; this.cs["province"] = rw.province; this.cs["province_id"] = rw.province_id; this.cs["prv_code"] = rw.prv_code; this.cs["ref_no"] = rw.ref_no; this.cs["updated_by"] = rw.updated_by;
                    this.cs["updated_on"] = rw.updated_on; this.cs["verified_on"] = rw.verified_on; this.cs["ward_no"] = rw.ward_no; this.cs["incident_date"] = rw.incident_date; this.cs["gps_east"] = rw.gps_east; this.cs["gps_south"] = rw.gps_south;

                    this.cs["other_info_source_comment"] = !String.IsNullOrEmpty(rw.other_info_source) ? rw.other_info_source : String.Empty;
                    this.cs["other_emergency_level_comment"] = !String.IsNullOrEmpty(rw.other_emergency_level) ? rw.other_emergency_level : String.Empty;

                    this.cs["num_house_affected"] = rw.num_house_affected; this.cs["prev_case_no"] = rw.prev_case_no; this.cs["prev_province_id"] = rw.prev_province_id; this.cs["prev_district_id"] = rw.prev_district_id; this.cs["prev_country"] = rw.prev_country;
                    this.cs["prev_ward_no"] = rw.prev_ward_no; this.cs["prev_place_name"] = rw.prev_place_name; this.cs["prev_gps_east"] = rw.prev_gps_east; this.cs["prev_gps_south"] = rw.prev_gps_south; this.cs["cultivation_land"] = rw.cultivation_land;
                    this.cs["current_place_legal"] = rw.current_place_legal; this.cs["vulnerability_id"] = rw.vulnerability_id; this.cs["displacement_status_id"] = rw.displacement_status_id;

                    this.cs["incident_type_id"] = rw.incident_type_id; this.cs["severity_level"] = rw.severity_level;
                    this.cs["other_incident_type"] = rw.other_incident_type;  this.cs["other_severity_level"] = rw.other_severity_level;
                    this.cs["other_displacement_status"] = rw.other_displacement_status;

                    this.cs["num_individuals"] = rw.num_individuals; this.cs["num_households"] = rw.num_households;
                    this.cs["num_communities"] = rw.num_communities; this.cs["hal"] = rw.hal;
                    this.cs["closed"] = rw.closed; this.cs["closed_comments"] = rw.closed_comments;

                    this.CaseRow["ext_incident_type"] = rw.ext_incident_type != null ? rw.ext_incident_type : String.Empty;
                    this.CaseRow["incident_type"] = rw.incident_type != null ? rw.incident_type : String.Empty;
                }
            }            
        }

        public void InitCaseGroupIncidentLists()
        {
            this.cig = new List<Hashtable>();
            this.pep = new List<Hashtable>();
            this.cas = new List<Hashtable>();

            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
            {
                List<VwTypeIncident> rwi = db.VwTypeIncidents.ToList();
                if (rwi.Count != 0)
                    foreach (VwTypeIncident rw in rwi)
                    {
                        Hashtable obj = new Hashtable();

                        obj.Add("id", 0);
                        obj.Add("case_incident_group_id", 0);
                        obj.Add("type_incident_id", rw.id);
                        obj.Add("incident_type", rw.incident_type);
                        obj.Add("inc_severity_id", 0);
                        obj.Add("selected", false);
                        obj.Add("other_comment", String.Empty);
                        obj.Add("other_severity_comment", String.Empty);
                        obj.Add("other_table_name", "t_case_incidents");
                        obj.Add("other_distinguish_id", 0);
                        this.cig.Add(obj);

                        List<VwTypePerpetrator> rwp = db.VwTypePerpetrators.ToList();
                        if (rwp.Count != 0)
                            foreach (VwTypePerpetrator rp in rwp)
                            {
                                obj = new Hashtable();

                                obj.Add("id", 0);
                                obj.Add("case_incident_id", 0);
                                obj.Add("type_perpetrator_id", rp.id);
                                obj.Add("type_perpetrator", rp.type_perpetrator);
                                obj.Add("incident_type", rw.incident_type);
                                obj.Add("type_incident_id", rw.id);
                                obj.Add("selected", false);
                                obj.Add("other_comment", String.Empty);
                                obj.Add("other_table_name", "t_perpetrators");
                                obj.Add("other_distinguish_id", 0);
                                obj.Add("other_option_id", this.otheroptionid);
                                obj.Add("num", 0);
                                this.pep.Add(obj);
                            }

                        List<VwTypeCause> rwc = db.VwTypeCauses.ToList();
                        if (rwc.Count != 0)
                            foreach (VwTypeCause rc in rwc)
                            {
                                obj = new Hashtable();

                                obj.Add("id", 0);
                                obj.Add("case_incident_id", 0);
                                obj.Add("type_cause_id", rc.id);
                                obj.Add("type_cause", rc.type_cause);
                                obj.Add("incident_type", rw.incident_type);
                                obj.Add("type_incident_id", rw.id);
                                obj.Add("selected", false);
                                obj.Add("other_comment", String.Empty);
                                obj.Add("other_table_name", "t_causes");
                                obj.Add("other_distinguish_id", 0);
                                obj.Add("other_option_id", this.otheroptionid);
                                this.cas.Add(obj);
                            }
                    }
            }
        }

        public void CigIncidents(Int32 cigid)
        {
            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
            {
                List<VwCaseIncidentOther> rwi = (from obj in db.VwCaseIncidentOthers where obj.case_incident_group_id == cigid select obj).ToList();
                if (rwi.Count != 0)
                    foreach (VwCaseIncidentOther rw in rwi)
                    {
                        foreach (Hashtable hi in this.cig)
                            if (Int16.Parse(hi["type_incident_id"].ToString()) == rw.type_incident_id)
                            {
                                hi["selected"] = true;
                                hi["id"] = rw.id;
                                hi["case_incident_group_id"] = rw.case_incident_group_id;
                                hi["inc_severity_id"] = rw.inc_severity_id;
                                hi["other_comment"] = !String.IsNullOrEmpty(rw.other_incident) ? rw.other_incident : String.Empty;
                                hi["other_severity_comment"] = !String.IsNullOrEmpty(rw.other_incident_severity) ? rw.other_incident_severity : String.Empty;
                            }

                        List<VwCaseIncidentPerpetratorOther> rwp = (from obj in db.VwCaseIncidentPerpetratorOthers where obj.case_incident_id == rw.id select obj).ToList();
                        if (rwp.Count != 0)
                            foreach (VwCaseIncidentPerpetratorOther pp in rwp)
                                foreach (Hashtable hp in this.pep)
                                    if (pp.type_perpetrator_id == Int16.Parse(hp["type_perpetrator_id"].ToString()) && pp.type_incident_id == Int16.Parse(hp["type_incident_id"].ToString()))
                                    {
                                        hp["id"] = pp.id;
                                        hp["case_incident_id"] = pp.case_incident_id;
                                        hp["num"] = pp.num.Value;
                                        hp["selected"] = true;
                                        hp["other_comment"] = !String.IsNullOrEmpty(pp.other_perpetrator) ? pp.other_perpetrator : String.Empty;
                                        break;
                                    }

                        List<VwCaseIncidentCauseOther> rwc = (from obj in db.VwCaseIncidentCauseOthers where obj.case_incident_id == rw.id select obj).ToList();
                        if (rwc.Count != 0)
                            foreach (VwCaseIncidentCauseOther ica in rwc)
                                foreach (Hashtable hc in this.cas)
                                    if (ica.type_cause_id == Int16.Parse(hc["type_cause_id"].ToString()) && ica.type_incident_id == Int16.Parse(hc["type_incident_id"].ToString()))
                                    {
                                        hc["id"] = ica.id;
                                        hc["case_incident_id"] = ica.case_incident_id;
                                        hc["selected"] = true;
                                        hc["other_comment"] = !String.IsNullOrEmpty(ica.other_cause) ? ica.other_cause : String.Empty;
                                        break;
                                    }
                    }
            }
        }

        public Hashtable CaseRow
        {
            get { return this.cs; }
            set { this.cs = value; }
        }

        public void GrabCaseGroupIncidents(NameValueCollection prms, Int32 cigid)
        {
            Regex rgxp = new Regex("^\\d+$");

            String strinf = String.Empty;
            String stremg = String.Empty;
            String strothr = String.Empty;
            String strothrfld = String.Empty;
            String snm = String.Empty;

            String ky = String.Empty;
            String vl = String.Empty;
            Boolean bvl = false;

            Int16 rid = 0;

            foreach (Hashtable h in this.cig)
            {
                h["selected"] = false; h["other_comment"] = String.Empty; h["other_severity_comment"] = String.Empty; h["inc_severity_id"] = 0;
            }
            foreach (Hashtable h in this.pep)
            {
                h["selected"] = false; h["other_comment"] = String.Empty; h["num"] = 0;
            }
            foreach (Hashtable h in this.cas)
            {
                h["selected"] = false; h["other_comment"] = String.Empty;
            }

            if (this.cs.Count != 0) Int16.TryParse(this.cs["id"].ToString(), out rid);

            this.cidlt = new List<Hashtable>();

            foreach (Hashtable p in this.pep)
            {
                snm = "0";
                ky = String.Format(this.pep_nm_mask, p["type_incident_id"].ToString(), p["type_perpetrator_id"].ToString());
                vl = IncUtilxs.GetRequestParamValue(prms, ky);
                if (rgxp.IsMatch(vl)) snm = vl.Trim();

                ky = String.Format(this.pep_chk_mask, p["type_incident_id"].ToString(), p["type_perpetrator_id"].ToString());
                vl = IncUtilxs.GetRequestParamValue(prms, ky);
                bvl = !String.IsNullOrEmpty(vl);

                strothrfld = String.Format(this.othrpepmask, p["type_incident_id"].ToString(), p["type_perpetrator_id"].ToString());
                strothr = IncUtilxs.GetRequestParamValue(prms, strothrfld);
                if (!String.IsNullOrEmpty(strothr))
                {
                    p["other_comment"] = strothr.Trim(); bvl = true;
                }
                else
                    if (Int16.Parse(p["type_perpetrator_id"].ToString()) == this.OtherOptionID)
                        bvl = false;

                if (Int32.Parse(snm) > 0 && !bvl) bvl = true;

                p["selected"] = bvl;
            }

            foreach (Hashtable a in this.cas)
            {
                ky = String.Format(this.cau_chk_mask, a["type_incident_id"].ToString(), a["type_cause_id"].ToString());
                vl = IncUtilxs.GetRequestParamValue(prms, ky);
                bvl = !String.IsNullOrEmpty(vl);

                strothrfld = String.Format(this.othrcaumask, a["type_incident_id"].ToString(), a["type_cause_id"].ToString());
                strothr = IncUtilxs.GetRequestParamValue(prms, strothrfld);
                if (!String.IsNullOrEmpty(strothr))
                {
                    a["other_comment"] = strothr.Trim(); bvl = true;
                }
                else
                    if (Int16.Parse(a["type_cause_id"].ToString()) == this.otheroptionid)
                        bvl = false;

                a["selected"] = bvl;
            }

            foreach (Hashtable hi in this.cig)
            {
                hi["case_incident_group_id"] = cigid;

                ky = String.Format(this.svrty_drp_mask, hi["type_incident_id"]);
                vl = IncUtilxs.GetRequestParamValue(prms, ky);
                if (!String.IsNullOrEmpty(vl))
                {
                    hi["inc_severity_id"] = vl;
                    if (!Boolean.Parse(hi["selected"].ToString()) && (Int16.Parse(vl)) != 0)
                        hi["selected"] = true;
                }

                strothrfld = String.Format(this.othrcisvrtymsk, hi["type_incident_id"].ToString());
                strothr = IncUtilxs.GetRequestParamValue(prms, strothrfld);
                if (!string.IsNullOrEmpty(strothr))
                {
                    hi["other_severity_comment"] = strothr.Trim();
                    hi["inc_severity_id"] = this.otheroptionid;
                    if (!Boolean.Parse(hi["selected"].ToString()))
                        hi["selected"] = true;
                }
                else
                    if (Int16.Parse(hi["inc_severity_id"].ToString()) == this.otheroptionid)
                    {
                        hi["inc_severity_id"] = 0;
                        hi["selected"] = false;
                    }

                strothrfld = String.Format(this.othrcimask, hi["type_incident_id"].ToString());
                strothr = IncUtilxs.GetRequestParamValue(prms, strothrfld);
                if (!String.IsNullOrEmpty(strothr))
                {
                    hi["other_comment"] = strothr.Trim();
                    hi["selected"] = true;
                }

                if (!Boolean.Parse(hi["selected"].ToString()))
                {
                    bvl = false;
                    foreach (Hashtable hp in this.pep)
                        if ((Int16.Parse(hp["type_incident_id"].ToString()) == Int16.Parse(hi["type_incident_id"].ToString())) && Boolean.Parse(hp["selected"].ToString()))
                        {
                            bvl = true; break;
                        }

                    hi["selected"] = bvl;
                    if (bvl) continue;

                    bvl = false;
                    foreach (Hashtable hc in this.cas)
                        if ((Int16.Parse(hc["type_incident_id"].ToString()) == Int16.Parse(hi["type_incident_id"].ToString())) && Boolean.Parse(hc["selected"].ToString()))
                        {
                            bvl = true; break;
                        }

                    hi["selected"] = bvl;
                }
            }

            foreach (Hashtable hi in this.cig)
                if (!Boolean.Parse(hi["selected"].ToString()) && (Int32.Parse(hi["id"].ToString()) != 0)) this.cidlt.Add(hi);

            // Capture No. of Perpetrators
            foreach (Hashtable hp in this.pep)
                if (Boolean.Parse(hp["selected"].ToString()))
                {                    
                    ky = String.Format(this.pep_nm_mask, hp["type_incident_id"].ToString(), hp["type_perpetrator_id"].ToString());
                    vl = IncUtilxs.GetRequestParamValue(prms, ky);
                    if (rgxp.IsMatch(vl)) hp["num"] = Int32.Parse(vl);
                }
        }

        public void GrabCaseAssistance(NameValueCollection prms)
        {
            Regex rgxp = new Regex("^\\d+$");
            Regex rgxpd = new Regex("^\\d+\\.\\d{1,4}$");

            String prvdrmsk = IncUtilxs.GetDictionaryKeyVal("assist_provider_mask");
            String dnrmsk = IncUtilxs.GetDictionaryKeyVal("assist_donor_mask");
            String prjmsk = IncUtilxs.GetDictionaryKeyVal("assist_project_mask");            
            String nummsk = IncUtilxs.GetDictionaryKeyVal("assist_num_mask");
            String qtymsk = IncUtilxs.GetDictionaryKeyVal("assist_qty_mask");
            String bnfcrymsk = IncUtilxs.GetDictionaryKeyVal("assist_beneficiary_mask");
            String dscrptmsk = IncUtilxs.GetDictionaryKeyVal("assist_descript_mask");

            String ky = String.Empty, vl = String.Empty;
            Boolean b = false;

            foreach (Hashtable ha in this.ass)
            {
                ha["selected"] = false;

                ky = String.Format(prvdrmsk, ha["type_assistance_id"].ToString(), ha["case_incident_group_id"].ToString());
                vl = IncUtilxs.GetRequestParamValue(prms, ky);
                if (!String.IsNullOrEmpty(vl))
                    if (rgxp.IsMatch(vl))
                        if (Int32.Parse(vl) != 0)
                        {
                            ha["provider_id"] = Int32.Parse(vl);
                            ha["selected"] = true;
                        }

                ky = String.Format(dnrmsk, ha["type_assistance_id"].ToString(), ha["case_incident_group_id"].ToString());
                vl = IncUtilxs.GetRequestParamValue(prms, ky);
                if (!String.IsNullOrEmpty(vl))
                    if (rgxp.IsMatch(vl))
                        if (Int32.Parse(vl) != 0)
                        {
                            ha["donor_id"] = Int16.Parse(vl);
                            ha["selected"] = true;
                        }

                ky = String.Format(prjmsk, ha["type_assistance_id"].ToString(), ha["case_incident_group_id"].ToString());
                vl = IncUtilxs.GetRequestParamValue(prms, ky);
                if (!String.IsNullOrEmpty(vl))
                    if (rgxp.IsMatch(vl))
                        if (Int32.Parse(vl) != 0)
                        {
                            ha["project_id"] = Int32.Parse(vl);
                            ha["selected"] = true;
                        }

                ky = String.Format(nummsk, ha["type_assistance_id"].ToString(), ha["case_incident_group_id"].ToString());
                vl = IncUtilxs.GetRequestParamValue(prms, ky);
                if (!String.IsNullOrEmpty(vl))
                    if (rgxp.IsMatch(vl))
                        if (Int32.Parse(vl) != 0)
                        {
                            ha["num"] = Int32.Parse(vl);
                            ha["selected"] = true;
                        }

                ky = String.Format(qtymsk, ha["type_assistance_id"].ToString(), ha["case_incident_group_id"].ToString());
                vl = IncUtilxs.GetRequestParamValue(prms, ky);
                if (!String.IsNullOrEmpty(vl))
                    if (rgxp.IsMatch(vl) || rgxpd.IsMatch(vl))
                        if (Decimal.Parse(vl) != 0)
                        {
                            ha["qty"] = Decimal.Parse(vl);
                            ha["selected"] = true;
                        }

                ky = String.Format(dscrptmsk, ha["type_assistance_id"].ToString(), ha["case_incident_group_id"].ToString());
                vl = IncUtilxs.GetRequestParamValue(prms, ky);
                if (!String.IsNullOrEmpty(vl))
                {
                    ha["descript"] = vl.Trim();
                    ha["selected"] = true;
                }

                ky = String.Format(bnfcrymsk, ha["type_assistance_id"].ToString(), ha["case_incident_group_id"].ToString());
                vl = IncUtilxs.GetRequestParamValue(prms, ky);
                if (!String.IsNullOrEmpty(vl))
                    if (rgxp.IsMatch(vl))
                        if (Int16.Parse(vl) != 0)
                        {
                            ha["population_group_id"] = Int16.Parse(vl);
                            ha["selected"] = true;
                        }

                ky = String.Format(this.othrassmask, ha["type_assistance_id"].ToString(), ha["case_incident_group_id"].ToString());
                vl = IncUtilxs.GetRequestParamValue(prms, ky);
                b = !String.IsNullOrEmpty(vl);
                if (b)
                {
                    ha["other_comment"] = vl;
                    ha["selected"] = b;
                }
                else
                    if (Int16.Parse(ha["type_assistance_id"].ToString()) == this.otheroptionid)
                        ha["selected"] = false;
            }
        }

        public Boolean IsAssistanceChecked()
        {
            Boolean ok = false;

            if (this.ass != null)
                if (this.ass.Count != 0)
                    foreach (Hashtable ha in this.ass)
                    {
                        ok = Boolean.Parse(ha["selected"].ToString());
                        if (ok) break;
                    }

            return ok;
        }

        public void WriteCaseRecord(String usr, ref String msg, ref Boolean ok)
        {
            String csno = String.Empty;
            msg = String.Empty;

            int? rid = 0;
            Int32 i = 0, rslt = 0;

            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
            {
                Int32.TryParse(this.cs["id"].ToString(), out i); rid = i;

                try
                {
                    rslt = db.p_write_case(ref rid, Int16.Parse(this.cs["province_id"].ToString()), Int16.Parse(this.cs["district_id"].ToString()), this.cs["country"].ToString(), Int16.Parse(this.cs["org_unit_id"].ToString()),
                        Int16.Parse(this.cs["num_individuals"].ToString()), Int32.Parse(this.cs["ward_no"].ToString()), Int16.Parse(this.cs["info_source_id"].ToString()), Byte.Parse(this.cs["info_verify_scale"].ToString()), this.cs["date_reported"].ToString(),
                        this.cs["incident_date"].ToString(), this.cs["ref_no"].ToString(), this.cs["place_name"].ToString(), Int16.Parse(this.cs["emergency_level_id"].ToString()), this.cs["gps_east"].ToString(), this.cs["gps_south"].ToString(),
                        Boolean.Parse(this.cs["is_verified"].ToString()), this.cs["verified_on"].ToString(), this.cs["comments"].ToString(), usr, this.cs["other_info_source_comment"].ToString(), this.cs["other_emergency_level_comment"].ToString(), ref csno,
                        Int32.Parse(this.cs["num_households"].ToString()), Int32.Parse(this.cs["num_communities"].ToString()), this.cs["prev_case_no"].ToString(), Int16.Parse(this.cs["prev_province_id"].ToString()), Int16.Parse(this.cs["prev_district_id"].ToString()), this.cs["prev_country"].ToString(), Int32.Parse(this.cs["prev_ward_no"].ToString()),
                        this.cs["prev_place_name"].ToString(), this.cs["prev_gps_east"].ToString(), this.cs["prev_gps_south"].ToString(), Boolean.Parse(this.cs["cultivation_land"].ToString()), Boolean.Parse(this.cs["current_place_legal"].ToString()), Int16.Parse(this.cs["vulnerability_id"].ToString()),
                        Int16.Parse(this.cs["displacement_status_id"].ToString()), Int16.Parse(this.cs["incident_type_id"].ToString()), Int16.Parse(this.cs["severity_level"].ToString()), this.cs["other_incident_type"].ToString(), this.cs["other_severity_level"].ToString(), this.cs["other_displacement_status"].ToString(), Byte.Parse(this.cs["hal"].ToString()),
                        Boolean.Parse(this.cs["closed"].ToString()), this.cs["closed_comments"].ToString());

                    if (i == 0)
                    {
                        i = rid.Value; this.cs["id"] = i; this.cs["case_no"] = csno;
                    }

                    ok = (i != 0);
                }
                catch (Exception xcp)
                {
                    ok = false;
                    msg = !String.IsNullOrEmpty(msg) ? msg + "!!! Error : " + xcp.Message.ToString() : "!!! Error : " + xcp.Message.ToString();
                }
            }
        }

        public Boolean AnythingToSave()
        {
            Boolean ok = false;

            foreach (Hashtable hi in this.cig)
                if (Boolean.Parse(hi["selected"].ToString()))
                {
                    ok = true;
                    break;
                }

            return ok;
        }

        public void WriteGroupIncidents(String usr, ref String msg, ref Boolean ok)
        {
            int? aid = 0;
            Int32 rslt = 0;

            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
            {
                try
                {
                    foreach (Hashtable hi in this.cig)
                    {
                        int? cid = Int32.Parse(hi["id"].ToString());

                        if (Boolean.Parse(hi["selected"].ToString()))
                        {
                            rslt = db.p_write_case_incident(ref cid, Int32.Parse(hi["case_incident_group_id"].ToString()), Int16.Parse(hi["type_incident_id"].ToString()), Int16.Parse(hi["inc_severity_id"].ToString()), hi["other_comment"].ToString(), hi["other_severity_comment"].ToString(), usr);
                            hi["id"] = cid.Value;
                        }
                        else
                            if (cid.Value != 0)
                                rslt = db.ExecuteCommand("delete from t_case_incidents where id = {0}", cid.Value);
                    }

                    foreach (Hashtable hi in this.cig)
                    {
                        foreach (Hashtable p in this.pep)
                            if (Int16.Parse(hi["type_incident_id"].ToString()) == Int16.Parse(p["type_incident_id"].ToString()) && (Boolean.Parse(hi["selected"].ToString())))
                                if (Boolean.Parse(p["selected"].ToString()))
                                    p["case_incident_id"] = Int32.Parse(hi["id"].ToString());

                        foreach (Hashtable c in this.cas)
                            if (Int16.Parse(hi["type_incident_id"].ToString()) == Int16.Parse(c["type_incident_id"].ToString()) && (Boolean.Parse(hi["selected"].ToString())))
                                if (Boolean.Parse(c["selected"].ToString()))
                                    c["case_incident_id"] = Int32.Parse(hi["id"].ToString());
                    }

                    foreach (Hashtable hi in this.cig)
                        if (Boolean.Parse(hi["selected"].ToString()))
                            rslt = db.ExecuteCommand("delete from t_perpetrators where case_incident_id = {0}; delete from t_causes where case_incident_id = {0}", Int32.Parse(hi["id"].ToString()));

                    foreach (Hashtable p in this.pep)
                        if (Boolean.Parse(p["selected"].ToString()))
                        {
                            aid = Int32.Parse(p["id"].ToString());
                            rslt = db.p_write_perpetrator(ref aid, Int32.Parse(p["case_incident_id"].ToString()), Int16.Parse(p["type_perpetrator_id"].ToString()), Int32.Parse(p["num"].ToString()), p["other_comment"].ToString(), usr);
                        }

                    foreach (Hashtable c in this.cas)
                        if (Boolean.Parse(c["selected"].ToString()))
                        {
                            aid = Int32.Parse(c["id"].ToString());
                            rslt = db.p_write_cause(ref aid, Int32.Parse(c["case_incident_id"].ToString()), Int16.Parse(c["type_cause_id"].ToString()), c["other_comment"].ToString(), usr);
                        }

                    ok = true;
                }
                catch (Exception xcp)
                {
                    ok = false;
                    msg = !String.IsNullOrEmpty(msg) ? msg + xcp.Message.ToString() : xcp.Message.ToString();
                }
            }
        }

        public void WriteAssistance(Hashtable ha, ref String msg, ref Boolean ok)
        {
            Int32 rslt = 0;
            Int32? aid = 0, i = 0;

            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
            {
                try
                {
                    aid = Int32.Parse(ha["id"].ToString());
                    rslt = db.p_write_assistance(ref aid, Int32.Parse(ha["cig"].ToString()), ha["assist_date"].ToString(), Byte.Parse(ha["assist_action_id"].ToString()), ha["comment"].ToString(), ha["user"].ToString());
                    ha["id"] = aid.Value;

                    foreach (Hashtable h in this.ass)
                    {
                        i = Int32.Parse(h["id"].ToString());
                        if (Boolean.Parse(h["selected"].ToString()))
                            db.p_write_assist_items(ref i, aid.Value, Int16.Parse(h["type_assistance_id"].ToString()), h["other_comment"].ToString(), Int16.Parse(h["provider_id"].ToString()), Int16.Parse(h["project_id"].ToString()), Int16.Parse(h["population_group_id"].ToString()), Int32.Parse(h["num"].ToString()), Decimal.Parse(h["qty"].ToString()), h["descript"].ToString());
                        else
                            if (i.Value != 0)
                                rslt = db.ExecuteCommand("delete from t_assist_items where id = {0}", i.Value);
                    }

                    ok = true;
                    msg = "Assistance record saved successfully !";
                }
                catch (Exception xcp)
                {
                    msg = String.Format("!!! Error : {0}", xcp.Message.ToString());
                    ok = false;
                }
            }
        }

        public void CollectAssistance(Int32 aid)
        {
            this.ass = new List<Hashtable>();

            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
            {
                List<VwTypeAssistance> rwa = db.VwTypeAssistances.ToList();
                if (rwa.Count != 0)
                    foreach (VwTypeAssistance rw in rwa)
                    {
                        Hashtable oba = new Hashtable();

                        oba.Add("id", 0);
                        oba.Add("case_incident_group_id", 0);
                        oba.Add("type_assistance_id", rw.id);
                        oba.Add("assistance_id", 0);
                        oba.Add("type_assistance", rw.type_assistance);                        
                        oba.Add("selected", false);
                        oba.Add("other_comment", String.Empty);
                        oba.Add("other_table_name", "t_assist_items");
                        oba.Add("other_distinguish_id", 0);
                        oba.Add("other_option_id", this.otheroptionid);
                        oba.Add("provider_id", 0);
                        oba.Add("descript", String.Empty);
                        oba.Add("donor_id", 0);
                        oba.Add("num", 0);
                        oba.Add("population_group_id", 0);
                        oba.Add("qty", 0.00);
                        oba.Add("project_id", 0);
                        this.ass.Add(oba);
                    }

                List<VwAssistanceAssist> rws = (from obj in db.VwAssistanceAssists where obj.id == aid select obj).ToList();
                if (rws != null)
                    if (rws.Count != 0)
                        foreach (VwAssistanceAssist rw in rws)
                            foreach (Hashtable h in this.ass)
                                if (rw.type_assistance_id == Int16.Parse(h["type_assistance_id"].ToString()))
                                {
                                    h["id"] = rw.assist_item_id;
                                    h["case_incident_group_id"] = rw.case_incident_group_id;
                                    h["selected"] = true;
                                    h["other_comment"] = rw.other_option_comment == null ? String.Empty : rw.other_option_comment;
                                    h["assistance_id"] = rw.id;
                                    h["qty"] = rw.qty.Value;
                                    h["provider_id"] = rw.provider_id.Value;
                                    h["project_id"] = rw.project_id.Value;
                                    h["population_group_id"] = rw.population_group_id.Value;
                                    h["num"] = rw.num.Value;
                                    h["descript"] = rw.descript;
                                    h["donor_id"] = rw.donor_id.Value;
                                }
            }
        }

        public Int16 OtherOptionID
        {
            get { return this.otheroptionid; }
        }

        public List<Hashtable> CaseGroupIncidents
        {
            get { return this.cig; }
        }

        public List<Hashtable> CaseIncidentCauses
        {
            get { return this.cas; }
        }

        public List<Hashtable> CaseIncidentPerpetrators
        {
            get { return this.pep; }
        }

        public Byte OtherInfoSourceDistinguishID
        {
            get { return this.othrinfsrcid; }
        }

        public Byte OtherEmergencyDistinguishID
        {
            get { return this.othremrgncyid; }
        }

        public Boolean IsDbRecord()
        {
            Int32 i = 0;
            Boolean ans = false;

            if (this.cs != null)
                if (this.cs.ContainsKey("id"))
                    if (this.cs["id"] != null)
                    {
                        Int32.TryParse(this.cs["id"].ToString(), out i);
                        ans = (i > 0);
                    }

            return ans;
        }

        public List<Hashtable> DeleteCaseGroupIncidents
        {
            get { return this.cidlt; }
        }

        public List<Hashtable> DeleteCaseGroupIncidentGroups
        {
            get { return this.cigdlt; }
        }

        public List<Hashtable> CaseAssistance
        {
            get { return this.ass; }
        }

        public List<PrimaryPerpetrator> GetPrimaryPerpetrators()
        {
            if (this.CaseRow != null)
                if (this.CaseRow["id"] != null)
                    using (IncDataClassesDataContext db = new IncDataClassesDataContext())
                        this.prm_pem = db.PrimaryPerpetrators.Where(obj => obj.case_id == Int32.Parse(this.CaseRow["id"].ToString())).ToList();

            return this.prm_pem;
        }

        public List<PrimaryCause> GetPrimaryCauses()
        {
            if (this.CaseRow != null)
                if (this.CaseRow["id"] != null)
                    using (IncDataClassesDataContext db = new IncDataClassesDataContext())
                        this.prm_cau = db.PrimaryCauses.Where(obj => obj.case_id == Int32.Parse(this.CaseRow["id"].ToString())).ToList();

            return this.prm_cau;
        }

        public void WritePrimaryPerpetrators(List<Hashtable> itms, String usrnm, String othrcmnt)
        {
            if ((itms != null) && (this.CaseRow != null))
                if (this.CaseRow["id"] != null)
                    using (IncDataClassesDataContext db = new IncDataClassesDataContext())
                    {
                        List<PrimaryPerpetrator> csprmp = db.PrimaryPerpetrators.Where(obj => obj.case_id == Int16.Parse(this.CaseRow["id"].ToString())).ToList();
                        if (csprmp != null)
                            if (csprmp.Count != 0)
                                foreach (PrimaryPerpetrator p in csprmp)
                                    if (!itms.Exists(hsh => Int16.Parse(hsh["type_perpetrator_id"].ToString()) == p.type_perpetrator_id))
                                        db.ExecuteCommand("delete from t_primary_perpetrators where id = {0}", p.id);

                        foreach (Hashtable hsh in itms)
                            db.p_write_primary_perpetrator(Int32.Parse(hsh["id"].ToString()), Int32.Parse(hsh["case_id"].ToString()), Int16.Parse(hsh["type_perpetrator_id"].ToString()), usrnm, Int32.Parse(hsh["num"].ToString()), othrcmnt);
                    }                    
        }

        public void WritePrimaryCauses(List<Hashtable> itms, String usrnm, String othrcmnt)
        {
            if ((itms != null) && (this.CaseRow != null))
                if (this.CaseRow["id"] != null)
                    using (IncDataClassesDataContext db = new IncDataClassesDataContext())
                    {
                        List<PrimaryCause> csprmcau = db.PrimaryCauses.Where(obj => obj.case_id == Int16.Parse(this.CaseRow["id"].ToString())).ToList();
                        if (csprmcau != null)
                            if (csprmcau.Count != 0)
                                foreach (PrimaryCause c in csprmcau)
                                    if (!itms.Exists(hsh => Int16.Parse(hsh["type_cause_id"].ToString()) == c.type_cause_id))
                                        db.ExecuteCommand("delete from t_primary_causes where id = {0}", c.id);

                        foreach (Hashtable hsh in itms)
                            db.p_write_primary_cause(Int32.Parse(hsh["id"].ToString()), Int32.Parse(hsh["case_id"].ToString()), Int16.Parse(hsh["type_cause_id"].ToString()), usrnm, othrcmnt);
                    }
        }

        public SqlDataReader GetNodeIndividuals(Int32 prntcig)
        {
            SqlDataReader rdr = null;

            Int32 csid = Int32.Parse(this.CaseRow["id"].ToString());

            String sql = String.Format("select * from v_individuals where parent_id = {0} and case_id = {1}", prntcig, csid);
            if (!this.IsDbRecord()) sql = String.Format("select * from v_cache_individuals where parent_id = {0}", prntcig);

            using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["incAdoNet"].ConnectionString))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand(sql, cnn);
                rdr = cmd.ExecuteReader();
            }

            return rdr;
        }

        public Dictionary<String, String> GetIndividual(Int32 indvid)
        {
            Dictionary<String, String> dct = null;

            String sql = String.Format("select * from v_individuals where id = {0}", indvid);
            if (!this.IsDbRecord()) sql = String.Format("select * from v_cache_individuals where id = {0}", indvid);

            using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["incAdoNet"].ConnectionString))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand(sql, cnn);
                SqlDataReader rdr = cmd.ExecuteReader();

                while(rdr.Read())
                {
                    dct = new Dictionary<string, string>();
                    for (Int16 i = 0; i < rdr.FieldCount; i++)
                        dct.Add(rdr.GetName(i), rdr[i] != null ? rdr[i].ToString() : String.Empty);
                    break;
                }
            }

            return dct;
        }

        public List<Dictionary<String, String>> GetNodeChildren(String sesscacheid, Int32 prntid)
        {
            List<Dictionary<String, String>> lst = new List<Dictionary<string, string>>();

            Int32 csid = Int32.Parse(this.CaseRow["id"].ToString());

            String sql = String.Format("select * from v_case_incident_groups where parent_id = {0} and case_id = {1}", prntid, csid);
            if (!this.IsDbRecord()) sql = String.Format("select *, group_name as case_group_name from t_cache_case_incident_groups where parent_id = {0} and cache_case_no = '{1}'", prntid, sesscacheid);

            using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["incAdoNet"].ConnectionString))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand(sql, cnn);
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Dictionary<String, String> dct = new Dictionary<string, string>();

                    for (Int16 i = 0; i < rdr.FieldCount; i++)
                        dct.Add(rdr.GetName(i), rdr[i] != null ? rdr[i].ToString() : String.Empty);

                    lst.Add(dct);
                }
            }

            return lst;
        }

        public List<Dictionary<String, String>> GetCaseNodeType(String sesschid, Byte ndtyp)
        {
            List<Dictionary<String, String>> lst = new List<Dictionary<string, string>>();
            Int32 csid = Int32.Parse(this.CaseRow["id"].ToString());
            String sql = String.Format("select * from v_case_incident_groups where case_id = {0} and hierarchy_level = {1}", csid, ndtyp);
            if (!this.IsDbRecord()) sql = String.Format("select *, group_name as case_group_name from t_cache_case_incident_groups where cache_case_no = '{0}' and population_group_id = {1}", sesschid, ndtyp);

            using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["incAdoNet"].ConnectionString))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand(sql, cnn);
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Dictionary<String, String> dct = new Dictionary<string, string>();

                    for (Int16 i = 0; i < rdr.FieldCount; i++) dct.Add(rdr.GetName(i), rdr[i] != null ? rdr[i].ToString() : String.Empty);
                    lst.Add(dct);
                }
            }

            return lst;
        }

        public List<Dictionary<String, String>> GetBranchNodeType(String sesschid, Int32 prntid, Byte lvl)
        {
            List<Dictionary<String, String>> lst = new List<Dictionary<string, string>>();
            Int32 csid = Int32.Parse(this.CaseRow["id"].ToString());
            String sql = String.Format("select * from v_case_incident_groups where case_id = {0} and parent_id = {1} and hierarchy_level = {2}", csid, prntid, lvl);
            if (!this.IsDbRecord()) sql = String.Format("select *, group_name as case_group_name from t_cache_case_incident_groups where cache_case_no = '{0}' and parent_id = {1} and population_group_id = {2}", sesschid, prntid, lvl);

            using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["incAdoNet"].ConnectionString))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand(sql, cnn);
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Dictionary<String, String> dct = new Dictionary<string, string>();

                    for (Int16 i = 0; i < rdr.FieldCount; i++) dct.Add(rdr.GetName(i), rdr[i] != null ? rdr[i].ToString() : String.Empty);
                    lst.Add(dct);
                }
            }

            return lst;
        }     

        public void DeletePeopleGroup(Int32 i, ref String errmsg)
        {
            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
                try
                {
                    if (this.IsDbRecord())
                        this.DeleteCaseCascade(db, i);
                    else
                        this.DeleteCacheCascade(db, i);
                }
                catch (Exception xcp)
                {
                    errmsg = xcp.Message;
                }
        }

        private void DeleteCaseCascade(IncDataClassesDataContext db, Int32 cigid)
        {
            CaseIncidentGroup cig = db.CaseIncidentGroups.FirstOrDefault(obj => obj.parent_id == cigid);
            if (cig != null)
            {
                this.DeleteCaseCascade(db, cig.id);
                String s = String.Format("delete from t_case_incident_groups where id = {0}", cigid);
                Int32 i = db.ExecuteCommand(s);
            }
            else
            {
                String s = String.Format("delete from t_case_incident_groups where id = {0}", cigid);
                Int32 i = db.ExecuteCommand(s);
            }            
        }

        private void DeleteCacheCascade(IncDataClassesDataContext db, Int32 cigid)
        {
            CacheCaseIncidentGroup cig = db.CacheCaseIncidentGroups.FirstOrDefault(obj => obj.parent_id == cigid);
            if (cig != null)
            {
                this.DeleteCacheCascade(db, cig.id);
                String s = String.Format("delete from t_cache_case_incident_groups where id = {0}", cigid);
                Int32 i = db.ExecuteCommand(s);
            }
            else
            {
                String s = String.Format("delete from t_cache_case_incident_groups where id = {0}", cigid);
                Int32 i = db.ExecuteCommand(s);
            }            
        }

        public void WritePersonGroup(String sesschid, String usrnm, String grpnm, Int32 prntid, Byte hlvl, ref Int32 grpid, ref Boolean ok, ref String msg)
        {
            Int32? xid = grpid;
            Int32 csrwid = 0;        

            Int32.TryParse(this.CaseRow["id"].ToString(), out csrwid);

            msg = String.Empty;
            ok = true;

            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
                if (!this.IsDbRecord())                
                    try
                    {
                        Int32 i = db.p_write_cache_group(ref xid, csrwid, sesschid, hlvl, prntid, grpnm, usrnm);
                        grpid = xid.Value;
                        this.CheckCacheHierarchy(db, grpid, sesschid, hlvl, usrnm);
                    }
                    catch (Exception xcp)
                    {
                        msg = xcp.Message;
                        ok = false;
                    }                
                else                
                    try
                    {
                        Int32 rslt = db.p_write_case_incident_group(ref xid, csrwid, hlvl, prntid, grpnm, usrnm);
                        grpid = xid.Value;
                        this.CheckCaseHierarchy(db, grpid, csrwid, hlvl, usrnm);
                    }
                    catch (Exception xcp)
                    {
                        msg = xcp.Message;
                        ok = false;
                    }                
        }

        public void CollectCommunityHousehold(Int32 cigid, ref Int32 cmmntyid, ref Int32 hsholdid)
        {
            String hlvl = IncUtilxs.GetDictionaryKeyVal("household_id");
            String clvl = IncUtilxs.GetDictionaryKeyVal("community_id");
            Int32 aid = cigid;

            cmmntyid = 0; hsholdid = 0;

            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
                if (this.IsDbRecord())
                    for (Byte a = 3; a > 0; a--)
                    {
                        CaseIncidentGroup xig = db.CaseIncidentGroups.FirstOrDefault(obj => obj.population_group_id == a && obj.id == aid && obj.case_id == Int32.Parse(this.CaseRow["id"].ToString()));
                        if (xig != null)
                        {
                            if (xig.population_group_id == Int16.Parse(hlvl))
                                hsholdid = xig.id;
                            else
                                if (xig.population_group_id == Int16.Parse(clvl))
                                    cmmntyid = xig.id;

                            aid = xig.parent_id.Value;
                        }
                    }
                else
                    for (Byte a = 3; a > 0; a--)
                    {
                        CacheCaseIncidentGroup xig = db.CacheCaseIncidentGroups.FirstOrDefault(obj => obj.population_group_id == a && obj.id == aid);
                        if (xig != null)
                        {
                            if (xig.population_group_id == Int16.Parse(hlvl))
                                hsholdid = xig.id;
                            else
                                if (xig.population_group_id == Int16.Parse(clvl))
                                    cmmntyid = xig.id;

                            aid = xig.parent_id.Value;
                        }
                    }
        }

        public void WriteIndividual(Dictionary<String, String> dct, String usrnm, ref String emsg)
        {
            Int32? xid = Int32.Parse(dct["id"].ToString());
            Int32? cigid = 0;
            Int32 i = 0;

            emsg = String.Empty;

            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
                try
                {
                    if (this.IsDbRecord())
                    {
                        if (xid.Value == 0)
                            i = db.p_write_case_incident_group(ref cigid, Int32.Parse(dct["case_id"].ToString()), Int16.Parse(dct["hierarchy_level"]), Int32.Parse(dct["parent_id"]), String.Empty, usrnm);
                        else
                            cigid = Int32.Parse(dct["case_incident_group_id"]);
                        

                        i = db.p_write_individual(ref xid, Int32.Parse(dct["case_id"]), Int32.Parse(dct["parent_id"]), ref cigid, dct["unit_role"], Byte.Parse(dct["individual_class"]), dct["lname"], dct["mdname"], dct["fnames"], dct["gender"], dct["national_id"], dct["dob"], dct["country_of_birth"], dct["place_of_birth"], dct["nationality"], Byte.Parse(dct["age"]), dct["marital_status"], dct["level_of_education"], dct["occupation"], dct["employer"], dct["remarks"], dct["contact_phone"], dct["email"], dct["physical_address"], Int16.Parse(dct["displacement_status_id"]), Int16.Parse(dct["vulnerability_id"]), Boolean.Parse(dct["is_breadwinner"]), usrnm);
                        this.CheckCaseHierarchy(db, cigid.Value, Int32.Parse(dct["case_id"].ToString()), Byte.Parse(dct["hierarchy_level"]), usrnm);
                    }
                    else
                    {
                        if (xid.Value == 0)                        
                            i = db.p_write_cache_group(ref cigid, Int32.Parse(dct["case_id"]), dct["session_cache_id"], Int16.Parse(dct["hierarchy_level"]), Int32.Parse(dct["parent_id"]), String.Empty, usrnm);
                        else
                            cigid = Int32.Parse(dct["case_incident_group_id"]);
                        
                        i = db.p_write_cache_individual(ref xid, cigid.Value, dct["unit_role"], Byte.Parse(dct["individual_class"]), dct["lname"], dct["mdname"], dct["fnames"], dct["gender"], dct["national_id"], dct["dob"], dct["country_of_birth"], dct["place_of_birth"], dct["nationality"], Byte.Parse(dct["age"]), dct["marital_status"], dct["level_of_education"], dct["occupation"], dct["employer"], dct["remarks"], dct["contact_phone"], dct["email"], dct["physical_address"], Int16.Parse(dct["displacement_status_id"]), Int16.Parse(dct["vulnerability_id"]), Boolean.Parse(dct["is_breadwinner"]), usrnm);
                        this.CheckCacheHierarchy(db, cigid.Value, dct["session_cache_id"], Byte.Parse(dct["hierarchy_level"]), usrnm);
                    }

                    dct["id"] = xid.Value.ToString();
                    dct["case_incident_group_id"] = cigid.Value.ToString();
                }
                catch (Exception xcp)
                {
                    emsg = String.Format("!!! Error : {0}", xcp.Message);
                }            
        }

        private void CheckCacheHierarchy(IncDataClassesDataContext db, Int32 cigid, String cacheid, Byte lvl, String usr)
        {
            Int32? i = 0;
            Int32 x = 0;
            Byte h = --lvl;

            if (!(lvl > 0)) return;

            var xbj = from obj in db.CacheCaseIncidentGroups
                      join pobj in db.CacheCaseIncidentGroups on obj.parent_id equals pobj.id
                      where (obj.id == cigid && obj.cache_case_no.Equals(cacheid))
                      select obj;

            if (xbj.Count() == 0)
            {
                CacheCaseIncidentGroup ucig = db.CacheCaseIncidentGroups.FirstOrDefault(obj => obj.cache_case_no.Equals(cacheid) && obj.population_group_id == h && obj.group_name.Equals("UnKnown"));
                if (ucig == null)
                {
                    x = db.p_write_cache_group(ref i, 0, cacheid, h, 0, "UnKnown", usr);
                    String q = String.Format("update t_cache_case_incident_groups set parent_id = {0} where id = {1}", i.Value.ToString(), cigid.ToString());
                    x = db.ExecuteCommand(q);
                    this.CheckCacheHierarchy(db, i.Value, cacheid, h, usr);                    
                }
                else
                {
                    String q = String.Format("update t_cache_case_incident_groups set parent_id = {0} where id = {1}", ucig.id.ToString(), cigid.ToString());
                    x = db.ExecuteCommand(q);
                }
            }
        }

        private void CheckCaseHierarchy(IncDataClassesDataContext db, Int32 cigid, Int32 csid, Byte lvl, String usr)
        {
            Int32? i = 0;
            Int32 x = 0;
            Byte h = --lvl;

            if (!(lvl > 0)) return;

            var xbj = from obj in db.CaseIncidentGroups
                      join pobj in db.CaseIncidentGroups on obj.parent_id equals pobj.id
                      where (obj.id == cigid && obj.case_id == csid)
                      select obj;

            if (xbj.Count() == 0)
            {
                VwCaseIncidentGroup ucig = db.VwCaseIncidentGroups.FirstOrDefault(obj => obj.case_id == csid && obj.hierarchy_level == h && obj.case_group_name.Equals("UnKnown"));
                if (ucig == null)
                {
                    x = db.p_write_case_incident_group(ref i, csid, h, 0, "UnKnown", usr);
                    String q = String.Format("update t_case_incident_groups set parent_id = {0} where id = {1}", i.Value.ToString(), cigid.ToString());
                    x = db.ExecuteCommand(q);
                    this.CheckCaseHierarchy(db, i.Value, csid, h, usr);
                }
                else
                {
                    String q = String.Format("update t_cache_case_incident_groups set parent_id = {0} where id = {1}", ucig.id.ToString(), cigid.ToString());
                    x = db.ExecuteCommand(q);
                }
            }
        }

        private void WriteCigItem(IncDataClassesDataContext db, Dictionary<String, String> dct, ref List<Dictionary<String, Int32>> cignos)
        {
            Int32? cigid = 0;
            Int32 i = 0;

            CacheCaseIncidentGroup cigobj = db.CacheCaseIncidentGroups.FirstOrDefault(obj => obj.id == Int32.Parse(dct["parent_id"]) && obj.cache_case_no.Equals(dct["cache_case_no"]));
            if (cigobj != null)
            {
                Dictionary<String, Int32> xitm = cignos.FirstOrDefault(obj => obj["old"] == Int32.Parse(dct["parent_id"]));

                if (xitm == null)
                {
                    Dictionary<String, String> ndct = new Dictionary<string, string>();

                    ndct.Add("cache_case_no", cigobj.cache_case_no);
                    ndct.Add("case_id", dct["case_id"]);
                    ndct.Add("group_name", cigobj.group_name);
                    ndct.Add("id", cigobj.id.ToString());
                    ndct.Add("parent_id", cigobj.parent_id.ToString());
                    ndct.Add("population_group_id", cigobj.population_group_id.ToString());
                    ndct.Add("updated_by", cigobj.updated_by);
                    ndct.Add("updated_on", cigobj.updated_on.Value.ToString());
                    ndct.Add("user_name", dct["user_name"]);

                    this.WriteCigItem(db, ndct, ref cignos);
                }
                else
                {
                    i = db.p_write_case_incident_group(ref cigid, Int32.Parse(dct["case_id"]), Int16.Parse(dct["population_group_id"]), xitm["new"], dct["group_name"], dct["user_name"]);
                    cignos.Add(new Dictionary<string, int>() { { "old", Int32.Parse(dct["id"]) }, { "new", cigid.Value }, { "parent_id", xitm["new"] } });
                }
            }
            else
            {
                Dictionary<String, Int32> xitm = cignos.FirstOrDefault(obj => obj["old"] == Int32.Parse(dct["id"]));

                if (xitm == null)
                {
                    i = db.p_write_case_incident_group(ref cigid, Int32.Parse(dct["case_id"]), Int16.Parse(dct["population_group_id"]), Int32.Parse(dct["parent_id"]), dct["group_name"], dct["user_name"]);
                    cignos.Add(new Dictionary<string, int>() { { "old", Int32.Parse(dct["id"]) }, { "new", cigid.Value }, { "parent_id", Int32.Parse(dct["parent_id"]) } });
                }
            }            
        }

        public void WriteCacheAffectedGroups(String sesscachid, String usrnm, ref String errmsg)
        {
            List<Dictionary<String, Int32>> lst = new List<Dictionary<string, int>>();

            Int32 csrwid = Int32.Parse(this.CaseRow["id"].ToString());
            Int32 i = 0;
            
            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
                try
                {
                    foreach (CacheCaseIncidentGroup cig in db.CacheCaseIncidentGroups.Where(obj => obj.cache_case_no.Equals(sesscachid)).OrderBy(obj => obj.population_group_id).ToList())
                    {
                        Dictionary<String, String> dct = new Dictionary<string, string>();

                        dct.Add("cache_case_no", cig.cache_case_no);
                        dct.Add("case_id", csrwid.ToString());
                        dct.Add("group_name", cig.group_name);
                        dct.Add("id", cig.id.ToString());
                        dct.Add("parent_id", cig.parent_id.ToString());
                        dct.Add("population_group_id", cig.population_group_id.ToString());
                        dct.Add("updated_by", cig.updated_by);
                        dct.Add("updated_on", cig.updated_on.Value.ToString());
                        dct.Add("user_name", usrnm);

                        this.WriteCigItem(db, dct, ref lst);
                    }
                    
                    foreach (VwCacheIndividual indv in db.VwCacheIndividuals.Where(obj => obj.cache_case_no.Equals(sesscachid)).ToList())
                    {
                        Dictionary<String, Int32> prnt = lst.FirstOrDefault(obj => obj["old"] == indv.parent_id);
                        Dictionary<String, Int32> chld = lst.FirstOrDefault(obj => obj["old"] == indv.case_incident_group_id);

                        if (prnt == null || chld == null) continue;
                                                
                        Int32? xid = 0, cigid = chld["new"];
                        Int32 prntid = prnt["new"];

                        i = db.p_write_individual(ref xid, csrwid, prntid, ref cigid, indv.unit_role, indv.individual_class, indv.lname, indv.mdname, indv.fnames, indv.gender, indv.national_id, indv.dob.Value.ToString("dd/MM/yyyy"), indv.country_of_birth, indv.place_of_birth, indv.nationality, indv.age, indv.marital_status, indv.level_of_education, indv.occupation, indv.employer, indv.remarks, indv.contact_phone, indv.email, indv.physical_address, indv.displacement_status_id, indv.vulnerability_id, indv.is_breadwinner == 1, @usrnm);
                    }
                }
                catch (Exception xcp)
                {
                    errmsg = xcp.Message;
                }               
        }
    }
}