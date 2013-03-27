using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Pinsonault.Web;

public partial class custom_controls_FilterTimeFrame : System.Web.UI.UserControl
{
  protected void Page_Load(object sender, EventArgs e)
    {
        Support.RegisterComponentWithClientManager(Page, rdlTF.ClientID, null, "moduleOptionsContainer");
    }
    protected override void OnLoad(EventArgs e)
    {

        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "TimeFrame", "function timeFrameTypeChanged(f,d){var c=f.get_value();var e=new Date();switch(parseInt(c,10)){case 1:var b=new Date();b.setMonth(b.getMonth()+1);b.setDate(0);$('#timeFrame').hide();$('#txtFrom').val(String.format('{0}/1/{1}',e.getMonth()+1,e.getFullYear()));$('#txtTo').val(b.format('MM/dd/yyyy'));break;case 2:$('#timeFrame').hide();$('#txtFrom').val(String.format('1/1/{0}',e.getFullYear()));$('#txtTo').val(e.format('MM/dd/yyyy'));break;case 3:$('#timeFrame').show();break}reportFiltersResize()};", true);
        
        rdlTF.OnClientLoad = "timeFrameTypeChanged";
        rdlTF.OnClientSelectedIndexChanged = "timeFrameTypeChanged"; 
        base.OnLoad(e);
    }
}
