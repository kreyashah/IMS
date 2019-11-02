using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;
using inc;

namespace iom
{
    public class AjaxCase
    {
        private Dictionary<String, String> ajxitms = null;
        private Dictionary<String, String> dttypes = null;

        private AjaxCase()
        {
            this.InitDictionary(String.Empty);
        }

        public AjaxCase(Dictionary<String, String> input)
        {
            this.InitDictionary(input["id"]);

            this.ajxitms["id"] = input["id"];
            this.ajxitms["org_unit_id"] = input["org_unit_id"];
            this.ajxitms["province_id"] = input["province_id"];
            this.ajxitms["district_id"] = input["district_id"];
            this.ajxitms["ward_no"] = input["ward_no"];
            this.ajxitms["place_name"] = input["place_name"];
            this.ajxitms["date_reported"] = input["date_reported"];
            this.ajxitms["incident_type_id"] = input["incident_type_id"];
            this.ajxitms["other_incident_type"] = input["other_incident_type"];
            this.ajxitms["displacement_status_id"] = input["displacement_status_id"];
            this.ajxitms["other_displacement_status"] = input["other_displacement_status"];
            this.ajxitms["num_individuals"] = input["num_individuals"];
            this.ajxitms["num_households"] = input["num_households"];
            this.ajxitms["num_communities"] = input["num_communities"];
            this.ajxitms["gps_east"] = input["gps_east"];
            this.ajxitms["gps_south"] = input["gps_south"];

            this.ajxitms["peps"] = input["peps"];
            this.ajxitms["nmpep"] = input["nmpep"];
            this.ajxitms["caus"] = input["caus"];
            this.ajxitms["other_pep"] = input["other_pep"];
            this.ajxitms["other_caus"] = input["other_caus"];

            this.ajxitms["user"] = input["user"];
        }

        public String WriteAjaxCase()
        {
            String msg = "Saved case record successfully";
            String othr = IncUtilxs.GetDictionaryKeyVal("other_option_id");

            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
            {
                Regex rgxp = new Regex("^\\d+$");
                int? cid = Int32.Parse(this.ajxitms["id"]);
                String csno = this.ajxitms["case_no"];
                Int32 rslt = 0;

                try
                {                                       
                    rslt = db.p_write_case(ref cid, Int16.Parse(this.ajxitms["province_id"].ToString()), Int16.Parse(this.ajxitms["district_id"].ToString()), this.ajxitms["country_code"].ToString(), Int16.Parse(this.ajxitms["org_unit_id"].ToString()),
                        Int16.Parse(this.ajxitms["num_individuals"].ToString()), Int32.Parse(this.ajxitms["ward_no"].ToString()), Int16.Parse(this.ajxitms["info_source_id"].ToString()), Byte.Parse(this.ajxitms["info_verify_scale"].ToString()), this.ajxitms["date_reported"].ToString(),
                        this.ajxitms["incident_date"].ToString(), this.ajxitms["ref_no"].ToString(), this.ajxitms["place_name"].ToString(), Int16.Parse(this.ajxitms["emergency_level_id"].ToString()), this.ajxitms["gps_east"].ToString(), this.ajxitms["gps_south"].ToString(),
                        Boolean.Parse(this.ajxitms["is_verified"].ToString()), this.ajxitms["verified_on"].ToString(), this.ajxitms["comments"].ToString(), this.ajxitms["user"], this.ajxitms["other_info_source"].ToString(), this.ajxitms["other_emergency_level"].ToString(), ref csno,
                        Int32.Parse(this.ajxitms["num_households"].ToString()), Int32.Parse(this.ajxitms["num_communities"].ToString()), this.ajxitms["prev_case_no"].ToString(), Int16.Parse(this.ajxitms["prev_province_id"].ToString()), Int16.Parse(this.ajxitms["prev_district_id"].ToString()), this.ajxitms["prev_country"].ToString(), Int32.Parse(this.ajxitms["prev_ward_no"].ToString()),
                        this.ajxitms["prev_place_name"].ToString(), this.ajxitms["prev_gps_east"].ToString(), this.ajxitms["prev_gps_south"].ToString(), Boolean.Parse(this.ajxitms["cultivation_land"].ToString()), Boolean.Parse(this.ajxitms["current_place_legal"].ToString()), Int16.Parse(this.ajxitms["vulnerability_id"].ToString()),
                        Int16.Parse(this.ajxitms["displacement_status_id"].ToString()), Int16.Parse(this.ajxitms["incident_type_id"].ToString()), Int16.Parse(this.ajxitms["severity_level"].ToString()), this.ajxitms["other_incident_type"].ToString(), this.ajxitms["other_severity_level"].ToString(), this.ajxitms["other_displacement_status"].ToString(), Byte.Parse(this.ajxitms["hal"].ToString()),
                        Boolean.Parse(this.ajxitms["closed"].ToString()), this.ajxitms["closed_comments"].ToString());

                    Boolean rmvd;

                    if (!String.IsNullOrEmpty(this.ajxitms["peps"]) || !String.IsNullOrEmpty(this.ajxitms["nmpep"]))
                    {                        
                        Dictionary<Int16, Int32> pitms = new Dictionary<short,int>();

                        List<String> nmpep = new List<string>();
                        List<String> peps = new List<string>();

                        String[] drry = this.ajxitms["nmpep"].Split(',');
                        for (Int16 i = 0; i < drry.Length; i++)
                            if (!String.IsNullOrEmpty(drry[i])) nmpep.Add(drry[i]);

                        drry = this.ajxitms["peps"].Split(',');
                        for (Int16 i = 0; i < drry.Length; i++)
                            if (rgxp.IsMatch(drry[i])) peps.Add(drry[i]);
                        
                        foreach (String snm in nmpep)
                        {
                            String[] sarry = snm.Split('.');
                            if (sarry.Length > 1)
                                if (rgxp.IsMatch(sarry[0]) && rgxp.IsMatch(sarry[1])) pitms.Add(Int16.Parse(sarry[0]), Int32.Parse(sarry[1]));
                        }
                        
                        foreach (String sp in peps)
                            if (!pitms.Keys.Contains(Int16.Parse(sp))) pitms.Add(Int16.Parse(sp), 0);

                        if (!String.IsNullOrEmpty(this.ajxitms["other_pep"]))                        
                            if (!pitms.Keys.Contains(Int16.Parse(othr))) pitms.Add(Int16.Parse(othr), 0);                        

                        if (pitms.Count != 0)
                        {
                            List<PrimaryPerpetrator> peplst = db.PrimaryPerpetrators.Where(obj => obj.case_id == cid.Value).ToList();
                            if (peplst != null)
                            {
                                foreach (PrimaryPerpetrator pp in peplst)
                                    if (!pitms.Keys.Contains(pp.type_perpetrator_id) || pp.type_perpetrator_id.ToString().Equals(othr))
                                        rslt = db.ExecuteCommand("delete from t_primary_perpetrators where id = {0}", pp.id.ToString());

                                foreach (KeyValuePair<Int16, Int32> kvl in pitms)
                                    rslt = db.p_write_primary_perpetrator(0, cid.Value, kvl.Key, this.ajxitms["user"], kvl.Value, this.ajxitms["other_pep"]);
                            }
                        }
                    }
                    else
                        rslt = db.ExecuteCommand("delete from t_primary_perpetrators where case_id = {0}", cid.Value.ToString());

                    if (!String.IsNullOrEmpty(this.ajxitms["caus"]))
                    {
                        List<String> citms = new List<string>();

                        String[] drry = this.ajxitms["caus"].Split(',');
                        for (Int16 i = 0; i < drry.Length; i++)
                            if (rgxp.IsMatch(drry[i])) citms.Add(drry[i]);

                        if (!String.IsNullOrEmpty(this.ajxitms["other_caus"]))
                        {
                            if (!citms.Contains(othr)) citms.Add(othr);
                        }
                        else rmvd = citms.Remove(othr);

                        if (citms.Count != 0)
                        {
                            List<PrimaryCause> caus = db.PrimaryCauses.Where(obj => obj.case_id == cid.Value).ToList();
                            if (caus != null)
                            {
                                foreach (PrimaryCause ca in caus)
                                    if (!citms.Contains(ca.type_cause_id.ToString()) || ca.type_cause_id.ToString().Equals(othr))
                                        rslt = db.ExecuteCommand("delete from t_primary_causes where id = {0}", ca.id.ToString());

                                foreach (String s in citms)
                                    rslt = db.p_write_primary_cause(0, cid.Value, Int16.Parse(s), this.ajxitms["user"], this.ajxitms["other_caus"]);
                            }
                        }
                    }
                    else
                        rslt = db.ExecuteCommand("delete from t_primary_causes where case_id = {0}", cid.Value.ToString());
                }
                catch (Exception xcp)
                {
                    msg = xcp.Message;
                }
            }

            return msg;
        }

        private void InitDictionary(String sid)
        {
            this.ajxitms = new Dictionary<string, string>();
            this.dttypes = new Dictionary<string,string>();

            String sql = String.Format("select c.COLUMN_NAME, c.DATA_TYPE from INFORMATION_SCHEMA.COLUMNS c where c.TABLE_NAME = '{0}' order by 1", "v_case_row");

            using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["IncAdoNet"].ConnectionString))
            {
                cnn.Open();

                SqlCommand cmd = new SqlCommand(sql, cnn);
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    this.ajxitms.Add(rdr[0].ToString(), String.Empty);
                    this.dttypes.Add(rdr[0].ToString(), rdr[1].ToString());
                }
                rdr.Close();

                if (String.IsNullOrEmpty(sid)) return;
                
                Regex rgxp = new Regex("^\\d+$");
                if (!rgxp.IsMatch(sid)) return;                

                sql = "select * from v_case_row where id = {0}";
                sql = String.Format(sql, sid.Trim());

                cmd = new SqlCommand(sql, cnn);
                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                    for (Int16 i = 0; i < rdr.FieldCount; i++)
                    {
                        String fld = rdr.GetName(i);
                        String vl = String.Empty;

                        if (rdr.GetValue(i) == null) continue;
                        if (String.IsNullOrEmpty(rdr.GetValue(i).ToString())) continue;

                        switch (this.dttypes[fld])
                        {
                            case "bit":
                                vl = rdr.GetSqlBoolean(i).ToString();
                                break;
                            case "date":
                            case "datetime":
                                vl = DateTime.Parse(rdr.GetSqlValue(i).ToString(), System.Globalization.CultureInfo.CreateSpecificCulture("en-GB")).ToString("dd/MM/yyyy");
                                break;
                            default:
                                vl = rdr.GetValue(i).ToString();
                                break;
                        }

                        this.ajxitms[fld] = vl.Trim();
                    }                
            }
        }
    }

    public static class AjaxUtils
    {
        public static String GetDistricts(String prv)
        {
            List<Dictionary<String, String>> lst = new List<Dictionary<string, string>>();
            JavaScriptSerializer json = new JavaScriptSerializer();
            Regex rgxp = new Regex("^\\d+$");

            if (!rgxp.IsMatch(prv)) prv = "0";

            lst.Add(new Dictionary<string, string>() { { "district_id", "0"}, { "district", "UnSpecified" } });

            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
            {
                List<VwDistrict> rws = db.VwDistricts.Where(obj => obj.province_id == Int16.Parse(prv)).ToList();
                if (rws != null)
                    foreach (VwDistrict d in rws) lst.Add(new Dictionary<string, string>() { { "district_id", d.id.ToString() }, { "district", d.district } });
            }

            return json.Serialize((Object)lst);
        }

        public static String GetWards(String prv, String dstrct)
        {
            List<Dictionary<String, String>> lst = new List<Dictionary<string, string>>();
            JavaScriptSerializer json = new JavaScriptSerializer();
            Regex rgxp = new Regex("^\\d+$");

            if (!rgxp.IsMatch(prv)) prv = "0";
            if (!rgxp.IsMatch(dstrct)) dstrct = "0";

            lst.Add(new Dictionary<String, String>() { { "ward_id", "0" }, { "ward", "UnSpecified" } });

            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
            {
                List<Ward> rws = db.Wards.Where(obj => obj.province_pcode == Int16.Parse(prv) && obj.district_pcode == Int16.Parse(dstrct)).Distinct().ToList();
                if (rws != null)
                    foreach (Ward w in rws) lst.Add(new Dictionary<string, string>() { { "ward_id", w.ward_no.ToString() }, { "ward", w.ward_no.ToString() } });
            }

            return json.Serialize((Object)lst);
        }

        public static String GetDistrictsAndWards(String prv, String dstrct)
        {
            List<Dictionary<String, String>> wds = new List<Dictionary<string, string>>();
            List<Dictionary<String, String>> dstrcts = new List<Dictionary<string, string>>();
            Dictionary<String, Object> dct = new Dictionary<string, object>();
            JavaScriptSerializer json = new JavaScriptSerializer();

            wds.Add(new Dictionary<string, string>() { { "ward_id", "0" }, { "ward", "UnSpecified" } });
            dstrcts.Add(new Dictionary<string, string>() { { "district_id", "0" }, { "district", "UnSpecified" } });

            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
            {
                foreach (VwDistrict ds in db.VwDistricts.Where(obj => obj.province_id == Int16.Parse(prv)).ToList())
                    dstrcts.Add(new Dictionary<string, string>() { { "district_id", ds.id.ToString() }, { "district", ds.district } });

                foreach (Ward wd in db.Wards.Where(obj => obj.province_pcode == Int16.Parse(prv) && obj.district_pcode == Int16.Parse(dstrct)).ToList())
                    wds.Add(new Dictionary<string, string>() { { "ward_id", wd.ward_no.ToString() }, { "ward", wd.ward_no.ToString() } });
            }

            dct.Add("districts", dstrcts);
            dct.Add("wards", wds);

            return json.Serialize((Object)dct);
        }

        public static String GetProjects(String dnrid)
        {
            Regex rgxp = new Regex("^\\d+$");

            List<Dictionary<String, String>> lst = new List<Dictionary<string, string>>();
            JavaScriptSerializer json = new JavaScriptSerializer();
            Int16 i = 0;

            lst.Add(new Dictionary<string, string>() { { "id", "0"}, { "project_code", "UnSpecified" } });

            if (!String.IsNullOrEmpty(dnrid))
                if (rgxp.IsMatch(dnrid)) Int16.TryParse(dnrid, out i);

            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
                foreach (VwProject prj in db.VwProjects.Where(obj => obj.donor_id == i).ToList())
                    lst.Add(new Dictionary<string, string>() { { "id", prj.id.ToString() }, { "project_code", prj.project_code } });

            return json.Serialize((Object)lst);
        }

        public static String GetOptionAnswerTypes(String anstyp)
        {
            List<Dictionary<String, String>> lst = new List<Dictionary<string, string>>();
            JavaScriptSerializer json = new JavaScriptSerializer();

            String optans = IncUtilxs.GetDictionaryKeyVal("option_list_answer");
            String optlst = IncUtilxs.GetDictionaryKeyVal("option_types");

            if (anstyp.Equals(optans)) lst = IncUtilxs.DictKeyValString(optlst, ';', ',');
            if (lst.Count == 0) lst.Add(new Dictionary<string, string>() { { "key", "0" }, { "val", "N/A" } });

            return json.Serialize((Object)lst);
        }

        public static String GetGriDataTypes(String anstyp)
        {
            List<Dictionary<String, String>> lst = new List<Dictionary<string, string>>();
            JavaScriptSerializer json = new JavaScriptSerializer();

            String grdans = IncUtilxs.GetDictionaryKeyVal("grid_answer");
            String grdtypes = IncUtilxs.GetDictionaryKeyVal("grid_data_types");

            if (anstyp.Equals(grdans)) lst = IncUtilxs.DictKeyValString(grdtypes, ';', ',');
            if (lst.Count == 0) lst.Add(new Dictionary<string, string>() { { "key", "0" }, { "val", "N/A" } });

            return json.Serialize((Object)lst);
        }

        public static String HandleQOptions(String sid, String qid, String onm, String usr)
        {
            Dictionary<String, Object> dct = new Dictionary<string, object>();
            List<Dictionary<String, String>> lst = new List<Dictionary<string, string>>();
            JavaScriptSerializer json = new JavaScriptSerializer();
            Regex rgxp = new Regex("^\\d+$");

            dct.Add("message", String.Empty);

            lst.Add(new Dictionary<string, string>() { { "id", "0" }, { "answer_option", "None" } });

            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
            {
                if (rgxp.IsMatch(sid) && rgxp.IsMatch(qid) && !String.IsNullOrEmpty(onm))
                {
                    Int32? rid = Int32.Parse(sid);
                    Int32 rslt = 0;

                    try
                    {
                        QListOption rw = db.QListOptions.FirstOrDefault(obj => obj.answer_option.ToUpper().Equals(onm.ToUpper().Trim()) && obj.id != rid.Value && obj.question_id == Int32.Parse(qid));
                        if (rw == null)
                            rslt = db.p_write_qlist_option(ref rid, Int32.Parse(qid), onm.Trim(), usr);
                        else
                            dct["message"] = String.Format("Answer option '{0}' already exists", onm.Trim());
                    }
                    catch (Exception xcp)
                    {
                        dct["message"] = xcp.Message.ToString();
                    }
                }

                List<QListOption> rws = db.QListOptions.Where(obj => obj.question_id == Int32.Parse(qid)).ToList();
                foreach (QListOption x in rws)
                    lst.Add(new Dictionary<string, string>() { { "id", x.id.ToString() }, { "answer_option", x.answer_option } });
            }

            dct.Add("data", lst);

            return json.Serialize((Object)dct);
        }

        public static String RemoveQOption(String sid, String qid)
        {
            Dictionary<String, Object> dct = new Dictionary<string, object>();
            List<Dictionary<String, String>> lst = new List<Dictionary<string, string>>();
            JavaScriptSerializer json = new JavaScriptSerializer();
            Regex rgxp = new Regex("^\\d+$");

            dct.Add("message", String.Empty);

            lst.Add(new Dictionary<string, string>() { { "id", "0" }, { "answer_option", "None" } });

            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
            {
                Int32 cnt = db.VwSurveyOptionAnswers.Count(obj => obj.question_id == Int32.Parse(qid) && obj.answer_id == Int32.Parse(sid));

                if (cnt != 0)
                    dct["message"] = "Answers have been supplied for this option. Can't carry out the action you requested";
                else
                    if (rgxp.IsMatch(sid) && rgxp.IsMatch(qid) && !sid.Equals("0"))
                    {
                        String s = String.Format("delete from t_qlist_options where id = {0}", sid);
                        try
                        {
                            Int32 rslt = db.ExecuteCommand(s);
                        }
                        catch (Exception xcp)
                        {
                            dct["message"] = xcp.Message.ToString();
                        }
                    }

                List<QListOption> rws = db.QListOptions.Where(obj => obj.question_id == Int32.Parse(qid)).ToList();
                foreach (QListOption c in rws)
                    lst.Add(new Dictionary<string, string>() { { "id", c.id.ToString() }, { "answer_option", c.answer_option } });
            }

            dct.Add("data", lst);

            return json.Serialize((Object)dct);
        }

        public static String HandleQColumn(String sid, String qid, String dttyp, String cnm, String usr)
        {
            Dictionary<String, Object> dct = new Dictionary<string, object>();
            List<Dictionary<String, String>> lst = new List<Dictionary<string, string>>();
            JavaScriptSerializer json = new JavaScriptSerializer();
            Regex rgxp = new Regex("^\\d+$");

            dct.Add("message", String.Empty);

            lst.Add(new Dictionary<string, string>() { { "id", "0" }, { "colname", "None" } });
            
            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
            {
                if (rgxp.IsMatch(sid) && rgxp.IsMatch(qid) && !String.IsNullOrEmpty(cnm))
                {
                    Int32? rid = Int32.Parse(sid);
                    Int32 rslt = 0, clen = 30;

                    try
                    {
                        IncDictionary ditm = db.IncDictionaries.FirstOrDefault(obj => obj.dict_key.Equals("grid_column_length"));
                        if (ditm != null)
                            if (rgxp.IsMatch(ditm.dict_value)) clen = Int32.Parse(ditm.dict_value);

                        cnm = cnm.Trim().Length > clen ? cnm.Trim().Substring(0, clen) : cnm.Trim();

                        QGridColumn rw = db.QGridColumns.FirstOrDefault(obj => obj.column_name.ToUpper().Equals(cnm.ToUpper().Trim()) && obj.question_id == Int32.Parse(qid) && obj.id != Int32.Parse(sid));
                        if (rw == null)
                            rslt = db.p_write_qgrid_column(ref rid, Int32.Parse(qid), Byte.Parse(dttyp), cnm.Trim(), usr);
                        else
                            dct["message"] = String.Format("Answer grid column {0} already exists", cnm.Trim());
                    }
                    catch (Exception xcp)
                    {
                        dct["message"] = xcp.Message.ToString();
                    }
                }

                List<QGridColumn> rws = db.QGridColumns.Where(obj => obj.question_id == Int32.Parse(qid)).ToList();
                foreach (QGridColumn q in rws)
                {
                    String coldtyp = IncUtilxs.ColumnDataType(q.column_data_type.Value);
                    String colnm = String.IsNullOrEmpty(coldtyp) ? q.column_name : String.Format("{0} ({1})", q.column_name, coldtyp);
                    lst.Add(new Dictionary<string, string>() { { "id", q.id.ToString() }, { "colname", colnm } });
                }
            }

            dct.Add("data", lst);

            return json.Serialize((Object)dct);
        }

        public static String HandleQRow(String sid, String qid, String isothr, String rnm, String usr)
        {
            Dictionary<String, Object> dct = new Dictionary<string, object>();
            List<Dictionary<String, String>> lst = new List<Dictionary<string, string>>();
            JavaScriptSerializer json = new JavaScriptSerializer();
            Regex rgxp = new Regex("^\\d+$");

            dct.Add("message", String.Empty);

            lst.Add(new Dictionary<string, string>() { { "id", "0" }, { "rowname", "None" } });

            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
            {
                if (rgxp.IsMatch(sid) && rgxp.IsMatch(qid) && !String.IsNullOrEmpty(rnm))
                {
                    Int32? rid = Int32.Parse(sid);
                    Int32 rslt = 0;                    

                    try
                    {
                        QGridRow rw = db.QGridRows.FirstOrDefault(obj => obj.row_name.ToUpper().Equals(rnm.ToUpper().Trim()) && obj.question_id == Int32.Parse(qid) && obj.id != Int32.Parse(sid));
                        if (rw == null)
                            rslt = db.p_write_qgrid_row(ref rid, Int32.Parse(qid), !isothr.Equals("0"), rnm.Trim(), usr);
                        else
                            dct["message"] = String.Format("Grid answer row {0} already exists", rnm.Trim());
                    }
                    catch (Exception xcp)
                    {
                        dct["message"] = xcp.Message.ToString();
                    }
                }

                List<QGridRow> rws = db.QGridRows.Where(obj => obj.question_id == Int32.Parse(qid)).ToList();
                foreach (QGridRow r in rws)
                    lst.Add(new Dictionary<string, string>() { { "id", r.id.ToString() }, { "rowname", r.is_other.Value == true ? r.row_name + " (Is Other)" : r.row_name } });
            }

            dct.Add("data", lst);

            return json.Serialize((Object)dct);
        }

        public static String RemoveQColumn(String sid, String qid)
        {
            Dictionary<String, Object> dct = new Dictionary<string, object>();
            List<Dictionary<String, String>> lst = new List<Dictionary<string, string>>();
            JavaScriptSerializer json = new JavaScriptSerializer();
            Regex rgxp = new Regex("^\\d+$");

            dct.Add("message", String.Empty);

            lst.Add(new Dictionary<string, string>() { { "id", "0" }, { "colname", "None" } });            
            
            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
            {
                Int32 cnt = db.VwSurveyGridAnswers.Count(obj => obj.question_id == Int32.Parse(qid) && obj.qgrid_col_id == Int32.Parse(sid));

                if (cnt != 0)
                    dct["message"] = "Answers have been supplied for this grid column. Can't carry out the action you requested";
                else
                    if (rgxp.IsMatch(sid) && rgxp.IsMatch(qid) && !sid.Equals("0"))
                    {
                        String s = String.Format("delete from t_qgrid_cols where id = {0}", sid);
                        try
                        {
                            Int32 rslt = db.ExecuteCommand(s);
                        }
                        catch (Exception xcp)
                        {
                            dct["message"] = xcp.Message.ToString();
                        }
                    }

                List<QGridColumn> rws = db.QGridColumns.Where(obj => obj.question_id == Int32.Parse(qid)).ToList();
                foreach (QGridColumn c in rws)
                {
                    String coldtyp = IncUtilxs.ColumnDataType(c.column_data_type.Value);
                    String colnm = String.IsNullOrEmpty(coldtyp) ? c.column_name : String.Format("{0} ({1})", c.column_name, coldtyp);
                    lst.Add(new Dictionary<string, string>() { { "id", c.id.ToString() }, { "colname", colnm } });
                }
            }

            dct.Add("data", lst);

            return json.Serialize((Object)dct);
        }

        public static String RemoveQRow(String sid, String qid)
        {
            Dictionary<String, Object> dct = new Dictionary<string, object>();
            List<Dictionary<String, String>> lst = new List<Dictionary<string, string>>();
            JavaScriptSerializer json = new JavaScriptSerializer();
            Regex rgxp = new Regex("^\\d+$");

            dct.Add("message", String.Empty);

            lst.Add(new Dictionary<string, string>() { { "id", "0" }, { "rowname", "None" } });

            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
            {
                Int32 cnt = db.VwSurveyGridAnswers.Count(obj => obj.question_id == Int32.Parse(qid) && obj.qgrid_row_id == Int32.Parse(sid));

                if (cnt != 0)
                    dct["message"] = "Answers have been supplied for this grid row. Can't carry out the action you requested";
                else
                    if (rgxp.IsMatch(sid) && rgxp.IsMatch(qid) && !sid.Equals("0"))
                    {
                        String s = String.Format("delete from t_qgrid_rows where id = {0}", sid);
                        try
                        {
                            Int32 rslt = db.ExecuteCommand(s);
                        }
                        catch (Exception xcp)
                        {
                            dct["message"] = xcp.Message.ToString();
                        }
                    }

                List<QGridRow> rws = db.QGridRows.Where(obj => obj.question_id == Int32.Parse(qid)).ToList();
                foreach (QGridRow r in rws)
                    lst.Add(new Dictionary<string, string>() { { "id", r.id.ToString() }, { "rowname", r.is_other.Value == true ? r.row_name + " (Is Other)" : r.row_name } });
            }

            dct.Add("data", lst);

            return json.Serialize((Object)dct);
        }

        public static String GotoSearchMemberPage(String spg)
        {
            List<Dictionary<String, String>> lst = new List<Dictionary<string, string>>();
            Dictionary<String, Object> dct = new Dictionary<string, object>();
            JavaScriptSerializer json = new JavaScriptSerializer();
            Regex rgxp = new Regex("^\\d+$");
            
            if (String.IsNullOrEmpty(spg)) spg = "0";

            dct.Add("count", "0");
            dct.Add("message", String.Empty);
            dct.Add("pages", "0");
            dct.Add("page", "0");

            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
            {
                String ntfymsk = String.Empty;
                Int32 sz = 0, len = 5, pgs = 1, rmndr = 0, skp = 0;

                IncDictionary dctitm = db.IncDictionaries.FirstOrDefault(obj => obj.dict_key.Equals("list_length"));
                if (dctitm != null) Int32.TryParse(dctitm.dict_value.ToString(), out len);

                IncDictionary uobj = db.IncDictionaries.FirstOrDefault(obj => obj.dict_key.Equals("search_notify_users"));
                if (uobj != null) ntfymsk = uobj.dict_value;

                if (rgxp.IsMatch(spg))
                {
                    skp = (Int32.Parse(spg) > 1 ? Int32.Parse(spg) - 1 : 0) * len;
                    dct["page"] = spg;
                }

                sz = db.IncUsers.Count(obj => obj.enabled == true);

                List<IncUser> rws = db.IncUsers.Where(obj => obj.enabled == true).OrderBy(obj => obj.lname).Skip(skp).Take(len).ToList();
                foreach (IncUser r in rws)
                    lst.Add(new Dictionary<string, string>() { { "id", String.Format(ntfymsk, r.id.ToString()) }, { "name", String.Format("{0}, {1}", r.lname, r.fnames) } });

                if (sz > len)
                {
                    pgs = sz / len;
                    rmndr = sz % len;
                    if (rmndr > 0) ++pgs;
                }

                dct["pages"] = pgs.ToString();
                dct["count"] = sz.ToString();
            }

            dct.Add("data", lst);
            return json.Serialize((Object)dct);
        }

        public static String GetCaseRow(String csno)
        {
            Dictionary<String, Object> csrw = new Dictionary<string, object>();

            List<Dictionary<String, String>> prvs = new List<Dictionary<string, string>>();
            List<Dictionary<String, String>> dstrcts = new List<Dictionary<string, string>>();
            List<Dictionary<String, String>> wds = new List<Dictionary<string, string>>();

            Dictionary<String, Object> dct = new Dictionary<string, object>();
            JavaScriptSerializer json = new JavaScriptSerializer();

            String prvid = "0";
            String dstrct = "0";
            String wd = "0";
            String plc = String.Empty;

            dct.Add("found", "0");
            dct.Add("province_id", "0");
            dct.Add("district_id", "0");
            dct.Add("ward_id", "0");
            dct.Add("place_name", String.Empty);

            prvs.Add(new Dictionary<string, string>() { { "province_id", "0" }, { "province", "UnSpecified" } });
            dstrcts.Add(new Dictionary<string, string>() { { "district_id", "0" }, { "district", "UnSpecified" } });
            wds.Add(new Dictionary<string, string>() { { "ward_id", "0" }, { "ward", "UnSpecified" } });

            using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["incAdoNet"].ConnectionString))
            {
                cnn.Open();

                String s = String.Format("select * from v_case_row where case_no = '{0}'", csno);
                SqlCommand cmd = new SqlCommand(s, cnn);
                SqlDataReader rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {                    
                    dct["found"] = "1";

                    prvid = rdr["province_id"].ToString();
                    dstrct = rdr["district_id"].ToString();
                    wd = rdr["ward_no"].ToString();
                    plc = rdr["place_name"] != null ? rdr["place_name"].ToString() : String.Empty;

                    for (Int16 i = 0; i < rdr.FieldCount; i++)
                    {
                        String fnm = rdr.GetName(i);
                        String fvl = rdr[fnm].ToString();

                        csrw.Add(fnm, fvl);                     
                    }
                }
                rdr.Close();                                

                s = "select * from v_provinces order by province";
                cmd = new SqlCommand(s, cnn);
                rdr = cmd.ExecuteReader();
                while (rdr.Read()) prvs.Add(new Dictionary<string, string>() { { "province_id", rdr["id"].ToString() }, { "province", rdr["province"].ToString() } });                
                rdr.Close();

                s = String.Format("select * from v_districts where province_id = {0} order by district", prvid.ToString());
                cmd = new SqlCommand(s, cnn);
                rdr = cmd.ExecuteReader();
                while (rdr.Read()) dstrcts.Add(new Dictionary<string, string>() { { "district_id", rdr["id"].ToString() }, { "district", rdr["district"].ToString() } });                
                rdr.Close();

                s = String.Format("select distinct ward_no from t_wards where province_pcode = {0} and district_pcode = {1} order by 1", prvid.ToString(), dstrct.ToString());
                cmd = new SqlCommand(s, cnn);
                rdr = cmd.ExecuteReader();
                while (rdr.Read()) wds.Add(new Dictionary<string, string>() { { "ward_id", rdr[0].ToString() }, { "ward", rdr[0].ToString() } });                
                rdr.Close();
            }

            dct["province_id"] = prvid;
            dct["district_id"] = dstrct;
            dct["ward_id"] = wd;
            dct["place_name"] = plc;

            dct.Add("provinces", prvs);
            dct.Add("districts", dstrcts);
            dct.Add("wards", wds);
            dct.Add("data", csrw);

            return json.Serialize((Object)dct);
        }

        public static String MakeCopyOptions(String qid, String did, String usr)
        {
            List<Dictionary<String, String>> lst = new List<Dictionary<string, string>>();
            Dictionary<String, Object> dct = new Dictionary<string, object>();
            JavaScriptSerializer json = new JavaScriptSerializer();

            Int32? rid = 0;

            dct["message"] = String.Empty;

            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
            {
                Int32 i = 0;

                i = db.SurveyAnswers.Count(obj => obj.question_id == Int32.Parse(qid));

                if (i != 0)
                    dct["message"] = "Answers have been supplied to this question, individually add options";
                else
                {
                    i = db.ExecuteCommand("delete from t_qlist_options where question_id = {0}", Int32.Parse(qid));

                    foreach (QListOption opt in db.QListOptions.Where(obj => obj.question_id == Int32.Parse(did)).ToList())
                    {
                        rid = 0;
                        i = db.p_write_qlist_option(ref rid, Int32.Parse(qid), opt.answer_option, usr);
                    }

                    foreach (QListOption opt in db.QListOptions.Where(obj => obj.question_id == Int32.Parse(qid)).ToList())
                        lst.Add(new Dictionary<string, string>() { { "id", opt.id.ToString() }, { "answer_option", opt.answer_option } });
                }
            }

            lst.Insert(0, new Dictionary<string, string>() { { "id", "0" }, { "answer_option", "None" } });
            dct["data"] = lst;

            return json.Serialize((Object)dct);
        }

        public static String MakeCopyColumns(String qid, String did, String dttyp, String usr)
        {
            List<Dictionary<String, String>> lst = new List<Dictionary<string, string>>();
            Dictionary<String, Object> dct = new Dictionary<string, object>();
            JavaScriptSerializer json = new JavaScriptSerializer();

            Int32? rid = 0;

            dct["message"] = String.Empty;

            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
            {
                Int32 i = 0;

                i = db.SurveyAnswers.Count(obj => obj.question_id == Int32.Parse(qid));

                if (i != 0)
                    dct["message"] = "Answers have been supplied to this question, individually add columns";
                else
                {
                    i = db.ExecuteCommand("delete from t_qgrid_cols where question_id = {0}", Int32.Parse(qid));

                    foreach (QGridColumn r in db.QGridColumns.Where(obj => obj.question_id == Int32.Parse(did)).ToList())
                    {
                        rid = 0;
                        i = db.p_write_qgrid_column(ref rid, Int32.Parse(qid), Byte.Parse(dttyp), r.column_name, usr);
                    }

                    foreach (QGridColumn r in db.QGridColumns.Where(obj => obj.question_id == Int32.Parse(qid)).ToList())
                    {
                        String coldtyp = IncUtilxs.ColumnDataType(r.column_data_type.Value);
                        String colnm = String.IsNullOrEmpty(coldtyp) ? r.column_name : String.Format("{0} ({1})", r.column_name, coldtyp);
                        lst.Add(new Dictionary<string, string>() { { "id", r.id.ToString() }, { "colname", colnm } });
                    }
                }
            }

            lst.Insert(0, new Dictionary<string, string>() { { "id", "0" }, { "colname", "None" } });
            dct["data"] = lst;

            return json.Serialize((Object)dct);
        }

        public static String MakeCopyRows(String qid, String did, String usr)
        {
            List<Dictionary<String, String>> lst = new List<Dictionary<string, string>>();
            Dictionary<String, Object> dct = new Dictionary<string, object>();
            JavaScriptSerializer json = new JavaScriptSerializer();

            Int32? rid = 0;

            dct["message"] = String.Empty;

            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
            {
                Int32 i = 0;

                i = db.SurveyAnswers.Count(obj => obj.question_id == Int32.Parse(qid));

                if (i != 0)
                    dct["message"] = "Answers have been supplied to this question, individually add rows";
                else
                {
                    i = db.ExecuteCommand("delete from t_qgrid_rows where question_id = {0}", Int32.Parse(qid));

                    foreach (QGridRow r in db.QGridRows.Where(obj => obj.question_id == Int32.Parse(did)).ToList())
                    {                        
                        rid = 0;
                        i = db.p_write_qgrid_row(ref rid, Int32.Parse(qid), r.is_other.Value, r.row_name, usr);
                    }

                    foreach (QGridRow r in db.QGridRows.Where(obj => obj.question_id == Int32.Parse(qid)).ToList())
                        lst.Add(new Dictionary<string, string>() { { "id", r.id.ToString() }, { "rowname", r.is_other.Value == true ? r.row_name + " (Is Other)" : r.row_name } });
                }
            }

            lst.Insert(0, new Dictionary<string, string>() { { "id", "0" }, { "rowname", "None" } });
            dct["data"] = lst;

            return json.Serialize((Object)dct);
        }

        public static String GetOPMProvinces()
        {
            List<Dictionary<String, String>> lst = new List<Dictionary<string, string>>();
            Dictionary<String, Object> dct = new Dictionary<string, object>();
            JavaScriptSerializer json = new JavaScriptSerializer();

            dct["message"] = String.Empty;

            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
            {
                foreach (Province p in db.Provinces.Where(obj => obj.id != 0).ToList())
                    lst.Add(new Dictionary<string, string>() { { "id", p.id.ToString() }, { "province", p.province }, { "latitude", String.Empty }, { "longitude", String.Empty } });

                foreach (Dictionary<String, String> d in lst)
                {
                    ZimPlace zmp = db.ZimPlaces.FirstOrDefault(obj => obj.place.Equals(d["province"]));
                    if (zmp != null)
                    {
                        d["latitude"] = zmp.latitude;
                        d["longitude"] = zmp.longitude;
                    }
                }
            }

            dct["data"] = lst;

            return json.Serialize((Object)dct);
        }

        public static String GetOPMDistricts()
        {
            List<Dictionary<String, String>> lst = new List<Dictionary<string, string>>();
            Dictionary<String, Object> dct = new Dictionary<string, object>();
            JavaScriptSerializer json = new JavaScriptSerializer();

            dct["message"] = String.Empty;

            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
            {
                foreach (District ds in db.Districts.Where(obj => obj.id != 0).ToList())
                    lst.Add(new Dictionary<string, string>() { { "id", ds.id.ToString() }, { "district", ds.district }, { "latitude", String.Empty }, { "longitude", String.Empty } });

                foreach (Dictionary<String, String> d in lst)
                {
                    ZimPlace zmp = db.ZimPlaces.FirstOrDefault(obj => obj.place.Equals(d["district"]));
                    if (zmp != null)
                    {
                        d["latitude"] = zmp.latitude;
                        d["longitude"] = zmp.longitude;
                    }
                }
            }

            dct["data"] = lst;

            return json.Serialize((Object)dct);
        }

        public static String GetCaseSummaryMapData(String yf, String yt, String mf, String mt, String inc, String caus, String pep, String styp)
        {            
            List<Dictionary<String, String>> lst = new List<Dictionary<string, string>>();
            Dictionary<String, Object> dct = new Dictionary<string, object>();
            JavaScriptSerializer json = new JavaScriptSerializer();
            
            DateTime dtfrm = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            DateTime dtto = dtfrm.AddMonths(1).AddDays(-1);
            String s = String.Empty;

            dct["message"] = String.Empty;

            try
            {
                mf = mf.Trim().Length == 1 ? "0" + mf.Trim() : mf;
                mt = mt.Trim().Length == 1 ? "0" + mt.Trim() : mt;

                s = String.Format("01/{0}/{1}", mf, yf);
                dtfrm = DateTime.ParseExact(s, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                
                s = String.Format("01/{0}/{1}", mt, yt);
                dtto = DateTime.ParseExact(s, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);

                Int32 cmpr = DateTime.Compare(dtto, dtfrm);
                if (cmpr < 0) dtto = dtfrm;

                dtto = dtto.AddMonths(1).AddDays(-1);
            }
            catch (Exception xcp)
            {
                dct["message"] = String.Format("!!! Error : Invalid dates : {0}", xcp.Message);

                dtfrm = dtfrm = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                dtto = dtfrm.AddMonths(1).AddDays(-1);
            }

            String sproc = "p_prep_provmap_data";
            if (styp.Equals("2"))
                sproc = "p_prep_distmap_data";
            else
                if (styp.Equals("3"))
                    sproc = "p_prep_pointmap_data";
            
            using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["incAdoNet"].ConnectionString))
            {
                cnn.Open();

                SqlCommand cmd = null;
                SqlDataReader rdr = null;

                s = String.Format("execute {0} '{1}', '{2}', {3}, {4}, {5}", sproc, dtfrm.ToString("dd/MM/yyyy"), dtto.ToString("dd/MM/yyyy"), inc, caus, pep);
                cmd = new SqlCommand(s, cnn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Dictionary<String, String> rw = new Dictionary<string, string>();

                    for (int f = 0; f < rdr.FieldCount; f++)
                        switch (rdr.GetName(f).ToString())
                        {
                            case "date_reported":
                                rw[rdr.GetName(f)] = rdr[f] != null ? DateTime.Parse(rdr[f].ToString()).ToString("dd/MM/yyyy") : String.Empty;
                                break;
                            case "sum_affected":
                            case "count_cases":
                            case "ward_no":
                            case "num_households":
                            case "num_individuals":
                                rw[rdr.GetName(f)] = rdr[f] != null ? IncUtilxs.SilenceZero(rdr[f].ToString()) : String.Empty;
                                break;
                            default:
                                rw[rdr.GetName(f)] = rdr[f] != null ? rdr[f].ToString() : String.Empty;
                                break;
                        }
                    
                    lst.Add(rw);
                }
            }

            using (IncDataClassesDataContext db = new IncDataClassesDataContext())
                foreach (Dictionary<String, String> rw in lst) GrabGPScoords(db, rw);

            dct["data"] = lst;

            return json.Serialize((Object)dct);
        }

        private static void GrabGPScoords(IncDataClassesDataContext db, Dictionary<String, String> rw)
        {
            if (rw.Keys.Contains("lng") && rw.Keys.Contains("lat"))
            {
                if (!String.IsNullOrEmpty(rw["lng"]) && !String.IsNullOrEmpty(rw["lat"])) return;

                ZimPlace zmp;
                Boolean found = false;

                if (!String.IsNullOrEmpty(rw["place_name"]))
                {
                    zmp = db.ZimPlaces.FirstOrDefault(obj => obj.place.ToUpper().Contains(rw["place_name"].ToUpper()));
                    if (zmp != null)
                    {
                        rw["lat"] = zmp.latitude;
                        rw["lng"] = zmp.longitude;
                        found = true;
                    }
                }

                if (found) return;
                if (!String.IsNullOrEmpty(rw["district"]))
                {
                    zmp = db.ZimPlaces.FirstOrDefault(obj => obj.place.ToUpper().Contains(rw["district"].ToUpper()));
                    if (zmp != null)
                    {
                        rw["lat"] = zmp.latitude;
                        rw["lng"] = zmp.longitude;
                        found = true;
                    }
                }

                if (found) return;
                if (!String.IsNullOrEmpty(rw["province"]))
                {
                    zmp = db.ZimPlaces.FirstOrDefault(obj => obj.place.ToUpper().Contains(rw["province"].ToUpper()));
                    if (zmp != null)
                    {
                        rw["lat"] = zmp.latitude;
                        rw["lng"] = zmp.longitude;                        
                    }
                }
            }
        }

        public static String GetAreaCases(String prv, String dst)
        {
            List<Dictionary<String, String>> lst = new List<Dictionary<string, string>>();
            Dictionary<String, Object> dct = new Dictionary<string, object>();
            JavaScriptSerializer json = new JavaScriptSerializer();

            using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["incAdoNet"].ConnectionString))
            {
                cnn.Open();

                String qt = "select cs.id, cn.case_no, cs.province_id, cs.district_id, cs.ward_no, cs.date_reported, cs.place_name, ti.incident_type";
                qt += " from t_cases cs";
                qt += " inner join t_case_numbers cn on cn.case_id = cs.id";
                qt += " inner join t_type_incidents ti on ti.id = cs.incident_type_id";
                qt += " where cs.province_id = {0} and cs.district_id = {1}";
                qt += " order by cn.case_no";

                String sql = String.Format(qt, prv, dst);

                SqlCommand cmd = new SqlCommand(sql, cnn);
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    Dictionary<String, String> itm = new Dictionary<string, string>();

                    for (Int32 i = 0; i < rdr.FieldCount; i++)
                        if (rdr.GetName(i).ToString().Equals("date_reported"))
                            itm.Add(rdr.GetName(i).ToString(), rdr.GetValue(i) != System.DBNull.Value ? DateTime.Parse(rdr.GetValue(i).ToString()).ToString("dd/MM/yyyy") : String.Empty);
                        else
                            itm.Add(rdr.GetName(i).ToString(), rdr.GetValue(i) != System.DBNull.Value ? rdr.GetValue(i).ToString() : String.Empty);

                    lst.Add(itm);
                }
            }

            dct["data"] = lst;

            return json.Serialize((Object)dct);
        }

        public static String GetCaseAttributes(String csid)
        {            
            Dictionary<String, Object> dct = new Dictionary<string, object>();
            Dictionary<String, String> csdct = new Dictionary<string,string>();
            JavaScriptSerializer json = new JavaScriptSerializer();

            List<String> dtflds = new List<string>() { "date_reported", "incident_date", "verified_on", "inserted_on", "updated_on" };

            using (SqlConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["incAdoNet"].ConnectionString))
            {
                cnn.Open();

                String sql = String.Format("select * from v_case_row where id = {0}", csid);

                SqlCommand cmd = new SqlCommand(sql, cnn);
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    for (Int16 i = 0; i < rdr.FieldCount; i++)
                        if (dtflds.Contains(rdr.GetName(i).ToString()))
                            csdct.Add(rdr.GetName(i).ToString(), rdr.GetValue(i) != System.DBNull.Value ? DateTime.Parse(rdr.GetValue(i).ToString()).ToString("dd/MM/yyyy") : String.Empty);
                        else
                            csdct.Add(rdr.GetName(i).ToString(), rdr.GetValue(i) != System.DBNull.Value ? rdr.GetValue(i).ToString() : String.Empty);

                    break;
                }
                rdr.Close();
            }

            dct["data"] = csdct;

            return json.Serialize((Object)dct);
        }
    }
}
