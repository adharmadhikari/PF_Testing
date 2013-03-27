<%@ Control Language="C#" AutoEventWireup="true" CodeFile="footer.ascx.cs" Inherits="controls_footer" %>
<%@ OutputCache VaryByParam="none" Shared="true" Duration="10" %>
<div class="inside">
    <asp:HyperLink ID="disclaimer" NavigateUrl="javascript:disclaimer()" runat="server" Text="Privacy Policy"></asp:HyperLink>
    |
    <asp:HyperLink ID="terms" NavigateUrl="javascript:terms()" runat="server" Text="Terms and Conditions"></asp:HyperLink>
    - &copy; Pinsonault Associates, LLC. All Rights Reserved.
</div>
