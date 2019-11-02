<%@ Page Title="" Language="C#" MasterPageFile="~/base.Master" AutoEventWireup="true" CodeBehind="error.aspx.cs" Inherits="iom.error" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="width: 100%; padding-top: 30px; text-align: center">
        <asp:Image runat="server" ID="img_error" AlternateText="A system error has occurred" ImageUrl="~/incimages/frustrationok.jpg" Height="230" />
    </div>
</asp:Content>
