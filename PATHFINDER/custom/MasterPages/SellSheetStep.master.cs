using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class custom_MasterPages_SellSheetStep : MasterPageBase
{
    public int RequestedStep
    {
        get { return steps.RequestedStep; }
    }
    public int CurrentStep 
    {
        get { return steps.CurrentStep; }
    }

    protected override void OnPreRender(EventArgs e)
    {
        if ( steps.HasError )
        {
            StepBody.Visible = false;
            StepError.Visible = true;
            errorMsg.InnerText = string.Format(Resources.Resource.Message_FormularySellSheets_Step_Error, steps.CurrentStepText);
        }
        Page.Title = steps.StepShortName;

        base.OnPreRender(e);
    }
}


