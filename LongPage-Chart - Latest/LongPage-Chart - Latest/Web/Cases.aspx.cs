using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web
{
    public partial class CasesManagement : Page
    {
        DBMangement dbMan = new DBMangement();
        protected override void Render(HtmlTextWriter output)
        {
            Page.ClientScript.RegisterForEventValidation(grdIndividuals.UniqueID);
            foreach (GridViewRow i in grdIndividuals.Rows)
            {
                Page.ClientScript.RegisterForEventValidation(i.UniqueID);
            }
            base.Render(output);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                if (Session["user"] == null)
                    Response.Redirect("Login.aspx");

                if (ddlAssistanceType.Items.Count == 0)
                {
                    ddlAssistanceType.Items.Add("UnSpecified");
                    ddlAssistanceType.Items[0].Value = "-1";
                    foreach (var item in dbMan.GetAssistanceType())
                    {
                        ddlAssistanceType.Items.Add(item.AssistanceTypeName);
                        ddlAssistanceType.Items[ddlAssistanceType.Items.Count - 1].Value = item.Id.ToString();
                    }
                }
                if (PopddlAssistanceType.Items.Count == 0)
                {
                    PopddlAssistanceType.Items.Add("UnSpecified");
                    PopddlAssistanceType.Items[0].Value = "-1";
                    foreach (var item in dbMan.GetAssistanceType())
                    {
                        PopddlAssistanceType.Items.Add(item.AssistanceTypeName);
                        PopddlAssistanceType.Items[PopddlAssistanceType.Items.Count - 1].Value = item.Id.ToString();
                    }
                }

                List<OrganizationItem> lstOrganization = dbMan.GetOrganization();
                if (sltOrgan.Items.Count == 0)
                {
                    sltOrgan.Items.Add("UnSpecified");
                    sltOrgan.Items[0].Value = "-1";
                    for (int i = 1; i <= lstOrganization.Count; i++)
                    {
                        sltOrgan.Items.Add(lstOrganization[i - 1].Name.ToString());
                        sltOrgan.Items[i].Value = lstOrganization[i - 1].ID.ToString();
                    }
                }
                List<ProvinceItem> lstProvince = dbMan.GetProvince();
                if (sltProvince.Items.Count == 0)
                {
                    sltProvince.Items.Add("UnSpecified");
                    sltProvince.Items[0].Value = "-1";
                    for (int i = 1; i <= lstProvince.Count; i++)
                    {
                        sltProvince.Items.Add(lstProvince[i - 1].Name.ToString());
                        sltProvince.Items[i].Value = lstProvince[i - 1].ID.ToString();
                    }
                    cmbPreProvince.Items.Add("UnSpecified");
                    cmbPreProvince.Items[0].Value = "-1";
                    for (int i = 1; i <= lstProvince.Count; i++)
                    {
                        cmbPreProvince.Items.Add(lstProvince[i - 1].Name.ToString());
                        cmbPreProvince.Items[i].Value = lstProvince[i - 1].ID.ToString();
                    }
                }
                if (sltDistrict.Items.Count == 0)
                {
                    sltDistrict.Items.Add("UnSpecified");
                    sltDistrict.Items[0].Value = "-1";
                }
                if (sltLLG.Items.Count == 0)
                {
                    sltLLG.Items.Add("UnSpecified");
                    sltLLG.Items[0].Value = "-1";
                }
                if (sltWardno.Items.Count == 0)
                {
                    sltWardno.Items.Add("UnSpecified");
                    sltWardno.Items[0].Value = "-1";
                }
                List<IncidentItem> lstIncident = dbMan.GetIncident();
                if (sltTypeIncident.Items.Count == 0)
                {
                    sltTypeIncident.Items.Add("UnSpecified");
                    sltTypeIncident.Items[0].Value = "-1";
                    for (int i = 1; i <= lstIncident.Count; i++)
                    {
                        sltTypeIncident.Items.Add(lstIncident[i - 1].Name.ToString());
                        sltTypeIncident.Items[i].Value = lstIncident[i - 1].ID.ToString();
                    }
                }
                List<DisplacementItem> lstDisplacement = dbMan.GetDisplacement();
                if (sltDisplacementStatus.Items.Count == 0)
                {
                    sltDisplacementStatus.Items.Add("UnSpecified");
                    sltDisplacementStatus.Items[0].Value = "-1";
                    cmbDisplacementIndi.Items.Add("UnSpecified");
                    cmbDisplacementIndi.Items[0].Value = "-1";
                    for (int i = 1; i <= lstDisplacement.Count; i++)
                    {
                        sltDisplacementStatus.Items.Add(lstDisplacement[i - 1].Name.ToString());
                        sltDisplacementStatus.Items[i].Value = lstDisplacement[i - 1].ID.ToString();

                        cmbDisplacementIndi.Items.Add(lstDisplacement[i - 1].Name.ToString());
                        cmbDisplacementIndi.Items[i].Value = lstDisplacement[i - 1].ID.ToString();
                    }
                }
                List<UnitItem> lstUnit = dbMan.GetUnits();
                if (cmbRHUnit.Items.Count == 0)
                {
                    cmbRHUnit.Items.Add("UnSpecified");
                    cmbRHUnit.Items[0].Value = "-1";
                    for (int i = 1; i <= lstUnit.Count; i++)
                    {
                        cmbRHUnit.Items.Add(lstUnit[i - 1].Name.ToString());
                        cmbRHUnit.Items[i].Value = lstUnit[i - 1].ID.ToString();
                    }
                    cmbSSUnit.Items.Add("UnSpecified");
                    cmbSSUnit.Items[0].Value = "-1";
                    for (int i = 1; i <= lstUnit.Count; i++)
                    {
                        cmbSSUnit.Items.Add(lstUnit[i - 1].Name.ToString());
                        cmbSSUnit.Items[i].Value = lstUnit[i - 1].ID.ToString();
                    }
                    cmbEPLUnit.Items.Add("UnSpecified");
                    cmbEPLUnit.Items[0].Value = "-1";
                    for (int i = 1; i <= lstUnit.Count; i++)
                    {
                        cmbEPLUnit.Items.Add(lstUnit[i - 1].Name.ToString());
                        cmbEPLUnit.Items[i].Value = lstUnit[i - 1].ID.ToString();
                    }
                    cmbEPSUnit.Items.Add("UnSpecified");
                    cmbEPSUnit.Items[0].Value = "-1";
                    for (int i = 1; i <= lstUnit.Count; i++)
                    {
                        cmbEPSUnit.Items.Add(lstUnit[i - 1].Name.ToString());
                        cmbEPSUnit.Items[i].Value = lstUnit[i - 1].ID.ToString();
                    }
                    cmbNGPPUnit.Items.Add("UnSpecified");
                    cmbNGPPUnit.Items[0].Value = "-1";
                    for (int i = 1; i <= lstUnit.Count; i++)
                    {
                        cmbNGPPUnit.Items.Add(lstUnit[i - 1].Name.ToString());
                        cmbNGPPUnit.Items[i].Value = lstUnit[i - 1].ID.ToString();
                    }
                    cmbTLUnit.Items.Add("UnSpecified");
                    cmbTLUnit.Items[0].Value = "-1";
                    for (int i = 1; i <= lstUnit.Count; i++)
                    {
                        cmbTLUnit.Items.Add(lstUnit[i - 1].Name.ToString());
                        cmbTLUnit.Items[i].Value = lstUnit[i - 1].ID.ToString();
                    }
                    cmbMPTUnit.Items.Add("UnSpecified");
                    cmbMPTUnit.Items[0].Value = "-1";
                    for (int i = 1; i <= lstUnit.Count; i++)
                    {
                        cmbMPTUnit.Items.Add(lstUnit[i - 1].Name.ToString());
                        cmbMPTUnit.Items[i].Value = lstUnit[i - 1].ID.ToString();
                    }
                    cmbIFOLUnit.Items.Add("UnSpecified");
                    cmbIFOLUnit.Items[0].Value = "-1";
                    for (int i = 1; i <= lstUnit.Count; i++)
                    {
                        cmbIFOLUnit.Items.Add(lstUnit[i - 1].Name.ToString());
                        cmbIFOLUnit.Items[i].Value = lstUnit[i - 1].ID.ToString();
                    }
                    cmbSCEUnit.Items.Add("UnSpecified");
                    cmbSCEUnit.Items[0].Value = "-1";
                    for (int i = 1; i <= lstUnit.Count; i++)
                    {
                        cmbSCEUnit.Items.Add(lstUnit[i - 1].Name.ToString());
                        cmbSCEUnit.Items[i].Value = lstUnit[i - 1].ID.ToString();
                    }
                    cmbSCCUnit.Items.Add("UnSpecified");
                    cmbSCCUnit.Items[0].Value = "-1";
                    for (int i = 1; i <= lstUnit.Count; i++)
                    {
                        cmbSCCUnit.Items.Add(lstUnit[i - 1].Name.ToString());
                        cmbSCCUnit.Items[i].Value = lstUnit[i - 1].ID.ToString();
                    }
                    cmbHHSUnit.Items.Add("UnSpecified");
                    cmbHHSUnit.Items[0].Value = "-1";
                    for (int i = 1; i <= lstUnit.Count; i++)
                    {
                        cmbHHSUnit.Items.Add(lstUnit[i - 1].Name.ToString());
                        cmbHHSUnit.Items[i].Value = lstUnit[i - 1].ID.ToString();
                    }
                    cmbBAAUnit.Items.Add("UnSpecified");
                    cmbBAAUnit.Items[0].Value = "-1";
                    for (int i = 1; i <= lstUnit.Count; i++)
                    {
                        cmbBAAUnit.Items.Add(lstUnit[i - 1].Name.ToString());
                        cmbBAAUnit.Items[i].Value = lstUnit[i - 1].ID.ToString();
                    }
                    cmbIFCUnit.Items.Add("UnSpecified");
                    cmbIFCUnit.Items[0].Value = "-1";
                    for (int i = 1; i <= lstUnit.Count; i++)
                    {
                        cmbIFCUnit.Items.Add(lstUnit[i - 1].Name.ToString());
                        cmbIFCUnit.Items[i].Value = lstUnit[i - 1].ID.ToString();
                    }
                    cmbGFUnit.Items.Add("UnSpecified");
                    cmbGFUnit.Items[0].Value = "-1";
                    for (int i = 1; i <= lstUnit.Count; i++)
                    {
                        cmbGFUnit.Items.Add(lstUnit[i - 1].Name.ToString());
                        cmbGFUnit.Items[i].Value = lstUnit[i - 1].ID.ToString();
                    }
                    cmbBUnit.Items.Add("UnSpecified");
                    cmbBUnit.Items[0].Value = "-1";
                    for (int i = 1; i <= lstUnit.Count; i++)
                    {
                        cmbBUnit.Items.Add(lstUnit[i - 1].Name.ToString());
                        cmbBUnit.Items[i].Value = lstUnit[i - 1].ID.ToString();
                    }
                    cmbOUnit.Items.Add("UnSpecified");
                    cmbOUnit.Items[0].Value = "-1";
                    for (int i = 1; i <= lstUnit.Count; i++)
                    {
                        cmbOUnit.Items.Add(lstUnit[i - 1].Name.ToString());
                        cmbOUnit.Items[i].Value = lstUnit[i - 1].ID.ToString();
                    }
                }
                List<CauseItem> lstCauses = dbMan.GetCauses();
                if (cmbCauses.Items.Count == 0)
                {
                    cmbCauses.Items.Add("UnSpecified");
                    cmbCauses.Items[0].Value = "-1";
                    for (int i = 1; i <= lstCauses.Count; i++)
                    {
                        cmbCauses.Items.Add(lstCauses[i - 1].Name.ToString());
                        cmbCauses.Items[i].Value = lstCauses[i - 1].ID.ToString();
                    }
                }
                List<LevelofEducationItem> lstEducation = dbMan.GetLevelofEducation();
                if (cmbLevelEducation.Items.Count == 0)
                {
                    cmbLevelEducation.Items.Add("UnSpecified");
                    cmbLevelEducation.Items[0].Value = "-1";
                    for (int i = 1; i <= lstEducation.Count; i++)
                    {
                        cmbLevelEducation.Items.Add(lstEducation[i - 1].Name.ToString());
                        cmbLevelEducation.Items[i].Value = lstEducation[i - 1].ID.ToString();
                    }
                }
                List<MaritalItem> lstMarital = dbMan.GetMaritalStatus();
                if (cmbMarital.Items.Count == 0)
                {
                    cmbMarital.Items.Add("UnSpecified");
                    cmbMarital.Items[0].Value = "-1";
                    for (int i = 1; i <= lstMarital.Count; i++)
                    {
                        cmbMarital.Items.Add(lstMarital[i - 1].Name.ToString());
                        cmbMarital.Items[i].Value = lstMarital[i - 1].ID.ToString();
                    }
                }
                List<OccupationItem> lstOccupation = dbMan.GetOccupation();
                if (cmbOccupation.Items.Count == 0)
                {
                    cmbOccupation.Items.Add("UnSpecified");
                    cmbOccupation.Items[0].Value = "-1";
                    for (int i = 1; i <= lstOccupation.Count; i++)
                    {
                        cmbOccupation.Items.Add(lstOccupation[i - 1].Name.ToString());
                        cmbOccupation.Items[i].Value = lstOccupation[i - 1].ID.ToString();
                    }
                }
                List<RoleItem> lstRole = dbMan.GetRoles();
                if (cmbRole.Items.Count == 0)
                {
                    cmbRole.Items.Add("UnSpecified");
                    cmbRole.Items[0].Value = "-1";
                    for (int i = 1; i <= lstRole.Count; i++)
                    {
                        cmbRole.Items.Add(lstRole[i - 1].Name.ToString());
                        cmbRole.Items[i].Value = lstRole[i - 1].ID.ToString();
                    }
                }
                List<VulnerabilityItem> lstVul = dbMan.GetVulnerability();
                if (cmbVulnerability.Items.Count == 0)
                {
                    cmbVulnerability.Items.Add("UnSpecified");
                    cmbVulnerability.Items[0].Value = "-1";
                    for (int i = 1; i <= lstVul.Count; i++)
                    {
                        cmbVulnerability.Items.Add(lstVul[i - 1].Name.ToString());
                        cmbVulnerability.Items[i].Value = lstVul[i - 1].ID.ToString();
                    }
                }
                List<CountryItem> lstCountries = dbMan.GetCountries();
                if (cmbPreCountry.Items.Count == 0)
                {
                    cmbPreCountry.Items.Add("UnSpecified");
                    cmbPreCountry.Items[0].Value = "-1";

                    cmbCountryBirth.Items.Add("UnSpecified");
                    cmbCountryBirth.Items[0].Value = "-1";
                    for (int i = 1; i <= lstCountries.Count; i++)
                    {
                        cmbPreCountry.Items.Add(lstCountries[i - 1].Name.ToString());
                        cmbPreCountry.Items[i].Value = lstCountries[i - 1].ID.ToString();

                        cmbCountryBirth.Items.Add(lstCountries[i - 1].Name.ToString());
                        cmbCountryBirth.Items[i].Value = lstCountries[i - 1].ID.ToString();
                    }
                }
                if (cmbPreDistrict.Items.Count == 0)
                {
                    cmbPreDistrict.Items.Add("UnSpecified");
                    cmbPreDistrict.Items[0].Value = "-1";
                }
                if (cmbPreWardno.Items.Count == 0)
                {
                    cmbPreWardno.Items.Add("UnSpecified");
                    cmbPreWardno.Items[0].Value = "-1";
                }
                List<CommunityItem> lstCommunity = dbMan.GetCommunity();
                if (cmbCommunity.Items.Count == 0)
                {
                    cmbCommunity.Items.Add("UnSpecified");
                    cmbCommunity.Items[0].Value = "-1";

                    cmbRenameCommunity.Items.Add("UnSpecified");
                    cmbRenameCommunity.Items[0].Value = "-1";

                    cmbRemoveCommunity.Items.Add("UnSpecified");
                    cmbRemoveCommunity.Items[0].Value = "-1";

                    for (int i = 1; i <= lstCommunity.Count; i++)
                    {
                        cmbCommunity.Items.Add(lstCommunity[i - 1].Name.ToString());
                        cmbCommunity.Items[i].Value = lstCommunity[i - 1].ID.ToString();

                        cmbRenameCommunity.Items.Add(lstCommunity[i - 1].Name.ToString());
                        cmbRenameCommunity.Items[i].Value = lstCommunity[i - 1].ID.ToString();

                        cmbRemoveCommunity.Items.Add(lstCommunity[i - 1].Name.ToString());
                        cmbRemoveCommunity.Items[i].Value = lstCommunity[i - 1].ID.ToString();
                    }
                }
                if (cmbHousehold.Items.Count == 0)
                {
                    cmbHousehold.Items.Add("UnSpecified");
                    cmbHousehold.Items[0].Value = "-1";

                    cmbRenameHousehold.Items.Add("UnSpecified");
                    cmbRenameHousehold.Items[0].Value = "-1";

                    cmbRemoveHousehold.Items.Add("UnSpecified");
                    cmbRemoveHousehold.Items[0].Value = "-1";
                }
                if (sltProvince.SelectedIndex == 0)
                {
                    string zero4 = "0000";
                    string id = (dbMan.GetMaxIDFromTable("main_information") + 1).ToString();
                    id = zero4.Substring(0, 4 - id.Length) + id;
                    lblCaseNo.Text = DateTime.Now.ToString("MM/yyyy") + "/" + id;
                }
                else
                {
                    string zero4 = "0000";
                    string id = (dbMan.GetMaxIDFromTable("main_information") + 1).ToString();
                    id = zero4.Substring(0, 4 - id.Length) + id;
                    List<ProvinceItem> lstProvince1 = dbMan.GetProvince(Convert.ToInt32(sltProvince.SelectedValue));
                    string provinceCode = "";
                    if (lstProvince.Count > 0)
                        provinceCode = lstProvince1[0].Code;
                    lblCaseNo.Text = provinceCode + "/" + DateTime.Now.ToString("MM/yyyy") + "/" + id;
                }

                string caseId = Request.QueryString["id"];
                if (string.IsNullOrEmpty(caseId))
                    hdnMainId.Value = "0";
                else
                {
                    hdnMainId.Value = caseId;
                    btnSave.Text = "Update";
                    OnEditCases(Convert.ToInt32(caseId));
                }
            }
        }

        protected void sltProvince_SelectedIndexChanged(object sender, EventArgs e)
        {
            ProvinceSelectedIndex(sltProvince.SelectedValue);
        }

        protected void sltDistrict_SelectedIndexChanged(object sender, EventArgs e)
        {
            DistrictSelectedIndex(sltDistrict.SelectedValue);
        }

        protected void sltLLG_SelectedIndexChanged(object sender, EventArgs e)
        {
            LLGSelectedIndex(sltLLG.SelectedValue);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            bool isSave = btnSave.Text == "Save" ? true : false;
            try
            {

                //Base Information
                #region Base Information
                string organizationId = sltOrgan.SelectedValue;
                string provinceId = sltProvince.SelectedValue;
                string districtId = sltDistrict.SelectedValue;
                string llgId = sltLLG.SelectedValue;
                string wardnoId = sltWardno.SelectedValue;
                string placeName = txtPlaceName.Text;
                string dateIncidentOccured = dtDateIncidentOccured.Text;
                string dateIncidentReported = dtDateIncidentReported.Text;
                string caseno = lblCaseNo.Text;

                int maininfo_id = dbMan.InsertMainInformation(hdnMainId.Value, organizationId, provinceId, districtId, llgId, wardnoId, placeName, dateIncidentOccured, dateIncidentReported, caseno);
                #endregion

                //Affected Numbers
                #region Affected Numbers
                string numberAffectedHH = txtNumberAffectedHH.Value;
                string numberAffectedIndividuals = txtNumberAffectedIndividuals.Value;
                string males1 = txtMales1.Value;
                string female1 = txtFemales1.Value;
                string displacedNoHH = txtDisplacedNoHH.Value;
                string displacedNoIndividuals = txtDisplacedNoIndividuals.Value;
                string males2 = txtMales2.Value;
                string female2 = txtFemales2.Value;
                #endregion

                //Affected Populations
                #region Affected Populations
                string affectedPopulaction11 = txtAffectedPopulation11.Value;
                string affectedPopulaction12 = txtAffectedPopulation12.Value;
                string affectedPopulaction13 = txtAffectedPopulation13.Value;
                string affectedPopulaction14 = txtAffectedPopulation14.Value;
                //string affectedPopulaction15 = txtAffectedPopulation15.Value;
                //string affectedPopulaction16 = txtAffectedPopulation16.Value;

                string affectedPopulaction21 = txtAffectedPopulation21.Value;
                string affectedPopulaction22 = txtAffectedPopulation22.Value;
                string affectedPopulaction23 = txtAffectedPopulation23.Value;
                string affectedPopulaction24 = txtAffectedPopulation24.Value;
                //string affectedPopulaction25 = txtAffectedPopulation25.Value;
                //string affectedPopulaction26 = txtAffectedPopulation26.Value;

                string affectedPopulaction31 = txtAffectedPopulation31.Value;
                string affectedPopulaction32 = txtAffectedPopulation32.Value;
                string affectedPopulaction33 = txtAffectedPopulation33.Value;
                string affectedPopulaction34 = txtAffectedPopulation34.Value;
                //string affectedPopulaction35 = txtAffectedPopulation35.Value;
                //string affectedPopulaction36 = txtAffectedPopulation36.Value;

                string affectedPopulaction41 = txtAffectedPopulation41.Value;
                string affectedPopulaction42 = txtAffectedPopulation42.Value;
                string affectedPopulaction43 = txtAffectedPopulation43.Value;
                string affectedPopulaction44 = txtAffectedPopulation44.Value;
                //string affectedPopulaction45 = txtAffectedPopulation45.Value;
                //string affectedPopulaction46 = txtAffectedPopulation46.Value;

                string affectedPopulaction51 = txtAffectedPopulation51.Value;
                string affectedPopulaction52 = txtAffectedPopulation52.Value;
                string affectedPopulaction53 = txtAffectedPopulation53.Value;
                string affectedPopulaction54 = txtAffectedPopulation54.Value;
                //string affectedPopulaction55 = txtAffectedPopulation55.Value;
                //string affectedPopulaction56 = txtAffectedPopulation56.Value;

                string affectedPopulaction61 = txtAffectedPopulation61.Value;
                string affectedPopulaction62 = txtAffectedPopulation62.Value;
                string affectedPopulaction63 = txtAffectedPopulation63.Value;
                string affectedPopulaction64 = txtAffectedPopulation64.Value;
                //string affectedPopulaction65 = txtAffectedPopulation65.Value;
                //string affectedPopulaction66 = txtAffectedPopulation66.Value;

                string affectedPopulationOther = txtAffectedPopulationOther.Value;

                string incidentId = sltTypeIncident.SelectedValue;
                string incidentOther = txtTypeIncidentOther.Text;
                string displacementId = sltDisplacementStatus.SelectedValue;
                string displacementOther = txtDisplacementStatusOther.Text;

                string pregnant = txtPregnant.Value;
                string lactating = txtLactating.Value;
                string orphans = txtOrphans.Value;
                string childheaded = txtChildheaded.Value;
                string ederly = txtEderly.Value;
                string disabled = txtDisabled.Value;
                string chronically = txtChronicallyill.Value;

                string agegroup4 = txtAgegroup4.Value;
                string agegroup5 = txtAgegroup5.Value;
                string agegroup18 = txtAgegroup18.Value;
                string agegroup31 = txtAgegroup31.Value;
                string agegroup44 = txtAgegroup44.Value;
                string agegroup57 = txtAgegroup57.Value;
                string agegroup65 = txtAgegroup65.Value;

                string needfood = chkEducation.Checked ? "1" : "0";
                string nutrition = chkNutrition.Checked ? "1" : "0";
                string water = chkWater.Checked ? "1" : "0";
                string sanitation = chkSanitation.Checked ? "1" : "0";
                string hygiene = chkHygiene.Checked ? "1" : "0";
                string shelter = chkShelter.Checked ? "1" : "0";
                string education = chkEducation.Checked ? "1" : "0";
                string health = chkHealth.Checked ? "1" : "0";
                string taother = taOther.Value;

                List<string> fieldList = new List<string>();
                List<string> valueList = new List<string>();
                if (isSave)
                {
                    fieldList.Add("id");
                    valueList.Add((dbMan.GetMaxIDFromTable("affected_population") + 1).ToString());
                }

                fieldList.Add("main_information_id");
                valueList.Add(maininfo_id.ToString());

                fieldList.Add("number_affected_hh");
                valueList.Add(numberAffectedHH);

                fieldList.Add("number_affected_individuals");
                valueList.Add(numberAffectedIndividuals);

                fieldList.Add("males1");
                valueList.Add(males1);

                fieldList.Add("females1");
                valueList.Add(female1);

                fieldList.Add("displaced_hh");
                valueList.Add(displacedNoHH);

                fieldList.Add("displaced_individuals");
                valueList.Add(displacedNoIndividuals);

                fieldList.Add("males2");
                valueList.Add(males2);

                fieldList.Add("females2");
                valueList.Add(female2);

                fieldList.Add("affected_population_11");
                valueList.Add(affectedPopulaction11);

                fieldList.Add("affected_population_12");
                valueList.Add(affectedPopulaction12);

                fieldList.Add("affected_population_13");
                valueList.Add(affectedPopulaction13);

                fieldList.Add("affected_population_14");
                valueList.Add(affectedPopulaction14);

                //fieldList.Add("affected_population_15");
                //valueList.Add(affectedPopulaction15);

                //fieldList.Add("affected_population_16");
                //valueList.Add(affectedPopulaction16);

                fieldList.Add("affected_population_21");
                valueList.Add(affectedPopulaction21);

                fieldList.Add("affected_population_22");
                valueList.Add(affectedPopulaction22);

                fieldList.Add("affected_population_23");
                valueList.Add(affectedPopulaction23);

                fieldList.Add("affected_population_24");
                valueList.Add(affectedPopulaction24);

                //fieldList.Add("affected_population_25");
                //valueList.Add(affectedPopulaction25);

                //fieldList.Add("affected_population_26");
                //valueList.Add(affectedPopulaction26);

                fieldList.Add("affected_population_31");
                valueList.Add(affectedPopulaction31);

                fieldList.Add("affected_population_32");
                valueList.Add(affectedPopulaction32);

                fieldList.Add("affected_population_33");
                valueList.Add(affectedPopulaction33);

                fieldList.Add("affected_population_34");
                valueList.Add(affectedPopulaction34);

                //fieldList.Add("affected_population_35");
                //valueList.Add(affectedPopulaction35);

                //fieldList.Add("affected_population_36");
                //valueList.Add(affectedPopulaction36);

                fieldList.Add("affected_population_41");
                valueList.Add(affectedPopulaction41);

                fieldList.Add("affected_population_42");
                valueList.Add(affectedPopulaction42);

                fieldList.Add("affected_population_43");
                valueList.Add(affectedPopulaction43);

                fieldList.Add("affected_population_44");
                valueList.Add(affectedPopulaction44);

                //fieldList.Add("affected_population_45");
                //valueList.Add(affectedPopulaction45);

                //fieldList.Add("affected_population_46");
                //valueList.Add(affectedPopulaction46);

                fieldList.Add("affected_population_51");
                valueList.Add(affectedPopulaction51);

                fieldList.Add("affected_population_52");
                valueList.Add(affectedPopulaction52);

                fieldList.Add("affected_population_53");
                valueList.Add(affectedPopulaction53);

                fieldList.Add("affected_population_54");
                valueList.Add(affectedPopulaction54);

                //fieldList.Add("affected_population_55");
                //valueList.Add(affectedPopulaction55);

                //fieldList.Add("affected_population_56");
                //valueList.Add(affectedPopulaction56);

                fieldList.Add("affected_population_61");
                valueList.Add(affectedPopulaction61);

                fieldList.Add("affected_population_62");
                valueList.Add(affectedPopulaction62);

                fieldList.Add("affected_population_63");
                valueList.Add(affectedPopulaction63);

                fieldList.Add("affected_population_64");
                valueList.Add(affectedPopulaction64);

                //fieldList.Add("affected_population_65");
                //valueList.Add(affectedPopulaction65);

                //fieldList.Add("affected_population_66");
                //valueList.Add(affectedPopulaction66);

                fieldList.Add("affected_population_other");
                valueList.Add(affectedPopulationOther);

                fieldList.Add("incident_id");
                valueList.Add(incidentId);

                fieldList.Add("incident_other");
                valueList.Add(incidentOther);

                fieldList.Add("displacement_id");
                valueList.Add(displacementId);

                fieldList.Add("displacement_other");
                valueList.Add(displacementOther);

                fieldList.Add("pregnant");
                valueList.Add(pregnant);

                fieldList.Add("lactating");
                valueList.Add(lactating);

                fieldList.Add("orphans");
                valueList.Add(orphans);

                fieldList.Add("childheaded");
                valueList.Add(childheaded);

                fieldList.Add("ederly");
                valueList.Add(ederly);

                fieldList.Add("disabled");
                valueList.Add(disabled);

                fieldList.Add("chronicallyill");
                valueList.Add(chronically);

                fieldList.Add("agegroup4");
                valueList.Add(agegroup4);

                fieldList.Add("agegroup5");
                valueList.Add(agegroup5);

                fieldList.Add("agegroup18");
                valueList.Add(agegroup18);

                fieldList.Add("agegroup31");
                valueList.Add(agegroup31);

                fieldList.Add("agegroup44");
                valueList.Add(agegroup44);

                fieldList.Add("agegroup57");
                valueList.Add(agegroup57);

                fieldList.Add("agegroup65");
                valueList.Add(agegroup65);

                fieldList.Add("foodneed");
                valueList.Add(needfood);

                fieldList.Add("nutrition");
                valueList.Add(nutrition);

                fieldList.Add("water");
                valueList.Add(water);

                fieldList.Add("sanitation");
                valueList.Add(sanitation);

                fieldList.Add("hygiene");
                valueList.Add(hygiene);

                fieldList.Add("shelter");
                valueList.Add(shelter);

                fieldList.Add("education");
                valueList.Add(education);

                fieldList.Add("health");
                valueList.Add(health);

                fieldList.Add("checkgroup_other");
                valueList.Add(taother);

                if (isSave)
                    dbMan.InsertIntoTable("affected_population", fieldList, valueList);
                else
                    dbMan.UpdateTable("affected_population", hdnaffectedpopulation.Value, fieldList, valueList);
                fieldList.Clear();
                valueList.Clear();
                #endregion

                // Damaged Infrastructure
                #region Damaged Infrastructure
                string rhqty = txtRHQty.Value;
                string rhunit = cmbRHUnit.SelectedValue;
                string rhcomment = txtRHComment.Value;

                string ssqty = txtSSQty.Value;
                string ssunit = cmbSSUnit.SelectedValue;
                string sscomment = txtSSComment.Value;

                string eplqty = txtEPLQty.Value;
                string eplunit = cmbEPLUnit.SelectedValue;
                string eplcomment = txtEPLComment.Value;

                string epsqty = txtEPSQty.Value;
                string epsunit = cmbEPSUnit.SelectedValue;
                string epscomment = txtEPSComment.Value;

                string ngppqty = txtNGPPQty.Value;
                string ngppunit = cmbNGPPUnit.SelectedValue;
                string ngppcomment = txtNGPPComment.Value;

                string tlqty = txtTLQty.Value;
                string tlunit = cmbTLUnit.SelectedValue;
                string tlcomment = txtTLComment.Value;

                string mptqty = txtMPTQty.Value;
                string mptunit = cmbMPTUnit.SelectedValue;
                string mptcomment = txtMPTComment.Value;

                string ifolqty = txtIFOLQty.Value;
                string ifolunit = cmbIFOLUnit.SelectedValue;
                string ifolcomment = txtIFOLComment.Value;

                string sceqty = txtSCEQty.Value;
                string sceunit = cmbSCEUnit.SelectedValue;
                string scecomment = txtSCEComment.Value;

                string sccqty = txtSCCQty.Value;
                string sccunit = cmbSCCUnit.SelectedValue;
                string scccomment = txtSCCComment.Value;

                string hhsdqty = txtHHSQty.Value;
                string hhsdunit = cmbHHSUnit.SelectedValue;
                string hhsdcomment = txtHSSComment.Value;

                string baqty = txtBAAQty.Value;
                string baunit = cmbBAAUnit.SelectedValue;
                string bacomment = txtBAAComment.Value;

                string ifcqty = txtIFCQty.Value;
                string ifcunit = cmbIFCUnit.SelectedValue;
                string ifccomment = txtIFCComment.Value;

                string gfqty = txtGFQty.Value;
                string gfunit = cmbGFUnit.SelectedValue;
                string gfcomment = txtGFComment.Value;

                string bqty = txtBQty.Value;
                string bunit = cmbBUnit.SelectedValue;
                string bcomment = txtBComment.Value;

                string olabel = txtDIOther.Value;
                string oqty = txtOQty.Value;
                string ounit = cmbOUnit.SelectedValue;
                string ocomment = txtOComment.Value;

                string typelocation = txtTypeLocation.Value;
                string gpsSouth = txtSouth.Value;
                string gpsEast = txtEast.Value;

                if (isSave)
                {
                    fieldList.Add("id");
                    valueList.Add((dbMan.GetMaxIDFromTable("damaged_infrastructure") + 1).ToString());
                }

                fieldList.Add("main_information_id");
                valueList.Add(maininfo_id.ToString());

                fieldList.Add("rh_qty");
                valueList.Add(rhqty);

                fieldList.Add("rh_unit");
                valueList.Add(rhunit);

                fieldList.Add("rh_comment");
                valueList.Add(rhcomment);

                fieldList.Add("ss_qty");
                valueList.Add(ssqty);

                fieldList.Add("ss_unit");
                valueList.Add(ssunit);

                fieldList.Add("ss_comment");
                valueList.Add(sscomment);

                fieldList.Add("epl_qty");
                valueList.Add(eplqty);

                fieldList.Add("epl_unit");
                valueList.Add(eplunit);

                fieldList.Add("epl_comment");
                valueList.Add(eplcomment);

                fieldList.Add("eps_qty");
                valueList.Add(epsqty);

                fieldList.Add("eps_unit");
                valueList.Add(epsunit);

                fieldList.Add("eps_comment");
                valueList.Add(epscomment);

                fieldList.Add("ngpp_qty");
                valueList.Add(ngppqty);

                fieldList.Add("ngpp_unit");
                valueList.Add(ngppunit);

                fieldList.Add("ngpp_comment");
                valueList.Add(ngppcomment);

                fieldList.Add("tl_qty");
                valueList.Add(tlqty);

                fieldList.Add("tl_unit");
                valueList.Add(tlunit);

                fieldList.Add("tl_comment");
                valueList.Add(tlcomment);

                fieldList.Add("mpt_qty");
                valueList.Add(mptqty);

                fieldList.Add("mpt_unit");
                valueList.Add(mptunit);

                fieldList.Add("mpt_comment");
                valueList.Add(mptcomment);

                fieldList.Add("ifol_qty");
                valueList.Add(ifolqty);

                fieldList.Add("ifol_unit");
                valueList.Add(ifolunit);

                fieldList.Add("ifol_comment");
                valueList.Add(ifolcomment);

                fieldList.Add("sce_qty");
                valueList.Add(sceqty);

                fieldList.Add("sce_unit");
                valueList.Add(sceunit);

                fieldList.Add("sce_comment");
                valueList.Add(scecomment);

                fieldList.Add("scc_qty");
                valueList.Add(sccqty);

                fieldList.Add("scc_unit");
                valueList.Add(sccunit);

                fieldList.Add("scc_comment");
                valueList.Add(scccomment);

                fieldList.Add("hhs_qty");
                valueList.Add(hhsdqty);

                fieldList.Add("hhs_unit");
                valueList.Add(hhsdunit);

                fieldList.Add("hhs_comment");
                valueList.Add(hhsdcomment);

                fieldList.Add("ba_qty");
                valueList.Add(baqty);

                fieldList.Add("ba_unit");
                valueList.Add(baunit);

                fieldList.Add("ba_comment");
                valueList.Add(bacomment);

                fieldList.Add("ifc_qty");
                valueList.Add(ifcqty);

                fieldList.Add("ifc_unit");
                valueList.Add(ifcunit);

                fieldList.Add("ifc_comment");
                valueList.Add(ifccomment);

                fieldList.Add("gf_qty");
                valueList.Add(gfqty);

                fieldList.Add("gf_unit");
                valueList.Add(gfunit);

                fieldList.Add("gf_comment");
                valueList.Add(gfcomment);

                fieldList.Add("b_qty");
                valueList.Add(bqty);

                fieldList.Add("b_unit");
                valueList.Add(bunit);

                fieldList.Add("b_comment");
                valueList.Add(bcomment);

                fieldList.Add("other_qty");
                valueList.Add(oqty);

                fieldList.Add("other_unit");
                valueList.Add(ounit);

                fieldList.Add("other_comment");
                valueList.Add(ocomment);

                fieldList.Add("other_caption");
                valueList.Add(olabel);

                fieldList.Add("type_location");
                valueList.Add(typelocation);

                fieldList.Add("gps_south");
                valueList.Add(gpsSouth);

                fieldList.Add("gps_east");
                valueList.Add(gpsEast);

                if (isSave)
                    dbMan.InsertIntoTable("damaged_infrastructure", fieldList, valueList);
                else
                    dbMan.UpdateTable("damaged_infrastructure", hdndamagedInfrastructure.Value, fieldList, valueList);
                fieldList.Clear();
                valueList.Clear();
                #endregion

                // Access to Essential Services
                #region Essential Services
                string schools = chkSchools.Checked ? "1" : "0";
                string schoolComment = txtSchoolComment.Value;

                string hc = chkHC.Checked ? "1" : "0";
                string hcComment = txtHCComment.Value;

                string ps = chkPS.Checked ? "1" : "0";
                string psComment = txtPSComment.Value;

                string wssw = chkWSSW.Checked ? "1" : "0";
                string wsswComment = txtWSSWComment.Value;

                string fp = chkFP.Checked ? "1" : "0";
                string fpComment = txtFPComment.Value;

                string aesOther = txtAESOther.Value;
                string aeso = chkAESOther.Checked ? "1" : "0";
                string aesoComment = txtAESOtherComment.Value;

                string byroad = chkRoad.Checked ? "1" : "0";
                string byair = chkAir.Checked ? "1" : "0";
                string byboat = chkBoat.Checked ? "1" : "0";
                string byother = chkAPROther.Checked ? "1" : "0";
                string byothercomment = txtAPROther.Value;

                string comment = txtComment.Value;

                if (isSave)
                {
                    fieldList.Add("id");
                    valueList.Add((dbMan.GetMaxIDFromTable("access_essential_service") + 1).ToString());
                }

                fieldList.Add("main_information_id");
                valueList.Add(maininfo_id.ToString());

                fieldList.Add("school_chk");
                valueList.Add(schools);

                fieldList.Add("school_comment");
                valueList.Add(schoolComment);

                fieldList.Add("hospital_chk");
                valueList.Add(hc);

                fieldList.Add("hospital_comment");
                valueList.Add(hcComment);

                fieldList.Add("policing_chk");
                valueList.Add(ps);

                fieldList.Add("policing_comment");
                valueList.Add(psComment);

                fieldList.Add("water_chk");
                valueList.Add(wssw);

                fieldList.Add("water_comment");
                valueList.Add(wsswComment);

                fieldList.Add("food_chk");
                valueList.Add(fp);

                fieldList.Add("food_comment");
                valueList.Add(fpComment);

                fieldList.Add("other_chk");
                valueList.Add(aeso);

                fieldList.Add("other_comment");
                valueList.Add(aesoComment);

                fieldList.Add("other_caption");
                valueList.Add(aesOther);

                fieldList.Add("byroad_chk");
                valueList.Add(byroad);

                fieldList.Add("byair_chk");
                valueList.Add(byair);

                fieldList.Add("byboat_chk");
                valueList.Add(byboat);

                fieldList.Add("aprother_chk");
                valueList.Add(byother);

                fieldList.Add("aprother_comment");
                valueList.Add(byothercomment);

                fieldList.Add("comment");
                valueList.Add(comment);

                if (isSave)
                    dbMan.InsertIntoTable("access_essential_service", fieldList, valueList);
                else
                    dbMan.UpdateTable("access_essential_service", hdnEssentialServices.Value, fieldList, valueList);
                fieldList.Clear();
                valueList.Clear();
                #endregion

                // Causes
                #region Cause Info
                string causeId = cmbCauses.SelectedValue;
                string specify = chkSpecify.Checked ? "1" : "0";
                string specifyComment = txtSpecify.Value;

                string fkp = txtFKP.Value;
                string ckp = txtCKP.Value;
                string tm = txtTM.Value;

                string caseclosed = chkCaseClosed.Checked ? "1" : "0";
                string caseclosedComment = txtCaseClosed.Value;
                string orn = txtORN.Value;

                if (isSave)
                {
                    fieldList.Add("id");
                    valueList.Add((dbMan.GetMaxIDFromTable("causes_info") + 1).ToString());
                }

                fieldList.Add("main_information_id");
                valueList.Add(maininfo_id.ToString());

                fieldList.Add("cause_id");
                valueList.Add(causeId);

                fieldList.Add("specify_chk");
                valueList.Add(specify);

                fieldList.Add("specify_comment");
                valueList.Add(specifyComment);

                fieldList.Add("gfkp_comment");
                valueList.Add(fkp);

                fieldList.Add("ckp_comment");
                valueList.Add(ckp);

                fieldList.Add("gtm_comment");
                valueList.Add(tm);

                fieldList.Add("caseclosed_chk");
                valueList.Add(caseclosed);

                fieldList.Add("caseclosed_comment");
                valueList.Add(caseclosedComment);

                fieldList.Add("orn_comment");
                valueList.Add(orn);

                if (isSave)
                    dbMan.InsertIntoTable("causes_info", fieldList, valueList);
                else
                    dbMan.UpdateTable("causes_info", hdnCauses.Value, fieldList, valueList);
                fieldList.Clear();
                valueList.Clear();
                #endregion

                //Previous Location
                #region Previous Location
                string preprovinceId = cmbPreProvince.SelectedValue;
                string predistrictId = cmbPreDistrict.SelectedValue;
                string prewardnoId = cmbPreWardno.SelectedValue;
                string precountryId = cmbPreCountry.SelectedValue;
                string preplacename = txtPrePlaceName.Value;
                string pregpssouth = txtGPSSouth.Value;
                string pregpseast = txtGPSEast.Value;
                if (isSave)
                {
                    fieldList.Add("id");
                    valueList.Add((dbMan.GetMaxIDFromTable("previous_location") + 1).ToString());
                }

                fieldList.Add("main_information_id");
                valueList.Add(maininfo_id.ToString());

                fieldList.Add("province_id");
                valueList.Add(preprovinceId);

                fieldList.Add("district_id");
                valueList.Add(predistrictId);

                fieldList.Add("wardno_id");
                valueList.Add(prewardnoId);

                fieldList.Add("country_id");
                valueList.Add(precountryId);

                fieldList.Add("placename");
                valueList.Add(preplacename);

                fieldList.Add("gps_south");
                valueList.Add(pregpssouth);

                fieldList.Add("gps_east");
                valueList.Add(pregpseast);

                if (isSave)
                    dbMan.InsertIntoTable("previous_location", fieldList, valueList);
                else
                    dbMan.UpdateTable("previous_location", hdnPreviousLocation.Value, fieldList, valueList);

                fieldList.Clear();
                valueList.Clear();
                #endregion

                //affected groups
                #region Affected Groups
                string community_id = cmbCommunity.SelectedValue;
                string household_id = cmbHousehold.SelectedValue;
                string individual_id = Convert.ToString(grdIndividuals.SelectedValue);

                if (isSave)
                {
                    fieldList.Add("id");
                    valueList.Add((dbMan.GetMaxIDFromTable("affected_group") + 1).ToString());
                }
                fieldList.Add("main_information_id");
                valueList.Add(maininfo_id.ToString());
                fieldList.Add("community_id");
                valueList.Add(community_id);
                fieldList.Add("household_id");
                valueList.Add(household_id);
                fieldList.Add("individual_id");
                valueList.Add(individual_id);
                if (isSave)
                    dbMan.InsertIntoTable("affected_group", fieldList, valueList);
                else
                    dbMan.UpdateTable("affected_group", hdnAffectedGroups.Value, fieldList, valueList);
                fieldList.Clear();
                valueList.Clear();
                #endregion

                string message = "Case details have been " + (isSave ? "saved" : "updated") + " successfully.";
                string script = "window.onload = function(){ alert('" + message + "'); window.location ='/';};";
                ClientScript.RegisterStartupScript(this.GetType(), "SuccessMessage", script, true);
            }
            catch (Exception ex)
            {
                string message = "Case details not saved.";
                string script = "window.onload = function(){ alert('";
                script += message;
                script += "')};";
                ClientScript.RegisterStartupScript(this.GetType(), "ErrorMessage", script, true);
            }
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            InitializeControls();
        }

        protected void InitializeControls()
        {

            string zero4 = "0000";
            string id = (dbMan.GetMaxIDFromTable("main_information") + 1).ToString();
            id = zero4.Substring(0, 4 - id.Length) + id;
            lblCaseNo.Text = DateTime.Now.ToString("MM/yyyy") + "/" + id;
            List<OrganizationItem> lstOrganization = dbMan.GetOrganization();
            sltOrgan.Items.Clear();
            sltOrgan.Items.Add("UnSpecified");
            sltOrgan.Items[0].Value = "-1";
            for (int i = 1; i <= lstOrganization.Count; i++)
            {
                sltOrgan.Items.Add(lstOrganization[i - 1].Name.ToString());
                sltOrgan.Items[i].Value = lstOrganization[i - 1].ID.ToString();
            }
            List<ProvinceItem> lstProvince = dbMan.GetProvince();
            sltProvince.Items.Clear();
            sltProvince.Items.Add("UnSpecified");
            sltProvince.Items[0].Value = "-1";
            for (int i = 1; i <= lstProvince.Count; i++)
            {
                sltProvince.Items.Add(lstProvince[i - 1].Name.ToString());
                sltProvince.Items[i].Value = lstProvince[i - 1].ID.ToString();
            }
            cmbPreProvince.Items.Clear();
            cmbPreProvince.Items.Add("UnSpecified");
            cmbPreProvince.Items[0].Value = "-1";
            for (int i = 1; i <= lstProvince.Count; i++)
            {
                cmbPreProvince.Items.Add(lstProvince[i - 1].Name.ToString());
                cmbPreProvince.Items[i].Value = lstProvince[i - 1].ID.ToString();
            }
            sltDistrict.Items.Clear();
            sltDistrict.Items.Add("UnSpecified");
            sltDistrict.Items[0].Value = "-1";
            sltWardno.Items.Clear();
            sltWardno.Items.Add("UnSpecified");
            sltWardno.Items[0].Value = "-1";
            List<IncidentItem> lstIncident = dbMan.GetIncident();
            sltTypeIncident.Items.Clear();
            sltTypeIncident.Items.Add("UnSpecified");
            sltTypeIncident.Items[0].Value = "-1";
            for (int i = 1; i <= lstIncident.Count; i++)
            {
                sltTypeIncident.Items.Add(lstIncident[i - 1].Name.ToString());
                sltTypeIncident.Items[i].Value = lstIncident[i - 1].ID.ToString();
            }
            List<DisplacementItem> lstDisplacement = dbMan.GetDisplacement();
            sltDisplacementStatus.Items.Clear();
            sltDisplacementStatus.Items.Add("UnSpecified");
            sltDisplacementStatus.Items[0].Value = "-1";
            cmbDisplacementIndi.Items.Clear();
            cmbDisplacementIndi.Items.Add("UnSpecified");
            cmbDisplacementIndi.Items[0].Value = "-1";
            for (int i = 1; i <= lstDisplacement.Count; i++)
            {
                sltDisplacementStatus.Items.Add(lstDisplacement[i - 1].Name.ToString());
                sltDisplacementStatus.Items[i].Value = lstDisplacement[i - 1].ID.ToString();
                cmbDisplacementIndi.Items.Add(lstDisplacement[i - 1].Name.ToString());
                cmbDisplacementIndi.Items[i].Value = lstDisplacement[i - 1].ID.ToString();
            }

            List<UnitItem> lstUnit = dbMan.GetUnits();
            cmbRHUnit.Items.Clear();
            cmbRHUnit.Items.Add("UnSpecified");
            cmbRHUnit.Items[0].Value = "-1";
            for (int i = 1; i <= lstUnit.Count; i++)
            {
                cmbRHUnit.Items.Add(lstUnit[i - 1].Name.ToString());
                cmbRHUnit.Items[i].Value = lstUnit[i - 1].ID.ToString();
            }
            cmbSSUnit.Items.Clear();
            cmbSSUnit.Items.Add("UnSpecified");
            cmbSSUnit.Items[0].Value = "-1";
            for (int i = 1; i <= lstUnit.Count; i++)
            {
                cmbSSUnit.Items.Add(lstUnit[i - 1].Name.ToString());
                cmbSSUnit.Items[i].Value = lstUnit[i - 1].ID.ToString();
            }
            cmbEPLUnit.Items.Clear();
            cmbEPLUnit.Items.Add("UnSpecified");
            cmbEPLUnit.Items[0].Value = "-1";
            for (int i = 1; i <= lstUnit.Count; i++)
            {
                cmbEPLUnit.Items.Add(lstUnit[i - 1].Name.ToString());
                cmbEPLUnit.Items[i].Value = lstUnit[i - 1].ID.ToString();
            }
            cmbEPSUnit.Items.Clear();
            cmbEPSUnit.Items.Add("UnSpecified");
            cmbEPSUnit.Items[0].Value = "-1";
            for (int i = 1; i <= lstUnit.Count; i++)
            {
                cmbEPSUnit.Items.Add(lstUnit[i - 1].Name.ToString());
                cmbEPSUnit.Items[i].Value = lstUnit[i - 1].ID.ToString();
            }
            cmbNGPPUnit.Items.Clear();
            cmbNGPPUnit.Items.Add("UnSpecified");
            cmbNGPPUnit.Items[0].Value = "-1";
            for (int i = 1; i <= lstUnit.Count; i++)
            {
                cmbNGPPUnit.Items.Add(lstUnit[i - 1].Name.ToString());
                cmbNGPPUnit.Items[i].Value = lstUnit[i - 1].ID.ToString();
            }
            cmbTLUnit.Items.Clear();
            cmbTLUnit.Items.Add("UnSpecified");
            cmbTLUnit.Items[0].Value = "-1";
            for (int i = 1; i <= lstUnit.Count; i++)
            {
                cmbTLUnit.Items.Add(lstUnit[i - 1].Name.ToString());
                cmbTLUnit.Items[i].Value = lstUnit[i - 1].ID.ToString();
            }
            cmbMPTUnit.Items.Clear();
            cmbMPTUnit.Items.Add("UnSpecified");
            cmbMPTUnit.Items[0].Value = "-1";
            for (int i = 1; i <= lstUnit.Count; i++)
            {
                cmbMPTUnit.Items.Add(lstUnit[i - 1].Name.ToString());
                cmbMPTUnit.Items[i].Value = lstUnit[i - 1].ID.ToString();
            }
            cmbIFOLUnit.Items.Clear();
            cmbIFOLUnit.Items.Add("UnSpecified");
            cmbIFOLUnit.Items[0].Value = "-1";
            for (int i = 1; i <= lstUnit.Count; i++)
            {
                cmbIFOLUnit.Items.Add(lstUnit[i - 1].Name.ToString());
                cmbIFOLUnit.Items[i].Value = lstUnit[i - 1].ID.ToString();
            }
            cmbSCEUnit.Items.Clear();
            cmbSCEUnit.Items.Add("UnSpecified");
            cmbSCEUnit.Items[0].Value = "-1";
            for (int i = 1; i <= lstUnit.Count; i++)
            {
                cmbSCEUnit.Items.Add(lstUnit[i - 1].Name.ToString());
                cmbSCEUnit.Items[i].Value = lstUnit[i - 1].ID.ToString();
            }
            cmbSCCUnit.Items.Clear();
            cmbSCCUnit.Items.Add("UnSpecified");
            cmbSCCUnit.Items[0].Value = "-1";
            for (int i = 1; i <= lstUnit.Count; i++)
            {
                cmbSCCUnit.Items.Add(lstUnit[i - 1].Name.ToString());
                cmbSCCUnit.Items[i].Value = lstUnit[i - 1].ID.ToString();
            }
            cmbHHSUnit.Items.Clear();
            cmbHHSUnit.Items.Add("UnSpecified");
            cmbHHSUnit.Items[0].Value = "-1";
            for (int i = 1; i <= lstUnit.Count; i++)
            {
                cmbHHSUnit.Items.Add(lstUnit[i - 1].Name.ToString());
                cmbHHSUnit.Items[i].Value = lstUnit[i - 1].ID.ToString();
            }
            cmbBAAUnit.Items.Clear();
            cmbBAAUnit.Items.Add("UnSpecified");
            cmbBAAUnit.Items[0].Value = "-1";
            for (int i = 1; i <= lstUnit.Count; i++)
            {
                cmbBAAUnit.Items.Add(lstUnit[i - 1].Name.ToString());
                cmbBAAUnit.Items[i].Value = lstUnit[i - 1].ID.ToString();
            }
            cmbIFCUnit.Items.Clear();
            cmbIFCUnit.Items.Add("UnSpecified");
            cmbIFCUnit.Items[0].Value = "-1";
            for (int i = 1; i <= lstUnit.Count; i++)
            {
                cmbIFCUnit.Items.Add(lstUnit[i - 1].Name.ToString());
                cmbIFCUnit.Items[i].Value = lstUnit[i - 1].ID.ToString();
            }
            cmbGFUnit.Items.Clear();
            cmbGFUnit.Items.Add("UnSpecified");
            cmbGFUnit.Items[0].Value = "-1";
            for (int i = 1; i <= lstUnit.Count; i++)
            {
                cmbGFUnit.Items.Add(lstUnit[i - 1].Name.ToString());
                cmbGFUnit.Items[i].Value = lstUnit[i - 1].ID.ToString();
            }
            cmbBUnit.Items.Clear();
            cmbBUnit.Items.Add("UnSpecified");
            cmbBUnit.Items[0].Value = "-1";
            for (int i = 1; i <= lstUnit.Count; i++)
            {
                cmbBUnit.Items.Add(lstUnit[i - 1].Name.ToString());
                cmbBUnit.Items[i].Value = lstUnit[i - 1].ID.ToString();
            }
            cmbOUnit.Items.Clear();
            cmbOUnit.Items.Add("UnSpecified");
            cmbOUnit.Items[0].Value = "-1";
            for (int i = 1; i <= lstUnit.Count; i++)
            {
                cmbOUnit.Items.Add(lstUnit[i - 1].Name.ToString());
                cmbOUnit.Items[i].Value = lstUnit[i - 1].ID.ToString();
            }
            List<CauseItem> lstCauses = dbMan.GetCauses();
            cmbCauses.Items.Clear();
            cmbCauses.Items.Add("UnSpecified");
            cmbCauses.Items[0].Value = "-1";
            for (int i = 1; i <= lstCauses.Count; i++)
            {
                cmbCauses.Items.Add(lstCauses[i - 1].Name.ToString());
                cmbCauses.Items[i].Value = lstCauses[i - 1].ID.ToString();
            }
            List<LevelofEducationItem> lstEducation = dbMan.GetLevelofEducation();

            cmbLevelEducation.Items.Clear();
            cmbLevelEducation.Items.Add("UnSpecified");
            cmbLevelEducation.Items[0].Value = "-1";
            for (int i = 1; i <= lstEducation.Count; i++)
            {
                cmbLevelEducation.Items.Add(lstEducation[i - 1].Name.ToString());
                cmbLevelEducation.Items[i].Value = lstEducation[i - 1].ID.ToString();
            }

            List<MaritalItem> lstMarital = dbMan.GetMaritalStatus();

            cmbMarital.Items.Clear();
            cmbMarital.Items.Add("UnSpecified");
            cmbMarital.Items[0].Value = "-1";
            for (int i = 1; i <= lstMarital.Count; i++)
            {
                cmbMarital.Items.Add(lstMarital[i - 1].Name.ToString());
                cmbMarital.Items[i].Value = lstMarital[i - 1].ID.ToString();
            }
            List<OccupationItem> lstOccupation = dbMan.GetOccupation();

            cmbOccupation.Items.Clear();
            cmbOccupation.Items.Add("UnSpecified");
            cmbOccupation.Items[0].Value = "-1";
            for (int i = 1; i <= lstOccupation.Count; i++)
            {
                cmbOccupation.Items.Add(lstOccupation[i - 1].Name.ToString());
                cmbOccupation.Items[i].Value = lstOccupation[i - 1].ID.ToString();
            }

            List<RoleItem> lstRole = dbMan.GetRoles();

            cmbRole.Items.Clear();
            cmbRole.Items.Add("UnSpecified");
            cmbRole.Items[0].Value = "-1";
            for (int i = 1; i <= lstRole.Count; i++)
            {
                cmbRole.Items.Add(lstRole[i - 1].Name.ToString());
                cmbRole.Items[i].Value = lstRole[i - 1].ID.ToString();
            }

            List<VulnerabilityItem> lstVul = dbMan.GetVulnerability();

            cmbVulnerability.Items.Clear();
            cmbVulnerability.Items.Add("UnSpecified");
            cmbVulnerability.Items[0].Value = "-1";
            for (int i = 1; i <= lstVul.Count; i++)
            {
                cmbVulnerability.Items.Add(lstVul[i - 1].Name.ToString());
                cmbVulnerability.Items[i].Value = lstVul[i - 1].ID.ToString();
            }

            List<CountryItem> lstCountries = dbMan.GetCountries();

            cmbPreCountry.Items.Clear();
            cmbPreCountry.Items.Add("UnSpecified");
            cmbPreCountry.Items[0].Value = "-1";

            cmbCountryBirth.Items.Clear();
            cmbCountryBirth.Items.Add("UnSpecified");
            cmbCountryBirth.Items[0].Value = "-1";
            for (int i = 1; i <= lstCountries.Count; i++)
            {
                cmbPreCountry.Items.Add(lstCountries[i - 1].Name.ToString());
                cmbPreCountry.Items[i].Value = lstCountries[i - 1].ID.ToString();

                cmbCountryBirth.Items.Add(lstCountries[i - 1].Name.ToString());
                cmbCountryBirth.Items[i].Value = lstCountries[i - 1].ID.ToString();
            }


            cmbPreDistrict.Items.Clear();
            cmbPreDistrict.Items.Add("UnSpecified");
            cmbPreDistrict.Items[0].Value = "-1";

            cmbPreWardno.Items.Clear();
            cmbPreWardno.Items.Add("UnSpecified");
            cmbPreWardno.Items[0].Value = "-1";

            List<CommunityItem> lstCommunity = dbMan.GetCommunity();

            cmbCommunity.Items.Clear();
            cmbCommunity.Items.Add("UnSpecified");
            cmbCommunity.Items[0].Value = "-1";

            cmbRenameCommunity.Items.Clear();
            cmbRenameCommunity.Items.Add("UnSpecified");
            cmbRenameCommunity.Items[0].Value = "-1";

            cmbRemoveCommunity.Items.Clear();
            cmbRemoveCommunity.Items.Add("UnSpecified");
            cmbRemoveCommunity.Items[0].Value = "-1";

            for (int i = 1; i <= lstCommunity.Count; i++)
            {
                cmbCommunity.Items.Add(lstCommunity[i - 1].Name.ToString());
                cmbCommunity.Items[i].Value = lstCommunity[i - 1].ID.ToString();

                cmbRenameCommunity.Items.Add(lstCommunity[i - 1].Name.ToString());
                cmbRenameCommunity.Items[i].Value = lstCommunity[i - 1].ID.ToString();

                cmbRemoveCommunity.Items.Add(lstCommunity[i - 1].Name.ToString());
                cmbRemoveCommunity.Items[i].Value = lstCommunity[i - 1].ID.ToString();
            }

            cmbHousehold.Items.Clear();
            cmbHousehold.Items.Add("UnSpecified");
            cmbHousehold.Items[0].Value = "-1";

            cmbRenameHousehold.Items.Clear();
            cmbRenameHousehold.Items.Add("UnSpecified");
            cmbRenameHousehold.Items[0].Value = "-1";

            cmbRemoveHousehold.Items.Clear();
            cmbRemoveHousehold.Items.Add("UnSpecified");
            cmbRemoveHousehold.Items[0].Value = "-1";

            txtPlaceName.Text = "";
            dtDateIncidentOccured.Text = "0";
            dtDateIncidentReported.Text = "0";

            txtNumberAffectedHH.Value = "";
            txtNumberAffectedIndividuals.Value = "";
            txtMales1.Value = "";
            txtFemales1.Value = "";
            txtDisplacedNoHH.Value = "";
            txtDisplacedNoIndividuals.Value = "";
            txtMales2.Value = "";
            txtFemales2.Value = "";

            txtAffectedPopulation11.Value = "";
            txtAffectedPopulation12.Value = "";
            txtAffectedPopulation13.Value = "";
            txtAffectedPopulation14.Value = "";
            //txtAffectedPopulation15.Value = "";
            //txtAffectedPopulation16.Value = "";

            txtAffectedPopulation21.Value = "";
            txtAffectedPopulation22.Value = "";
            txtAffectedPopulation23.Value = "";
            txtAffectedPopulation24.Value = "";
            //txtAffectedPopulation25.Value = "";
            //txtAffectedPopulation26.Value = "";

            txtAffectedPopulation31.Value = "";
            txtAffectedPopulation32.Value = "";
            txtAffectedPopulation33.Value = "";
            txtAffectedPopulation34.Value = "";
            //txtAffectedPopulation35.Value = "";
            //txtAffectedPopulation36.Value = "";

            txtAffectedPopulation41.Value = "";
            txtAffectedPopulation42.Value = "";
            txtAffectedPopulation43.Value = "";
            txtAffectedPopulation44.Value = "";
            //txtAffectedPopulation45.Value = "";
            //txtAffectedPopulation46.Value = "";

            txtAffectedPopulation51.Value = "";
            txtAffectedPopulation52.Value = "";
            txtAffectedPopulation53.Value = "";
            txtAffectedPopulation54.Value = "";
            //txtAffectedPopulation55.Value = "";
            //txtAffectedPopulation56.Value = "";

            txtAffectedPopulation61.Value = "";
            txtAffectedPopulation62.Value = "";
            txtAffectedPopulation63.Value = "";
            txtAffectedPopulation64.Value = "";
            //txtAffectedPopulation65.Value = "";
            //txtAffectedPopulation66.Value = "";

            txtTypeIncidentOther.Text = "";
            txtDisplacementStatusOther.Text = "";

            txtPregnant.Value = "";
            txtLactating.Value = "";
            txtOrphans.Value = "";
            txtChildheaded.Value = "";
            txtEderly.Value = "";
            txtDisabled.Value = "";
            txtChronicallyill.Value = "";

            txtAgegroup4.Value = "";
            txtAgegroup5.Value = "";
            txtAgegroup18.Value = "";
            txtAgegroup31.Value = "";
            txtAgegroup44.Value = "";
            txtAgegroup57.Value = "";
            txtAgegroup65.Value = "";

            chkNeedfood.Checked = false;
            chkNutrition.Checked = false;
            chkWater.Checked = false;
            chkSanitation.Checked = false;
            chkHygiene.Checked = false;
            chkShelter.Checked = false;
            chkEducation.Checked = false;
            chkHealth.Checked = false;
            taOther.Value = "";

            txtRHQty.Value = "";
            txtRHComment.Value = "";

            txtSSQty.Value = "";
            txtSSComment.Value = "";

            txtEPLQty.Value = "";
            txtEPLComment.Value = "";

            txtEPSQty.Value = "";
            txtEPSComment.Value = "";

            txtNGPPQty.Value = "";
            txtNGPPComment.Value = "";

            txtTLQty.Value = "";
            txtTLComment.Value = "";

            txtMPTQty.Value = "";
            txtMPTComment.Value = "";

            txtIFOLQty.Value = "";
            txtIFOLComment.Value = "";

            txtSCEQty.Value = "";
            txtSCEComment.Value = "";

            txtSCCQty.Value = "";
            txtSCCComment.Value = "";

            txtHHSQty.Value = "";
            txtHSSComment.Value = "";

            txtBAAQty.Value = "";
            txtBAAComment.Value = "";

            txtIFCQty.Value = "";
            txtIFCComment.Value = "";

            txtGFQty.Value = "";
            txtGFComment.Value = "";

            txtBQty.Value = "";
            txtBComment.Value = "";

            txtOQty.Value = "";
            txtOComment.Value = "";
            txtDIOther.Value = "";

            txtTypeLocation.Value = "";
            txtEast.Value = "";
            txtSouth.Value = "";

            chkSchools.Checked = false;
            chkHC.Checked = false;
            chkPS.Checked = false;
            chkWSSW.Checked = false;
            chkFP.Checked = false;
            chkAESOther.Checked = false;

            txtAESOther.Value = "";

            txtSchoolComment.Value = "";
            txtHCComment.Value = "";
            txtPSComment.Value = "";
            txtWSSWComment.Value = "";
            txtFPComment.Value = "";
            txtAESOtherComment.Value = "";

            chkRoad.Checked = false;
            chkAir.Checked = false;
            chkBoat.Checked = false;
            chkAPROther.Checked = false;
            txtAPROther.Value = "";

            txtComment.Value = "";

            chkSpecify.Checked = false;
            txtSpecify.Value = "";

            txtFKP.Value = "";
            txtCKP.Value = "";
            txtTM.Value = "";

            chkCaseClosed.Checked = false;
            txtCaseClosed.Value = "";
            txtORN.Value = "";
            txtPrePlaceName.Value = "";
            txtGPSEast.Value = "";
            txtGPSSouth.Value = "";

            txtAddCommunity.Value = "";
            txtRenameCommunity.Value = "";
            txtAddHousehold.Value = "";
            txtRenameHousehold.Value = "";

            ClearIndividuals();

        }

        protected void cmbPreProvince_SelectedIndexChanged(object sender, EventArgs e)
        {
            int provinceId = Convert.ToInt32(cmbPreProvince.SelectedValue);

            List<DistrictItem> lstDistrict = dbMan.GetDistrict(provinceId);
            cmbPreDistrict.Items.Clear();
            cmbPreDistrict.Items.Add("UnSpecified");
            cmbPreDistrict.Items[0].Value = "-1";
            for (int i = 1; i <= lstDistrict.Count; i++)
            {
                cmbPreDistrict.Items.Add(lstDistrict[i - 1].Name.ToString());
                cmbPreDistrict.Items[i].Value = lstDistrict[i - 1].ID.ToString();
            }
            cmbPreWardno.Items.Clear();
            cmbPreWardno.Items.Add("UnSpecified");
            cmbPreWardno.Items[0].Value = "-1";
        }

        protected void cmbPreDistrict_SelectedIndexChanged(object sender, EventArgs e)
        {
            int district_id = Convert.ToInt32(cmbPreDistrict.SelectedValue);

            List<WardnoItem> lstWardno = dbMan.GetWardno(district_id);
            cmbPreWardno.Items.Clear();
            cmbPreWardno.Items.Add("UnSpecified");
            cmbPreWardno.Items[0].Value = "-1";
            for (int i = 1; i <= lstWardno.Count; i++)
            {
                cmbPreWardno.Items.Add(lstWardno[i - 1].Name.ToString());
                cmbPreWardno.Items[i].Value = lstWardno[i - 1].ID.ToString();
            }
        }

        protected void cmbCommunity_SelectedIndexChanged(object sender, EventArgs e)
        {
            int communityId = Convert.ToInt32(cmbCommunity.SelectedValue);

            List<HouseholdItem> lstHousehold = dbMan.GetHousehold(communityId);
            cmbHousehold.Items.Clear();
            cmbHousehold.Items.Add("UnSpecified");
            cmbHousehold.Items[0].Value = "-1";

            cmbRenameHousehold.Items.Clear();
            cmbRenameHousehold.Items.Add("UnSpecified");
            cmbRenameHousehold.Items[0].Value = "-1";

            cmbRemoveHousehold.Items.Clear();
            cmbRemoveHousehold.Items.Add("UnSpecified");
            cmbRemoveHousehold.Items[0].Value = "-1";

            for (int i = 1; i <= lstHousehold.Count; i++)
            {
                cmbHousehold.Items.Add(lstHousehold[i - 1].Name.ToString());
                cmbHousehold.Items[i].Value = lstHousehold[i - 1].ID.ToString();

                cmbRenameHousehold.Items.Add(lstHousehold[i - 1].Name.ToString());
                cmbRenameHousehold.Items[i].Value = lstHousehold[i - 1].ID.ToString();

                cmbRemoveHousehold.Items.Add(lstHousehold[i - 1].Name.ToString());
                cmbRemoveHousehold.Items[i].Value = lstHousehold[i - 1].ID.ToString();
            }

            SqlDatabase.EnableCaching = false;
            grdIndividuals.DataBind();
            SqlDatabase.EnableCaching = true;
            ClearIndividuals();
        }

        protected void btnAddCommunity_Click(object sender, EventArgs e)
        {
            string community_name = txtAddCommunity.Value;

            int id = dbMan.InsertCommunity(community_name);

            cmbCommunity.Items.Add(community_name);
            cmbCommunity.Items[cmbCommunity.Items.Count - 1].Value = id.ToString();

            cmbRenameCommunity.Items.Add(community_name);
            cmbRenameCommunity.Items[cmbRenameCommunity.Items.Count - 1].Value = id.ToString();

            cmbRemoveCommunity.Items.Add(community_name);
            cmbRemoveCommunity.Items[cmbRemoveCommunity.Items.Count - 1].Value = id.ToString();
        }

        protected void btnRenameCommunity_Click(object sender, EventArgs e)
        {
            int communityId = Convert.ToInt32(cmbRenameCommunity.SelectedValue);
            string value = txtRenameCommunity.Value;


            dbMan.UpdateCommunity(communityId, value);

            for (int i = 0; i < cmbCommunity.Items.Count; i++)
            {
                if (cmbCommunity.Items[i].Value == communityId.ToString())
                    cmbCommunity.Items[i].Text = value;
            }

            for (int i = 0; i < cmbRenameCommunity.Items.Count; i++)
            {
                if (cmbRenameCommunity.Items[i].Value == communityId.ToString())
                    cmbRenameCommunity.Items[i].Text = value;
            }

            for (int i = 0; i < cmbRemoveCommunity.Items.Count; i++)
            {
                if (cmbRemoveCommunity.Items[i].Value == communityId.ToString())
                    cmbRemoveCommunity.Items[i].Text = value;
            }
        }

        protected void btnRemoveCommunity_Click(object sender, EventArgs e)
        {
            string community_id = cmbRemoveCommunity.SelectedValue;

            dbMan.DeleteItem("community", community_id);
            ListItem deleteItem = cmbRemoveCommunity.SelectedItem;
            cmbRemoveCommunity.Items.Remove(deleteItem);
            cmbRenameCommunity.Items.Remove(deleteItem);
            cmbCommunity.Items.Remove(deleteItem);
        }

        protected void cmbHousehold_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlDatabase.EnableCaching = false;
            grdIndividuals.DataBind();
            SqlDatabase.EnableCaching = true;
            ClearIndividuals();
        }

        protected void btnAddHousehold_Click(object sender, EventArgs e)
        {
            string household_name = txtAddHousehold.Value;
            string community_id = cmbCommunity.SelectedValue;

            int id = dbMan.InsertHousehold(Convert.ToInt32(community_id), household_name);

            cmbHousehold.Items.Add(household_name);
            cmbHousehold.Items[cmbHousehold.Items.Count - 1].Value = id.ToString();

            cmbRenameHousehold.Items.Add(household_name);
            cmbRenameHousehold.Items[cmbRenameHousehold.Items.Count - 1].Value = id.ToString();

            cmbRemoveHousehold.Items.Add(household_name);
            cmbRemoveHousehold.Items[cmbRemoveHousehold.Items.Count - 1].Value = id.ToString();
        }

        protected void btnRenameHousehold_Click(object sender, EventArgs e)
        {
            int householdId = Convert.ToInt32(cmbRenameHousehold.SelectedValue);
            string value = txtRenameHousehold.Value;


            dbMan.UpdateHousehold(householdId, value);

            for (int i = 0; i < cmbHousehold.Items.Count; i++)
            {
                if (cmbHousehold.Items[i].Value == householdId.ToString())
                    cmbHousehold.Items[i].Text = value;
            }

            for (int i = 0; i < cmbRenameHousehold.Items.Count; i++)
            {
                if (cmbRenameHousehold.Items[i].Value == householdId.ToString())
                    cmbRenameHousehold.Items[i].Text = value;
            }

            for (int i = 0; i < cmbRemoveHousehold.Items.Count; i++)
            {
                if (cmbRemoveHousehold.Items[i].Value == householdId.ToString())
                    cmbRemoveHousehold.Items[i].Text = value;
            }
        }

        protected void btnRemoveHousehold_Click(object sender, EventArgs e)
        {
            string household_id = cmbRemoveHousehold.SelectedValue;

            dbMan.DeleteItem("household", household_id);
            ListItem deleteItem = cmbRemoveHousehold.SelectedItem;
            cmbRemoveHousehold.Items.Remove(deleteItem);
            cmbRemoveHousehold.Items.Remove(deleteItem);
            cmbHousehold.Items.Remove(deleteItem);
        }

        protected void grdIndividuals_SelectedIndexChanged(object sender, EventArgs e)
        {
            //grdIndividuals.SelectRow(grdIndividuals.SelectedIndex);

            List<IndividualItem> items = dbMan.GetIndividualsById(Convert.ToInt32(grdIndividuals.SelectedValue));
            if (items.Count == 0)
                return;
            IndividualItem item = items[0];

            LoadIndividualData(item);
        }

        protected void LoadIndividualData(IndividualItem data)
        {
            txtFirstName.Value = data.FirstName;
            txtMiddleName.Value = data.MiddleName;
            txtLastName.Value = data.LastName;
            cmbGender.Value = data.Gender.ToString();
            txtDOB.Value = data.DOB;
            txtAge.Value = data.Age.ToString();
            cmbCountryBirth.SelectedValue = data.CountyOfBirth.ToString();
            txtPlaceBirth.Value = data.PlaceOfBirth;
            txtNationality.Value = data.Nationality;
            txtNationalId.Value = data.NationalID;
            txtContactPhone.Value = data.ContactPhone;
            txtPhysicalAddrss.Value = data.PhysicalAddress;
            txtEmail.Value = data.Email;
            cmbMarital.SelectedValue = data.MaritalStatus.ToString();
            cmbRole.SelectedValue = data.Role.ToString();
            chkIsBreadWinner.Checked = data.IsBreadWinner == 1;
            cmbLevelEducation.SelectedValue = data.LevelOfEducation.ToString();
            cmbOccupation.SelectedValue = data.Occupation.ToString();
            txtEmployer.Value = data.Employer;
            cmbDisplacementIndi.SelectedValue = data.DisplacementStatus.ToString();
            txtIndividualComment.Value = data.Comment;
            cmbVulnerability.SelectedValue = data.VulnerabilityLevel.ToString();

        }

        protected void grdIndividuals_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onclick"] =
                     Page.ClientScript.GetPostBackClientHyperlink(grdIndividuals, "Select$" + e.Row.RowIndex);
            }
        }

        protected void grdIndividuals_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (ViewState["PreviousRowIndex"] != null)
            {
                var previousRowIndex = (int)ViewState["PreviousRowIndex"];
                GridViewRow PreviousRow = grdIndividuals.Rows[previousRowIndex];
                PreviousRow.ForeColor = System.Drawing.Color.Black;
            }
            int currentRowIndex = Int32.Parse(e.CommandArgument.ToString());
            GridViewRow row = grdIndividuals.Rows[currentRowIndex];
            row.ForeColor = System.Drawing.Color.Blue;
            ViewState["PreviousRowIndex"] = currentRowIndex;
        }

        protected void btnIndividualSave_Click(object sender, EventArgs e)
        {
            if (grdIndividuals.SelectedIndex == -1)
            {
                List<string> fieldList = new List<string>();
                List<string> valueList = new List<string>();

                string household_id = cmbHousehold.SelectedValue;

                valueList.Add((dbMan.GetMaxIDFromTable("individuals") + 1).ToString());
                valueList.Add(household_id);
                valueList.Add(txtFirstName.Value);
                valueList.Add(txtMiddleName.Value);
                valueList.Add(txtLastName.Value);
                valueList.Add(cmbGender.Value);
                valueList.Add(txtDOB.Value);
                valueList.Add(txtAge.Value);
                valueList.Add(cmbCountryBirth.SelectedValue);
                valueList.Add(txtPlaceBirth.Value);
                valueList.Add(txtNationality.Value);
                valueList.Add(txtNationalId.Value);
                valueList.Add(txtContactPhone.Value);
                valueList.Add(txtPhysicalAddrss.Value);
                valueList.Add(txtEmail.Value);
                valueList.Add(cmbMarital.SelectedValue);
                valueList.Add(cmbRole.SelectedValue);
                valueList.Add(chkIsBreadWinner.Checked ? "1" : "0");
                valueList.Add(cmbLevelEducation.SelectedValue);
                valueList.Add(cmbOccupation.SelectedValue);
                valueList.Add(txtEmployer.Value);
                valueList.Add(txtIndividualComment.Value);
                valueList.Add(cmbDisplacementIndi.SelectedValue);
                valueList.Add(cmbVulnerability.SelectedValue);
                fieldList.Add("id");
                fieldList.Add("household_id");
                fieldList.Add("first_name");
                fieldList.Add("middle_name");
                fieldList.Add("last_name");
                fieldList.Add("gender");
                fieldList.Add("dob");
                fieldList.Add("age");
                fieldList.Add("country_of_birth");
                fieldList.Add("place_of_birth");
                fieldList.Add("nationality");
                fieldList.Add("national_id");
                fieldList.Add("contact_phone");
                fieldList.Add("physical_address");
                fieldList.Add("email");
                fieldList.Add("marital_status");
                fieldList.Add("role");
                fieldList.Add("is_bread_winner");
                fieldList.Add("level_of_education");
                fieldList.Add("occupation");
                fieldList.Add("employer");
                fieldList.Add("comment");
                fieldList.Add("displacement_status");
                fieldList.Add("vulnerability_level");

                dbMan.InsertIntoTable("individuals", fieldList, valueList);
            }
            else
            {
                List<string> fieldList = new List<string>();
                List<string> valueList = new List<string>();


                valueList.Add(txtFirstName.Value);
                valueList.Add(txtMiddleName.Value);
                valueList.Add(txtLastName.Value);
                valueList.Add(cmbGender.Value);
                valueList.Add(txtDOB.Value);
                valueList.Add(txtAge.Value);
                valueList.Add(cmbCountryBirth.SelectedValue);
                valueList.Add(txtPlaceBirth.Value);
                valueList.Add(txtNationality.Value);
                valueList.Add(txtNationalId.Value);
                valueList.Add(txtContactPhone.Value);
                valueList.Add(txtPhysicalAddrss.Value);
                valueList.Add(txtEmail.Value);
                valueList.Add(cmbMarital.SelectedValue);
                valueList.Add(cmbRole.SelectedValue);
                valueList.Add(chkIsBreadWinner.Checked ? "1" : "0");
                valueList.Add(cmbLevelEducation.SelectedValue);
                valueList.Add(cmbOccupation.SelectedValue);
                valueList.Add(txtEmployer.Value);
                valueList.Add(txtIndividualComment.Value);
                valueList.Add(cmbDisplacementIndi.SelectedValue);
                valueList.Add(cmbVulnerability.SelectedValue);

                fieldList.Add("first_name");
                fieldList.Add("middle_name");
                fieldList.Add("last_name");
                fieldList.Add("gender");
                fieldList.Add("dob");
                fieldList.Add("age");
                fieldList.Add("country_of_birth");
                fieldList.Add("place_of_birth");
                fieldList.Add("nationality");
                fieldList.Add("national_id");
                fieldList.Add("contact_phone");
                fieldList.Add("physical_address");
                fieldList.Add("email");
                fieldList.Add("marital_status");
                fieldList.Add("role");
                fieldList.Add("is_bread_winner");
                fieldList.Add("level_of_education");
                fieldList.Add("occupation");
                fieldList.Add("employer");
                fieldList.Add("comment");
                fieldList.Add("displacement_status");
                fieldList.Add("vulnerability_level");

                string individual_id = grdIndividuals.SelectedValue.ToString();
                dbMan.UpdateTable("individuals", individual_id, fieldList, valueList);
            }
            SqlDatabase.EnableCaching = false;
            grdIndividuals.DataBind();
            SqlDatabase.EnableCaching = true;
        }

        protected void btnIndividualClear_Click(object sender, EventArgs e)
        {
            ClearIndividuals();
        }

        protected void ClearIndividuals()
        {
            IndividualItem data = new IndividualItem();
            grdIndividuals.SelectedIndex = -1;
            txtFirstName.Value = data.FirstName;
            txtMiddleName.Value = data.MiddleName;
            txtLastName.Value = data.LastName;
            cmbGender.Value = data.Gender.ToString();
            txtDOB.Value = data.DOB;
            txtAge.Value = data.Age.ToString();
            cmbCountryBirth.SelectedValue = "-1";
            txtPlaceBirth.Value = data.PlaceOfBirth;
            txtNationality.Value = data.Nationality;
            txtNationalId.Value = data.NationalID;
            txtContactPhone.Value = data.ContactPhone;
            txtPhysicalAddrss.Value = data.PhysicalAddress;
            txtEmail.Value = data.Email;
            cmbMarital.SelectedValue = "-1";
            cmbRole.SelectedValue = "-1";
            chkIsBreadWinner.Checked = data.IsBreadWinner == 1;
            cmbLevelEducation.SelectedValue = "-1";
            cmbOccupation.SelectedValue = "-1";
            txtEmployer.Value = data.Employer;
            cmbDisplacementIndi.SelectedValue = "-1";
            cmbVulnerability.SelectedValue = "-1";
            txtIndividualComment.Value = "";

            if (ViewState["PreviousRowIndex"] != null && grdIndividuals.Rows.Count > 0)
            {
                var previousRowIndex = (int)ViewState["PreviousRowIndex"];
                GridViewRow PreviousRow = grdIndividuals.Rows[previousRowIndex];
                PreviousRow.ForeColor = System.Drawing.Color.Black;
            }
        }

        protected void btnIndividualDelete_Click(object sender, EventArgs e)
        {
            string individual_id = grdIndividuals.SelectedValue.ToString();

            dbMan.DeleteItem("individuals", individual_id);

            SqlDatabase.EnableCaching = false;
            grdIndividuals.DataBind();
            SqlDatabase.EnableCaching = true;

            ClearIndividuals();
        }

        private void OnEditCases(int caseId)
        {
            var result = dbMan.GetMainInformation(caseId);
            #region Base Information
            sltOrgan.SelectedValue = result.OrganizationId;
            ProvinceSelectedIndex(result.ProvinceId);
            sltProvince.SelectedValue = result.ProvinceId;
            DistrictSelectedIndex(result.DistrictId);
            sltDistrict.SelectedValue = result.DistrictId;
            LLGSelectedIndex(result.LlgId);
            sltLLG.SelectedValue = result.LlgId;
            sltWardno.SelectedValue = result.WardnoId;
            txtPlaceName.Text = result.PlaceName;

            dtDateIncidentOccured.Text = result.IncidentOccured.ToString("yyyy-MM-dd");
            dtDateIncidentReported.Text = result.IncidentReported.ToString("yyyy-MM-dd");
            lblCaseNo.Text = result.CaseNo;
            #endregion

            #region Affected Population Information
            var affected = dbMan.GetAffectedPopulation(caseId);
            //Affected Numbers
            #region Affected Numbers
            hdnaffectedpopulation.Value = Convert.ToString(affected["id"]);
            txtNumberAffectedHH.Value = Convert.ToString(affected["number_affected_hh"]);
            txtNumberAffectedIndividuals.Value = Convert.ToString(affected["number_affected_individuals"]);
            txtMales1.Value = Convert.ToString(affected["males1"]);
            txtFemales1.Value = Convert.ToString(affected["females1"]);
            txtDisplacedNoHH.Value = Convert.ToString(affected["displaced_hh"]);
            txtDisplacedNoIndividuals.Value = Convert.ToString(affected["displaced_individuals"]);
            txtMales2.Value = Convert.ToString(affected["males2"]);
            txtFemales2.Value = Convert.ToString(affected["females2"]);
            #endregion

            //Affected Populations
            #region Affected Populations
            txtAffectedPopulation11.Value = Convert.ToString(affected["affected_population_11"]);
            txtAffectedPopulation12.Value = Convert.ToString(affected["affected_population_12"]);
            txtAffectedPopulation13.Value = Convert.ToString(affected["affected_population_13"]);
            txtAffectedPopulation14.Value = Convert.ToString(affected["affected_population_14"]);
            //txtAffectedPopulation15.Value = Convert.ToString(affected["affected_population_15"]);
            //txtAffectedPopulation16.Value = Convert.ToString(affected["affected_population_16"]);

            txtAffectedPopulation21.Value = Convert.ToString(affected["affected_population_21"]);
            txtAffectedPopulation22.Value = Convert.ToString(affected["affected_population_22"]);
            txtAffectedPopulation23.Value = Convert.ToString(affected["affected_population_23"]);
            txtAffectedPopulation24.Value = Convert.ToString(affected["affected_population_24"]);
            //txtAffectedPopulation25.Value = Convert.ToString(affected["affected_population_25"]);
            //txtAffectedPopulation26.Value = Convert.ToString(affected["affected_population_26"]);

            txtAffectedPopulation31.Value = Convert.ToString(affected["affected_population_31"]);
            txtAffectedPopulation32.Value = Convert.ToString(affected["affected_population_32"]);
            txtAffectedPopulation33.Value = Convert.ToString(affected["affected_population_33"]);
            txtAffectedPopulation34.Value = Convert.ToString(affected["affected_population_34"]);
            //txtAffectedPopulation35.Value = Convert.ToString(affected["affected_population_35"]);
            //txtAffectedPopulation36.Value = Convert.ToString(affected["affected_population_36"]);

            txtAffectedPopulation41.Value = Convert.ToString(affected["affected_population_41"]);
            txtAffectedPopulation42.Value = Convert.ToString(affected["affected_population_42"]);
            txtAffectedPopulation43.Value = Convert.ToString(affected["affected_population_43"]);
            txtAffectedPopulation44.Value = Convert.ToString(affected["affected_population_44"]);
            //txtAffectedPopulation45.Value = Convert.ToString(affected["affected_population_45"]);
            //txtAffectedPopulation46.Value = Convert.ToString(affected["affected_population_46"]);

            txtAffectedPopulation51.Value = Convert.ToString(affected["affected_population_51"]);
            txtAffectedPopulation52.Value = Convert.ToString(affected["affected_population_52"]);
            txtAffectedPopulation53.Value = Convert.ToString(affected["affected_population_53"]);
            txtAffectedPopulation54.Value = Convert.ToString(affected["affected_population_54"]);
            //txtAffectedPopulation55.Value = Convert.ToString(affected["affected_population_55"]);
            //txtAffectedPopulation56.Value = Convert.ToString(affected["affected_population_56"]);

            txtAffectedPopulation61.Value = Convert.ToString(affected["affected_population_61"]);
            txtAffectedPopulation62.Value = Convert.ToString(affected["affected_population_62"]);
            txtAffectedPopulation63.Value = Convert.ToString(affected["affected_population_63"]);
            txtAffectedPopulation64.Value = Convert.ToString(affected["affected_population_64"]);
            //txtAffectedPopulation65.Value = Convert.ToString(affected["affected_population_65"]);
            //txtAffectedPopulation66.Value = Convert.ToString(affected["affected_population_66"]);

            txtAffectedPopulationOther.Value = Convert.ToString(affected["affected_population_other"]);

            sltTypeIncident.SelectedValue = Convert.ToString(affected["incident_id"]);
            txtTypeIncidentOther.Text = Convert.ToString(affected["incident_other"]);
            sltDisplacementStatus.SelectedValue = Convert.ToString(affected["displacement_id"]);
            txtDisplacementStatusOther.Text = Convert.ToString(affected["displacement_other"]);

            txtPregnant.Value = Convert.ToString(affected["pregnant"]);
            txtLactating.Value = Convert.ToString(affected["lactating"]);
            txtOrphans.Value = Convert.ToString(affected["orphans"]);
            txtChildheaded.Value = Convert.ToString(affected["childheaded"]);
            txtEderly.Value = Convert.ToString(affected["ederly"]);
            txtDisabled.Value = Convert.ToString(affected["disabled"]);
            txtChronicallyill.Value = Convert.ToString(affected["chronicallyill"]);

            txtAgegroup4.Value = Convert.ToString(affected["agegroup4"]);
            txtAgegroup5.Value = Convert.ToString(affected["agegroup5"]);
            txtAgegroup18.Value = Convert.ToString(affected["agegroup18"]);
            txtAgegroup31.Value = Convert.ToString(affected["agegroup31"]);
            txtAgegroup44.Value = Convert.ToString(affected["agegroup44"]);
            txtAgegroup57.Value = Convert.ToString(affected["agegroup57"]);
            txtAgegroup65.Value = Convert.ToString(affected["agegroup65"]);

            chkEducation.Checked = Convert.ToBoolean(affected["foodneed"]);
            chkNutrition.Checked = Convert.ToBoolean(affected["nutrition"]);
            chkWater.Checked = Convert.ToBoolean(affected["water"]);
            chkSanitation.Checked = Convert.ToBoolean(affected["sanitation"]);
            chkHygiene.Checked = Convert.ToBoolean(affected["hygiene"]);
            chkShelter.Checked = Convert.ToBoolean(affected["shelter"]);
            chkEducation.Checked = Convert.ToBoolean(affected["education"]);
            chkHealth.Checked = Convert.ToBoolean(affected["health"]);
            taOther.Value = Convert.ToString(affected["checkgroup_other"]);
            #endregion
            #endregion

            #region Damaged Infrastructure
            var damaged = dbMan.GetDamagedInfrastructure(caseId);
            hdndamagedInfrastructure.Value = Convert.ToString(damaged["id"]);

            txtRHQty.Value = Convert.ToString(damaged["rh_qty"]);
            cmbRHUnit.SelectedValue = Convert.ToString(damaged["rh_unit"]);
            txtRHComment.Value = Convert.ToString(damaged["rh_comment"]);

            txtSSQty.Value = Convert.ToString(damaged["ss_qty"]);
            cmbSSUnit.SelectedValue = Convert.ToString(damaged["ss_unit"]);
            txtSSComment.Value = Convert.ToString(damaged["ss_comment"]);

            txtEPLQty.Value = Convert.ToString(damaged["epl_qty"]);
            cmbEPLUnit.SelectedValue = Convert.ToString(damaged["epl_unit"]);
            txtEPLComment.Value = Convert.ToString(damaged["epl_comment"]);

            txtEPSQty.Value = Convert.ToString(damaged["eps_qty"]);
            cmbEPSUnit.SelectedValue = Convert.ToString(damaged["eps_unit"]);
            txtEPSComment.Value = Convert.ToString(damaged["eps_comment"]);

            txtNGPPQty.Value = Convert.ToString(damaged["ngpp_qty"]);
            cmbNGPPUnit.SelectedValue = Convert.ToString(damaged["ngpp_unit"]);
            txtNGPPComment.Value = Convert.ToString(damaged["ngpp_comment"]);

            txtTLQty.Value = Convert.ToString(damaged["tl_qty"]);
            cmbTLUnit.SelectedValue = Convert.ToString(damaged["tl_unit"]);
            txtTLComment.Value = Convert.ToString(damaged["tl_comment"]);

            txtMPTQty.Value = Convert.ToString(damaged["mpt_qty"]);
            cmbMPTUnit.SelectedValue = Convert.ToString(damaged["mpt_unit"]);
            txtMPTComment.Value = Convert.ToString(damaged["mpt_comment"]);

            txtIFOLQty.Value = Convert.ToString(damaged["ifol_qty"]);
            cmbIFOLUnit.SelectedValue = Convert.ToString(damaged["ifol_unit"]);
            txtIFOLComment.Value = Convert.ToString(damaged["ifol_comment"]);

            txtSCEQty.Value = Convert.ToString(damaged["sce_qty"]);
            cmbSCEUnit.SelectedValue = Convert.ToString(damaged["sce_unit"]);
            txtSCEComment.Value = Convert.ToString(damaged["sce_comment"]);

            txtSCCQty.Value = Convert.ToString(damaged["scc_qty"]);
            cmbSCCUnit.SelectedValue = Convert.ToString(damaged["scc_unit"]);
            txtSCCComment.Value = Convert.ToString(damaged["scc_comment"]);

            txtHHSQty.Value = Convert.ToString(damaged["hhs_qty"]);
            cmbHHSUnit.SelectedValue = Convert.ToString(damaged["hhs_unit"]);
            txtHSSComment.Value = Convert.ToString(damaged["hhs_comment"]);

            txtBAAQty.Value = Convert.ToString(damaged["ba_qty"]);
            cmbBAAUnit.SelectedValue = Convert.ToString(damaged["ba_unit"]);
            txtBAAComment.Value = Convert.ToString(damaged["ba_comment"]);

            txtIFCQty.Value = Convert.ToString(damaged["ifc_qty"]);
            cmbIFCUnit.SelectedValue = Convert.ToString(damaged["ifc_unit"]);
            txtIFCComment.Value = Convert.ToString(damaged["ifc_comment"]);

            txtGFQty.Value = Convert.ToString(damaged["gf_qty"]);
            cmbGFUnit.SelectedValue = Convert.ToString(damaged["gf_unit"]);
            txtGFComment.Value = Convert.ToString(damaged["gf_comment"]);

            txtBQty.Value = Convert.ToString(damaged["b_qty"]);
            cmbBUnit.SelectedValue = Convert.ToString(damaged["b_unit"]);
            txtBComment.Value = Convert.ToString(damaged["b_comment"]);

            txtDIOther.Value = Convert.ToString(damaged["other_caption"]);
            txtOQty.Value = Convert.ToString(damaged["other_qty"]);
            cmbOUnit.SelectedValue = Convert.ToString(damaged["other_unit"]);
            txtOComment.Value = Convert.ToString(damaged["other_comment"]);

            txtTypeLocation.Value = Convert.ToString(damaged["type_location"]);
            txtSouth.Value = Convert.ToString(damaged["gps_south"]);
            txtEast.Value = Convert.ToString(damaged["gps_east"]);
            #endregion

            #region Essential Services
            var essential = dbMan.GetEssentialServices(caseId);
            hdnEssentialServices.Value = Convert.ToString(essential["id"]);

            chkSchools.Checked = Convert.ToBoolean(essential["school_chk"]);
            txtSchoolComment.Value = Convert.ToString(essential["school_comment"]);

            chkHC.Checked = Convert.ToBoolean(essential["hospital_chk"]);
            txtHCComment.Value = Convert.ToString(essential["hospital_comment"]);

            chkPS.Checked = Convert.ToBoolean(essential["policing_chk"]);
            txtPSComment.Value = Convert.ToString(essential["policing_chk"]);

            chkWSSW.Checked = Convert.ToBoolean(essential["water_chk"]);
            txtWSSWComment.Value = Convert.ToString(essential["water_comment"]);

            chkFP.Checked = Convert.ToBoolean(essential["food_chk"]);
            txtFPComment.Value = Convert.ToString(essential["food_comment"]);

            txtAESOther.Value = Convert.ToString(essential["other_caption"]);
            chkAESOther.Checked = Convert.ToBoolean(essential["other_chk"]);
            txtAESOtherComment.Value = Convert.ToString(essential["other_comment"]);

            chkRoad.Checked = Convert.ToBoolean(essential["byroad_chk"]);
            chkAir.Checked = Convert.ToBoolean(essential["byair_chk"]);
            chkBoat.Checked = Convert.ToBoolean(essential["byboat_chk"]);
            chkAPROther.Checked = Convert.ToBoolean(essential["aprother_chk"]);
            txtAPROther.Value = Convert.ToString(essential["aprother_comment"]);

            txtComment.Value = Convert.ToString(essential["comment"]);
            #endregion

            // Causes
            #region Cause Info
            var caseInfo = dbMan.GetCausesInfo(caseId);
            hdnCauses.Value = Convert.ToString(caseInfo["id"]);
            cmbCauses.SelectedValue = Convert.ToString(caseInfo["cause_id"]);
            chkSpecify.Checked = Convert.ToString(caseInfo["specify_chk"]) == "1";
            txtSpecify.Value = Convert.ToString(caseInfo["specify_comment"]);

            txtFKP.Value = Convert.ToString(caseInfo["gfkp_comment"]);
            txtCKP.Value = Convert.ToString(caseInfo["ckp_comment"]);
            txtTM.Value = Convert.ToString(caseInfo["gtm_comment"]);

            chkCaseClosed.Checked = Convert.ToString(caseInfo["caseclosed_chk"]) == "1";
            txtCaseClosed.Value = Convert.ToString(caseInfo["caseclosed_comment"]);
            txtORN.Value = Convert.ToString(caseInfo["orn_comment"]);
            #endregion

            //Previous Location
            #region Previous Location
            var pLocation = dbMan.GetPreviousLocation(caseId);
            hdnPreviousLocation.Value = Convert.ToString(pLocation["id"]);

            cmbPreProvince.SelectedValue = Convert.ToString(pLocation["province_id"]);
            cmbPreDistrict.SelectedValue = Convert.ToString(pLocation["district_id"]);
            cmbPreWardno.SelectedValue = Convert.ToString(pLocation["wardno_id"]);
            cmbPreCountry.SelectedValue = Convert.ToString(pLocation["country_id"]);
            txtPrePlaceName.Value = Convert.ToString(pLocation["placename"]);
            txtGPSSouth.Value = Convert.ToString(pLocation["gps_south"]);
            txtGPSEast.Value = Convert.ToString(pLocation["gps_east"]);
            #endregion

            //affected groups
            #region Affected Groups
            var aGroup = dbMan.GetAffectedGroup(caseId);
            hdnAffectedGroups.Value = Convert.ToString(aGroup["id"]);
            cmbCommunity.SelectedValue = Convert.ToString(aGroup["community_id"]);

            int communityId = Convert.ToInt32(cmbCommunity.SelectedValue);

            List<HouseholdItem> lstHousehold = dbMan.GetHousehold(communityId);
            cmbHousehold.Items.Clear();
            cmbHousehold.Items.Add("UnSpecified");
            cmbHousehold.Items[0].Value = "-1";

            cmbRenameHousehold.Items.Clear();
            cmbRenameHousehold.Items.Add("UnSpecified");
            cmbRenameHousehold.Items[0].Value = "-1";

            cmbRemoveHousehold.Items.Clear();
            cmbRemoveHousehold.Items.Add("UnSpecified");
            cmbRemoveHousehold.Items[0].Value = "-1";

            for (int i = 1; i <= lstHousehold.Count; i++)
            {
                cmbHousehold.Items.Add(lstHousehold[i - 1].Name.ToString());
                cmbHousehold.Items[i].Value = lstHousehold[i - 1].ID.ToString();

                cmbRenameHousehold.Items.Add(lstHousehold[i - 1].Name.ToString());
                cmbRenameHousehold.Items[i].Value = lstHousehold[i - 1].ID.ToString();

                cmbRemoveHousehold.Items.Add(lstHousehold[i - 1].Name.ToString());
                cmbRemoveHousehold.Items[i].Value = lstHousehold[i - 1].ID.ToString();
            }

            SqlDatabase.EnableCaching = false;
            grdIndividuals.DataBind();
            SqlDatabase.EnableCaching = true;
            ClearIndividuals();

            cmbHousehold.SelectedValue = Convert.ToString(aGroup["household_id"]);
            //grdIndividuals.SelectedValue = Convert.ToString(aGroup["individual_id"]);

            #endregion

            #region Assistance Grid
            gvAssistance.DataSource = dbMan.GetAssistance(caseId);
            gvAssistance.DataBind();
            #endregion
        }

        private void ProvinceSelectedIndex(string provinceId)
        {
            List<DistrictItem> lstDistrict = dbMan.GetDistrict(Convert.ToInt32(provinceId));
            sltDistrict.Items.Clear();
            sltDistrict.Items.Add("UnSpecified");
            sltDistrict.Items[0].Value = "-1";
            for (int i = 1; i <= lstDistrict.Count; i++)
            {
                sltDistrict.Items.Add(lstDistrict[i - 1].Name.ToString());
                sltDistrict.Items[i].Value = lstDistrict[i - 1].ID.ToString();
            }
            sltLLG.Items.Clear();
            sltLLG.Items.Add("UnSpecified");
            sltLLG.Items[0].Value = "-1";
            sltWardno.Items.Clear();
            sltWardno.Items.Add("UnSpecified");
            sltWardno.Items[0].Value = "-1";

            string zero4 = "0000";
            string id = string.Empty;
            if (hdnMainId.Value == "0")
                id = (dbMan.GetMaxIDFromTable("main_information") + 1).ToString();
            else
                id = hdnMainId.Value;
            id = zero4.Substring(0, 4 - id.Length) + id;
            List<ProvinceItem> lstProvince = dbMan.GetProvince(Convert.ToInt32(sltProvince.SelectedValue));
            string provinceCode = "";
            if (lstProvince.Count > 0)
                provinceCode = lstProvince[0].Code;
            lblCaseNo.Text = provinceCode + "/" + DateTime.Now.ToString("MM/yyyy") + "/" + id;
        }

        private void DistrictSelectedIndex(string district_id)
        {
            List<LlgItem> llgItems = dbMan.GetLLg(Convert.ToInt32(district_id));
            sltLLG.Items.Clear();
            sltLLG.Items.Add("UnSpecified");
            sltLLG.Items[0].Value = "-1";
            for (int i = 1; i <= llgItems.Count; i++)
            {
                sltLLG.Items.Add(llgItems[i - 1].Name.ToString());
                sltLLG.Items[i].Value = llgItems[i - 1].ID.ToString();
            }

        }

        private void LLGSelectedIndex(string llg_id)
        {
            List<WardnoItem> lstWardno = dbMan.GetWardno(Convert.ToInt32(llg_id));
            sltWardno.Items.Clear();
            sltWardno.Items.Add("UnSpecified");
            sltWardno.Items[0].Value = "-1";
            for (int i = 1; i <= lstWardno.Count; i++)
            {
                sltWardno.Items.Add(lstWardno[i - 1].Name.ToString());
                sltWardno.Items[i].Value = lstWardno[i - 1].ID.ToString();
            }
        }

        [WebMethod(EnableSession = true)]
        public static string SaveAssistance(AssistanceModel model)
        {
            //dbMan.InsertOrUpdateAssistance("0", lblCaseNo.Text, txtADate.Text, ddlAssistanceType.SelectedValue, txtABeneficiaries.Text, txtAComment.Text);
            //ClientScript.RegisterStartupScript(this.GetType(), "closeModal", "closeModal();", true);
            DBMangement dbMan = new DBMangement();
            int result = dbMan.InsertOrUpdateAssistance(model.Id, model.CaseNo, model.AssistanceDate, model.AssistanceTypeId, model.Beneficiaries, model.Comment);
            return JsonConvert.SerializeObject(new JsonResponse() { Status = result > 0 ? 1 : 0 });
        }

        protected void btnAssistanceSave_Click(object sender, EventArgs e)
        {
            assId.Value = assId.Value == "" ? "0" : assId.Value;
            dbMan.InsertOrUpdateAssistance(Convert.ToInt32(assId.Value),
                lblCaseNo.Text,
                txtADate.Text,
                Convert.ToInt32(ddlAssistanceType.SelectedValue),
                Convert.ToInt32(txtABeneficiaries.Text),
                txtAComment.Text);
            ClientScript.RegisterStartupScript(this.GetType(), "closeModal", "closeModal();", true);
        }

        protected void gvAssistance_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvAssistance.EditIndex = -1;
        }

        protected void gvAssistance_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvAssistance.EditIndex = e.NewEditIndex;
            popup.Show();

        }

        protected void gvAssistance_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int userid = Convert.ToInt32(gvAssistance.DataKeys[e.RowIndex].Value.ToString());
            GridViewRow row = gvAssistance.Rows[e.RowIndex];
            var dropDownList = (DropDownList)row.FindControl("ddlAssistanceType1");
            Label lblID = (Label)row.FindControl("lblID");
            //TextBox txtname=(TextBox)gr.cell[].control[];  
            TextBox textName = (TextBox)row.Cells[0].Controls[0];
            TextBox textadd = (TextBox)row.Cells[1].Controls[0];
            TextBox textc = (TextBox)row.Cells[2].Controls[0];
            //TextBox textadd = (TextBox)row.FindControl("txtadd");  
            //TextBox textc = (TextBox)row.FindControl("txtc");  
            gvAssistance.EditIndex = -1;

        }

        protected void gvAssistance_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridViewRow row = gvAssistance.Rows[e.RowIndex];
            Label lbldeleteid = (Label)row.FindControl("lblID");
            //Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value.ToString());
        }

        protected void gvAssistance_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    DropDownList ddlAssistanceType1 = (e.Row.FindControl("ddlAssistanceType1") as DropDownList);

            //    if (ddlAssistanceType1.Items.Count == 0)
            //    {
            //        ddlAssistanceType1.Items.Add("UnSpecified");
            //        ddlAssistanceType1.Items[0].Value = "-1";
            //        foreach (var item in dbMan.GetAssistanceType())
            //        {
            //            ddlAssistanceType1.Items.Add(item.AssistanceTypeName);
            //            ddlAssistanceType1.Items[ddlAssistanceType1.Items.Count - 1].Value = item.Id.ToString();
            //        }
            //    }

            //    ddlAssistanceType1.DataBind();
            //    string country = (e.Row.FindControl("gvlblAssistanceTypeName") as Label).Text;
            //    ddlAssistanceType1.Items.FindByValue("ddlAssistanceType1").Selected = true;
            //}
        }

        protected void Add(object sender, EventArgs e)
        {
            PophdnassId.Value = "0";
            PoplblAssistanceCNo.Text = lblCaseNo.Text;
            PopddlAssistanceType.SelectedValue = "-1";
            PoptxtADate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            PoptxtABeneficiaries.Text = "";
            PoptxtAComment.Text = "";
            popup.Show();
        }

        protected void btnPopupSave_Click(object sender, EventArgs e)
        {
            PophdnassId.Value = PophdnassId.Value == "" ? "0" : PophdnassId.Value;
            dbMan.InsertOrUpdateAssistance(Convert.ToInt32(PophdnassId.Value),
                PoplblAssistanceCNo.Text,
                PoptxtADate.Text,
                Convert.ToInt32(PopddlAssistanceType.SelectedValue),
                Convert.ToInt32(PoptxtABeneficiaries.Text),
                PoptxtAComment.Text);

            string caseId = Request.QueryString["id"];

            gvAssistance.DataSource = dbMan.GetAssistance(Convert.ToInt32(caseId));
            gvAssistance.DataBind();

            popup.Hide();
        }

        protected void btnPopupCancel_Click(object sender, EventArgs e)
        {
            popup.Hide();
        }

        protected void lnkEdit_Click(object sender, EventArgs e)
        {
            LinkButton lb = (LinkButton)sender;
            GridViewRow row = (GridViewRow)lb.NamingContainer;
            if (row != null)
            {
                //int index = row.RowIndex; //gets the row index selected
                PophdnassId.Value = (row.FindControl("gvlblId") as Label).Text;
                PoplblAssistanceCNo.Text = (row.FindControl("gvlblCaseno") as Label).Text;
                PopddlAssistanceType.SelectedValue = (row.FindControl("AssistanceType") as Label).Text;

                DateTime date = DateTime.ParseExact((row.FindControl("gvlbltxtDate") as Label).Text, "MM/dd/yyyy", null);
                PoptxtADate.Text = date.ToString("yyyy-MM-dd");

                PoptxtABeneficiaries.Text = (row.FindControl("gvlblBeneficiaries") as Label).Text;
                PoptxtAComment.Text = (row.FindControl("gvlblComments") as Label).Text;
            }
            popup.Show();
        }
    }
}