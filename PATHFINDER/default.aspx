<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="default.aspx.cs" Inherits="_Default" EnableTheming="true" Theme="pathfinder" %>

<asp:Content runat="server" ContentPlaceHolderID="Main" ID="Main">
    <asp:ScriptManagerProxy runat="server" ID="scriptManagerProxy1">
        <Scripts>
            <asp:ScriptReference Path="~/content/scripts/jquery-ui-1.7.2.custom.min.js" />
            <asp:ScriptReference Path="~/content/scripts/login.js" />
        </Scripts>
    </asp:ScriptManagerProxy>
    <script type="text/javascript">
        var _window;
        function pathfinderrx() {
            if (!_window)
                _window = window.open("login.aspx", "PathfinderRx", "left=0,top=0,resizable=1,height=" + window.screen.height + ",width=" + window.screen.width);
            else {
                try {
                    _window.focus();
                }
                catch (ex) {
                    _window = null;
                    pathfinderrx();
                }
            }
        }
    </script>
    <div id="loginPage" class="signIn">
        <asp:Image ID="Image1" SkinID="topImage" runat="server" />
        <div class="inside">
            <div style="text-align: center">
                <h1>
                    Welcome to</h1>
            </div>
            <br />
            <p>
                &nbsp;</p>
            <div class="welcome">
                <asp:Image SkinID="loginLogo" ID="PFLogo" runat="server" /> </div>
            <div class="clearAll">
            </div>            
            <div style="text-align: center"><h2><a href="javascript:pathfinderrx()">Sign In</a></h2></div>
        </div>
        <div class="clearAll">
        </div>
        <asp:Image ID="Image2" SkinID="btmImage" runat="server" />
    </div>
    
         <telerik:radwindowmanager EnableEmbeddedSkins="false" Skin="pathfinder" id="RadWindowManager1" runat="server" DestroyOnClose="true" Modal="true" 
           Behaviors="Close" VisibleTitlebar="false">    
    </telerik:radwindowmanager>


</asp:Content>
