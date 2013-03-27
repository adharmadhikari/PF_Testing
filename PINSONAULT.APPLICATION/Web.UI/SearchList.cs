using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace Pinsonault.Web.UI
{
    /// <summary>
    /// Summary description for SearchList
    /// </summary>
    public class SearchList : ScriptControl
    {
        public SearchList()
        {
        }

        public string Target { get; set; }

        public string ClientManagerID { get; set; }
        public string ContainerID { get; set; }
        public string ServiceUrl { get; set; }
        public string QueryFormat { get; set; }
        public string QueryValues { get; set; }
        public string DataField { get; set; }
        public string DataValue { get; set; }
        public string TextField { get; set; }
        public string MultiSelectHeaderText { get; set; }
        public string WaterMarkText { get; set; }
        public bool MultiSelect { get; set; }
        public int MaxHeight { get; set; }
        public int OffsetX { get; set; }
        public int OffsetY { get; set; }

        //clientManager
        //queryValues

        protected virtual void AddScriptDescriptors(List<ScriptDescriptor> list)
        {

            ScriptControlDescriptor d = new ScriptControlDescriptor("Pathfinder.UI.SearchList", Target);

            //d.AddProperty(
            if(!string.IsNullOrEmpty(ClientManagerID))
                d.AddComponentProperty("clientManager", ClientManagerID);

            if(!string.IsNullOrEmpty(ContainerID))
                d.AddProperty("containerID", ContainerID);

            if(!string.IsNullOrEmpty(ServiceUrl))
                d.AddProperty("serviceUrl", ServiceUrl);

            if(!string.IsNullOrEmpty(QueryFormat))
                d.AddProperty("queryFormat", QueryFormat);

            if ( !string.IsNullOrEmpty(QueryValues) )
                d.AddProperty("queryValues", QueryValues);

            if ( !string.IsNullOrEmpty(DataField) )
                d.AddProperty("dataField", DataField);

            if ( !string.IsNullOrEmpty(TextField) )
                d.AddProperty("textField", TextField);

            if (!string.IsNullOrEmpty(DataValue))
                d.AddProperty("dataValue", DataValue);

            if (!string.IsNullOrEmpty(MultiSelectHeaderText))
                d.AddProperty("multiSelectHeaderText", MultiSelectHeaderText);

            if (!string.IsNullOrEmpty(WaterMarkText))
                d.AddProperty("waterMarkText", WaterMarkText);

            d.AddProperty("multiSelect", MultiSelect);

            if ( MaxHeight > 0 )
                d.AddProperty("maxHeight", MaxHeight);

            if ( OffsetX != 0 )
                d.AddProperty("offsetX", OffsetX);
            if ( OffsetY != 0 )
                d.AddProperty("offsetY", OffsetY);

            list.Add(d);
        }

        protected override IEnumerable<ScriptDescriptor> GetScriptDescriptors()
        {
            List<ScriptDescriptor> list = new List<ScriptDescriptor>();
            AddScriptDescriptors(list);
            return list;
        }

        protected override IEnumerable<ScriptReference> GetScriptReferences()
        {
            //Not bothering to set any script references - we are assuming Pathfinder.UI.SearchList class is located in ClientManager.js which should already be included.
            return null;
        }
    }
}