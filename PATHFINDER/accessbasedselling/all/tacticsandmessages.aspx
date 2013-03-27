<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/Modal.master" Theme="pathfinder" AutoEventWireup="true" CodeFile="tacticsandmessages.aspx.cs" Inherits="accessbasedselling_all_tacticsandmessages" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="title" Runat="Server">
    Tactics/Messages
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="tools" Runat="Server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="main" Runat="Server">

    <script type="text/javascript">

        $(document).ready(loadPage);

        function loadPage()
        {
            var ids = '<%= Request.QueryString["p"] %>';
            
            var review = <%= Request.QueryString["review"] %>;

            ids = ids.split("|");

            var id;
            var text;
            var tbl = $get("tblSummary");
            var row;
            for (var i = 0; i < ids.length; i++)
            {                
                id = ids[i];
                if (id != "")
                {
                    $("#plan" + id).attr("checked", true);
//                    text = $("label[for=plan" + id + "]").text();
//                    row = tbl.insertRow(-1);
//                    for (var j = 0; j < 8; j++)
//                    {
//                        row.insertCell(-1);
//                    }
//                    $(row.cells[0]).text(text);
                }
            }
            
            if(review)
            {
                $("#tblSelect").hide();
                $("#tblSummary").hide();
                $("#tblReview").show();
            }
        }

        function getHtml(id)
        {
            var html = "<ul>";
            $(id).each(function() { if (this.checked) html += "<li>" + $("label[for=" + this.id + "]").text(); });
            html += "</ul>";

            if (html != "<ul></ul>")
                return html;

            return null;
        }

        function save()
        {
            var id;
            var text;
            var tbl = $get("tblSummary");
            var row;
            var ids = [];
            var l = tbl.rows.length;
            
            for (var r = 0; r < l - 1; r++)
            {
                tbl.deleteRow(1);
            }

            var html1 = getHtml(".tact");
            var html2 = getHtml(".msg");
            var html3 = getHtml(".train");
            var html4 = getHtml(".strat");
            var d = new Date();

            if (html1 || html2 || html3 || html4)
            {
                $(".plan").each(function() { if (this.checked) ids[ids.length] = this.id; });

                for (var i = 0; i < ids.length; i++)
                {
                    id = ids[i];
                    if (id != "")
                    {
                        text = $("label[for=" + id + "]").text();
                        row = tbl.insertRow(-1);
                        for (var j = 0; j < 8; j++)
                        {
                            row.insertCell(-1);
                        }
                        $(row.cells[0]).text(text);
                    }
                }


                for (var i = 1; i < tbl.rows.length; i++)
                {
                    $(tbl.rows[i].cells[1]).html(html1);
                    $(tbl.rows[i].cells[2]).html(html2);
                    $(tbl.rows[i].cells[3]).html(html3);
                    $(tbl.rows[i].cells[4]).html(html4);
                    $(tbl.rows[i].cells[5]).html("District 2");
                    $(tbl.rows[i].cells[6]).html(d.format("MM/dd/yyyy"));
                    $(tbl.rows[i].cells[7]).html("&nbsp;");
                }
            }  
        }
    </script>

     <div style="margin:10px 20px 10px 20px;cursor:default;" class="dashboardTable">
        <table id="tblSelect" style="text-align:left;width:100%;border: solid 1px #cccccc;border-left-style:none;border-bottom-style:none;" cellspacing="0">
            <tr style="background-color:#2D58A8;color:#fff;">
                <td>Account Name</td>
                <td>Assign Tactics</td>
                <td>Assign Messaging - Pull-Through</td>
                <td>Assign Training</td>                                                
                <td>Strategic Planning</td>
            </tr>
            <tr>
                <td>  
                    <input id="plan4" class="plan" type="checkbox" /><label for="plan4">Humana PDP Enhanced S5884-030 (Part D)</label><br />
                    <input id="plan1" class="plan" type="checkbox" /><label for="plan1">Blue Shield of California (CA)</label><br />
                    <input id="plan2" class="plan" type="checkbox" /><label for="plan2">Universal Ameri - MemberHealth</label><br />
                    <input id="plan3" class="plan" type="checkbox" /><label for="plan3">UHC/Pacificare/AARP Med D</label><br />
                    <input id="plan5" class="plan" type="checkbox" /><label for="plan5">United Healthcare</label> 
                </td>
                <td>     
                    <input id="tact1" class="tact" type="checkbox" /><label for="tact1">Combination Status Card.pdf</label><br />
                    <input id="tact2" class="tact" type="checkbox" /><label for="tact2">Prior Authorization Tip Sheet.pdf</label><br />
                    <input id="tact3" class="tact" type="checkbox" /><label for="tact3">Formulary Sell Sheet</label>       
                </td>
                <td>                    
                    <input id="msg1" class="msg" type="checkbox" /><label for="msg1">Managed Care Selling Message</label>
                </td>
                <td>                    
                    <input id="train1" class="train" type="checkbox" /><label for="train1">Pull-Through - Life Cycle</label><br />
                    <input id="train2" class="train" type="checkbox" /><label for="train2">Pull-Through -  Role Play</label><br />
                    <input id="train3" class="train" type="checkbox" /><label for="train3">Assessment</label>
                </td>                                                
                <td>                    
                    <input id="strat1" class="strat" type="checkbox" /><label for="strat1">Planning The Call</label>
                </td>                                                                
            </tr>            
        </table>
        
        <br />
        
        <table id="tblSummary" style="text-align:left;width:100%;border: solid 1px #cccccc;border-left-style:none;border-bottom-style:none;"  class="dashboardTable" cellspacing="0">
            <tr style="background-color:#2D58A8;color:#fff;">
                <td>Account Name</td>
                <td>Assign Tactics</td>
                <td>Assign Messaging - Pull-Through</td>
                <td>Assign Training</td>                                                
                <td>Strategic Planning</td>
                <td>Created By</td>
                <td>Created Date</td>
                <td>Regional Director Notes</td>
            </tr>        
        </table>

        <table id="tblReview" style="display:none;text-align:left;width:100%;border: solid 1px #cccccc;border-left-style:none;border-bottom-style:none;"  class="dashboardTable" cellspacing="0">
            <tr style="background-color:#2D58A8;color:#fff;">
                <td>Account Name</td>                
                <td>Assign Tactics</td>
                <td>Status</td>
                <td>Assign Messaging - Pull-Through</td>
                <td>Status</td>
                <td>Assign Training</td>                                                
                <td>Status</td>
                <td>Strategic Planning</td>
                <td>Status</td>     
            </tr>        
           <tr>
                <td rowspan="3">Humana PDP Enhanced S5884-030 (Part D)</td>
                <td>4107 Combo Status Card.pdf</td>
                <td style="background-color:#B8BDE0">Complete</td>
                <td>Managed Care Selling</td>
                <td style="background-color:#B8BDE0">Complete</td>
                <td>Pull-Through - Life Cycle</td>
                <td style="background-color:#B8BDE0">Complete</td>
                <td>Planning The Call</td>
                <td style="background-color:#B8BDE0">Complete</td>
          </tr>
          <tr>
                <td>FL-2536-28 PA Trip Sheet r9.pd</td>
                <td style="background-color:#B8BDE0">Complete</td>
                <td>Managed Care Selling</td>
                <td style="background-color:#B8BDE0">Complete</td>
                <td>Pull-Through - Role Play</td>
                <td style="background-color:#B8BDE0">Complete</td>
                <td>Planning The Call</td>
                <td style="background-color:#B8BDE0">Complete</td>
            </tr>
            <tr>
                <td>Formulary Sell Sheet</td>
                <td style="background-color:#B8BDE0">Complete</td>
                <td>Managed Care Selling</td>
                <td style="background-color:#B8BDE0">Complete</td>
                <td>Assessment</td>
                <td style="background-color:#B8BDE0">100%</td>
                <td>Planning The Call</td>
                <td style="background-color:#B8BDE0">Complete</td>
            </tr>
           <tr>
                <td rowspan="3">Universal Ameri Member Health</td>
                <td>4107 Combo Status Card.pdf</td>
                <td style="background-color:#B8BDE0">Complete</td>
                <td>Managed Care Selling</td>
                <td style="background-color:#B8BDE0">Complete</td>
                <td>Pull-Through - Life Cycle</td>
                <td style="background-color:#B8BDE0">Complete</td>
                <td>Planning The Call</td>
                <td style="background-color:#B8BDE0">Complete</td>
          </tr>
          <tr>
                <td>FL-2536-28 PA Trip Sheet r9.pd</td>
                <td style="background-color:#B8BDE0">Complete</td>
                <td>Managed Care Selling</td>
                <td style="background-color:#B8BDE0">Complete</td>
                <td>Pull-Through - Role Play</td>
                <td style="background-color:#B8BDE0">Complete</td>
                <td>Planning The Call</td>
                <td style="background-color:#B8BDE0">Complete</td>
            </tr>
            <tr>
                <td>Formulary Sell Sheet</td>
                <td style="background-color:#B8BDE0">Complete</td>
                <td>Managed Care Selling</td>
                <td style="background-color:#B8BDE0">Complete</td>
                <td>Assessment</td>
                <td style="background-color:#B8BDE0">100%</td>
                <td>Planning The Call</td>
                <td style="background-color:#B8BDE0">Complete</td>
            </tr>            
        </table>
        
        <div class="tactic_btns" style="text-align:right;">
            <input class="button" type="button" onclick="save()" value="Save" style="width:70px" />
            <input class="button" type="button" value="Edit" style="width:70px" />            
        </div>
     </div>
</asp:Content>

