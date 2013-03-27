using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class custom_warner_formularyhistoryreporting_controls_FilterTimeFrameComparison : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //Rename controls for selection data
        RadComboBox Year1 = (RadComboBox)filterTimeFrameComparisonSelector1.FindControl("Year");
        RadComboBox Year2 = (RadComboBox)filterTimeFrameComparisonSelector2.FindControl("Year");
        //RadComboBox Year3 = (RadComboBox)filterTimeFrameComparisonSelector3.FindControl("Year");

        RadComboBox MonthQuarter1 = (RadComboBox)filterTimeFrameComparisonSelector1.FindControl("Month_Quarter");
        RadComboBox MonthQuarter2 = (RadComboBox)filterTimeFrameComparisonSelector2.FindControl("Month_Quarter");
        //RadComboBox MonthQuarter3 = (RadComboBox)filterTimeFrameComparisonSelector3.FindControl("Month_Quarter");

        RadComboBox MonthQuarterSelection1 = (RadComboBox)filterTimeFrameComparisonSelector1.FindControl("Month_Quarter_Selection");
        RadComboBox MonthQuarterSelection2 = (RadComboBox)filterTimeFrameComparisonSelector2.FindControl("Month_Quarter_Selection");
        //RadComboBox MonthQuarterSelection3 = (RadComboBox)filterTimeFrameComparisonSelector3.FindControl("Month_Quarter_Selection");

        //RadComboBox RollingQuarterSelection1 = (RadComboBox)filterTimeFrameComparisonSelector1.FindControl("Rolling_Quarter_Selection");
        //RadComboBox RollingQuarterSelection2 = (RadComboBox)filterTimeFrameComparisonSelector2.FindControl("Rolling_Quarter_Selection");
        //RadComboBox RollingQuarterSelection3 = (RadComboBox)filterTimeFrameComparisonSelector3.FindControl("Rolling_Quarter_Selection");

        Year1.ID = "Year1";
        Year2.ID = "Year2";
        //Year3.ID = "Year3";

        MonthQuarter1.ID = "MonthQuarter1";
        MonthQuarter2.ID = "MonthQuarter2";
        //MonthQuarter3.ID = "MonthQuarter3";

        MonthQuarterSelection1.ID = "MonthQuarterSelection1";
        MonthQuarterSelection2.ID = "MonthQuarterSelection2";
        //MonthQuarterSelection3.ID = "MonthQuarterSelection3";

        //RollingQuarterSelection1.ID = "RollingQuarterSelection1";
        //RollingQuarterSelection2.ID = "RollingQuarterSelection2";
        //RollingQuarterSelection3.ID = "RollingQuarterSelection3";
    }
}
