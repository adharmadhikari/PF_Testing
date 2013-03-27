using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class usercontent_BenefitDesignDefinitions : PageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        int[] image_a = {1,12,20,4,6,17};
        

        string section_id = Request.QueryString["section_id"];

        if (Array.IndexOf(image_a, Convert.ToInt32(section_id)) != -1)
        {
            bd_image.Src = "app_themes/pathfinder/images/Benefit_Design_Legend.jpg";
        }
        else
        {
            bd_image.Src = "app_themes/pathfinder/images/Benefit_Design_Legend.jpg";
        }
        
    }
}
