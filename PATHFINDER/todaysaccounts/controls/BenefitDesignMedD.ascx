<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BenefitDesignMedD.ascx.cs" Inherits="controls_MedDBenefitDesign" %>
       
        <div ID="BDHeader2" class="areaHeader" runat="server"><img src="content/images/arwDwnW.gif"   id="arrowR" />Medicare Part D</div>
             <telerik:RadGrid SkinID="radTable" runat="server" ID="gridMedDBenefitDesg" AllowPaging="false" 
             AllowFilteringByColumn="false" EnableEmbeddedSkins="false" Width="100%"
             DataSourceID="dsMedDBenefitDesign" AllowSorting="false">        
                <MasterTableView autogeneratecolumns="false" ClientDataKeyNames="Plan_ID,Formulary_ID, Prod_ID,Prod_Name,Segment_ID" Width="100%">
                   <Columns>
                    <telerik:GridBoundColumn DataField="Prod_Name" HeaderStyle-CssClass="rgHeader postback" HeaderText="Plan Product" UniqueName="Prod_Name" ItemStyle-CssClass="firstCol"/>
                    <telerik:GridBoundColumn DataField="Prod_Type_Name" HeaderStyle-CssClass="rgHeader postback" HeaderText="Type" UniqueName="Prod_Type_Name" ItemStyle-Width="40px"  />
                    <telerik:GridBoundColumn DataField="Formulary_Name" HeaderStyle-CssClass="rgHeader postback" HeaderText='<%$ Resources:Resource, Label_BD_FormularyName %>' UniqueName="Formulary_Name" />
                    <telerik:GridBoundColumn DataField="Formulary_Lives" HeaderStyle-CssClass="rgHeader postback" HeaderText='<%$ Resources:Resource, Label_BD_Lives %>' UniqueName="Formulary_Lives" DataFormatString="{0:n0}" ItemStyle-CssClass="alignRight"  />
                    <telerik:GridBoundColumn DataField="Percentage_Lives" HeaderStyle-CssClass="rgHeader postback" HeaderText='<%$ Resources:Resource, Label_BD_PercentLives %>' UniqueName="Percentage_Lives" DataFormatString="{0:n0}%" ItemStyle-CssClass="alignRight" />
                    </Columns> 
                </MasterTableView>
                <ClientSettings>
                    <Scrolling AllowScroll="False" UseStaticHeaders="False" />
                </ClientSettings>
           </telerik:RadGrid>
           <%-- <div class="clDate" runat="server" id="formularyDate">
                <%= Pinsonault.Web.Support.GetDataUpdateDateByKey("Part-D Formulary", Resources.Resource.Label_Section_Last_Updated)%>
           </div>--%>
    
 <asp:EntityDataSource ID="dsMedDBenefitDesign" runat="server" EntitySetName="BenefitDesignMedDSet" 
    ConnectionString="name=PathfinderEntities" DefaultContainerName="PathfinderEntities" 
    AutoGenerateWhereClause="false" Where="it.Plan_ID = @Plan_ID"   OrderBy="it.Segment_ID, it.Formulary_Lives Desc, it.Prod_Name, it.Formulary_Name asc" >
    <WhereParameters>       
        <asp:QueryStringParameter QueryStringField="plan_ID" Name="Plan_ID" Type="Int32" />
    </WhereParameters>        
</asp:EntityDataSource>  

<asp:EntityDataSource ID="dsBenefitDesignMedDState" runat="server" EntitySetName="BenefitDesignMedDSet" 
        ConnectionString="name=PathfinderEntities" DefaultContainerName="PathfinderEntities" 
        AutoGenerateWhereClause="false" Where="it.Plan_ID = @Plan_ID AND it.Prod_ID = @Prod_ID"   
        OrderBy="it.Prod_Name, it.Formulary_Name asc, it.Formulary_Lives Desc" >
    <WhereParameters>
        <asp:QueryStringParameter QueryStringField="Plan_ID" Name="Plan_ID" Type="Int32" />    
        <asp:QueryStringParameter QueryStringField="Prod_ID" Name="Prod_ID" Type="Int32" />      
    </WhereParameters>        
</asp:EntityDataSource>     

