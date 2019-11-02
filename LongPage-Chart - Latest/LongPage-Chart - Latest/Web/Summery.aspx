<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="Summery.aspx.cs" Inherits="Web.Summery" EnableEventValidation="false" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row" style="margin-top: 0px;">
        <span class="group-title">Summary Reports</span>
    </div>
    <div class="row" style="padding-left: 20px;">
        <asp:Button ID="btnExport" runat="server" CssClass="btn btn-red btn-xs btn-primary" Text="OK" ValidationGroup="vgAddHousehold" Width="50" OnClick="btnExport_Click"/>
    </div>
    <div class="row">
        <div class="col-md-6">
            <div class="row">
                <input type="radio" id="rdoCasualtiesList" runat="server" />Casualties List
            </div>
            <div class="row">
                <input type="radio" id="rdoNumberOfCasualtiesPeriod" runat="server" />Number of casualties for the period
            </div>
            <div class="row">
                <input type="radio" id="rdoDamagedInfrastructure" runat="server" />Damaged Infrastructure
            </div>
            <div class="row">
                <input type="radio" id="rdoAccesstoEssentialServices" runat="server" />Access of Essential Service
            </div>
            <div class="row">
                <input type="radio" id="rdoListofpeopleaffected" runat="server" />List of people affected
            </div>
            <div class="row">
                <input type="radio" id="rdoDisasterincidentsandcauses" runat="server" />Disaster - incidents and causes
            </div>
            <%--<div class="row">
                <input type="radio" id="rdoRunningAssistanceProvided" runat="server" />Running assistance provided to affected people
            </div>
            <div class="row">
                <input type="radio" id="rdoDisastersIncidents" runat="server" />Disasters/incidents and their causes
            </div>--%>
            <div class="row">
                <input type="radio" id="rdoAmountOfFunds" runat="server" />Est. amount of funds used by type or assistance
            </div>
            <div class="row">
                <input type="radio" id="rdoIncidentAndCauses" runat="server" />Incident And Causes
            </div>
        </div>
        <div class="col-md-6">
            <div class="col-md-12">
                <div class="row">
                    <div class="col-md-6">
                        <label>Period From(M/Y)</label>
                    </div>
                    <div class="col-md-6">
                        <label>Period To(M/Y)</label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">            
                        <asp:DropDownList ID="cmbYearFrom" runat="server"  CssClass="col-md-5 padding-0 float_l">
                        </asp:DropDownList>
                        <span class="float_l">/</span>
                        <asp:DropDownList ID="cmbMonthFrom" runat="server"  CssClass="col-md-6 padding-0 float_l">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-6">  
                        <asp:DropDownList ID="cmbYearTo" runat="server"  CssClass="col-md-5 padding-0 float_l">
                        </asp:DropDownList>
                        <span class="float_l">/</span>
                        <asp:DropDownList ID="cmbMonthTo" runat="server"  CssClass="col-md-6 padding-0 float_l">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <label>Filter by Province</label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:DropDownList ID="cmbProvince" runat="server"  CssClass="col-md-12 padding-0">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <label>Filter by Displacement Status</label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">
                        <asp:DropDownList ID="cmbDisplacement" runat="server"  CssClass="col-md-12 padding-0">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
