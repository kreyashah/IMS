using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web
{
    public partial class Contact : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user"] == null)
                Response.Redirect("Login.aspx");
        }

        [System.Web.Services.WebMethod(EnableSession = true)]
        public static string GetChartData()
        {
            string chartData = string.Empty;
            DBMangement dbMan = new DBMangement();
            SqlConnection sqlConnection = new SqlConnection(dbMan.ConnectionString());
            sqlConnection.Open();
            SqlCommand cmd = new SqlCommand
            {
                CommandText = "chartNoIndividualsByHazard",
                CommandType = CommandType.StoredProcedure,
                Connection = sqlConnection
            };
            AllChart allChart = new AllChart();
            DataTable t1 = new DataTable();
            using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd))
            {
                sqlDataAdapter.Fill(t1);
            }
            allChart.Hazards = ConvertDataTable<ChartNoIndividualsByHazard>(t1);
            cmd.CommandText = "chartNoIndividualsByProvince";
            t1 = new DataTable();
            using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd))
            {
                sqlDataAdapter.Fill(t1);
            }
            allChart.ByProvinces = ConvertDataTable<ChartNoIndividualsByProvince>(t1);
            cmd.CommandText = "chartIDPsByIncidentAndProvince";
            t1 = new DataTable();
            using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd))
            {
                sqlDataAdapter.Fill(t1);
            }
            var chart3Data = ConvertDataTable<ChartIdPsByIncidentAndProvinceData>(t1);

            var groupChart = chart3Data.GroupBy(x => x.ProvinceName)
                .Select(x => new GroupChartList
                {
                    Name = x.FirstOrDefault()?.ProvinceName,
                    DataList = x.Select(y => y).OrderBy(z => z.Incident).ToList()
                }).ToList();

            var groupChartData = chart3Data.GroupBy(x => x.Incident)
                .Select(x =>
                new GroupChartData
                {
                    label = x.Key,
                    backgroundColor = x.FirstOrDefault(y => y.Incident == x.Key)?.Color,
                    borderWidth = 1,
                    data = groupChart.Select(y =>
                    {
                        return y.DataList.FirstOrDefault(z => z.Incident == x.Key)?.Population ?? 0;
                    }).ToList()
                }).ToList();
            allChart.IncidentAndProvince = new ChartIdPsByIncidentAndProvince
            {
                groupCharts = groupChart,
                groupChartDatas = groupChartData,
            };
            cmd.CommandText = "piePercentageIDPsByProvince";
            t1 = new DataTable();
            using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(cmd))
            {
                sqlDataAdapter.Fill(t1);
            }
            allChart.PercentageIdPsByProvinces = ConvertDataTable<ChartPercentageIdPsByProvince>(t1);
            sqlConnection.Close();
            chartData = JsonConvert.SerializeObject(allChart);
            return chartData;
        }

        private static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }
        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }
    }
    public class AllChart
    {
        public List<ChartNoIndividualsByHazard> Hazards { get; set; }
        public List<ChartNoIndividualsByProvince> ByProvinces { get; set; }
        public ChartIdPsByIncidentAndProvince IncidentAndProvince { get; set; }
        public List<ChartPercentageIdPsByProvince> PercentageIdPsByProvinces { get; set; }
    }

    public class ChartNoIndividualsByHazard
    {
        public string Incident { get; set; }
        public int Population { get; set; }
    }

    public class ChartNoIndividualsByProvince
    {
        public string ProvinceName { get; set; }
        public int Population { get; set; }
    }

    public class ChartIdPsByIncidentAndProvinceData
    {
        public int Id { get; set; }
        public int PId { get; set; }
        public string ProvinceName { get; set; }
        public int IId { get; set; }
        public string Incident { get; set; }
        public string Color { get; set; }
        public int Population { get; set; }
    }

    public class ChartIdPsByIncidentAndProvince
    {
        public List<GroupChartList> groupCharts { get; set; }
        public List<GroupChartData> groupChartDatas { get; set; }
    }

    public class GroupChartList
    {
        public string Name { get; set; }
        public List<ChartIdPsByIncidentAndProvinceData> DataList { get; set; }
    }

    public class ChartPercentageIdPsByProvince
    {
        public string ProvinceName { get; set; }
        public decimal Population { get; set; }
        public string Color { get; set; }
    }

    public class GroupChartData
    {
        public string label { get; set; }
        public int borderWidth { get; set; }
        public string backgroundColor { get; set; }
        public List<int> data { get; set; }
    }
}