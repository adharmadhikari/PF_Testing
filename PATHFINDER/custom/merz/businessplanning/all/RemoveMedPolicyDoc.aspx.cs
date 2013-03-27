using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PathfinderClientModel;
using Pinsonault.Web;
using System.IO;
using Pinsonault.Application.Merz; 

public partial class custom_merz_businessplanning_all_RemoveMedPolicyDoc :  PageBase
{
    protected override void OnInit(EventArgs e)
    {
        //dsMPDoc.ConnectionString = PathfinderApplication.Session.ClientConnectionString;
      
        titleText.Text = "Remove Selected Medical Policy Document";
        base.OnInit(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            RemoveMPDoc.Visible = true;
            this.Msglbl.Visible = false;
        }
    }

    //Called when "Yes" button is clicked.
    protected void Yesbtn_Click(object sender, EventArgs e)
    {
        String MP_Name = Page.Request.QueryString["MP_Name"].ToString() ;

        using (PathfinderMerzEntities context = new PathfinderMerzEntities())
        {
            ////Updates SBusinessPlanMedicalPolicyDocSet.Med_Policy_Status_ID field to 2(i.e. Deleted).
            if (RemoveMedicalPolicyDoc(System.Convert.ToInt32(Page.Request.QueryString["MP_ID"]), context))
            {
                MoveFileToDeletedFolder(Page.Request.QueryString["MP_ID"].ToString());
                
                //Confirmation message is displayed after Delete.
                RemoveMPDoc.Visible = false;
                this.Msglbl.Text = "<div><BR/>Selected medical policy document has been removed successfully.</div>";
                this.Msglbl.Visible = true;
            }
        }

        //Calls Javascript function RefreshMySSList() to refresh sell sheet grids.
        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "RefreshMedPolicyDocList", "RefreshMedPolicyDocList();", true);  
    }

    protected void MoveFileToDeletedFolder(String filename)
    {
        string folderPath = Support.GetClientFolder("bp_medical_policy");

        //TODO: need to create deleted folder in client folder and move this code in application support
        string strDeleteFolder = Path.Combine(folderPath, "deleted");

        var file = (from f in Directory.GetFiles(folderPath) where Path.GetFileNameWithoutExtension(f).Equals(filename) select f).FirstOrDefault();

        if (File.Exists(file))
            File.Move(file, file.Replace(folderPath, strDeleteFolder));
    }

    //To change the status of sell sheets from active to deactive hence removing it from the list.
    public static bool RemoveMedicalPolicyDoc(int MP_ID, PathfinderMerzEntities context)
    {
        BusinessPlanMedicalPolicyDoc mpdoc = context.BusinessPlanMedicalPolicyDocSet.FirstOrDefault(m => m.Medical_Policy_ID == MP_ID);
        if (mpdoc != null)
        {
            //Status = 2 for deleted.
            mpdoc.Med_Policy_Status_ID = 2;
            context.SaveChanges();
            return true;
        }
        return false;
    }

}
