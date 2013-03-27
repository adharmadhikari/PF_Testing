<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DistrictProfileTrxChart.aspx.cs" Inherits="DistrictProfileTrxChart" %>
<%@ Register Src="~/powerplanrx/controls/DistrictProfileChart.ascx" TagName="DistrictProfileChart" TagPrefix="pinso" %>
<%@ Register Src="~/powerplanrx/controls/DistrictProfileGrid_All.ascx" TagName="DistrictProfileGrid_All" TagPrefix="pinso" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
 
      <div class="DistrictProfileDiv">
            <asp:HiddenField ID="hdnQString" runat="server" />
          <%--<div class="exportControls">
               
               <a class="pdf" href="javascript:exportFile('hdnQString','DistrictProfileChart_ChartComm','DistrictProfileChart_ChartPartD','DistrictProfileTrxchart','pdf')" >PDF</a>        
               <a class="print" href="javascript:PrintPage('divPageContent','District Profile Chart')" >Print</a> 
           </div> --%>  
          <div id="divPageContent">                      
                   
                       <!-- <asp:Label ID ="lblPageHeading" runat="server" Text="District Profile - Brand Trx - Pie Chart" CssClass="physList" ></asp:Label>  -->
                    <pinso:DistrictProfileChart runat="server" ID="DistrictProfileChart" />
                    <pinso:DistrictProfileGrid_All runat="server" ID="DistrictProfileGrid_All" />
         </div>
     </div>  
    </form>
</body>
</html>
