<%@ Control Language="C#" AutoEventWireup="true" CodeFile="KeyContactsList.ascx.cs" Inherits="controls_KeyContactsList" %>
<%@ Register src="~/todaysaccounts/controls/KeyContactsListGrid.ascx" tagname="KeyContactsListGrid" tagprefix="pinso" %>
     
    <div class="title">
        <%--<telerik:RadComboBox ID="rdcmbDesg" runat="server" Width="65%" DropDownWidth="150px" EnableEmbeddedSkins="false" SkinID="planInfoCombo" Skin="pathfinder" AppendDataBoundItems="true" DataSourceID="dsLkpContactType" DataTextField="Name" DataValueField="ID">
            <Items>
                <telerik:RadComboBoxItem Value="0" Text="(All)"/>
            </Items>
        </telerik:RadComboBox>--%>
     </div>
     <div class="tools"><a id="AddKCLnk" href="javascript:OpenKC('AddKC','');">Send New +</a></div> 
     <div class="clearAll"></div>

    <pinso:KeyContactsListGrid runat="server" ID="keyContactsListGrid" OnClientRowSelected="onKCGridRowClick" />
   

<%--<asp:EntityDataSource ID="dsLkpContactType" runat="server" EntitySetName="ContactTypeSet" ConnectionString="name=PathfinderEntities" DefaultContainerName="PathfinderEntities" 
        AutoGenerateWhereClause="true">
</asp:EntityDataSource>--%>      
     