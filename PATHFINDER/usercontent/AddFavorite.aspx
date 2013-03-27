<%@ Page Title="" Theme="pathfinder" Language="C#" MasterPageFile="~/MasterPages/Modal.master" AutoEventWireup="true" CodeFile="AddFavorite.aspx.cs" Inherits="usercontent_AddFavorite" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="title" Runat="Server">
    <asp:Literal runat="server" Text='<%$ Resources:Resource, DialogTitle_AddFavorite %>' />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="main" Runat="Server">
    <div class="modalForm">
    <span class="coreTextBox">
        <span class="bg">
            <span class="bg2">
        <input type="text" id="name" onkeypress="keyPress(event)" class="textBox"/>
            </span>
        </span>
    </span>
    <br />
    <input class="formButton" type="button" id="save" onclick="saveFavorite()" value='<%= Resources.Resource.Favorite_Save %>' />
    <br />
    <input class="formButton" type="button" id="cancel" onclick="$closeWindow()" value='<%= Resources.Resource.Favorite_Cancel %>' />
    <span id="errMsg"></span>
    </div>
    <script type="text/javascript">

            $addHandler($get("name"), "keypress", keyPress);
            $("#errMsg").ajaxError(saveFavoriteError);
            
            function keyPress(e)
            {
                if (e.charCode == 13)
                {
                    saveFavorite();
                    e.stopPropagation();
                    e.preventDefault();
                }
            }

            function saveFavorite()
            {
                $.post("../services/pathfinderservice.svc/AddFavorite", { Name: $get("name").value, Data: window.top.clientManager.get_CurrentUIStateAsText() }, saveFavoriteComplete);               
            }

            function saveFavoriteComplete(html, status, response)
            {
                if (status == "success")
                    $closeWindow();
                else
                    $("#errMsg").text("Favorite was not saved.");                
            }
            
            function saveFavoriteError(event, request, settings)
            {
                $("#errMsg").text("Favorite was not saved.");
            }

    </script>
</asp:Content>

