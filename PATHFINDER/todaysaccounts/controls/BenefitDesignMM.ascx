<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BenefitDesignMM.ascx.cs" Inherits="controls_MMBenefitDesign" %>
  <div ID="BDHeader3" class="areaHeader" runat="server"><img src="content/images/arwDwnW.gif"   id="arrowR" /> Managed Medicaid</div>
             <telerik:RadGrid SkinID="radTable" runat="server" ID="gridMMBenefitDesg" AllowPaging="false" AllowFilteringByColumn="false" EnableEmbeddedSkins="false" Width="100%"
             DataSourceID="dsMMBenefitDesign" AllowSorting="true">        
                <MasterTableView autogeneratecolumns="false" ClientDataKeyNames="Plan_ID,Formulary_ID,Formulary_Name, Segment_ID" Width="100%" AllowSorting="true">
                   <Columns>
                   
                    <telerik:GridBoundColumn DataField="Formulary_Name" HeaderStyle-CssClass="rgHeader postback" HeaderText='<%$ Resources:Resource, Label_BD_FormularyName %>' UniqueName="Formulary_Name" ItemStyle-CssClass="firstCol" />
                    <telerik:GridBoundColumn DataField="Formulary_Lives" HeaderStyle-CssClass="rgHeader postback" HeaderText='<%$ Resources:Resource, Label_BD_Lives %>' UniqueName="Formulary_Lives" DataFormatString="{0:n0}" ItemStyle-CssClass="alignRight"  />
                    <telerik:GridBoundColumn DataField="Percentage_Lives" HeaderStyle-CssClass="rgHeader postback" HeaderText='<%$ Resources:Resource, Label_BD_PercentLives %>' UniqueName="Percentage_Lives" DataFormatString="{0:n0}%" ItemStyle-CssClass="alignRight" />
                    </Columns>
                   </MasterTableView>
                 <ClientSettings>
                     <Scrolling AllowScroll="False" UseStaticHeaders="False" />
                </ClientSettings>
           </telerik:RadGrid>
        
 <asp:EntityDataSource ID="dsMMBenefitDesign" runat="server" EntitySetName="BenefitDesignManagedMedicaidSet" ConnectionString="name=PathfinderEntities" DefaultContainerName="PathfinderEntities" 
        AutoGenerateWhereClause="false" Where="it.Plan_ID = @Plan_ID"  OrderBy="it.Formulary_Lives Desc" >
    <WhereParameters>       
        <asp:QueryStringParameter QueryStringField="plan_ID" Name="Plan_ID" Type="Int32" />
    </WhereParameters>
</asp:EntityDataSource>      
