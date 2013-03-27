using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace Pinsonault.Web
{

    /// <summary>
    /// Data writer for Sell Sheet map
    /// </summary>
    public class SellSheetsMapDataWriter : MapDataWriterBase
    {
        public SellSheetsMapDataWriter(XmlTextWriter writer, string connectionString) : base(writer, connectionString) { }
    }
}