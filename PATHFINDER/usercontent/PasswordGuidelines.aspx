<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="PasswordGuidelines.aspx.cs" Inherits="usercontent_PasswordGuidelines" %>
<%@ OutputCache NoStore="true" Duration="1" VaryByParam="None" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
    <head id="Head1" runat="server">
        <title>Change Password Guidelines</title> 
        <link id="Link2" runat="server" href="~/content/styles/main.css" rel="stylesheet" type="text/css" />
    </head>
     
    <body>
        <form id="form1" runat="server">            
             <div class="tileContainerHeader">
                <div id="header" class="title">Change Password Guidelines</div>
             </div>
            <div id="divTile2">
                <div class="disclaimer" id="divGenericSecurityClient" runat="server" visible="true">
                    <div class="intro">
                        <asp:Label id="lblPwdLength" runat="server"></asp:Label>                        
                    </div>
                </div> 
                <div class="disclaimer" id="divHighSecurityClient" runat="server" visible="false">
                    <div class="intro">
                    <asp:Label id="lblhighsecurityPwdLength" runat="server"></asp:Label>
                        <br />New password should be composed from elements from at least 3 of the following categories: 
                        <ul>
                            <li>Upper case English alpha characters</li> 
                            <li>Lower case English alpha characters</li>
                            <li>Numeric characters </li>
                            <li>Special characters i.e. $, #, %, @ Etc. </li>
                       </ul>
                    </div>
                </div> 
            </div>
        </form> 
    </body>
</html> 



