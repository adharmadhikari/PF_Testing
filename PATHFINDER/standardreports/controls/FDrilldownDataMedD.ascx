<%@ Control Language="C#" AutoEventWireup="true" CodeFile="FDrilldownDataMedD.ascx.cs" Inherits="standardreports_controls_FDrilldownDataMedD" %>

<%--<%@ Register src="~/standardreports/controls/FormularyUpdateDate.ascx" tagname="FormularyUpdateDate" tagprefix="pinso" %>
--%>
<%--<pinso:FormularyUpdateDate runat="server" ID="formularyUpdateDate" />--%>
   <div ID="BDHeader1" class="areaHeader" runat="server">  
        <div id="FDDHeaderMedD" class="tileContainerHeader" style="margin-top: 2px !important;">
                    <div class="title"><img src="content/images/arwDwnW.gif" id="arrowR" /><asp:Literal runat="server" ID="Literal4" Text='  Medicare Part-D' /></div>
                    <div class="pagination" style="float:right"></div>
                    <div class="clearAll"></div>
        </div>
    </div> 
    <telerik:RadGrid SkinID="radTable" runat="server" ID="gridFMedD" AllowSorting="true"   
        PageSize="10" AllowPaging="true" AllowFilteringByColumn="false" EnableEmbeddedSkins="false"> 
        <MasterTableView AutoGenerateColumns="False" ClientDataKeyNames="Plan_Name" PageSize="10" 
         AllowMultiColumnSorting="true" HeaderStyle-Wrap="true"  >
            <Columns>
                 <telerik:GridBoundColumn DataField="Geography_Name" HeaderText="Geography" HeaderStyle-Width="7%" 
                           SortExpression="Geography_Name"    UniqueName="Geography_Name" DataType="System.String"  ItemStyle-CssClass="firstCol geogName"></telerik:GridBoundColumn>
                 <telerik:GridBoundColumn DataField="Plan_Name" HeaderText="Account Name" HeaderStyle-Width="8%"  ItemStyle-CssClass="planName"    
                            SortExpression="Plan_Name" ItemStyle-Wrap="true" HeaderStyle-Wrap="true"   UniqueName="Plan_Name" DataType="System.String" /> 
                <telerik:GridBoundColumn DataField="PartD_Lives" DataFormatString="{0:n0}" ItemStyle-CssClass="alignRight partDLives"
                    DataType="System.Int32" HeaderText="Pharmacy Lives" HeaderStyle-Width="7%" ItemStyle-Width="7%"
                    SortExpression="PartD_Lives" UniqueName="PartD_Lives"></telerik:GridBoundColumn>
                 <telerik:GridBoundColumn DataField="Product_Name" ItemStyle-CssClass="prodName" HeaderText="Product" UniqueName="Product_Name" DataType="System.String" HeaderStyle-Width="10%"  />
                 <telerik:GridBoundColumn DataField="Formulary_Name" ItemStyle-CssClass="formularyName" HeaderText="Formulary" UniqueName="Formulary_Name" DataType="System.String" HeaderStyle-Width="8%"/>
                 <telerik:GridBoundColumn DataField="Formulary_Lives" DataFormatString="{0:n0}" ItemStyle-CssClass="alignRight notmerged formularyLives"
                     DataType="System.Int32" HeaderText="Formulary Lives" HeaderStyle-Width="7%"
                     SortExpression="Formulary_Lives" UniqueName="Formulary_Lives"></telerik:GridBoundColumn>
                 
                     
                
                
                 <telerik:GridBoundColumn DataField="Drug_Name" HeaderText="Drug"  ItemStyle-CssClass="notmerged drugName" 
                       UniqueName="Drug_Name" DataType="System.String" HeaderStyle-Width="8%"></telerik:GridBoundColumn>
       
                 <telerik:GridBoundColumn DataField="Tier_Abbr" HeaderText="Tier"  ItemStyle-CssClass="notmerged tierName" HeaderStyle-Width="4%"
                         UniqueName="Tier_Abbr" DataType="System.String"></telerik:GridBoundColumn>
                
                <telerik:GridBoundColumn DataField="Copay_Range" HeaderText="Copay Range" HeaderStyle-Width="5%"
                           UniqueName="Copay_Range" DataType="System.String" ItemStyle-CssClass="alignRight notmerged copayRange"></telerik:GridBoundColumn>
                
                <%-- sl 5/10/2011 
              to Add 'Formulary Status' column 
              --%> 
                   <telerik:GridBoundColumn DataField="Formulary_Status_Name" HeaderText="Status" HeaderStyle-Width="5%" ItemStyle-CssClass="statusName" 
                          UniqueName="Formulary_Status_Name" ItemStyle-Wrap="true" DataType="System.String"></telerik:GridBoundColumn>
                
                <%-- ------------------- --%>
                
                <telerik:GridBoundColumn DataField="PA" HeaderText="PA"  ItemStyle-CssClass="notmerged paCol" HeaderStyle-Width="3%"
                         UniqueName="PA" DataType="System.String"></telerik:GridBoundColumn>
                
                <telerik:GridBoundColumn DataField="QL" HeaderText="QL"  ItemStyle-CssClass="notmerged qlCol" HeaderStyle-Width="3%"
                         UniqueName="QL" DataType="System.String"></telerik:GridBoundColumn>
                
                 <telerik:GridBoundColumn DataField="ST" HeaderText="ST"  ItemStyle-CssClass="notmerged stCol" HeaderStyle-Width="3%"
                          UniqueName="ST" DataType="System.String"></telerik:GridBoundColumn>
                          
              <%-- sl 5/10/2011 more info icon: similar to Today's Accounts Benefit Design 
              to Add 'Formulary Status' column and to display 'more info' popup from 'i' icon(4 params)
                'Comments' position: different based on Section
              --%> 
              
            
                  
              
              <telerik:GridBoundColumn DataField="Plan_ID" Visible="false"></telerik:GridBoundColumn>
              <telerik:GridBoundColumn DataField="Formulary_ID" Visible="false"></telerik:GridBoundColumn>
              <telerik:GridBoundColumn DataField="Drug_ID" Visible="false"></telerik:GridBoundColumn>
              <telerik:GridBoundColumn DataField="Segment_ID" Visible="false"></telerik:GridBoundColumn>
              <telerik:GridBoundColumn DataField="Formulary_Status_ID" Visible="false"></telerik:GridBoundColumn>
              <telerik:GridBoundColumn DataField="Formulary_Status_Name" Visible="false"></telerik:GridBoundColumn>
                
              <telerik:GridHyperLinkColumn DataNavigateUrlFormatString='javascript:OpenNotesViewer({0},{1},{2},{3},"comments",null,null,200,200,"ctl00_Tile3_fdrilldowndataMedD_gridFMedD");' DataNavigateUrlFields="Plan_ID,Drug_ID,Formulary_ID,Segment_ID" UniqueName="Comments" DataTextFormatString='{0}' DataTextField ="Comments" ItemStyle-CssClass="commentsCell" HeaderText="&nbsp;" HeaderStyle-Width="3%" ></telerik:GridHyperLinkColumn>               
             
             
             
            </Columns>
            <PagerStyle Visible="false" />
            
            <SortExpressions>
          
                <%--<telerik:GridSortExpression FieldName="Plan_Name" SortOrder="Ascending" />--%>
                <telerik:GridSortExpression FieldName="Geography_Name" SortOrder="Ascending"/>                
                <telerik:GridSortExpression FieldName="Product_Name" />
                <%--<telerik:GridSortExpression FieldName="Formulary_Name" />--%>
               <%-- <telerik:GridSortExpression FieldName="Covered_Lives" />
                <telerik:GridSortExpression FieldName="Commercial_Pharmacy_Lives" />--%>
                <telerik:GridSortExpression FieldName="Drug_Name" />            
            </SortExpressions>
        </MasterTableView>
   

       <ClientSettings >
            <DataBinding Location="~/standardreports/services/pathfinderdataservice.svc" DataService-TableName="ReportsFormularyDrilldownSet"/>               
            <Scrolling AllowScroll="True" UseStaticHeaders="True" />
            <Selecting AllowRowSelect="false" />
        </ClientSettings> 
    </telerik:RadGrid>


   <pinso:RadGridWrapper runat="server" ID="radGridWrapper" Target="gridFMedD" PagingSelector="#FDDHeaderMedD .pagination" RequiresFilter="true" AutoUpdate="true"  LoadingText=""/>
