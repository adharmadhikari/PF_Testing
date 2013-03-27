using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PathfinderClientModel;
using Pinsonault.Web;
using System.Text;
using System.Runtime.Serialization.Json;
using System.IO;
using Pinsonault.Application.MarketplaceAnalytics;

public partial class marketplaceanalytics_controls_FilterTimeFrame : System.Web.UI.UserControl
{
    protected override void OnInit(EventArgs e)
    {
        string connectionString = Pinsonault.Web.Session.GetClientApplicationConnectionString("MarketplaceAnalytics");

        dsMonths.ConnectionString = connectionString;
        dsQuarters.ConnectionString = connectionString;
        dsRollingQuarters.ConnectionString = connectionString;
        dsYears.ConnectionString = connectionString;
        
        base.OnInit(e);
    }

    protected override void OnLoad(EventArgs e)
    {
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, Year_Selection.ClientID, null, "timeframeArea");
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, Quarter_Selection.ClientID, null, "timeframeArea");
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, Month_Selection.ClientID, null, "timeframeArea");
        Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, Rolling_Selection.ClientID, null, "timeframeArea");

        
        if (!Page.IsPostBack)
        {
            using (PathfinderMarketplaceAnalyticsEntities context = new PathfinderMarketplaceAnalyticsEntities(Pinsonault.Web.Session.GetClientApplicationConnectionString("MarketplaceAnalytics")))
            {
                //Get available years, quarters and months to create JSON variable to dynamically populate TimeFrame container options
                var yearQuery =
                    (from d in context.LkpMarketplaceYearsSet
                     orderby d.Year
                     select d.Year);

                //Get available rolling quarters
                string sbRollingQuarter = ""; 

                var rollingQuarterQuery = context.MSRollingQuarterlyBaseSet
                    .Select(q=>q.Data_Quarter).Distinct().ToList().Select(p => new GenericListItem { ID = p.ToString(), Name = p.ToString() });

                Type[] types = { typeof(GenericListItem[]), typeof(GenericListItem) };
                DataContractJsonSerializer serializer = serializer = new DataContractJsonSerializer(typeof(GenericListItem), "root", types);

                using (MemoryStream ms = new MemoryStream())
                {
                    serializer.WriteObject(ms, rollingQuarterQuery.ToArray());
                    string script = UTF8Encoding.UTF8.GetString(ms.ToArray());
                    sbRollingQuarter = string.Format("var rollingQtr = {0};", script);
                }

                //Process years and quarters
                List<int> years = yearQuery.ToList();

                int yearCount = years.Count();
                StringBuilder sbQuarter = new StringBuilder();
                StringBuilder sbMonth = new StringBuilder();
                               

                sbQuarter.Append("var yrQtr = {");
                sbMonth.Append("var yrMth = {");

                int x = 1;
                foreach (int y in years)
                {
                    sbMonth.AppendFormat("{0}:", y);

                    var monthQuery = (from d in context.LkpMarketplaceMonthYearsSet
                                      join n in context.LkpMarketplaceShortLongMonthNamesSet on
                                      d.Data_Month equals n.Number
                                      orderby n.ShortName
                                      where d.Data_Year == y
                                      select n).ToList().Select(p => new GenericListItem { ID = p.Number.ToString(), Name = p.ShortName });

                    using (MemoryStream ms = new MemoryStream())
                    {
                        serializer.WriteObject(ms, monthQuery.ToArray());
                        string script = UTF8Encoding.UTF8.GetString(ms.ToArray());
                        sbMonth.Append(script);
                    }

                    sbQuarter.AppendFormat("{0}:", y);

                    var quarterQuery = (from d in context.LkpMarketplaceQuarterYearsSet
                                        join n in context.LkpMarketplaceShortLongQuarterNamesSet on
                                        d.Data_Quarter equals n.Number
                                        orderby n.ShortName
                                        where d.Data_Year == y
                                        select n).ToList().Select(p => new GenericListItem { ID = p.Number.ToString(), Name = p.ShortName });

                    using (MemoryStream ms = new MemoryStream())
                    {
                        serializer.WriteObject(ms, quarterQuery.ToArray());
                        string script = UTF8Encoding.UTF8.GetString(ms.ToArray());
                        sbQuarter.Append(script);
                    }

                    if (x != yearCount) //only add a comma if it is not the last item
                    {
                        sbMonth.Append(",");
                        sbQuarter.Append(",");
                    }

                    x++;
                }

                sbMonth.Append("};");
                sbQuarter.Append("};");

                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "yrMth", sbMonth.ToString(), true);
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "yrQtr", sbQuarter.ToString(), true);
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "rollingQtr", sbRollingQuarter, true);

                //ClientScriptManager csm = Page.ClientScript;
                //csm.RegisterStartupScript(typeof(Page), "InitializeCheckboxes", "$('#timeFrameYearContainer input:checkbox, #timeFrameQuarterContainer input:checkbox, #timeFrameMonthContainer input:checkbox').each(function(){var id=$(this).attr('id');if($(this).attr('checked'))$('#timeFrameContainer label[for='+id+']').addClass('selectedDateOption');$(this).click(function(){checkboxCheckStatus(this,false)})});$('#timeFrameYearContainer input:checkbox').click(function(){$('#timeFrameYearContainer label').not('#timeFrameYearContainer label[for='+$(this).attr('id')+']').removeClass('selectedDateOption');$('#timeFrameYearContainer input:checkbox:checked').not(this).removeAttr('checked');if($('#timeFrameYearContainer input:checked').length==0)clearQuarterMonth();else $('#timeFrameContainer .selectAll').show()});clearQuarterMonth();$('#timeFrameMonthContainer span').each(function(idx){if(idx==5)$(this).after('<br/><br/>')});$('#timeFrameYearContainer label').each(function(){$(this).bind('click',function(){clearQuarterMonth();for(var key in yrQtr[$(this).text()]){var cbxID=$('#timeFrameQuarterContainer label:contains('+yrQtr[$(this).text()][key].Name+')').attr('for');$('#'+cbxID).removeAttr('disabled');$('#timeFrameQuarterContainer label[for='+cbxID+']').removeClass('disabledCheckbox')}for(var key in yrMth[$(this).text()]){var cbxID=$('#timeFrameMonthContainer label:contains('+yrMth[$(this).text()][key].Name+')').attr('for');$('#'+cbxID).removeAttr('disabled');$('#timeFrameMonthContainer label[for='+cbxID+']').removeClass('disabledCheckbox')}})});", true);

                //Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "CheckBoxFunctions", "function toggleChecked(status,containerID){if(containerID=='timeFrameQuarterContainer'){$resetContainer('divMonthContainer');$('#timeFrameMonthContainer label').removeClass('selectedDateOption')}else{$resetContainer('divQuarterContainer');$('#timeFrameQuarterContainer label').removeClass('selectedDateOption')}$('#'+containerID+' input:checkbox').each(function(){if(!$(this).attr('disabled')){$(this).attr('checked',status);checkboxCheckStatus(this,true)}})}function checkboxCheckStatus(ctrl,allClicked){var id=$(ctrl).attr('id');if($('#'+id).attr('checked'))$('#timeFrameContainer label[for='+id+']').addClass('selectedDateOption');else $('#timeFrameContainer label[for='+id+']').removeClass('selectedDateOption');if(!allClicked){if($('#'+id).parents('#timeFrameQuarterContainer').length>0){$('#optionAllQuarter').attr('checked','');$resetContainer('divMonthContainer');$('#timeFrameMonthContainer label').removeClass('selectedDateOption')}if($('#'+id).parents('#timeFrameMonthContainer').length>0){$('#optionAllMonth').attr('checked','');$resetContainer('divQuarterContainer');$('#timeFrameQuarterContainer label').removeClass('selectedDateOption')}}}function clearQuarterMonth(){$resetContainer('divQuarterContainer');$resetContainer('divMonthContainer');$('#timeFrameMonthContainer :input').attr('disabled',true);$('#timeFrameMonthContainer label').addClass('disabledCheckbox').removeClass('selectedDateOption');$('#timeFrameQuarterContainer :input').attr('disabled',true);$('#timeFrameQuarterContainer label').addClass('disabledCheckbox').removeClass('selectedDateOption');$('#timeFrameContainer .selectAll').hide()}", true);
            }
        }
    }
}