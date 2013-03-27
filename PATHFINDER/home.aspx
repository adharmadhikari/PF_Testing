<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="home.aspx.cs" Inherits="_home" EnableTheming="true" Theme="pathfinder" %>
<%@ OutputCache VaryByParam="None" Duration="1"  NoStore="true" %>

<asp:Content runat="server" ContentPlaceHolderID="Main" ID="Main">
    <asp:ScriptManagerProxy runat="server" ID="scriptManagerProxy">
        <Scripts>
            <asp:ScriptReference Path="~/content/scripts/jquery-ui-1.7.2.custom.min.js" />
            <asp:ScriptReference Path="~/content/scripts/jquery.cookie.js" />
            <asp:ScriptReference Path="~/content/scripts/default.js" />
        </Scripts>
    </asp:ScriptManagerProxy>
    
    <script type="text/javascript">
        function selectFavorite(id)
        {
            $("#ctl00_main_hFavoriteID").val(id);
            $("#ctl00_main_favoritePostBackProxy").click();
        }

        function onApplicationChanged()
        {
            //clear ui state cookie - no longer valid if application changed.
            document.cookie = "s=";
            
            setApplicationID();
            $("#ctl00_main_applicationPostBackProxy").click();
        }
        
        function setApplicationID()
        {
            $get('ctl00_main_hApplicationID').value = $find("ctl00_main_rcbApplication").get_value();
        }
    </script>
    
    <telerik:radwindowmanager EnableEmbeddedSkins="false" Skin="pathfinder" id="RadWindowManager1" runat="server" DestroyOnClose="true" Modal="true"  Behaviors="Close" VisibleTitlebar="false" />

    <div class="home">
        <h2>
            Welcome to your Tactics Dashboard <%= Session["FirstName"] %>. Lets get started!</h2>
        <a href="usercontent/accountmanagement.aspx">Manage account</a>
        <ul id="sortable">
            <li id="item-1">Reporting and Analytics 
                <asp:LinkButton runat="server" ID="linkApp" PostBackUrl="~/dashboard.aspx" OnClientClick="setApplicationID()">
                    <img src="content/images/reporting.jpg" alt="Reporting &amp;amp; Analytics" />
                </asp:LinkButton>     
                <div style="display:none">
                    <asp:Button runat="server" ID="applicationPostBackProxy" PostBackUrl="~/dashboard.aspx" />
                </div>                           
                <telerik:RadComboBox runat="server" ID="rcbApplication" OnClientSelectedIndexChanged="onApplicationChanged" DataTextField="Name" DataValueField="ID" DataSourceID="dsApplications" OnDataBound="OnApplicationsDataBound"  EnableEmbeddedSkins="false" SkinID="dashboardDropDown"  Skin="pathfinder" />
                
                <asp:HiddenField runat="server" ID="hApplicationID" Value="0" />
                <asp:HiddenField runat="server" ID="hFavoriteID" Value="0" />
                <div style="display:none">
                    <asp:Button runat="server" ID="favoritePostBackProxy" PostBackUrl="~/dashboard.aspx" />
                </div>
            </li>
            <li id="item-2">Notes<a href="#"><img src="content/images/notes.jpg" alt="Notes" /></a><a href="#">Create
                Notes</a></li>
            <li id="item-3">Favorites<a runat="server" id="favoritesOptions" href="javascript:void(0)" onclick="$openWindow('usercontent/favorites.aspx',null, null, 450, 500)"><img src="content/images/favorites.jpg" alt="Favorites" /></a><a
                href="#">My saved reports</a></li>
            <li id="item-4">Messaging<a href="#"><img src="content/images/messaging.jpg" alt="Messaging" /></a><a
                href="#">Send or receive messages</a></li>
            <li id="item-5">Calendar<img src="content/images/calendar.jpg" alt="Calendar" /><a href="#">Saved</a><a
                href="#">New</a></li>
            <li id="item-6">Search<img src="content/images/search.jpg" alt="Search" /><a href="#">Search for
                help or information</a></li>
        </ul>
    </div>
    
    <asp:EntityDataSource runat="server" ID="dsApplications" ConnectionString="name=PathfinderEntities" DefaultContainerName="PathfinderEntities" EntitySetName="ClientApplicationSet" AutoGenerateWhereClause="true">
        <WhereParameters>
            <asp:SessionParameter Name="Client_ID" SessionField="ClientID" Type="Int32" />
        </WhereParameters>
    </asp:EntityDataSource>
</asp:Content>
