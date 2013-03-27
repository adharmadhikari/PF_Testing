<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PlanInfo.ascx.cs" Inherits="controls_planinfo"
    EnableTheming="true" %>
<div class="addplan" id="addplanOpt">
    <a id="Add" href="javascript:OpenPlanInfo('AddPlan','')">Add</a> <a id="Delete" href="javascript:OpenPlanInfo('DelPlan','')">
        | Delete</a>
</div>
<div class="clearAll">
</div>
<telerik:RadGrid SkinID="radTable" runat="server" ID="gridPlanInfo" AllowSorting="true"
    PageSize="50" AllowPaging="true" AllowFilteringByColumn="true" EnableEmbeddedSkins="false">
    <MasterTableView AllowSorting="true" AutoGenerateColumns="False" ClientDataKeyNames="Plan_ID,Section_ID,VISN"
        PageSize="50" Width="100%">
        <Columns>
            <%-- DataFormatString="{0 }" will fix issue for A&P that only shows A if attribute is not present.  Don't know why because AT&T does not have issue and others that have '&' in name.  Adding space or any other chars seem to fix issue for A&P.--%>
            <telerik:GridBoundColumn DataField="Plan_Name" DataFormatString="{0} " HeaderText='<%$ Resources:Resource, Label_Plan_Name %>'
                SortExpression="Plan_Name" UniqueName="Plan_Name" DataType="System.String" ItemStyle-CssClass="firstCol planName">
                <FilterTemplate>
                    <div class="searchTextBox">
                        <input class="textBox" type="text" onkeyup="clientManager.setPlanInfoGridTimeout(this, {dataField:'Plan_Name', filterType:'Contains'})" /></div>
                </FilterTemplate>
            </telerik:GridBoundColumn>
            <%--<telerik:GridBoundColumn DataField="Prod_Name" DataFormatString="{0} " HeaderText='<%$ Resources:Resource, Label_Plan_Product %>'
                SortExpression="Prod_Name" UniqueName="Prod_Name" DataType="System.String" ItemStyle-CssClass="prodName">
                <FilterTemplate>
                    <div class="searchTextBox"><input class="textBox" type="text" onkeyup="clientManager.setPlanInfoGridTimeout(this, {dataField:'Prod_Name', filterType:'Contains'})" /></div>                    
                </FilterTemplate>
            </telerik:GridBoundColumn> --%>
             <telerik:GridBoundColumn DataField="Plan_City" ItemStyle-CssClass="notmerged planCity" HeaderText="City"
                SortExpression="Plan_City" UniqueName="Plan_City">
                 <FilterTemplate>
                    <div class="searchTextBox">
                        <input class="textBox" type="text" onkeyup="clientManager.setPlanInfoGridTimeout(this, {dataField:'Plan_City', filterType:'Contains'})" />
                    </div>
                </FilterTemplate>
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="Plan_State" ItemStyle-CssClass="planState" HeaderText='<%$ Resources:Resource, Label_State %>'
                SortExpression="Plan_State" UniqueName="Plan_State">
                <FilterTemplate>
                    <telerik:RadComboBox EnableEmbeddedSkins="false" SkinID="planInfoCombo" Skin="pathfinder"
                        runat="server" ID="rdlStates" DataSourceID="dsStates" Width="90%" DropDownWidth="120px"
                        DataTextField="Name" DataValueField="ID" AppendDataBoundItems="true" MaxHeight="200"
                        OnClientSelectedIndexChanged="onFilterPlansByState">
                        <Items>
                            <telerik:RadComboBoxItem Value="" Text="" />
                        </Items>
                    </telerik:RadComboBox>
                </FilterTemplate>
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="VISN" ItemStyle-CssClass="notmerged VISN" HeaderText='<%$ Resources:Resource, Label_VISN %>'
                SortExpression="VISN" UniqueName="VISN">
                <FilterTemplate>
                    <telerik:RadComboBox EnableEmbeddedSkins="false" SkinID="planInfoCombo" Skin="pathfinder"
                        runat="server" ID="rdlVISN" Width="90%" MaxHeight="200" OnClientSelectedIndexChanged="onFilterPlansByVISN">
                        <Items>
                            <telerik:RadComboBoxItem Value="" Text="" />
                            <telerik:RadComboBoxItem Value="1" Text="1" />
                            <telerik:RadComboBoxItem Value="2" Text="2" />
                            <telerik:RadComboBoxItem Value="3" Text="3" />
                            <telerik:RadComboBoxItem Value="4" Text="4" />
                            <telerik:RadComboBoxItem Value="5" Text="5" />
                            <telerik:RadComboBoxItem Value="6" Text="6" />
                            <telerik:RadComboBoxItem Value="7" Text="7" />
                            <telerik:RadComboBoxItem Value="8" Text="8" />
                            <telerik:RadComboBoxItem Value="9" Text="9" />
                            <telerik:RadComboBoxItem Value="10" Text="10" />
                            <telerik:RadComboBoxItem Value="11" Text="11" />
                            <telerik:RadComboBoxItem Value="12" Text="12" />
                            <telerik:RadComboBoxItem Value="15" Text="15" />
                            <telerik:RadComboBoxItem Value="16" Text="16" />
                            <telerik:RadComboBoxItem Value="18" Text="18" />
                            <telerik:RadComboBoxItem Value="19" Text="19" />
                            <telerik:RadComboBoxItem Value="20" Text="20" />
                            <telerik:RadComboBoxItem Value="21" Text="21" />
                            <telerik:RadComboBoxItem Value="22" Text="22" />
                            <telerik:RadComboBoxItem Value="23" Text="23" />
                        </Items>
                    </telerik:RadComboBox>
                </FilterTemplate>
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="Plan_Type_Name" HeaderText='<%$ Resources:Resource, Label_Plan_Type %>'
                ItemStyle-CssClass="notmerged planType" SortExpression="Plan_Type_Name" UniqueName="Plan_Type_Name">
                <FilterTemplate>
                    <telerik:RadComboBox EnableEmbeddedSkins="false" SkinID="planInfoCombo" Skin="pathfinder"
                        runat="server" AppendDataBoundItems="true" DataSourceID="dsPlanType" ID="rdlPlanType"
                        MaxHeight="200" Width="90%" DropDownWidth="150px" DataTextField="Name" DataValueField="ID"
                        OnClientSelectedIndexChanged="onFilterPlansByPlanType">
                        <Items>
                            <telerik:RadComboBoxItem Value="" Text="" />
                        </Items>
                    </telerik:RadComboBox>
                </FilterTemplate>
            </telerik:GridBoundColumn>
            <%-- sl: Section_Name for 'All' channel selection --%>
            <telerik:GridBoundColumn DataField="Section_Name" HeaderText="Section Name" ItemStyle-CssClass="notmerged secName"
                SortExpression="Section_Name" UniqueName="Section_Name">
                <FilterTemplate>
                    <telerik:RadComboBox EnableEmbeddedSkins="false" SkinID="planInfoCombo" Skin="pathfinder"
                        runat="server" AppendDataBoundItems="true" ID="rdlSections" MaxHeight="200" Width="90%"
                        DropDownWidth="150px" DataTextField="Name" DataValueField="ID" OnClientSelectedIndexChanged="onFilterPlansBySection"
                        OnPreRender="rdlSections_PreRender">
                        <Items>
                            <telerik:RadComboBoxItem Value="" Text="" />
                        </Items>
                    </telerik:RadComboBox>
                </FilterTemplate>
            </telerik:GridBoundColumn>
            <%-- end --%>
            <telerik:GridBoundColumn DataField="MAC_Jurisdictions" ItemStyle-CssClass="notmerged macJur"
                HeaderText='<%$ Resources:Resource, Label_MAC_Jurisdictions %>' SortExpression="MAC_Jurisdictions"
                UniqueName="MAC_Jurisdictions">
                <FilterTemplate>
                </FilterTemplate>
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="Total_Covered" DataFormatString="{0:n0}" DataType="System.Int32"
                ItemStyle-CssClass="alignRight totalLives" HeaderText='Total Covered&#8224;'
                SortExpression="Total_Covered" UniqueName="Total_Covered">
                <FilterTemplate>
                    <telerik:RadComboBox EnableEmbeddedSkins="false" SkinID="planInfoCombo" Skin="pathfinder"
                        runat="server" ID="rdlCoveredLives" DataSourceID="dsLivesRange" DataTextField="Name"
                        DataValueField="ID" MaxHeight="200" Width="90%" DropDownWidth="150px" OnClientSelectedIndexChanged="onFilterPlansByCoveredLives"
                        AppendDataBoundItems="true">
                        <Items>
                            <telerik:RadComboBoxItem Value="" Text="" />
                        </Items>
                    </telerik:RadComboBox>
                </FilterTemplate>
            </telerik:GridBoundColumn>
            <%--            <telerik:GridBoundColumn DataField="Medical_Lives" DataFormatString="{0:n0}" DataType="System.Int32" ItemStyle-CssClass="notmerged alignRight medicalLives" HeaderText="Medical Lives"
                SortExpression="Medical_Lives" UniqueName="Medical_Lives">
                <FilterTemplate>
                    <telerik:RadComboBox EnableEmbeddedSkins="false" SkinID="planInfoCombo" Skin="pathfinder" runat="server" ID="rdlMedicalLives" DataSourceID="dsLivesRange" DataTextField="Name" DataValueField="ID" MaxHeight="200" Width="90%" DropDownWidth="150px" OnClientSelectedIndexChanged="onFilterPlansByMedicalLives" AppendDataBoundItems="true">
                        <Items>
                            <telerik:RadComboBoxItem Value="" Text="" />
                        </Items>
                    </telerik:RadComboBox>
                </FilterTemplate>
            </telerik:GridBoundColumn>--%>
            <telerik:GridBoundColumn HtmlEncode="false" DataField="Rx_Lives" HeaderStyle-Wrap="true"
                DataFormatString="{0:n0}" DataType="System.Int32" ItemStyle-CssClass="notmerged alignRight rxLives"
                HeaderText="Rx Lives*" SortExpression="Rx_Lives" UniqueName="Rx_Lives">
                <FilterTemplate>
                    <telerik:RadComboBox EnableEmbeddedSkins="false" SkinID="planInfoCombo" Skin="pathfinder"
                        runat="server" ID="rdlRxLives" DataSourceID="dsLivesRange" DataTextField="Name"
                        DataValueField="ID" MaxHeight="200" Width="90%" DropDownWidth="150px" OnClientSelectedIndexChanged="onFilterPlansByRxLives"
                        AppendDataBoundItems="true">
                        <Items>
                            <telerik:RadComboBoxItem Value="" Text="" />
                        </Items>
                    </telerik:RadComboBox>
                </FilterTemplate>
            </telerik:GridBoundColumn>
            <%--<telerik:GridBoundColumn HtmlEncode="false" DataField="Plan_Managed_Lives" HeaderStyle-Wrap="true" DataFormatString="{0:n0}" DataType="System.Int32" ItemStyle-CssClass="notmerged alignRight planManagedLives" HeaderText="Plan Managed<br />Rx Lives*"
                SortExpression="Plan_Managed_Lives" UniqueName="Plan_Managed_Lives">
                <FilterTemplate>
                    <telerik:RadComboBox EnableEmbeddedSkins="false" SkinID="planInfoCombo" Skin="pathfinder" runat="server" ID="rdlPlanManagedLives" DataSourceID="dsLivesRange" DataTextField="Name" DataValueField="ID" MaxHeight="200" Width="90%" DropDownWidth="150px" OnClientSelectedIndexChanged="onFilterPlansByPlanManagedLives" AppendDataBoundItems="true">
                        <Items>
                            <telerik:RadComboBoxItem Value="" Text="" />
                        </Items>
                    </telerik:RadComboBox>
                </FilterTemplate>
            </telerik:GridBoundColumn>
            
            <telerik:GridBoundColumn HtmlEncode="false" DataField="PBM_Managed_Lives" HeaderStyle-Wrap="true" DataFormatString="{0:n0}" DataType="System.Int32" ItemStyle-CssClass="notmerged alignRight pbmManagedLives" HeaderText="PBM Managed<br />Rx Lives"
                SortExpression="PBM_Managed_Lives" UniqueName="PBM_Managed_Lives">
                <FilterTemplate>
                    <telerik:RadComboBox EnableEmbeddedSkins="false" SkinID="planInfoCombo" Skin="pathfinder" runat="server" ID="rdlPBMManagedLives" DataSourceID="dsLivesRange" DataTextField="Name" DataValueField="ID" MaxHeight="200" Width="90%" DropDownWidth="150px" OnClientSelectedIndexChanged="onFilterPlansByPBMManagedLives" AppendDataBoundItems="true">
                        <Items>
                            <telerik:RadComboBoxItem Value="" Text="" />
                        </Items>
                    </telerik:RadComboBox>
                </FilterTemplate>
            </telerik:GridBoundColumn>--%>
            <telerik:GridBoundColumn HtmlEncode="false" DataField="Employer_Lives" HeaderStyle-Wrap="true"
                DataFormatString="{0:n0}" DataType="System.Int32" ItemStyle-CssClass="notmerged alignRight employerLives"
                HeaderText="Rx Lives<br />(Employer)" SortExpression="Employer_Lives" UniqueName="Employer_Lives">
                <FilterTemplate>
                    <telerik:RadComboBox EnableEmbeddedSkins="false" SkinID="planInfoCombo" Skin="pathfinder"
                        runat="server" ID="rdlEmployerLives" DataSourceID="dsLivesRange" DataTextField="Name"
                        DataValueField="ID" MaxHeight="200" Width="90%" DropDownWidth="150px" OnClientSelectedIndexChanged="onFilterPlansByEmployerLives"
                        AppendDataBoundItems="true">
                        <Items>
                            <telerik:RadComboBoxItem Value="" Text="" />
                        </Items>
                    </telerik:RadComboBox>
                </FilterTemplate>
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn HtmlEncode="false" DataField="Formulary_Managed_by" HeaderStyle-Wrap="true"
                ItemStyle-CssClass="notmerged formularyManagedBy" HeaderText="Formulary<br/>Managed By"
                SortExpression="Formulary_Managed_by" UniqueName="Formulary_Managed_by">
                <FilterTemplate>
                </FilterTemplate>
            </telerik:GridBoundColumn>
            <%--<telerik:GridBoundColumn DataField="Total_Pharmacy" DataFormatString="{0:n0}" ItemStyle-CssClass="alignRight totalPharm"
                DataType="System.Int32" HeaderText='Pharmacy Lives*'
                SortExpression="Total_Pharmacy" HeaderStyle-Wrap="true" UniqueName="Total_Pharmacy">
                <FilterTemplate>
                    <telerik:RadComboBox EnableEmbeddedSkins="false" DataSourceID="dsLivesRange" DataTextField="Name" DataValueField="ID" SkinID="planInfoCombo" Skin="pathfinder" runat="server" ID="rdlPharmacyLives" MaxHeight="200" Width="90%" DropDownWidth="150px" OnClientSelectedIndexChanged="onFilterPlansByPharmacyLives" AppendDataBoundItems="true">
                        <Items>
                            <telerik:RadComboBoxItem Value="" Text="" />                            
                        </Items>
                    </telerik:RadComboBox>
                </FilterTemplate>
            </telerik:GridBoundColumn> --%>
            <%--<telerik:GridBoundColumn DataField="Commercial_Pharmacy_Lives" DataFormatString="{0:n0}" ItemStyle-CssClass="alignRight commercialPharm"
                DataType="System.Int32" HeaderText="Pharmacy Lives"
                SortExpression="Commercial_Pharmacy_Lives" HeaderStyle-Wrap="true" UniqueName="Commercial_Pharmacy_Lives">
                <FilterTemplate>
                    <telerik:RadComboBox EnableEmbeddedSkins="false" DataSourceID="dsLivesRange" DataTextField="Name" DataValueField="ID" SkinID="planInfoCombo" Skin="pathfinder" runat="server" ID="rdlCommercialPharmacyLives" MaxHeight="200" Width="90%" DropDownWidth="150px" OnClientSelectedIndexChanged="onFilterPlansByPharmacyLives" AppendDataBoundItems="true">
                        <Items>
                            <telerik:RadComboBoxItem Value="" Text="" />                            
                        </Items>
                    </telerik:RadComboBox>
                </FilterTemplate>
            </telerik:GridBoundColumn>  
                    
            <telerik:GridBoundColumn DataField="Medicaid_Mcare_Enrollment" DataFormatString="{0:n0}" ItemStyle-CssClass="alignRight totalMedicaid"
                DataType="System.Int32" HeaderText="Medicaid Lives"
                SortExpression="Medicaid_Mcare_Enrollment" UniqueName="Medicaid_Mcare_Enrollment">
                <FilterTemplate>
                    <telerik:RadComboBox EnableEmbeddedSkins="false" DataSourceID="dsLivesRange" DataTextField="Name" DataValueField="ID" SkinID="planInfoCombo" Skin="pathfinder" runat="server" ID="rdlMedicaidLives" MaxHeight="200" Width="90%" DropDownWidth="150px" OnClientSelectedIndexChanged="onFilterPlansByMedicaidLives" AppendDataBoundItems="true">
                        <Items>
                            <telerik:RadComboBoxItem Value="" Text="" />                            
                        </Items>
                    </telerik:RadComboBox>
                </FilterTemplate>
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="Fortune_500_Rank" ItemStyle-CssClass="alignRight fortuneRank"
                DataType="System.Int32" HeaderText='<%$ Resources:Resource, Label_Fortune_500_Rank %>'
                SortExpression="Fortune_500_Rank" UniqueName="Fortune_500_Rank" AllowFiltering="false">
            </telerik:GridBoundColumn>--%>
            <%-- kink of hackish but setting website inside span to handle click event instead of using hyperlink so we can grab event information --%>
            <%--<telerik:GridBoundColumn DataFormatString='<%$ Resources:Resource, HTML_PlanWebsite_Link %>' DataField="Plan_WebSite" AllowFiltering="false"  HeaderText='<%$ Resources:Resource, Label_Website %>' UniqueName="Plan_WebSite" ItemStyle-CssClass="links planWebsite" />            --%>
        </Columns>
        <SortExpressions>
            <telerik:GridSortExpression FieldName="Plan_Name" SortOrder="Ascending" />
        </SortExpressions>
        <PagerStyle Visible="false" />
        <FilterItemStyle CssClass="planInfoFilterRow" />
    </MasterTableView>
    <ClientSettings>
        <DataBinding Location="~/todaysaccounts/services/pathfinderdataservice.svc" DataService-TableName="PlanInfoListViewSet" />
        <Scrolling AllowScroll="True" UseStaticHeaders="True" ScrollHeight="100%" />
        <Selecting AllowRowSelect="True" />
    </ClientSettings>
</telerik:RadGrid>
<pinso:RadGridWrapper runat="server" ID="radGridWrapper" Target="gridPlanInfo" ClientTypeName="Pathfinder.UI.PlanInfoGridWrapper"
    MergeRows="true" RequiresFilter="false" AutoUpdate="false" PagingSelector="#divTile2Container .pagination" />
<asp:EntityDataSource runat="server" ID="dsStates" ConnectionString="name=PathfinderEntities"
    DefaultContainerName="PathfinderEntities" EntitySetName="StateSet" OrderBy="it.Name" />
<asp:EntityDataSource runat="server" ID="dsPlanType" ConnectionString="name=PathfinderEntities"
    DefaultContainerName="PathfinderEntities" EntitySetName="PlanTypeSet" OrderBy="it.Name" />
<asp:EntityDataSource runat="server" ID="dsLivesRange" ConnectionString="name=PathfinderEntities"
    DefaultContainerName="PathfinderEntities" EntitySetName="LivesRangeSet" OrderBy="it.Sort_Order" />
<%--<asp:EntityDataSource runat="server" ID="dsSections" ConnectionString="name=PathfinderEntities" DefaultContainerName="PathfinderEntities"
  EntitySetName="ClientApplicationAccess"
   AutoGenerateWhereClause="true" OrderBy="it.Section_ID">
  <WhereParameters>
  <asp:SessionParameter Name="Client_ID" SessionField="Client_ID" Type="Int32" ConvertEmptyStringToNull="true" />
  <asp:Parameter Name="App_ID" DefaultValue="1" Type="Int32" />
  </WhereParameters>
  
  
  
  </asp:EntityDataSource>--%>
<div id="planInfoLegend">
    <table width="100%" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td valign="middle">
                &#8224;Commercial = Medical Lives; *State Medicaid = FFS Lives; *PBM = Where PBM is managing the formulary
            </td>
            <%--<td valign="middle">
                <div id="planInfoDistribution" onclick="javascript:clientManager.openPieChartViewer(700,600);"></div>
            </td>--%>
        </tr>
    </table>
</div>
