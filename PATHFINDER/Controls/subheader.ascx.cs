using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using PathfinderModel;
using System.Text;

public partial class controls_subheader : System.Web.UI.UserControl
{
    protected override void OnLoad(EventArgs e)
    {
        favoritesOptions.Visible = Context.User.IsInRole("fav");

        base.OnLoad(e);
    }
}
