<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BenefitDesignVA.ascx.cs" Inherits="todaysaccounts_controls_BenefitDesignVA" %>
<div ID="BDHeader1" class="areaHeader" runat="server">VA</div>
     <telerik:RadGrid SkinID="radTable" runat="server" ID="gridVABenefitDesg" AllowPaging="false" AllowFilteringByColumn="false" EnableEmbeddedSkins="false" Width="100%"
     DataSourceID="dsVABenefitDesign">        
        <MasterTableView autogeneratecolumns="false" ClientDataKeyNames="Plan_ID,Formulary_ID,Formulary_Name, Segment_ID">
           <Columns>
            <telerik:GridBoundColumn DataField="Formulary_Name" HeaderStyle-CssClass="rgHeader postback" HeaderStyle-Width="100px" HeaderText='<%$ Resources:Resource, Label_BD_FormularyName %>' UniqueName="Formulary_Name" ItemStyle-CssClass="firstCol"  />
            <telerik:GridBoundColumn DataField="Formulary_Lives" HeaderStyle-CssClass="rgHeader postback" HeaderStyle-Width="100px" HeaderText='<%$ Resources:Resource, Label_BD_Lives %>' UniqueName="Formulary_Lives" DataFormatString="{0:n0}" ItemStyle-CssClass="alignRight" />
            <telerik:GridBoundColumn DataField="Percentage_Lives" HeaderStyle-CssClass="rgHeader postback" HeaderStyle-Width="100px" HeaderText='<%$ Resources:Resource, Label_BD_PercentLives %>' UniqueName="Percentage_Lives" DataFormatString="{0:n0}%" ItemStyle-CssClass="alignRight" />
            </Columns>                
        </MasterTableView>
   </telerik:RadGrid>
  <%-- <div class="clDate" runat="server" id="formularyDate">
        <%= Pinsonault.Web.Support.GetDataUpdateDateByKey("VA Formulary", Resources.Resource.Label_Section_Last_Updated)%>
   </div>--%>
 <asp:EntityDataSource ID="dsVABenefitDesign" runat="server" EntitySetName="BenefitDesignSet" ConnectionString="name=PathfinderEntities" DefaultContainerName="PathfinderEntities" 
        AutoGenerateWhereClause="false" Where="it.Formulary_Lives <> 0 and it.Plan_ID = @Plan_ID">
    <WhereParameters>       
        <asp:QueryStringParameter QueryStringField="plan_ID" Name="Plan_ID" Type="Int32" />
    </WhereParameters>
</asp:EntityDataSource> 
