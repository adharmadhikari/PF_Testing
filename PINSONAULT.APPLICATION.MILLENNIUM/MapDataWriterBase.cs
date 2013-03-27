using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using PathfinderModel;
using Pinsonault.Web;

namespace Pinsonault.Application.Millennium
{

    public class MapDataWriterAttributes
    {
        private MapDataWriterAttributes() { }

        public const string ID = "id";
        public const string Enabled = "enabled";
        public const string ToolTip = "tooltip";
        public const string State = "state";
        public const string StateName = "statename";
        public const string Category = "category";
        public const string Enrollment = "totalcovered";
        public const string MATTYMst = "mst";
        public const string MATTYTrx = "trx";

        public class Categories
        {
            public const string Default = "d";
            public const string NotCovered = "3";
            public const string Covered = "1";
            public const string CoveredWithRestrictions = "2";
            public const string NotAvailable = "0";
            public const string Unknown = "u";
        }
    }

    /// <summary>
    /// Base class for generating FlashMaps xml data for the "areas".
    /// </summary>
    public abstract class MapDataWriterBase
    {
        public MapDataWriterBase(XmlTextWriter writer, string connectionString)
        {
            this.writer = writer;
            ConnectionString = connectionString;
        }

        protected XmlTextWriter writer { get; private set; }

        public string ConnectionString { get; set; }

        public int UserID { get; set; }

        protected virtual IEnumerable<PathfinderModel.MapGeographyData> GetData(PathfinderMillenniumEntities context, ApplicationState applicationState)
        {
            return context.GetMapViewCoverageExecutiveReports(applicationState.Channel, applicationState.Drug, applicationState.UserID, Pinsonault.Web.Session.ClientID, applicationState.restrictions);
        }

        protected virtual IQueryable<State> GetDefaultData(PathfinderModel.PathfinderEntities context, ApplicationState applicationState)
        {
            return context.StateSet;
        }

        public virtual string DefaultCategory
        {
            get { return MapDataWriterAttributes.Categories.Default; }
        }

        /// <summary>
        /// Automatically set in WriteData when it can be determined if Map is enabled for specified application channel.
        /// </summary>
        protected bool MapIsEnabled { get; private set; }

        public void WriteData(ApplicationState applicationState)
        {

            writer.WriteStartElement("areas");
            
            using ( PathfinderModel.PathfinderEntities context = new PathfinderModel.PathfinderEntities() )
            {
                //context.MapViewCoverageSet.MergeOption = System.Data.Objects.MergeOption.NoTracking;
                context.StateEnrollmentSet.MergeOption = System.Data.Objects.MergeOption.NoTracking;
                context.DrugCoverageByStateSet.MergeOption = System.Data.Objects.MergeOption.NoTracking;
                context.StateSet.MergeOption = System.Data.Objects.MergeOption.NoTracking;

                //This restricts the map drilldown and tooltip functionality
                MapIsEnabled = true;// context.UserModuleSet.Count(um => um.App_ID == applicationState.Application
                                    //                                                 && um.Section_ID == applicationState.Channel
                                    //                                                 && um.User_ID == UserID
                                    //                                                 && (um.Module_Key == "map" || um.Module_Key == "geographiccoverage")) > 0;

                using (PathfinderMillenniumEntities c = new PathfinderMillenniumEntities())
                {
                    var q = GetData(c, applicationState);

                    if (q != null)
                    {
                        foreach (PathfinderModel.MapGeographyData item in q)
                        {
                            WriteArea(item, applicationState);
                        }

                        //no records returned for drug
                        if (q.Count() == 0)
                        {
                            var q2 = GetDefaultData(context, applicationState);

                            foreach (State item in q2)
                            {
                                WriteArea(item, applicationState, MapDataWriterAttributes.Categories.Unknown);
                            }
                        }
                    }
                    else
                    {
                        var q3 = GetDefaultData(context, applicationState);

                        foreach (State item in q3)
                        {
                            //at this point it is not CP or Med-D so just show default covered so map isn't all white for Unknown - may want to change to something else
                            WriteArea(item, applicationState, DefaultCategory);
                        }
                    }
                }
            }

            writer.WriteEndElement();

        }

        protected virtual void WriteArea(PathfinderModel.MapGeographyData item, ApplicationState applicationState)
        {
            writer.WriteStartElement("area");

            WriteAreaAttributes(item, applicationState);

            writer.WriteEndElement();
        }

        protected virtual void WriteArea(State item, ApplicationState applicationState, string defaultCategory)
        {
            writer.WriteStartElement("area");

            WriteAreaAttributes(item, applicationState, defaultCategory);

            writer.WriteEndElement();
        }

        protected virtual void WriteAreaAttributes(PathfinderModel.MapGeographyData item, ApplicationState applicationState)
        {
            WriteAreaAttributes(applicationState, item.GeographyID, item.GeographyName, GetAreaCategory(item, applicationState), item.Enrollment, null, null);//, item.MATTY_Mst, item.MATTY_Trx);
        }

        protected virtual void WriteAreaAttributes(State item, ApplicationState applicationState, string defaultCoverage)
        {
            WriteAreaAttributes(applicationState, item.ID, item.Name, defaultCoverage, -1, null, null);
        }

        protected virtual void WriteAreaAttributes(ApplicationState applicationState, string GeographyID, string GeographyName, string category, int Enrollment, int? Mst, int? Trx)
        {
            writer.WriteAttributeString(MapDataWriterAttributes.ID, string.Format("us_{0}", GeographyID.ToLower()));

            writer.WriteAttributeString(MapDataWriterAttributes.State, GeographyID.ToUpper());

            writer.WriteAttributeString(MapDataWriterAttributes.Category, category);

            if ( !IsAreaEnabled(applicationState, GeographyID) )
            {
                writer.WriteAttributeString(MapDataWriterAttributes.Enabled, "false");
                writer.WriteAttributeString(MapDataWriterAttributes.ToolTip, "false");
            }

            writer.WriteAttributeString(MapDataWriterAttributes.StateName, GeographyName);

            writer.WriteAttributeString(MapDataWriterAttributes.Enrollment, Enrollment > -1 ? Enrollment.ToString("n0") : "");
            writer.WriteAttributeString(MapDataWriterAttributes.MATTYMst, Mst != null ? Mst.Value.ToString("n0") : "");
            writer.WriteAttributeString(MapDataWriterAttributes.MATTYTrx, Trx != null ? Trx.Value.ToString("n0") : "");
        }

        protected virtual string GetAreaCategory(PathfinderModel.MapGeographyData item, ApplicationState applicationState)
        {
            if ( item.Category != null )
            {
                if ( applicationState.Geography.HasRegion(item.GeographyID) && Pinsonault.Web.Session.IsInAlignment(item.GeographyID) )
                {
                    //if ( item.Formulary_Status_ID != null )
                    return item.Category;
                    //else
                    //    return MapDataWriterAttributes.Categories.Unknown; //unknown
                }
                else
                    return MapDataWriterAttributes.Categories.NotAvailable;
            }
            else
                return MapDataWriterAttributes.Categories.Unknown;
        }

        protected virtual bool IsAreaEnabled(ApplicationState applicationState, string ID)
        {
            return MapIsEnabled; //;applicationState.Channel == 1 || applicationState.Channel == 6 || applicationState.Channel == 9 || applicationState.Channel == 11 || applicationState.Channel == 17;
        }
    }
}