<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/SingleSectionNoHeader.master" AutoEventWireup="true" CodeFile="default.aspx.cs" Inherits="custom_pinso_all_section2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="scriptContainer" Runat="Server">
    <script type="text/javascript">
        var _index = 1;
           var windowHeight = $(window); 
           var divHeight = windowHeight.height(); 
           var divWidth = windowHeight.width(); 
           if (divWidth <= 1024) {
               var pictureSize = "sm";
              }
           else {
               var pictureSize = "lg";
           }

        function back()
        {
            if (_index > 1)
            {
                _index--;
                changePic(_index, pictureSize);
            }

            $("#linkBack").attr("disabled", _index <= 1);
        }

        function getMax()
        {
            return 17;
        }
        
        function next()
        {
            var max = getMax();    
            
            if (_index < max)
            {
                _index++;
                changePic(_index, pictureSize);
            }
        }

        function changePic(index, size)
        {
            $("#imgDemo").attr("src", "custom/pinso/all/demoimages/" + clientManager.get_ApplicationManager().getChannelUrlName(clientManager.get_Channel()) + "_" + size + "_" + index + ".png");


            var max = getMax();
         
            $("#linkNext").attr("disabled", index >= max);
            $("#linkBack").attr("disabled", index <= 1);
        }
        //style="width:100%;height:100%;"
        $(document).ready(function() { changePic(1, pictureSize); });
        $("#divTile3Container").css("border", "none");
    </script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="Tile3" Runat="Server">
    <div class="ssnh" style="height:25px;right:5px;bottom:5px;position:absolute;">
        <a disabled=true id="linkBack" style="text-align:right; padding-right:10px" href="javascript:back()">
            &lt; Back
        </a>
        <a id="linkNext" href="javascript:next()">Next &gt;</a>
    </div>
    <div style="width:100%;height:95%;">
    <img id="imgDemo" alt="" style="width:100%; height:100%" />
    </div>
    <%--<iframe frameborder="0" style="width:100%;height:100%" src="custom/pinso/all/sellsheetsframe.aspx"></iframe>--%>
</asp:Content>

