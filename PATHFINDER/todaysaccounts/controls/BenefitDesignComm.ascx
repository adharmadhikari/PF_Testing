<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BenefitDesignComm.ascx.cs" Inherits="controls_CommBenefitDesign" %>
        <!--Getting selected plan's Segment_ID in hidden variable-->
       <%-- <asp:FormView runat="server" ID="SegIDCommCovLives" DataSourceID="dsPlanListVW" CellPadding="0" CellSpacing="0" Width="100%" Visible="false">
            <ItemTemplate>
                <asp:HiddenField ID="Seg_ID" Value='<%# Eval("Segment_ID")%>' runat="server" /> 
            </ItemTemplate> 
        </asp:FormView> --%>
        <div ID="BDHeader1" class="areaHeader" runat="server"><img src="content/images/arwDwnW.gif"   id="arrowR" />Commercial</div>
             <telerik:RadGrid SkinID="radTable" runat="server" ID="gridCommBenefitDesg" AllowPaging="false" AllowFilteringByColumn="false" EnableEmbeddedSkins="false" Width="100%"
             DataSourceID="dsCommBenefitDesign" AllowSorting="true">        
                <MasterTableView autogeneratecolumns="false" ClientDataKeyNames="Plan_ID,Formulary_ID,Formulary_Name, Segment_ID" Width="100%" AllowSorting="true">
                   <Columns>
                   <%-- <telerik:GridHyperLinkColumn DataNavigateUrlFormatString='javascript:BenefitDesgFormularyName_Click({0},{1});' DataNavigateUrlFields="Plan_ID,Formulary_ID" UniqueName="Formulary_Name" HeaderText="Formulary Name"  DataTextField="Formulary_Name" ItemStyle-CssClass="firstCol" ></telerik:GridHyperLinkColumn> --%>
                    <telerik:GridBoundColumn DataField="Formulary_Name" HeaderStyle-CssClass="rgHeader postback" HeaderText='<%$ Resources:Resource, Label_BD_FormularyName %>' UniqueName="Formulary_Name" ItemStyle-CssClass="firstCol" />
                    <telerik:GridBoundColumn DataField="Formulary_Lives" HeaderStyle-CssClass="rgHeader postback" HeaderText='<%$ Resources:Resource, Label_BD_Lives %>' UniqueName="Formulary_Lives" DataFormatString="{0:n0}" ItemStyle-CssClass="alignRight"  />
                    <telerik:GridBoundColumn DataField="Percentage_Lives" HeaderStyle-CssClass="rgHeader postback" HeaderText='<%$ Resources:Resource, Label_BD_PercentLives %>' UniqueName="Percentage_Lives" DataFormatString="{0:n0}%" ItemStyle-CssClass="alignRight" />
                    </Columns>
                   </MasterTableView>
                 <ClientSettings>
                     <Scrolling AllowScroll="False" UseStaticHeaders="False" />
                </ClientSettings>
           </telerik:RadGrid>
           <%--<div class="clDate" runat="server" id="formularyDate">
                <%= Pinsonault.Web.Support.GetDataUpdateDateByKey("Commercial Formulary", Resources.Resource.Label_Section_Last_Updated) %>
           </div>--%>
 <asp:EntityDataSource ID="dsCommBenefitDesign" runat="server" EntitySetName="BenefitDesignCommercialSet" ConnectionString="name=PathfinderEntities" DefaultContainerName="PathfinderEntities" 
        AutoGenerateWhereClause="false" Where="it.Plan_ID = @Plan_ID"  OrderBy="it.Formulary_Lives Desc" >
    <WhereParameters>       
        <asp:QueryStringParameter QueryStringField="plan_ID" Name="Plan_ID" Type="Int32" />
    </WhereParameters>
</asp:EntityDataSource>      

 <%--<asp:EntityDataSource ID="dsPlanListVW" runat="server" EntitySetName="PlanInfoListViewSet" DefaultContainerName="PathfinderClientEntities" 
        AutoGenerateWhereClause="true">
        <WhereParameters>       
            <asp:QueryStringParameter QueryStringField="plan_ID" Name="Plan_ID" Type="Int32" />
        </WhereParameters>
</asp:EntityDataSource>--%>
