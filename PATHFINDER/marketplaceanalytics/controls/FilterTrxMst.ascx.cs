using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Runtime.Serialization.Json;
using System.IO;
using Telerik.Web.UI;
using Pinsonault.Web.UI;

public partial class todaysanalytics_controls_FilterTrxMst : UserControl, IFilterControl 
{
    public todaysanalytics_controls_FilterTrxMst()
    {
        ContainerID = "toolbarArea";
    }

    public string ContainerID { get; set; }

    protected override void OnInit(EventArgs e)
    {
        using (PathfinderModel.PathfinderEntities ctx = new PathfinderModel.PathfinderEntities())
        {
            string nrx = "nrx";
            string msn = "msn";
            var p = ctx.ClientModuleSet;
            var q = ctx.ModuleSet;

            int iNrx = p.Where(cm => cm.Client_ID == Pinsonault.Web.Session.ClientID).Where(cm => cm.Modules.Module_Key == nrx).Count();
            if (iNrx > 0)
            {
                //add nrx value in list, if client has nrx enabled
                ListItem linrx = new ListItem();
                linrx.Text = "NRx";
                linrx.Value = "Nrx";
                Trx_Mst.Items.Add(linrx);
            }
            int iMsn = p.Where(cm => cm.Client_ID == Pinsonault.Web.Session.ClientID).Where(cm => cm.Modules.Module_Key == msn).Count();
            if (iMsn > 0)
            {
                //add msn value in list, if client has msn enabled
                ListItem limsn = new ListItem();
                limsn.Text = "Msn";
                limsn.Value = "Msn";
                Trx_Mst.Items.Add(limsn);
            }
        }

        base.OnLoad(e);
    }

    protected override void OnLoad(EventArgs e)
    {

        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, Trx_Mst.ClientID, null, ContainerID);

        
    }

    #region IFilterControl Members

    string _defaultValue;
    public string DefaultValue
    {
        get
        {
            if (_defaultValue == null)
                return "TRx";

            return _defaultValue;
        }
        set
        {
            _defaultValue = value;
        }
    }

    #endregion
}
