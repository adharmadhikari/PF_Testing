using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;
using System.Xml;
using System.Text;
using System.Web.SessionState;

namespace Pinsonault.Web
{
    /// <summary>
    /// Http handler for processing requests for map data.
    /// </summary>
    public abstract class MapDataHandler : GenericHttpHandler
    {
        public MapDataHandler()
        {
        }

        public override bool IsReusable
        {
            get { return true; }
        }

        protected override void InternalProcessRequest(HttpContext context)
        {
            context.Response.ContentType = System.Net.Mime.MediaTypeNames.Text.Xml;
            context.Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(Pinsonault.Web.Support.PageCacheDuration));

            if ( context.User.Identity.IsAuthenticated )
            {
                ApplicationState applicationState = new ApplicationState();
                string data = context.Request.QueryString["s"];
                if ( !string.IsNullOrEmpty(data) )
                {
                    DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(ApplicationState));
                    using ( MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(data)) )
                    {
                        applicationState = (ApplicationState)json.ReadObject(ms);
                    }
                }

                if ( applicationState.Application != Pinsonault.Web.Identifiers.FormularySellSheets )
                {
                    using ( XmlTextWriter writer = new XmlTextWriter(context.Response.Output) )
                    {
                        MapDataWriterBase mapWriter = CreateMapDataWriter(writer, applicationState);

                        mapWriter.UserID = Pinsonault.Web.Session.UserID;

                        mapWriter.WriteData(applicationState);
                    }
                }
                else
                    context.Response.TransmitFile(HttpContext.Current.Server.MapPath("~/Controls/Map/fmASMap/areas/areas_sell_sheet.xml"));
            }
            else
                throw new HttpException(401, "Unauthorized user");
        }


        protected virtual MapDataWriterBase CreateMapDataWriter(XmlTextWriter writer, ApplicationState applicationState)
        {
            switch ( applicationState.Application )
            {
                default://Today's accounts
                    return new GeographicCoverageTAMapDataWriter(writer, Pinsonault.Web.Session.ClientConnectionString);

                case 3://Standard Reports
                    return new GeographicCoverageMapDataWriter(writer, Pinsonault.Web.Session.ClientConnectionString);

                case 8://Formulary Sell Sheets
                    return new SellSheetsMapDataWriter(writer, null);
            }
        }

    }
}