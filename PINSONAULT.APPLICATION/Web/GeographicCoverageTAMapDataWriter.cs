using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using PathfinderModel;


namespace Pinsonault.Web
{
    /// <summary>
    /// Summary description for GeographicCoverageTAMapDataWriter
    /// </summary>
    public class GeographicCoverageTAMapDataWriter : MapDataWriterBase
    {
        //bool _activateMap = false;

        public GeographicCoverageTAMapDataWriter(XmlTextWriter writer, string connectionString)
            : base(writer, connectionString)
        {
            //_activateMap = HttpContext.Current.User.IsInRole("actvmap");
        }

        public override string DefaultCategory
        {
            get { return "tad"; }
        }

        protected override string GetAreaCategory(MapGeographyData item, ApplicationState applicationState)
        {

            string category = base.GetAreaCategory(item, applicationState);

            //if ( !_activateMap )
            //{
                if ( category == MapDataWriterAttributes.Categories.Default  
                        || category == MapDataWriterAttributes.Categories.Covered
                        || category == MapDataWriterAttributes.Categories.CoveredWithRestrictions
                        || category == MapDataWriterAttributes.Categories.NotCovered )
                {
                    category = "ta";
                }
                else if ( category != MapDataWriterAttributes.Categories.NotAvailable )
                {
                    category = "tad";
                }

            //}

            return category;
        }
    }
}