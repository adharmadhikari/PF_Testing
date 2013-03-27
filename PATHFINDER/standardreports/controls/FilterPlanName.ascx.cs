using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Data.Objects;
using System.Collections.Specialized;
using System.IO;
using System.Runtime.Serialization.Json;

public partial class standardreports_controls_FilterPlanName : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }


    //protected override void OnLoad(EventArgs e)
    //{

    //    int MasterDB = 2;
    //    int clientID = Pinsonault.Web.Session.ClientID;

    //    if (MasterDB == 1)
    //    {
    //        using (PathfinderModel.PathfinderEntities context = new PathfinderModel.PathfinderEntities())
    //        {
    //            var q = from p in context.FDrilldownSet
    //                    where p.Client_ID == clientID
    //                    select new
    //                    {
    //                        Plan_ID = p.Plan_ID,
    //                        Plan_Name = p.Plan_Name
    //                    };
    //            Plan_ID.DataSource = q.Distinct();
    //            Plan_ID.DataBind();
    //        }

    //    }

    //    else
    //    {
    //        using (PathfinderClientModel.PathfinderClientEntities context = new PathfinderClientModel.PathfinderClientEntities())
    //         {
    //            var q = from p in context.FDrilldownSet
    //                    select new
    //                    {
    //                        Plan_ID = p.Plan_ID,
    //                        Plan_Name = p.Plan_Name
    //                    };
    //            Plan_ID.DataSource = q.Distinct();
    //            Plan_ID.DataBind();


    //        }




    //    }

    //    base.OnLoad(e);
    //}





}
