using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using Pinsonault.Application.PowerPlanRx;

public partial class createcampaign_step4_bestpractices : System.Web.UI.Page, IEditPage
{
    protected override void OnLoad(EventArgs e)
    {        
        base.OnLoad(e);
    }


    #region IEditPage Members

    public bool Save()
    {  
        return true;
    }

    public void Reset()
    {
      
    }

    public void Edit()
    {
        
    }

    #endregion
}
