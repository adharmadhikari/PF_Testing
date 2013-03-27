<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DistrictRegionBrandReportPDF.aspx.cs" Inherits="DistrictRegionBrandReportPDF" %>
<%@ Register Src="~/powerplanrx/controls/DistrictRegionBrand_All.ascx" TagName="DistrictRegionBrand_All" TagPrefix="pinso" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
      .headerList 
     {
        padding:5px;
        background:#215fa0;
        font-weight:bold;
        color:#fff;
     }
    .physList
    {
        width:100%;
        margin-bottom:20px;
        font-weight:bold;
    }   
    div.tileSubHeader
    {
    background:#215FA0;
    color:#fff;
    font-weight:bold;
    }
    .itemList 
    {
        padding:5px;  
        font-weight:normal;  
    }
    </style>  
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" /> 
    
    <div>    
    <table>
        <tr>
            <td>
                <asp:Label ID ="lblPageHeading" runat="server" Text="District Region Profile - Brand Trx - Pie Chart" CssClass="physList" ></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Image ID="imgCommercialChart" runat="server" Width="1000px" Height="400px"/>
            </td>
        </tr> 
          
    </table>  
    <br />
        <pinso:DistrictRegionBrand_All runat="server" ID="DistrictRegionBrand_All" />       
    <br />
              
    </div>
    </form>
</body>
</html>
