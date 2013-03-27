using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Dundas.Maps.WebControl;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Json;
using Pinsonault.Web;
using PathfinderClientModel;
using System.Data.Objects;
using System.Collections.Specialized;

public partial class controls_map : System.Web.UI.UserControl
{
     protected override void OnLoad(EventArgs e)
     {
        configureMap(Request.QueryString["s"]);

         base.OnLoad(e);
     }

     Color getRegionDefaultColor()
     {
         return Color.FromArgb(90,131,206);
     }

     Color getRegionColorByCoverageStatusID(int CoverageStatusID)
     {
         switch (CoverageStatusID)
         {
             case 0:
                 return Color.White;
             case 1:
                 return Color.FromArgb(0, 202, 56);
             case 2:
                 return Color.FromArgb(250, 206, 12);
             case 3:
                 return Color.FromArgb(237, 0, 38);
             default:
                 return Color.LightGray;
         }
     }

    protected void refreshFromDataSource(ApplicationState applicationState)
    {
        //Get coverage status for geographic coverage report - if requesting from standard reports
        Dictionary<string, PathfinderClientModel.MapViewCoverage> formularyCoverageStatuses = null;
        if (applicationState.Application == 3 && applicationState.Drug != null)
        {

            //string s = applicationState.Channel;
            //string[] cha = s.Split(',');
            List<string> s = applicationState.Channel;
            string[] cha = s.ToArray(); 
            int[] channelValues = new int[cha.Length];
            for (int x = 0; x < cha.Length; x++)
            {
                channelValues[x] = Convert.ToInt32(cha[x].ToString());
            }


            using (PathfinderClientModel.PathfinderClientEntities context = new PathfinderClientModel.PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString))
            {
                //formularyCoverageStatuses = context.MapViewCoverageSet.Where(m => m.Section_ID == applicationState.Channel && m.Drug_ID == applicationState.Drug).ToDictionary(m => m.Geography_ID);

                string query = "SELECT VALUE  O FROM MapViewCoverageSet AS O WHERE O.Section_ID IN {" + applicationState.Channel + "} AND m.Drug_ID == "+applicationState.Drug;
                ObjectQuery<MapViewCoverage> o = new ObjectQuery<MapViewCoverage>(query, context);
                formularyCoverageStatuses = o.ToDictionary(m => m.Geography_ID);
            }
        }
        
        bool regionSelected = applicationState.Region != null && applicationState.Region != "US";
        string altName;
        PathfinderClientModel.MapViewCoverage mapViewCoverage;

        //update shapes
        foreach ( Shape shape in map.Shapes )
        {
            altName = shape["ALT_NAME1"].ToString();
            
            shape.MapAreaAttributes = string.Format("onclick='clientManager.set_Region(\"{0}\");'", altName);

            shape.Visible = !regionSelected || (string.Compare(altName, applicationState.Region, true) ==0);

            //if requesting from Standard reports the color of the state is based on Coverage Status
            if (formularyCoverageStatuses != null)
            {
                if (formularyCoverageStatuses.ContainsKey(altName))
                {
                    mapViewCoverage = formularyCoverageStatuses[altName];
                    shape.Color = getRegionColorByCoverageStatusID(mapViewCoverage.Coverage_Status_ID);
                }
                else
                    shape.Color = getRegionColorByCoverageStatusID(-1);
            }
            else
                shape.Color = getRegionDefaultColor();

            shape.Name = altName;

            shape.Font = new Font("Arial", 6F);

            //restore offset regions
            if ( altName == "HI" )
            {
                shape.Offset.X = 51;
                shape.Offset.Y = 5;
            }
            else if ( altName == "AK" )
            {
                shape.Offset.X = 38;
                shape.Offset.Y = -36;
                shape.ScaleFactor = 0.4;
            }

            //if current region is the selected one then update map to zoom in on that state
            if (regionSelected && shape.Visible)
            {
                shape.MapAreaAttributes = "";

                //need to adjust for regions that are offset (such as Hawaii and Alaska)
                shape.Offset.X = 0;
                shape.Offset.Y = 0;
                shape.ScaleFactor = 1;

                shape.CentralPointOffset.X = 0;
                shape.CentralPointOffset.Y = 0;

                map.MapLimits.MaximumX = shape.ShapeData.MaximumExtent.X + .5;
                map.MapLimits.MaximumY = shape.ShapeData.MaximumExtent.Y + .5;
                map.MapLimits.MinimumX = shape.ShapeData.MinimumExtent.X - .5;
                map.MapLimits.MinimumY = shape.ShapeData.MinimumExtent.Y - .5;
            }
        }

    }

    void configureMap(string args)
    {
        ApplicationState appState;
        DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(ApplicationState));
        using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(args)))
        {
            appState = (ApplicationState)json.ReadObject(ms);
        }

        //make map bigger for Standard Reports
        if (appState.Application != Pinsonault.Web.Identifiers.TodaysAccounts)
        {
            map.Width = new Unit(370);
            map.Height = new Unit(230); 
        }

        refreshFromDataSource(appState);

        //Full US
        if ( string.IsNullOrEmpty(appState.Region) )
        {
            map.MapLimits.MaximumX = -70;
            map.MapLimits.MaximumY = 50;
            map.MapLimits.MinimumX = -123;
            map.MapLimits.MinimumY = 24;

            map.Viewport.ContentAutoFitMargin = 5;
            map.Viewport.Zoom = 100;
            map.Viewport.ViewCenter.X = 50.0F;
            map.Viewport.ViewCenter.Y = 50.0F;

            mapTools.Visible = false;
            //imgInfo.Style[HtmlTextWriterStyle.Display] = "none";
        }
        else //Single state
        {
            mapTools.Visible = true;
        }
    }
}
