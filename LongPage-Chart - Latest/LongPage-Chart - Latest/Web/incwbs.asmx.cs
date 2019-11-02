using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;
using inc;

namespace iom
{
    /// <summary>
    /// Summary description for incwbs
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class incwbs : System.Web.Services.WebService
    {
        [WebMethod]
        public String MapLatitudeLongitude(String yr, String mn, String inc_typ, String pep_type, String caus_typ, String pg)
        {
            System.Web.Script.Serialization.JavaScriptSerializer json = new System.Web.Script.Serialization.JavaScriptSerializer();            
            List<String> whr = new List<string>();
            Hashtable itms = new Hashtable();
            String sjson = String.Empty;

            if (!String.IsNullOrEmpty(yr))
                whr.Add(String.Format("year(cs.date_reported) = {0}", yr));
            else
                whr.Add("year(cs.date_reported) = 0");

            if (!String.IsNullOrEmpty(mn))
                if (!mn.Equals("0"))
                    whr.Add(String.Format("month(cs.date_reported) = {0}", mn));

            if (!String.IsNullOrEmpty(inc_typ))
                if (!inc_typ.Equals("0"))
                    whr.Add(String.Format("cs.incident_type_id = {0}", inc_typ));

            if (!String.IsNullOrEmpty(pep_type))           
                if (!pep_type.Equals("0"))
                    whr.Add(String.Format("pp.type_perpetrator_id = {0}", pep_type));

            if (!String.IsNullOrEmpty(caus_typ))
                if (!caus_typ.Equals("0"))
                    whr.Add(String.Format("ca.type_cause_id = {0}", caus_typ));


            itms["pages"] = 1;
            itms["page"] = pg;
            IncUtilxs.GetLatLongItems(whr, ref itms);

            sjson = json.Serialize((Object)itms);
            return sjson;
        }

        [WebMethod]
        public String InlineSave(String csid, String org, String prv, String dstrct, String wd, String plcnm, String dtrptd, String inctyp, String othrinctyp, String dsplcsts, String othrdsplcsts, String iaffctd, String haffctd, String caffctd, String gpse, String gpss, String peps, String nmpep, String othrpep, String caus, String othrcaus, String usr)
        {
            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            Dictionary<String, String> dct = new Dictionary<string, string>(), res = new Dictionary<string,string>();
            String jsn = String.Empty;

            if (!IncUtilxs.IsUserAllowed(usr, Actions.CanWrite))
            {
                res.Add("message", "!!! Error : You don't have permission to carry out requested action");

                return js.Serialize((Object)res);
            }

            dct["id"] = csid;
            dct["org_unit_id"] = org;
            dct["province_id"] = prv;
            dct["district_id"] = dstrct;
            dct["ward_no"] = wd;
            dct["place_name"] = plcnm;
            dct["date_reported"] = dtrptd;
            dct["incident_type_id"] = inctyp;
            dct["other_incident_type"] = othrinctyp;
            dct["displacement_status_id"] = dsplcsts;
            dct["other_displacement_status"] = othrdsplcsts;
            dct["num_individuals"] = iaffctd;
            dct["num_households"] = haffctd;
            dct["num_communities"] = caffctd;
            dct["gps_east"] = gpse;
            dct["gps_south"] = gpss;

            dct["peps"] = peps;
            dct["nmpep"] = nmpep;
            dct["caus"] = caus;
            dct["other_pep"] = othrpep;
            dct["other_caus"] = othrcaus;

            dct["user"] = usr;

            AjaxCase cs = new AjaxCase(dct);
            String msg = cs.WriteAjaxCase();

            res.Add("message", msg);

            return js.Serialize((Object)res);
        }

        [WebMethod]
        public String GetDistricts(String prv)
        {
            return AjaxUtils.GetDistricts(prv);
        }

        [WebMethod]
        public String GetDistrictWards(String prv, String dstrct)
        {
            return AjaxUtils.GetDistrictsAndWards(prv, dstrct);
        }

        [WebMethod]
        public String GetWards(String prv, String dstrct)
        {
            return AjaxUtils.GetWards(prv, dstrct);
        }

        [WebMethod]
        public String GetDonorProjects(String dnrid)
        {
            return AjaxUtils.GetProjects(dnrid);
        }

        [WebMethod]
        public String GetQListOption(String sid, String qid, String optnm, String usr)
        {
            return AjaxUtils.HandleQOptions(sid, qid, optnm, usr);
        }

        [WebMethod]
        public String DeleteQOption(String sid, String qid)
        {
            return AjaxUtils.RemoveQOption(sid, qid);
        }

        [WebMethod]
        public String GetQColumn(String sid, String qid, String dttyp, String colname, String usr)
        {
            return AjaxUtils.HandleQColumn(sid, qid, dttyp, colname, usr);
        }

        [WebMethod]
        public String GetQRow(String sid, String qid, String isothr, String rowname, String usr)
        {
            return AjaxUtils.HandleQRow(sid, qid, isothr, rowname, usr);
        }

        [WebMethod]
        public String DeleteQColumn(String sid, String qid)
        {
            return AjaxUtils.RemoveQColumn(sid, qid);
        }

        [WebMethod]
        public String DeleteQRow(String sid, String qid)
        {
            return AjaxUtils.RemoveQRow(sid, qid);
        }

        [WebMethod]
        public String PageSearchMember(String spg)
        {
            return AjaxUtils.GotoSearchMemberPage(spg);
        }

        [WebMethod]
        public String GetCaseRecord(String csno)
        {
            return AjaxUtils.GetCaseRow(csno);
        }

        [WebMethod]
        public String GetCopyOptions(String qid, String nid, String usr)
        {
            return AjaxUtils.MakeCopyOptions(qid, nid, usr);
        }

        [WebMethod]
        public String GetCopyColumns(String qid, String nid, String dttyp, String usr)
        {
            return AjaxUtils.MakeCopyColumns(qid, nid, dttyp, usr);
        }

        [WebMethod]
        public String GetCopyRows(String qid, String nid, String usr)
        {
            return AjaxUtils.MakeCopyRows(qid, nid, usr);
        }

        [WebMethod]
        public String GetCaseSummaries(String yf, String yt, String mf, String mt, String inc, String caus, String pep, String styp)
        {
            return AjaxUtils.GetCaseSummaryMapData(yf, yt, mf, mt, inc, caus, pep, styp);
        }

        [WebMethod]
        public String ProvincialPoints()
        {
            return AjaxUtils.GetOPMProvinces();
        }

        [WebMethod]
        public String DistrictPoints()
        {
            return AjaxUtils.GetOPMDistricts();
        }

        [WebMethod]
        public String AreaCases(String prv, String dst)
        {
            return AjaxUtils.GetAreaCases(prv, dst);
        }

        [WebMethod]
        public String GetCaseRow(String csid)
        {
            return AjaxUtils.GetCaseAttributes(csid);
        }
    }
}
