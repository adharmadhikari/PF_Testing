<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MedicalPolicy.ascx.cs" Inherits="custom_merz_businessplanning_controls_MedicalPolicy" %>
<div class="rightBPTile divborder rightMedPDFTile rightMedTile" id="MedPolicyDiv" runat="server" >
    <div class="tileContainerHeader">
        <div class="leftBP leftBgTile title" id="MedPolicyHeader" runat="server">Document Upload</div>
        <%--<div class="rightBP rightSmTile" id="MedPolicyPager" runat="server" >--%>
            <div class="rightBP mplinks">&nbsp;<span id="separator31" class="pipe" runat="server"  >|</span>&nbsp;<a id="A2" href="javascript:openBPDeletewindow();" runat="server" visible="false">Delete</a></div>
            <div class="rightBP mplinks"><span id="separator3" class="pipe" runat="server"  >|</span>&nbsp;<a id="A1" href="javascript:openBPUploadwindow();" runat="server" visible="false">Upload</a></div>
            <div class="rightBP"><div id="MedPolicyPager" class="pagination"></div></div>            
            <%--<div class="clearAll"></div>--%>
        <%--</div>--%>
        <div class="clearAll"></div>
    </div>

    <telerik:RadGrid SkinID="radTable" ID="RadGridbppOLICY" runat="server"  GridLines="None" DataSourceID="" Visible="true" >
        <MasterTableView autogeneratecolumns="False" AllowSorting="true" AllowPaging="false" ClientDataKeyNames="Medical_Policy_ID, Medical_Policy_Name, Med_Policy_Status_ID, Comments" >
            <Columns>           
                <telerik:GridHyperLinkColumn ItemStyle-CssClass="firstCol firstGeneric" HeaderStyle-CssClass="firstCol firstGeneric" DataNavigateUrlFields= "Medical_Policy_ID" DataTextField="Medical_Policy_Name"  Target="_blank"   HeaderText="File Name" DataNavigateUrlFormatString= "~/custom/merz/businessplanning/all/PlanDocument.ashx?id={0}" />
                <%--<telerik:GridBoundColumn DataField="Upload_BY" HeaderText="Upload By" HeaderStyle-HorizontalAlign="Center"
                    SortExpression="Upload_BY" UniqueName="Upload_BY">
                </telerik:GridBoundColumn>--%>
                <telerik:GridBoundColumn DataField="Upload_DT" DataType="System.DateTime"  DataFormatString="{0:d}" HeaderStyle-HorizontalAlign="Center"
                    HeaderText="Date" SortExpression="Upload_DT" UniqueName="Upload_DT" ItemStyle-HorizontalAlign="Center">
                </telerik:GridBoundColumn>
                <telerik:GridHyperLinkColumn DataNavigateUrlFormatString='javascript:OpenMedPolicyDetails({0},"comments",null,null,200,150);' DataNavigateUrlFields="Medical_Policy_ID" UniqueName="Comments" ItemStyle-CssClass="commentsCell" HeaderText="&nbsp;" HeaderStyle-Width="5%"></telerik:GridHyperLinkColumn>    
            </Columns>
            <SortExpressions>
                <telerik:GridSortExpression FieldName="Upload_DT" SortOrder="Descending" />
                <%--<telerik:GridSortExpression FieldName="Medical_Policy_Name" SortOrder="Ascending" />--%>
            </SortExpressions>   
        </MasterTableView>
        <ClientSettings ClientEvents-OnDataBound="RadGridbppOLICY_onDataBound" Selecting-AllowRowSelect="true">
          <DataBinding Location="~/custom/merz/businessplanning/services/MerzDataService.svc/BusinessPlansSet(12)" DataService-TableName="BusinessPlanMedicalPolicyDocuments"/>
          <Scrolling AllowScroll="false" UseStaticHeaders="false" />
        </ClientSettings>
    </telerik:RadGrid>   
    <pinso:RadGridWrapper runat="server" ID="radGridWrapper" Target="RadGridbppOLICY" CustomPaging="false"  PagingSelector=""  MergeRows="false" RequiresFilter="false" AutoUpdate="false" AutoLoad="false" ShowNumberOfRecords="false" />

    <asp:GridView ID="grdvwMedPolicy" runat="server" AutoGenerateColumns="False" DataSourceID="dsMedPolicy"
        Width="100%" GridLines="none"  BorderStyle="None" CssClass="staticTable" Visible="false">
        <Columns>
            <asp:BoundField DataField="Medical_Policy_Name" HeaderText="File Name" Visible="true" HeaderStyle-CssClass="firstCol firstGeneric MedPolicyCol" ItemStyle-CssClass="firstCol firstGeneric MedPolicyCol" />
            <asp:TemplateField HeaderText="Document Type" Visible="true" HeaderStyle-CssClass="firstGeneric MedPolicyCol" ItemStyle-CssClass="firstGeneric MedPolicyCol" >
                <ItemTemplate><%# Eval("BusinessPlanDocumentTypes.Document_Type_Name")%>&nbsp;</ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="Upload_BY" HeaderText="Upload By" Visible="true" HeaderStyle-CssClass="firstGeneric MedPolicyCol" ItemStyle-CssClass="firstGeneric MedPolicyCol" />
            <asp:BoundField DataField="Upload_DT" HeaderText="Date"  DataFormatString="{0:d}" Visible="true" HeaderStyle-CssClass="firstGeneric MedPolicyCol" ItemStyle-CssClass="firstGeneric MedPolicyCol" />
        </Columns>
        <EmptyDataTemplate>
                <asp:Label ID="Label1" text="There are no records to display." runat="server"></asp:Label>
        </EmptyDataTemplate>
    </asp:GridView>
    <asp:EntityDataSource ID="dsMedPolicy" runat="server" ConnectionString="name=PathfinderMerzEntities"
                                    DefaultContainerName="PathfinderMerzEntities" EntitySetName="BusinessPlanMedicalPolicyDocSet"
                                    AutoGenerateWhereClause="false" OrderBy="it.[Upload_DT] DESC,it.[Medical_Policy_Name]" Where="it.BusinessPlan.Business_Plan_ID=@BP_ID and it.Med_Policy_Status_ID=1" 
                                    Include="BusinessPlanDocumentTypes">
        <WhereParameters>
            <asp:ControlParameter ControlID="ctl00$Tile3$BP_ID" PropertyName="Value" Name="BP_ID" Type="Int32" DefaultValue="0"/>  
        </WhereParameters>          
    </asp:EntityDataSource>      
</div>