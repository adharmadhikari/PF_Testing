using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PathfinderClientModel;
using Pinsonault.Web;

public partial class custom_controls_FilterProducts : System.Web.UI.UserControl
{
    public custom_controls_FilterProducts()
    {
        ContainerID = "moduleOptionsContainer";
        MaxDrugs = 3;
    }
    public string ContainerID { get; set; }
    public int MaxDrugs { get; set; }
    protected override void OnPreRender(EventArgs e)
    {
        //creates the drop down list for Products Discussed based on the Drug_Name
        using ( PathfinderClientEntities context = new PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString) )
        {
              var q = (from p in context.LkpProductsDiscussedSet
                    orderby p.Drug_Name
                    select p ).ToList().Select( p => new GenericListItem { ID = p.Products_Discussed_ID.ToString(), Name = p.Drug_Name });
              if (q != null)
              {
                  //List<GenericListItem> list = q.ToList();
                  //list.Insert(0, new GenericListItem { ID = "0", Name = "All" });
                  Support.RegisterGenericListVariable(this.Page, q.ToArray(), "productsDiscussed");
              }
        }
        base.OnPreRender(e);
    }
    protected override void OnLoad(EventArgs e)
    {
        Support.RegisterTierScriptVariable(this.Page);
        Support.RegisterComponentWithClientManager(this.Page, Products_Discussed_ID.ClientID, null, "moduleOptionsContainer");
        Products_Discussed_ID.OnClientLoad = "function(s,a){$createCheckboxDropdown(s.get_id(), 'ProductsIDOptionList', productsDiscussed, {'maxItems':" + MaxDrugs.ToString() + ",'defaultText':'" + Resources.Resource.Label_No_Selection + "'}, {'error':function(){$alert('Maximum " + MaxDrugs.ToString() + " drugs should be selected.');}}, 'moduleOptionsContainer');}";
        base.OnLoad(e);
    }
}
