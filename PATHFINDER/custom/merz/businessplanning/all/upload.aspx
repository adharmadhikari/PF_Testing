<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Modal.master" AutoEventWireup="true" CodeFile="upload.aspx.cs" Inherits="custom_merz_businessplanning_all_upload" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script type="text/javascript">



    function RefreshDDRs() {

        getMyKCGrid().control.get_masterTableView().rebind();

        var oManager = window.top.GetRadWindowManager();
        window.setTimeout(CloseWin, 4000);
    }

    function CloseWin() {
        var manager = window.top.GetRadWindowManager();

        var window1 = manager.getWindowByName("Popupwindow");
        if (window1 != null)
        { window1.close(); }
    }
    function getMyKCGrid() {

        return window.top.$get("ctl00_Tile3_MedicalPolicy1_RadGridbppOLICY");

    }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="title" Runat="Server">Document Upload
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="tools" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="main" Runat="Server">
 <br /><br />

    <table width="100%">
    <tr align="left">
        <td><asp:Literal runat="server" ID="Literal1" Text='<%$ Resources:Resource, Label_Document_Type %>' /> :</td>
        <td>
            <telerik:RadComboBox runat="server" ID="DocumentType" DataSourceID="dsDocType" DataTextField="Document_Type_Name" DataValueField="Document_Type_ID" 
                AppendDataBoundItems="true" Skin="pathfinder" EnableEmbeddedSkins="false" MaxHeight="300px" >
            <Items>
                <telerik:RadComboBoxItem Text="--No Selection--" Value="0" Selected="true"/>
            </Items>
            </telerik:RadComboBox>
        </td>
    </tr>
    <tr align="left">
        <td>File :</td>
        <td width="75%"><asp:FileUpload ID="FileUpload1" runat="server"  /></td>
    </tr>
    </table>
<br />
<asp:Button ID="btnUpload" runat="server" Text="Upload" onclick="btnUpload_Click"  CausesValidation="true"/>   
<br /><br />
 <asp:RequiredFieldValidator ID="reqexp1" runat="server" ControlToValidate="DocumentType" 
               ErrorMessage="Please select a valid document type." InitialValue="--No Selection--" Text="Please select a valid document type." 
               SetFocusOnError="true">
 </asp:RequiredFieldValidator>  
  <br /><asp:Label ID="msgLbl" runat="server" Text="" /><br />


    <asp:EntityDataSource ID="dsDocType" runat="server" EntitySetName="BusinessPlanDocumentTypesSet" DefaultContainerName="PathfinderMerzEntities" ConnectionString="name=PathfinderMerzEntities" 
        AutoGenerateWhereClause="true">
    </asp:EntityDataSource>    

 </asp:Content>

