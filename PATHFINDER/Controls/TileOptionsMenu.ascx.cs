using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Controls_TileOptionsMenu : System.Web.UI.UserControl
{
    /// <summary>
    /// If specified the user must have the appropriate role in order to view the menu options.
    /// </summary>
    public string UserRole { get; set; }

    /// <summary>
    /// If true the user will be presented with a warning dialog about the contents of the exported data.  They must hit Accept to continue.
    /// </summary>
    public bool ExportConfirm { get; set; }

    public string Module { get; set; }
    public string ExportHandler { get; set; }
    public string ContainerID { get; set; }

    public string Channel { get; set; }

    public Controls_TileOptionsMenu()
    {
        ExportConfirm = true; //default to true        
    }

    protected override void OnLoad(EventArgs e)
    {
        //PathfinderApplication.Support.RegisterComponentWithClientManager(this.Page, tileOptionsMenu.ClientID);
        Pinsonault.Web.Support.RegisterComponentWithClientManager(this.Page, tileOptionsMenu.ClientID, null, ContainerID);
        base.OnLoad(e);
    }

    protected override void OnPreRender(EventArgs e)
    {
        //Check role if one is specified.  Visibility of control will then be determined by user's role.
        if ( !string.IsNullOrEmpty(UserRole) )
        {
            Visible = HttpContext.Current.User.IsInRole(UserRole);
        }

        if ( ExportConfirm )
            tileOptionsMenu.OnClientItemClicked = "onExportMenuItemClicked2";

        foreach (RadMenuItem menuItem in tileOptionsMenu.Items[0].Items)
        {
            menuItem.Value = string.Format("{0}\"type\":\"{1}\",\"module\":\"{2}\",\"channel\":\"{3}\",\"exportHandler\":\"{4}\"{5}", "{", menuItem.Value, Module,Channel,ExportHandler, "}");
        }

        base.OnPreRender(e);
    }
}
