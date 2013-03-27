using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Text;
using PathfinderClientModel;
using Pinsonault.Application.Merz;

public partial class custom_merz_businessplanning_controls_filteraccountselection : System.Web.UI.UserControl
{
    public custom_merz_businessplanning_controls_filteraccountselection()
    {
        ContainerID = "moduleOptionsContainer";
    }
    public string ContainerID { get; set; }

    public bool IncludeAll { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, Thera_ID.ClientID, null, ContainerID);

        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, Section_ID.ClientID, null, ContainerID);
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, Plan_ID.ClientID, null, ContainerID);

        if (IncludeAll)
        {
            RadComboBoxItem itemThera = new RadComboBoxItem("All Therapeutic Areas");
            Thera_ID.Items.Add(itemThera);
            //RadComboBoxItem itemType = new RadComboBoxItem("All Account Types");
            //Section_ID.Items.Add(itemType);
            //RadComboBoxItem itemAccount = new RadComboBoxItem("All Account Names");
            //Plan_ID.Items.Add(itemAccount);
        }

        using (PathfinderMerzEntities context = new PathfinderMerzEntities())
        {
            var q = from t in context.BPTheraListSet
                    orderby t.Thera_Name 
                    select t;

            StringBuilder sb = new StringBuilder("var bpAcctTypes = {");

            IList<BPSectionList> bpSections = context.BPSectionListSet.OrderBy(o => o.Sort_Order).ToList();
            
            appendSectionList(sb, 0, bpSections);
            
            foreach (BPTheraList t in q)
            {
                
                    sb.Append(",");

                    appendSectionList(sb, t.Thera_ID, bpSections);

            }
            sb.Append("};");
            
            sb.AppendFormat("var includeAll = {0};", IncludeAll.ToString().ToLower());
            sb.AppendFormat("var theraCtlID = '{0}';var sectionCtlID = '{1}';var planCtlID = '{2}';", Thera_ID.ClientID, Section_ID.ClientID, Plan_ID.ClientID);
            
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "bpAcctTypes", sb.ToString(), true);
        }

    }
    void appendSectionList(StringBuilder sb, int theraID,  IList<BPSectionList> bpSections)
    {
        sb.AppendFormat("{0}:[", theraID);

        sb.Append(string.Join(",", bpSections.Where(s => s.Thera_ID == theraID).Select(s => string.Format("{0}ID:{1},Name:\"{2}\"{3}", "{", s.Section_ID, s.Section_Name, "}")).ToArray()));

        sb.AppendFormat("]");
    }
    
}
