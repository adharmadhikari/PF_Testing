using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using PathfinderClientModel;

public partial class custom_controls_SellSheetSteps : System.Web.UI.UserControl
{
    public string PageModule { get; protected set; }
    public int RequestedStep { get; set; }
    public int CurrentStep { get; set; }
    public string StepText { get { return lblStepTitle.Text; } }
    public string StepShortName { get; protected set; }
    public string CurrentStepText { get; protected set; }

    public bool InvalidStepAsException { get; set; }
    public bool HasError { get; protected set; }

    protected override void OnInit(EventArgs e)
    {
        InvalidStepAsException = true;//default to true

        PageModule = Path.GetFileNameWithoutExtension(Request.Url.AbsolutePath);

        dsSteps.ConnectionString = Pinsonault.Web.Session.ClientConnectionString;

        base.OnInit(e);
    }

    protected override void OnLoad(EventArgs e)
    {
        string sid = Request.QueryString["Sell_Sheet_ID"];
        int id;
        if ( !string.IsNullOrEmpty(sid) && Int32.TryParse(sid, out id) )
        {
            SellSheetStep requestedStep = null;
            int currentStep = 0;
            int? templateID;

            using ( PathfinderClientModel.PathfinderClientEntities context = new PathfinderClientModel.PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString) )
            {

                //Get sellsheets current step
                var vcurrentStep = (from ss in context.SellSheetSet
                               join step in context.SellSheetStepSet on ss.Current_Step equals step.Step_Key
                               where ss.Sell_Sheet_ID == id
                               select new { ss.Template_ID, step.Step_Order, step.Step_Name }).FirstOrDefault();
                
                currentStep = vcurrentStep.Step_Order;
                templateID = vcurrentStep.Template_ID;
                CurrentStepText = vcurrentStep.Step_Name;

                CurrentStep = currentStep;

                requestedStep = (from step in context.SellSheetStepSet
                                 where step.Step_Key == PageModule
                                 select step).FirstOrDefault();

                RequestedStep = requestedStep.Step_Order;

                litStepNum.Text = RequestedStep.ToString();

                //templateID = (from ss in context.SellSheetSet
                //              where ss.Sell_Sheet_ID == id
                //              select ss.Template_ID).FirstOrDefault();
            }

            if ( templateID.HasValue )
                litTemplateID.Text = templateID.Value.ToString();

            if ( requestedStep != null )
            {
                if ( currentStep < requestedStep.Step_Order )
                    HandleError("Step is not valid");

                lblStepDescription.Text = requestedStep.Step_Description;
                //lblStepTitle.Text = string.Format(Resources.Resource.Label_SellSheet_Step_Title, requestedStep.Step_Order, requestedStep.Step_Name);
                lblStepTitle.Text = requestedStep.Step_Name;
                litStepTip.Text = requestedStep.Step_Tip;
                StepShortName = requestedStep.Step_Short_Name;
                //sellSheetSteps.Style.Add(HtmlTextWriterStyle.BackgroundImage, string.Format("url(custom/{0}/sellsheets/images/bar{1}.jpg)", Pinsonault.Web.Session.ClientKey, requestedStep.Step_Order));
                string urlReplace = string.Format("custom/{0}/sellsheets/all/{1}.aspx", Pinsonault.Web.Session.ClientKey, requestedStep.Step_Key);
                string strUrl = Request.Url.AbsoluteUri.Replace(urlReplace, string.Format("custom/{0}/sellsheets/images/bar{1}.jpg", Pinsonault.Web.Session.ClientKey, requestedStep.Step_Order));
                sellSheetSteps.Style.Add(HtmlTextWriterStyle.BackgroundImage, string.Format("url({0})", strUrl)); 
            }
            else
                HandleError("Invalid step");

            base.OnLoad(e);
        }
        else
            throw new HttpException(500, "Missing or invalid sell sheet id");
    }


    public string GetClassName(string CurrentModule)
    {
        if ( string.Compare(CurrentModule, PageModule, true) == 0 )
            return "selectedStep";
        else
            return "";
    }

    void HandleError(string errorMessage)
    {
        if ( !InvalidStepAsException )
            throw new HttpException(500, errorMessage);
        else
        {
            HasError = true;
        }
    }
}
