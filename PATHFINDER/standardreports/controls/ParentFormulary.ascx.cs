using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class standardreports_controls_ParentFormulary : System.Web.UI.UserControl
{

    public standardreports_controls_ParentFormulary()
    {
        ContainerID = "section2";
    }

    public string ContainerID { get; set; }       

    protected override void OnInit(EventArgs e)
    {
        
        base.OnInit(e);
    }

    
    
    protected void Page_Load(object sender, EventArgs e)
    {
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, ParentPlanRadGrid.ClientID, null, ContainerID);
        
        if (Request.QueryString["Section_ID"].ToString() == "17")
        {
            ParentPlanRadGrid.Columns[5].Visible = true;
            ParentPlanRadGrid.Columns[4].Visible = false;

        }
        else
        {
            ParentPlanRadGrid.Columns[5].Visible = false;
            ParentPlanRadGrid.Columns[4].Visible = true;

        }
    }

   
}
