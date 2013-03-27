using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using PathfinderModel;
using PathfinderClientModel;
using Pinsonault.Web;
using System.IO;
using System.Text;

public partial class custom_pinso_sellsheets_classandtemplateselection : InputFormBase
{
    //protected string PageModule { get; set; }
    protected override void OnInit(EventArgs e)
    {
        //PageModule = Path.GetFileNameWithoutExtension(Request.Url.AbsolutePath);
        dsTemplates.ConnectionString = Pinsonault.Web.Session.ClientConnectionString;
        base.OnInit(e);
    }

    protected override void OnPreInit(EventArgs e)
    {
        this.Response.Cache.SetNoStore();
        base.OnPreInit(e);
    }

    protected override void OnLoad(EventArgs e)
    {
        Pinsonault.Web.Support.RegisterComponentWithClientManager(this, rdcmbDrugs.ClientID);
        Pinsonault.Web.Support.RegisterComponentWithClientManager(this, rdcmbTheraClass.ClientID);
        base.OnLoad(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        String CurrentStep = "";
        Int32 StatusID, StepOrder;

        if (!Page.IsPostBack)
        {

            using (PathfinderEntities pfe = new PathfinderEntities())
            {
                string script = getDrugListScript(18, pfe);
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "customdrugListOptions", script, true);
                //var ssThera = (from t in pfe.Client_App_Drug_ListSet
                //               where t.ClientID == 18 & t.App_ID == 8
                //               select new
                //               {
                //                   ID = t.TherapeuticClassID,
                //                   Name = t.TherapeuticClassName,
                //               }).Distinct().ToList().Select(t => new GenericListItem { ID = t.ID.ToString(), Name = t.Name });

                //Pinsonault.Web.Support.RegisterGenericListVariable(this.Page, ssThera.ToArray(), "ssTheraList");
            }
            Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "_sspagevars", string.Format("var drugCtrlID = '{0}'; var theraCtrlID = '{1}';", rdcmbDrugs.ClientID, rdcmbTheraClass.ClientID), true);

            lblBrand.Text = Pinsonault.Web.Session.ClientName;
            Int32 ssid = Convert.ToInt32(Request.QueryString["Sell_Sheet_ID"]);

            //Load page
            using (PathfinderClientEntities context = new PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString))
            {

                var ssDrugTheraQuery =
                    (from d in context.SellSheetDrugsSet
                     where d.Sell_Sheet_ID == ssid
                     select d).ToList().Select(d => string.Format("{0}|{1}", d.Drug_ID, d.Thera_ID));

                txtDrugID.Value = string.Join(",", ssDrugTheraQuery.ToArray());

                var ssTheraQuery =
                    (from d in context.SellSheetDrugsSet
                     where d.Sell_Sheet_ID == ssid
                     select d.Thera_ID).Distinct().ToList().Select(d => d.ToString());

                txtTheraID.Value = string.Join(",", ssTheraQuery.ToArray());

                //Select the Template ID
                SellSheet ss = null;
                ss = (from d in context.SellSheetSet
                      where d.Sell_Sheet_ID == ssid
                      select d).First();
                txtTemplateID.Value = ss.Template_ID.ToString();
                
                //Get Current_Step and Status_ID for selected sellsheet.
                CurrentStep = ss.Current_Step.ToString ();
                StatusID = Convert.ToInt32(ss.Status_ID);

                //Get the Step_Order for selected sellsheet's Current_Step.
                SellSheetStep ssSteps = null;
                ssSteps = (from d in context.SellSheetStepSet
                      where d.Step_Key == CurrentStep 
                      select d).First();
                StepOrder = Convert.ToInt32(ssSteps.Step_Order);

                //Show warning message("Saving changes will reset the plan selection") only for completed sell sheets 
                //with Current_Step is equal to or beyond Step4(Plan Selection).
                if ((StatusID == 2) && (StepOrder >= 4 ))
                    msglbl.Visible = true;
                else //Else hide the message.
                    msglbl.Visible = false;
            }

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
            Boolean DrugSelChanged;

            using (PathfinderClientEntities context = new PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString))
            {
                Int32 ssid = Convert.ToInt32(Request.QueryString["Sell_Sheet_ID"]);

                //Determine which drugs need to be removed from database
                var ssDrugsToDeleteQuery =
                    from d in context.SellSheetDrugsSet
                    where d.Sell_Sheet_ID == ssid
                    select d;

                //Add to this ArrayList when parsing the drug selections
                System.Collections.ArrayList drugsToAdd = new System.Collections.ArrayList();

                if (Request.Form[txtDrugID.UniqueID].IndexOf(",") > 0)
                {
                    //Multiple options selected
                    string[] splitDrug = Request.Form[txtDrugID.UniqueID].Split(new Char[] { ',' });
                    foreach (string s in splitDrug)
                    {
                        drugsToAdd.Add(s);

                        //Split each individual selection - first item is drug ID, second item is Thera ID
                        string[] splitTheraDrug = s.Split(new Char[] { '|' });
                        int drug = Convert.ToInt32(splitTheraDrug[0]);

                        //Append delete query
                        ssDrugsToDeleteQuery = ssDrugsToDeleteQuery.Where(d => d.Drug_ID != drug);                        
                    }
                }
                else //One option selected
                {
                    drugsToAdd.Add(Request.Form[txtDrugID.UniqueID]);

                    string[] splitTheraDrug = Request.Form[txtDrugID.UniqueID].Split(new Char[] { '|' });
                    int drug = Convert.ToInt32(splitTheraDrug[0]);

                    //Append delete query
                    ssDrugsToDeleteQuery = ssDrugsToDeleteQuery.Where(d => d.Drug_ID != drug);                    
                }

                if (ssDrugsToDeleteQuery != null)
                {
                    //If the count of drugs to delete is zero then there is no change in the selection 
                    //so set the drug selection changed flag to false else true.
                    if (ssDrugsToDeleteQuery.Count() == 0)
                        DrugSelChanged = false;
                    else
                        DrugSelChanged = true;
                }
                else
                {
                    DrugSelChanged = false;
                }

                //Delete drugs that are no longer selected
                foreach (var drugToDelete in ssDrugsToDeleteQuery.Select(d => d))
                    context.DeleteObject(drugToDelete);

                //Add drugs that are not in the database
                foreach (string s in drugsToAdd)
                {
                    string[] splitTheraDrug = s.Split(new Char[] { '|' });
                    int drug = Convert.ToInt32(splitTheraDrug[0]);
                    int thera = Convert.ToInt32(splitTheraDrug[1]);

                    //Check if the drug to be added to sell_sheet_drugs table already exists in the table.
                    var ssDrugsToAddQuery =
                        (from d in context.SellSheetDrugsSet
                        where d.Sell_Sheet_ID == ssid
                        where d.Drug_ID == drug
                        where d.Thera_ID == thera
                        select d).Count();

                    //If count is zero meaning the drug is newly added and it doesn't exists in the database for selected sell sheet.
                    if (ssDrugsToAddQuery == 0)
                    {
                        updateSSDrugs(ssid, splitTheraDrug);
                       
                        DrugSelChanged = true;
                    }
                }

                //If drug selection changed then delete data from sell_sheet_plans table to reset the plan selection.
                if (DrugSelChanged == true)
                {//Get list of plans for selected Sell_Sheet_ID
                    var ssPlansQuery =
                        from d in context.SellSheetPlansSet
                        where d.Sell_Sheet_ID == ssid
                        select d;

                    //Delete all the records from Sell_Sheet_Plans table for selected Sell_Sheet_ID
                    //since the selected plans are no longer valid after new drug selection.
                    foreach (var plan in ssPlansQuery) context.DeleteObject(plan);
                }

                ////First check to see if there are any drugs for the current sell sheet, if so delete them since this is an update
                //var ssDrugsQuery =
                //    from d in context.SellSheetDrugsSet
                //    where d.Sell_Sheet_ID == ssid
                //    select d;
                
                //foreach (var drug in ssDrugsQuery) context.DeleteObject(drug);

                ////Save changes to avoid PK violation on insert
                //context.SaveChanges();

                ////Insert the Sell Sheet Thera ID/Drugs
                ////First split the array of selections (7003|67,340|61, etc..)
                //if (Request.Form[txtDrugID.UniqueID].ToString().IndexOf(",") > 0)
                //{
                //    //Multiple options selected
                //    string[] splitDrug = Request.Form[txtDrugID.UniqueID].ToString().Split(new Char[] { ',' });
                //    foreach (string s in splitDrug)
                //    {
                //        //Second, split each individual selection - first item is drug ID, second item is Thera ID
                //        string[] splitTheraDrug = s.Split(new Char[] { '|' });
                //        updateSSDrugs(ssid, splitTheraDrug);
                //    }
                //}
                //else
                //{
                //    //One option selected
                //    string[] splitTheraDrug = Request.Form[txtDrugID.UniqueID].ToString().Split(new Char[] { '|' });
                //    updateSSDrugs(ssid, splitTheraDrug);
                //}


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

                //Update the Template ID, Territory and Modified fields
                SellSheet ssheet = null;
                ssheet = (from d in context.SellSheetSet
                          where d.Sell_Sheet_ID == ssid
                          select d).FirstOrDefault();
                ssheet.Template_ID = Convert.ToInt32(txtTemplateID.Value);
                ssheet.Modified_BY = Pinsonault.Web.Session.FullName;
                ssheet.Modified_DT = DateTime.UtcNow;
                ssheet.Territory_ID = Pinsonault.Web.Session.TerritoryID;

                if (DrugSelChanged == true)
                {
                    //If drug selection changed then: for completed sell sheets, change the status from completed to drafted.
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

                if (DrugSelChanged == true)
                {
                    //If drug selection changed then, if the Current_Step is equal to or beyond Step4(Plan Selection) 
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

    protected void updateSSDrugs(int ssid, string[] splitTheraDrug)
    {
        using (PathfinderClientEntities context = new PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString))
        {
            SellSheetDrugs ssDrugs = new SellSheetDrugs();
            ssDrugs.Sell_Sheet_ID = ssid;
            ssDrugs.Drug_ID = Convert.ToInt32(splitTheraDrug[0]);
            ssDrugs.Thera_ID = Convert.ToInt32(splitTheraDrug[1]);
            ssDrugs.Created_DT = DateTime.Now;
            context.AddToSellSheetDrugsSet(ssDrugs);
            context.SaveChanges();
        }
    }

    static string getDrugListScript(int clientID, PathfinderModel.PathfinderEntities context)
    {
        StringBuilder sb = new StringBuilder("var customdrugListOptions = {");
        StringBuilder sb2 = new StringBuilder("var custommarketBasketListOptions = [");

        int currentTheraID = 0;

        
       //IQueryable<Client_App_Drug_List> drugs = GetUserDrugList(18); //change here to your entity set

       using (PathfinderEntities pfe = new PathfinderEntities())
       {
           var drugs = from d in pfe.Client_App_Drug_ListSet
                   where
                   d.ClientID == clientID & d.App_ID == 8
                   orderby d.TherapeuticClassSortOrder, d.TherapeuticClassName, d.DrugSortOrder, d.Drug_Name
                   select d;
     
        bool hasDrugs = false;

        foreach (Client_App_Drug_List drug in drugs) ////change here to your entity set
        {
            if (drug.TherapeuticClassID != currentTheraID)
            {
                if (currentTheraID > 0)
                {
                    sb.Remove(sb.Length - 1, 1);
                    sb.Append("]");
                }
                sb.AppendFormat("{0}{1}:[", (currentTheraID > 0 ? "," : ""), drug.TherapeuticClassID);

                currentTheraID = drug.TherapeuticClassID;

                sb2.Append("{ID:");
                sb2.AppendFormat("{0},Name:\"{1}\"", drug.TherapeuticClassID, drug.TherapeuticClassName);
                sb2.Append("},");
            }

            sb.Append("{ID:");
            sb.AppendFormat("{0},Name:\"{1}\",Selected:{2}", drug.ID, drug.Drug_Name, drug.IsDrugSelected.ToString().ToLower());
            sb.Append("},");

            hasDrugs = true;
        }

        if (hasDrugs)
        {
            sb.Remove(sb.Length - 1, 1);
            sb.Append("]};");

            sb2.Remove(sb2.Length - 1, 1);
            sb2.Append("];");
        }
        else
        {
            sb.Append("};");
            sb2.Append("];");
        }

        sb2.Append(sb.ToString());

        return sb2.ToString();
       }
    }

    static IQueryable<Client_App_Drug_List> GetUserDrugList(int clientID)
    {
        using (PathfinderEntities pfe = new PathfinderEntities())
        {
            var q = from d in pfe.Client_App_Drug_ListSet
                    where
                    d.ClientID == clientID
                    orderby d.TherapeuticClassSortOrder, d.TherapeuticClassName, d.DrugSortOrder, d.Alternate_Drug_Name
                    select d;

            return q;
        }
    }

}
