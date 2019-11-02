using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Globalization;

namespace Web
{

    public class DBMangement
    {
        public string ConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["dcpConnectionString"].ConnectionString;
        }
        public List<OrganizationItem> GetOrganization()
        {
            return DBOrganization();
        }
        public List<ProvinceItem> GetProvince(int id = -1)
        {
            return DBProvince(id);
        }
        public List<DistrictItem> GetDistrict(int provinceId)
        {
            return DBDistrict(provinceId);
        }
        public List<LlgItem> GetLLg(int districtId)
        {
            return DBLLg(districtId);
        }
        public List<WardnoItem> GetWardno(int llgId)
        {
            return DBWardno(llgId);
        }
        public List<IncidentItem> GetIncident()
        {
            return DBIncident();
        }
        public List<DisplacementItem> GetDisplacement()
        {
            return DBDisplacement();
        }
        public List<UnitItem> GetUnits()
        {
            return DBUnits();
        }
        public List<CauseItem> GetCauses()
        {
            return DBCauses();
        }
        public List<CountryItem> GetCountries()
        {
            return DBCountries();
        }
        public List<CommunityItem> GetCommunity()
        {
            return DBCommunity();
        }
        public List<HouseholdItem> GetHousehold(int community_id)
        {
            return DBHousehold(community_id);
        }
        public List<IndividualItem> GetIndividualsById(int id)
        {
            return DBIndividualsById(id);
        }
        public List<LevelofEducationItem> GetLevelofEducation()
        {
            return DBLevelofEducation();
        }
        public List<MaritalItem> GetMaritalStatus()
        {
            return DBMaritalStatus();
        }
        public List<OccupationItem> GetOccupation()
        {
            return DBOccupation();
        }
        public List<RoleItem> GetRoles()
        {
            return DBRoles();
        }
        public List<VulnerabilityItem> GetVulnerability()
        {
            return DBVulnerability();
        }
        public List<int> GetReportedYears()
        {
            SqlConnection sqlConnection1 = new SqlConnection(this.ConnectionString());
            sqlConnection1.Open();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "GetReportedYears";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = sqlConnection1;


            reader = cmd.ExecuteReader();

            List<int> list = new List<int>();

            while (reader.Read())
            {
                list.Add(Convert.ToInt32(reader["Report_Year"]));
            }
            sqlConnection1.Close();

            return list;
        }
        public List<UserItem> GetValidUsers(string username, string password)
        {
            SqlConnection sqlConnection1 = new SqlConnection(this.ConnectionString());
            sqlConnection1.Open();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "GetValidUsers";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@username", SqlDbType.VarChar).Value = username;
            cmd.Parameters.Add("@password", SqlDbType.VarChar).Value = password;
            cmd.Connection = sqlConnection1;

            reader = cmd.ExecuteReader();

            List<UserItem> list = new List<UserItem>();

            while (reader.Read())
            {
                UserItem obj = new UserItem();
                if (FillUserInfoFromDataReader(obj, reader))
                    list.Add(obj);
            }
            sqlConnection1.Close();

            return list;
        }

        public List<UserItem> GetUserByID(int id)
        {
            SqlConnection sqlConnection1 = new SqlConnection(this.ConnectionString());
            sqlConnection1.Open();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "GetUsersByID";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
            cmd.Connection = sqlConnection1;

            reader = cmd.ExecuteReader();

            List<UserItem> list = new List<UserItem>();

            while (reader.Read())
            {
                UserItem obj = new UserItem();
                if (FillUserInfoFromDataReader(obj, reader))
                    list.Add(obj);
            }
            sqlConnection1.Close();

            return list;
        }
        public int InsertMainInformation(string mainid, string organizationId, string provinceId, string districtId, string llgId, string wardnoId, string placeName, string dateIncidentOccured, string dateIncidentReported, string caseno)
        {
            return DBInsertMainInformation(mainid, organizationId, provinceId, districtId, llgId, wardnoId, placeName, dateIncidentOccured, dateIncidentReported, caseno);
        }
        public MainInformation GetMainInformation(int mainId)
        {
            return DBMainInformation().FirstOrDefault(x => x.Id == mainId);
        }
        public DataRow GetAffectedPopulation(int mainId)
        {
            return DBAffectedPopulation(mainId).FirstOrDefault();
        }
        public DataRow GetDamagedInfrastructure(int mainId)
        {
            return DBDamagedInfrastructure(mainId).FirstOrDefault();
        }
        public DataRow GetEssentialServices(int mainId)
        {
            return DBEssentialServices(mainId).FirstOrDefault();
        }
        public DataRow GetCausesInfo(int mainId)
        {
            return DBCausesInfo(mainId).FirstOrDefault();
        }
        public List<AssistanceType> GetAssistanceType()
        {
            return DBAssistanceType();
        }
        public int InsertOrUpdateAssistance(int id, string Caseno, string AssistanceDate, int AssistanceType, int Beneficiaries, string Comments)
        {
            return DBInsertOrUpdateAssistance(id, Caseno, AssistanceDate, AssistanceType, Beneficiaries, Comments);
        }

        public DataRow GetPreviousLocation(int mainId)
        {
            return DBPreviousLocation(mainId).FirstOrDefault();
        }

        public DataRow GetAffectedGroup(int mainId)
        {
            return DBAffectedGroup(mainId).FirstOrDefault();
        }

        public DataTable GetAssistance(int mainId)
        {
            return DBAssistance(mainId);
        }

        public int InsertCommunity(string community_name)
        {
            return DBInsertCommunity(community_name);
        }
        public int InsertHousehold(int community_id, string household_name)
        {
            return DBInsertHousehold(community_id, household_name);
        }

        private List<OrganizationItem> DBOrganization()
        {
            SqlConnection sqlConnection1 = new SqlConnection(this.ConnectionString());
            sqlConnection1.Open();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "GetOrganization";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = sqlConnection1;


            reader = cmd.ExecuteReader();

            List<OrganizationItem> list = new List<OrganizationItem>();

            while (reader.Read())
            {
                OrganizationItem obj = new OrganizationItem();
                if (FillOrganizationInfoFromDataReader(obj, reader))
                    list.Add(obj);
            }
            sqlConnection1.Close();

            return list;
        }
        private List<ProvinceItem> DBProvince(int id)
        {
            SqlConnection sqlConnection1 = new SqlConnection(this.ConnectionString());
            sqlConnection1.Open();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "GetProvince";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
            cmd.Connection = sqlConnection1;


            reader = cmd.ExecuteReader();

            List<ProvinceItem> list = new List<ProvinceItem>();

            while (reader.Read())
            {
                ProvinceItem obj = new ProvinceItem();
                if (FillProvinceInfoFromDataReader(obj, reader))
                    list.Add(obj);
            }
            sqlConnection1.Close();

            return list;
        }
        private List<DistrictItem> DBDistrict(int provinceId)
        {
            SqlConnection sqlConnection1 = new SqlConnection(this.ConnectionString());
            sqlConnection1.Open();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "GetDistrictsInProvince";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@province_id", SqlDbType.Int).Value = provinceId;
            cmd.Connection = sqlConnection1;


            reader = cmd.ExecuteReader();

            List<DistrictItem> list = new List<DistrictItem>();

            while (reader.Read())
            {
                DistrictItem obj = new DistrictItem();
                if (FillDistrictInfoFromDataReader(obj, reader))
                    list.Add(obj);
            }
            sqlConnection1.Close();

            return list;
        }
        private List<LlgItem> DBLLg(int llgId)
        {
            SqlConnection sqlConnection1 = new SqlConnection(this.ConnectionString());
            sqlConnection1.Open();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "GetLlgsInDistrict";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@district_id", SqlDbType.Int).Value = llgId;
            cmd.Connection = sqlConnection1;


            reader = cmd.ExecuteReader();

            List<LlgItem> list = new List<LlgItem>();

            while (reader.Read())
            {
                LlgItem obj = new LlgItem();
                if (FillLLGInfoFromDataReader(obj, reader))
                    list.Add(obj);
            }
            sqlConnection1.Close();

            return list;
        }
        private List<WardnoItem> DBWardno(int llgId)
        {
            SqlConnection sqlConnection1 = new SqlConnection(this.ConnectionString());
            sqlConnection1.Open();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "GetWardnosInDistrict";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@llg_id", SqlDbType.Int).Value = llgId;
            cmd.Connection = sqlConnection1;


            reader = cmd.ExecuteReader();

            List<WardnoItem> list = new List<WardnoItem>();

            while (reader.Read())
            {
                WardnoItem obj = new WardnoItem();
                if (FillWardnoInfoFromDataReader(obj, reader))
                    list.Add(obj);
            }
            sqlConnection1.Close();

            return list;
        }
        private List<IncidentItem> DBIncident()
        {
            SqlConnection sqlConnection1 = new SqlConnection(this.ConnectionString());
            sqlConnection1.Open();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "GetIncident";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = sqlConnection1;


            reader = cmd.ExecuteReader();

            List<IncidentItem> list = new List<IncidentItem>();

            while (reader.Read())
            {
                IncidentItem obj = new IncidentItem();
                if (FillIncidentInfoFromDataReader(obj, reader))
                    list.Add(obj);
            }
            sqlConnection1.Close();

            return list;
        }
        private List<DisplacementItem> DBDisplacement()
        {
            SqlConnection sqlConnection1 = new SqlConnection(this.ConnectionString());
            sqlConnection1.Open();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "GetDisplacement";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = sqlConnection1;


            reader = cmd.ExecuteReader();

            List<DisplacementItem> list = new List<DisplacementItem>();

            while (reader.Read())
            {
                DisplacementItem obj = new DisplacementItem();
                if (FillDisplacementInfoFromDataReader(obj, reader))
                    list.Add(obj);
            }
            sqlConnection1.Close();

            return list;
        }
        private int DBInsertMainInformation(string main_id, string organization_id, string province_id, string district_id, string llg_id, string wardno_id, string placename, string incident_occured, string incident_reported, string caseno)
        {
            SqlConnection sqlConnection1 = new SqlConnection(this.ConnectionString());
            sqlConnection1.Open();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "InsertMainInformation";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@main_id", SqlDbType.Int).Value = main_id;
            cmd.Parameters.Add("@organization_id", SqlDbType.Int).Value = organization_id;
            cmd.Parameters.Add("@province_id", SqlDbType.Int).Value = province_id;
            cmd.Parameters.Add("@district_id", SqlDbType.Int).Value = district_id;
            cmd.Parameters.Add("@llg_id", SqlDbType.Int).Value = llg_id;
            cmd.Parameters.Add("@wardno_id", SqlDbType.Int).Value = wardno_id;
            cmd.Parameters.Add("@placename", SqlDbType.VarChar).Value = placename;
            var occured = DateTime.ParseExact(incident_occured, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            var reported = DateTime.ParseExact(incident_reported, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            cmd.Parameters.Add("@incident_occured", SqlDbType.Date).Value = occured;   // DateTime.ParseExact(incident_occured, "MM/dd/yyyy", null);
            cmd.Parameters.Add("@incident_reported", SqlDbType.Date).Value = reported; // DateTime.ParseExact(incident_reported, "MM/dd/yyyy", null);
            cmd.Parameters.Add("@caseno", SqlDbType.VarChar).Value = caseno;
            cmd.Connection = sqlConnection1;

            reader = cmd.ExecuteReader();
            sqlConnection1.Close();
            return main_id.Equals("0") ? GetMaxIDFromTable("main_information") : Convert.ToInt32(main_id);
        }
        private int DBInsertOrUpdateAssistance(int id, string Caseno, string AssistanceDate, int AssistanceType, int Beneficiaries, string Comments)
        {
            SqlConnection sqlConnection1 = new SqlConnection(this.ConnectionString());
            sqlConnection1.Open();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "InsertOrUpdateAssistance";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
            cmd.Parameters.Add("@Caseno", SqlDbType.VarChar).Value = Caseno;
            cmd.Parameters.Add("@AssistanceDate", SqlDbType.DateTime).Value = AssistanceDate;
            cmd.Parameters.Add("@AssistanceType", SqlDbType.Int).Value = AssistanceType;
            cmd.Parameters.Add("@Beneficiaries", SqlDbType.Int).Value = Beneficiaries;
            cmd.Parameters.Add("@Comments", SqlDbType.VarChar).Value = Comments;
            cmd.Connection = sqlConnection1;

            reader = cmd.ExecuteReader();
            sqlConnection1.Close();

            return GetMaxIDFromTable("main_information");
        }

        private int DBInsertCommunity(string community_name)
        {
            SqlConnection sqlConnection1 = new SqlConnection(this.ConnectionString());
            sqlConnection1.Open();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "InsertCommunity";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@community_name", SqlDbType.VarChar).Value = community_name;
            cmd.Connection = sqlConnection1;

            reader = cmd.ExecuteReader();
            sqlConnection1.Close();

            return GetMaxIDFromTable("community");
        }
        private int DBInsertHousehold(int community_id, string household_name)
        {
            SqlConnection sqlConnection1 = new SqlConnection(this.ConnectionString());
            sqlConnection1.Open();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "InsertHousehold";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@community_id", SqlDbType.Int).Value = community_id;
            cmd.Parameters.Add("@household_name", SqlDbType.VarChar).Value = household_name;
            cmd.Connection = sqlConnection1;

            reader = cmd.ExecuteReader();
            sqlConnection1.Close();

            return GetMaxIDFromTable("household");
        }
        private List<UnitItem> DBUnits()
        {
            SqlConnection sqlConnection1 = new SqlConnection(this.ConnectionString());
            sqlConnection1.Open();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "GetUnits";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = sqlConnection1;


            reader = cmd.ExecuteReader();

            List<UnitItem> list = new List<UnitItem>();

            while (reader.Read())
            {
                UnitItem obj = new UnitItem();
                if (FillUnitInfoFromDataReader(obj, reader))
                    list.Add(obj);
            }
            sqlConnection1.Close();

            return list;
        }
        private List<CauseItem> DBCauses()
        {
            SqlConnection sqlConnection1 = new SqlConnection(this.ConnectionString());
            sqlConnection1.Open();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "GetCauses";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = sqlConnection1;


            reader = cmd.ExecuteReader();

            List<CauseItem> list = new List<CauseItem>();

            while (reader.Read())
            {
                CauseItem obj = new CauseItem();
                if (FillCauseInfoFromDataReader(obj, reader))
                    list.Add(obj);
            }
            sqlConnection1.Close();

            return list;
        }
        private List<CountryItem> DBCountries()
        {
            SqlConnection sqlConnection1 = new SqlConnection(this.ConnectionString());
            sqlConnection1.Open();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "GetCountries";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = sqlConnection1;


            reader = cmd.ExecuteReader();

            List<CountryItem> list = new List<CountryItem>();

            while (reader.Read())
            {
                CountryItem obj = new CountryItem();
                if (FillCountryInfoFromDataReader(obj, reader))
                    list.Add(obj);
            }
            sqlConnection1.Close();

            return list;
        }
        private List<CommunityItem> DBCommunity()
        {
            SqlConnection sqlConnection1 = new SqlConnection(this.ConnectionString());
            sqlConnection1.Open();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "GetCommunity";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = sqlConnection1;


            reader = cmd.ExecuteReader();

            List<CommunityItem> list = new List<CommunityItem>();

            while (reader.Read())
            {
                CommunityItem obj = new CommunityItem();
                if (FillCommunityInfoFromDataReader(obj, reader))
                    list.Add(obj);
            }
            sqlConnection1.Close();

            return list;
        }
        private List<LevelofEducationItem> DBLevelofEducation()
        {
            SqlConnection sqlConnection1 = new SqlConnection(this.ConnectionString());
            sqlConnection1.Open();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "GetLevelofEducation";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = sqlConnection1;


            reader = cmd.ExecuteReader();

            List<LevelofEducationItem> list = new List<LevelofEducationItem>();

            while (reader.Read())
            {
                LevelofEducationItem obj = new LevelofEducationItem();
                if (FillEducationInfoFromDataReader(obj, reader))
                    list.Add(obj);
            }
            sqlConnection1.Close();

            return list;
        }
        private List<MaritalItem> DBMaritalStatus()
        {
            SqlConnection sqlConnection1 = new SqlConnection(this.ConnectionString());
            sqlConnection1.Open();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "GetMaritalStatus";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = sqlConnection1;


            reader = cmd.ExecuteReader();

            List<MaritalItem> list = new List<MaritalItem>();

            while (reader.Read())
            {
                MaritalItem obj = new MaritalItem();
                if (FillMaritalInfoFromDataReader(obj, reader))
                    list.Add(obj);
            }
            sqlConnection1.Close();

            return list;
        }
        private List<OccupationItem> DBOccupation()
        {
            SqlConnection sqlConnection1 = new SqlConnection(this.ConnectionString());
            sqlConnection1.Open();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "GetOccupation";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = sqlConnection1;


            reader = cmd.ExecuteReader();

            List<OccupationItem> list = new List<OccupationItem>();

            while (reader.Read())
            {
                OccupationItem obj = new OccupationItem();
                if (FillOccupationInfoFromDataReader(obj, reader))
                    list.Add(obj);
            }
            sqlConnection1.Close();

            return list;
        }
        private List<RoleItem> DBRoles()
        {
            SqlConnection sqlConnection1 = new SqlConnection(this.ConnectionString());
            sqlConnection1.Open();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "GetRoles";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = sqlConnection1;


            reader = cmd.ExecuteReader();

            List<RoleItem> list = new List<RoleItem>();

            while (reader.Read())
            {
                RoleItem obj = new RoleItem();
                if (FillRolesInfoFromDataReader(obj, reader))
                    list.Add(obj);
            }
            sqlConnection1.Close();

            return list;
        }
        private List<MainInformation> DBMainInformation()
        {
            SqlConnection sqlConnection1 = new SqlConnection(this.ConnectionString());
            sqlConnection1.Open();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "GetMainInformation";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = sqlConnection1;


            reader = cmd.ExecuteReader();

            List<MainInformation> list = new List<MainInformation>();

            while (reader.Read())
            {
                MainInformation obj = new MainInformation();
                if (FillMainInformationFromDataReader(obj, reader))
                    list.Add(obj);
            }
            sqlConnection1.Close();

            return list;
        }
        private List<DataRow> DBAffectedPopulation(int id)
        {
            SqlConnection sqlConnection1 = new SqlConnection(this.ConnectionString());
            sqlConnection1.Open();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "GetAffectedPopulation";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = sqlConnection1;
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            reader = cmd.ExecuteReader();

            var dt = new DataTable();
            dt.Load(reader);
            List<DataRow> drList = dt.AsEnumerable().ToList();

            sqlConnection1.Close();

            return drList;
        }
        private List<DataRow> DBDamagedInfrastructure(int id)
        {
            SqlConnection sqlConnection1 = new SqlConnection(this.ConnectionString());
            sqlConnection1.Open();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "GetDamagedInfrastructure";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = sqlConnection1;
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            reader = cmd.ExecuteReader();

            var dt = new DataTable();
            dt.Load(reader);
            List<DataRow> drList = dt.AsEnumerable().ToList();

            sqlConnection1.Close();

            return drList;
        }
        private List<DataRow> DBEssentialServices(int id)
        {
            SqlConnection sqlConnection1 = new SqlConnection(this.ConnectionString());
            sqlConnection1.Open();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "GetEssentialServices";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = sqlConnection1;
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            reader = cmd.ExecuteReader();

            var dt = new DataTable();
            dt.Load(reader);
            List<DataRow> drList = dt.AsEnumerable().ToList();

            sqlConnection1.Close();

            return drList;
        }
        private List<DataRow> DBCausesInfo(int id)
        {
            SqlConnection sqlConnection1 = new SqlConnection(this.ConnectionString());
            sqlConnection1.Open();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "GetCausesInfo";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = sqlConnection1;
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            reader = cmd.ExecuteReader();

            var dt = new DataTable();
            dt.Load(reader);
            List<DataRow> drList = dt.AsEnumerable().ToList();

            sqlConnection1.Close();

            return drList;
        }
        private List<DataRow> DBPreviousLocation(int id)
        {
            SqlConnection sqlConnection1 = new SqlConnection(this.ConnectionString());
            sqlConnection1.Open();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "GetPreviousLocation";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = sqlConnection1;
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            reader = cmd.ExecuteReader();

            var dt = new DataTable();
            dt.Load(reader);
            List<DataRow> drList = dt.AsEnumerable().ToList();

            sqlConnection1.Close();

            return drList;
        }
        private List<DataRow> DBAffectedGroup(int id)
        {
            SqlConnection sqlConnection1 = new SqlConnection(this.ConnectionString());
            sqlConnection1.Open();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "GetAffectedGroup";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = sqlConnection1;
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            reader = cmd.ExecuteReader();

            var dt = new DataTable();
            dt.Load(reader);
            List<DataRow> drList = dt.AsEnumerable().ToList();

            sqlConnection1.Close();

            return drList;
        }
        private DataTable DBAssistance(int id)
        {
            SqlConnection sqlConnection1 = new SqlConnection(this.ConnectionString());
            sqlConnection1.Open();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "GetAssistance";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = sqlConnection1;
            cmd.Parameters.Add("@MainId", SqlDbType.Int).Value = id;

            reader = cmd.ExecuteReader();

            var dt = new DataTable();
            dt.Load(reader);
            List<DataRow> drList = dt.AsEnumerable().ToList();

            sqlConnection1.Close();

            return dt;
        }
        private List<VulnerabilityItem> DBVulnerability()
        {
            SqlConnection sqlConnection1 = new SqlConnection(this.ConnectionString());
            sqlConnection1.Open();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "GetVulnerability";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = sqlConnection1;


            reader = cmd.ExecuteReader();

            List<VulnerabilityItem> list = new List<VulnerabilityItem>();

            while (reader.Read())
            {
                VulnerabilityItem obj = new VulnerabilityItem();
                if (FillVulnerabilityInfoFromDataReader(obj, reader))
                    list.Add(obj);
            }
            sqlConnection1.Close();

            return list;
        }
        private List<HouseholdItem> DBHousehold(int community_id)
        {
            SqlConnection sqlConnection1 = new SqlConnection(this.ConnectionString());
            sqlConnection1.Open();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "GetHousehold";
            cmd.Parameters.Add("@community_id", SqlDbType.Int).Value = community_id;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = sqlConnection1;


            reader = cmd.ExecuteReader();

            List<HouseholdItem> list = new List<HouseholdItem>();

            while (reader.Read())
            {
                HouseholdItem obj = new HouseholdItem();
                if (FillHouseholdInfoFromDataReader(obj, reader))
                    list.Add(obj);
            }
            sqlConnection1.Close();

            return list;
        }
        private List<IndividualItem> DBIndividualsById(int id)
        {
            SqlConnection sqlConnection1 = new SqlConnection(this.ConnectionString());
            sqlConnection1.Open();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "GetIndividualsByID";
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = sqlConnection1;


            reader = cmd.ExecuteReader();

            List<IndividualItem> list = new List<IndividualItem>();

            while (reader.Read())
            {
                IndividualItem obj = new IndividualItem();
                if (FillIndividualsInfoFromDataReader(obj, reader))
                    list.Add(obj);
            }
            sqlConnection1.Close();

            return list;
        }
        private List<AssistanceType> DBAssistanceType()
        {
            SqlConnection sqlConnection1 = new SqlConnection(this.ConnectionString());
            sqlConnection1.Open();
            SqlCommand cmd = new SqlCommand();
            SqlDataReader reader;

            cmd.CommandText = "GetAssistanceType";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = sqlConnection1;


            reader = cmd.ExecuteReader();

            List<AssistanceType> list = new List<AssistanceType>();

            while (reader.Read())
            {
                AssistanceType obj = new AssistanceType();
                if (FillAssistanceTypeDataReader(obj, reader))
                    list.Add(obj);
            }
            sqlConnection1.Close();

            return list;
        }

        public int GetMaxIDFromTable(string tableName)
        {
            SqlConnection sqlConnection = new SqlConnection(this.ConnectionString());
            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "SELECT MAX([id]) FROM " + tableName;
            cmd.Connection = sqlConnection;

            object value = cmd.ExecuteScalar();
            int id = 0;
            try
            {
                id = Convert.ToInt32(value);
            }
            catch (InvalidCastException e)
            {
                id = 0;
            }

            sqlConnection.Close();
            return id;
        }

        public void DeleteItem(string tableName, string id)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString());
            string queryString = "DELETE FROM " + tableName + " WHERE id='" + id + "'";
            SqlCommand command = new SqlCommand(queryString, connection);
            connection.Open();
            command.ExecuteScalar();
        }
        public void UpdateCommunity(int id, string community_name)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString());
            string queryString = "UPDATE community SET [community_name]='" + community_name + "' WHERE id='" + id.ToString() + "'";
            SqlCommand command = new SqlCommand(queryString, connection);
            connection.Open();
            command.ExecuteScalar();
        }
        public void UpdateHousehold(int id, string household_name)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString());
            string queryString = "UPDATE household SET [household_name]='" + household_name + "' WHERE id='" + id.ToString() + "'";
            SqlCommand command = new SqlCommand(queryString, connection);
            connection.Open();
            command.ExecuteScalar();
        }

        public void InsertIntoTable(string tableName, List<string> fieldList, List<string> values)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString());
            string queryString = "INSERT INTO " + tableName + "(";
            for (int i = 0; i < fieldList.Count(); i++)
            {
                if (i == 0)
                    queryString += "[" + fieldList[i] + "]";
                else
                    queryString += "," + "[" + fieldList[i] + "]";
            }
            queryString += ") values (";
            for (int i = 0; i < values.Count(); i++)
            {
                if (i == 0)
                    queryString += "'" + values[i] + "'";
                else
                    queryString += ", '" + values[i] + "'";
            }
            queryString += ");";
            SqlCommand command = new SqlCommand(queryString, connection);
            connection.Open();
            command.ExecuteScalar();
        }

        public void UpdateTable(string tableName, string id, List<string> fieldList, List<string> values)
        {
            SqlConnection connection = new SqlConnection(this.ConnectionString());
            string queryString = "UPDATE " + tableName + " SET ";
            for (int i = 0; i < fieldList.Count(); i++)
            {
                if (i == 0)
                    queryString += "[" + fieldList[i] + "]='" + values[i] + "'";
                else
                    queryString += "," + "[" + fieldList[i] + "]='" + values[i] + "'";
            }
            queryString += " WHERE id='" + id + "'";
            SqlCommand command = new SqlCommand(queryString, connection);
            connection.Open();
            command.ExecuteScalar();
        }
        private static bool FillOrganizationInfoFromDataReader(OrganizationItem obj, IDataRecord dr)
        {
            obj.ID = Convert.ToInt32(dr["id"]);
            obj.Name = Convert.ToString(dr["organ_name"]);

            return true;
        }
        private static bool FillProvinceInfoFromDataReader(ProvinceItem obj, IDataRecord dr)
        {
            obj.ID = Convert.ToInt32(dr["id"]);
            obj.Name = Convert.ToString(dr["province_name"]);
            obj.Code = Convert.ToString(dr["province_code"]);

            return true;
        }
        private static bool FillDistrictInfoFromDataReader(DistrictItem obj, IDataRecord dr)
        {
            obj.ID = Convert.ToInt32(dr["id"]);
            obj.ProvinceID = Convert.ToInt32(dr["province_id"]);
            obj.Name = Convert.ToString(dr["district_name"]);

            return true;
        }
        private static bool FillWardnoInfoFromDataReader(WardnoItem obj, IDataRecord dr)
        {
            obj.ID = Convert.ToInt32(dr["id"]);
            obj.LlgID = Convert.ToInt32(dr["llg_id"]);
            obj.Name = Convert.ToString(dr["ward_no"]);

            return true;
        }
        private static bool FillLLGInfoFromDataReader(LlgItem obj, IDataRecord dr)
        {
            obj.ID = Convert.ToInt32(dr["id"]);
            obj.DistrictID = Convert.ToInt32(dr["district_id"]);
            obj.Name = Convert.ToString(dr["llg_name"]);

            return true;
        }
        private static bool FillMainInformationFromDataReader(MainInformation obj, IDataRecord dr)
        {
            obj.Id = Convert.ToInt32(dr["id"]);
            obj.OrganizationId = Convert.ToString(dr["organization_id"]);
            obj.ProvinceId = Convert.ToString(dr["province_id"]);
            obj.DistrictId = Convert.ToString(dr["district_id"]);
            obj.LlgId = Convert.ToString(dr["llg_id"]);
            obj.WardnoId = Convert.ToString(dr["wardno_id"]);
            obj.PlaceName = Convert.ToString(dr["placename"]);
            obj.IncidentOccured = Convert.ToDateTime(dr["date_incident_occured"]);
            obj.IncidentReported = Convert.ToDateTime(dr["date_incident_reported"]);
            obj.CaseNo = Convert.ToString(dr["caseno"]);

            return true;
        }
        private static bool FillIncidentInfoFromDataReader(IncidentItem obj, IDataRecord dr)
        {
            obj.ID = Convert.ToInt32(dr["id"]);
            obj.Name = Convert.ToString(dr["incident"]);

            return true;
        }
        private static bool FillDisplacementInfoFromDataReader(DisplacementItem obj, IDataRecord dr)
        {
            obj.ID = Convert.ToInt32(dr["id"]);
            obj.Name = Convert.ToString(dr["displacement"]);

            return true;
        }
        private static bool FillUnitInfoFromDataReader(UnitItem obj, IDataRecord dr)
        {
            obj.ID = Convert.ToInt32(dr["id"]);
            obj.Name = Convert.ToString(dr["unit_name"]);

            return true;
        }
        private static bool FillCauseInfoFromDataReader(CauseItem obj, IDataRecord dr)
        {
            obj.ID = Convert.ToInt32(dr["id"]);
            obj.Name = Convert.ToString(dr["cause_name"]);

            return true;
        }
        private static bool FillCountryInfoFromDataReader(CountryItem obj, IDataRecord dr)
        {
            obj.ID = Convert.ToInt32(dr["id"]);
            obj.Name = Convert.ToString(dr["country_name"]);

            return true;
        }
        private static bool FillCommunityInfoFromDataReader(CommunityItem obj, IDataRecord dr)
        {
            obj.ID = Convert.ToInt32(dr["id"]);
            obj.Name = Convert.ToString(dr["community_name"]);

            return true;
        }
        private static bool FillHouseholdInfoFromDataReader(HouseholdItem obj, IDataRecord dr)
        {
            obj.ID = Convert.ToInt32(dr["id"]);
            obj.CommunityID = Convert.ToInt32(dr["community_id"]);
            obj.Name = Convert.ToString(dr["household_name"]);

            return true;
        }
        private static bool FillEducationInfoFromDataReader(LevelofEducationItem obj, IDataRecord dr)
        {
            obj.ID = Convert.ToInt32(dr["id"]);
            obj.Name = Convert.ToString(dr["education_name"]);

            return true;
        }
        private static bool FillMaritalInfoFromDataReader(MaritalItem obj, IDataRecord dr)
        {
            obj.ID = Convert.ToInt32(dr["id"]);
            obj.Name = Convert.ToString(dr["marital_name"]);

            return true;
        }
        private static bool FillOccupationInfoFromDataReader(OccupationItem obj, IDataRecord dr)
        {
            obj.ID = Convert.ToInt32(dr["id"]);
            obj.Name = Convert.ToString(dr["occupation_name"]);

            return true;
        }
        private static bool FillRolesInfoFromDataReader(RoleItem obj, IDataRecord dr)
        {
            obj.ID = Convert.ToInt32(dr["id"]);
            obj.Name = Convert.ToString(dr["role_name"]);

            return true;
        }
        private static bool FillVulnerabilityInfoFromDataReader(VulnerabilityItem obj, IDataRecord dr)
        {
            obj.ID = Convert.ToInt32(dr["id"]);
            obj.Name = Convert.ToString(dr["vulnerability_name"]);

            return true;
        }
        private static bool FillUserInfoFromDataReader(UserItem obj, IDataRecord dr)
        {
            obj.ID = Convert.ToInt32(dr["id"]);
            obj.FirstName = Convert.ToString(dr["first_name"]);
            obj.LastName = Convert.ToString(dr["last_name"]);
            obj.UserName = Convert.ToString(dr["user_name"]);
            obj.UserGroup = Convert.ToInt32(dr["user_group"]);
            obj.UserOrganization = Convert.ToInt32(dr["user_org"]);
            obj.Email = Convert.ToString(dr["email"]);
            obj.NotifyNewCase = Convert.ToInt32(dr["notify_new_case"]);
            obj.Enabled = Convert.ToInt32(dr["enabled"]);
            obj.Locked = Convert.ToInt32(dr["locked"]);
            obj.Password = Convert.ToString(dr["password"]);

            return true;
        }
        private static bool FillIndividualsInfoFromDataReader(IndividualItem obj, IDataRecord dr)
        {
            obj.ID = Convert.ToInt32(dr["id"]);
            obj.HouseholdID = Convert.ToInt32(dr["household_id"]);
            obj.FirstName = Convert.ToString(dr["first_name"]);
            obj.MiddleName = Convert.ToString(dr["middle_name"]);
            obj.LastName = Convert.ToString(dr["last_name"]);
            obj.Gender = Convert.ToInt32(dr["gender"]);
            obj.DOB = Convert.ToString(dr["dob"]);
            obj.Age = Convert.ToInt32(dr["age"]);
            obj.PlaceOfBirth = Convert.ToString(dr["place_of_birth"]);
            obj.Nationality = Convert.ToString(dr["nationality"]);
            obj.NationalID = Convert.ToString(dr["national_id"]);
            obj.ContactPhone = Convert.ToString(dr["contact_phone"]);
            obj.PhysicalAddress = Convert.ToString(dr["physical_address"]);
            obj.Email = Convert.ToString(dr["email"]);
            obj.MaritalStatus = Convert.ToInt32(dr["marital_status"]);
            obj.Role = Convert.ToInt32(dr["role"]);
            obj.IsBreadWinner = Convert.ToInt32(dr["is_bread_winner"]);
            obj.LevelOfEducation = dr["level_of_education"].ToString() != "" ? Convert.ToInt32(dr["level_of_education"]) : 0;
            obj.Occupation = Convert.ToInt32(dr["occupation"]);
            obj.Employer = Convert.ToString(dr["employer"]);
            obj.Comment = Convert.ToString(dr["comment"]);
            obj.DisplacementStatus = Convert.ToInt32(dr["displacement_status"]);
            obj.VulnerabilityLevel = Convert.ToInt32(dr["vulnerability_level"]);
            obj.CountyOfBirth = Convert.ToInt32(dr["country_of_birth"]);

            return true;
        }
        private static bool FillAssistanceTypeDataReader(AssistanceType obj, IDataRecord dr)
        {
            obj.Id = Convert.ToInt32(dr["id"]);
            obj.AssistanceTypeName = Convert.ToString(dr["AssistanceTypeName"]);
            return true;
        }
    }
}