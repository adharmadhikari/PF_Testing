using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class usercontent_Favorites : PageBase
{
    protected override void OnLoad(EventArgs e)
    {
        if ( !Context.User.IsInRole("fav") )
            throw new HttpException(403, Resources.Resource.Message_Favorites_Permission);

        base.OnLoad(e);
    }
}
