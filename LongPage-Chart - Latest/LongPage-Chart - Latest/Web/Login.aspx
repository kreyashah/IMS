<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Web.Login" %>

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

	        <div style="margin-top: 150px; width: 350px; height: 230px; border: 1px solid #ddd; border-radius: 0px;margin-left: auto; margin-right: auto;background: rgb(252,252,252);box-shadow: 0 0 4px 0 rgba(0,0,0,.08),0 2px 4px 0 rgba(0,0,0,.12);">
                
                <div id="site_title" style="margin-left: 0px; margin-right: 0px;"><h1><a>NDC National Disaster Centre</a></h1></div>
                <div class="clearfix"></div>
                <fieldset>
                    <br />
                    <div class='container' style="margin-left: 20px;">
                        <asp:Label ID="Name" runat="server" Text="UserName:" CssClass="lbl" Width="80"/>
                        <asp:TextBox ID="txtUserName" runat="server" Height="22px" Width="200"/>
                    </div>
                    <div class='container' style="margin-top: 10px;margin-left: 20px;">
                        <asp:Label ID="lblPwd" runat="server" Text="Password:" CssClass="lbl"  Width="80"/>
                        <asp:TextBox ID="txtPwd" runat="server" TextMode="Password" CssClass="pwd" Height="22px" Width="200"/>
                    </div>

                    <div class='container' style="margin-top: 20px;padding-left: 150px;">
                        <asp:Button ID="btnLogIn" runat="server" Text="Sign In" OnClick="btnLogIn_Click" Width="150"/>
                    </div>
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="error"/>
                    <br /><br />
                    <asp:Label ID="lblMsg" runat="server" Text="" CssClass="lbl"/>
                </fieldset>
            </div>
        </div> <!-- end of wrapper -->
    </form>
</body>
</html>