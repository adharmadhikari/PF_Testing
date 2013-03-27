<%@ Control Language="C#" AutoEventWireup="true" CodeFile="alert.ascx.cs" Inherits="Controls_alert" %>
    <div id="dashboardAlert">
        <div class="header">
            <div class="title"><div id="warningIcon" class="warningIcon"></div><div class="text"></div></div>
            <div class="tools">
                <img id="Img1" class="showHideBtn close" alt="close" title="close" runat="server" src="~/content/images/spacer.gif" onclick="$hideAlert()" />
            </div>
            <div class="clearAll"></div>
        </div>
        <div class="message"></div>    
    </div>    