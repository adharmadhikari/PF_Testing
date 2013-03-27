using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using Pinsonault.Web;

namespace Pinsonault.Web.UI
{
    /// <summary>
    /// Client side generated CheckboxListClientControl
    /// </summary>
    public class CheckboxListClientControl : ScriptControl
    {
        public CheckboxListClientControl()
        {
        }

        public string Target { get; set; }
        public int MaxItems { get; set; }
        public int BreakCount { get; set; }
        public string DefaultValue { get; set; }

        public string ContainerID { get; set; }

        protected virtual void AddScriptDescriptors(List<ScriptDescriptor> list)
        {
            Control target = FindControl(Target);
            
            if ( target != null )
            {
                ScriptControlDescriptor d = new ScriptControlDescriptor("Pathfinder.UI.CheckboxList", target.ClientID);

                if ( MaxItems > 0 )
                    d.AddProperty("maxItems", MaxItems);

                if ( BreakCount > 0 )
                    d.AddProperty("breakCount", BreakCount);

                if ( !string.IsNullOrEmpty(DefaultValue) )
                    d.AddProperty("defaultValue", DefaultValue);

                list.Add(d);
            }
        }

        protected override IEnumerable<ScriptDescriptor> GetScriptDescriptors()
        {
            List<ScriptDescriptor> list = new List<ScriptDescriptor>();
            AddScriptDescriptors(list);
            return list;
        }

        protected override IEnumerable<ScriptReference> GetScriptReferences()
        {
            //Not bothering to set any script references - we are assuming Pathfinder.UI.GridWrapper class is located in ClientManager.js which should already be included.
            return null;
        }

        protected override void OnLoad(EventArgs e)
        {
            Control target = FindControl(Target);
            Support.RegisterComponentWithClientManager(Page, target.ClientID, null, ContainerID);

            base.OnLoad(e);
        }
    }
}