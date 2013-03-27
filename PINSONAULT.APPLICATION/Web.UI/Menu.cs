using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace Pinsonault.Web.UI
{
    /// <summary>
    /// Client side generated menu control (more like toolbar).  Used by dashboard for Module options but can be used for selections.
    /// </summary>
    public class Menu : ScriptControl
    {
        public Menu() {}

        public override string CssClass
        {
            get { return TargetCssClass; }
            set { TargetCssClass = value; }
        }

        public string Target { get; set; }
        public string TargetCssClass { get; set; }
        public string SelectedCssClass { get; set; }
        public string OnClientInitialized { get; set; }
        public string OnClientItemClicked { get; set; }

        protected virtual void AddScriptDescriptors(List<ScriptDescriptor> list)
        {

            ScriptBehaviorDescriptor d = new ScriptBehaviorDescriptor("Pathfinder.UI.Menu", Target);

            if ( !string.IsNullOrEmpty(TargetCssClass) )
                d.AddProperty("cssClass", TargetCssClass);
            if ( !string.IsNullOrEmpty(SelectedCssClass) )
                d.AddProperty("selectedCssClass", SelectedCssClass);

            if(!string.IsNullOrEmpty(OnClientInitialized ))
                d.AddEvent("initialized", OnClientInitialized);

            if ( !string.IsNullOrEmpty(OnClientItemClicked) )
                d.AddEvent("itemClicked", OnClientItemClicked);

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
            //Not bothering to set any script references - we are assuming Pathfinder.UI.Menu class is located in Menu.js which should already be included.
            return null;
        }
    }
}