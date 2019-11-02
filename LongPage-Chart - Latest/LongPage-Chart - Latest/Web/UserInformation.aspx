<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="UserInformation.aspx.cs" Inherits="Web.UserInformation" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>User Information</h2>
    <p>Here you can see your user informations. You can change your login password.</p>
    <div class="cleaner h40"></div>
        
           
    <form method="post" name="contact" action="#">
        <div class="col_w420 float_l">
            <div id="contact_form">
            
                <h4>Contact Information</h4>

				    <label for="txtFirstName" class="contact-label">FirstName:</label> <input type="text" id="txtFirstName" runat="server" name="author" class="input_field" readonly />
				    <div class="cleaner h10"></div>
				    <label for="txtLastName" class="contact-label">LastName:</label> <input type="text" id="txtLastName" runat="server" name="author" class="input_field" readonly />
				    <div class="cleaner h10"></div>
				    <label for="txtUserName" class="contact-label">UserName:</label> <input type="text" id="txtUserName" runat="server" name="author" class="input_field" readonly />
				    <div class="cleaner h10"></div>
				    <label for="txtEmail" class="contact-label">Email:</label> <input type="text" id="txtEmail" runat="server" name="author" class="input_field" readonly />
				    <div class="cleaner h10"></div>
				    <label for="txtOraganization" class="contact-label">Organization:</label> <input type="text" id="txtOraganization" runat="server" name="author" class="input_field" readonly />
				    <div class="cleaner h10"></div>
				    <label for="txtUsergroup" class="contact-label">User Group:</label> <input type="text" id="txtUsergroup" runat="server" name="author" class="input_field" readonly />
				    <div class="cleaner h10"></div>
                    
            </div> 
        </div>
            
        <div class="col_w420 float_r">
            <h4>Change Password</h4>
            
                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                    <ContentTemplate>
                        <label for="txtFirstName" class="contact-label">Old Passowrd:</label> <input type="password" id="txtOldPwd" runat="server" name="author" class="input_field" />
				        <div class="cleaner h10"></div>
				        <label for="txtFirstName" class="contact-label">New Password:</label> <input type="password" id="txtNewPwd" runat="server" name="author" class="input_field" />
				        <div class="cleaner h10"></div>
				        <label for="txtFirstName" class="contact-label">Confirm Password:</label> <input type="password" id="txtConfirmPwd" runat="server" name="author" class="input_field" />
				        <div class="cleaner h10"></div>
                                            
                        <asp:Button ID="btnSave" runat="server" CssClass="btn btn-sm btn-primary submit_btn float_r" Text="Change" ValidationGroup="" OnClick="btnSave_Click"/>  
                        <asp:Button ID="btnReset" runat="server" CssClass="btn btn-sm submit_btn float_r" Text="Reset" ValidationGroup="" OnClick="btnReset_Click1"/>  
                        
                        <div class="cleaner h10"></div>
				        <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label> 
				        <div class="cleaner h10"></div>
                                
                    </ContentTemplate>
                </asp:UpdatePanel>
		
        </div>
    </form>
            
    <div class="cleaner"></div>
</asp:Content>
