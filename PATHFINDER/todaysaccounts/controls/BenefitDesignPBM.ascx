<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BenefitDesignPBM.ascx.cs" Inherits="controls_PBMBenefitDesign" %>
  <div ID="BDHeader1" class="areaHeader" runat="server">PBM</div>
             <telerik:RadGrid SkinID="radTable" runat="server" ID="gridPBMBenefitDesg" AllowPaging="false" AllowFilteringByColumn="false" EnableEmbeddedSkins="false" Width="100%"
             DataSourceID="dsPBMBenefitDesign">        
                <MasterTableView autogeneratecolumns="false" ClientDataKeyNames="Plan_ID,Formulary_ID,Formulary_Name, Segment_ID">
                   <Columns>
                    <telerik:GridBoundColumn DataField="Formulary_Name" HeaderStyle-CssClass="rgHeader postback" HeaderStyle-Width="100px" HeaderText='<%$ Resources:Resource, Label_BD_FormularyName %>' UniqueName="Formulary_Name" ItemStyle-CssClass="firstCol" />
                    <telerik:GridBoundColumn DataField="Formulary_Lives" HeaderStyle-CssClass="rgHeader postback" HeaderStyle-Width="100px" HeaderText='<%$ Resources:Resource, Label_BD_Lives %>' UniqueName="Formulary_Lives" DataFormatString="{0:n0}"/>
                    <telerik:GridBoundColumn DataField="Percentage_Lives" HeaderStyle-CssClass="rgHeader postback" HeaderStyle-Width="100px" HeaderText='<%$ Resources:Resource, Label_BD_PercentLives %>' UniqueName="Percentage_Lives" DataFormatString="{0:n0}%"/>
                    </Columns>                
                </MasterTableView>
           </telerik:RadGrid>
    
 <asp:EntityDataSource ID="dsPBMBenefitDesign" runat="server" EntitySetName="BenefitDesignSet" ConnectionString="name=PathfinderEntities" DefaultContainerName="PathfinderEntities" 
        AutoGenerateWhereClause="false" Where="it.Formulary_Lives <> 0 and it.Segment_ID = 4 and it.Plan_ID = @Plan_ID" OrderBy="it.Formulary_Lives Desc">
    <WhereParameters>       
        <asp:QueryStringParameter QueryStringField="plan_ID" Name="Plan_ID" Type="Int32" />
    </WhereParameters>
</asp:EntityDataSource>  