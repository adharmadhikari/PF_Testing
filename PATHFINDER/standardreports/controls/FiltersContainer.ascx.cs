using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using Pinsonault.Web.UI;

public partial class standardreports_controls_FiltersContainer : System.Web.UI.UserControl
{
    protected override void OnLoad(EventArgs e)
    {
        string module = Request.QueryString["module"];
        int channelID = 0;                

        int.TryParse(Request.QueryString["channel"], out channelID);

        using ( PathfinderModel.PathfinderEntities context = new PathfinderModel.PathfinderEntities() )
        {

            Control filterControl;

            IQueryable<PathfinderModel.SectionReportFilter> filters = context.GetReportFilters(module, Pinsonault.Web.Session.ClientID, channelID);
            var q = from f in filters
                    orderby f.Display_Order, f.Filter_ID 
                    select new
                    {
                        FilterName = f.ReportFilter.Filter_Name,                        
                        ControlSource = f.ReportFilter.Control_Source,
                        IsOption = f.Is_Option,
                        DefaultValue = f.Default_Value
                    };

            foreach ( var filter in q )
            {
                try
                {
                    filterControl = Page.LoadControl(filter.ControlSource);
                    filterControl.ID = filter.FilterName.Replace(' ', '_');
                    if ( !string.IsNullOrEmpty(filter.DefaultValue) && filterControl is IFilterControl )
                        ((IFilterControl)filterControl).DefaultValue = filter.DefaultValue;
                }
                catch ( Exception ex )
                {
                    //rethrow as custom exception so it does not return as a 404
                    throw new HttpException(500, ex.Message);
                }

                if ( !filter.IsOption )
                    placeholder.Controls.Add(filterControl);
                else
                    placeholder2.Controls.Add(filterControl);
            }
        }

        
        //titleRptOptions.Visible = placeholder2.Controls.Count > 0;


        base.OnLoad(e);
    }
}
