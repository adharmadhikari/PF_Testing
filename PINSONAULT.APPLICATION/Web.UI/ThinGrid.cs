using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace Pinsonault.Web.UI
{
    /// <summary>
    /// Summary description for ThinGrid
    /// </summary>
    public class ThinGrid : System.Web.UI.ScriptControl
    {
        public ThinGrid()
        {
        }

        Control _targetControl = null;

        public string Target { get; set; }
        public string Url { get; set; }
        public string Params { get; set; }
        public bool StaticHeader { get; set; }

        public string LoadSelector { get; set; }

        /// <summary>
        /// same as OnRowSelected - old property left in for compatibility
        /// </summary>
        public string OnClick
        {
            get { return this.OnRowSelected; }
            set { this.OnRowSelected = value; }
        }
        public string OnRowSelecting { get; set; }
        public string OnRowSelected { get; set; }
        public string OnDataBinding { get; set; }
        public string OnDataBound { get; set; }

        public bool AllowMultiSelect { get; set; }
        public bool AutoLoad { get; set; }

        public string ContainerID { get; set; }
        public bool RequestPageCount { get; set; }
        public bool EnablePaging { get; set; }
        public string pageContainer { get; set; }
        public string pageSelector { get; set; }
        public string pageSize { get; set; }
        public bool AutoUpdate { get; set; }
        public bool getSelectedData { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!string.IsNullOrEmpty(Target))
            {
                _targetControl = FindControl(Target);
            }
        }

        protected override IEnumerable<ScriptDescriptor> GetScriptDescriptors()
        {
            List<ScriptDescriptor> list = new List<ScriptDescriptor>();

            ScriptControlDescriptor d = new ScriptControlDescriptor("Pathfinder.UI.ThinGrid", _targetControl != null ? _targetControl.ClientID : Target);
            list.Add(d);

            //add props and events
            if ( !string.IsNullOrEmpty(Url) )
                d.AddProperty("url", Page.ResolveUrl(Url));

            if ( !string.IsNullOrEmpty(Params) )
                d.AddProperty("params", Params);

            if ( StaticHeader )
                d.AddProperty("staticHeader", true);

            if ( !string.IsNullOrEmpty(OnRowSelecting) )
                d.AddEvent("rowSelecting", OnRowSelecting);

            if ( !string.IsNullOrEmpty(OnClick) )
                d.AddEvent("rowSelected", OnClick);

            if (!string.IsNullOrEmpty(OnDataBinding))
                d.AddEvent("dataBinding", OnDataBinding);

            if (!string.IsNullOrEmpty(OnDataBound))
                d.AddEvent("dataBound", OnDataBound);

            if ( !string.IsNullOrEmpty(LoadSelector) )
                d.AddProperty("loadSelector", LoadSelector);

            if ( AllowMultiSelect )
                d.AddProperty("allowMultiSelect", true);

            if ( AutoLoad )
                d.AddProperty("autoLoad", true);

            if (!string.IsNullOrEmpty(ContainerID))
                d.AddProperty("containerID", ContainerID);

            if (RequestPageCount)
                d.AddProperty("requestPageCount", true);           

            if (EnablePaging)
                d.AddProperty("enablePaging", true);

            if (!string.IsNullOrEmpty(pageContainer))
                d.AddProperty("pageContainer", pageContainer);

            if (!string.IsNullOrEmpty(pageSelector))
                d.AddProperty("pageSelector", pageSelector);

            if (!string.IsNullOrEmpty(pageSize))
                d.AddProperty("pageSize", pageSize);

            if (AutoUpdate)
                d.AddProperty("autoUpdate", true);  

            if(getSelectedData)
                d.AddProperty("getSelectedData", true);

            return list;
        }

        protected override IEnumerable<System.Web.UI.ScriptReference> GetScriptReferences()
        {
            //not used so just return empty list.  this is where you could force a javascript file to be included but I usually add manually
            return new List<ScriptReference>();
        }
    }
}