<%@ Control Language="C#" AutoEventWireup="true" CodeFile="businessplanningscript.ascx.cs" Inherits="custom_reckitt_businessplanning_controls_businessplanningscript" %>
<%@ OutputCache Shared="true" VaryByParam="None" Duration="100" %>
<script type="text/javascript">

var planID = <%=Request["Plan_ID"] %>;
var initgrid = false;

function onKCDataBinding(sender, args)
{            
    if(!initgrid)
    {  
        $setGridFilter(sender, "Plan_ID", planID, "EqualTo", "System.Int32");
        initgrid = true;
        sender.dataBind();//correct filters should be set so bind now
    }
}
 </script>