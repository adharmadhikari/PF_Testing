<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Modal.master" AutoEventWireup="true" CodeFile="about.aspx.cs" Inherits="usercontent_about" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="title" Runat="Server">Support
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="tools" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="main" Runat="Server">
    <div class="about">
        <div class="aboutLogo">
            <asp:Image ID="logo" runat="server" SkinID="loginLogo" />
        </div>
        <div class="aboutDesc">
            <%--&lt;Description of what PathfinderRx is should go here.&gt;--%>
            <br />
            <div class="aboutPhone"><asp:Label runat="server" ID="HyperLink1" Text='<%$ Resources:Resource, Label_Customer_Support %>' />:&nbsp;800.372.9009</div>            
            <div class="aboutTime">Monday through Friday 8:30am-5:00pm (EST)</div>
            <br />
            <br />
            <br />
            <div><asp:HyperLink runat="server" ID="supportEmail" /></div>
        </div>
        <div class="aboutFooter">
            <div>
                Version:
                <asp:Label runat="server" ID="version" Text='<%$ AppSettings:appVersion %>' />&nbsp;&bull;&nbsp;<asp:Label runat="server" ID="Label1" Text='<%$ AppSettings:appDate %>' />            
            </div>
        </div>
    </div>
</asp:Content>

