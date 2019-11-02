using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

namespace Web
{
    public partial class Summery : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user"] == null)
                Response.Redirect("Login.aspx");

            DBMangement dbMan = new DBMangement();
            List<int> lstYears = dbMan.GetReportedYears();
            if (cmbYearFrom.Items.Count == 0)
            {
                for (int i = 1; i <= lstYears.Count; i++)
                {
                    cmbYearFrom.Items.Add(lstYears[i - 1].ToString());
                    cmbYearFrom.Items[i - 1].Value = lstYears[i - 1].ToString();

                    cmbYearTo.Items.Add(lstYears[i - 1].ToString());
                    cmbYearTo.Items[i - 1].Value = lstYears[i - 1].ToString();
                }
            }
            if (cmbMonthFrom.Items.Count == 0)
            {
                string[] month = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
                for (int i = 1; i <= 12; i++)
                {
                    cmbMonthFrom.Items.Add(month[i - 1]);
                    cmbMonthFrom.Items[i - 1].Value = i.ToString();

                    cmbMonthTo.Items.Add(month[i - 1]);
                    cmbMonthTo.Items[i - 1].Value = i.ToString();
                }
            }
            List<ProvinceItem> lstProvince = dbMan.GetProvince();
            if (cmbProvince.Items.Count == 0)
            {
                cmbProvince.Items.Add("All");
                cmbProvince.Items[0].Value = "-1";
                for (int i = 1; i <= lstProvince.Count; i++)
                {
                    cmbProvince.Items.Add(lstProvince[i - 1].Name.ToString());
                    cmbProvince.Items[i].Value = lstProvince[i - 1].ID.ToString();
                }
            }
            List<DisplacementItem> lstDisplacement = dbMan.GetDisplacement();
            if (cmbDisplacement.Items.Count == 0)
            {
                cmbDisplacement.Items.Add("All");
                cmbDisplacement.Items[0].Value = "-1";
                for (int i = 1; i <= lstDisplacement.Count; i++)
                {
                    cmbDisplacement.Items.Add(lstDisplacement[i - 1].Name.ToString());
                    cmbDisplacement.Items[i].Value = lstDisplacement[i - 1].ID.ToString();
                }
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            if (rdoCasualtiesList.Checked)
            {
                ExportCasualtiesList();
            }
            else if (rdoNumberOfCasualtiesPeriod.Checked)
            {
                ExportNumberOfCasualtiesPeriod();
            }
            else if (rdoDamagedInfrastructure.Checked)
            {
                ExportDamagedInfrastructure();
            }
            else if (rdoAccesstoEssentialServices.Checked)
            {
                ExportAccesstoEssentialServices();
            }
            else if (rdoListofpeopleaffected.Checked)
            {
                ExportListofpeopleaffected();
            }
            else if (rdoDisasterincidentsandcauses.Checked)
            {
                ExportListofpeopleaffected();
            }
            else if (rdoAmountOfFunds.Checked)
            {
                ExportAmountOfFundsCommunityByTypeOrAssistance();
            }
            else if (rdoIncidentAndCauses.Checked)
            {
                ExportIncidentAndCausesreport();
            }
        }

        private void ExportCasualtiesList()
        {
            DBMangement dbMan = new DBMangement();
            SqlConnection sqlConnection1 = new SqlConnection(dbMan.ConnectionString());
            sqlConnection1.Open();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "GetSummeryData";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@province_id", SqlDbType.Int).Value = cmbProvince.SelectedValue;
            cmd.Parameters.Add("@displacement_id", SqlDbType.Int).Value = cmbDisplacement.SelectedValue;
            cmd.Parameters.Add("@fromYear", SqlDbType.Int).Value = cmbYearFrom.SelectedValue;
            cmd.Parameters.Add("@fromMonth", SqlDbType.Int).Value = cmbMonthFrom.SelectedValue;
            cmd.Parameters.Add("@toYear", SqlDbType.Int).Value = cmbYearTo.SelectedValue;
            cmd.Parameters.Add("@toMonth", SqlDbType.Int).Value = cmbMonthTo.SelectedValue;
            cmd.Connection = sqlConnection1;


            reader = cmd.ExecuteReader();


            string excelName = "CasualtiesList";

            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment;filename=" + excelName + ".xls");
            Response.AddHeader("Content-Type", "application/vnd.ms-excel");

            Response.Write("Case No.");
            Response.Write("\t");
            Response.Write("Date Incident Occured");
            Response.Write("\t");
            Response.Write("Province");
            Response.Write("\t");
            Response.Write("District");
            Response.Write("\t");
            Response.Write("Casualty");
            Response.Write("\t");
            Response.Write("No. Males18+");
            Response.Write("\t");
            Response.Write("No. Females 18+");
            Response.Write("\t");
            Response.Write("No. Males 0-17");
            Response.Write("\t");
            Response.Write("No. Females 0-17");
            Response.Write("\t");
            Response.Write("No. Communities");
            Response.Write("\t");
            Response.Write("No. Households");
            Response.Write("\t");
            Response.Write("\n");

            while (reader.Read())
            {
                Response.Write(reader["caseno"]);
                Response.Write("\t");
                Response.Write(reader["date_incident_occured"]);
                Response.Write("\t");
                Response.Write(reader["province_name"]);
                Response.Write("\t");
                Response.Write(reader["district_name"]);
                Response.Write("\t");
                Response.Write("Injured");
                Response.Write("\t");
                Response.Write(reader["affected_population_11"]);
                Response.Write("\t");
                Response.Write(reader["affected_population_12"]);
                Response.Write("\t");
                Response.Write(reader["affected_population_13"]);
                Response.Write("\t");
                Response.Write(reader["affected_population_14"]);
                Response.Write("\t");
                Response.Write(reader["affected_population_15"]);
                Response.Write("\t");
                Response.Write(reader["affected_population_16"]);
                Response.Write("\t");
                Response.Write("\n");

                Response.Write("");
                Response.Write("\t");
                Response.Write("");
                Response.Write("\t");
                Response.Write("");
                Response.Write("\t");
                Response.Write("");
                Response.Write("\t");
                Response.Write("Stranded or Marooned");
                Response.Write("\t");
                Response.Write(reader["affected_population_21"]);
                Response.Write("\t");
                Response.Write(reader["affected_population_22"]);
                Response.Write("\t");
                Response.Write(reader["affected_population_23"]);
                Response.Write("\t");
                Response.Write(reader["affected_population_24"]);
                Response.Write("\t");
                Response.Write(reader["affected_population_25"]);
                Response.Write("\t");
                Response.Write(reader["affected_population_26"]);
                Response.Write("\t");
                Response.Write("\n");

                Response.Write("");
                Response.Write("\t");
                Response.Write("");
                Response.Write("\t");
                Response.Write("");
                Response.Write("\t");
                Response.Write("");
                Response.Write("\t");
                Response.Write("Missing");
                Response.Write("\t");
                Response.Write(reader["affected_population_31"]);
                Response.Write("\t");
                Response.Write(reader["affected_population_32"]);
                Response.Write("\t");
                Response.Write(reader["affected_population_33"]);
                Response.Write("\t");
                Response.Write(reader["affected_population_34"]);
                Response.Write("\t");
                Response.Write(reader["affected_population_35"]);
                Response.Write("\t");
                Response.Write(reader["affected_population_36"]);
                Response.Write("\t");
                Response.Write("\n");

                Response.Write("");
                Response.Write("\t");
                Response.Write("");
                Response.Write("\t");
                Response.Write("");
                Response.Write("\t");
                Response.Write("");
                Response.Write("\t");
                Response.Write("Dead");
                Response.Write("\t");
                Response.Write(reader["affected_population_41"]);
                Response.Write("\t");
                Response.Write(reader["affected_population_42"]);
                Response.Write("\t");
                Response.Write(reader["affected_population_43"]);
                Response.Write("\t");
                Response.Write(reader["affected_population_44"]);
                Response.Write("\t");
                Response.Write(reader["affected_population_45"]);
                Response.Write("\t");
                Response.Write(reader["affected_population_46"]);
                Response.Write("\t");
                Response.Write("\n");

                Response.Write("");
                Response.Write("\t");
                Response.Write("");
                Response.Write("\t");
                Response.Write("");
                Response.Write("\t");
                Response.Write("");
                Response.Write("\t");
                Response.Write("Ailing");
                Response.Write("\t");
                Response.Write(reader["affected_population_51"]);
                Response.Write("\t");
                Response.Write(reader["affected_population_52"]);
                Response.Write("\t");
                Response.Write(reader["affected_population_53"]);
                Response.Write("\t");
                Response.Write(reader["affected_population_54"]);
                Response.Write("\t");
                Response.Write(reader["affected_population_55"]);
                Response.Write("\t");
                Response.Write(reader["affected_population_56"]);
                Response.Write("\t");
                Response.Write("\n");

                Response.Write("");
                Response.Write("\t");
                Response.Write("");
                Response.Write("\t");
                Response.Write("");
                Response.Write("\t");
                Response.Write("");
                Response.Write("\t");
                Response.Write("Other");
                Response.Write("\t");
                Response.Write(reader["affected_population_61"]);
                Response.Write("\t");
                Response.Write(reader["affected_population_62"]);
                Response.Write("\t");
                Response.Write(reader["affected_population_63"]);
                Response.Write("\t");
                Response.Write(reader["affected_population_64"]);
                Response.Write("\t");
                Response.Write(reader["affected_population_65"]);
                Response.Write("\t");
                Response.Write(reader["affected_population_66"]);
                Response.Write("\t");
                Response.Write("\n");
            }
            sqlConnection1.Close();

            Response.End();
        }

        private void ExportNumberOfCasualtiesPeriod()
        {
            DBMangement dbMan = new DBMangement();
            SqlConnection sqlConnection1 = new SqlConnection(dbMan.ConnectionString());
            sqlConnection1.Open();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "GetSummeryData";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@province_id", SqlDbType.Int).Value = cmbProvince.SelectedValue;
            cmd.Parameters.Add("@displacement_id", SqlDbType.Int).Value = cmbDisplacement.SelectedValue;
            cmd.Parameters.Add("@fromYear", SqlDbType.Int).Value = cmbYearFrom.SelectedValue;
            cmd.Parameters.Add("@fromMonth", SqlDbType.Int).Value = cmbMonthFrom.SelectedValue;
            cmd.Parameters.Add("@toYear", SqlDbType.Int).Value = cmbYearTo.SelectedValue;
            cmd.Parameters.Add("@toMonth", SqlDbType.Int).Value = cmbMonthTo.SelectedValue;
            cmd.Connection = sqlConnection1;


            reader = cmd.ExecuteReader();

            string excelName = "NumberOfCasualtiesPeriod";

            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment;filename=" + excelName + ".xls");
            Response.AddHeader("Content-Type", "application/vnd.ms-excel");

            Response.Write("Case No.");
            Response.Write("\t");
            Response.Write("Date Incident Occured");
            Response.Write("\t");
            Response.Write("Province");
            Response.Write("\t");
            Response.Write("District");
            Response.Write("\t");
            Response.Write("Totals");
            Response.Write("\t");
            Response.Write("\n");

            while (reader.Read())
            {
                Response.Write(reader["caseno"]);
                Response.Write("\t");
                Response.Write(reader["date_incident_occured"]);
                Response.Write("\t");
                Response.Write(reader["province_name"]);
                Response.Write("\t");
                Response.Write(reader["district_name"]);
                Response.Write("\t");
                Response.Write(reader["casualties_total"]);
                Response.Write("\t");
                Response.Write("\n");
            }
            sqlConnection1.Close();

            Response.End();
        }
        private void ExportDamagedInfrastructure()
        {
            DBMangement dbMan = new DBMangement();
            SqlConnection sqlConnection1 = new SqlConnection(dbMan.ConnectionString());
            sqlConnection1.Open();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "GetSummeryData";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@province_id", SqlDbType.Int).Value = cmbProvince.SelectedValue;
            cmd.Parameters.Add("@displacement_id", SqlDbType.Int).Value = cmbDisplacement.SelectedValue;
            cmd.Parameters.Add("@fromYear", SqlDbType.Int).Value = cmbYearFrom.SelectedValue;
            cmd.Parameters.Add("@fromMonth", SqlDbType.Int).Value = cmbMonthFrom.SelectedValue;
            cmd.Parameters.Add("@toYear", SqlDbType.Int).Value = cmbYearTo.SelectedValue;
            cmd.Parameters.Add("@toMonth", SqlDbType.Int).Value = cmbMonthTo.SelectedValue;
            cmd.Connection = sqlConnection1;


            reader = cmd.ExecuteReader();

            string excelName = "DamagedInfrastructure";

            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment;filename=" + excelName + ".xls");
            Response.AddHeader("Content-Type", "application/vnd.ms-excel");

            Response.Write("Case No.");
            Response.Write("\t");
            Response.Write("Date Incident Occured");
            Response.Write("\t");
            Response.Write("Province");
            Response.Write("\t");
            Response.Write("District");
            Response.Write("\t");
            Response.Write("Roads & Highways");
            Response.Write("\t");
            Response.Write("Service Stations");
            Response.Write("\t");
            Response.Write("Electrical power lines");
            Response.Write("\t");
            Response.Write("Electrical power substation");
            Response.Write("\t");
            Response.Write("Natural gas, petroleum pipelines");
            Response.Write("\t");
            Response.Write("Telephone lines");
            Response.Write("\t");
            Response.Write("Mobile phone towers");
            Response.Write("\t");
            Response.Write("Internet or fibre optic lines");
            Response.Write("\t");
            Response.Write("Schools, Colleges and Educational Training Facilities");
            Response.Write("\t");
            Response.Write("Shopping Centre or Complex");
            Response.Write("\t");
            Response.Write("Homes, houses or similar dwelling place");
            Response.Write("\t");
            Response.Write("Building and apartments");
            Response.Write("\t");
            Response.Write("Industrial Facility or Complex");
            Response.Write("\t");
            Response.Write("Government Facility");
            Response.Write("\t");
            Response.Write("Bridges");
            Response.Write("\t");
            Response.Write("Other");
            Response.Write("\t");
            Response.Write("\n");

            while (reader.Read())
            {
                Response.Write(reader["caseno"]);
                Response.Write("\t");
                Response.Write(reader["date_incident_occured"]);
                Response.Write("\t");
                Response.Write(reader["province_name"]);
                Response.Write("\t");
                Response.Write(reader["district_name"]);
                Response.Write("\t");
                Response.Write(reader["rh_qty"]);
                Response.Write("\t");
                Response.Write(reader["ss_qty"]);
                Response.Write("\t");
                Response.Write(reader["epl_qty"]);
                Response.Write("\t");
                Response.Write(reader["eps_qty"]);
                Response.Write("\t");
                Response.Write(reader["ngpp_qty"]);
                Response.Write("\t");
                Response.Write(reader["tl_qty"]);
                Response.Write("\t");
                Response.Write(reader["mpt_qty"]);
                Response.Write("\t");
                Response.Write(reader["ifol_qty"]);
                Response.Write("\t");
                Response.Write(reader["sce_qty"]);
                Response.Write("\t");
                Response.Write(reader["scc_qty"]);
                Response.Write("\t");
                Response.Write(reader["hhs_qty"]);
                Response.Write("\t");
                Response.Write(reader["ba_qty"]);
                Response.Write("\t");
                Response.Write(reader["ifc_qty"]);
                Response.Write("\t");
                Response.Write(reader["gf_qty"]);
                Response.Write("\t");
                Response.Write(reader["b_qty"]);
                Response.Write("\t");
                Response.Write(reader["other_qty"]);
                Response.Write("\t");
                Response.Write("\n");
            }
            sqlConnection1.Close();

            Response.End();
        }
        private void ExportAccesstoEssentialServices()
        {
            DBMangement dbMan = new DBMangement();
            SqlConnection sqlConnection1 = new SqlConnection(dbMan.ConnectionString());
            sqlConnection1.Open();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "GetSummeryData";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@province_id", SqlDbType.Int).Value = cmbProvince.SelectedValue;
            cmd.Parameters.Add("@displacement_id", SqlDbType.Int).Value = cmbDisplacement.SelectedValue;
            cmd.Parameters.Add("@fromYear", SqlDbType.Int).Value = cmbYearFrom.SelectedValue;
            cmd.Parameters.Add("@fromMonth", SqlDbType.Int).Value = cmbMonthFrom.SelectedValue;
            cmd.Parameters.Add("@toYear", SqlDbType.Int).Value = cmbYearTo.SelectedValue;
            cmd.Parameters.Add("@toMonth", SqlDbType.Int).Value = cmbMonthTo.SelectedValue;
            cmd.Connection = sqlConnection1;


            reader = cmd.ExecuteReader();

            string excelName = "AccesstoEssentialServices";

            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment;filename=" + excelName + ".xls");
            Response.AddHeader("Content-Type", "application/vnd.ms-excel");

            Response.Write("Case No.");
            Response.Write("\t");
            Response.Write("Date Incident Occured");
            Response.Write("\t");
            Response.Write("Province");
            Response.Write("\t");
            Response.Write("District");
            Response.Write("\t");
            Response.Write("Schools");
            Response.Write("\t");
            Response.Write("Hospital or Clinic");
            Response.Write("\t");
            Response.Write("Policing & Security");
            Response.Write("\t");
            Response.Write("Water Supplies (Safe water)");
            Response.Write("\t");
            Response.Write("Food Provisions");
            Response.Write("\t");
            Response.Write("Other");
            Response.Write("\t");
            Response.Write("\n");

            while (reader.Read())
            {
                Response.Write(reader["caseno"]);
                Response.Write("\t");
                Response.Write(reader["date_incident_occured"]);
                Response.Write("\t");
                Response.Write(reader["province_name"]);
                Response.Write("\t");
                Response.Write(reader["district_name"]);
                Response.Write("\t");
                Response.Write(reader["school_chk"]);
                Response.Write("\t");
                Response.Write(reader["hospital_chk"]);
                Response.Write("\t");
                Response.Write(reader["policing_chk"]);
                Response.Write("\t");
                Response.Write(reader["water_chk"]);
                Response.Write("\t");
                Response.Write(reader["food_chk"]);
                Response.Write("\t");
                Response.Write(reader["other_chk"]);
                Response.Write("\t");
                Response.Write("\n");
            }
            sqlConnection1.Close();

            Response.End();
        }
        private void ExportListofpeopleaffected()
        {
            DBMangement dbMan = new DBMangement();
            SqlConnection sqlConnection1 = new SqlConnection(dbMan.ConnectionString());
            sqlConnection1.Open();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "GetSummeryData";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@province_id", SqlDbType.Int).Value = cmbProvince.SelectedValue;
            cmd.Parameters.Add("@displacement_id", SqlDbType.Int).Value = cmbDisplacement.SelectedValue;
            cmd.Parameters.Add("@fromYear", SqlDbType.Int).Value = cmbYearFrom.SelectedValue;
            cmd.Parameters.Add("@fromMonth", SqlDbType.Int).Value = cmbMonthFrom.SelectedValue;
            cmd.Parameters.Add("@toYear", SqlDbType.Int).Value = cmbYearTo.SelectedValue;
            cmd.Parameters.Add("@toMonth", SqlDbType.Int).Value = cmbMonthTo.SelectedValue;
            cmd.Connection = sqlConnection1;


            reader = cmd.ExecuteReader();

            string excelName = "Listofpeopleaffected";

            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment;filename=" + excelName + ".xls");
            Response.AddHeader("Content-Type", "application/vnd.ms-excel");

            Response.Write("Case No.");
            Response.Write("\t");
            Response.Write("Date Incident Occured");
            Response.Write("\t");
            Response.Write("Province");
            Response.Write("\t");
            Response.Write("District");
            Response.Write("\t");
            Response.Write("Nationality");
            Response.Write("\t");
            Response.Write("Vulnerability Level");
            Response.Write("\t");
            Response.Write("Role");
            Response.Write("\t");
            Response.Write("Physical Address");
            Response.Write("\t");
            Response.Write("Occupation");
            Response.Write("\t");
            Response.Write("National ID");
            Response.Write("\t");
            Response.Write("Marital Status");
            Response.Write("\t");
            Response.Write("Level of Education");
            Response.Write("\t");
            Response.Write("Is Bread Winner");
            Response.Write("\t");
            Response.Write("Employer");
            Response.Write("\t");
            Response.Write("Email");
            Response.Write("\t");
            Response.Write("Displacement Status");
            Response.Write("\t");
            Response.Write("Contact Phone");
            Response.Write("\t");
            Response.Write("Comments");
            Response.Write("\t");
            Response.Write("\n");

            while (reader.Read())
            {
                Response.Write(reader["caseno"]);
                Response.Write("\t");
                Response.Write(reader["date_incident_occured"]);
                Response.Write("\t");
                Response.Write(reader["province_name"]);
                Response.Write("\t");
                Response.Write(reader["district_name"]);
                Response.Write("\t");
                Response.Write(reader["nationality"]);
                Response.Write("\t");
                Response.Write(reader["vulnerability_level"]);
                Response.Write("\t");
                Response.Write(reader["role"]);
                Response.Write("\t");
                Response.Write(reader["physical_address"]);
                Response.Write("\t");
                Response.Write(reader["occupation"]);
                Response.Write("\t");
                Response.Write(reader["national_id"]);
                Response.Write("\t");
                Response.Write(reader["marital_status"]);
                Response.Write("\t");
                Response.Write(reader["level_of_education"]);
                Response.Write("\t");
                Response.Write(reader["is_bread_winner"]);
                Response.Write("\t");
                Response.Write(reader["employer"]);
                Response.Write("\t");
                Response.Write(reader["email"]);
                Response.Write("\t");
                Response.Write(reader["displacement_status"]);
                Response.Write("\t");
                Response.Write(reader["contact_phone"]);
                Response.Write("\t");
                Response.Write(reader["comment"]);
                Response.Write("\t");
                Response.Write("\n");
            }
            sqlConnection1.Close();

            Response.End();
        }
        private void ExportAmountOfFundsCommunityByTypeOrAssistance()
        {
            DBMangement dbMan = new DBMangement();
            SqlConnection sqlConnection1 = new SqlConnection(dbMan.ConnectionString());
            sqlConnection1.Open();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "GetSummeryData";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@province_id", SqlDbType.Int).Value = cmbProvince.SelectedValue;
            cmd.Parameters.Add("@displacement_id", SqlDbType.Int).Value = cmbDisplacement.SelectedValue;
            cmd.Parameters.Add("@fromYear", SqlDbType.Int).Value = cmbYearFrom.SelectedValue;
            cmd.Parameters.Add("@fromMonth", SqlDbType.Int).Value = cmbMonthFrom.SelectedValue;
            cmd.Parameters.Add("@toYear", SqlDbType.Int).Value = cmbYearTo.SelectedValue;
            cmd.Parameters.Add("@toMonth", SqlDbType.Int).Value = cmbMonthTo.SelectedValue;
            cmd.Connection = sqlConnection1;


            reader = cmd.ExecuteReader();

            string excelName = "ListOfAssistance.xls";

            var result = new List<AmountOfFundsCommunity>();
            while (reader.Read())
            {
                result.Add(new AmountOfFundsCommunity
                {
                    DateIncidentOccurred = Convert.ToString(reader["date_incident_reported"]),
                    Province = Convert.ToString(reader["province_name"]),
                    District = Convert.ToString(reader["district_name"]),
                    Llg = Convert.ToString(reader["llg_name"]),
                    Ward = Convert.ToString(reader["ward_no"]),
                    Placename = Convert.ToString(reader["placename"]),
                    Caseno = Convert.ToString(reader["caseno"]),
                    AssistanceDate = Convert.ToString(reader["AssistanceDate"]),
                    AssistanceTypeName = Convert.ToString(reader["AssistanceTypeName"]),
                    Beneficiaries = Convert.ToInt32(reader["ABeneficiaries"]),
                    Comments = Convert.ToString(reader["Comments"])
                });

            }
            sqlConnection1.Close();
            CreateExcelFile.CreateExcelDocument(result, excelName, Response);
        }
        private void ExportIncidentAndCausesreport()
        {
            DBMangement dbMan = new DBMangement();
            SqlConnection sqlConnection1 = new SqlConnection(dbMan.ConnectionString());
            sqlConnection1.Open();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "GetSummeryData";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@province_id", SqlDbType.Int).Value = cmbProvince.SelectedValue;
            cmd.Parameters.Add("@displacement_id", SqlDbType.Int).Value = cmbDisplacement.SelectedValue;
            cmd.Parameters.Add("@fromYear", SqlDbType.Int).Value = cmbYearFrom.SelectedValue;
            cmd.Parameters.Add("@fromMonth", SqlDbType.Int).Value = cmbMonthFrom.SelectedValue;
            cmd.Parameters.Add("@toYear", SqlDbType.Int).Value = cmbYearTo.SelectedValue;
            cmd.Parameters.Add("@toMonth", SqlDbType.Int).Value = cmbMonthTo.SelectedValue;
            cmd.Connection = sqlConnection1;

            reader = cmd.ExecuteReader();

            string excelName = "IncidentAndCauses.xls";
            var result = new List<IncidentAndCauses>();
            while (reader.Read())
            {
                result.Add(new IncidentAndCauses()
                {
                    CaseNo = Convert.ToString(reader["caseno"]),
                    DateIncidentOccurred = Convert.ToString(reader["date_incident_occured"]),
                    MaleCasualties = Convert.ToInt32(reader["MaleCasualties"]),
                    FemaleCasualties = Convert.ToInt32(reader["FemaleCasualties"]),
                    ChildCasualties = Convert.ToInt32(reader["ChildCasualties"]),
                    TotalCasualties = Convert.ToInt32(reader["casualties_total"]),
                    AssistanceDate = Convert.ToString(reader["AssistanceDate"]),
                    AssistanceTypeName = Convert.ToString(reader["AssistanceTypeName"]),
                    Beneficiaries = Convert.ToInt32(reader["ABeneficiaries"]),
                    AffectedHouseholds = Convert.ToInt32(reader["Anumber_affected_hh"]),
                    Province = Convert.ToString(reader["province_name"]),
                    District = Convert.ToString(reader["district_name"]),
                    Ward = Convert.ToString(reader["ward_no"]),
                    Incident = Convert.ToString(reader["incident_name"]),
                    NeedsFood = Convert.ToInt32(reader["Afoodneed"]),
                    NutritionNeeds = Convert.ToInt32(reader["Anutrition"]),
                    NeedsWater = Convert.ToInt32(reader["Awater"]),
                    SanitationNeeds = Convert.ToInt32(reader["Asanitation"]),
                    HygieneNeeds = Convert.ToInt32(reader["Ahygiene"]),
                    NeedsShelter = Convert.ToInt32(reader["Ashelter"]),
                    EducationNeeds = Convert.ToInt32(reader["Aeducation"]),
                    HealthNeeds = Convert.ToInt32(reader["Ahealth"]),
                    OthereNeeds = Convert.ToString(reader["checkgroup_other"]),
                    Cause = Convert.ToString(reader["cause_name"]),
                    DisplacementStatus = Convert.ToString(reader["displacement_status"])
                });
            }
            sqlConnection1.Close();
            CreateExcelFile.CreateExcelDocument(result, excelName, Response);
        }
    }

    public class IncidentAndCauses
    {
        public string CaseNo { get; set; }
        public int MaleCasualties { get; set; }
        public int FemaleCasualties { get; set; }
        public int ChildCasualties { get; set; }
        public int TotalCasualties { get; set; }
        public string AssistanceTypeName { get; set; }
        public int AffectedHouseholds { get; set; }
        public string DateIncidentOccurred { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string Ward { get; set; }
        public string Incident { get; set; }
        public int NeedsFood { get; set; }
        public int NutritionNeeds { get; set; }
        public int NeedsWater { get; set; }
        public int SanitationNeeds { get; set; }
        public int HygieneNeeds { get; set; }
        public int NeedsShelter { get; set; }
        public int EducationNeeds { get; set; }
        public int HealthNeeds { get; set; }
        public string OthereNeeds { get; set; }
        public string Cause { get; set; }
        public string DisplacementStatus { get; set; }
        public int Beneficiaries { get; set; }
        public string AssistanceDate { get; set; }
    }

    public class AmountOfFundsCommunity
    {
        public string DateIncidentOccurred { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string Llg { get; set; }
        public string Ward { get; set; }
        public string Placename { get; set; }
        public string Caseno { get; set; }
        public string AssistanceDate { get; set; }
        public string AssistanceTypeName { get; set; }
        public int Beneficiaries { get; set; }
        public string Comments { get; set; }
    }
}