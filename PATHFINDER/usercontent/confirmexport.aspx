<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Modal.master" AutoEventWireup="true" CodeFile="confirmexport.aspx.cs" Inherits="usercontent_confirmexport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="title" Runat="Server">
    <asp:Literal runat="server" Text='<%$ Resources:Resource, DialogTitle_Export_Confirmation %>' />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="tools" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="main" Runat="Server">
    <script type="text/javascript">
        //Chrome fix     
        $(document).ready(function()
        {   
            if (chrome)
                $('.exportConfirmText').height(100);            
        });
        
        function onConfirmed(type, module, channel,customHandler)
        {
            if (!customHandler)
                window.top.clientManager.exportView(type, true, module,channel);
            else
                customHandler(type, module);
                
            $closeWindow();
        }

        function onCancelled()
        {
            $closeWindow();
        }
    </script>
    
    <div class="exportConfirmText">
        <asp:Literal runat="server" ID="confirmText" />
    </div>
    <div class="modalFormButtons" style="text-align:center;">        
        <pinso:CustomButtonNonServer runat="server" ID="btnAccept" Text="Accept" onclick="" />
        <pinso:CustomButtonNonServer runat="server" Text="Cancel" onclick="onCancelled()" />
    </div>
</asp:Content>

