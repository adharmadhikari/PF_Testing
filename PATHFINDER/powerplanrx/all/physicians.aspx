<%@ Page Title="PhysicianList" Language="C#" Theme="impact" AutoEventWireup="true" CodeFile="physicians.aspx.cs" Inherits="physicians" 
    EnableEventValidation ="false" %>
<%@ Register Src="~/powerplanrx/controls/PhysiciansList.ascx" TagName="PhysiciansList" TagPrefix="pinso" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server"><title></title>
    
</head>
<body>
    <form id="form1" runat="server">  
   
     <div class="DistrictProfileDiv">
        <div class="exportControls" id= "divExport" runat="server">            
            <a class="excel" href="Export.aspx?page=physicians&type=excel&<%=Request.QueryString%>">Excel</a>
            <a class="pdf" href="Export.aspx?page=physicians&type=pdf&<%=Request.QueryString%>">PDF</a> 
        </div>    
            <pinso:PhysiciansList runat="server" ID="PhysiciansList" />
     </div>        
    </form>
</body>
</html>
