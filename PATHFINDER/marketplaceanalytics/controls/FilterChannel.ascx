<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FilterChannel.ascx.cs" Inherits="todaysanalytics_controls_FilterChannel" %>
<div>
    
    <%-- 
    <select id="Segment_ID">
        <option value="1">Commercial</option>
        <option value="2">Medicare Part D</option>
    </select>
    --%>
    <div class="filterGeo">
    <asp:Literal runat="server" ID="filterLabel" Text="Channel" />
    </div>
    
     <telerik:RadComboBox runat="server" ID="Section_ID"  DataTextField="Name" DataValueField="ID"  AppendDataBoundItems="true" Skin="pathfinder" EnableEmbeddedSkins="false" DropDownWidth="185px">
        <Items>
            <telerik:RadComboBoxItem Text="Combined (Commercial, Part D, Managed Medicaid)" Value="-1"/> 
        </Items>
    </telerik:RadComboBox>
    <telerik:RadComboBox runat="server" ID="Plan_ID"  DataTextField="Name" DataValueField="ID" AppendDataBoundItems="true" Skin="pathfinder" EnableEmbeddedSkins="false" DropDownWidth="375px" >
    </telerik:RadComboBox>
    <pinso:ClientValidator runat="server" id="validator1" target="Plan_ID" DataField="Plan_ID" Required="true" Text='Please select an account.' />
    
<%--     <asp:EntityDataSource runat="server" ID="dsFormularyType" DefaultContainerName="PathfinderEntities" ConnectionString="name=PathfinderEntities" EntitySetName="ClientApplicationAccessSet" OrderBy="it.Section.Name" AutoGenerateWhereClause="true">
        <WhereParameters>
            <asp:SessionParameter SessionField="ClientID" Name="ClientID" Type="Int32" DefaultValue="0" />
            <asp:Parameter Name="ApplicationID" DefaultValue="2" Type="Int32" />
        </WhereParameters>
     </asp:EntityDataSource>--%>
       
   
</div>