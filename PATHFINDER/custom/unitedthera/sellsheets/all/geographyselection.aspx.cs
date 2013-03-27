using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using PathfinderClientModel;
using Pinsonault.Web;

public partial class custom_unitedthera_sellsheets_geographyselection : InputFormBase
{
    protected override void OnPreInit(EventArgs e)
    {
        this.Response.Cache.SetNoStore();
        base.OnPreInit(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Int32 ssid = Convert.ToInt32(Request.QueryString["Sell_Sheet_ID"]);
        String CurrentStep = "";
        Int32 StatusID, StepOrder;

        txtSSID.Value = ssid.ToString();

        //Load page
        using (PathfinderClientEntities context = new PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString))
        {
            var ssStatesQuery =
                (from d in context.SellSheetStatesSet
                 where d.Sell_Sheet_ID == ssid
                 select d).ToList().Select(d => d.State_ID);

            txtStates.Value = string.Join(",", ssStatesQuery.ToArray());

            //Select the Geography Name
            SellSheet ss = null;
            ss = (from d in context.SellSheetSet
                  where d.Sell_Sheet_ID == ssid
                  select d).First();

            txtGeoName.Text = ss.Geography_Name;

            //Get Current_Step and Status_ID for selected sellsheet.
            CurrentStep = ss.Current_Step.ToString();
            StatusID = Convert.ToInt32(ss.Status_ID);

            //Get the Step_Order for selected sellsheet's Current_Step.
            SellSheetStep ssSteps = null;
            ssSteps = (from d in context.SellSheetStepSet
                       where d.Step_Key == CurrentStep
                       select d).First();
            StepOrder = Convert.ToInt32(ssSteps.Step_Order);

            //Show warning message("Saving changes will reset the plan selection") only for completed sell sheets 
            //with Current_Step is equal to or beyond Step4(Plan Selection).
            if ((StatusID == 2) && (StepOrder >= 4))
                msglbl.Visible = true;
            else //Else hide the message.
                msglbl.Visible = false;
        }
    }

    protected override bool IsRequestValid()
    {
        return true;
    }

    protected void btnNext_Click(object sender, EventArgs e)
    {
        try
        {
            String CurrentStep = "";
            Int32 StatusID, StepOrder;
            Boolean GeoChanged;

            using (PathfinderClientEntities context = new PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString))
            {
                Int32 ssid = Convert.ToInt32(Request.QueryString["Sell_Sheet_ID"]);

                //Determine which states need to be removed from database
                var ssStatesToDeleteQuery =
                    from d in context.SellSheetStatesSet
                    where d.Sell_Sheet_ID == ssid
                    select d;

                //Add to this ArrayList when parsing the drug selections
                System.Collections.ArrayList statesToAdd = new System.Collections.ArrayList();

                if (Request.Form[txtStates.UniqueID].IndexOf(",") > 0)
                {
                    string[] splitStates = Request.Form[txtStates.UniqueID].Split(new Char[] { ',' });

                    foreach (string s in splitStates)
                    {
                        string state = s.Replace("us_", "").ToUpper();

                        statesToAdd.Add(state);

                        //Append delete query                        
                        ssStatesToDeleteQuery = ssStatesToDeleteQuery.Where(d => d.State_ID != state);
                    }
                }
                else
                {
                    string state = Request.Form[txtStates.UniqueID].Replace("us_", "").ToUpper();

                    statesToAdd.Add(state);

                    //Append delete query
                    ssStatesToDeleteQuery = ssStatesToDeleteQuery.Where(d => d.State_ID != state);
                }

                if (ssStatesToDeleteQuery != null)
                {
                    //If the state count to be deleted is zero then there is no change in the selection 
                    //so set the geo changed flag to false else true.
                    if (ssStatesToDeleteQuery.Count() == 0)
                        GeoChanged = false;
                    else
                        GeoChanged = true;
                }
                else
                {
                    GeoChanged = false;
                }

                //Delete drugs that are no longer selected
                foreach (var stateToDelete in ssStatesToDeleteQuery.Select(d => d))
                    context.DeleteObject(stateToDelete);

                //Add states that are not in the database
                foreach (string s in statesToAdd)
                {
                    var ssStatesToAddQuery =
                        (from d in context.SellSheetStatesSet
                         where d.Sell_Sheet_ID == ssid
                         where d.State_ID == s
                         select d).Count();

                    //If count is zero meaning the state is newly added and it doesn't exists in the database for selected sell sheet.
                    if (ssStatesToAddQuery == 0)
                    {
                        updateSSState(ssid, s);
                        GeoChanged = true;
                    }
                }

                //If geo changed then delete data from sell_sheet_plans table to reset the plan selection.
                if (GeoChanged == true)
                {
                    //Get list of plans for selected Sell_Sheet_ID
                    var ssPlansQuery =
                        from d in context.SellSheetPlansSet
                        where d.Sell_Sheet_ID == ssid
                        select d;

                    //Delete all the records from Sell_Sheet_Plans table for selected Sell_Sheet_ID
                    //since the selected plans are no longer valid after new geography selection.
                    foreach (var plan in ssPlansQuery) context.DeleteObject(plan);
                }

                //Get Current_Step and Status_ID for selected sellsheet.
                SellSheet ssSelSheet = null;
                ssSelSheet = (from d in context.SellSheetSet
                              where d.Sell_Sheet_ID == ssid
                              select d).First();
                CurrentStep = ssSelSheet.Current_Step.ToString();
                StatusID = Convert.ToInt32(ssSelSheet.Status_ID);

                //Get the Step_Order for selected sellsheet's Current_Step.
                SellSheetStep ssSteps = null;
                ssSteps = (from d in context.SellSheetStepSet
                           where d.Step_Key == CurrentStep
                           select d).First();
                StepOrder = Convert.ToInt32(ssSteps.Step_Order);
                //---------------------------------------------------------

                //Update the Geography Name and Modified fields
                SellSheet ssheet = null;
                ssheet = (from d in context.SellSheetSet
                          where d.Sell_Sheet_ID == ssid
                          select d).FirstOrDefault();

                ssheet.Geography_Name = Request.Form[txtGeoName.UniqueID];
                ssheet.Modified_BY = Pinsonault.Web.Session.FullName;
                ssheet.Modified_DT = DateTime.UtcNow;
                ssheet.Territory_ID = Pinsonault.Web.Session.TerritoryID;

                if (GeoChanged == true)
                {
                    //If geo changed then: for completed sell sheets, change the status from completed to drafted.
                    if (StatusID == 2)
                        ssheet.Status_ID = 1;
                }
                //Logic below is to check if current step needs to be updated
                if (Master.RequestedStep == Master.CurrentStep)
                {
                    string nextStep = (from ss in context.SellSheetStepSet
                                       where ss.Step_Order == (Master.RequestedStep + 1)
                                       select ss.Step_Key).FirstOrDefault();
                    ssheet.Current_Step = nextStep;
                }

                if (GeoChanged == true)
                {
                    //If geo changed then, if the Current_Step is equal to or beyond Step4(Plan Selection) 
                    //then set currentstep to step4(planselection) so user has to finish step4 before going further.
                    if (StepOrder >= 4)
                        ssheet.Current_Step = "planselection";
                }
                context.SaveChanges();

                PostBackResult.Success = true;
            }
        }
        catch
        {
            PostBackResult.Success = false;
        }
    }

    protected void updateSSState(int ssid, string stateID)
    {
        using (PathfinderClientEntities context = new PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString))
        {
            SellSheetStates ssStates = new SellSheetStates();
            ssStates.Sell_Sheet_ID = ssid;
            ssStates.State_ID = stateID;
            
            context.AddToSellSheetStatesSet(ssStates);
            context.SaveChanges();
        }
    }
}