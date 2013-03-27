using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class standardreports_controls_FormularyStatusTemplateData : System.Web.UI.UserControl
{
    public RadGrid HostedGrid { get { return gridformularystatus; } }

    public string GridTitle 
    { 
        get { return labelTitle.Text; } 
        set { labelTitle.Text = value; }
    }

    protected override void OnLoad(EventArgs e)
    {
            string val = Request.QueryString["Section_ID"];
            if (!string.IsNullOrEmpty(val))
            {
                gridformularystatus.MasterTableView.DataKeyNames = new string[] { "Drug_ID", "Drug_Name", "Geography_ID", "Section_ID" };
                string[] section = val.Split(',');
                if (section.Length > 1)
                {
                    SetColumnVisibility();
                }
            }
            else
            {
                  gridformularystatus.MasterTableView.DataKeyNames = new string[] {"Drug_ID","Drug_Name","Geography_ID"};

                //gridformularystatus.MasterTableView.Columns.FindByUniqueNameSafe("Section_Name").Display = true;
                //SetColumnVisibility();

            }
            GridColumn col = gridformularystatus.MasterTableView.Columns.FindByUniqueNameSafe("Formulary_Lives");

            switch (val)
            {
                case "17":
                    col.HeaderText = "Medicare Part D Lives";
                    break;
                case "1":
                    col.HeaderText = "Commercial Pharmacy Lives";
                    break;
                case "6":
                    col.HeaderText = "Managed Medicaid Lives";
                    break;
                //case "4":
                //    col.HeaderText = "Lives";
                //    break;                           
            }
            base.OnLoad(e);
        
    }

    private void SetColumnVisibility()
    {
        gridformularystatus.MasterTableView.Columns.FindByUniqueNameSafe("Section_Name").Display = true;
        gridformularystatus.MasterTableView.Columns.FindByUniqueNameSafe("Drug_Name").Display = false;
    }

    protected void gridformularystatus_ItemDataBound(object sender, GridItemEventArgs e)
    {
        //if (e.Item is GridDataItem)
        //{
        //    GridDataItem item = (GridDataItem)e.Item;
        //    if (!string.IsNullOrEmpty(Request.QueryString["Section_ID"]))
        //    {
               
        //        HyperLink link1 = (HyperLink)item["F1"].Controls[0];
        //        link1.NavigateUrl = string.Format("javascript:gridFSDrilldown_setfilter({0}, \"{1}\", 1, f1Text, \"{2}\",{3})", item.GetDataKeyValue("Drug_ID"), item.GetDataKeyValue("Drug_Name"), item.GetDataKeyValue("Geography_ID"), item.GetDataKeyValue("Section_ID"));

        //        HyperLink link2 = (HyperLink)item["F2"].Controls[0];
        //        link2.NavigateUrl = string.Format("javascript:gridFSDrilldown_setfilter({0}, \"{1}\", 2, f2Text, \"{2}\",{3})", item.GetDataKeyValue("Drug_ID"), item.GetDataKeyValue("Drug_Name"), item.GetDataKeyValue("Geography_ID"), item.GetDataKeyValue("Section_ID"));

        //        HyperLink link3 = (HyperLink)item["F3"].Controls[0];
        //        link3.NavigateUrl = string.Format("javascript:gridFSDrilldown_setfilter({0}, \"{1}\", 3, f3Text, \"{2}\",{3})", item.GetDataKeyValue("Drug_ID"), item.GetDataKeyValue("Drug_Name"), item.GetDataKeyValue("Geography_ID"), item.GetDataKeyValue("Section_ID"));

        //        HyperLink link4 = (HyperLink)item["F5"].Controls[0];
        //        link4.NavigateUrl = string.Format("javascript:gridFSDrilldown_setfilter({0}, \"{1}\", 4, f5Text, \"{2}\",{3})", item.GetDataKeyValue("Drug_ID"), item.GetDataKeyValue("Drug_Name"), item.GetDataKeyValue("Geography_ID"), item.GetDataKeyValue("Section_ID"));
        //    }
        //    else
        //    {
        //        HyperLink link1 = (HyperLink)item["F1"].Controls[0];
        //        link1.NavigateUrl = string.Format("javascript:gridFSDrilldown_setfilter({0}, \"{1}\", 1, f1Text, \"{2}\",0)", item.GetDataKeyValue("Drug_ID"), item.GetDataKeyValue("Drug_Name"), item.GetDataKeyValue("Geography_ID"));

        //        HyperLink link2 = (HyperLink)item["F2"].Controls[0];
        //        link2.NavigateUrl = string.Format("javascript:gridFSDrilldown_setfilter({0}, \"{1}\",2, f2Text, \"{2}\",0)", item.GetDataKeyValue("Drug_ID"), item.GetDataKeyValue("Drug_Name"), item.GetDataKeyValue("Geography_ID"));

        //        HyperLink link3 = (HyperLink)item["F3"].Controls[0];
        //        link3.NavigateUrl = string.Format("javascript:gridFSDrilldown_setfilter({0}, \"{1}\", 3, f3Text, \"{2}\",0)", item.GetDataKeyValue("Drug_ID"), item.GetDataKeyValue("Drug_Name"), item.GetDataKeyValue("Geography_ID"));

        //        HyperLink link4 = (HyperLink)item["F5"].Controls[0];
        //        link4.NavigateUrl = string.Format("javascript:gridFSDrilldown_setfilter({0}, \"{1}\", 4, f5Text, \"{2}\",0)", item.GetDataKeyValue("Drug_ID"), item.GetDataKeyValue("Drug_Name"), item.GetDataKeyValue("Geography_ID"));
        //    }
        //}
    }
}
  