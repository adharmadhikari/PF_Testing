<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="login.aspx.cs" Inherits="login" EnableTheming="true" Theme="pathfinder" %>
<%@ Register Src="~/Controls/login.ascx" TagName="login" TagPrefix="pinso" %>

<asp:Content runat="server" ContentPlaceHolderID="Main" ID="Main">
    <asp:ScriptManagerProxy runat="server" ID="scriptManagerProxy">
        <Scripts>
            <%--<asp:ScriptReference Path="~/content/scripts/jquery-ui-1.7.2.custom.min.js" />--%>
            <asp:ScriptReference Path="~/content/scripts/login.js" />
        </Scripts>
    </asp:ScriptManagerProxy>
    
     <telerik:radwindowmanager EnableEmbeddedSkins="false" Skin="pathfinder" id="RadWindowManager1" runat="server" DestroyOnClose="true" Modal="true" 
           Behaviors="Close" VisibleTitlebar="false">    
    </telerik:radwindowmanager>
        
    <pinso:login runat="server" ID="loginCtrl" />
        
        <div id="dialog" style="background-color:#fff;"></div>
                <div class="require">PathfinderRx runs best with the following specifications; <span>Operating System</span> - Windows XP, <span>Browser</span> - Internet Explorer V.8, <span>Screen Resolution</span> - 1024x768 or higher, <span>Flash Player</span> - V.8 or higher</div>

</asp:Content>
