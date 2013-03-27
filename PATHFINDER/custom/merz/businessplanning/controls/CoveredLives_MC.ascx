<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CoveredLives_MC.ascx.cs" Inherits="custom_merz_businessplanning_controls_CoveredLives_MC" %>
<div id="lfView">
     <telerik:RadGrid SkinID="radTable" runat="server" ID="gridCoveredLives" AllowSorting="true"  AllowPaging="false" AllowFilteringByColumn="false" EnableEmbeddedSkins="false"
     DataSourceID="dsCoveredLives">        
        <MasterTableView autogeneratecolumns="false" ClientDataKeyNames="Plan_ID" AllowSorting="false">
           <Columns>
            <telerik:GridBoundColumn DataField="Covered_Lives_Type_Name" UniqueName="Covered_Lives_Type_Name" HeaderText="Lives Distribution" HeaderStyle-Width="65%" ItemStyle-CssClass="firstCol" />
            <telerik:GridBoundColumn DataField="Covered_Lives" HeaderText="" UniqueName="Covered_Lives" HeaderStyle-Width="35%" DataFormatString="{0:n0}"  ItemStyle-CssClass="alignRight" />
            </Columns>  
            <SortExpressions>
                <telerik:GridSortExpression FieldName="Covered_Lives_Order" SortOrder="Ascending" />   
            </SortExpressions>               
        </MasterTableView>           
    </telerik:RadGrid>
    <br />
    <div id="MC_IputArea"  visible="true" runat="server" class="MedicareTile">
        <div id="CLReadOnly" runat="server" visible="false">Medicare PartB Enrollment: <asp:Label ID="MedCarrierABlbl" Text="" runat="server" ></asp:Label> </div>
        <div id="CLEdit" runat="server" visible="false" >
                 Medicare PartB Enrollment: <asp:TextBox ID="MedCarrierABtxt" Text="" runat="server" MaxLength="9"/>
                 <pinso:ClientValidator ID="cvMedPartB" Target="MedCarrierABtxt" DataType="Integer" Required="false"  runat="server" Text ="Please enter valid Medicare PartB Enrollment."/> 
        </div>
    </div>
    
     <telerik:RadGrid SkinID="radTable" runat="server" ID="gridProductCoveredLives" AllowSorting="false"  AllowPaging="false" AllowFilteringByColumn="false" EnableEmbeddedSkins="false" DataSourceID="dsproductCoveredLives">        
        <MasterTableView autogeneratecolumns="false" ClientDataKeyNames="Prod_ID">
           <Columns>
            <telerik:GridBoundColumn DataField="Prod_Type_Name" UniqueName="Prod_Type_Name" HeaderText="Product Lives Distribution" HeaderStyle-Width="50%" ItemStyle-CssClass="firstCol" />
            <telerik:GridBoundColumn DataField="TotalProdTypePharmacyLives" HeaderText="" UniqueName="TotalProdTypePharmacyLives" HeaderStyle-Width="50%" DataFormatString="{0:n0}"  ItemStyle-CssClass="alignRight" />
            </Columns>  
            <SortExpressions>
                <telerik:GridSortExpression FieldName="Prod_Type_ID" SortOrder="Ascending" />   
            </SortExpressions>               
        </MasterTableView>           
    </telerik:RadGrid>   
</div>   
    <asp:EntityDataSource ID="dsCoveredLives" runat="server" EntitySetName="V_CoveredLivesSet" ConnectionString="name=PathfinderEntities" DefaultContainerName="PathfinderEntities" 
         AutoGenerateOrderByClause="false" Select="" 
        Where="it.Plan_ID =@Plan_ID" > 
        <WhereParameters>       
            <asp:QueryStringParameter QueryStringField="plan_ID" Name="Plan_ID" Type="Int32" />
        </WhereParameters>
    </asp:EntityDataSource>

<%--Where="(it.Covered_Lives_Order=9 or it.Covered_Lives_Order=10 or it.Covered_Lives_Order=11) and it.Plan_ID =@Plan_ID" > --%>
    
    <asp:SqlDataSource ID="dsproductCoveredLives" runat="server" ConnectionString="<%$ ConnectionStrings:Pathfinder %>" 
        SelectCommand="usp_MedD_ProductCoveredLives" SelectCommandType="StoredProcedure">      
        <SelectParameters>
            <asp:QueryStringParameter QueryStringField="Prod_ID" Name="Prod_ID" Type="Int32" /> 
            <asp:QueryStringParameter QueryStringField="Plan_State" Name="Prod_State" Type="string" />            
        </SelectParameters>
    </asp:SqlDataSource> 
    
    <asp:EntityDataSource ID="dsPlanInfo" runat="server" EntitySetName="PlanInfoSet" ConnectionString="name=PathfinderEntities" DefaultContainerName="PathfinderEntities" 
        AutoGenerateWhereClause="true">
        <WhereParameters>       
            <asp:QueryStringParameter QueryStringField="plan_ID" Name="Plan_ID" Type="Int32" />
        </WhereParameters>
    </asp:EntityDataSource>