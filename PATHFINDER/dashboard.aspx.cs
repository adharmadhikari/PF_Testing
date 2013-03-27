using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PathfinderModel;
using System.Text;

public partial class dashboard : PageBase
{
    protected override void OnLoad(EventArgs e)
    {
        ((MasterPageBase)Master).PreventPostback();

        mapOptions.Visible = Context.User.IsInRole("actvmap");

        menuMyAccts.Visible = Context.User.IsInRole("myaccts");

        //map.Visible = IsTodaysAccounts;

        base.OnLoad(e);
    }

    protected string ApplicationID
    {
        get
        {
            if ( PreviousPage != null )
            {
                HiddenField hiddenField = PreviousPage.FindControl("ctl00$Main$hApplicationID") as HiddenField;
                if ( hiddenField != null )
                {
                    string value = hiddenField.Value;
                    if ( !string.IsNullOrEmpty(value) )
                        return value;
                }
            }

            return "1";
        }    
    }

    protected bool IsTodaysAccounts
    {
        get
        {
            string currentState = CurrentUIState;
            if(!string.IsNullOrEmpty(currentState))
            {
                return currentState.IndexOf(",\"Application\":1,") > -1;
            }

            return true; //return true because TA is default app
        }
    }

    protected string CurrentUIState
    {
        get
        {
            if ( PreviousPage != null )
            {
                HiddenField hiddenField = PreviousPage.FindControl("ctl00$Main$hFavoriteID") as HiddenField;
                if ( hiddenField != null )
                {
                    string value = hiddenField.Value;
                    if ( !string.IsNullOrEmpty(value) )
                    {
                        int id = 0;
                        if ( int.TryParse(value, out id) )
                        {
                            if ( id > 0 )
                            {
                                using ( PathfinderModel.PathfinderEntities context = new PathfinderModel.PathfinderEntities() )
                                {

                                    Favorite favorite = context.FavoriteSet.FirstOrDefault(f => f.Favorite_ID == id && f.User.User_ID == Pinsonault.Web.Session.UserID);
                                    if ( favorite != null )
                                        return favorite.Data;
                                }
                            }
                        }
                    }
                }
            }

            HttpCookie cookie = Request.Cookies["s"];
            if ( cookie != null )
            {
                string val = cookie.Value;
                if(!string.IsNullOrEmpty(val))
                {
                    return HttpUtility.UrlDecode(val);
                }
            }

            return "\"\"";
        }
    }

    protected override void OnPreRender(EventArgs e)
    {
        Pinsonault.Web.Support.RegisterScriptVariables(this);

        base.OnPreRender(e);
    }
}
