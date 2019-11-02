<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="Web.Admin"  EnableEventValidation="false" %>

<!DOCTYPE html>

<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>National Disaster Center</title>

    <asp:PlaceHolder runat="server">
        <%: Scripts.Render("~/bundles/modernizr") %>
    </asp:PlaceHolder>

    <webopt:bundlereference runat="server" path="~/Content/css" />
    <link href="~/favicon.ico" rel="shortcut icon" type="image/x-icon" />

</head>
<body>
    <form runat="server">
        <asp:ScriptManager runat="server">
            <Scripts>
                <%--To learn more about bundling scripts in ScriptManager see https://go.microsoft.com/fwlink/?LinkID=301884 --%>
                <%--Framework Scripts--%>
                <asp:ScriptReference Name="MsAjaxBundle" />
                <asp:ScriptReference Name="jquery" />
                <asp:ScriptReference Name="bootstrap" />
                <asp:ScriptReference Name="respond" />
                <asp:ScriptReference Name="WebForms.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebForms.js" />
                <asp:ScriptReference Name="WebUIValidation.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebUIValidation.js" />
                <asp:ScriptReference Name="MenuStandards.js" Assembly="System.Web" Path="~/Scripts/WebForms/MenuStandards.js" />
                <asp:ScriptReference Name="GridView.js" Assembly="System.Web" Path="~/Scripts/WebForms/GridView.js" />
                <asp:ScriptReference Name="DetailsView.js" Assembly="System.Web" Path="~/Scripts/WebForms/DetailsView.js" />
                <asp:ScriptReference Name="TreeView.js" Assembly="System.Web" Path="~/Scripts/WebForms/TreeView.js" />
                <asp:ScriptReference Name="WebParts.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebParts.js" />
                <asp:ScriptReference Name="Focus.js" Assembly="System.Web" Path="~/Scripts/WebForms/Focus.js" />
                <asp:ScriptReference Name="WebFormsBundle" />
                <%--Site Scripts--%>
            </Scripts>
        </asp:ScriptManager>

        
        <div id="tooplate_wrapper">
	        <div id="tooplate_header">
                <div id="site_title"><h1><a href="#"> NDC National Disaster Centre</a></h1></div>
        
                <div id="tooplate_menu" style="padding-left: 300px;">
                    <ul>
                        <li><a href="~/Default" id="aHomeLink" runat="server">Home</a></li>
                        <li><a href="~/Admin"   id="aCaseLink" runat="server"  class="current">Administrator</a></li>
                        <li><a href="~/Logout" id="a1" runat="server" class="last">Logout</a></li>
                    </ul>    	
                </div> <!-- end of tooplate_menu -->
    
	        </div> <!-- end of forever header -->
    
            <div id="tooplate_main">
                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                    <ContentTemplate>
                        <div class="row" style="margin-top: 0px;">
                            <span class="group-title">User Management</span>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <hr />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:Button ID="btnSave" runat="server" CssClass="btn btn-red btn-xs btn-primary" Text="Save" ValidationGroup="" OnClick="btnSave_Click"/>  
                                <asp:Button ID="btnDelete" runat="server" CssClass="btn btn-red btn-xs btn-primary" Text="Delete" ValidationGroup="" OnClick="btnDelete_Click"/>  
                                <asp:Button ID="btnNew" runat="server" CssClass="btn btn-red btn-xs btn-primary" Text="New" ValidationGroup="" OnClick="btnNew_Click"/>  
                                <div class="pull-right" style="margin-right: 10px;">
                                    <asp:Button ID="btnFilter" runat="server" CssClass="btn btn-red btn-xs btn-primary float_r" Text="Filter" ValidationGroup="vgAddHousehold" Width="50" OnClick="btnFilter_Click"/>
                                    <input id="txtFilter" runat="server" type="text" class="float_r" style="width: 100px; margin-top: -1px; margin-right: 4px;" value=""/>  
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <hr />
                            </div>
                        </div>
                        <div class="row" style="padding-right: 50px;">
                            <div class="col-md-6">
                                <div class="col-md-4" style="text-align: right;">First Name(s)</div>
                                <div class="col-md-8">
                                    <asp:TextBox ID="txtFirstName" runat="server" Height="22px" Width="300"/>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="col-md-4" style="text-align: right;">Last Name</div>
                                <div class="col-md-8">
                                    <asp:TextBox ID="txtLastName" runat="server" Height="22px" Width="300"/>
                                </div>
                            </div>
                        </div>
                        <div class="row" style="padding-right: 50px;">
                            <div class="col-md-6">
                                <div class="col-md-4" style="text-align: right;">User Name</div>
                                <div class="col-md-8">
                                    <asp:TextBox ID="txtUserName" runat="server" Height="22px" Width="300"/>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="col-md-4" style="text-align: right;">User Group</div>
                                <div class="col-md-8">
                                    <asp:DropDownList ID="cmbUserGroup" runat="server" Height="22" Width="300">
                                        <asp:ListItem Value="0">User</asp:ListItem>
                                        <asp:ListItem Value="1">Admin</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="row" style="padding-right: 50px;">
                            <div class="col-md-6">
                                <div class="col-md-4" style="text-align: right;padding-left: 0px;">User's Organization</div>
                                <div class="col-md-8">
                                    <asp:DropDownList ID="cmbOrganization" runat="server" Height="22" Width="300">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="col-md-4" style="text-align: right;">Email</div>
                                <div class="col-md-8">
                                    <asp:TextBox ID="txtEmail" runat="server" Height="22px" Width="300"/>
                                </div>
                            </div>
                        </div>
                        <div class="row" style="padding-right: 50px;">
                            <div class="col-md-6">
                                <div class="col-md-4" style="text-align: right;padding-left: 0px;">Notify New Case</div>
                                <div class="col-md-8">
                                    <asp:CheckBox ID="chkNotifyNewCase" runat="server" />
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="col-md-4" style="text-align: right;">Locked</div>
                                <div class="col-md-8">
                                    <asp:CheckBox ID="chkLocked" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="row" style="padding-right: 50px;">
                            <div class="col-md-6">
                                <div class="col-md-4" style="text-align: right;padding-left: 0px;">Enabled</div>
                                <div class="col-md-8">
                                    <asp:CheckBox ID="chkEnabled" runat="server" />
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="col-md-4" style="text-align: right;"></div>
                                <div class="col-md-8">
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <hr />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12" style="padding-right: 30px;">
                                <asp:GridView ID="grdUser" runat="server" AutoGenerateColumns="False" DataKeyNames="id" DataSourceID="SqlDatabase" CellPadding="4" CssClass="table col-md-12" ForeColor="#333333" GridLines="None" EnablePersistedSelection="True"
                                     OnRowCreated="grdUser_RowCreated" OnSelectedIndexChanged="grdUser_SelectedIndexChanged" OnRowCommand="grdUser_RowCommand">
                                    <AlternatingRowStyle BackColor="White" />
                                    <Columns>
                                        <asp:BoundField DataField="first_name" HeaderText="First Name" SortExpression="first_name" />
                                        <asp:BoundField DataField="last_name" HeaderText="Last Name" SortExpression="last_name" />
                                        <asp:BoundField DataField="user_name" HeaderText="User Name" SortExpression="user_name" />
                                        <asp:BoundField DataField="email" HeaderText="Email" SortExpression="email" />
                                        <asp:BoundField DataField="organ_name" HeaderText="Organization" SortExpression="organ_name" />
                                        <asp:BoundField DataField="user_group" HeaderText="User Group" SortExpression="user_group" ReadOnly="True" />
                                        <asp:BoundField DataField="notify_new_case" HeaderText="Notify" SortExpression="notify_new_case" ReadOnly="True" />
                                        <asp:BoundField DataField="locked" HeaderText="Locked" ReadOnly="True" SortExpression="locked" />
                                        <asp:BoundField DataField="enabled" HeaderText="Enabled" ReadOnly="True" SortExpression="enabled" />
                                    </Columns>
                                    <EditRowStyle BackColor="#2461BF" />
                                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" Height="20px" />
                                    <RowStyle BackColor="#EFF3FB" />
                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                    <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                    <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                    <SortedDescendingHeaderStyle BackColor="#4870BE" />
                                </asp:GridView>
                                <asp:SqlDataSource ID="SqlDatabase" runat="server" ConnectionString="<%$ ConnectionStrings:dcpConnectionString %>" SelectCommand="GetUsersByFilter" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:Parameter DefaultValue="  " Name="filter" Type="String" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div> <!-- end of main -->
    
            <div id="tooplate_footer">
    	        Copyright © 2048 <a href="#">National Disaster Center</a>
	        </div> <!-- end of footer -->
    
        </div> <!-- end of wrapper -->
    </form>
</body>
</html>

