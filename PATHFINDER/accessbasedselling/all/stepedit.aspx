<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Modal.master" AutoEventWireup="true" CodeFile="stepedit.aspx.cs" Inherits="accessbasedselling_all_stepedit" %>
<%@ Register Namespace="Pathfinder" TagPrefix="pinso" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="title" Runat="Server">
 Step Edit
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="tools" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="main" Runat="Server">
  <script type="text/javascript">
      function ChangeImg(param) {
           $("#content1").css("visibility", "visible");
          if (param == 1) {
              $get("content1").innerHTML = "<img src='../../content/imagesabs/fs_tiersstepedit.jpg' />";
          }
          else if (param == 2) {
              $get("content1").innerHTML = "<img src='../../content/imagesabs/fs_stepeditpriorauth.jpg' />";
          }
          
      }
  </script>

  <table width="100%">
  <tr>
    <td width="100%">
     
        <table  width="100%">
            <tr>
                <td width="10%"><a id="btn1" href="javascript:ChangeImg(1);"><img id="imgTiersStepEdit" src="../../content/imagesabs/fs_btn1.jpg"/></a></td>
                <td width="90%" align="left"><a id="btn2" href="javascript:ChangeImg(2);"><img id="imgStepEditPriorAuth" src="../../content/imagesabs/fs_btn2.jpg" /></a></td>
            </tr>
        </table>
        
    </td>
  </tr>

  <tr>
   <td width="100%" style="border-bottom: solid 2px black !important;" colspan="2" >&nbsp;</td>
  </tr>
              
  <tr>
   <td width="100%" style="height:30px;" colspan="2" >&nbsp;</td>
  </tr>
              
  <tr>
    <td style="border-right: solid 1px black !important;">
        <div id="content1"  style="visibility:visible;"><img id="imgContent1" src="../../content/imagesabs/fs_tiersstepedit.jpg"/></div>
    </td>
    <td style="padding:0px 20px 0px 20px;" align="left"><img id="imgSlideshow" src="../../content/imagesabs/fs_slideshow.bmp"/>&nbsp;&nbsp;&nbsp;</td>
  </tr>
  
  </table>
</asp:Content>

