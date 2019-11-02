using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web
{

    public partial class _Default : System.Web.UI.Page
    {
        int maxTotals = 13;
        int maxProvince = 0;
        bool isColumnCount = false;
        Decimal[] grdCasualtiesTotals = new Decimal[13];
        Decimal[] GridView3Totals = new Decimal[13];
        Decimal[] gridProvince;
        private long totalCasualties = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["user"] == null)
                Response.Redirect("Login.aspx");
            DBMangement dbMan = new DBMangement();
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
                cmbIncidentProvince.Items.Add("All");
                cmbIncidentProvince.Items[0].Value = "-1";
                for (int i = 1; i <= lstProvince.Count; i++)
                {
                    cmbIncidentProvince.Items.Add(lstProvince[i - 1].Name.ToString());
                    cmbIncidentProvince.Items[i].Value = lstProvince[i - 1].ID.ToString();
                }
            }
            List<DisplacementItem> lstDisplacement = dbMan.GetDisplacement();
            if (cmbDisplacement.Items.Count == 0)
            {
                cmbDisplacement.Items.Add("All");
                cmbDisplacement.Items[0].Value = "-1";
                cmbIncidentsDisplacement.Items.Add("All");
                cmbIncidentsDisplacement.Items[0].Value = "-1";
                cmbCasualtiesDisplacement.Items.Add("All");
                cmbCasualtiesDisplacement.Items[0].Value = "-1";
                for (int i = 1; i <= lstDisplacement.Count; i++)
                {
                    cmbDisplacement.Items.Add(lstDisplacement[i - 1].Name.ToString());
                    cmbDisplacement.Items[i].Value = lstDisplacement[i - 1].ID.ToString();

                    cmbIncidentsDisplacement.Items.Add(lstDisplacement[i - 1].Name.ToString());
                    cmbIncidentsDisplacement.Items[i].Value = lstDisplacement[i - 1].ID.ToString();

                    cmbCasualtiesDisplacement.Items.Add(lstDisplacement[i - 1].Name.ToString());
                    cmbCasualtiesDisplacement.Items[i].Value = lstDisplacement[i - 1].ID.ToString();
                }
            }
            List<int> lstYears = dbMan.GetReportedYears();
            if (cmbYear.Items.Count == 0)
            {
                cmbYear.Items.Add("All");
                cmbYear.Items[0].Value = "-1";

                cmbCasualtiesYear.Items.Add("All");
                cmbCasualtiesYear.Items[0].Value = "-1";

                cmbIncidentYear.Items.Add("All");
                cmbIncidentYear.Items[0].Value = "-1";
                for (int i = 1; i <= lstYears.Count; i++)
                {
                    cmbYear.Items.Add(lstYears[i - 1].ToString());
                    cmbYear.Items[i].Value = lstYears[i - 1].ToString();

                    cmbCasualtiesYear.Items.Add(lstYears[i - 1].ToString());
                    cmbCasualtiesYear.Items[i].Value = lstYears[i - 1].ToString();

                    cmbIncidentYear.Items.Add(lstYears[i - 1].ToString());
                    cmbIncidentYear.Items[i].Value = lstYears[i - 1].ToString();
                }
            }
            cmbCasualtiesMonth.Items.Add("All");
            cmbCasualtiesMonth.Items[0].Value = "-1";

            for (int i = 1; i <= 12; i++)
            {
                cmbCasualtiesMonth.Items.Add(i.ToString());
                cmbCasualtiesMonth.Items[i].Value = i.ToString();
            }

            grdCasualtiesByProvince.Columns.Clear();

            BoundField incidentField = new BoundField
            {
                DataField = "incident",
                HeaderText = "Incident"
            };
            grdCasualtiesByProvince.Columns.Add(incidentField);
            for (int i = 0; i < lstProvince.Count; i++)
            {
                ProvinceItem item = lstProvince[i];
                BoundField test = new BoundField
                {
                    DataField = "province_" + item.ID.ToString(),
                    HeaderText = item.Code
                };
                grdCasualtiesByProvince.Columns.Add(test);
            }

            //SqlDataSourceCasualties.EnableCaching = false;
            //grdCasualtiesByProvince.DataBind();
            //SqlDataSourceCasualties.EnableCaching = true;
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            if (txtFilter.Value == "")
                txtFilter.Value = " ";
            SqlDatabase.SelectParameters["filter"].DefaultValue = txtFilter.Value;
            SqlDatabase.DataBind();
        }

        //protected void grdIndividuals_RowCreated(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.Footer)
        //    {
        //        // Get the OrderTotalLabel Label control in the footer row.
        //        Label total = (Label)e.Row.FindControl("OrderTotalLabel");

        //        // Display the grand total of the order formatted as currency.
        //        if (total != null)
        //        {
        //            total.Text = Convert.ToString(orderTotal);
        //        }
        //    }
        //}

        protected void grdIndividuals_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Get the cell that contains the item total.
                TableCell cell = e.Row.Cells[6];
                // Get the DataBoundLiteralControl control that contains the 
                // data-bound value.
                DataBoundLiteralControl boundControl = (DataBoundLiteralControl)cell.Controls[0];

                // Remove the '$' character so that the type converter works properly.
                string itemTotal = Convert.ToString(boundControl.Text.Replace(@"\r\n", ""));

                // Add the total for an item (row) to the order total.
                totalCasualties += Convert.ToInt64(itemTotal);
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[6].Text = Convert.ToString(totalCasualties);
            }
        }

        protected void grdCasualtiesByMonth_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                //Initialize totals array
                for (int k = 1; k < maxTotals; k++)
                    grdCasualtiesTotals[k] = 0;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Collect totals from relevant cells
                DataRowView rowView = (DataRowView)e.Row.DataItem;
                for (int k = 1; k < maxTotals; k++)
                {
                    if (DBNull.Value.Equals(rowView[k + 1]))
                        grdCasualtiesTotals[k] += 0;
                    else
                        grdCasualtiesTotals[k] += Convert.ToDecimal(rowView[k + 1]);
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                for (int k = 1; k < maxTotals; k++)
                    e.Row.Cells[k].Text = Convert.ToString(grdCasualtiesTotals[k]);
            }
        }

        protected void GridView3_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                //Initialize totals array
                for (int k = 1; k < maxTotals; k++)
                    GridView3Totals[k] = 0;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Collect totals from relevant cells
                DataRowView rowView = (DataRowView)e.Row.DataItem;
                for (int k = 1; k < maxTotals; k++)
                {
                    if (DBNull.Value.Equals(rowView[k + 1]))
                        GridView3Totals[k] += 0;
                    else
                        GridView3Totals[k] += Convert.ToDecimal(rowView[k + 1]);
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                for (int k = 1; k < maxTotals; k++)
                    e.Row.Cells[k].Text = Convert.ToString(GridView3Totals[k]);
            }
        }

        protected void grdCasualtiesByProvince_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            maxProvince = e.Row.Cells.Count;
            if (!isColumnCount)
            {
                gridProvince = new decimal[maxProvince];
                isColumnCount = true;
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                //Initialize totals array
                for (int k = 1; k < maxProvince; k++)
                    gridProvince[k] = 0;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Collect totals from relevant cells
                DataRowView rowView = (DataRowView)e.Row.DataItem;
                for (int k = 1; k < maxProvince; k++)
                {
                    if (DBNull.Value.Equals(rowView[k + 1]))
                        gridProvince[k] += 0;
                    else
                        gridProvince[k] += Convert.ToDecimal(rowView[k + 1]);
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "Total";
                for (int k = 1; k < maxProvince; k++)
                    e.Row.Cells[k].Text = Convert.ToString(gridProvince[k]);
            }
        }
    }
}