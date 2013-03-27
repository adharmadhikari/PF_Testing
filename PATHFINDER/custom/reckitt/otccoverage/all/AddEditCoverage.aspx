<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Modal.master" AutoEventWireup="true" CodeFile="AddEditCoverage.aspx.cs" Inherits="custom_reckitt_otccoverage_AddEditCoverage"  EnableViewState="true"%>
<%@ OutputCache Duration="1" VaryByParam="None" NoStore="true"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<!--[if IE 8]>
        <style type="text/css">
         .divmain
         {
 	        height: 520px;
 	        overflow: scroll !important;
         }
         
         .modalFormButtonsOTC
        {
	        padding:10px 10px 0 0;
	        text-align:right;
        }

        .modalFormButtonsOTC .coreBtn 
        {
	        background-position:0 0;
	        margin-left:7px;
	        padding-bottom:5px;
	        background:transparent url(../../../../App_Themes/pathfinder/images/btnLf.gif) no-repeat scroll 0 0;
        }
        .modalFormButtonsOTC .coreBtn, .modalFormButtonsOTC .coreBtn * 
        {
	        display:inline-block;
	        height:18px;
        }
        .modalFormButtonsOTC .coreBtn .bg 
        {
	        background-position: top right;
	        background-repeat:no-repeat;
	        padding-right:9px;
	        height:18px;
        }
        .modalFormButtonsOTC .coreBtn .bg2 
        {
	        background-position:0 -52px;
	        background-repeat:repeat-x;
	        height:18px;
        }
        .modalFormButtonsOTC .coreBtn input
        {
	        border:none 0px;
	        background:none;
	        height:18px;
	        line-height:12px;
	        background:url(../../../../App_Themes/pathfinder/images/btnBg.gif) !important;
        }
        .modalFormButtonsOTC .coreBtn span 
        {
	        background-image:url(../../../../App_Themes/pathfinder/images/btnRt.gif);
        }
        </style>
    <![endif]-->  
    <!--[if IE 7]>
        <style type="text/css">
         .divmain
         {
 	        height: 520px;
 	        overflow-y: scroll;
            overflow-x: hidden;
         }
         
         .modalFormButtonsOTC
        {
	        padding:10px 10px 0 0;
	        text-align:right;
        }

        .modalFormButtonsOTC .coreBtn 
        {
	        background-position:0 0;
	        margin-left:7px;
	        padding-bottom:5px;
	        background:transparent url(../../../../App_Themes/pathfinder/images/btnLf.gif) no-repeat scroll 0 0;
        }
        .modalFormButtonsOTC .coreBtn, .modalFormButtonsOTC .coreBtn * 
        {
	        display:inline-block;
	        height:18px;
        }
        .modalFormButtonsOTC .coreBtn .bg 
        {
	        background-position: top right;
	        background-repeat:no-repeat;
	        padding-right:9px;
	        height:18px;
        }
        .modalFormButtonsOTC .coreBtn .bg2 
        {
	        background-position:0 -52px;
	        background-repeat:repeat-x;
	        height:18px;
        }
        .modalFormButtonsOTC .coreBtn input
        {
	        border:none 0px;
	        background:none;
	        height:18px;
	        line-height:12px;
	        background:url(../../../../App_Themes/pathfinder/images/btnBg.gif) !important;
        }
        .modalFormButtonsOTC .coreBtn span 
        {
	        background-image:url(../../../../App_Themes/pathfinder/images/btnRt.gif);
        }
        </style>
    <![endif]-->
    <!--[if lt IE 7]>
        <style type="text/css">
         .divmain
         {
 	        width:100% !important;
            overflow-y: auto;
            overflow-x: hidden;
            height:520px;
         }
         
         #ctl00_main_formViewOTC td .modalFormButtonsOTC 
         {
    	    padding:10px 10px 0 0;
	        text-align:right;
         }

        #ctl00_main_formViewOTC td .modalFormButtonsOTC .coreBtn 
        {
            background-position:0 0;
	        margin-left:7px;
	        padding-bottom:5px;
	        background:transparent url(../../../../App_Themes/pathfinder/images/btnLf.gif) no-repeat scroll 0 0;
        }
        
        #ctl00_main_formViewOTC td .modalFormButtonsOTC .coreBtn, .modalFormButtonsOTC .coreBtn * 
        {
            display:inline-block;
	        height:18px;
        }
        
        #ctl00_main_formViewOTC td .modalFormButtonsOTC .coreBtn .bg 
        {
            background-position: top right;
	        background-repeat:no-repeat;
	        padding-right:9px;
	        height:18px;
        }
        
        #ctl00_main_formViewOTC td .modalFormButtonsOTC .coreBtn .bg2 
        {
            background-position:0 -52px;
	        background-repeat:repeat-x;
	        height:18px;
        }
        
        #ctl00_main_formViewOTC td .modalFormButtonsOTC .coreBtn input
        {
            border:none 0px;
	        background:none;
	        height:18px;
	        line-height:12px;
	        background:url(../../../../App_Themes/pathfinder/images/btnBg.gif) !important;
        }
        
        #ctl00_main_formViewOTC td .modalFormButtonsOTC .coreBtn span 
        {
	       background-image:url(../../../../App_Themes/pathfinder/images/btnRt.gif);
        }

        </style>
    <![endif]-->
 <script type="text/javascript">
      function ClearForm()
     {
         //Reset the form values.
         document.forms[0].reset();
         
         //Enable/Disable textarea based on associated checkbox's status.
         EnableDisableText($get("ctl00_main_formViewOTC_Product_Other"));
         EnableDisableText($get("ctl00_main_formViewOTC_Is_CoughCold"));
         EnableDisableText($get("ctl00_main_formViewOTC_Is_Health_Fairs"));
         EnableDisableText($get("ctl00_main_formViewOTC_Is_Education_Brochures"));
         EnableDisableText($get("ctl00_main_formViewOTC_Is_Other"));

         //To reset all validation controls.
         $(".validatorcss").css("visibility", "hidden");
     }

    //Refresh OTC Coverage grid after every insert/update.
     function RefreshOTC()
     {
         getOTCGrid().control.get_masterTableView().rebind();

         //window.setTimeout(CloseWin, 4000);
         CloseWin();
     }

     function CloseWin()
     {
         var manager = window.top.GetRadWindowManager();

         var window1 = manager.getWindowByName("AddOTC");
         if (window1 != null)
         { window1.close(); }

         var window3 = manager.getWindowByName("ViewOTC");
         if (window3 != null)
         { window3.close(); }
     }
     function getOTCGrid()
     {
         return window.top.$get("ctl00_Tile3_OTCCoverageMain1_gridOTCCoverage");
     }

     //Enable/Disable textarea associated with checkboxes based on checkbox's on/off status.
    //Enable textarea when checkbox is checked and disable if unchecked.
     function EnableDisableText(sender)
     {
         var str = "";
         if (sender.name == "ctl00$main$formViewOTC$Product_Other")
             str = "ctl00_main_formViewOTC_Product_Other_Text";
         else if (sender.name == "ctl00$main$formViewOTC$Is_CoughCold")
             str = "ctl00_main_formViewOTC_Desc_CoughCold_Kits";
         else if (sender.name == "ctl00$main$formViewOTC$Is_Health_Fairs")
             str = "ctl00_main_formViewOTC_Desc_Health_Fairs";
         else if (sender.name == "ctl00$main$formViewOTC$Is_Education_Brochures")
             str = "ctl00_main_formViewOTC_Desc_Education_Brochures";
         else if (sender.name == "ctl00$main$formViewOTC$Is_Other")
             str = "ctl00_main_formViewOTC_Desc_Other";

         if (str != "")
         {
             if (sender.checked)
                 $get(str).disabled = false;
             else
             {
                 $get(str).value = "";
                 $get(str).disabled = true;
             }
         }
     }
 </script>  
 <style type="text/css"> 
 td
 {
 	vertical-align: top;
 	font-weight : bold;
    width: auto !important; 
    text-align: left !important; 
 }
 
 td .lefttd
 {
   border-right: #ccc 1px solid !important;  
   border-bottom: #ccc 1px solid !important;
   width : 30% !important;
   text-align: left !important; 
 }
 
 td .righttd
 {
    border-bottom: #ccc 1px solid !important;
    width : 70% !important;
 }
 
 td .tdwidth
 {
 	width:30% !important; 
 }
 
 td .viewtdwidth
 {
 	width:45% !important; 
 }
 
 .rdcmbMS 
 {
 	width: 200px !important;
 }
 
 /****Apply this only for plans dropdown****/
 #comboTB td .rdcmbPlan table td
 {
 	width: 180px !important 
 }
 /****-------------------****/
   
 #comboTB td .RadComboBox table td input
 {
 	width: 180px !important;
 	margin-left: 0px !important; 
 	margin-right: 0px !important; 
 }
 
  #comboTB td .RadComboBox table td a
 {
 	width: 10px !important;
 }
 
 .lbl
 {
 	font-size:10px !important; 
 	vertical-align: bottom !important; 
 	white-space:nowrap;
 }
 
 .genOTCTable, #ccrForm .genOTCTable 
{
	width:100% !important;
	text-align: left;  
}
.genOTCTable th, .areaHeader
{
	padding:2px 5px;
	font-weight:bold;
}
.genOTCTable th.buttons
{
	text-align:right;
}
.genOTCTable td
{
	padding:2px 5px;
	width:50%;
	text-align:left;
	cursor:pointer;
	vertical-align: top;
}

.genOTCTable td td
{
	padding:0px;
}
.genOTCTable label
{
	width:80px;
	display:inline-table;
	font-weight:bold;
	vertical-align: top;
}
.genOTCTable textarea 
{
	vertical-align: text-top !important;
}

 .comboTable
{
	width:100% !important;
}
 </style>
</asp:Content>
<asp:Content ContentPlaceHolderID="title" Runat="Server" ID="title" >
<asp:Literal runat="server" id="titleText" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="tools" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="main" Runat="Server">
<div id="frmDiv" runat="server" visible="true" class="divmain">
<asp:HiddenField ID="hdnSelectedSegment" Value ="" runat="server" />
<asp:HiddenField ID="PlanNameHdn" runat="server" Value="" Visible="false" /> 

    <div id="combodiv" runat="server" visible="false">
        <table class="comboTable" id="comboTB">
            <tr>
                <td align="left" width="15%">Market Segment:</td>
                <td width="30%">
                     <telerik:RadComboBox ID="rdcmbMktSegment" runat="server" DropDownWidth="150px"  Height="100px" EnableEmbeddedSkins="false" SkinID="planInfoCombo" Skin="pathfinder" AppendDataBoundItems="true" DataTextField="Name" DataValueField="ID"  AutoPostBack="true"  OnSelectedIndexChanged="rdcmbMktSegment_SelectedIndexChanged" CssClass="rdcmbMS" CausesValidation="false">
                    </telerik:RadComboBox>
                </td>
                <td align="left" width="15%">
                <asp:Label ID="planlbl" runat="server" Visible="true" Text="Select Plan:"></asp:Label>  
                </td>
                <td>
                     <telerik:RadComboBox ID="rdcmbPlans" runat="server" DropDownWidth="220px" Height="100px" EnableEmbeddedSkins="false" SkinID="planInfoCombo" Skin="pathfinder" AppendDataBoundItems="true" DataSourceID="dsPlans" DataTextField="Plan_Name" DataValueField="Plan_ID" OnSelectedIndexChanged="rdcmbPlans_SelectedIndexChanged" OnDataBound="rdcmbPlans_OnDataBound" Visible="true" CssClass="rdcmbPlan">
                    </telerik:RadComboBox>&nbsp;
                </td>
            </tr>
            <tr>
            <td colspan="4" align="center">
                <asp:Label ID="noplanlbl" runat="server" Visible="false" Text="No plans available for selected market segment." style="color:Red;font-weight:bold;"></asp:Label>  
            </td>
            </tr>
            <tr>
            <td colspan="4"><hr /></td>
            </tr>
        </table>
    </div>
                    
<div id="OTCMain" align="center">
    <asp:FormView  runat="server" ID="formViewOTC" DefaultMode="Insert"   
           DataSourceID="dsOTCCoverage" CellPadding="0" CellSpacing="0" Width="100%" DataKeyNames="OTC_Coverage_Id" valign="top">
        <ItemTemplate>
                    <table cellpadding="0"  cellspacing="0" class="genOTCTable">
                            <tr align="left">
                                <td class="viewtdwidth lefttd">
                                <asp:Label runat="server" ID="id0" Text="Does the organization offer OTC coverage?"></asp:Label></td>
                                <td class="righttd"><%# ConvertDBValues(Convert.ToString(Eval("Is_OTC_Coverage")))%>&nbsp;</td>
                            </tr>
                             <tr align="left">
                                  <td class="lefttd">
                                  <asp:Label runat="server" ID="id1" Text="If organization does offer OTC coverage, which products?"></asp:Label></td>
                                  <td height="90px" class="righttd"><asp:Label id="Products_Coverage" runat="server" Text=""></asp:Label>&nbsp;
                                  </td>
                            </tr>
                             <tr align="left">
                                <td class="lefttd"><asp:Label runat="server" ID="id2" Text="Do OTC products require Rx for benefit coverage?"></asp:Label></td>
                                <td class="righttd"><%# ConvertDBValues(Convert.ToString(Eval("Rx_Require")))%>&nbsp;</td>
                            </tr>
                            <tr align="left">
                                <td class="lefttd"><asp:Label runat="server" ID="id21" Text="What is the co-pay level for OTCs?"></asp:Label></td>
                                <td class="righttd"><%# ConvertCopayLevelDBValues(Convert.ToString(Eval("OTC_Co_Pay_Level")))%>&nbsp;</td>
                            </tr>
                             <tr align="left">
                                <td class="lefttd"><asp:Label runat="server" ID="id3" Text="Is customer contracting on OTC products?"></asp:Label></td>
                                <td class="righttd"><%# ConvertDBValues(Convert.ToString(Eval("Is_Customer_Contracting")))%>&nbsp;</td>
                            </tr>
                             <tr align="left">
                                <td class="lefttd"><asp:Label runat="server" ID="id4" Text="Do opportunities exist to participate in OTC patient and/or provider educational programs?"></asp:Label></td>
                                <td class="righttd"><%# ConvertDBValues(Convert.ToString(Eval("Is_Opportunities_Exist")))%>&nbsp;</td>
                            </tr>
                             <tr align="left">
                                <td valign="top"  class="lefttd"><asp:Label runat="server" ID="id5" valign="top" Text="If opportunities exist, please describe"></asp:Label></td>
                                <td height="130px" class="righttd">
                                   <div>
                                        <%# ShowDesc(Convert.ToBoolean(Eval("Is_CoughCold")), Convert.ToString(Eval("Desc_CoughCold_Kits")), "Cough/Cold Kits")%>
                                        <%# ShowDesc(Convert.ToBoolean(Eval("Is_Health_Fairs")), Convert.ToString(Eval("Desc_Health_Fairs")), "Health Fairs")%>
                                        <%# ShowDesc(Convert.ToBoolean(Eval("Is_Education_Brochures")), Convert.ToString(Eval("Desc_Education_Brochures")), "Educational Brochures")%>
                                        <%# ShowDesc(Convert.ToBoolean(Eval("Is_Other")), Convert.ToString(Eval("Desc_Other")), "Other")%>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="lefttd"><asp:Label runat="server" ID="id6" Text="Which individual(s) are responsible for value add programs?"></asp:Label></td>
                                <td height="60px" class="righttd"><%# Eval("Person_Responsible_ForValueadd_Prgms")%>&nbsp;</td>
                            </tr>
                              <tr>
                                <td class="lefttd"><asp:Label runat="server" ID="id7" Text="Who is the person responsible for providing educational programs?"></asp:Label></td>
                                <td height="60px"  class="righttd"><%# Eval("Person_Responsible_ForEducational_Prgms")%>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="lefttd"><asp:Label runat="server" ID="id8" Text="Would your organization be interested in creating a cough/cold educational program for employers/employees?"></asp:Label></td>
                                <td class="righttd"><%# ConvertDBValues(Convert.ToString(Eval("Is_CoughCold_EduProgram_ForEmps")))%>&nbsp;</td>
                            </tr>
                             <tr>
                                <td class="lefttd"><asp:Label runat="server" ID="id9" Text="Who is the person responsible for making these decisions?"></asp:Label></td>
                                <td height="60px" class="righttd"><%# Eval("Person_Responsible_ForDecision")%>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="lefttd"><asp:Label runat="server" ID="id10" Text="Who is responsible for antibiotic educations?"></asp:Label></td>
                                <td height="60px" class="righttd"><%# Eval("Person_Responsible_ForAntibiotic_Education")%>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="lefttd"><asp:Label runat="server" ID="id11" Text="What type of programs have you done?"></asp:Label></td>
                                <td height="50px" class="righttd"><%# Eval("Type_of_Programs")%>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="lefttd"><asp:Label runat="server" ID="id12" Text="Do you own clinics and/or pharmacies associated to your organization?"></asp:Label></td>
                                <td class="righttd"><%# ConvertDBValues(Convert.ToString(Eval("Is_ClinicPharmacy_Associated")))%>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="lefttd"><asp:Label runat="server" ID="id13" Text="If yes, which?"></asp:Label></td>
                                <td height="60px" class="righttd"><%# Eval("ClinicPharmacy")%>&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="lefttd"><asp:Label runat="server" ID="id14" Text="Who manages the relationship between your organization and the clinics or pharmacy?"></asp:Label></td>
                                <td height="60px" class="righttd"><%# Eval("Person_Managing_ClinicPharmacy")%>&nbsp;</td>
                            </tr>
                            <tr><td><br /></td></tr>
                    </table>
                    <div id="ViewEditDiv" runat="server" class="modalFormButtonsOTC">
                            <pinso:CustomButton ID="ViewEditbtn" runat="server" Text="Edit Coverage" Visible="true"  onclick="ViewEditbtn_Click" />
                            <pinso:CustomButton ID="Closebtn" width="50px" runat="server" Text="Close" OnClientClick="javascript:CloseWin();"  />&nbsp;&nbsp;&nbsp;
                    </div>
        </ItemTemplate> 
        
        <EditItemTemplate>
                   <table cellpadding="0"  cellspacing="0" class="genOTCTable">
                     <tr align="left">
                        <td align="left" valign="top" class="tdwidth lefttd">
                                <asp:Label runat="server" ID="id0" Text="Does the organization offer OTC coverage?"></asp:Label>
                         </td>
                         <td class="righttd">
                                <pinso:RadiobuttonValueList ID="Is_OTC_Coverage" runat="server" BorderStyle="None" RepeatDirection="Horizontal" SelectedVal='<%# Bind("Is_OTC_Coverage") %>'>
                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem> 
                                    <asp:ListItem Text="No" Value="2"></asp:ListItem> 
                                    <asp:ListItem Text="Not Available" Value="0"></asp:ListItem> 
                                </pinso:RadiobuttonValueList>
                                
                                &nbsp;
                           </td>
                            </tr>
                             <tr align="left">
                                  <td class="lefttd">
                                  <asp:Label runat="server" ID="id1" Text="If organization does offer OTC coverage, which products?"></asp:Label></td>
                                  <td height="90px" class="righttd">
                                    <table width="100%">
                                    <tr>
                                        <td><asp:CheckBox ID="Product_PPI" runat="server" Text ="PPI" Checked='<%# Bind("Product_PPI")%>'  /></td>                     
                                        <td><asp:CheckBox ID="Product_Antihistamines" runat="server" Text ="Antihistamines" Checked='<%# Bind("Product_Antihistamines")%>'  /></td>
                                    </tr>
                                    <tr>
                                        <td><asp:CheckBox ID="Product_Pediatric_OTC" runat="server" Text ="Pediatric OTC" Checked='<%# Bind("Product_Pediatric_OTC")%>'  /></td>                   
                                        <td><asp:CheckBox ID="Product_Smoking" runat="server" Text ="Smoking" Checked='<%# Bind("Product_Smoking")%>'  /></td>    
                                    </tr>                                        
                                    <tr>
                                        <td ><asp:CheckBox ID="Product_Obesity" runat="server" Text ="Obesity" Checked='<%# Bind("Product_Obesity")%>'  /></td>                     
                                        <td ><asp:CheckBox ID="Product_Other" runat="server" Text ="Other" Checked='<%# Bind("Product_Other")%>'  onClick="javascript:EnableDisableText(this);"/><asp:TextBox ID="Product_Other_Text" runat="server" Text='<%# Bind("Product_Other_Text")%>' Enabled='<%# Eval("Product_Other")%>'  MaxLength="100" Columns="35" Rows="3" TextMode="MultiLine"></asp:TextBox><label class="lbl">(max: 100 chars)</label>
                                        <br /><asp:RegularExpressionValidator  CssClass="validatorcss" ID="regText1" runat="server" ControlToValidate="Product_Other_Text" 
                                        ValidationExpression="^[\s\S]{0,100}$" ErrorMessage="Maximum 100 characters are allowed in the textarea." Text="Maximum 100 characters are allowed in the textarea."  SetFocusOnError="true" > 
                                        </asp:RegularExpressionValidator>
                                        </td>
                                    </tr>                                        
                                    </table>
                                  </td>
                            </tr>
                             <tr align="left">
                                <td class="lefttd"><asp:Label runat="server" ID="id2" Text="Do OTC products require Rx for benefit coverage?"></asp:Label></td>
                                <td class="righttd">
                                <pinso:RadiobuttonValueList ID="Rx_Require" runat="server" BorderStyle="None" RepeatDirection="Horizontal" SelectedVal='<%# Bind("Rx_Require") %>'>
                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem> 
                                    <asp:ListItem Text="No" Value="2"></asp:ListItem> 
                                    <asp:ListItem Text="Not Available" Value="0"></asp:ListItem> 
                                </pinso:RadiobuttonValueList>
                                </td>
                            </tr>
                            <tr align="left">
                                <td class="lefttd"><asp:Label runat="server" ID="id21" Text="What is the co-pay level for OTCs?"></asp:Label></td>
                                <td width="60%" class="righttd">
                                <pinso:RadiobuttonValueList ID="OTC_Co_Pay_Level" runat="server" BorderStyle="None" RepeatDirection="Horizontal" SelectedVal='<%# Bind("OTC_Co_Pay_Level") %>'>
                                    <asp:ListItem Text="I" Value="1"></asp:ListItem> 
                                    <asp:ListItem Text="II" Value="2"></asp:ListItem> 
                                    <asp:ListItem Text="III" Value="3"></asp:ListItem> 
                                </pinso:RadiobuttonValueList>
                                </td>
                            </tr>
                             <tr align="left">
                                <td class="lefttd"><asp:Label runat="server" ID="id3" Text="Is customer contracting on OTC products?"></asp:Label></td>
                                <td class="righttd">
                                <pinso:RadiobuttonValueList ID="Is_Customer_Contracting" runat="server" BorderStyle="None" RepeatDirection="Horizontal" SelectedVal='<%# Bind("Is_Customer_Contracting") %>'>
                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem> 
                                    <asp:ListItem Text="No" Value="2"></asp:ListItem> 
                                    <asp:ListItem Text="Not Available" Value="0"></asp:ListItem> 
                                </pinso:RadiobuttonValueList>
                                </td>
                            </tr>
                             <tr align="left">
                                <td class="lefttd"><asp:Label runat="server" ID="id4" Text="Do opportunities exist to participate in OTC patient and/or provider educational programs?"></asp:Label></td>
                                <td class="righttd">
                                <pinso:RadiobuttonValueList ID="Is_Opportunities_Exist" runat="server" BorderStyle="None" RepeatDirection="Horizontal" SelectedVal='<%# Bind("Is_Opportunities_Exist") %>'>
                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem> 
                                    <asp:ListItem Text="No" Value="2"></asp:ListItem> 
                                    <asp:ListItem Text="Not Available" Value="0"></asp:ListItem> 
                                </pinso:RadiobuttonValueList>
                                </td>
                            </tr>
                             <tr align="left">
                                <td valign="top"  class="lefttd"><asp:Label runat="server" ID="id5" valign="top" Text="If opportunities exist, please describe"></asp:Label></td>
                                <td height="130px" class="righttd">
                                   <table>
                                    <tr>
                                    <td width="20%"><asp:CheckBox ID="Is_CoughCold" runat="server" Text ="Cough/Cold Kits:" Checked='<%# Bind("Is_CoughCold")%>' onClick="javascript:EnableDisableText(this);"/></td><td><asp:TextBox ID="Desc_CoughCold_Kits" runat="server" Text='<%# Bind("Desc_CoughCold_Kits")%>' MaxLength="100" Columns="40" Rows="3" TextMode="MultiLine" Enabled='<%# Eval("Is_CoughCold")%>'></asp:TextBox><label class="lbl">(max: 100 chars)</label>
                                    <br /><asp:RegularExpressionValidator  CssClass="validatorcss" ID="RegularExpressionValidator1" runat="server" ControlToValidate="Desc_CoughCold_Kits" 
                                        ValidationExpression="^[\s\S]{0,100}$" ErrorMessage="Maximum 100 characters are allowed in the textarea." Text="Maximum 100 characters are allowed in the textarea."  SetFocusOnError="true" > 
                                        </asp:RegularExpressionValidator>
                                    </td>
                                    </tr>
                                    <tr><td><br /></td></tr>
                                   <tr>
                                    <td><asp:CheckBox ID="Is_Health_Fairs" runat="server" Text ="Health Fairs:" Checked='<%# Bind("Is_Health_Fairs")%>' onClick="javascript:EnableDisableText(this);"/></td><td><asp:TextBox ID="Desc_Health_Fairs" runat="server" Text='<%# Bind("Desc_Health_Fairs")%>' MaxLength="100" Columns="40" Rows="3" TextMode="MultiLine" Enabled='<%# Eval("Is_Health_Fairs")%>'></asp:TextBox><label class="lbl">(max: 100 chars)</label>
                                    <br /><asp:RegularExpressionValidator  CssClass="validatorcss" ID="RegularExpressionValidator2" runat="server" ControlToValidate="Desc_Health_Fairs" 
                                        ValidationExpression="^[\s\S]{0,100}$" ErrorMessage="Maximum 100 characters are allowed in the textarea." Text="Maximum 100 characters are allowed in the textarea."  SetFocusOnError="true" > 
                                        </asp:RegularExpressionValidator>
                                    </td>
                                    </tr>
                                    <tr><td><br /></td></tr>
                                   <tr>
                                    <td><asp:CheckBox ID="Is_Education_Brochures" runat="server" Text ="Educational Brochures:" Checked='<%# Bind("Is_Education_Brochures")%>' onClick="javascript:EnableDisableText(this);" /></td><td><asp:TextBox ID="Desc_Education_Brochures" runat="server" Text='<%# Bind("Desc_Education_Brochures")%>' MaxLength="100" Columns="40" Rows="3" TextMode="MultiLine" Enabled='<%# Eval("Is_Education_Brochures")%>'></asp:TextBox><label class="lbl">(max: 100 chars)</label>
                                    <br /><asp:RegularExpressionValidator  CssClass="validatorcss" ID="RegularExpressionValidator3" runat="server" ControlToValidate="Desc_Education_Brochures" 
                                        ValidationExpression="^[\s\S]{0,100}$" ErrorMessage="Maximum 100 characters are allowed in the textarea." Text="Maximum 100 characters are allowed in the textarea."  SetFocusOnError="true" > 
                                        </asp:RegularExpressionValidator>
                                    </td>
                                    </tr>
                                    <tr><td><br /></td></tr>
                                   <tr>
                                    <td><asp:CheckBox ID="Is_Other" runat="server" Text ="Other:" Checked='<%# Bind("Is_Other")%>'  onClick="javascript:EnableDisableText(this);"/></td><td><asp:TextBox ID="Desc_Other" runat="server" Text='<%# Bind("Desc_Other")%>' MaxLength="100" Columns="40" Rows="3" TextMode="MultiLine" Enabled='<%# Eval("Is_Other")%>'></asp:TextBox><label class="lbl">(max: 100 chars)</label>
                                    <br /><asp:RegularExpressionValidator  CssClass="validatorcss" ID="RegularExpressionValidator4" runat="server" ControlToValidate="Desc_Other" 
                                        ValidationExpression="^[\s\S]{0,100}$" ErrorMessage="Maximum 100 characters are allowed in the textarea." Text="Maximum 100 characters are allowed in the textarea."  SetFocusOnError="true" > 
                                        </asp:RegularExpressionValidator>
                                    </td>
                                    </tr>
                                   </table> 
                                </td>
                            </tr>
                            <tr>
                                <td class="lefttd"><asp:Label runat="server" ID="id6" Text="Which individual(s) are responsible for value add programs?"></asp:Label></td>
                                <td height="90px"  class="righttd">
                                <asp:TextBox ID="Person_Responsible_ForValueadd_Prgms" runat="server" Text='<%# Bind("Person_Responsible_ForValueadd_Prgms")%>' MaxLength="100" Columns="60" Rows="5" TextMode="MultiLine"></asp:TextBox><label class="lbl">(max: 100 chars)</label>
                                <br /><asp:RegularExpressionValidator  CssClass="validatorcss" ID="RegularExpressionValidator5" runat="server" ControlToValidate="Person_Responsible_ForValueadd_Prgms" 
                                        ValidationExpression="^[\s\S]{0,100}$" ErrorMessage="Maximum 100 characters are allowed in the textarea." Text="Maximum 100 characters are allowed in the textarea."  SetFocusOnError="true" > 
                                        </asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="lefttd"><asp:Label runat="server" ID="id7" Text="Who is the person responsible for providing educational programs?"></asp:Label></td>
                                <td height="80px" class="righttd">
                                <asp:TextBox ID="Person_Responsible_ForEducational_Prgms" runat="server" Text='<%# Bind("Person_Responsible_ForEducational_Prgms")%>' MaxLength="100" Columns="60" Rows="5" TextMode="MultiLine"></asp:TextBox><label  class="lbl">(max: 100 chars)</label>
                                <br /><asp:RegularExpressionValidator  CssClass="validatorcss" ID="RegularExpressionValidator6" runat="server" ControlToValidate="Person_Responsible_ForEducational_Prgms" 
                                        ValidationExpression="^[\s\S]{0,100}$" ErrorMessage="Maximum 100 characters are allowed in the textarea." Text="Maximum 100 characters are allowed in the textarea."  SetFocusOnError="true" > 
                                        </asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="lefttd"><asp:Label runat="server" ID="id8" Text="Would your organization be interested in creating a cough/cold educational program for employers/employees?"></asp:Label></td>
                                <td class="righttd">
                                <pinso:RadiobuttonValueList ID="Is_CoughCold_EduProgram_ForEmps" runat="server" BorderStyle="None" RepeatDirection="Horizontal" SelectedVal='<%# Bind("Is_CoughCold_EduProgram_ForEmps") %>'>
                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem> 
                                    <asp:ListItem Text="No" Value="2"></asp:ListItem> 
                                    <asp:ListItem Text="Not Available" Value="0"></asp:ListItem> 
                                </pinso:RadiobuttonValueList>
                                </td>
                            </tr>
                             <tr>
                                <td class="lefttd"><asp:Label runat="server" ID="id9" Text="Who is the person responsible for making these decisions?"></asp:Label></td>
                                <td height="80px" class="righttd">
                                <asp:TextBox ID="Person_Responsible_ForDecision" runat="server" Text='<%# Bind("Person_Responsible_ForDecision")%>' MaxLength="100" Columns="60" Rows="5" TextMode="MultiLine"></asp:TextBox><label class="lbl">(max: 100 chars)</label>
                                <br /><asp:RegularExpressionValidator  CssClass="validatorcss" ID="RegularExpressionValidator7" runat="server" ControlToValidate="Person_Responsible_ForDecision" 
                                        ValidationExpression="^[\s\S]{0,100}$" ErrorMessage="Maximum 100 characters are allowed in the textarea." Text="Maximum 100 characters are allowed in the textarea."  SetFocusOnError="true" > 
                                        </asp:RegularExpressionValidator>
                                &nbsp;</td>
                            </tr>
                            <tr>
                                <td class="lefttd"><asp:Label runat="server" ID="id10" Text="Who is responsible for antibiotic educations?"></asp:Label></td>
                                <td height="80px" class="righttd">
                                <asp:TextBox ID="Person_Responsible_ForAntibiotic_Education" runat="server" Text='<%# Bind("Person_Responsible_ForAntibiotic_Education")%>' MaxLength="100" Columns="60" Rows="5" TextMode="MultiLine"></asp:TextBox><label class="lbl">(max: 100 chars)</label>
                                <br /><asp:RegularExpressionValidator  CssClass="validatorcss" ID="RegularExpressionValidator8" runat="server" ControlToValidate="Person_Responsible_ForAntibiotic_Education" 
                                        ValidationExpression="^[\s\S]{0,100}$" ErrorMessage="Maximum 100 characters are allowed in the textarea." Text="Maximum 100 characters are allowed in the textarea."  SetFocusOnError="true" > 
                                        </asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="lefttd"><asp:Label runat="server" ID="id11" Text="What type of programs have you done?"></asp:Label></td>
                                <td height="50px" class="righttd">
                                <asp:TextBox ID="Type_of_Programs" runat="server" Text='<%# Bind("Type_of_Programs")%>' MaxLength="100" Columns="60" Rows="5" TextMode="MultiLine"></asp:TextBox><label class="lbl">(max: 100 chars)</label>
                                <br /><asp:RegularExpressionValidator  CssClass="validatorcss" ID="RegularExpressionValidator9" runat="server" ControlToValidate="Type_of_Programs" 
                                        ValidationExpression="^[\s\S]{0,100}$" ErrorMessage="Maximum 100 characters are allowed in the textarea." Text="Maximum 100 characters are allowed in the textarea."  SetFocusOnError="true" > 
                                        </asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="lefttd"><asp:Label runat="server" ID="id12" Text="Do you own clinics and/or pharmacies associated to your organization?"></asp:Label></td>
                                <td class="righttd">
                                <pinso:RadiobuttonValueList ID="Is_ClinicPharmacy_Associated" runat="server" BorderStyle="None" RepeatDirection="Horizontal" SelectedVal='<%# Bind("Is_ClinicPharmacy_Associated") %>'>
                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem> 
                                    <asp:ListItem Text="No" Value="2"></asp:ListItem> 
                                    <asp:ListItem Text="Not Available" Value="0"></asp:ListItem> 
                                </pinso:RadiobuttonValueList>
                                </td>
                            </tr>
                            <tr>
                                <td class="lefttd"><asp:Label runat="server" ID="id13" Text="If yes, which?"></asp:Label></td>
                                <td height="80px" class="righttd">
                                <asp:TextBox ID="ClinicPharmacy" runat="server" Text='<%# Bind("ClinicPharmacy")%>' MaxLength="100" Columns="60" Rows="5" TextMode="MultiLine"></asp:TextBox><label class="lbl">(max: 100 chars)</label>
                                <br /><asp:RegularExpressionValidator  CssClass="validatorcss" ID="RegularExpressionValidator10" runat="server" ControlToValidate="ClinicPharmacy" 
                                        ValidationExpression="^[\s\S]{0,100}$" ErrorMessage="Maximum 100 characters are allowed in the textarea." Text="Maximum 100 characters are allowed in the textarea."  SetFocusOnError="true" > 
                                        </asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="lefttd"><asp:Label runat="server" ID="id14" Text="Who manages the relationship between your organization and the clinics or pharmacy?"></asp:Label></td>
                                <td height="90px" class="righttd">
                                <asp:TextBox ID="Person_Managing_ClinicPharmacy" runat="server" Text='<%# Bind("Person_Managing_ClinicPharmacy")%>' MaxLength="100" Columns="60" Rows="5" TextMode="MultiLine"></asp:TextBox><label class="lbl">(max: 100 chars)</label>
                                <br /><asp:RegularExpressionValidator  CssClass="validatorcss" ID="regText" runat="server" ControlToValidate="Person_Managing_ClinicPharmacy" 
                                ValidationExpression="^[\s\S]{0,100}$" ErrorMessage="Maximum 100 characters are allowed in the textarea." Text="Maximum 100 characters are allowed in the textarea."  SetFocusOnError="true" > 
                                </asp:RegularExpressionValidator>
                            </tr>
                            <tr><td><br /></td></tr>
                    </table>
                    
                   
                    
            <div class="modalFormButtonsOTC">
                            <pinso:CustomButton ID="Editbtn" runat="server" Text="Edit" width="50px" Visible="true" CommandName="Update"  />
                            <pinso:CustomButton ID="Resetbtn" width="50px" runat="server" Text="Reset" OnClientClick="javascript:ClearForm(); return false;"  />&nbsp;&nbsp;&nbsp;
            </div>
        </EditItemTemplate>
        
        <InsertItemTemplate>
                    <table cellpadding="0"  cellspacing="0" class="genOTCTable">
                     <tr align="left">
                        <td align="left" valign="top" class="tdwidth lefttd">
                                <asp:Label runat="server" ID="id0" Text="Does the organization offer OTC coverage?"></asp:Label>
                         </td>
                         <td class="righttd">
                                <pinso:RadiobuttonValueList ID="Is_OTC_Coverage" runat="server" BorderStyle="None" RepeatDirection="Horizontal" SelectedVal='<%# Bind("Is_OTC_Coverage") %>'>
                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem> 
                                    <asp:ListItem Text="No" Value="2"></asp:ListItem> 
                                    <asp:ListItem Text="Not Available" Value="0"></asp:ListItem> 
                                </pinso:RadiobuttonValueList>
                                &nbsp;
                           </td>
                            </tr>
                             <tr align="left">
                                  <td class="lefttd">
                                  <asp:Label runat="server" ID="id1" Text="If organization does offer OTC coverage, which products?"></asp:Label></td>
                                  <td height="90px" class="righttd">
                                    <table width="100%">
                                    <tr>
                                        <td width="30%"><asp:CheckBox ID="Product_PPI" runat="server" Text ="PPI" Checked='<%# Bind("Product_PPI")%>'  /></td>                     
                                        <td width="70%"><asp:CheckBox ID="Product_Antihistamines" runat="server" Text ="Antihistamines" Checked='<%# Bind("Product_Antihistamines")%>'  /></td>
                                    </tr>
                                    <tr>
                                        <td width="30%"><asp:CheckBox ID="Product_Pediatric_OTC" runat="server" Text ="Pediatric OTC" Checked='<%# Bind("Product_Pediatric_OTC")%>'  /></td>                   
                                        <td width="70%"><asp:CheckBox ID="Product_Smoking" runat="server" Text ="Smoking" Checked='<%# Bind("Product_Smoking")%>'  /></td>    
                                    </tr>                                        
                                    <tr>
                                        <td width="30%"><asp:CheckBox ID="Product_Obesity" runat="server" Text ="Obesity" Checked='<%# Bind("Product_Obesity")%>'  /></td>                     
                                        <td width="70%"><asp:CheckBox ID="Product_Other" runat="server" Text ="Other" Checked='<%# Bind("Product_Other")%>' onClick="javascript:EnableDisableText(this);"/><asp:TextBox ID="Product_Other_Text" runat="server" Text='<%# Bind("Product_Other_Text")%>' MaxLength="100" Columns="35" Rows="3" TextMode="MultiLine" Enabled="false"  ></asp:TextBox><label class="lbl">(max: 100 chars)</label>
                                        <br /><asp:RegularExpressionValidator  CssClass="validatorcss" ID="regText1" runat="server" ControlToValidate="Product_Other_Text" 
                                        ValidationExpression="^[\s\S]{0,100}$" ErrorMessage="Maximum 100 characters are allowed in the textarea." Text="Maximum 100 characters are allowed in the textarea."  SetFocusOnError="true" > 
                                        </asp:RegularExpressionValidator>
                                        </td>
                                    </tr>                                        
                                    </table>
                                  </td>
                            </tr>
                             <tr align="left">
                                <td class="lefttd"><asp:Label runat="server" ID="id2" Text="Do OTC products require Rx for benefit coverage?"></asp:Label></td>
                                <td class="righttd">
                                <pinso:RadiobuttonValueList ID="Rx_Require" runat="server" BorderStyle="None" RepeatDirection="Horizontal" SelectedVal='<%# Bind("Rx_Require") %>'>
                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem> 
                                    <asp:ListItem Text="No" Value="2"></asp:ListItem> 
                                    <asp:ListItem Text="Not Available" Value="0"></asp:ListItem> 
                                </pinso:RadiobuttonValueList>
                                </td>
                            </tr>
                            <tr align="left">
                                <td class="lefttd"><asp:Label runat="server" ID="id21" Text="What is the co-pay level for OTCs?"></asp:Label></td>
                                <td width="60%" class="righttd">
                                <pinso:RadiobuttonValueList ID="OTC_Co_Pay_Level" runat="server" BorderStyle="None" RepeatDirection="Horizontal" SelectedVal='<%# Bind("OTC_Co_Pay_Level") %>'>
                                    <asp:ListItem Text="I" Value="1"></asp:ListItem> 
                                    <asp:ListItem Text="II" Value="2"></asp:ListItem> 
                                    <asp:ListItem Text="III" Value="3"></asp:ListItem> 
                                </pinso:RadiobuttonValueList> 
                                </td>
                            </tr>
                             <tr align="left">
                                <td class="lefttd"><asp:Label runat="server" ID="id3" Text="Is customer contracting on OTC products?"></asp:Label></td>
                                <td class="righttd">
                                 <pinso:RadiobuttonValueList ID="Is_Customer_Contracting" runat="server" BorderStyle="None" RepeatDirection="Horizontal" SelectedVal='<%# Bind("Is_Customer_Contracting") %>'>
                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem> 
                                    <asp:ListItem Text="No" Value="2"></asp:ListItem> 
                                    <asp:ListItem Text="Not Available" Value="0"></asp:ListItem> 
                                </pinso:RadiobuttonValueList> 
                                </td>
                            </tr>
                             <tr align="left">
                                <td class="lefttd"><asp:Label runat="server" ID="id4" Text="Do opportunities exist to participate in OTC patient and/or provider educational programs?"></asp:Label></td>
                                <td class="righttd">
                                <pinso:RadiobuttonValueList ID="Is_Opportunities_Exist" runat="server" BorderStyle="None" RepeatDirection="Horizontal" SelectedVal='<%# Bind("Is_Opportunities_Exist") %>'>
                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem> 
                                    <asp:ListItem Text="No" Value="2"></asp:ListItem> 
                                    <asp:ListItem Text="Not Available" Value="0"></asp:ListItem> 
                                </pinso:RadiobuttonValueList> 
                                </td>
                            </tr>
                             <tr align="left">
                                <td valign="top" class="lefttd"><asp:Label runat="server" ID="id5" valign="top" Text="If opportunities exist, please describe"></asp:Label></td>
                                <td height="130px" class="righttd">
                                   <table>
                                    <tr>
                                    <td width="20%"><asp:CheckBox ID="Is_CoughCold" runat="server" Text ="Cough/Cold Kits:" Checked='<%# Bind("Is_CoughCold")%>' onClick="javascript:EnableDisableText(this);"/></td><td><asp:TextBox ID="Desc_CoughCold_Kits" runat="server" Text='<%# Bind("Desc_CoughCold_Kits")%>' MaxLength="100" Columns="40" Rows="3" TextMode="MultiLine" Enabled="false"  ></asp:TextBox><label class="lbl">(max: 100 chars)</label>
                                    <br /><asp:RegularExpressionValidator  CssClass="validatorcss" ID="RegularExpressionValidator11" runat="server" ControlToValidate="Desc_CoughCold_Kits" 
                                        ValidationExpression="^[\s\S]{0,100}$" ErrorMessage="Maximum 100 characters are allowed in the textarea." Text="Maximum 100 characters are allowed in the textarea."  SetFocusOnError="true"> 
                                        </asp:RegularExpressionValidator>
                                    </td>
                                    </tr>
                                    <tr><td><br /></td></tr>
                                   <tr>
                                    <td><asp:CheckBox ID="Is_Health_Fairs" runat="server" Text ="Health Fairs:" Checked='<%# Bind("Is_Health_Fairs")%>' onClick="javascript:EnableDisableText(this);"/></td><td><asp:TextBox ID="Desc_Health_Fairs" runat="server" Text='<%# Bind("Desc_Health_Fairs")%>' MaxLength="100" Columns="40" Rows="3" TextMode="MultiLine" Enabled="false"></asp:TextBox><label class="lbl">(max: 100 chars)</label>
                                    <br /><asp:RegularExpressionValidator  CssClass="validatorcss" ID="RegularExpressionValidator12" runat="server" ControlToValidate="Desc_Health_Fairs" 
                                        ValidationExpression="^[\s\S]{0,100}$" ErrorMessage="Maximum 100 characters are allowed in the textarea." Text="Maximum 100 characters are allowed in the textarea."  SetFocusOnError="true" > 
                                        </asp:RegularExpressionValidator>
                                    </td>
                                    </tr>
                                    <tr><td><br /></td></tr>
                                   <tr>
                                    <td><asp:CheckBox ID="Is_Education_Brochures" runat="server" Text ="Educational Brochures:" Checked='<%# Bind("Is_Education_Brochures")%>'  onClick="javascript:EnableDisableText(this);"/></td><td><asp:TextBox ID="Desc_Education_Brochures" runat="server" Text='<%# Bind("Desc_Education_Brochures")%>' MaxLength="100" Columns="40" Rows="3" TextMode="MultiLine" Enabled="false"></asp:TextBox><label class="lbl">(max: 100 chars)</label>
                                    <br /><asp:RegularExpressionValidator  CssClass="validatorcss" ID="RegularExpressionValidator13" runat="server" ControlToValidate="Desc_Education_Brochures" 
                                        ValidationExpression="^[\s\S]{0,100}$" ErrorMessage="Maximum 100 characters are allowed in the textarea." Text="Maximum 100 characters are allowed in the textarea."  SetFocusOnError="true" > 
                                        </asp:RegularExpressionValidator>
                                    </td>
                                    </tr>
                                    <tr><td><br /></td></tr>
                                   <tr>
                                    <td><asp:CheckBox ID="Is_Other" runat="server" Text ="Other:" Checked='<%# Bind("Is_Other")%>'  onClick="javascript:EnableDisableText(this);"/></td><td><asp:TextBox ID="Desc_Other" runat="server" Text='<%# Bind("Desc_Other")%>' MaxLength="100" Columns="40" Rows="3" TextMode="MultiLine" Enabled="false"></asp:TextBox><label class="lbl">(max: 100 chars)</label>
                                    <br /><asp:RegularExpressionValidator  CssClass="validatorcss" ID="RegularExpressionValidator14" runat="server" ControlToValidate="Desc_Other" 
                                        ValidationExpression="^[\s\S]{0,100}$" ErrorMessage="Maximum 100 characters are allowed in the textarea." Text="Maximum 100 characters are allowed in the textarea."  SetFocusOnError="true" > 
                                        </asp:RegularExpressionValidator>
                                    </td>
                                    </tr>
                                   </table> 
                                </td>
                            </tr>
                            <tr>
                                <td class="lefttd"><asp:Label runat="server" ID="id6" Text="Which individual(s) are responsible for value add programs?"></asp:Label></td>
                                <td height="90px"  class="righttd">
                                <asp:TextBox ID="Person_Responsible_ForValueadd_Prgms" runat="server" Text='<%# Bind("Person_Responsible_ForValueadd_Prgms")%>' MaxLength="100" Columns="60" Rows="5" TextMode="MultiLine"></asp:TextBox><label class="lbl">(max: 100 chars)</label>
                                <br /><asp:RegularExpressionValidator  CssClass="validatorcss" ID="RegularExpressionValidator15" runat="server" ControlToValidate="Person_Responsible_ForValueadd_Prgms" 
                                        ValidationExpression="^[\s\S]{0,100}$" ErrorMessage="Maximum 100 characters are allowed in the textarea." Text="Maximum 100 characters are allowed in the textarea."  SetFocusOnError="true" > 
                                        </asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="lefttd"><asp:Label runat="server" ID="id7" Text="Who is the person responsible for providing educational programs?"></asp:Label></td>
                                <td height="80px" class="righttd">
                                <asp:TextBox ID="Person_Responsible_ForEducational_Prgms" runat="server" Text='<%# Bind("Person_Responsible_ForEducational_Prgms")%>' MaxLength="100" Columns="60" Rows="5" TextMode="MultiLine"></asp:TextBox><label class="lbl">(max: 100 chars)</label>
                                <br /><asp:RegularExpressionValidator  CssClass="validatorcss" ID="RegularExpressionValidator16" runat="server" ControlToValidate="Person_Responsible_ForEducational_Prgms" 
                                        ValidationExpression="^[\s\S]{0,100}$" ErrorMessage="Maximum 100 characters are allowed in the textarea." Text="Maximum 100 characters are allowed in the textarea."  SetFocusOnError="true" > 
                                        </asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="lefttd"><asp:Label runat="server" ID="id8" Text="Would your organization be interested in creating a cough/cold educational program for employers/employees?"></asp:Label></td>
                                <td class="righttd">
                                <pinso:RadiobuttonValueList ID="Is_CoughCold_EduProgram_ForEmps" runat="server" BorderStyle="None" RepeatDirection="Horizontal" SelectedVal='<%# Bind("Is_CoughCold_EduProgram_ForEmps") %>'>
                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem> 
                                    <asp:ListItem Text="No" Value="2"></asp:ListItem> 
                                    <asp:ListItem Text="Not Available" Value="0"></asp:ListItem> 
                                </pinso:RadiobuttonValueList>
                                </td>
                            </tr>
                             <tr>
                                <td class="lefttd"><asp:Label runat="server" ID="id9" Text="Who is the person responsible for making these decisions?"></asp:Label></td>
                                <td height="80px" class="righttd">
                                <asp:TextBox ID="Person_Responsible_ForDecision" runat="server" Text='<%# Bind("Person_Responsible_ForDecision")%>' MaxLength="100" Columns="60" Rows="5" TextMode="MultiLine"></asp:TextBox><label class="lbl">(max: 100 chars)</label>
                                <br /><asp:RegularExpressionValidator  CssClass="validatorcss" ID="RegularExpressionValidator17" runat="server" ControlToValidate="Person_Responsible_ForDecision" 
                                        ValidationExpression="^[\s\S]{0,100}$" ErrorMessage="Maximum 100 characters are allowed in the textarea." Text="Maximum 100 characters are allowed in the textarea."  SetFocusOnError="true" > 
                                        </asp:RegularExpressionValidator>
                                &nbsp;</td>
                            </tr>
                            <tr>
                                <td class="lefttd"><asp:Label runat="server" ID="id10" Text="Who is responsible for antibiotic educations?"></asp:Label></td>
                                <td height="80px" width="70%">
                                <asp:TextBox ID="Person_Responsible_ForAntibiotic_Education" runat="server" Text='<%# Bind("Person_Responsible_ForAntibiotic_Education")%>' MaxLength="100" Columns="60" Rows="5" TextMode="MultiLine"></asp:TextBox><label class="lbl">(max: 100 chars)</label>
                                <br /><asp:RegularExpressionValidator  CssClass="validatorcss" ID="RegularExpressionValidator18" runat="server" ControlToValidate="Person_Responsible_ForAntibiotic_Education" 
                                        ValidationExpression="^[\s\S]{0,100}$" ErrorMessage="Maximum 100 characters are allowed in the textarea." Text="Maximum 100 characters are allowed in the textarea."  SetFocusOnError="true" > 
                                        </asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="lefttd"><asp:Label runat="server" ID="id11" Text="What type of programs have you done?"></asp:Label></td>
                                <td height="50px" class="righttd">
                                <asp:TextBox ID="Type_of_Programs" runat="server" Text='<%# Bind("Type_of_Programs")%>' MaxLength="100" Columns="60" Rows="5" TextMode="MultiLine"></asp:TextBox><label class="lbl">(max: 100 chars)</label>
                                <br /><asp:RegularExpressionValidator  CssClass="validatorcss" ID="RegularExpressionValidator19" runat="server" ControlToValidate="Type_of_Programs" 
                                        ValidationExpression="^[\s\S]{0,100}$" ErrorMessage="Maximum 100 characters are allowed in the textarea." Text="Maximum 100 characters are allowed in the textarea."  SetFocusOnError="true" > 
                                        </asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="lefttd"><asp:Label runat="server" ID="id12" Text="Do you own clinics and/or pharmacies associated to your organization?"></asp:Label></td>
                                <td class="righttd">
                                <pinso:RadiobuttonValueList ID="Is_ClinicPharmacy_Associated" runat="server" BorderStyle="None" RepeatDirection="Horizontal" SelectedVal='<%# Bind("Is_ClinicPharmacy_Associated") %>'>
                                    <asp:ListItem Text="Yes" Value="1"></asp:ListItem> 
                                    <asp:ListItem Text="No" Value="2"></asp:ListItem> 
                                    <asp:ListItem Text="Not Available" Value="0"></asp:ListItem> 
                                </pinso:RadiobuttonValueList> 
                                </td>
                            </tr>
                            <tr>
                                <td class="lefttd"><asp:Label runat="server" ID="id13" Text="If yes, which?"></asp:Label></td>
                                <td height="80px" class="righttd">
                                <asp:TextBox ID="ClinicPharmacy" runat="server" Text='<%# Bind("ClinicPharmacy")%>' MaxLength="100" Columns="60" Rows="5" TextMode="MultiLine"></asp:TextBox><label class="lbl">(max: 100 chars)</label>
                                <br /><asp:RegularExpressionValidator  CssClass="validatorcss" ID="RegularExpressionValidator20" runat="server" ControlToValidate="ClinicPharmacy" 
                                        ValidationExpression="^[\s\S]{0,100}$" ErrorMessage="Maximum 100 characters are allowed in the textarea." Text="Maximum 100 characters are allowed in the textarea."  SetFocusOnError="true" > 
                                        </asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="lefttd"><asp:Label runat="server" ID="id14" Text="Who manages the relationship between your organization and the clinics or pharmacy?"></asp:Label></td>
                                <td height="90px" class="righttd">
                                <asp:TextBox ID="Person_Managing_ClinicPharmacy" runat="server" Text='<%# Bind("Person_Managing_ClinicPharmacy")%>' MaxLength="100"  Columns="60" Rows="5" TextMode="MultiLine"></asp:TextBox><label class="lbl">(max: 100 chars)</label>
                                <br /><asp:RegularExpressionValidator  CssClass="validatorcss" ID="RegularExpressionValidator21" runat="server" ControlToValidate="Person_Managing_ClinicPharmacy" 
                                        ValidationExpression="^[\s\S]{0,100}$" ErrorMessage="Maximum 100 characters are allowed in the textarea." Text="Maximum 100 characters are allowed in the textarea."  SetFocusOnError="true"> 
                                        </asp:RegularExpressionValidator>
                            </tr>
                            <tr><td><br /></td></tr>
                    </table>
            <div class="modalFormButtonsOTC">
                    <pinso:CustomButton ID="Addbtn" runat="server" Text="Add" Visible="true" CommandName="Insert"  />
                    <pinso:CustomButton ID="Resetbtn" runat="server" Text="Reset" OnClientClick="javascript:ClearForm(); return false;"  />&nbsp;&nbsp;&nbsp;
           </div>

        </InsertItemTemplate>
        
    </asp:FormView>

     </div>
     
    <asp:EntityDataSource ID="dsOTCCoverage" runat="server" EntitySetName="tblOTCCoverageSet" DefaultContainerName="PathfinderReckittEntities" ConnectionString="name=PathfinderReckittEntities" 
        AutoGenerateWhereClause="true" EnableInsert="true" EnableUpdate="true" OnInserting="AddData" OnUpdating="EditData" OnInserted="ConfirmMsg"  OnUpdated="ConfirmMsg">
        <WhereParameters>       
            <asp:QueryStringParameter QueryStringField="PlanID" Name="Plan_ID" Type="Int32" ConvertEmptyStringToNull="true"/>
            <asp:QueryStringParameter QueryStringField="OTCID" Name="OTC_Coverage_Id" Type="Int32" ConvertEmptyStringToNull="true" />
        </WhereParameters>
    </asp:EntityDataSource>    
    
               
  <asp:EntityDataSource ID="dsPlans" runat="server" EntitySetName="PlanSearchOTCSet" DefaultContainerName="PathfinderReckittEntities" ConnectionString="name=PathfinderReckittEntities" 
    AutoGenerateWhereClause="true">
     <WhereParameters>
         <asp:ControlParameter  ControlID="rdcmbMktSegment" PropertyName="SelectedValue"  Name="ID" DefaultValue="1"  Type = "Int32" /> 
         <asp:SessionParameter Name="AE_UserID" SessionField="UserID" />  
     </WhereParameters>  
    </asp:EntityDataSource> 
     <asp:Label ID="Msglbl" runat="server" Text="" Visible="false"></asp:Label>

    <div align="left" style="padding-top:100px">
        &nbsp;&nbsp;<asp:Label align="left" ID="CloseMsglbl" runat="server" ForeColor="Red" Text="Please note that this window will close in 4 seconds...." Visible="false"></asp:Label>
    </div>  



</div>
 
</asp:Content>

