using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class standardreports_controls_ChildFormulary : System.Web.UI.UserControl
{

    public standardreports_controls_ChildFormulary()
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
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, ChildPlanRadGrid.ClientID, null, ContainerID);

        //if (Request.QueryString["Section_ID"].ToString() == "17" || Request.QueryString["Section_ID"].ToString() == "4")
        //{
        //    ChildPlanRadGrid.Columns[6].Visible = true;
        //    ChildPlanRadGrid.Columns[5].Visible = false;
        //}
        //else
        //{
        //    ChildPlanRadGrid.Columns[6].Visible = false;
        //    ChildPlanRadGrid.Columns[5].Visible = true;
        //}
    }
    
}
