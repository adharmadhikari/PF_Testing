using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Pinsonault.Web.Data ;
using System.Data;
using Utilities = Pinsonault.Web.Utilities;
using Pinsonault.Application.Merz; 
using Pinsonault.Data;

public partial class custom_merz_businessplanning_all_OpenDocDetails : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        using (PathfinderMerzEntities context = new PathfinderMerzEntities())
        {
            int iMedical_Policy_ID = Convert.ToInt32(System.Web.HttpContext.Current.Request.QueryString["Medical_Policy_ID"]);
            string strMain = "";
            //get details from the database

            BusinessPlanMedicalPolicyDoc mp = (from d in context.BusinessPlanMedicalPolicyDocSet 
                                               where d.Medical_Policy_ID == iMedical_Policy_ID 
                                               select d).First();
            String sMedical_Policy_Name = mp.Medical_Policy_Name.ToString();
            String sUpload_BY = mp.Upload_BY.ToString();
            String sUpload_DT = mp.Upload_DT.ToString();
             
            //Load BusinessPlanDocumentTypes
            mp.BusinessPlanDocumentTypesReference.Load() ;
            BusinessPlanDocumentTypes dt = mp.BusinessPlanDocumentTypes;

            Int32 iDocument_Type_ID = dt.Document_Type_ID;
            string sDocument_Type_Name = dt.Document_Type_Name;

            this.titleText.Text = "Document Details";

            strMain = string.Concat(strMain, "<b>File Name:</b> " + sMedical_Policy_Name + "<br/>");
            strMain = string.Concat(strMain, "<b>Document Type: </b>" + sDocument_Type_Name + "<br/>");
            strMain = string.Concat(strMain, "<b>Uploaded By:</b> " + sUpload_BY + "<br/>");
            strMain = string.Concat(strMain, "<b>Uploaded Date:</b> " + sUpload_DT + "<br/>");
            this.notesText.Text = strMain;

        }
    }
}
