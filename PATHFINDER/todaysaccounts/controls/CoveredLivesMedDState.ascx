<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CoveredLivesMedDState.ascx.cs" Inherits="controls_CoveredLivesMedDState" %>
<div id="lfView">
   <telerik:RadGrid SkinID="radTable" runat="server" ID="gridProductCoveredLives" AllowSorting="true"  AllowPaging="false" AllowFilteringByColumn="false" EnableEmbeddedSkins="false"
     DataSourceID="dsproductCoveredLives">        
        <MasterTableView autogeneratecolumns="false" ClientDataKeyNames="Prod_ID">
           <Columns>
            <telerik:GridBoundColumn DataField="Prod_Type_Name" UniqueName="Prod_Type_Name" HeaderText="Lives Distribution" HeaderStyle-Width="50%" ItemStyle-CssClass="firstCol" />
            <telerik:GridBoundColumn DataField="TotalProdTypePharmacyLives" HeaderText="" UniqueName="TotalProdTypePharmacyLives" HeaderStyle-Width="50%" DataFormatString="{0:n0}"  ItemStyle-CssClass="alignRight" />
            </Columns>  
            <SortExpressions>
                <telerik:GridSortExpression FieldName="Prod_Type_ID" SortOrder="Ascending" />   
            </SortExpressions>               
        </MasterTableView>           
    </telerik:RadGrid>
    
    <asp:FormView runat="server" ID="FormView1" DataSourceID="dsPlanInfo" CellPadding="0" CellSpacing="0" Width="100%">
        <%--<ItemTemplate>
            <table  class="genTable" cellpadding="0" cellspacing="0" border="0" >
                <tr>
                    <td class="sectionDisclaimer"><%# Eval("Last_Update_DT", Resources.Resource.Label_Section_Last_Updated)%></td>
                </tr>                
            </table>
        </ItemTemplate> --%>
    </asp:FormView>    
</div>   
    
<asp:SqlDataSource ID="dsproductCoveredLives" runat="server" ConnectionString="<%$ ConnectionStrings:Pathfinder %>"
    SelectCommand="usp_MedD_ProductCoveredLives" SelectCommandType="StoredProcedure">      
    <SelectParameters>
        <asp:QueryStringParameter QueryStringField="Prod_ID" Name="Prod_ID" Type="Int32" /> 
        <asp:QueryStringParameter QueryStringField="Prod_State" Name="Prod_State" Type="string" />            
    </SelectParameters>
</asp:SqlDataSource> 

<asp:EntityDataSource ID="dsPlanInfo" runat="server" EntitySetName="PlanInfoSet" ConnectionString="name=PathfinderEntities" DefaultContainerName="PathfinderEntities" 
    AutoGenerateWhereClause="true">
    <WhereParameters>       
        <asp:QueryStringParameter QueryStringField="plan_ID" Name="Plan_ID" Type="Int32" />
    </WhereParameters>
</asp:EntityDataSource>