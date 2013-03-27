<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PhysiciansPDF.aspx.cs" Inherits="PhysiciansPDF" %>
<%@ Register Src="~/powerplanrx/controls/PhysiciansList.ascx" TagName="PhysiciansList" TagPrefix="pinso" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
    .physList
    {
        width:100%;
        margin-bottom:20px;
        font-weight:bold;
    }
    .headerList 
    {
        padding:5px;
        background:#215fa0;
        font-weight:bold;
        color:#fff;
    }
   .terrHeader 
    {
        padding:5px;
        text-align:left;
        background:#215fa0;
        font-weight:bold;
        color:#fff;
    }  
    .itemList 
    {
        padding:5px;  
        font-weight:normal;  
    }
    .physListHeader
   {
	padding:5px;
    background:#215fa0;
    font-weight:bold;
    color:#fff;  
	text-align:center;
  }
</style> 
</head>

<body>
    <form id="form1" runat="server">
    <div>
        <pinso:PhysiciansList runat="server" ID="PhysiciansList" />
    </div>
    </form>
</body>
</html>
