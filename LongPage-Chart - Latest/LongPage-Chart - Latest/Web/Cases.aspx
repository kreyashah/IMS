<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Cases.aspx.cs" Inherits="Web.CasesManagement" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        .mrg-b{
            margin-bottom:10px;
        }
        input[type="date"]::before {
            color: #ffffff;
            content: attr(placeholder) ": ";
        }

        input[type="date"]:focus::before {
            content: "" !important;
        }

        .modalBackground {
            background-color: Black;
            filter: alpha(opacity=90);
            opacity: 0.8;
        }

        .modalPopup {
            background-color: #FFFFFF;
            border-width: 3px;
            border-style: solid;
            /*border-color: black;*/
            padding-top: 10px;
            padding-left: 10px;
            width: 600px;
            height: 450px;
        }
    </style>
    <div class="control-group">
    </div>
    <div class="control-group no-line">
        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary btn-xs" Text="Save" OnClick="btnSave_Click" />
        <asp:Button ID="btnNew" runat="server" CssClass="btn btn-primary btn-xs" Text="New" OnClick="btnNew_Click" CausesValidation="False" />
        <asp:HiddenField ID="hdnMainId" runat="server" />
    </div>

    <hr>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="control-group no-line">
                <div class="half-line">
                    <div class="my-md-4">
                        <label class="font-9">Case No.</label>
                    </div>
                    <div class="my-md-4">
                        <asp:Label ID="lblCaseNo" runat="server" Text="GE/08/2019/0001"></asp:Label>
                    </div>
                </div>
                <div class="half-line">
                    <div class="my-md-4">
                        <label class="font-9">Organization</label>
                    </div>
                    <div class="my-md-8">
                        <asp:DropDownList ID="sltOrgan" runat="server">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator runat="server" ID="reqOrgan" InitialValue="-1" ControlToValidate="sltOrgan" ErrorMessage="<span  class='required_alert'>*</span>" />
                    </div>
                </div>
                <div class="clearfix"></div>
            </div>
            <div class="control-group no-line">
                <div class="half-line">
                    <div class="my-md-4">
                        <label class="font-9">Province (Incident Occurred)</label>
                    </div>
                    <div class="my-md-8">
                        <asp:DropDownList ID="sltProvince" runat="server" OnSelectedIndexChanged="sltProvince_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator runat="server" ID="reqProvince" InitialValue="-1" ControlToValidate="sltProvince" ErrorMessage="<span  class='required_alert'>*</span>" />
                    </div>
                </div>
                <div class="half-line">
                    <div class="my-md-4">
                        <label class="font-9">District (Incident Occurred)</label>
                    </div>
                    <div class="my-md-8">
                        <asp:DropDownList ID="sltDistrict" runat="server" AutoPostBack="true" OnSelectedIndexChanged="sltDistrict_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator runat="server" ID="reqDistrict" InitialValue="-1" ControlToValidate="sltDistrict" ErrorMessage="<span  class='required_alert'>*</span>" />
                    </div>
                </div>
                <div class="clearfix"></div>
            </div>
            <div class="control-group no-line">
                <div class="half-line">
                    <div class="my-md-4">
                        <label class="font-9">LLG (Incident Occurred)</label>
                    </div>
                    <div class="my-md-8">
                        <asp:DropDownList ID="sltLLG" runat="server" AutoPostBack="true" OnSelectedIndexChanged="sltLLG_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator11" InitialValue="-1" ControlToValidate="sltWardno" ErrorMessage="<span  class='required_alert'>*</span>" />
                    </div>
                </div>
                <div class="half-line">
                    <div class="my-md-4">
                        <label class="font-9">Ward Name (Incident Occurred)</label>
                    </div>
                    <div class="my-md-8">
                        <asp:DropDownList ID="sltWardno" runat="server">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator runat="server" ID="reqWardno" InitialValue="-1" ControlToValidate="sltWardno" ErrorMessage="<span  class='required_alert'>*</span>" />
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="half-line">
        <div class="my-md-4">
            <label class="font-9">Place Name (Incident Occurred)</label>
        </div>
        <div class="my-md-8">
            <asp:TextBox ID="txtPlaceName" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator runat="server" ID="reqPlaceName" InitialValue="" ControlToValidate="txtPlaceName" ErrorMessage="<span  class='required_alert'>*</span>" />
        </div>
    </div>
    <div class="clearfix"></div>
    <div class="control-group no-line">
        <div class="half-line">
            <div class="my-md-4">
                <label class="font-9">Date Incident Occurred</label>
            </div>
            <div class="my-md-8">
                <asp:TextBox ID="dtDateIncidentOccured" TextMode="Date" runat="server"></asp:TextBox>
                <%--<input id="dtDateIncidentOccured" type="text" runat="server" />--%>
                <asp:RequiredFieldValidator runat="server" ID="reqDateIncidentOccured" InitialValue="" ControlToValidate="dtDateIncidentOccured" ErrorMessage="<span  class='required_alert'>*</span>" />
            </div>
        </div>
        <div class="half-line">
            <div class="my-md-4">
                <label class="font-9">Date Incident Reported</label>
            </div>
            <div class="my-md-8">
                <asp:TextBox ID="dtDateIncidentReported" TextMode="Date" runat="server"></asp:TextBox>
                <%--<input id="dtDateIncidentReported" type="text" runat="server" />--%>
                <asp:RequiredFieldValidator runat="server" ID="reqDateIncidentReported" InitialValue="" ControlToValidate="dtDateIncidentReported" ErrorMessage="<span  class='required_alert'>*</span>" />
            </div>
        </div>
        <div class="clearfix"></div>
    </div>

    <div class="control-group no-line">
        <div class="half-line padding-0">
            <div style="width: 100%;">
                <div class="my-md-5 table-header font-9">Number of Affected HH</div>
                <div class="my-md-7 table-header font-9">Number of Affected Individuals</div>
                <asp:HiddenField ID="hdnaffectedpopulation" runat="server" />
            </div>
            <div style="width: 100%; margin-top: 25px;">
                <div class="my-md-5 align-center">
                    <input id="txtNumberAffectedHH" type="number" runat="server">
                    <asp:RequiredFieldValidator runat="server" ID="reqNumberAffectedHH" InitialValue="" ControlToValidate="txtNumberAffectedHH" ErrorMessage="<span class='required_alert'>*</span>" />
                </div>
                <div class="my-md-7 align-center">
                    <input id="txtNumberAffectedIndividuals" type="number" runat="server">
                    <asp:RequiredFieldValidator runat="server" ID="reqNumberAffectedIndividuals" InitialValue="" ControlToValidate="txtNumberAffectedIndividuals" ErrorMessage="<span  class='required_alert'>*</span>" />
                </div>
            </div>
            <div class="clearfix"></div>
        </div>
        <div class="half-line padding-0">
            <div style="width: 100%;">
                <div class="my-md-5 table-header font-9">No. Affected Males</div>
                <div class="my-md-7 table-header font-9">No. Affected Females</div>
            </div>
            <div style="width: 100%; margin-top: 25px;">
                <div class="my-md-5 align-center">
                    <input id="txtMales1" type="number" runat="server">
                </div>
                <div class="my-md-7 align-center">
                    <input id="txtFemales1" type="number" runat="server">
                </div>
            </div>
            <div class="clearfix"></div>
        </div>
        <div class="clearfix"></div>
    </div>

    <div class="control-group no-line">
        <div class="half-line padding-0">
            <div style="width: 100%;">
                <div class="my-md-5 table-header font-9">Displaced No of HH</div>
                <div class="my-md-7 table-header font-9">Displaced No of Individuals</div>
                <asp:HiddenField ID="hdndamagedInfrastructure" runat="server" />
            </div>
            <div style="width: 100%; margin-top: 25px;">
                <div class="my-md-5 align-center">
                    <input id="txtDisplacedNoHH" type="number" runat="server">
                </div>
                <div class="my-md-7 align-center">
                    <input id="txtDisplacedNoIndividuals" type="number" runat="server">
                </div>
            </div>
            <div class="clearfix"></div>
        </div>
        <div class="half-line padding-0">
            <div style="width: 100%;">
                <div class="my-md-5 table-header font-9">No. Displaced Males</div>
                <div class="my-md-7 table-header font-9">No. Displaced Females</div>
            </div>
            <div style="width: 100%; margin-top: 25px;">
                <div class="my-md-5 align-center">
                    <input id="txtMales2" type="number" runat="server">
                </div>
                <div class="my-md-7 align-center">
                    <input id="txtFemales2" type="number" runat="server">
                </div>
            </div>
            <div class="clearfix"></div>
        </div>
        <div class="clearfix"></div>
    </div>
    <div class="control-group no-line">
        <div class="affected-population">
            Affected Population
        </div>
    </div>
    <div class="row">
        <div class="col-lg-9 col-md-12 col-xs-12">
            <table>
                <thead>
                    <tr>
                        <th colspan="2">Casualty</th>
                        <th>No. Males 18+</th>
                        <th>No. Females 18+</th>
                        <th>No. Males 0-17</th>
                        <th>No. Females 0-17</th>
                        <%--<th>No. Communities</th>
                        <th>No. Households</th>--%>
                        <th>Totals</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td colspan="2">Injured</td>
                        <td>
                            <input id="txtAffectedPopulation11" type="number" value="" runat="server" onchange="affected_population_changed()" /></td>
                        <td>
                            <input id="txtAffectedPopulation12" type="number" value="" runat="server" onchange="affected_population_changed()" /></td>
                        <td>
                            <input id="txtAffectedPopulation13" type="number" value="" runat="server" onchange="affected_population_changed()" /></td>
                        <td>
                            <input id="txtAffectedPopulation14" type="number" value="" runat="server" onchange="affected_population_changed()" /></td>
                        <%--<td><input id="txtAffectedPopulation15" type="number" value="" runat="server" onchange="affected_population_changed()"/></td>--%>
                        <%--<td><input id="txtAffectedPopulation16" type="number" value="" runat="server" onchange="affected_population_changed()"/></td>--%>
                        <td><span id="ap_h1_sum">0</span></td>
                    </tr>
                    <tr>
                        <td colspan="2">Stranded or Marooned</td>
                        <td>
                            <input id="txtAffectedPopulation21" type="number" value="" runat="server" onchange="affected_population_changed()" /></td>
                        <td>
                            <input id="txtAffectedPopulation22" type="number" value="" runat="server" onchange="affected_population_changed()" /></td>
                        <td>
                            <input id="txtAffectedPopulation23" type="number" value="" runat="server" onchange="affected_population_changed()" /></td>
                        <td>
                            <input id="txtAffectedPopulation24" type="number" value="" runat="server" onchange="affected_population_changed()" /></td>
                        <%--<td><input id="txtAffectedPopulation25" type="number" value="" runat="server" onchange="affected_population_changed()" /></td>--%>
                        <%--<td><input id="txtAffectedPopulation26" type="number" value="" runat="server" onchange="affected_population_changed()" /></td>--%>
                        <td><span id="ap_h2_sum">0</span></td>
                    </tr>
                    <tr>
                        <td colspan="2">Missing</td>
                        <td>
                            <input id="txtAffectedPopulation31" type="number" value="" runat="server" onchange="affected_population_changed()" /></td>
                        <td>
                            <input id="txtAffectedPopulation32" type="number" value="" runat="server" onchange="affected_population_changed()" /></td>
                        <td>
                            <input id="txtAffectedPopulation33" type="number" value="" runat="server" onchange="affected_population_changed()" /></td>
                        <td>
                            <input id="txtAffectedPopulation34" type="number" value="" runat="server" onchange="affected_population_changed()" /></td>
                        <%--<td><input id="txtAffectedPopulation35" type="number" value="" runat="server" onchange="affected_population_changed()" /></td>--%>
                        <%--<td><input id="txtAffectedPopulation36" type="number" value="" runat="server" onchange="affected_population_changed()" /></td>--%>
                        <td><span id="ap_h3_sum">0</span></td>
                    </tr>
                    <tr>
                        <td colspan="2">Dead</td>
                        <td>
                            <input id="txtAffectedPopulation41" type="number" value="" runat="server" onchange="affected_population_changed()" /></td>
                        <td>
                            <input id="txtAffectedPopulation42" type="number" value="" runat="server" onchange="affected_population_changed()" /></td>
                        <td>
                            <input id="txtAffectedPopulation43" type="number" value="" runat="server" onchange="affected_population_changed()" /></td>
                        <td>
                            <input id="txtAffectedPopulation44" type="number" value="" runat="server" onchange="affected_population_changed()" /></td>
                        <%--<td><input id="txtAffectedPopulation45" type="number" value="" runat="server" onchange="affected_population_changed()" /></td>
            		    <td><input id="txtAffectedPopulation46" type="number" value="" runat="server" onchange="affected_population_changed()" /></td>--%>
                        <td><span id="ap_h4_sum">0</span></td>
                    </tr>
                    <tr>
                        <td colspan="2">Ailing</td>
                        <td>
                            <input id="txtAffectedPopulation51" type="number" value="" runat="server" onchange="affected_population_changed()" /></td>
                        <td>
                            <input id="txtAffectedPopulation52" type="number" value="" runat="server" onchange="affected_population_changed()" /></td>
                        <td>
                            <input id="txtAffectedPopulation53" type="number" value="" runat="server" onchange="affected_population_changed()" /></td>
                        <td>
                            <input id="txtAffectedPopulation54" type="number" value="" runat="server" onchange="affected_population_changed()" /></td>
                        <%--<td><input id="txtAffectedPopulation55" type="number" value="" runat="server" onchange="affected_population_changed()" /></td>--%>
                        <%--<td><input id="txtAffectedPopulation56" type="number" value="" runat="server" onchange="affected_population_changed()" /></td>--%>
                        <td><span id="ap_h5_sum">0</span></td>
                    </tr>
                    <tr>
                        <td class="casualty_other">Other</td>
                        <td>
                            <input id="txtAffectedPopulationOther" type="text" value="" runat="server" /></td>
                        <td>
                            <input id="txtAffectedPopulation61" type="number" value="" runat="server" onchange="affected_population_changed()" /></td>
                        <td>
                            <input id="txtAffectedPopulation62" type="number" value="" runat="server" onchange="affected_population_changed()" /></td>
                        <td>
                            <input id="txtAffectedPopulation63" type="number" value="" runat="server" onchange="affected_population_changed()" /></td>
                        <td>
                            <input id="txtAffectedPopulation64" type="number" value="" runat="server" onchange="affected_population_changed()" /></td>
                        <%--<td><input id="txtAffectedPopulation65" type="number" value=""  runat="server" onchange="affected_population_changed()"/></td>--%>
                        <%--<td><input id="txtAffectedPopulation66" type="number" value=""  runat="server" onchange="affected_population_changed()"/></td>--%>
                        <td><span id="ap_h6_sum">0</span></td>
                    </tr>
                    <tr class="casualty_totals">
                        <td colspan="2" style="text-align: left">Totals</td>
                        <td><span id="ap_v1_sum">0</span></td>
                        <td><span id="ap_v2_sum">0</span></td>
                        <td><span id="ap_v3_sum">0</span></td>
                        <td><span id="ap_v4_sum">0</span></td>
                        <%--<td><span id="ap_v5_sum">0</span></td>
            		    <td><span id="ap_v6_sum">0</span></td>--%>
                        <td style="text-align: left;"><span id="ap_total_sum">0</span></td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="col-lg-3 col-md-6 col-xs-6">
            <div class="form-group" style="margin-bottom: 0;">
                <label class="damage">
                    Type of Incident<br />
                    (Primary incident i.e.)</label>
                <asp:DropDownList ID="sltTypeIncident" runat="server">
                </asp:DropDownList>
                <asp:RequiredFieldValidator runat="server" ID="reqTypeIncident" InitialValue="-1" ControlToValidate="sltTypeIncident" ErrorMessage="<span  class='required_alert'>*</span>" />
            </div>
            <div class="form-group">
                <label class="if-other">If other...</label>
                <asp:TextBox ID="txtTypeIncidentOther" runat="server"></asp:TextBox>
            </div>

            <div class="form-group" style="margin-bottom: 0;">
                <label class="if-other">
                    Displacement Status<br />
                    (If Displacement)</label>
                <asp:DropDownList ID="sltDisplacementStatus" runat="server">
                </asp:DropDownList>
                <asp:RequiredFieldValidator runat="server" ID="reqDisplacementStatus" InitialValue="-1" ControlToValidate="sltDisplacementStatus" ErrorMessage="<span  class='required_alert'>*</span>" />
            </div>
            <div class="form-group">
                <label class="if-other">If other...</label>
                <asp:TextBox ID="txtDisplacementStatusOther" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="clearfix"></div>
        <div class="col-md-12 col-xs-12 patient" style="margin-top: 30px;">
            <table>
                <thead>
                    <tr class="patient-name">
                        <th>Pregnant Women</th>
                        <th>Lactating Women</th>
                        <th>Orphans</th>
                        <th>Child Head of HH</th>
                        <th>Elderly</th>
                        <th>Disability</th>
                        <th>Chronically Ill</th>
                    </tr>
                </thead>
                <tbody>
                    <tr class="patient-td">
                        <td>
                            <input id="txtPregnant" runat="server" type="number" value="" /></td>
                        <td>
                            <input id="txtLactating" type="number" runat="server" value="" /></td>
                        <td>
                            <input id="txtOrphans" type="number" runat="server" value="" /></td>
                        <td>
                            <input id="txtChildheaded" type="number" runat="server" value="" /></td>
                        <td>
                            <input id="txtEderly" type="number" runat="server" value="" /></td>
                        <td>
                            <input id="txtDisabled" type="number" runat="server" value="" /></td>
                        <td>
                            <input id="txtChronicallyill" type="number" runat="server" value="" /></td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="clearfix"></div>
        <div class="col-md-12 col-xs-12 patient-data" style="margin-top: 30px;">
            <div class="control-group no-line">
                <div class="affected-population">
                    Age Groups  
            &nbsp
                </div>
            </div>
            <table>
                <thead>
                    <tr class="patient-checkbox-name">
                        <th>< 1</th>
                        <th>1 - 4</th>
                        <th>5 - 9</th>
                        <th>10 - 14</th>
                        <th>15 - 18</th>
                        <th>19 - 59</th>
                        <th>60 +</th>
                        <th>Totals</th>
                    </tr>
                </thead>
                <tbody>
                    <tr class="patient-data-td">
                        <td>
                            <input id="txtAgegroup4" type="number" runat="server" value="" onchange="agegoup_changed()" /></td>
                        <td>
                            <input id="txtAgegroup5" type="number" runat="server" value="" onchange="agegoup_changed()" /></td>
                        <td>
                            <input id="txtAgegroup18" type="number" runat="server" value="" onchange="agegoup_changed()" /></td>
                        <td>
                            <input id="txtAgegroup31" type="number" runat="server" value="" onchange="agegoup_changed()" /></td>
                        <td>
                            <input id="txtAgegroup44" type="number" runat="server" value="" onchange="agegoup_changed()" /></td>
                        <td>
                            <input id="txtAgegroup57" type="number" runat="server" value="" onchange="agegoup_changed()" /></td>
                        <td>
                            <input id="txtAgegroup65" type="number" runat="server" value="" onchange="agegoup_changed()" /></td>
                        <td><span id="age_total">0</span></td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="clearfix"></div>
        <div class="col-md-12 col-xs-12 patient-checkbox" style="margin-top: 30px;">
            <div class="control-group no-line">
                <div class="affected-population">
                    Immediate Needs
            &nbsp
                </div>
            </div>
            <table>
                <colgroup>
                    <col width="10%" />
                    <col width="10%" />
                    <col width="10%" />
                    <col width="10%" />
                    <col width="10%" />
                    <col width="10%" />
                    <col width="10%" />
                    <col width="10%" />
                    <col width="20%" />
                </colgroup>
                <thead>
                    <tr class="patient-checkbox-name">
                        <th>Food</th>
                        <th>Nutrition</th>
                        <th>Water</th>
                        <th>Sanitation</th>
                        <th>Hygiene</th>
                        <th>Shelter</th>
                        <th>Education</th>
                        <th>Health</th>
                        <th>Other</th>
                    </tr>
                </thead>
                <tbody>
                    <tr class="patient-checkbox-td">
                        <td>
                            <input id="chkNeedfood" type="checkbox" runat="server" value="" /></td>
                        <td>
                            <input id="chkNutrition" type="checkbox" runat="server" value="" /></td>
                        <td>
                            <input id="chkWater" type="checkbox" runat="server" value="" /></td>
                        <td>
                            <input id="chkSanitation" type="checkbox" runat="server" value="" /></td>
                        <td>
                            <input id="chkHygiene" type="checkbox" runat="server" value="" /></td>
                        <td>
                            <input id="chkShelter" type="checkbox" runat="server" value="" /></td>
                        <td>
                            <input id="chkEducation" type="checkbox" runat="server" value="" /></td>
                        <td>
                            <input id="chkHealth" type="checkbox" runat="server" value="" /></td>
                        <td>
                            <textarea id="taOther" runat="server" name=""></textarea></td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="col-md-12 col-lg-9 col-xs-12" style="margin-top: 30px;" class="damaged-infrastructure">
            <table>
                <colgroup>
                    <col width="50%"></col>
                    <col width="10%"></col>
                    <col width="15%"></col>
                    <col width="25%"></col>
                </colgroup>
                <thead>
                    <tr class="damaged-infras-name">
                        <th>Damaged Infrastructure</th>
                        <th>Qty</th>
                        <th>M. Unit</th>
                        <th>Comments</th>
                    </tr>
                </thead>
                <tbody>
                    <tr class="damaged-infras-td">
                        <td>Roads & Highways</td>
                        <td>
                            <input id="txtRHQty" type="number" runat="server" name="" value="" /></td>
                        <td>
                            <asp:DropDownList ID="cmbRHUnit" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <input id="txtRHComment" type="text" runat="server" class="comment" /></td>
                    </tr>
                    <tr class="damaged-infras-td">
                        <td>Service Stations</td>
                        <td>
                            <input id="txtSSQty" type="number" runat="server" name="" value="" /></td>
                        <td>
                            <asp:DropDownList ID="cmbSSUnit" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <input id="txtSSComment" type="text" runat="server" class="comment" /></td>
                    </tr>
                    <tr class="damaged-infras-td">
                        <td>Electrical power lines</td>
                        <td>
                            <input id="txtEPLQty" type="number" runat="server" name="" value="" /></td>
                        <td>
                            <asp:DropDownList ID="cmbEPLUnit" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <input id="txtEPLComment" type="text" runat="server" class="comment" /></td>
                    </tr>
                    <tr class="damaged-infras-td">
                        <td>Electrical power substation</td>
                        <td>
                            <input id="txtEPSQty" type="number" runat="server" name="" value="" /></td>
                        <td>
                            <asp:DropDownList ID="cmbEPSUnit" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <input id="txtEPSComment" type="text" runat="server" class="comment" /></td>
                    </tr>
                    <tr class="damaged-infras-td">
                        <td>Natural gas, petroleum pipelines</td>
                        <td>
                            <input id="txtNGPPQty" type="number" runat="server" name="" value="" /></td>
                        <td>
                            <asp:DropDownList ID="cmbNGPPUnit" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <input id="txtNGPPComment" type="text" runat="server" class="comment" /></td>
                    </tr>
                    <tr class="damaged-infras-td">
                        <td>Telephone lines</td>
                        <td>
                            <input id="txtTLQty" type="number" runat="server" name="" value="" /></td>
                        <td>
                            <asp:DropDownList ID="cmbTLUnit" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <input id="txtTLComment" type="text" runat="server" class="comment" /></td>
                    </tr>
                    <tr class="damaged-infras-td">
                        <td>Mobile phone towers</td>
                        <td>
                            <input id="txtMPTQty" type="number" runat="server" name="" value="" /></td>
                        <td>
                            <asp:DropDownList ID="cmbMPTUnit" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <input id="txtMPTComment" type="text" runat="server" class="comment" /></td>
                    </tr>
                    <tr class="damaged-infras-td">
                        <td>Internet or fibre optic lines</td>
                        <td>
                            <input id="txtIFOLQty" type="number" runat="server" name="" value="" /></td>
                        <td>
                            <asp:DropDownList ID="cmbIFOLUnit" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <input id="txtIFOLComment" type="text" runat="server" class="comment" /></td>
                    </tr>
                    <tr class="damaged-infras-td">
                        <td>Schools, Colleges and Educational Training Facilities</td>
                        <td>
                            <input id="txtSCEQty" type="number" runat="server" name="" value="" /></td>
                        <td>
                            <asp:DropDownList ID="cmbSCEUnit" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <input id="txtSCEComment" type="text" runat="server" class="comment" /></td>
                    </tr>
                    <tr class="damaged-infras-td">
                        <td>Shopping Centre or Complex</td>
                        <td>
                            <input id="txtSCCQty" type="number" runat="server" name="" value="" /></td>
                        <td>
                            <asp:DropDownList ID="cmbSCCUnit" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <input id="txtSCCComment" type="text" runat="server" class="comment" /></td>
                    </tr>
                    <tr class="damaged-infras-td">
                        <td>Homes, houses or similar dwelling place</td>
                        <td>
                            <input id="txtHHSQty" type="number" runat="server" name="" value="" /></td>
                        <td>
                            <asp:DropDownList ID="cmbHHSUnit" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <input id="txtHSSComment" type="text" runat="server" class="comment" /></td>
                    </tr>
                    <tr class="damaged-infras-td">
                        <td>Building and apartments</td>
                        <td>
                            <input id="txtBAAQty" type="number" runat="server" name="" value="" /></td>
                        <td>
                            <asp:DropDownList ID="cmbBAAUnit" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <input id="txtBAAComment" type="text" runat="server" class="comment" /></td>
                    </tr>
                    <tr class="damaged-infras-td">
                        <td>Industrial Facility or Complex</td>
                        <td>
                            <input id="txtIFCQty" type="number" runat="server" name="" value="" /></td>
                        <td>
                            <asp:DropDownList ID="cmbIFCUnit" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <input id="txtIFCComment" type="text" runat="server" class="comment" /></td>
                    </tr>
                    <tr class="damaged-infras-td">
                        <td>Government Facility</td>
                        <td>
                            <input id="txtGFQty" type="number" runat="server" name="" value="" /></td>
                        <td>
                            <asp:DropDownList ID="cmbGFUnit" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <input id="txtGFComment" type="text" runat="server" class="comment" /></td>
                    </tr>
                    <tr class="damaged-infras-td">
                        <td>Bridges</td>
                        <td>
                            <input id="txtBQty" type="number" runat="server" name="" value="" /></td>
                        <td>
                            <asp:DropDownList ID="cmbBUnit" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <input id="txtBComment" type="text" runat="server" class="comment" /></td>
                    </tr>
                    <tr class="damaged-infras-td">
                        <td>Other
                            <input id="txtDIOther" runat="server" type="text" name="" style="width: 90%;"></td>
                        <td>
                            <input id="txtOQty" type="number" runat="server" name="" value="" /></td>
                        <td>
                            <asp:DropDownList ID="cmbOUnit" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <input id="txtOComment" type="text" runat="server" class="comment" /></td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="col-md-6 col-xs-6 col-lg-3" style="margin-top: 30px;">
            <div class="form-group">
                <label class="if-other">Type of location</label>
                <input id="txtTypeLocation" runat="server" type="text">
            </div>
            <div class="form-group">
                <label class="if-other">GPS Coordinate (Lat/Long)</label>
                <div class="form-group col-md-6 col-xs-6 col-lg-6" style="padding: 0;">
                    <input id="txtSouth" runat="server" type="number" name="" style="width: 97%;">
                </div>
                <div class="form-group col-md-6 col-xs-6 col-lg-6" style="padding: 0;">
                    <input id="txtEast" runat="server" type="number" name="" style="width: 97%;">
                </div>
            </div>
        </div>
        <div class="clearfix"></div>
        <div class="col-md-12 col-lg-9 col-xs-12 access-table" style="margin-top: 30px;">
            <table>
                <colgroup>
                    <col width="40%"></col>
                    <col width="10%"></col>
                    <col width="50%"></col>
                </colgroup>
                <thead>
                    <tr class="access-table-name-th">
                        <th>Affected Essential Service(s)</th>
                        <th>Accessibility</th>
                        <th>Comments</th>
                    </tr>
                </thead>
                <tbody>
                    <tr class="access-table-td">
                        <asp:HiddenField ID="hdnEssentialServices" runat="server" />
                        <td>Schools</td>
                        <td>
                            <input id="chkSchools" runat="server" type="checkbox"></td>
                        <td>
                            <input id="txtSchoolComment" runat="server" type="text" class="comments"></td>
                    </tr>
                    <tr class="access-table-td">
                        <td>Hospital or Clinic</td>
                        <td>
                            <input id="chkHC" runat="server" type="checkbox" name=""></td>
                        <td>
                            <input id="txtHCComment" runat="server" type="text" name="" class="comments"></td>
                    </tr>
                    <tr class="access-table-td">
                        <td>Policing & Security</td>
                        <td>
                            <input id="chkPS" runat="server" type="checkbox" name=""></td>
                        <td>
                            <input id="txtPSComment" runat="server" type="text" name="" class="comments"></td>
                    </tr>
                    <tr class="access-table-td">
                        <td>Water Supplies (Safe water)</td>
                        <td>
                            <input id="chkWSSW" runat="server" type="checkbox" name=""></td>
                        <td>
                            <input id="txtWSSWComment" runat="server" type="text" name="" class="comments"></td>
                    </tr>
                    <tr class="access-table-td">
                        <td>Food Provisions</td>
                        <td>
                            <input id="chkFP" runat="server" type="checkbox" name=""></td>
                        <td>
                            <input id="txtFPComment" runat="server" type="text" name="" class="comments"></td>
                    </tr>
                    <tr class="access-table-td">
                        <td>Other
                            <input id="txtAESOther" runat="server" type="text" name="" style="width: 70%;"></td>
                        <td>
                            <input id="chkAESOther" runat="server" type="checkbox" name=""></td>
                        <td>
                            <input id="txtAESOtherComment" runat="server" type="text" name="" class="comments"></td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="col-md-6 col-xs-6 col-lg-3" style="margin-top: 30px;">
            <div class="form-group">
                <p>Access to Location</p>
                <p style="line-height: 0;">
                    <input id="chkRoad" runat="server" type="checkbox" name="" style="width: 30px;"><span>By Road</span>
                </p>
                <p style="line-height: 0;">
                    <input id="chkAir" runat="server" type="checkbox" name="" style="width: 30px;"><span>By Air</span>
                </p>
                <p style="line-height: 0;">
                    <input id="chkBoat" runat="server" type="checkbox" name="" style="width: 30px;"><span>By Boat</span>
                </p>
                <p style="line-height: 0;">
                    <input id="chkAPROther" runat="server" type="checkbox" name="" style="width: 30px;"><span>Other</span>
                    <input id="txtAPROther" runat="server" type="text" name="" style="width: 30%;">
                </p>
            </div>
        </div>
        <div class="clearfix"></div>
        <div class="col-md-12 col-xs-12 col-lg-10" style="margin-top: 30px;">
            <div class="form-group">
                <label class="damage">Comment (Case overview)</label>
                <textarea id="txtComment" runat="server" class="case-overview"></textarea>
                <asp:RequiredFieldValidator runat="server" ID="reqComment" InitialValue="-1" ControlToValidate="txtComment" ErrorMessage="<span  class='required_alert'>*</span>" />
            </div>
        </div>

        <div class="clearfix"></div>
        <div class="col-md-12 col-xs-12 col-lg-12" style="margin-top: 30px;">
            <div class="row" style="margin-bottom: 10px;">
                <div class="col-md-2" style="width: 80px;">
                    <p style="line-height: 0; font-weight: bold; margin-top: 10px;">Causes</p>
                </div>
                <div class="col-md-10">
                    <asp:HiddenField ID="hdnCauses" runat="server" />
                    <asp:DropDownList ID="cmbCauses" runat="server">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6 col-xs-6 col-lg-6">
                    <div class="form-group">
                        <input id="chkSpecify" runat="server" type="checkbox" name="" style="width: 30px;"><span style="width: 60px;">Specify</span>
                        <input id="txtSpecify" runat="server" type="text" name="" style="width: 60%;">
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6 col-xs-6 col-lg-6">
                    <div class="form-group">
                        <span class="causes-keys">Govt.Focal or key Person</span>
                        <input id="txtFKP" runat="server" type="text" name="" style="width: 60%;">
                    </div>
                </div>
                <div class="col-md-6 col-xs-6 col-lg-6">
                    <div class="form-group">
                        <span class="causes-keys">Community Key Person</span>
                        <input id="txtCKP" runat="server" type="text" name="" style="width: 60%;">
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-11 col-xs-11 col-lg-11">
                    <div class="form-group">
                        <label class="if-other">Govt.Team Members</label>
                        <textarea id="txtTM" runat="server" name="" class="team-members"></textarea>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-6 col-xs-6 col-lg-6">
                    <span class="causes-keys">Case Closed ?</span><br />
                    <input id="chkCaseClosed" style="width: 20px;" runat="server" type="checkbox" name="">
                    <input id="txtCaseClosed" runat="server" type="text" style="width: 90%;" />
                </div>
                <div class="col-md-6 col-xs-6 col-lg-6">
                    <span class="causes-keys">Organisation's Reference No. ? (if applicable)</span>
                    <input id="txtORN" runat="server" type="text" name="">
                </div>
            </div>
        </div>
        <div class="clearfix"></div>
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <div class="col-md-12 col-xs-12 col-lg-12" style="margin-top: 30px;">
                    <p style="line-height: 0; font-weight: bold;">Previous Location (If displaced)</p>
                    <div class="row" style="background: rgb(255, 255, 230); padding: 10px; margin-left: 0px; margin-top: 25px; margin-right: 15px; border: 1px solid gray;">
                        <asp:HiddenField ID="hdnPreviousLocation" runat="server" />
                        <div class="col-md-4 col-xs-4 col-lg-4">
                            <div class="row">
                                <label class="if-other col-md-4">Province</label>
                                <asp:DropDownList ID="cmbPreProvince" runat="server" CssClass="col-md-8" AutoPostBack="true" OnSelectedIndexChanged="cmbPreProvince_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            <div class="row">
                                <label class="if-other col-md-4">Country</label>
                                <asp:DropDownList ID="cmbPreCountry" runat="server" CssClass="col-md-8">
                                </asp:DropDownList>
                            </div>
                            <div class="row">
                                <label class="if-other col-md-12">GPS Coordinaes (Lat/Long)</label>
                            </div>
                            <div class="row">
                                <div class="form-group col-md-6 col-xs-6 col-lg-6" style="padding: 0;">
                                    <input id="txtGPSSouth" runat="server" type="text" name="" style="width: 97%;">
                                </div>
                                <div class="form-group col-md-6 col-xs-6 col-lg-6" style="padding: 0;">
                                    <input id="txtGPSEast" runat="server" type="text" name="" style="width: 97%;">
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4 col-xd-4 col-lg-4">
                            <div class="row">
                                <label class="if-other col-md-4">District</label>
                                <asp:DropDownList ID="cmbPreDistrict" runat="server" AutoPostBack="true" class="col-md-8" OnSelectedIndexChanged="cmbPreDistrict_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            <div class="row">
                                <label class="if-other col-lg-4">Place Name</label>
                                <input id="txtPrePlaceName" runat="server" type="text" class="col-md-8" name="">
                            </div>
                        </div>
                        <div class="col-md-4 col-xd-4 col-lg-4">
                            <div class="row" style="margin-bottom: 0;">
                                <label class="if-other col-md-4">Ward No.</label>
                                <asp:DropDownList ID="cmbPreWardno" CssClass="col-md-8" runat="server">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <div class="clearfix"></div>
        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <div class="col-md-12 col-xs-12 col-lg-12" style="margin-top: 30px;">
                    <p style="line-height: 0; font-weight: bold;">Affected Groups</p>
                    <div class="row" style="background: rgb(255, 255, 230); border: 1px solid gray; padding-top: 15px; margin-left: 0px; margin-top: 25px; margin-right: 15px;">
                        <div class="col-md-3 col-xs-3 col-lg-3" style="border-right: 1px solid #ddd;">
                            <asp:HiddenField ID="hdnAffectedGroups" runat="server" />
                            <div class="form-group">
                                <label>Community</label>
                                <asp:DropDownList ID="cmbCommunity" runat="server" CssClass="col-md-11 padding-0" OnSelectedIndexChanged="cmbCommunity_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator10" InitialValue="-1" ControlToValidate="cmbCommunity" ErrorMessage="<span  class='required_alert'>*</span>" />
                            </div>
                            <hr class="affected-group">
                            <div class="form-group">
                                <div class="col-md-5" style="padding: 0;">
                                    <asp:Button ID="btnAddCommunity" runat="server" CssClass="btn btn-red btn-xs" Text="Add" BackColor="Red" OnClick="btnAddCommunity_Click" ValidationGroup="vgAddCommunity" />
                                </div>
                                <div class="col-md-7" style="padding: 0;">
                                    <input id="txtAddCommunity" runat="server" type="text" name="" value="" style="width: 85%;" validationgroup="vgAddCommunity">
                                    <asp:RequiredFieldValidator runat="server" ID="reqAddCommunity" InitialValue="" ControlToValidate="txtAddCommunity" ValidationGroup="vgAddCommunity" ErrorMessage="<span  class='required_alert'>*</span>" />
                                </div>
                                <div class="clearfix"></div>
                            </div>
                            <div class="clearfix"></div>
                            <div class="form-group">
                                <div class="col-md-5" style="padding: 0;">
                                    <asp:Button ID="btnRenameCommunity" runat="server" CssClass="btn btn-red btn-xs" Text="Rename" BackColor="Red" OnClick="btnRenameCommunity_Click" ValidationGroup="vgRenameCommunity" />
                                </div>
                                <asp:DropDownList ID="cmbRenameCommunity" runat="server" CssClass="col-md-6  padding-0">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator runat="server" ID="reqcmbRenameCommunity" InitialValue="-1" ControlToValidate="cmbRenameCommunity" ValidationGroup="vgRenameCommunity" ErrorMessage="<span  class='required_alert'>*</span>" />
                                <input id="txtRenameCommunity" runat="server" type="text" value="" style="width: 90%; margin-top: 7px;">
                                <asp:RequiredFieldValidator runat="server" ID="reqRenameCommunity" InitialValue="" ControlToValidate="txtRenameCommunity" ValidationGroup="vgRenameCommunity" ErrorMessage="<span  class='required_alert'>*</span>" />
                            </div>
                            <div class="form-group">
                                <div class="col-md-5" style="padding: 0;">
                                    <asp:Button ID="btnRemoveCommunity" runat="server" CssClass="btn btn-red btn-xs" Text="Remove" BackColor="Red" OnClick="btnRemoveCommunity_Click" ValidationGroup="vgRemoveCommunity" />
                                </div>
                                <asp:DropDownList ID="cmbRemoveCommunity" runat="server" CssClass="col-md-6 padding-0">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator runat="server" ID="reqRemoveCommunity" InitialValue="-1" ControlToValidate="cmbRemoveCommunity" ValidationGroup="vgRemoveCommunity" ErrorMessage="<span  class='required_alert'>*</span>" />
                            </div>
                            <div class="form-group">
                                <div class="col-md-5" style="padding: 0; display: none;">
                                    <asp:Button ID="addAssistance" runat="server" CssClass="btn btn-primary btn-xs"
                                        Text="Assistance" ValidationGroup="vgModel1" data-toggle="modal" data-target="#assistanceModel" />

                                    <div id="assistanceModel" class="modal fade" role="dialog">

                                        <div class="modal-dialog modal-lg">
                                            <!-- Modal content-->
                                            <div class="modal-content">
                                                <div class="modal-header">
                                                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                                                    <h4 class="modal-title">Add Assistance</h4>
                                                </div>
                                                <div class="modal-body">
                                                    <asp:UpdatePanel ID="upAssistanceModel" runat="server">
                                                        <ContentTemplate>
                                                            <div class="row">
                                                                <div class="col-md-6">
                                                                    <label>Case Number</label>
                                                                    <br />
                                                                    <asp:HiddenField ID="assId" runat="server"></asp:HiddenField>
                                                                    <asp:Label ID="lblAssistanceCNo" runat="server"></asp:Label>
                                                                </div>
                                                                <div class="col-md-6">
                                                                    <label>Assistance Type</label>
                                                                    <asp:DropDownList ID="ddlAssistanceType" runat="server"></asp:DropDownList>
                                                                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator12" InitialValue="-1" ControlToValidate="ddlAssistanceType" ValidationGroup="vgModel" ErrorMessage="<span  class='required_alert'>*</span>" />
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="col-md-6">
                                                                    <label>Date :</label>
                                                                    <asp:TextBox TextMode="Date" ID="txtADate" runat="server"></asp:TextBox>
                                                                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator14" InitialValue=" " ControlToValidate="txtADate" ValidationGroup="vgModel" ErrorMessage="<span  class='required_alert'>*</span>" />
                                                                </div>
                                                                <div class="col-md-6">
                                                                    <label>No. Of Beneficiaries</label>
                                                                    <asp:TextBox TextMode="Number" ID="txtABeneficiaries" runat="server"></asp:TextBox>
                                                                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator15" InitialValue=" " ControlToValidate="txtABeneficiaries" ValidationGroup="vgModel" ErrorMessage="<span  class='required_alert'>*</span>" />
                                                                </div>
                                                            </div>
                                                            <div class="row">
                                                                <div class="col-md-12">
                                                                    <label>Comment</label>
                                                                    <asp:TextBox TextMode="MultiLine" CssClass="form-control" ID="txtAComment" runat="server"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                                <div class="modal-footer">
                                                    <button type="button" class="btn btn-danger" data-dismiss="modal">Close</button>

                                                    <asp:Button Text="Save" UseSubmitBehavior="false" ID="btnAssistanceSave" OnClick="btnAssistanceSave_Click"
                                                        CssClass="btn btn-primary" runat="server" ValidationGroup="vgModel" />
                                                </div>
                                            </div>
                                        </div>

                                    </div>

                                </div>
                            </div>
                            <div class="form-group">

                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                                    <ContentTemplate>
                                        <asp:Button ID="btnAdd" ValidationGroup="test" runat="server" BackColor="#629DD2" CssClass="btn btn-primary mrg-b" Text="Assistance" OnClick="Add" />

                                        <asp:GridView ID="gvAssistance" runat="server" AutoGenerateColumns="False" DataKeyNames="id"
                                            CellPadding="4" CssClass="tableassistance col-md-12" BackColor="#629DD2" ForeColor="#333333" OnRowDataBound="gvAssistance_RowDataBound" OnRowCancelingEdit="gvAssistance_RowCancelingEdit" OnRowEditing="gvAssistance_RowEditing" OnRowUpdating="gvAssistance_RowUpdating" OnRowDeleting="gvAssistance_RowDeleting">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Case no" SortExpression="Caseno" Visible="false">
                                                    <ItemTemplate>
                                                        <%#Eval("Caseno") %>
                                                        <asp:Label ID="gvlblCaseno" runat="server" Visible="false" Text='<%# Eval("Caseno") %>'></asp:Label>
                                                        <asp:Label ID="gvlblId" runat="server" Visible="false" Text='<%# Eval("id") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Assistance Date" SortExpression="AssistanceDate">
                                                    <ItemTemplate>
                                                        <%#Eval("AssistanceDate","{0:MM/dd/yyyy}") %>
                                                        <asp:Label ID="gvlbltxtDate" runat="server" Visible="false" Text='<%#Eval("AssistanceDate","{0:MM/dd/yyyy}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Assistance Type" SortExpression="AssistanceTypeName" ItemStyle-Width="300px">
                                                    <ItemTemplate>
                                                        <asp:Label ID="gvlblAssistanceTypeName" runat="server" Text='<%# Eval("AssistanceTypeName") %>'></asp:Label>
                                                        <asp:Label Visible="false" ID="AssistanceType" runat="server" Text='<%# Eval("AssistanceType") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Beneficiaries" SortExpression="Beneficiaries">
                                                    <ItemTemplate>
                                                        <asp:Label ID="gvlblBeneficiaries" runat="server" Text='<%# Eval("Beneficiaries") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Comments" SortExpression="Comments" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="gvlblComments" runat="server" Text='<%# Eval("Comments") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField Visible="false" DataField="AssistanceType" HeaderText="AssistanceType" SortExpression="AssistanceType" />
                                                <%--<asp:CommandField ShowEditButton="true" />--%>
                                                <asp:TemplateField ItemStyle-Width="30px" HeaderText="">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkEdit" runat="server" ValidationGroup="test4" Text="Edit" OnClick="lnkEdit_Click"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:CommandField ShowDeleteButton="true" Visible="false" />
                                            </Columns>
                                            <EditRowStyle BackColor="#2461BF" />
                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#629DD2" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Middle" />
                                            <PagerStyle BackColor="#507CD1" ForeColor="White" HorizontalAlign="Center" Height="20px" />
                                            <RowStyle BackColor="#EFF3FB" />
                                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                            <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                            <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                            <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                            <SortedDescendingHeaderStyle BackColor="#4870BE" />
                                        </asp:GridView>

                                        <asp:Panel ID="pnlAddEdit" runat="server" CssClass="modalPopup" Style="display: none">
                                            <asp:Label Font-Bold="true" ID="Label4" runat="server" Text="Add Assistance"></asp:Label>
                                            <br />
                                            <table align="center" style="width: 100%;">
                                                <tr>
                                                    <td>
                                                        <label>Case Number</label>
                                                    </td>
                                                    <td>
                                                        <asp:HiddenField ID="PophdnassId" runat="server"></asp:HiddenField>
                                                        <asp:Label ID="PoplblAssistanceCNo" runat="server" Style="width: 80%;"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label>Assistance Type</label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="PopddlAssistanceType" runat="server" Style="width: 80%;"></asp:DropDownList>
                                                        <asp:RequiredFieldValidator runat="server" ID="pRequiredFieldValidator13" InitialValue="-1" ControlToValidate="PopddlAssistanceType" ValidationGroup="vgpModel" ErrorMessage="<span  class='required_alert'>*</span>" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label>Date :</label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox TextMode="Date" ID="PoptxtADate" runat="server" Style="width: 80%;"></asp:TextBox>
                                                        <asp:RequiredFieldValidator runat="server" ID="pRequiredFieldValidator14" InitialValue=" " ControlToValidate="PoptxtADate" ValidationGroup="vgpModel" ErrorMessage="<span  class='required_alert'>*</span>" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label>No. Of Beneficiaries</label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox TextMode="Number" ID="PoptxtABeneficiaries" runat="server" Style="width: 80%;"></asp:TextBox>
                                                        <asp:RequiredFieldValidator runat="server" ID="pRequiredFieldValidator16" InitialValue=" " ControlToValidate="PoptxtABeneficiaries" ValidationGroup="vgpModel" ErrorMessage="<span  class='required_alert'>*</span>" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <label>Comment</label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox TextMode="MultiLine" CssClass="form-control" ID="PoptxtAComment" runat="server" Style="width: 80%;"></asp:TextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnPopupSave" runat="server" Text="Save" Height="30px" ValidationGroup="vgpModel" CssClass="btn btn-primary" OnClick="btnPopupSave_Click" />
                                                    </td>
                                                    <td>
                                                        <asp:Button ID="btnPopupCancel" runat="server" Text="Cancel" Height="30px" ValidationGroup="test2" CssClass="btn btn-primary" BackColor="Red" OnClick="btnPopupCancel_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>

                                        <asp:LinkButton ID="lnkFake" runat="server"></asp:LinkButton>
                                        <cc1:ModalPopupExtender ID="popup" runat="server" DropShadow="false"
                                            PopupControlID="pnlAddEdit" TargetControlID="lnkFake"
                                            BackgroundCssClass="modalBackground">
                                        </cc1:ModalPopupExtender>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="gvAssistance" EventName="RowEditing" />
                                        <asp:AsyncPostBackTrigger ControlID="btnPopupSave" EventName="Click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnPopupCancel" EventName="Click" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <div class="col-md-3 col-xs-3 col-lg-3" style="border-right: 1px solid #ddd;">
                            <div class="form-group">
                                <label>Household</label>
                                <asp:DropDownList ID="cmbHousehold" runat="server" CssClass="col-md-11 padding-0" OnSelectedIndexChanged="cmbHousehold_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator9" InitialValue="-1" ControlToValidate="cmbHousehold" ErrorMessage="<span  class='required_alert'>*</span>" />
                            </div>
                            <hr class="affected-group">
                            <div class="form-group">
                                <div class="col-md-5" style="padding: 0;">
                                    <asp:Button ID="btnAddHousehold" runat="server" CssClass="btn btn-red btn-xs" Text="Add" BackColor="Red" OnClick="btnAddHousehold_Click" ValidationGroup="vgAddHousehold" />
                                </div>
                                <div class="col-md-7" style="padding: 0;">
                                    <input id="txtAddHousehold" runat="server" type="text" name="" value="" style="width: 85%;">
                                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" InitialValue="" ControlToValidate="txtAddHousehold" ValidationGroup="vgAddHousehold" ErrorMessage="<span  class='required_alert'>*</span>" />
                                </div>
                                <div class="clearfix"></div>
                            </div>
                            <div class="clearfix"></div>
                            <div class="form-group">
                                <div class="col-md-5" style="padding: 0;">
                                    <asp:Button ID="btnRenameHousehold" runat="server" CssClass="btn btn-red btn-xs" Text="Rename" BackColor="Red" OnClick="btnRenameHousehold_Click" ValidationGroup="vgRenameHousehold" />
                                </div>
                                <asp:DropDownList ID="cmbRenameHousehold" runat="server" CssClass="col-md-6 padding-0">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" InitialValue="-1" ControlToValidate="cmbRenameHousehold" ValidationGroup="vgRenameHousehold" ErrorMessage="<span  class='required_alert'>*</span>" />
                                <input id="txtRenameHousehold" runat="server" type="text" name="" value="" style="margin-top: 7px; width: 90%;">
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" InitialValue="" ControlToValidate="txtRenameHousehold" ValidationGroup="vgRenameHousehold" ErrorMessage="<span  class='required_alert'>*</span>" />
                            </div>
                            <div class="form-group">
                                <div class="col-md-5" style="padding: 0;">
                                    <asp:Button ID="btnRemoveHousehold" runat="server" CssClass="btn btn-red btn-xs" Text="Remove" BackColor="Red" OnClick="btnRemoveHousehold_Click" ValidationGroup="vgRemoveHousehold" />
                                </div>
                                <asp:DropDownList ID="cmbRemoveHousehold" runat="server" CssClass="col-md-6 padding-0">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator4" InitialValue="-1" ControlToValidate="cmbRemoveHousehold" ValidationGroup="vgRemoveHousehold" ErrorMessage="<span  class='required_alert'>*</span>" />
                            </div>
                        </div>
                        <div class="col-md-2 col-xs-2 col-lg-2">
                            <div class="form-group">
                                <label>Individuals</label>
                            </div>
                            <div class="form-group">
                                <asp:GridView ID="grdIndividuals" runat="server" AutoGenerateColumns="False" DataKeyNames="id" DataSourceID="SqlDatabase" CellPadding="4" CssClass="table col-md-12" ForeColor="#333333" GridLines="None" EnablePersistedSelection="True" OnRowCreated="grdIndividuals_RowCreated" OnSelectedIndexChanged="grdIndividuals_SelectedIndexChanged" OnRowCommand="grdIndividuals_RowCommand">
                                    <AlternatingRowStyle BackColor="White" />
                                    <Columns>
                                        <asp:BoundField DataField="first_name" HeaderText="Individual" SortExpression="first_name" />
                                        <asp:BoundField DataField="last_name" HeaderText="Name" SortExpression="last_name" />
                                    </Columns>
                                    <EditRowStyle BackColor="#2461BF" />
                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                    <RowStyle BackColor="#EFF3FB" />
                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                    <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                    <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                    <SortedDescendingHeaderStyle BackColor="#4870BE" />
                                </asp:GridView>
                                <asp:SqlDataSource ID="SqlDatabase" runat="server" ConnectionString="<%$ ConnectionStrings:dcpConnectionString %>" SelectCommand="GetIndividuals" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:ControlParameter ControlID="cmbHousehold" Name="household_id" PropertyName="SelectedValue" Type="Int32" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </div>
                        </div>
                        <div class="col-md-4 col-xs-4 col-lg-4" style="border-left: 1px solid #ddd;">
                            <div class="form-group">
                                <label>Individual's Details</label>
                            </div>
                            <div class="row">
                                <label class="col-md-4 font-9">First Name(s)</label>
                                <div class="col-md-8">
                                    <input id="txtFirstName" runat="server" type="text" name="" value="">
                                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator5" InitialValue="" ControlToValidate="txtFirstName" ValidationGroup="vgIndividuals" ErrorMessage="<span  class='required_alert'>*</span>" />
                                </div>
                                <div class="clearfix"></div>
                            </div>
                            <div class="row">
                                <label class="col-md-4 font-9">Middle Name</label>
                                <div class="col-md-8">
                                    <input id="txtMiddleName" runat="server" type="text" name="" value="">
                                </div>
                                <div class="clearfix"></div>
                            </div>
                            <div class="row">
                                <label class="col-md-4 font-9">Last Name(s)*</label>
                                <div class="col-md-8">
                                    <input id="txtLastName" runat="server" type="text" name="" value="">
                                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator6" InitialValue="" ControlToValidate="txtLastName" ValidationGroup="vgIndividuals" ErrorMessage="<span  class='required_alert'>*</span>" />
                                </div>
                                <div class="clearfix"></div>
                            </div>
                            <div class="row">
                                <label class="col-md-4 font-9">Gender</label>
                                <div class="col-md-8">
                                    <select id="cmbGender" runat="server" name="">
                                        <option value="0">Male</option>
                                        <option value="1">Female</option>
                                    </select>
                                </div>
                                <div class="clearfix"></div>
                            </div>
                            <div class="row">
                                <label class="col-md-4 font-9">D.O.B</label>
                                <div class="col-md-8">
                                    <input id="txtDOB" runat="server" type="text">
                                </div>
                                <div class="clearfix"></div>
                            </div>
                            <div class="row">
                                <label class="col-md-4 font-9">Age</label>
                                <div class="col-md-8">
                                    <input id="txtAge" runat="server" type="number" />
                                </div>
                                <div class="clearfix"></div>
                            </div>
                            <div class="row">
                                <label class="col-md-4 font-9">Country of Birth</label>
                                <div class="col-md-8">
                                    <asp:DropDownList ID="cmbCountryBirth" runat="server">
                                    </asp:DropDownList>
                                </div>
                                <div class="clearfix"></div>
                            </div>
                            <div class="row">
                                <label class="col-md-4 font-9">Place of Birth</label>
                                <div class="col-md-8">
                                    <input id="txtPlaceBirth" runat="server" type="text" name="" value="">
                                </div>
                                <div class="clearfix"></div>
                            </div>
                            <div class="row">
                                <label class="col-md-4 font-9">Nationality</label>
                                <div class="col-md-8">
                                    <input id="txtNationality" runat="server" type="text" name="" value="">
                                </div>
                                <div class="clearfix"></div>
                            </div>
                            <div class="row">
                                <label class="col-md-4 font-9">National ID</label>
                                <div class="col-md-8">
                                    <input id="txtNationalId" runat="server" type="text" name="" value="">
                                </div>
                                <div class="clearfix"></div>
                            </div>
                            <div class="row">
                                <label class="col-md-4 font-9">Contact Phone</label>
                                <div class="col-md-8">
                                    <input id="txtContactPhone" runat="server" type="text" name="" value="">
                                </div>
                                <div class="clearfix"></div>
                            </div>
                            <div class="row">
                                <label class="col-md-4 font-9">Physical Address</label>
                                <div class="col-md-8">
                                    <textarea id="txtPhysicalAddrss" runat="server" name="" style="width: 90%; border: 1px solid #ddd;"></textarea>
                                </div>
                                <div class="clearfix"></div>
                            </div>
                            <div class="row">
                                <label class="col-md-4 font-9">Email</label>
                                <div class="col-md-8">
                                    <input id="txtEmail" runat="server" type="text" name="" value="">
                                </div>
                                <div class="clearfix"></div>
                            </div>
                            <div class="row">
                                <label class="col-md-4 font-9">Marital Status</label>
                                <div class="col-md-8">
                                    <asp:DropDownList ID="cmbMarital" runat="server">
                                    </asp:DropDownList>
                                </div>
                                <div class="clearfix"></div>
                            </div>
                            <div class="row">
                                <label class="col-md-4 font-9">Role</label>
                                <div class="col-md-8">
                                    <asp:DropDownList ID="cmbRole" runat="server">
                                    </asp:DropDownList>
                                </div>
                                <div class="clearfix"></div>
                            </div>
                            <div class="row">
                                <label class="col-md-4 font-9">Is Bread Winner</label>
                                <div class="col-md-8">
                                    <input id="chkIsBreadWinner" runat="server" type="checkbox" name="" value="" style="width: 0px;">
                                </div>
                                <div class="clearfix"></div>
                            </div>
                            <div class="row">
                                <label class="col-md-4 font-9">Level of Education</label>
                                <div class="col-md-8">
                                    <asp:DropDownList ID="cmbLevelEducation" runat="server">
                                    </asp:DropDownList>
                                </div>
                                <div class="clearfix"></div>
                            </div>
                            <div class="row">
                                <label class="col-md-4 font-9">Occupation</label>
                                <div class="col-md-8">
                                    <asp:DropDownList ID="cmbOccupation" runat="server">
                                    </asp:DropDownList>
                                </div>
                                <div class="clearfix"></div>
                            </div>
                            <div class="row">
                                <label class="col-md-4 font-9">Employer</label>
                                <div class="col-md-8">
                                    <input id="txtEmployer" runat="server" type="text" name="" value="">
                                </div>
                                <div class="clearfix"></div>
                            </div>
                            <div class="row">
                                <label class="col-md-4 font-9">Comments</label>
                                <div class="col-md-8">
                                    <textarea id="txtIndividualComment" runat="server" name="" style="width: 90%; border: 1px solid #ddd;"></textarea>
                                </div>
                                <div class="clearfix"></div>
                            </div>
                            <div class="row">
                                <label class="col-md-4 font-9">Displacement Status</label>
                                <div class="col-md-8">
                                    <asp:DropDownList ID="cmbDisplacementIndi" runat="server">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator7" InitialValue="-1" ControlToValidate="cmbDisplacementIndi" ValidationGroup="vgIndividuals" ErrorMessage="<span  class='required_alert'>*</span>" />
                                </div>
                                <div class="clearfix"></div>
                            </div>
                            <div class="row">
                                <label class="col-md-4 font-9">Vulnerability Level</label>
                                <div class="col-md-8">
                                    <asp:DropDownList ID="cmbVulnerability" runat="server">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator8" InitialValue="-1" ControlToValidate="cmbVulnerability" ValidationGroup="vgIndividuals" ErrorMessage="<span  class='required_alert'>*</span>" />
                                </div>
                                btnAssistanceSave
                                <div class="clearfix"></div>
                            </div>
                            <div class="row text-right" style="padding-right: 35px; padding-top: 20px; padding-bottom: 30px;">
                                <asp:Button ID="btnIndividualSave" runat="server" CssClass="btn btn-xs" Width="80" BackColor="Red" Text="Save" OnClick="btnIndividualSave_Click" ValidationGroup="vgIndividuals" />
                                <asp:Button ID="btnIndividualClear" runat="server" CssClass="btn btn-xs" Width="80" BackColor="Red" Text="Clear" OnClick="btnIndividualClear_Click" CausesValidation="False" />
                                <asp:Button ID="btnIndividualDelete" runat="server" CssClass="btn btn-xs" Width="80" BackColor="Red" Text="Delete" OnClick="btnIndividualDelete_Click" ValidationGroup="a" />
                                <div class="clearfix"></div>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <script type="text/javascript">
        $(document).ready(function () {
            //setDate();
            affected_population_changed();
        });
        //On UpdatePanel Refresh
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        if (prm != null) {
            prm.add_endRequest(function (sender, e) {
                if (sender._postBackSettings.panelsToUpdate != null) {
                    closeModal();
                }
            });
        };

        $('#assistanceModel').on('shown.bs.modal', function () {
            var caseNo = $("#MainContent_lblCaseNo").text();
            $('#<%= lblAssistanceCNo.ClientID %>').text(caseNo);
        });

        function closeModal() {
            $('#assistanceModel').modal("hide");
            $('body').removeClass('modal-open');
            $('.modal-backdrop').remove();
        }

        //$(document).on("focus", "#MainContent_dtDateIncidentOccured,#MainContent_dtDateIncidentReported", function (e) {
        //    $(e.currentTarget).attr("type", "date");
        //});
        //$(document).on("blur", "#MainContent_dtDateIncidentOccured,#MainContent_dtDateIncidentReported", function (e) {
        //    $(e.currentTarget).attr("type", "text");
        //});

        function agegoup_changed() {
            var age4 = eval($("#<%= txtAgegroup4.ClientID %>").val());
            age4 = age4 == undefined ? 0 : age4;
            var age5 = eval($("#<%= txtAgegroup5.ClientID %>").val());
            age5 = age5 == undefined ? 0 : age5;
            var age18 = eval($("#<%= txtAgegroup18.ClientID %>").val());
            age18 = age18 == undefined ? 0 : age18;
            var age31 = eval($("#<%= txtAgegroup31.ClientID %>").val());
            age31 = age31 == undefined ? 0 : age31;
            var age44 = eval($("#<%= txtAgegroup44.ClientID %>").val());
            age44 = age44 == undefined ? 0 : age44;
            var age57 = eval($("#<%= txtAgegroup57.ClientID %>").val());
            age57 = age57 == undefined ? 0 : age57;
            var age65 = eval($("#<%= txtAgegroup65.ClientID %>").val());
            age65 = age65 == undefined ? 0 : age65;
            var sum = eval(age4 + age5 + age18 + age31 + age44 + age57 + age65);
            $("#age_total").html(sum);
        }

        function affected_population_changed() {
            var v11 = eval($("#<%= txtAffectedPopulation11.ClientID %>").val());
            var v12 = eval($("#<%= txtAffectedPopulation12.ClientID %>").val());
            var v13 = eval($("#<%= txtAffectedPopulation13.ClientID %>").val());
            var v14 = eval($("#<%= txtAffectedPopulation14.ClientID %>").val());
          <%--var v15 = eval($("#<%= txtAffectedPopulation15.ClientID %>").val());--%>
            <%--var v16 = eval($("#<%= txtAffectedPopulation16.ClientID %>").val());--%>

            var v21 = eval($("#<%= txtAffectedPopulation21.ClientID %>").val());
            var v22 = eval($("#<%= txtAffectedPopulation22.ClientID %>").val());
            var v23 = eval($("#<%= txtAffectedPopulation23.ClientID %>").val());
            var v24 = eval($("#<%= txtAffectedPopulation24.ClientID %>").val());
            <%--var v25 = eval($("#<%= txtAffectedPopulation25.ClientID %>").val());--%>
            <%--var v26 = eval($("#<%= txtAffectedPopulation26.ClientID %>").val());--%>

            var v31 = eval($("#<%= txtAffectedPopulation31.ClientID %>").val());
            var v32 = eval($("#<%= txtAffectedPopulation32.ClientID %>").val());
            var v33 = eval($("#<%= txtAffectedPopulation33.ClientID %>").val());
            var v34 = eval($("#<%= txtAffectedPopulation34.ClientID %>").val());
           <%-- var v35 = eval($("#<%= txtAffectedPopulation35.ClientID %>").val());--%>
            <%--var v36 = eval($("#<%= txtAffectedPopulation36.ClientID %>").val());--%>

            var v41 = eval($("#<%= txtAffectedPopulation41.ClientID %>").val());
            var v42 = eval($("#<%= txtAffectedPopulation42.ClientID %>").val());
            var v43 = eval($("#<%= txtAffectedPopulation43.ClientID %>").val());
            var v44 = eval($("#<%= txtAffectedPopulation44.ClientID %>").val());
           <%-- var v45 = eval($("#<%= txtAffectedPopulation45.ClientID %>").val());
            var v46 = eval($("#<%= txtAffectedPopulation46.ClientID %>").val());--%>

            var v51 = eval($("#<%= txtAffectedPopulation51.ClientID %>").val());
            var v52 = eval($("#<%= txtAffectedPopulation52.ClientID %>").val());
            var v53 = eval($("#<%= txtAffectedPopulation53.ClientID %>").val());
            var v54 = eval($("#<%= txtAffectedPopulation54.ClientID %>").val());
            <%--var v55 = eval($("#<%= txtAffectedPopulation55.ClientID %>").val());--%>
           <%-- var v56 = eval($("#<%= txtAffectedPopulation56.ClientID %>").val());--%>

            var v61 = eval($("#<%= txtAffectedPopulation61.ClientID %>").val());
            var v62 = eval($("#<%= txtAffectedPopulation62.ClientID %>").val());
            var v63 = eval($("#<%= txtAffectedPopulation63.ClientID %>").val());
            var v64 = eval($("#<%= txtAffectedPopulation64.ClientID %>").val());
           <%-- var v65 = eval($("#<%= txtAffectedPopulation65.ClientID %>").val());--%>
            <%--var v66 = eval($("#<%= txtAffectedPopulation66.ClientID %>").val());--%>

            v11 = v11 == undefined ? 0 : v11;
            v12 = v12 == undefined ? 0 : v12;
            v13 = v13 == undefined ? 0 : v13;
            v14 = v14 == undefined ? 0 : v14;
            //v15 = v15 == undefined ? 0 : v15;
            //v16 = v16 == undefined ? 0 : v16;

            v21 = v21 == undefined ? 0 : v21;
            v22 = v22 == undefined ? 0 : v22;
            v23 = v23 == undefined ? 0 : v23;
            v24 = v24 == undefined ? 0 : v24;
            //v25 = v25 == undefined ? 0 : v25;
            //v26 = v26 == undefined ? 0 : v26;

            v31 = v31 == undefined ? 0 : v31;
            v32 = v32 == undefined ? 0 : v32;
            v33 = v33 == undefined ? 0 : v33;
            v34 = v34 == undefined ? 0 : v34;
            //v35 = v35 == undefined ? 0 : v35;
            //v36 = v36 == undefined ? 0 : v36;

            v41 = v41 == undefined ? 0 : v41;
            v42 = v42 == undefined ? 0 : v42;
            v43 = v43 == undefined ? 0 : v43;
            v44 = v44 == undefined ? 0 : v44;
            //v45 = v45 == undefined ? 0 : v45;
            //v46 = v46 == undefined ? 0 : v46;

            v51 = v51 == undefined ? 0 : v51;
            v52 = v52 == undefined ? 0 : v52;
            v53 = v53 == undefined ? 0 : v53;
            v54 = v54 == undefined ? 0 : v54;
            //v55 = v55 == undefined ? 0 : v55;
            //v56 = v56 == undefined ? 0 : v56;

            v61 = v61 == undefined ? 0 : v61;
            v62 = v62 == undefined ? 0 : v62;
            v63 = v63 == undefined ? 0 : v63;
            v64 = v64 == undefined ? 0 : v64;
            //v65 = v65 == undefined ? 0 : v65;
            //v66 = v66 == undefined ? 0 : v66;

            var h1 = v11 + v12 + v13 + v14;
            var h2 = v21 + v22 + v23 + v24;
            var h3 = v31 + v32 + v33 + v34;
            var h4 = v41 + v42 + v43 + v44;
            var h5 = v51 + v52 + v53 + v54;
            var h6 = v61 + v62 + v63 + v64;

            var v1 = v11 + v21 + v31 + v41 + v51 + v61;
            var v2 = v12 + v22 + v32 + v42 + v52 + v62;
            var v3 = v13 + v23 + v33 + v43 + v53 + v63;
            var v4 = v14 + v24 + v34 + v44 + v54 + v64;
            var v5 = 0;
            var v6 = 0;

            var total = v1 + v2 + v3 + v4 + v5 + v6;

            $("#ap_v1_sum").html(v1);
            $("#ap_v2_sum").html(v2);
            $("#ap_v3_sum").html(v3);
            $("#ap_v4_sum").html(v4);
            //$("#ap_v5_sum").html(v5);
            //$("#ap_v6_sum").html(v6);

            $("#ap_h1_sum").html(h1);
            $("#ap_h2_sum").html(h2);
            $("#ap_h3_sum").html(h3);
            $("#ap_h4_sum").html(h4);
            $("#ap_h5_sum").html(h5);
            $("#ap_h6_sum").html(h6);

            $("#ap_total_sum").html(total);
        }


    </script>

</asp:Content>
