<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="referrals.aspx.cs" Inherits="Web.referrals" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("#frnt_uri").attr("value", getClientLeftPart());

            if ($("#is_error").val() == "1") $("#xtradtls").slideDown();

            $("#btnsv").click(function () {
                $("#xtradtls").slideDown();
            });

            $("#btncncl").click(function () {
                $("#xtradtls").slideUp();
            });

            $("#lbl_more").click(function () {
                $("#xtradtls").slideDown();
            });
        });
    </script>

    <style type="text/css">
        #srchbx { width: 100% }
        #srchflds tr th { text-align: left }
        #srchflds tr td { padding: 5px 0px; text-align: left; vertical-align: top }
        #srchdr tr td { padding-top: 5px; text-align: center; vertical-align: top }
        #refhdr { background-color: #FFFFCC }
        #refhdr { border: 2px solid #000099 }
        #refhdr tr td { text-align: center; vertical-align: top; padding: 5px 0px; font-weight: bold }
        .sep { background-color: #000099; padding: 0px; width: 100%; height: 2px }    
        #pstrefs tr th { padding: 5px 0px }  
        #pstrefs tr td { text-align: left; vertical-align: top }
        #btn_rmv { margin-top: 25px }
        #rcorgtb tr td { padding: 5px 0px; text-align: left; vertical-align: top }
        #ref_org_unit { margin-left: 10px }
        #xtradtls { border: 2px solid #000099; padding: 10px; margin-bottom: 20px }
        #xtradtls tr td { padding: 8px 0px; text-align: left; vertical-align: top }
        #fndbar { padding: 10px 0px }
        #reftb tr th { text-align: center }
        #reftb tr td { text-align: left }
        #qsrch tr td { text-align: left; vertical-align: top }
        #qsrch tr th { padding: 2px 0px }
        #srchttlbr tr td { padding: 5px 0px 15px 0px; text-align: left; vertical-align: top }
    </style>

      <div id="xtradtls" style="display: none; margin-top: 20px">
        <p style="font-weight: bold">Send email notification to ...</p>
        <table class="anyblk">
            <tr>
                <td style="width: 200px"><asp:Label runat="server" CssClass="fldlbls" Text="Email To 1" /></td>
                <td><asp:DropDownList runat="server" ID="emailuser" CssClass="drpdwn" /></td>
            </tr>
            <tr>
                <td><asp:Label runat="server" CssClass="fldlbls" Text="Email To 2 (optional)" /></td>
                <td><asp:DropDownList runat="server" ID="emailuser2" CssClass="drpdwn" /></td>
            </tr>
            <tr>
                <td><asp:Label runat="server" CssClass="fldlbls" Text="Copy To (optional)" /></td>
                <td><asp:TextBox runat="server" CssClass="txtfields" ID="copyemail" /></td>
            </tr>
            <tr>
                <td><asp:Label runat="server" CssClass="fldlbls" Text="Subject (required)" /></td>
                <td><asp:TextBox runat="server" CssClass="txtfields" ID="subject" /></td>
            </tr>
            <tr>
                <td><asp:Label runat="server" CssClass="fldlbls" Text="Introductory message (required)" /></td>
                <td><asp:TextBox runat="server" CssClass="memoflds" ID="message" TextMode="MultiLine" Rows="10" Columns="60" /></td>
            </tr>        
            <tr>            
                <td><asp:HiddenField runat="server" ClientIDMode="Static" ID="is_error" /></td>
                <td>
                    <asp:Button runat="server" CssClass="cntxtbttnslst" ID="btncncl" Text="Cancel" CausesValidation="false" />
                    <asp:Button runat="server" CssClass="cntxtbttns" ID="btnok" Text="Continue" 
                        CausesValidation="false" onclick="btnok_Click" />
                </td>
            </tr>
            <tr>
                <td></td>
                <td style="vertical-align: bottom">
                    <asp:Label runat="server" ID="err_label" Text="" CssClass="hghlghtbld" />
                </td>
            </tr>
        </table>
    </div>
    <div id="srchbx">
        <table id="srchttlbr" class="anyblk">
            <tr>
                <td style="width: 523px">
                    <span style="font-weight: bold">Search for individual ...</span>
                </td>
                <td>
                    <asp:Label runat="server" ID="lbl_more" CssClass="hghlghtbld" Text="More ..." ClientIDMode="Static" />
                </td>
            </tr>
        </table>        
        <table id="srchflds" class="anyblk">
            <tr>
                <th>Field</th>
                <th>Search Value</th>
                <th>Field</th>
                <th>Search Value</th>
            </tr>
            <tr>
                <td style="width: 120px"><asp:DropDownList runat="server" ID="drp_srch1" AutoPostBack="true" CausesValidation="false"
                        CssClass="drpdwnw" Width="110px"  /></td>
                <td style="width: 400px"><asp:PlaceHolder runat="server" ID="plchldr1" /></td>
                <td style="width: 120px"><asp:DropDownList runat="server" ID="drp_srch2" AutoPostBack="true" CausesValidation="false"
                        CssClass="drpdwnw" Width="110px" /></td>
                <td><asp:PlaceHolder runat="server" ID="plchldr2" /></td>
            </tr>
            <tr>
                <td><asp:DropDownList runat="server" ID="drp_srch3" CssClass="drpdwnw" AutoPostBack="true" CausesValidation="false"
                        Width="110px" /></td>
                <td><asp:PlaceHolder runat="server" ID="plchldr3" /></td>
                <td><asp:DropDownList runat="server" ID="drp_srch4" CssClass="drpdwnw" AutoPostBack="true" CausesValidation="false"
                        Width="110px" /></td>
                <td><asp:PlaceHolder runat="server" ID="plchldr4" /></td>
            </tr>
        </table>
    </div>
    <div id="fndbar">
        <asp:Button runat="server" ID="fndbttn" CssClass="cntxtbttns" 
            ToolTip="Find individuals" CausesValidation="false" Text="Find" 
            onclick="fndbttn_Click" />
    </div>
    <asp:Panel runat="server" ID="pnl_referral_mode">
        <div id="rforg" style="margin-top: 20px; border-top: 1px solid #DBDBFF; border-bottom: 1px solid #DBDBFF; padding: 10px 0px 15px 0px">
            <table id="rcorgtb" style="width: 100%">
                <tr>
                    <td style="width: 120px"><span style="font-weight: bold" class="fldlbls">Referral Org. Unit</span></td>
                    <td style="width: 400px"><asp:DropDownList runat="server" ID="rec_org_unit" CssClass="mddrpdwn" ClientIDMode="Static" AutoPostBack="true" /></td>
                    <td style="text-align: left; width: 120px"><span class="fldlbls" style="font-weight: bold">Reference No.</span></td>
                    <td><asp:Label runat="server" CssClass="hghlghtbld" ID="lbl_reference_no" /></td>
                </tr>
            </table>
        </div>
        <table id="mvbttns" style="width: 100%; margin-top: 15px">
            <tr>
                <td style="width: 550px"><span style="font-weight: bold">Search results ...</span></td>
                <td><span style="font-weight: bold">Individual(s) referred ...</span></td>
            </tr>
        </table>
        <table>
            <tr>
                <td style="text-align: left; vertical-align: top">                    
                    <asp:ListView runat="server" ID="lstsrch" DataSourceID="lnq_srch" 
                        onitemcommand="lstsrch_ItemCommand">
                        <LayoutTemplate>
                            <table runat="server" id="srchtb" clientidmode="Static" class="anyblk" style="width: 548px">
                                <tr runat="server" class="dtgrd">
                                    <th style="width: 40px"></th>
                                    <th style="width: 120px">Case No.</th>
                                    <th style="width: 160px">Name</th>
                                    <th style="width: 30px">G</th>
                                    <th>Incident</th>
                                </tr>
                                <tr runat="server" id="itemPlaceHolder" />                                
                            </table>
                            <div style="width: 100%; text-align: center; padding-top: 10px; padding-bottom: 10px; margin-top: 10px">
                                <asp:DataPager runat="server" ID="rslts_pgr" PageSize="5" PagedControlID="lstsrch">                
                                    <Fields>
                                        <asp:NumericPagerField ButtonCount="5" PreviousPageText="<<" NextPageText=">>" CurrentPageLabelCssClass="pgrbtn" NumericButtonCssClass="pgrbtn" />
                                    </Fields>
                                </asp:DataPager>
                            </div>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr runat="server" style="background-color: #FFFFEA">
                                <td style="text-align: center"><asp:Button runat="server" ID="btnchk_srch" CommandName="srchselected" CommandArgument='<%# Eval("id").ToString() %>' CssClass="nwbttns" Text=">>" /></td>
                                <td><a href='<%# String.Format("case.aspx?{0}={1}&{2}={3}#affected_groups", inc.IncUtilxs.GetParameterName("id"), Eval("case_id").ToString(), inc.IncUtilxs.GetParameterName("individual_id"), Eval("id").ToString())%>'><%# Eval("case_no") %></a></td>
                                <td><%# Eval("individual_entity_name")%></td>
                                <td><%# Eval("gender").ToString()[0] %></td>
                                <td><%# Eval("incident_type")%></td>                                
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr class="altrw" id="Tr1" runat="server">
                                <td style="text-align: center"><asp:Button runat="server" ID="btnchk_srch" CommandName="srchselected" CommandArgument='<%# Eval("id").ToString() %>' CssClass="nwbttns" Text=">>" /></td>
                                <td><a href='<%# String.Format("case.aspx?{0}={1}&{2}={3}#affected_groups", inc.IncUtilxs.GetParameterName("id"), Eval("case_id").ToString(), inc.IncUtilxs.GetParameterName("individual_id"), Eval("id").ToString())%>'><%# Eval("case_no") %></a></td>
                                <td><%# Eval("individual_entity_name")%></td>
                                <td><%# Eval("gender").ToString()[0] %></td>
                                <td><%# Eval("incident_type")%></td>
                            </tr>
                        </AlternatingItemTemplate>
                        <EmptyDataTemplate>
                            <table runat="server" id="srchtb" clientidmode="Static" class="anyblk" style="width: 548px">
                                <tr id="Tr7" runat="server" class="dtgrd">
                                    <th style="width: 40px"></th>
                                    <th style="width: 120px">Case No.</th>
                                    <th style="width: 160px">Name</th>
                                    <th style="width: 30px">G</th>
                                    <th>Incident</th>
                                </tr>
                            </table>
                        </EmptyDataTemplate>
                    </asp:ListView>
                </td>                
                <td style="text-align: right; vertical-align: top">                    
                    <asp:ListView runat="server" ID="lstrefs" DataSourceID="lnq_refs" 
                        onitemcommand="lstrefs_ItemCommand">                        
                        <LayoutTemplate>
                            <table runat="server" id="reftb" clientidmode="Static" style="width: 548px">
                                <tr id="Tr6" runat="server" style="background-color: #003399; color: #FFFFFF">
                                    <th style="width: 40px"></th>
                                    <th style="width: 120px">Case No.</th>
                                    <th style="width: 160px">Name</th>
                                    <th style="width: 30px">G</th>
                                    <th>Incident</th>
                                </tr>
                                <tr runat="server" id="itemPlaceHolder" />
                            </table>
                            <div style="width: 100%; text-align: center; padding-top: 10px; padding-bottom: 10px; margin-top: 10px">
                                <asp:DataPager runat="server" ID="rslts_pgr" PageSize="5" PagedControlID="lstrefs">                
                                    <Fields>
                                        <asp:NumericPagerField ButtonCount="5" PreviousPageText="<<" NextPageText=">>" CurrentPageLabelCssClass="pgrbtn" NumericButtonCssClass="pgrbtn" />
                                    </Fields>
                                </asp:DataPager>
                            </div>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr id="Tr2" runat="server" style="background-color: #FFFFEA">
                                <td style="text-align: center"><asp:Button runat="server" ID="btn_chkref" CssClass="nwbttns" Text="<<" CommandName="refselected" CommandArgument='<%# Eval("id").ToString() %>' /></td>
                                <td><a href='<%# String.Format("case.aspx?{0}={1}&{2}={3}#affected_groups", inc.IncUtilxs.GetParameterName("id"), Eval("case_id").ToString(), inc.IncUtilxs.GetParameterName("individual_id"), Eval("id").ToString())%>'><%# Eval("case_no") %></a></td>
                                <td><%# Eval("individual_entity_name")%></td>
                                <td><%# Eval("gender").ToString()[0] %></td>
                                <td><%# Eval("incident_type")%></td>                                  
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr class="altrw" id="Tr3" runat="server">
                                <td style="text-align: center"><asp:Button ID="btn_chkref" runat="server" CssClass="nwbttns" Text="<<" CommandName="refselected" CommandArgument='<%# Eval("id").ToString() %>' /></td>
                                <td><a href='<%# String.Format("case.aspx?{0}={1}&{2}={3}#affected_groups", inc.IncUtilxs.GetParameterName("id"), Eval("case_id").ToString(), inc.IncUtilxs.GetParameterName("individual_id"), Eval("id").ToString())%>'><%# Eval("case_no") %></a></td>
                                <td><%# Eval("individual_entity_name")%></td>
                                <td><%# Eval("gender").ToString()[0] %></td>
                                <td><%# Eval("incident_type")%></td>   
                            </tr>
                        </AlternatingItemTemplate>
                        <EmptyDataTemplate>
                            <table runat="server" id="reftb" clientidmode="Static" style="width: 548px">
                                <tr id="Tr6" runat="server" style="background-color: #003399; color: #FFFFFF">
                                    <th style="width: 40px"></th>
                                    <td><a href='<%# String.Format("case.aspx?{0}={1}", inc.IncUtilxs.GetParameterName("id"), Eval("case_id").ToString()) %>'><%# Eval("case_no") %></a></td>
                                    <th style="width: 160px">Name</th>
                                    <th style="width: 30px">G</th>
                                    <th>Incident</th>
                                </tr>
                            </table>
                        </EmptyDataTemplate>
                    </asp:ListView>
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel runat="server" ID="pnl_search">
        <p style="font-weight: bold">Search results ...</p>
        <asp:ListView runat="server" ID="lst_srch_mode" DataSourceID="lnq_rfsrchsrch">
            <LayoutTemplate>
                <table runat="server" class="anyblk" id="qsrch" clientidmode="Static">
                    <tr runat="server" class="dtgrd">
                        <th style="width: 130px">Ref. No.</th>
                        <th style="width: 100px">Date Referred</th>
                        <th style="width: 120px">First Name</th>
                        <th style="width: 120px">Last Name</th>
                        <th style="width: 110px">Referred By</th>
                        <th style="width: 110px">Referred To</th>
                        <th>Incident</th>
                        <th style="width: 120px">Displacement Status</th>
                        <th style="width: 110px">Vulnerability</th>
                    </tr>
                    <tr runat="server" id="itemPlaceHolder"></tr>
                </table>
                <div style="width: 100%; text-align: center; padding-top: 10px; padding-bottom: 10px; margin-top: 10px">
                    <asp:DataPager runat="server" ID="rslts_pgr" PageSize="15" PagedControlID="lst_srch_mode">                
                        <Fields>
                            <asp:NumericPagerField ButtonCount="4" PreviousPageText="<<" NextPageText=">>" CurrentPageLabelCssClass="pgrbtn" NumericButtonCssClass="pgrbtn" />
                        </Fields>
                    </asp:DataPager>
                </div>
            </LayoutTemplate>
            <ItemTemplate>
                <tr runat="server">
                    <td><a href='<%# String.Format("{0}?{1}={2}", this.Request.Url.AbsolutePath, inc.IncUtilxs.GetParameterName("id"), Eval("ref_id")) %>'><%# Eval("referral_no")%></a></td>
                    <td><%# DateTime.Parse(Eval("ref_date").ToString()).ToString("dd/MM/yyyy") %></td>
                    <td><a href='<%# String.Format("case.aspx?{0}={1}&{2}={3}#affected_groups", inc.IncUtilxs.GetParameterName("id"), Eval("case_id").ToString(), inc.IncUtilxs.GetParameterName("individual_id"), Eval("individual_id").ToString())%>'><%# Eval("fnames") %></a></td>
                    <td><a href='<%# String.Format("case.aspx?{0}={1}&{2}={3}#affected_groups", inc.IncUtilxs.GetParameterName("id"), Eval("case_id").ToString(), inc.IncUtilxs.GetParameterName("individual_id"), Eval("individual_id").ToString())%>'><%# Eval("lname") %></a></td>
                    <td><%# Eval("referred_by")%></td>
                    <td><%# Eval("referred_to")%></td>
                    <td><%# Eval("incident_type")%></td>
                    <td><%# Eval("ext_displacement_status")%></td>
                    <td><%# Eval("ext_type_vulnerability")%></td>
                </tr>
            </ItemTemplate>
            <AlternatingItemTemplate>
                <tr id="Tr9" runat="server" class="altrw">
                    <td><a href='<%# String.Format("{0}?{1}={2}", this.Request.Url.AbsolutePath, inc.IncUtilxs.GetParameterName("id"), Eval("ref_id")) %>'><%# Eval("referral_no")%></a></td>
                    <td><%# DateTime.Parse(Eval("ref_date").ToString()).ToString("dd/MM/yyyy") %></td>
                    <td><a href='<%# String.Format("case.aspx?{0}={1}&{2}={3}#affected_groups", inc.IncUtilxs.GetParameterName("id"), Eval("case_id").ToString(), inc.IncUtilxs.GetParameterName("individual_id"), Eval("individual_id").ToString())%>'><%# Eval("fnames") %></a></td>
                    <td><a href='<%# String.Format("case.aspx?{0}={1}&{2}={3}#affected_groups", inc.IncUtilxs.GetParameterName("id"), Eval("case_id").ToString(), inc.IncUtilxs.GetParameterName("individual_id"), Eval("individual_id").ToString())%>'><%# Eval("lname") %></a></td>
                    <td><%# Eval("referred_by")%></td>
                    <td><%# Eval("referred_to")%></td>
                    <td><%# Eval("incident_type")%></td>
                    <td><%# Eval("ext_displacement_status")%></td>
                    <td><%# Eval("ext_type_vulnerability")%></td>
                </tr>
            </AlternatingItemTemplate>
            <EmptyDataTemplate>
                <table id="qsrch" clientidmode="Static" runat="server" class="anyblk">
                    <tr id="Tr8" runat="server" class="dtgrd">
                        <th style="width: 130px">Ref. No.</th>
                        <th style="width: 100px">Date Referred</th>
                        <th style="width: 120px">First Name</th>
                        <th style="width: 120px">Last Name</th>
                        <th style="width: 110px">Referred By</th>
                        <th style="width: 110px">Referred To</th>
                        <th>Incident</th>
                        <th style="width: 120px">Displacement Status</th>
                        <th style="width: 110px">Vulnerability</th>
                    </tr>
                </table>
            </EmptyDataTemplate>
        </asp:ListView>
    </asp:Panel>
    <div class="sep" style="margin-top: 25px; margin-bottom: 25px"></div>
    <p style="font-weight: bold">Previous referrals made by current user ...</p>
    <asp:ListView runat="server" ID="lstpstrefs" DataSourceID="lnq_pstrefs">
        <LayoutTemplate>
            <table id="pstrefs" runat="server" class="anyblk" clientidmode="Static">
                <tr runat="server" class="dtgrd">
                    <th style="width: 150px">Ref. No.</th>                    
                    <th style="width: 150px">Referral To</th>
                    <th style="width: 150px">Individual's Name</th>
                    <th style="width: 50px">Gender</th>
                    <th style="width: 40px">Age</th>
                    <th>Physical Address</th>
                    <th style="width: 140px">Displacement Status</th>
                    <th style="width: 120px">Referral Made By</th>
                </tr>
                <tr runat="server" id="itemPlaceHolder" />
            </table>
            <div style="width: 100%; text-align: center; padding-top: 10px; padding-bottom: 10px; margin-top: 10px">
                <asp:DataPager runat="server" ID="rslts_pgr" PageSize="15" PagedControlID="lstpstrefs">                
                    <Fields>
                        <asp:NumericPagerField ButtonCount="4" PreviousPageText="<<" NextPageText=">>" CurrentPageLabelCssClass="pgrbtn" NumericButtonCssClass="pgrbtn" />
                    </Fields>
                </asp:DataPager>
            </div>
        </LayoutTemplate>
        <ItemTemplate>
            <tr runat="server">
                <td><a href='<%# String.Format("{0}?{1}={2}", this.Request.Url.AbsolutePath, inc.IncUtilxs.GetParameterName("id"), Eval("id").ToString()) %>'><%# Eval("referral_no") %></a></td>
                <td><%# Eval("rec_org")%></td>
                <td><a href='<%# String.Format("case.aspx?{0}={1}&{2}={3}", inc.IncUtilxs.GetParameterName("id"), Eval("case_id").ToString(), inc.IncUtilxs.GetParameterName("individual_id"), Eval("individual_id").ToString())%>'><%# Eval("individual_name")%></a></td>                
                <td><%# Eval("gender")%></td>
                <td><%# inc.IncUtilxs.SilenceZero(Eval("age")) %></td>
                <td><%# Eval("physical_address")%></td>
                <td><%# Eval("idisplacement_status")%></td>
                <td><%# Eval("referral_made_by")%></td>                
            </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr id="Tr4" runat="server" class="altrw">
                <td><a href='<%# String.Format("{0}?{1}={2}", this.Request.Url.AbsolutePath, inc.IncUtilxs.GetParameterName("id"), Eval("id").ToString()) %>'><%# Eval("referral_no") %></a></td>
                <td><%# Eval("rec_org")%></td>
                <td><a href='<%# String.Format("case.aspx?{0}={1}&{2}={3}", inc.IncUtilxs.GetParameterName("id"), Eval("case_id").ToString(), inc.IncUtilxs.GetParameterName("individual_id"), Eval("individual_id").ToString())%>'><%# Eval("individual_name")%></a></td>
                <td><%# Eval("gender")%></td>
                <td><%# inc.IncUtilxs.SilenceZero(Eval("age")) %></td>
                <td><%# Eval("physical_address")%></td>
                <td><%# Eval("idisplacement_status")%></td>
                <td><%# Eval("referral_made_by")%></td>
            </tr>
        </AlternatingItemTemplate>
        <EmptyDataTemplate>
            <table id="pstrefs" runat="server" class="anyblk" clientidmode="Static">
                <tr id="Tr5" runat="server" class="dtgrd">
                    <th style="width: 150px">Ref. No.</th>                    
                    <th style="width: 150px">Referral To</th>
                    <th style="width: 150px">Individual's Name</th>
                    <th style="width: 50px">Gender</th>
                    <th style="width: 40px">Age</th>
                    <th>Physical Address</th>
                    <th style="width: 140px">Displacement Status</th>
                    <th style="width: 120px">Referral Made By</th>   
                </tr>                
            </table>
        </EmptyDataTemplate>
    </asp:ListView>
    <div style="display: none">
        <asp:TextBox runat="server" ID="frnt_uri" ClientIDMode="Static" />
    </div>
    <asp:LinqDataSource runat="server" ID="lnq_srch" 
        ContextTypeName="iom.IncDataClassesDataContext" EntityTypeName="" 
        TableName="VwIndividuals" onselecting="lnq_srch_Selecting">
    </asp:LinqDataSource>
    <asp:LinqDataSource runat="server" ID="lnq_refs" 
        ContextTypeName="iom.IncDataClassesDataContext" EntityTypeName="" 
        TableName="VwReferrals" onselecting="lnq_refs_Selecting">
    </asp:LinqDataSource>
    <asp:LinqDataSource runat="server" ID="lnq_pstrefs" 
        ContextTypeName="iom.IncDataClassesDataContext" EntityTypeName="" 
        TableName="VwReferrals" onselecting="lnq_pstrefs_Selecting">
    </asp:LinqDataSource>
    <asp:LinqDataSource runat="server" ID="lnq_rfsrchsrch" 
        ContextTypeName="iom.IncDataClassesDataContext" EntityTypeName="" 
        onselecting="lnq_rfsrchsrch_Selecting" TableName="VwReferralSearches">
    </asp:LinqDataSource>

</asp:Content>
