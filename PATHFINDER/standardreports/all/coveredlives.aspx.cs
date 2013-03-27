using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Drawing;
using Dundas.Charting.WebControl;
using System.Collections.Generic;
using System.Reflection;
using System.Collections;
using System.Globalization;
using System.ComponentModel;
using Telerik.Web.UI;
using Pathfinder;
using Pinsonault.Data.Reports;

public partial class standardreports_all_coveredlives : PageBase
{

    ArrayList arrSegmentName = new ArrayList();
    ArrayList arrLives = new ArrayList();

    int _selectableDataIndex = 0;

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            
            object o = e.Row.DataItem;

            PropertyDescriptorCollection properties = ((ICustomTypeDescriptor)o).GetProperties();
            
            PropertyDescriptor pMarketSegmentId = properties["MarketSegmentId"];
            PropertyDescriptor pLis = properties["Is_LIS"]; 

            int intId = Convert.ToInt32(pMarketSegmentId.GetValue(o));
            int intIsLis = Convert.ToInt32(pLis.GetValue(o));

            if (intIsLis == 1)
                e.Row.Cells[2].CssClass = "srIndentedText";

            if (isSelectable(intId, intIsLis))
            {
                PropertyDescriptor pMarketSegmentName = properties["MarketSegmentName"];
                PropertyDescriptor pTotalLives = properties["TotalLives"]; 

                string strName = Convert.ToString(pMarketSegmentName.GetValue(o));
                int intLives = Convert.ToInt32(pTotalLives.GetValue(o));                

                e.Row.Attributes["_color"] = ReportColors.StandardReports.GetColorAsHexString(_selectableDataIndex % 6);
                e.Row.Attributes["_lives"] = intLives.ToString("n0");
                e.Row.Attributes["onclick"] = string.Format("showCoveredLivesDetails({0},{1})", intId,intIsLis);
                e.Row.Attributes["id"] = string.Format("ms{0}_{1}", intId,intIsLis);

                _selectableDataIndex++;
            }
        }

    }

    bool isSelectable(int segment,int IsLis)
    {
        //any valid segment or uninsured 101 (because it is shown in chart and that is selectable)
        return (segment < 100 ) || segment == 101;
    }
   
}
