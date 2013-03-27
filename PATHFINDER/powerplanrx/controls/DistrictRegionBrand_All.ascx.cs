using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class controls_DistrictRegionBrand_All : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string districtID = (Request.QueryString["dist"]).ToString();
        int segmentID = Convert.ToInt32((Request.QueryString["segment"]).ToString());
        int typeID = Convert.ToInt32((Request.QueryString["reporttype"]).ToString());
        
        if (districtID == "0")
        {
            if (typeID == 1)
            {
                grvRegion.Columns[2].Visible = true;
                grvRegion.Columns[3].Visible = true;
                grvRegion.Visible = true;
            }
            else if (typeID == 2)
            {
                grvRegion.Columns[4].Visible = true;
                grvRegion.Visible = true;
            } 
        }
        else
        {
           
            if (typeID == 1)
            {
                grvDistrict.Columns[1].Visible = true;
                grvDistrict.Columns[2].Visible = true;
                grvDistrict.Visible = true;
            }
            else if (typeID == 2)
            {                    
                grvDistrict.Columns[3].Visible = true;
                grvDistrict.Visible = true;
            }              
           
        }        

    }
}
