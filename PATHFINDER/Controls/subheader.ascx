<%@ Control Language="C#" AutoEventWireup="true" CodeFile="subheader.ascx.cs" Inherits="controls_subheader" %>
<div class="navbar">
    <div class="viewSelect">
       <telerik:RadMenu SkinID="mainMenu" EnableEmbeddedSkins="false" EnableViewState="false" runat="server" ID="applicationMenu" ClickToOpen="true"  />
       <%--MENUCHANGE --%>
       <telerik:RadComboBox ID="channelMenu" runat="server" EnableEmbeddedSkins="false" SkinID="planInfoCombo" Skin="pathfinder"  />
      
       <%--<telerik:RadMenu SkinID="channel" EnableEmbeddedSkins="false" EnableViewState="false" runat="server" ID="channelMenu" ClickToOpen="true"  />--%>
    </div>  
    <div class="addFav" runat="server" id="favoritesOptions">
            <span class="coreBtn">
                <span class="bg">
                    <span class="bg2">
                    <asp:Label CssClass="favLabel" runat="server" Text='<%$ Resources:Resource, Label_Add_To_Favorites %>'></asp:Label>  
                    <asp:HyperLink runat="server" SkinID="addFav" ID="HyperLink4" href="javascript:void(0)" onclick="$openWindow('usercontent/addfavorite.aspx', null, null, 450, 150)" ToolTip='<%$ Resources:Resource, Label_Add_To_Favorites_Tooltip %>'  CssClass="button" /></span>
                    <asp:HyperLink runat="server" SkinID="addFavDropDn" ID="linkViewFavorites" href="javascript:void(0)" onclick="clientManager.showFavoritesList()" ToolTip='<%$ Resources:Resource, Label_View_Favorites %>' Text='<%$ Resources:Resource, Label_View_Favorites %>'  />
                </span>
            </span>
    </div>
    <div  class="contactSearch" id="trainingOpt" onclick="openTrainingMenu()">
        <span class="coreTextBox ">
            <span class="bg">
                 <span class="bg2">
                      <span class="textBox" id="Span1" style="cursor:pointer;margin-top:0px;">View Full Training & Education Menu</span>
                 </span>
            </span>
        </span>    
    </div>
    <div class="contactSearch" id="contactSearchOpt">
        <span class="coreTextBox ">
            <span class="bg">
                 <span class="bg2">
                      <span class="textBox" id="contacts" style="cursor:pointer;margin-top:0px;"><asp:Literal runat="server" Text='<%$ Resources:Resource, DialogTitle_ContactSearch  %>'/></span>
                 </span>
            </span>
        </span>
            <%--<div class="goSearch"><a href="javascript:void(0)"></a></div>--%>
    </div>
    <div class="clearAll">
    </div>
</div>
