<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LivesDistribution.ascx.cs" Inherits="standardreports_controls_LivesDistribution" %>

<telerik:RadGrid runat="server" ID="gridLDReport" SkinID="radTable" AutoGenerateColumns="false" 
    EnableEmbeddedSkins="false" Skin="pathfinder" PageSize="50">
    <MasterTableView PageSize="50" AutoGenerateColumns="false" AllowSorting="true" AllowMultiColumnSorting="true">
        <Columns>
            <telerik:GridBoundColumn DataField="Plan_Name" UniqueName="Plan_Name" HeaderText="Account Name"  HeaderStyle-Width="250px"
               ItemStyle-CssClass="firstcol Plan_Name" HeaderStyle-CssClass="rgHeader Plan_Name" SortExpression="Plan_Name"/>
             <telerik:GridBoundColumn DataField="Section_Name" UniqueName="Section_Name" HeaderText="Section"  
                ItemStyle-CssClass="Geography_Name" HeaderStyle-CssClass="rgHeader Geography_Name" />
                
            <telerik:GridBoundColumn DataField="Geography_Name" UniqueName="Geography_Name" HeaderText="Geography"  
                ItemStyle-CssClass="Geography_Name" HeaderStyle-CssClass="rgHeader Geography_Name" SortExpression="Geography_Name"/>
            <telerik:GridBoundColumn DataField="Total_Covered" UniqueName="Total_Covered" HeaderText="Total Lives"  DataFormatString="{0:n0}" DataType="System.Int32"   
                ItemStyle-CssClass="alignRight Total_Covered" HeaderStyle-CssClass="rgHeader Total_Covered" SortExpression="Total_Covered"/> 

            <telerik:GridBoundColumn DataField="Medical_Lives" UniqueName="Medical_Lives" HeaderText="Total<br> Medical<br> Lives" DataType="System.Int32"  DataFormatString="{0:n0}" 
                ItemStyle-CssClass="alignRight Medical_Lives" HeaderStyle-CssClass="rgHeader Medical_Lives" Visible="false"/>  
               
            <telerik:GridBoundColumn DataField="Total_Pharmacy" UniqueName="Total_Pharmacy" HeaderText="Total<br> Pharmacy<br> Lives" DataType="System.Int32"  DataFormatString="{0:n0}"  
                ItemStyle-CssClass="alignRight Total_Pharmacy" HeaderStyle-CssClass="rgHeader Total_Pharmacy"/>
            <telerik:GridBoundColumn DataField="Total_Commercial_Lives" UniqueName="Total_Commercial_Lives" HeaderText="Total Commercial Lives"  DataType="System.Int32" DataFormatString="{0:n0}" 
                ItemStyle-CssClass="alignRight Total_Commercial_Lives" HeaderStyle-CssClass="rgHeader Total_Commercial_Lives"/> 
            <telerik:GridBoundColumn DataField="Commercial_Pharmacy_Lives" UniqueName="Commercial_Pharmacy_Lives" HeaderText="Commercial<br> Pharmacy<br> Lives" DataType="System.Int32" DataFormatString="{0:n0}" 
                ItemStyle-CssClass="alignRight Commercial_Pharmacy_Lives" HeaderStyle-CssClass="rgHeader Commercial_Pharmacy_Lives"/>    
            <telerik:GridBoundColumn DataField="HMO_Lives" UniqueName="HMO_Lives" HeaderText="HMO"  DataFormatString="{0:n0}" DataType="System.Int32"  
                ItemStyle-CssClass="alignRight HMO_Lives" HeaderStyle-CssClass="rgHeader HMO_Lives" /> 
            <telerik:GridBoundColumn DataField="PPO_Lives" UniqueName="PPO_Lives" HeaderText="PPO"  DataFormatString="{0:n0}"  DataType="System.Int32" 
                ItemStyle-CssClass="alignRight PPO_Lives" HeaderStyle-CssClass="rgHeader PPO_Lives"/>
            <telerik:GridBoundColumn DataField="POS_Lives" UniqueName="POS_Lives" HeaderText="POS"  DataFormatString="{0:n0}"  DataType="System.Int32" 
                ItemStyle-CssClass="alignRight POS_Lives" HeaderStyle-CssClass="rgHeader POS_Lives" />
            <telerik:GridBoundColumn DataField="CDH_Lives" UniqueName="CDH_Lives" HeaderText="Consumer<br> Driven<br> Health<br> (CDH)" DataType="System.Int32"   DataFormatString="{0:n0}" 
                ItemStyle-CssClass="alignRight CDH_Lives" HeaderStyle-CssClass="rgHeader CDH_Lives" />  
            <telerik:GridBoundColumn DataField="Self_Insured_Lives" UniqueName="Self_Insured_Lives" HeaderText="Self Insured"  DataType="System.Int32" DataFormatString="{0:n0}" 
                ItemStyle-CssClass="alignRight Self_Insured_Lives" HeaderStyle-CssClass="rgHeader Self_Insured_Lives"/>  
                
            <telerik:GridBoundColumn DataField="Managed_Medicaid_Medical_Lives" UniqueName="Managed_Medicaid_Medical_Lives" HeaderText="Managed Medicaid Medical Lives" DataType="System.Int32" DataFormatString="{0:n0}" 
                ItemStyle-CssClass="alignRight Managed_Medicaid_Medical_Lives" HeaderStyle-CssClass="rgHeader Managed_Medicaid_Medical_Lives"/>     
             <telerik:GridBoundColumn DataField="Managed_Medicaid_Lives" UniqueName="Managed_Medicaid_Lives" HeaderText="Managed Medicaid Rx Lives" DataType="System.Int32" DataFormatString="{0:n0}" 
                ItemStyle-CssClass="alignRight Managed_Medicaid_Lives" HeaderStyle-CssClass="rgHeader Managed_Medicaid_Lives"/>
               
            
            
            <telerik:GridBoundColumn DataField="Medicare_PartD_Lives" UniqueName="Medicare_PartD_Lives" HeaderText="Total<br> Medicare<br> Part D" DataType="System.Int32"  DataFormatString="{0:n0}" 
                ItemStyle-CssClass="alignRight Medicare_PartD_Lives" HeaderStyle-CssClass="rgHeader Medicare_PartD_Lives" />
            
            <telerik:GridBoundColumn DataField="MAPD_Lives" UniqueName="MAPD_Lives" HeaderText="MA-PD"  DataFormatString="{0:n0}" DataType="System.Int32"   
                ItemStyle-CssClass="alignRight MAPD_Lives" HeaderStyle-CssClass="rgHeader MAPD_Lives"/>
            <telerik:GridBoundColumn DataField="PDP_Lives" UniqueName="PDP_Lives" HeaderText="PDP"  DataFormatString="{0:n0}" DataType="System.Int32" 
                ItemStyle-CssClass="alignRight PDP_Lives" HeaderStyle-CssClass="rgHeader PDP_Lives" />           
            
            <telerik:GridBoundColumn DataField="MAPD_LIS_Lives" UniqueName="MAPD_LIS_Lives" HeaderText="MA-PD LIS"  DataFormatString="{0:n0}" DataType="System.Int32"   
                ItemStyle-CssClass="alignRight MAPD_LIS_Lives" HeaderStyle-CssClass="rgHeader MAPD_LIS_Lives"/>
            <telerik:GridBoundColumn DataField="PDP_LIS_Lives" UniqueName="PDP_LIS_Lives" HeaderText="PDP LIS"  DataFormatString="{0:n0}" DataType="System.Int32" 
                ItemStyle-CssClass="alignRight PDP_LIS_Lives" HeaderStyle-CssClass="rgHeader PDP_LIS_Lives" /> 
                
            <telerik:GridBoundColumn DataField="Employer_Lives" UniqueName="Employer_Lives" HeaderText="Employer Lives" DataType="System.Int32"  DataFormatString="{0:n0}" 
                ItemStyle-CssClass="alignRight Employer_Lives" HeaderStyle-CssClass="rgHeader Employer_Lives"/>
            
            <telerik:GridBoundColumn DataField="Fully_Insured_Lives" UniqueName="Fully_Insured_Lives" HeaderText="Fully Insured" DataType="System.Int32"  DataFormatString="{0:n0}" 
                ItemStyle-CssClass="alignRight Fully_Insured_Lives"  HeaderStyle-CssClass="rgHeader Fully_Insured_Lives" Visible="false" /> 
            <telerik:GridBoundColumn DataField="Labor_Organization_Lives" UniqueName="Labor_Organization_Lives" HeaderText="Labor Organization"  DataType="System.Int32" DataFormatString="{0:n0}" 
                ItemStyle-CssClass="alignRight Labor_Organization_Lives"  HeaderStyle-CssClass="rgHeader Labor_Organization_Lives" Visible="false" />
            <telerik:GridBoundColumn DataField="Other_Lives" UniqueName="Other_Lives" HeaderText="Other"  DataFormatString="{0:n0}" DataType="System.Int32" 
                ItemStyle-CssClass="alignRight Other_Lives" HeaderStyle-CssClass="rgHeader Other_Lives" Visible="false" />
            <telerik:GridBoundColumn DataField="Percent_ManagedCare" UniqueName="Percent_ManagedCare" HeaderText="% in Managed Care" DataType="System.Decimal"  DataFormatString="{0:n0}" 
                ItemStyle-CssClass="alignRight Percent_ManagedCare" HeaderStyle-CssClass="rgHeader Percent_ManagedCare" Visible="false" />  
             
                
            <telerik:GridBoundColumn DataField="Employer_Formulary" UniqueName="Employer_Formulary" HeaderText="Employer Formulary Lives"  DataFormatString="{0:n0}" DataType="System.Int32" 
                ItemStyle-CssClass="alignRight Employer_Formulary"  HeaderStyle-CssClass="rgHeader Employer_Formulary" /> 
            <telerik:GridBoundColumn DataField="Health_Plan_Lives" UniqueName="Health_Plan_Lives" HeaderText="Health<br> Plan<br> Lives" DataType="System.Int32"   DataFormatString="{0:n0}" 
                ItemStyle-CssClass="alignRight Health_Plan_Lives" HeaderStyle-CssClass="rgHeader Health_Plan_Lives" />                           
            <telerik:GridBoundColumn DataField="Health_Plan_Processor" UniqueName="Health_Plan_Processor" HeaderText="Health Plan Processor"  DataFormatString="{0:n0}" DataType="System.Int32" 
                ItemStyle-CssClass="alignRight Health_Plan_Processor"  HeaderStyle-CssClass="rgHeader Health_Plan_Processor" />
            <telerik:GridBoundColumn DataField="Health_Plan_Formulary" UniqueName="Health_Plan_Formulary" HeaderText="Health Plan Formulary"  DataFormatString="{0:n0}" DataType="System.Int32" 
                ItemStyle-CssClass="alignRight Health_Plan_Formulary"  HeaderStyle-CssClass="rgHeader Health_Plan_Formulary" /> 
            <telerik:GridBoundColumn DataField="Health_Plan_Commercial" UniqueName="Health_Plan_Commercial" HeaderText="Health Plan Commercial"  DataFormatString="{0:n0}" DataType="System.Int32" 
                ItemStyle-CssClass="alignRight Health_Plan_Commercial"  HeaderStyle-CssClass="rgHeader Health_Plan_Commercial" /> 
                                        
            <telerik:GridBoundColumn DataField="Health_Plan_Managed_Medicaid" UniqueName="Health_Plan_Managed_Medicaid" HeaderText="Health Plan Managed Medicaid"  DataFormatString="{0:n0}" DataType="System.Int32" 
                ItemStyle-CssClass="alignRight Health_Plan_Managed_Medicaid"  HeaderStyle-CssClass="rgHeader Health_Plan_Managed_Medicaid" /> 
            <telerik:GridBoundColumn DataField="Health_Plan_Medicare_Part_D" UniqueName="Health_Plan_Medicare_Part_D" HeaderText="Health Plan Medicare Part D"  DataFormatString="{0:n0}" DataType="System.Int32" 
                ItemStyle-CssClass="alignRight Health_Plan_Medicare_Part_D"  HeaderStyle-CssClass="rgHeader Health_Plan_Medicare_Part_D" />
            <telerik:GridBoundColumn DataField="Medicaid_Mcare_Enrollment" UniqueName="Medicaid_Mcare_Enrollment" HeaderText="Managed<br> Medicaid<br> Lives" DataType="System.Int32" DataFormatString="{0:n0}" 
               ItemStyle-CssClass="alignRight Medicaid_Mcare_Enrollment"  HeaderStyle-CssClass="rgHeader Medicaid_Mcare_Enrollment"/> 
            <telerik:GridBoundColumn DataField="Medicaid_Enrollment" UniqueName="Medicaid_Enrollment" HeaderText="Medicaid Enrollment" DataType="System.Int32"  DataFormatString="{0:n0}" 
                ItemStyle-CssClass="alignRight Medicaid_Enrollment" HeaderStyle-CssClass="rgHeader Medicaid_Enrollment" /> 
            <telerik:GridBoundColumn DataField="FFS_Lives" UniqueName="FFS_Lives" HeaderText="FFS Lives"  DataFormatString="{0:n0}" DataType="System.Int32" 
                ItemStyle-CssClass="alignRight FFS_Lives"  HeaderStyle-CssClass="rgHeader FFS_Lives" />                
        </Columns>
        <SortExpressions>
            <telerik:GridSortExpression FieldName="Plan_Name" SortOrder="Ascending" />
            <telerik:GridSortExpression FieldName="Geography_Name" SortOrder="Ascending" />
            <telerik:GridSortExpression FieldName="Total_Covered" SortOrder="Ascending" />
            <telerik:GridSortExpression FieldName="Total_Pharmacy" SortOrder="Ascending" />
        </SortExpressions>
    </MasterTableView>
    <ClientSettings>
        <Scrolling AllowScroll="true" UseStaticHeaders="true" />
        <DataBinding Location="~/standardreports/services/pathfinderdataservice.svc"  DataService-TableName="LivesDistributionSet"/>
    </ClientSettings>
</telerik:RadGrid>
<pinso:RadGridWrapper runat="server" ID="gridWrapper" Target="gridLDReport" PagingSelector="#tile3 .pagination:first"  CustomPaging="false" MergeRows="true"  RequiresFilter ="true" AutoLoad="false" ShowNumberOfRecords="true" />
