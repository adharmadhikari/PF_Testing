using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Pinsonault.Application.Millennium;
using System.Data;

public partial class custom_millennium_businessplanning_controls_filteraccountmanagers : System.Web.UI.UserControl
{
    public custom_millennium_businessplanning_controls_filteraccountmanagers()
    {
        ContainerID = "moduleOptionsContainer";
    }
    public string ContainerID { get; set; }

    public bool IncludeAll { get; set; }

    protected override void OnInit(EventArgs e)
    {
        //dsAcctMgr.ConnectionString = Pinsonault.Web.Session.ClientConnectionString;

        base.OnInit(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {

       string user_id = Session["UserID"].ToString();
       int userId = Int32.Parse(user_id);

            Pinsonault.Web.Support.RegisterComponentWithClientManager(Page, User_ID.ClientID, null, ContainerID);
            using (PathfinderMillenniumEntities context = new PathfinderMillenniumEntities())
            {
               //Check For Role_Id associated with the User_ID
                string userIdList = (from p in context.AccountManagerSet 
                                        where p.User_ID == userId
                                        select p.Role_ID).FirstOrDefault().ToString();

                if (Int32.Parse(userIdList) == 34)
                    {   //If Role_ID == 34 i.e. Home Office User
                        RadComboBoxItem itemAM = new RadComboBoxItem("--All Account Managers--");
                        User_ID.Items.Add(itemAM);
                        User_ID.DataSource = context.AccountManagerSet;
                        User_ID.DataBind();
                        
                    }
                    else
                    {
                        var uFullName = (from p in context.AccountManagerSet
                                             where p.User_ID == userId
                                             select p.FullName).ToList();
                        
                        if (uFullName.Count == 0)
                        {     //If User is not in the AccountMangerSet; then show Users Full name in the ComboBox
                              RadComboBoxItem itemAM = new RadComboBoxItem(Session["FirstName"].ToString()+" "+Session["LastName"].ToString());
                              itemAM.Value = userId.ToString();
                              User_ID.Items.Add(itemAM);
                              filterLabel.Visible = false;
                              User_ID.Style.Add("display", "none");
                        }
                        else
                        {       //Show User From AccountmangerSet into The ComboBox
                                string userFullName = uFullName.FirstOrDefault().ToString();
                                RadComboBoxItem itemAM1 = new RadComboBoxItem(userFullName);
                                itemAM1.Value = userId.ToString();
                                User_ID.Items.Add(itemAM1);
                                filterLabel.Visible = false;
                                User_ID.Style.Add("display", "none");
                            }
                    }
                
                
               
            }
    }

}
