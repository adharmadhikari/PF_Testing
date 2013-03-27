<%@ Page Language="C#" AutoEventWireup="true" CodeFile="businessplanning_pdf.aspx.cs" Inherits="custom_reckitt_businessplanning_all_businessplanning_pdf" %>
<%@ Register src="~/todaysaccounts/controls/PlanInfoAddress.ascx" tagname="PlanInfoAddress" tagprefix="pinso" %>
<%@ Register src="~/custom/reckitt/businessplanning/controls/KeyContacts.ascx" tagname="KeyContacts" tagprefix="pinso" %>
<%@ Register src="~/custom/reckitt/businessplanning/controls/AccountSummary.ascx" tagname="AccountSummary" tagprefix="pinso" %>
<%@ Register src="~/custom/reckitt/businessplanning/controls/GoalTactics.ascx" tagname="Goals" tagprefix="pinso" %>
<%@ Register src="~/todaysaccounts/controls/CoveredLives.ascx"  tagname="CoveredLives" tagprefix="pinso" %>
<%@ Register src="~/todaysaccounts/controls/CoveredLivesSM.ascx" tagname="CoveredLivesSM" tagprefix="pinso" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title></title>
        <link href="~/content/styles/main.css" rel="stylesheet" type="text/css" />
        <style type="text/css">
     
        div.tileContainerHeader 
        {
	        background:#2d58a7;
	        color:#fff
        /*	border-bottom:1px solid #CCC; */
        }
         .genBusinessPlanning 
        {
            width:90% !important;
            text-align: left;  
        }
         .genBusinessPlanning td
        {             
            padding: 2px 2px 5px 2px;
            text-align:left;
            cursor:pointer;
            vertical-align: top;
        }
         .genBusinessPlanning td.rn
         {
         	width:50%;
         }
          .genBP_AccountSummaryTable
        {
	        width:100% !important;
	        text-align: left;  
        }
        .genBP_AccountSummaryTable td
        {
	        padding:5px 2px 5px 2px;
	        width:50%;
	        text-align:left;
	        cursor:pointer;
	        vertical-align: top;
        }
        .dashboardTable .rgHeader
        {
	        color:#333333;

        }
        .dashboardTable th.rgHeader
        {
	        text-align:center !important;

        }
        .headerList 
        {
            padding:5px;
            background:#2d58a7;           
            font-weight:bold;
            color:#fff;
        }
          .itemList 
        {
            padding:5px;  
            font-weight:normal;  
        }
        
        .genTable, #rtView
        {
        	width:100% !important;
        }
        .genTable td.rn
        {
	        
	        font-weight: normal
        }      
        .genTable td
        {
        	border:1px solid #ccc;	       
        }
        
        .rgMasterTable
        {
            border:1px solid #ccc;
        }     
        .rgMasterTable
        {
            font-weight: normal;
        }
       
        </style>
       
    </head>
    <body>
       <form id="form1" runat="server">
       <asp:ScriptManager runat="server" ID="scriptManager">
           <Scripts>
             
           </Scripts>
       </asp:ScriptManager>
       <div>
          <div id="Div6" class="areaHeader tileContainerHeader" runat="server">
          <div class="title">
            <asp:Label ID="lblPlanName" runat="server" ></asp:Label>
          </div>
          <div class="clearAll"></div>
       </div>
          <br /><br />
              
          <table class="genBusinessPlanning">
            <tr>
            <td class="rn">  
                <div id="Div3" class="areaHeader tileContainerHeader" runat="server">
                     <div class="title"><asp:Literal runat="server" ID="Literal3" Text='<%$ Resources:Resource, SectionTitle_PlanInfoAddress %>' /></div> 
                     <div class="clearAll"></div>
                </div>
                <pinso:PlanInfoAddress ID="PlanInfoAddress" runat="server" ShowSectionDisclaimer="false"/>
            </td>
            <td class="rn">  
                <div id="Div5" class="areaHeader tileContainerHeader" runat="server">
                    <div class="title"><asp:Literal runat="server" ID="coveredLivesHdr" Text='<%$ Resources:Resource, SectionTitle_CoveredLives %>' ></asp:Literal></div> 
                    <div class="clearAll"></div>
               </div>
               <div>
                    <asp:Panel ID="pnlCoveredLives" runat="server" Visible="true">
                        <pinso:CoveredLives runat="server" ID="coveredLives" ShowSectionDisclaimer="false" />
                    </asp:Panel>
                    <asp:Panel ID="pnlStateMedicaid" runat="server" Visible="false">
                        <pinso:CoveredLivesSM runat="server" ID="coveredLivesSM" ShowSectionDisclaimer="false" />
                    </asp:Panel>
                </div>
             </td>
            
            </tr>
             <tr>
                <td colspan="2">
                     <div id="Div4" class="areaHeader tileContainerHeader" runat="server">
                        <div class="title"><asp:Literal runat="server" ID="Literal4" Text='<%$ Resources:Resource, SectionTitle_KeyContacts %>' /></div>
                        <div class="pagination" style="float:right"></div>
                        <div class="clearAll"></div>
                    </div> 
                    <div>
                        <pinso:KeyContacts ID="KeyContacts" runat="server" />    
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2">        
                     <div id="Div2" class="areaHeader tileContainerHeader" runat="server">
                         <div class="title"><asp:Literal runat="server" ID="Literal2" Text="Account Summary Statement"/></div>
                         <div class="clearAll"></div> 
                     </div>
                     <div>
                        <pinso:AccountSummary ID="AccountSummary" runat="server" />    
                    </div>
              </td>
            </tr>
            <tr>
                <td colspan="2">        
                    <div id="Div1" class="areaHeader tileContainerHeader" runat="server">
                      
                        <div class="clearAll"></div>  
                     </div>
                     <div>
                        <pinso:Goals ID="Goals" runat="server" />    
                    </div>
                </td>    
            </tr> 
          </table>       
          
       </div>  
      </form>
    </body>
</html>
