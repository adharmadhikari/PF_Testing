<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Popup.master" AutoEventWireup="true" CodeFile="PlanInfoDetailsPopup.aspx.cs" Inherits="todaysaccounts_commercial_PlanInfo" %>
<%@ Register src="~/todaysaccounts/controls/PlanInfoAddress.ascx" tagname="PlanInfoAddress" tagprefix="pinso" %>
<%@ Register src="~/todaysaccounts/controls/KeyContactsListGrid.ascx"  tagname="KeyContactsGrid" tagprefix="pinso" %>
<%@ Register src="~/todaysaccounts/controls/CoveredLives.ascx"  tagname="CoveredLives" tagprefix="pinso" %>
<%@ Register src="~/todaysaccounts/controls/BenefitDesignComm.ascx"  tagname="BenefitDesign" tagprefix="pinso" %>
<%@ Register src="~/todaysaccounts/controls/BenefitDesignMedD.ascx" tagname="MedDBenefitDesign" tagprefix="pinso" %>

<%-- Today's Accounts - Commercial -  Plan Information Modal --%>

<asp:Content ID="Content1" ContentPlaceHolderID="scriptContainer" Runat="Server">
    <script type="text/javascript">
        var planID = <%=Request["Plan_ID"] %>;
       
        var initgrid = false;

        function onKCDataBinding(sender, args)
        {            
            if(!initgrid)
            {  
                $clearGridFilter(sender, "Original_Section");
                $setGridFilter(sender, "Plan_ID", planID, "EqualTo", "System.Int32");
                initgrid = true;
                sender.dataBind();//correct filters should be set so bind now
            }
        }

    </script>
</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="main" Runat="Server">    

    <div class="popupTopLft">
        <div id="Div3" class="areaHeader" runat="server"><asp:Literal runat="server" ID="Literal3" Text='<%$ Resources:Resource, SectionTitle_PlanInfoAddress %>' /></div>        
        <pinso:PlanInfoAddress ID="PlanInfoAddress" runat="server" ContainerID="infoPopup" />
        <div id="Div1" class="areaHeader" runat="server"><asp:Literal runat="server" ID="coveredLivesHdr" Text='<%$ Resources:Resource, SectionTitle_CoveredLives %>' /></div>
        <pinso:CoveredLives runat="server" ID="coveredLives" ContainerID="infoPopup"  />        
    </div>
    
    <div class="popupTopRt">
        <div id="Div2" class="areaHeader" runat="server"><asp:Literal runat="server" ID="Literal2" Text='<%$ Resources:Resource, SectionTitle_BenefitDesign %>' /></div>
        <pinso:BenefitDesign runat="server" ID="benefitDesign"  ContainerID="infoPopup" />
        <pinso:MedDBenefitDesign ID="medDBenefitDesign" runat="server" ContainerID="infoPopup" />
    </div>
            
    <div class="clearAll"></div>    
    
    <div class="popupArea" id="keycontactsList" runat="server">
        <div class="areaHeader tileContainerHeader" runat="server">
            <div class="title"><asp:Literal runat="server" ID="Literal1" Text='<%$ Resources:Resource, SectionTitle_KeyContacts %>' /></div>
            <div class="pagination" style="float:right"></div>
            <div class="clearAll"></div>
        </div>
        <pinso:KeyContactsGrid runat="server" ID="keyContacts" OnClientDataBinding="onKCDataBinding" ContainerID="infoPopup" />
    </div>
</asp:Content>


