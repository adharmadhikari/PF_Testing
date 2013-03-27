<%@ Control Language="C#" AutoEventWireup="true" CodeFile="KeyContactsListVA.ascx.cs" Inherits="controls_VAKeyContactsList" %>
<%@ Register src="~/todaysaccounts/controls/KeyContactsListGridVA.ascx" tagname="VAKeyContactsListGrid" tagprefix="pinso" %>
     
     <div class="title">
     </div>
     <div class="tools"><a id="AddKCLnk" href="javascript:OpenVAKC('AddKC','','');">Send New +</a></div> 
     <div class="clearAll"></div>

    <pinso:VAKeyContactsListGrid runat="server" ID="keyContactsListGrid" OnClientRowSelected="onVAKCGridRowClick" />
   

     