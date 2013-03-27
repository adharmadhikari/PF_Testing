<%@ Control Language="C#" AutoEventWireup="true" CodeFile="CCRPlanGridView.ascx.cs" Inherits="custom_auxilium_customercontactreports_controls_CCRPlanGridView" %>
 <telerik:RadGrid SkinID="radTable" runat="server" ID="gridPlans" AllowSorting="True" EnableEmbeddedSkins="False" PageSize="50" AllowFilteringByColumn="true">        
       <MasterTableView autogeneratecolumns="false" ClientDataKeyNames="ID" AllowPaging="true" Width="100%" PageSize="50">
           <Columns>
            <telerik:GridBoundColumn DataField="Section_Name" HeaderStyle-Width="25%" HeaderText='<%$ Resources:Resource, Label_Section_Name %>' SortExpression="Section_Name" UniqueName="Section_Name" DataType ="System.String">                 
              <FilterTemplate>
                <telerik:radcombobox ID ="rdlSection" runat = "server" DataTextField = "Section_Name"
                                 DataValueField = "Section_ID" AppendDataBoundItems ="true" SkinID="planInfoCombo"  Skin="pathfinder" Width="90%" 
                                EnableEmbeddedSkins ="false" DropDownWidth="180px" 
                                OnClientSelectedIndexChanged="onFilterPlansSectionBySection">
                </telerik:radcombobox>
              </FilterTemplate>
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="Name" DataFormatString="{0} " HeaderStyle-Width="50%" HeaderText='<%$ Resources:Resource, Label_Plan_Name %>' ItemStyle-CssClass="planName" UniqueName="Name"  DataType ="System.String">          
                <FilterTemplate>
                    <div class="searchTextBox"><input class="textBox" type="text" onkeyup="setPlanSectionGridTimeout(this, {dataField:'Name', filterType:'Contains'})" /></div>                    
                </FilterTemplate>
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="Plan_State" HeaderStyle-Width="25%" ItemStyle-CssClass="planState" HeaderText='<%$ Resources:Resource, Label_State %>' SortExpression="Plan_State" UniqueName="Plan_State" DataType = "System.String" >
             <FilterTemplate>
                    <telerik:RadComboBox EnableEmbeddedSkins="false" SkinID="planInfoCombo" Skin="pathfinder"
                        runat="server" ID="rdlStates" Width="90%" DropDownWidth="120px"
                        DataTextField="Section_Name" DataValueField="Section_ID" AppendDataBoundItems="true" MaxHeight="200"
                        OnClientSelectedIndexChanged="onFilterPlansSectionByState">
                    </telerik:RadComboBox>
                </FilterTemplate>
                </telerik:GridBoundColumn>
           </Columns>    
             <SortExpressions>
                <telerik:GridSortExpression FieldName="Name" SortOrder="Ascending" />
            </SortExpressions>            
        </MasterTableView>
        
       <PagerStyle Visible="false" />
        <FilterItemStyle CssClass="planInfoFilterRow" />
        <ClientSettings ClientEvents-OnRowSelected="gridPlans_OnRowSelected">
            <DataBinding Location="~/custom/auxilium/customercontactreports/services/AuxiliumDataService.svc" DataService-TableName="PlanListSet" >
            </DataBinding>
            <Scrolling AllowScroll="true" UseStaticHeaders="true" /> 
            <Selecting AllowRowSelect="true" />
        </ClientSettings>   
    </telerik:RadGrid>    
     <pinso:RadGridWrapper runat="server" ID="gridWrapper" Target="gridPlans" MergeRows="false" PagingSelector="#tile8ContainerHeader .pagination"  RequiresFilter ="false" AutoLoad="true" />       
