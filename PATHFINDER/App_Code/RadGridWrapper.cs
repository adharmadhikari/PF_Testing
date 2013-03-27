using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace Pathfinder
{
    /// <summary>
    /// AJAX.Net Extender that is used to add additional functionality to the RadGrid control.  It allows for positioning a custom pager, supports client side default sorting by applying SortExpressions, row merging, and automatic registration with client manager.
    /// </summary>
    public class RadGridWrapper : ScriptControl
    {
        public RadGridWrapper()
        {
            AutoUpdate = true;            
            MergeRows = true;
            RequiresFilter = true;
            AutoLoad = false;
            ShowNumberOfRecords = true;

            ClientTypeName = "Pathfinder.UI.GridWrapper";

            NoRecordsText = Resources.Resource.Message_No_Records;
            LoadingText = Resources.Resource.Message_Loading;
        }

        Telerik.Web.UI.RadGrid _targetControl = null;
        public string Target { get; set; }
        public string ClientTypeName { get; set; }
        public string PagingSelector { get; set; }
        public string StaticData { get; set; }
        public bool RequiresFilter { get; set; }
        public bool CustomPaging { get; set; }
        public bool MergeRows { get; set; }
        public bool AutoUpdate { get; set; }
        public string NoRecordsText { get; set; }
        public string LoadingText { get; set; }
        public bool AutoLoad { get; set; }
        public string Expand { get; set; }
        public string ContainerID { get; set; }
        public int DrillDownLevel { get; set; }
        public bool ShowNumberOfRecords { get; set; }
        public string OnClientDataBinding { get; set; }
        /// <summary>
        /// Comma separated list of grid columns that require conversion to local system date/time
        /// </summary>
        public string UtcDateColumns { get; set; }

        protected override void OnLoad(EventArgs e)
        {            
            base.OnLoad(e);

            if ( !string.IsNullOrEmpty(Target) )
            {
                _targetControl = FindControl(Target) as Telerik.Web.UI.RadGrid;
                if ( _targetControl != null )
                {   
                    //Add OnDataBinding client event that cancels default request and clears grid - it is only called before the GridWrapper is applied to the RadGrid's element.
                    if ( RequiresFilter )
                        _targetControl.ClientSettings.ClientEvents.OnDataBinding = "function(s, a) { if(!s.get_element().control.GridWrapper){a.set_cancel(true);var mt = s.get_masterTableView();mt.set_dataSource([]);mt.dataBind();} }";
                    else
                        _targetControl.ClientSettings.ClientEvents.OnDataBinding = "function(s, a) { if(!s.get_element().control.GridWrapper){a.set_cancel(true);} }";                
                }
                else
                    throw new HttpException(500, string.Format("Target control '{0}' was not found or is not of type Telerik.Web.UI.RadGrid", Target));
            }
            else
                throw new HttpException(500, string.Format("Target property of RadGridWrapper has not been set.", Target));

        }

        protected virtual void AddScriptDescriptors(List<ScriptDescriptor> list)
        {
            if ( _targetControl != null )
            {
                ScriptBehaviorDescriptor d = new ScriptBehaviorDescriptor(ClientTypeName, _targetControl.ClientID);

                if ( !string.IsNullOrEmpty(PagingSelector) )
                    d.AddProperty("pagerSelector", PagingSelector);

                if ( !string.IsNullOrEmpty(StaticData) )
                    d.AddProperty("staticData", StaticData);

                if ( !AutoUpdate )
                    d.AddProperty("autoUpdate", false);

                if ( !RequiresFilter )
                    d.AddProperty("requiresFilter", false);

                if ( CustomPaging )
                    d.AddProperty("customPaging", true);

                if ( _targetControl.MasterTableView.SortExpressions.Count > 0 )
                {
                    d.AddProperty("sortOrder", _targetControl.MasterTableView.SortExpressions.GetSortString());
                }
                if ( !MergeRows )
                    d.AddProperty("mergeRows", false);

                if ( !string.IsNullOrEmpty(NoRecordsText) )
                    d.AddProperty("noRecordsText", NoRecordsText);

                if ( !string.IsNullOrEmpty(LoadingText) )
                    d.AddProperty("loadingText", LoadingText);

                if ( AutoLoad )
                    d.AddProperty("autoLoad", true);

                if ( !string.IsNullOrEmpty(ContainerID) )
                    d.AddProperty("containerID", ContainerID);

                if ( DrillDownLevel > 0 )
                    d.AddProperty("drillDownLevel", DrillDownLevel);

                if(!string.IsNullOrEmpty(OnClientDataBinding))
                    d.AddEvent("dataBinding", OnClientDataBinding);

                if ( !ShowNumberOfRecords )
                    d.AddProperty("showNumberOfRecords", false);

                if ( !string.IsNullOrEmpty(UtcDateColumns) )
                    d.AddProperty("utcDateColumns", UtcDateColumns);

                if ( Pinsonault.Web.Support.HasDataServicePatch )
                    d.AddProperty("inlineCountEnabled", true);

                if ( !string.IsNullOrEmpty(Expand) )
                    d.AddProperty("expand", Expand);

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

    }
}