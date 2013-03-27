using System;
using System.Web.UI;
using System.Collections.Generic;
using PathfinderModel;

public partial class controls_KeyContactsScript : UserControl
{
    /// <summary>
    /// Get Headquarter ids to substitue selected plan_id when viewing DOD accounts (all contacts are assigned to headquarters)
    /// </summary>
    public string DODHeadquarters
    {
        get
        {
            using (PathfinderEntities context = new PathfinderEntities())
            {
                List<string> list = new List<string>();

                foreach ( int id in context.GetDODHeadQuarters() )
                {
                    list.Add(id.ToString());
                }

                string ids = string.Join(",", list.ToArray());
                if ( !string.IsNullOrEmpty(ids) )
                    return ids;

                return "0";
            }
        }
    }
}