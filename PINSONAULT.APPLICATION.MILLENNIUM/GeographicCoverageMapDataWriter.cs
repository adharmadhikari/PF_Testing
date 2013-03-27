using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace Pinsonault.Application.Millennium
{
    /// <summary>
    /// Primary data writer for Pathfinder map
    /// </summary>
    public class GeographicCoverageMapDataWriter : MapDataWriterBase
    {
        public GeographicCoverageMapDataWriter(XmlTextWriter writer, string connectionString) : base(writer, connectionString) { }

    }
}