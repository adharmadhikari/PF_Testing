<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/SingleSection.master"
    AutoEventWireup="true" CodeFile="activityentry.aspx.cs" Inherits="custom_alcon_activityreporting_all_activityentry" %>

<%@ Register Src="~/custom/Alcon/ActivityReporting/controls/ActivityEntryScript.ascx"
    TagName="ActivityEntryScript" TagPrefix="pinso" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="scriptContainer" runat="Server">
    <pinso:ActivityEntryScript runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Tile3Title" runat="Server">
    Activity Entry
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Tile3Tools" runat="Server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Tile3" runat="Server">
    <div class="dateEntry">
        <b><asp:Label ID="lblActivityDate" runat="server" Text="Activity Date"></asp:Label>
        &nbsp;</b>
        <asp:TextBox id="txtActivityDate" runat="server" CssClass="datePicker" onblur="showGrid(this);" />
    </div>
    <div id="activityentryupdate" runat="server" >    <%-- style="visibility:hidden;">--%>
         <telerik:RadGrid SkinID="radTable" ID="gvActivityEntry" DataSourceID="dsActivity" AutoGenerateColumns="false"
            runat="server" BorderStyle="None">
            <MasterTableView DataKeyNames="Activity_Type_ID">
                <Columns>
                    <telerik:GridBoundColumn DataField="Activity_Type_Name" HeaderText="Activity Type"
                        SortExpression="Activity_Type_Name" ReadOnly="true" />
                    <telerik:GridTemplateColumn UniqueName="un_ActivityHours" HeaderText="Hours" ItemStyle-CssClass="activityhourstext">
                        <ItemTemplate>
                            <span id='<%# String.Format("{0}_{1}", Eval("Activity_Type_ID"), Eval("Activity_Hours")) %>'>
                                <asp:HiddenField ID="hTypeID" runat="server" Value='<%# Eval("Activity_Type_ID") %>' />
                                <asp:TextBox CssClass="datePicker" ID="txtHours" runat="server" Text='<%# Eval("Activity_Hours") %>' 
                                 onkeyup='<%# string.Format("ActivityEntryChanged(this,{0},{1})", Eval("Activity_Type_ID"), Eval("Activity_Hours")) %>'
                                   >
                                </asp:TextBox>
                            </span>
                     </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
            </MasterTableView>
        </telerik:RadGrid>
         
        
      <asp:SqlDataSource ID="dsActivity" runat="server" ConnectionString="<%$ ConnectionStrings:PathfinderClientDB_Format %>"
            SelectCommand="usp_DailyActivity_Select" SelectCommandType="StoredProcedure"
            UpdateCommandType="StoredProcedure" EnableCaching="false">
            <SelectParameters>
                <asp:SessionParameter Name="UserID" SessionField="UserID" DbType="Int32" />
                <asp:QueryStringParameter Name="ActivityDate" DefaultValue="1/1/1900" Type="DateTime" QueryStringField="ActivityDate" /> 
            </SelectParameters>
        </asp:SqlDataSource>  
        </div>   
            <div class="modalFormButtons"> 
                <pinso:CustomButton runat="server" ID="btnSubmit" Text="Submit" CssClass="postback validate"  
                    OnClick="btnSubmit_Click"  />
            </div>
</asp:Content>
