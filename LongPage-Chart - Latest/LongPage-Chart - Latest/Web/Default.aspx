<%@ Page Title="National Disaster Center" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Web._Default" EnableEventValidation="false" %>

<script runat="server">

// Create a variable to store the order total.


</script>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

  
    &nbsp
    
    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
        <ContentTemplate>
            <div class="row">
                <span class="group-title">Recently Reported Cases</span>
                <div class="pull-right" style="margin-right: 30px;">
                    <asp:Button ID="btnFilter" runat="server" CssClass="btn btn-red btn-xs btn-primary float_r" Text="Filter" ValidationGroup="vgAddHousehold" Width="50" OnClick="btnFilter_Click" />
                    <input id="txtFilter" runat="server" type="text" class="float_r" style="width: 100px; margin-top: -1px; margin-right: 4px;" value="" />
                </div>
            </div>
            <div class="row">
                <div class="col-md-12" style="padding-right: 30px;">
                    <asp:GridView ID="grdIndividuals" runat="server" AutoGenerateColumns="False" DataKeyNames="id" DataSourceID="SqlDatabase" CellPadding="4" CssClass="table col-md-12" ForeColor="#333333"
                        GridLines="None" EnablePersistedSelection="True" AllowPaging="True" ShowFooter="true"
                        OnRowDataBound="grdIndividuals_RowDataBound">
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:TemplateField HeaderText="Reported" SortExpression="date_incident_reported">
                                <ItemTemplate>
                                    <%#Eval("date_incident_reported","{0:MM/dd/yyyy}") %>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="TotalLabel" runat="server" Text="Total" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:HyperLinkField DataTextField="incident" DataNavigateUrlFields="Id" DataNavigateUrlFormatString="~/Cases.aspx?Id={0}" HeaderText="Incident" />
                            <asp:BoundField DataField="comment" HeaderText="Narrative" SortExpression="comment" />
                            <asp:BoundField DataField="cause_name" HeaderText="Causes" SortExpression="cause_name" />
                            <asp:BoundField DataField="district_name" HeaderText="District" SortExpression="district_name" />
                            <asp:BoundField DataField="placename" HeaderText="Place Name" SortExpression="placename" />
                            <asp:TemplateField HeaderText="Casualties" SortExpression="casualties">
                                <ItemTemplate>
                                    <%# string.IsNullOrEmpty(Eval("casualties").ToString()) ? "0": Eval("casualties")  %>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="OrderTotalLabel" runat="server" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:BoundField />
                            <asp:BoundField DataField="displacement" HeaderText="Status" ReadOnly="True" SortExpression="displacement" />
                        </Columns>
                        <EditRowStyle BackColor="#2461BF" />
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Middle" />
                        <PagerStyle BackColor="#507CD1" ForeColor="White" HorizontalAlign="Center" Height="20px" />
                        <RowStyle BackColor="#EFF3FB" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#F5F7FB" />
                        <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                        <SortedDescendingCellStyle BackColor="#E9EBEF" />
                        <SortedDescendingHeaderStyle BackColor="#4870BE" />
                    </asp:GridView>
                    <asp:SqlDataSource ID="SqlDatabase" runat="server" ConnectionString="<%$ ConnectionStrings:dcpConnectionString %>" SelectCommand="GetRecentlyReportedCases" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:Parameter DefaultValue=" " Name="filter" Type="String" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="row">
                <span class="group-title">Verified No. of Casualities by Month</span>
            </div>
            <div class="row">
                <div class="col-md-1">
                    <label style="font-size: 8pt; margin-top: 15px;">Filter By:</label>
                </div>
                <div class="col-md-3">
                    <label for="cmbProvince">Province</label>
                    <asp:DropDownList ID="cmbProvince" runat="server" CssClass="col-md-12 padding-0">
                    </asp:DropDownList>
                </div>
                <div class="col-md-3">
                    <label for="cmbYear">Year</label>
                    <asp:DropDownList ID="cmbYear" runat="server" CssClass="col-md-12 padding-0">
                    </asp:DropDownList>
                </div>
                <div class="col-md-3">
                    <label for="cmbDisplacement">Displacement Status</label>
                    <asp:DropDownList ID="cmbDisplacement" runat="server" CssClass="col-md-12 padding-0">
                    </asp:DropDownList>
                </div>
                <div class="col-md-2" style="padding-top: 12px;">
                    <asp:Button ID="btnVerifiedNoFilter" runat="server" CssClass="btn btn-red btn-xs btn-primary" Text="Filter" ValidationGroup="vgAddHousehold" />
                </div>
            </div>
            <div class="row">
                <div class="col-md-12" style="padding-right: 30px;">
                    <asp:GridView ID="grdCasualtiesByMonth" runat="server" AutoGenerateColumns="False" DataKeyNames="id" DataSourceID="SqlDataSourceCasualties" CellPadding="4"
                        CssClass="table col-md-12" ForeColor="#333333" GridLines="None" EnablePersistedSelection="True" ShowFooter="true" OnRowDataBound="grdCasualtiesByMonth_RowDataBound">
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:TemplateField HeaderText="Incident" SortExpression="incident">
                                <ItemTemplate>
                                    <%#Eval("incident") %>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="OrderTotalLabel" runat="server" Text="Total" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Jan" SortExpression="incident_1">
                                <ItemTemplate>
                                    <%#Eval("incident_1") %>
                                </ItemTemplate>
                                <FooterTemplate></FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Feb" SortExpression="incident_2">
                                <ItemTemplate>
                                    <%#Eval("incident_2") %>
                                </ItemTemplate>
                                <FooterTemplate></FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Mar" SortExpression="incident_3">
                                <ItemTemplate>
                                    <%#Eval("incident_3") %>
                                </ItemTemplate>
                                <FooterTemplate></FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Apr" SortExpression="incident_4">
                                <ItemTemplate>
                                    <%#Eval("incident_4") %>
                                </ItemTemplate>
                                <FooterTemplate></FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="May" SortExpression="incident_5">
                                <ItemTemplate>
                                    <%#Eval("incident_5") %>
                                </ItemTemplate>
                                <FooterTemplate></FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Jun" SortExpression="incident_6">
                                <ItemTemplate>
                                    <%#Eval("incident_6") %>
                                </ItemTemplate>
                                <FooterTemplate></FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Jul" SortExpression="incident_7">
                                <ItemTemplate>
                                    <%#Eval("incident_7") %>
                                </ItemTemplate>
                                <FooterTemplate></FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Aug" SortExpression="incident_8">
                                <ItemTemplate>
                                    <%#Eval("incident_8") %>
                                </ItemTemplate>
                                <FooterTemplate></FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Sep" SortExpression="incident_9">
                                <ItemTemplate>
                                    <%#Eval("incident_9") %>
                                </ItemTemplate>
                                <FooterTemplate></FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Oct" SortExpression="incident_10">
                                <ItemTemplate>
                                    <%#Eval("incident_10") %>
                                </ItemTemplate>
                                <FooterTemplate></FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Nov" SortExpression="incident_11">
                                <ItemTemplate>
                                    <%#Eval("incident_11") %>
                                </ItemTemplate>
                                <FooterTemplate></FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Dec" SortExpression="incident_12">
                                <ItemTemplate>
                                    <%#Eval("incident_12") %>
                                </ItemTemplate>
                                <FooterTemplate></FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EditRowStyle BackColor="#2461BF" />
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White"/>
                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#EFF3FB" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#F5F7FB" />
                        <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                        <SortedDescendingCellStyle BackColor="#E9EBEF" />
                        <SortedDescendingHeaderStyle BackColor="#4870BE" />
                    </asp:GridView>
                    <asp:SqlDataSource ID="SqlDataSourceCasualties" runat="server" ConnectionString="<%$ ConnectionStrings:dcpConnectionString %>" SelectCommand="GetCasualtiesByMonth" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="cmbProvince" Name="province_id" PropertyName="SelectedValue" Type="Int32" />
                            <asp:ControlParameter ControlID="cmbYear" Name="year" PropertyName="SelectedValue" Type="Int32" />
                            <asp:ControlParameter ControlID="cmbDisplacement" Name="displacement_id" PropertyName="SelectedValue" Type="Int32" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <div class="row">
                <span class="group-title">Verified No. of Casualties by Province</span>
            </div>
            <div class="row">
                <div class="col-md-1">
                    <label style="font-size: 8pt; margin-top: 15px;">Filter By:</label>
                </div>
                <div class="col-md-3">
                    <label for="cmbProvince">Year</label>
                    <asp:DropDownList ID="cmbCasualtiesYear" runat="server" CssClass="col-md-12 padding-0">
                    </asp:DropDownList>
                </div>
                <div class="col-md-3">
                    <label for="cmbYear">Month</label>
                    <asp:DropDownList ID="cmbCasualtiesMonth" runat="server" CssClass="col-md-12 padding-0">
                    </asp:DropDownList>
                </div>
                <div class="col-md-3">
                    <label for="cmbDisplacement">Displacement Status</label>
                    <asp:DropDownList ID="cmbCasualtiesDisplacement" runat="server" CssClass="col-md-12 padding-0">
                    </asp:DropDownList>
                </div>
                <div class="col-md-2" style="padding-top: 12px;">
                    <asp:Button ID="Button1" runat="server" CssClass="btn btn-red btn-xs btn-primary" Text="Filter" ValidationGroup="vgAddHousehold" />
                </div>
            </div>
            <div class="row">
                <div class="col-md-12" style="padding-right: 30px;">
                    <asp:GridView ID="grdCasualtiesByProvince" runat="server" AutoGenerateColumns="False" DataKeyNames="id" DataSourceID="SqlDataSourceCasualtiesProvince" CellPadding="4" CssClass="table col-md-12" ForeColor="#333333"
                        GridLines="None" EnablePersistedSelection="True" ShowFooter="true" OnRowDataBound="grdCasualtiesByProvince_RowDataBound">
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
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
                    <asp:SqlDataSource ID="SqlDataSourceCasualtiesProvince" runat="server" ConnectionString="<%$ ConnectionStrings:dcpConnectionString %>" SelectCommand="GetCasualtiesByProvince" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="cmbCasualtiesYear" Name="year" PropertyName="SelectedValue" Type="String" />
                            <asp:ControlParameter ControlID="cmbCasualtiesMonth" Name="month" PropertyName="SelectedValue" Type="String" />
                            <asp:ControlParameter ControlID="cmbCasualtiesDisplacement" Name="displacement_id" PropertyName="SelectedValue" Type="String" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
        <ContentTemplate>
            <div class="row">
                <span class="group-title">Verified No. of Incidents by Month</span>
            </div>
            <div class="row">


                <div class="col-md-1">
                    <label style="font-size: 8pt; margin-top: 15px;">Filter By:</label>
                </div>
                <div class="col-md-3">
                    <label for="cmbProvince">Province</label>
                    <asp:DropDownList ID="cmbIncidentProvince" runat="server" CssClass="col-md-12 padding-0">
                    </asp:DropDownList>
                </div>
                <div class="col-md-3">
                    <label for="cmbYear">Year</label>
                    <asp:DropDownList ID="cmbIncidentYear" runat="server" CssClass="col-md-12 padding-0">
                    </asp:DropDownList>
                </div>
                <div class="col-md-3">
                    <label for="cmbDisplacement">Displacement Status</label>
                    <asp:DropDownList ID="cmbIncidentsDisplacement" runat="server" CssClass="col-md-12 padding-0">
                    </asp:DropDownList>
                </div>
                <div class="col-md-2" style="padding-top: 12px;">
                    <asp:Button ID="Button2" runat="server" CssClass="btn btn-red btn-xs btn-primary" Text="Filter" ValidationGroup="vgAddHousehold" />
                </div>
            </div>
            <div class="row">
                <div class="col-md-12" style="padding-right: 30px;">
                    <asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="False" DataKeyNames="id" DataSourceID="SqlDataSourceIncidents" CellPadding="4" CssClass="table col-md-12"
                        ForeColor="#333333" GridLines="None" EnablePersistedSelection="True" ShowFooter="true" OnRowDataBound="GridView3_RowDataBound">
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:TemplateField HeaderText="Incident" SortExpression="incident">
                                <ItemTemplate>
                                    <%#Eval("incident") %>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="OrderTotalLabel" runat="server" Text="Total" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Jan" SortExpression="incident_1">
                                <ItemTemplate>
                                    <%#Eval("incident_1") %>
                                </ItemTemplate>
                                <FooterTemplate></FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Feb" SortExpression="incident_2">
                                <ItemTemplate>
                                    <%#Eval("incident_2") %>
                                </ItemTemplate>
                                <FooterTemplate></FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Mar" SortExpression="incident_3">
                                <ItemTemplate>
                                    <%#Eval("incident_3") %>
                                </ItemTemplate>
                                <FooterTemplate></FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Apr" SortExpression="incident_4">
                                <ItemTemplate>
                                    <%#Eval("incident_4") %>
                                </ItemTemplate>
                                <FooterTemplate></FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="May" SortExpression="incident_5">
                                <ItemTemplate>
                                    <%#Eval("incident_5") %>
                                </ItemTemplate>
                                <FooterTemplate></FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Jun" SortExpression="incident_6">
                                <ItemTemplate>
                                    <%#Eval("incident_6") %>
                                </ItemTemplate>
                                <FooterTemplate></FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Jul" SortExpression="incident_7">
                                <ItemTemplate>
                                    <%#Eval("incident_7") %>
                                </ItemTemplate>
                                <FooterTemplate></FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Aug" SortExpression="incident_8">
                                <ItemTemplate>
                                    <%#Eval("incident_8") %>
                                </ItemTemplate>
                                <FooterTemplate></FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Sep" SortExpression="incident_9">
                                <ItemTemplate>
                                    <%#Eval("incident_9") %>
                                </ItemTemplate>
                                <FooterTemplate></FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Oct" SortExpression="incident_10">
                                <ItemTemplate>
                                    <%#Eval("incident_10") %>
                                </ItemTemplate>
                                <FooterTemplate></FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Nov" SortExpression="incident_11">
                                <ItemTemplate>
                                    <%#Eval("incident_11") %>
                                </ItemTemplate>
                                <FooterTemplate></FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Dec" SortExpression="incident_12">
                                <ItemTemplate>
                                    <%#Eval("incident_12") %>
                                </ItemTemplate>
                                <FooterTemplate></FooterTemplate>
                            </asp:TemplateField>
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
                    <asp:SqlDataSource ID="SqlDataSourceIncidents" runat="server" ConnectionString="<%$ ConnectionStrings:dcpConnectionString %>" SelectCommand="GetIncidentsByMonth" SelectCommandType="StoredProcedure">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="cmbIncidentProvince" Name="province_id" PropertyName="SelectedValue" Type="Int32" />
                            <asp:ControlParameter ControlID="cmbIncidentYear" Name="year" PropertyName="SelectedValue" Type="Int32" />
                            <asp:ControlParameter ControlID="cmbIncidentsDisplacement" Name="displacement_id" PropertyName="SelectedValue" Type="Int32" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

