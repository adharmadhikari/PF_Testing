<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/SingleSection.master" AutoEventWireup="true" CodeFile="businessplanning.aspx.cs" Inherits="custom_merz_businessplanning_all_businessplanning" EnableViewState="true" %>
<%@ Register src="~/custom/merz/businessplanning/controls/BasicInfoCoveredLives.ascx" tagname="InfoLives" tagprefix="merz" %>
<%@ Register src="~/custom/merz/businessplanning/controls/MedicalPolicy.ascx" tagname="MedicalPolicy" tagprefix="merz" %>
<%@ Register src="~/custom/merz/businessplanning/controls/AddEditBusinessPlanning.ascx" tagname="AddEditBP" tagprefix="merz" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="scriptContainer" Runat="Server">
   <pinso:StylesheetOutput runat="server" ID="pdfStyleSheets" Visible="false">
    <StylesheetOutputEntries>
        <pinso:StylesheetOutputEntry Url="~/custom/merz/content/styles/pdf.css" />
    </StylesheetOutputEntries>
    </pinso:StylesheetOutput>
    
   <!--[if IE 6]>
        <style type="text/css">
            #MainBPSection
            {
    	        width:99% !important;
            }
        </style>
    <![endif]-->
   <!--[if IE 7]>
        <style type="text/css">
            #MainBPSection
            {
    	        width:98% !important;
            }
        </style>
    <![endif]-->
   <style type="text/css">

    .MedicareTile
    {
    	text-align: left;
    	font-size: 11px;
    }

    #divTile3
    {
    	overflow:auto;
    	overflow-x:hidden;
    	overflow-y:auto;
    }
 
    .businessplanning .title
    {
    	width:auto !important;
    }
    
    .leftSmTile 
    {
	    margin:2px 0px 2px 2px;
    }

    .leftBP
    {
    	float:left;
    }
    
    .rightBP
    {
    	float:right;
    }

    .mplinks
    {
    	padding:4px 0px 3px 0px;
    }

    .rightMedTile 
    {
	    
	    margin:2px 2px 2px 0px;
    }

    .leftBgTile 
    {	
    	
    }

    .pbmTile
    {
    	
    }
    
    .sppTile
    {
	    border-left:solid 1px #2d58a7;    
    }

    .bpTile
    {
    	border:solid 1px #2d58a7;
    	margin:2px;
    }

    .bpInfo
    {
    	background-color:#e8e8e8;
    	text-align:left;
    	padding:4px;
    }
    
    .bpInfo textarea
    {
    	width:99%;
    }
    
    .bpInfo .bpLabel
    {
    	font-weight:bold;
    }
    
    .leftBPTile
    {
    	float: left !important;
    }
    
    .rightBPTile
    {
    	float: right !important;
    }
    
    .rightSmTile 
    {
        width:23% !important;
    }

    .rightBgTile 
    {
        width:70% !important;
    }
    
    .divborder 
    {
        border: solid 1px #2d58a7;
        height: 196px !important;
        overflow-x: hidden;
 	 	overflow-y: auto;
 	 	vertical-align: top !important;
    }

    .businessplanning .staticTable 
    {
        border-collapse:separate;
    }
    
    .businessplanning .staticTable th
    {
    	background-color: #e8e8e8; 
    	border-left:solid 1px #fff;
    }
    .businessplanning .staticTable .firstCol
    {
    	border-left:none 1px #fff;
    }    
    

    /*///////////////////////////////////////////////////////////////*/
    
    div.dashboardTable
    {
        background-color:#E8E8E8;
    }
    
   
    
    .PDFDivHeader1
    {
        vertical-align: top; 
        text-align: left; 
        padding-top: 2px;
    	margin-top: 1px;
    	font-weight:bold;
    }
    
    .PDFDivMain
    {
        vertical-align: top; 
        height: 600px;
        text-align: left; 
    }
    
    .CoverageTile
    {
	    border-right: #ccc 1px solid;
	    border-bottom: #ccc 1px solid; 
    }

    /*////////////////////////////////////////////////////////*/
</style>

<script type="text/javascript" >
    clientManager.add_pageLoaded(BP_pageInitialized);
    clientManager.add_pageUnloaded(BP_pageUnloaded);
</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Tile3Title" Runat="Server">
    <asp:Literal runat="server" ID="Literal1" Text='<%$ Resources:Resource, SectionTitle_BusinessPlanning %>' />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Tile3Tools" Runat="Server">    
       <a id="EditBP" runat="server" href="javascript:EditBP();" visible="false">Edit</a> 
       <span id="separator1" runat="server" visible="false">|</span>
       <a id="SaveBP" href="javascript:SaveBP();" runat="server" visible="false">Save</a>
       <span id="separator2" runat="server" visible="false">|</span> 
        <a id="ExportBP" runat="server" href="javascript:ExportBP();" visible="false">Export</a> 
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Tile3" Runat="Server">
<asp:HiddenField ID="BP_ID" runat="server" Value="" />
<asp:HiddenField ID="hdnSection_ID" runat="server" Value="" />
<asp:HiddenField ID="hdnPlan_ID" runat="server" Value="" />
<asp:HiddenField ID="hdnThera_ID" runat="server" Value="" />
<asp:HiddenField ID="hdnreport" runat="server" Value="" />
<asp:HiddenField ID="hdnExport" runat="server" Value="" />

<div id="MainBPSection" style="width:100%;">
     <%--<div id="TopSection" class="AlignTopSection divborder" style="width:100%;">--%>
            <merz:InfoLives ID="InfoLives1" runat="server"/>
            <merz:MedicalPolicy ID="MedicalPolicy1" runat="server"/>
        <div class="clearAll"></div>
     <%--</div>--%>
     
     <div id="BottomSection" class="bottomBP" runat="server">
        <merz:AddEditBP ID="AddEditBP1" runat="server"/>
     </div>
 </div>
</asp:Content>


