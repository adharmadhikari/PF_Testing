using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PathfinderClientModel;

public partial class custom_dey_sellsheets_SellSheetCreatePreview : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Int32 SheetID = Convert.ToInt32(Request.QueryString["Sell_Sheet_ID"]);
        String strTemplateName = "" ;
        Boolean bCopay = true;

        using (PathfinderClientModel.PathfinderClientEntities clientContext = new PathfinderClientModel.PathfinderClientEntities(Pinsonault.Web.Session.ClientConnectionString))
        {
            //Select the Copay and TemplateName fields
            SellSheetMast ssMast = null;
            ssMast = (from d in clientContext.SellSheetMastSet
                      where d.Sell_Sheet_ID == SheetID
                      select d).First();

            if (!String.IsNullOrEmpty(ssMast.Template_Name))
                strTemplateName = ssMast.Template_Name.ToString();
            bCopay = ssMast.Use_Copay.Value; 
           
            if (ssMast.Template_ID == 1) //Hide Med-D
                SSPreview1MedD.Visible = false;

            if (ssMast.Template_ID == 2) //Hide Commercial
                SSPreview1.Visible = false;

            if (bCopay == true)
            {
                //Set Header colspan to 2
                SSPreview1.HeaderColspan = 2;
                SSPreview1MedD.HeaderColspan = 2;
            }
            else
            {
                //Set Header colspan 1
                SSPreview1.HeaderColspan = 1;
                SSPreview1MedD.HeaderColspan = 1;
            }
            
        }

      

        if (!String.IsNullOrEmpty(strTemplateName))
        {
            string strServerName = Request.ServerVariables["LOCAL_ADDR"];
            string strPort = Request.ServerVariables["SERVER_PORT"];
            
            //string strUrl = string.Format("http://{0}:{1}{2}"
            //                            , strServerName
            //                            , strPort
            //                            , Request.Url.AbsolutePath.Replace("custom/pinso/sellsheets/all/SellSheetCreatePreview.aspx", "content/images/" + strTemplateName.ToString ())
            //                            );
            string urlReplace = string.Format("custom/{0}/sellsheets/all/SellSheetCreatePreview.aspx", Pinsonault.Web.Session.ClientKey);
            string strUrl = Request.Url.AbsoluteUri.Replace(urlReplace, string.Format("content/images/{0}", strTemplateName.ToString()));
            //string templateName = Server.MapPath(string.Format("../templates/{0}", strTemplateName));
            //System.Drawing.Image objImage = System.Drawing.Image.FromFile(templateName);
            //int width = objImage.Width;
            //int height = objImage.Height;  


            if (strTemplateName.IndexOf("_portrait") > 0)
            {
                SSPreviewMain.Style.Add("height", "1055px");
                SSPreviewMain.Style.Add("width", "820px");
            }
            else
            {
                SSPreviewMain.Style.Add("height", "815px");
                SSPreviewMain.Style.Add("width", "1102px");
            }
        }
    }
}
