using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Controls_favoritesList : System.Web.UI.UserControl
{
    public string SelectFavoritesFunctionFormat { get; set; }

    bool _enableDelete = true;
    public bool EnableDelete 
    {
        get { return _enableDelete; }
        set { _enableDelete = value; } 
    }
   
    protected override void OnLoad(EventArgs e)
    {
        if ( string.IsNullOrEmpty(SelectFavoritesFunctionFormat) )
            throw new ApplicationException("SelectFavoritesFunctionFormat property has not been set on the user control.  Please set to a string format that takes two arguments for Favorite ID and Favorite Data.");

        gridView.Columns[1].Visible = EnableDelete;

        base.OnLoad(e);
    }
}
