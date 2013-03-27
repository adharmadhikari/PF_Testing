using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Collections;
using System.Data.Objects;


public partial class standardreports_all_formularydrilldown : PageBase
{
    protected override void OnPreRenderComplete(EventArgs e)
    {
        //string val = Request.QueryString["Section_ID"];
        //string[] arrSecID = val.Split(',');

        ////Hide and show the grid based on Section_ID filter selection.
        //if (arrSecID.Length > 1)
        //{
        //    if (arrSecID.Contains("1"))
        //    {
        //        // this.FDDHeaderComm.Visible = true;
        //        this.divFDDComm.Style.Remove("display");
        //        this.divFDDComm.Style.Add("display", "");
        //    }
        //    else
        //    {
        //        // this.FDDHeaderComm.Visible = false;
        //        this.divFDDComm.Style.Remove("display");
        //        this.divFDDComm.Style.Add("display", "none");
        //    }
        //}
        //else
        //{
        //    if (arrSecID[0] == "0") //if All option is selected then make everything visible.
        //    {
        //        //  this.FDDHeaderComm.Visible = true;
        //        this.divFDDComm.Visible = true;
        //    }
        //}

        base.OnPreRenderComplete(e);
    }
}
