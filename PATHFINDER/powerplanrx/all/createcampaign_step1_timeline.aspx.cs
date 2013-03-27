using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Pinsonault.Application.PowerPlanRx;

public partial class createcampaign_step1_timeline : System.Web.UI.Page, IEditPage
{
    protected override void OnInit(EventArgs e)
    {
        dsCampaignInfo.ConnectionString = Pinsonault.Web.Session.ClientDBConnectionString;
        base.OnInit(e);
    }
    protected override void OnLoad(EventArgs e)
    {   
        if( ((MasterPage)this.Master).IsPageEditable )
            formView.ChangeMode(FormViewMode.Edit);
        
        base.OnLoad(e);
    }
    #region IEditPage Members

    public bool Save()
    {        
        formView.UpdateItem(true);
        return true;
    }

    public void Reset()
    {
        //formView.DataBind();
        formView.ChangeMode(FormViewMode.ReadOnly);
    }
    public void Edit()
    {
        formView.ChangeMode(FormViewMode.Edit);
        
    }

    #endregion
}
