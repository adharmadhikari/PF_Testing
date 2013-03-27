<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CCRDrilldownScript.ascx.cs" Inherits="custom_controls_CCRDrilldownScript" %>
<%@ OutputCache Shared="true" VaryByParam="None" Duration="100" %>
<script type="text/javascript">


    clientManager.add_pageInitialized(pageInitialized);
    clientManager.add_pageUnloaded(pageUnloaded);

    function pageInitialized() {

        alert(clientManager.get_SelectionData()["Meeting_Activity_ID"]);
    }

    function pageUnloaded() {
        clientManager.remove_pageInitialized(pageInitialized);
        clientManager.remove_pageUnloaded(pageUnloaded);
    }

    
  
</script>

