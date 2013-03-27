<%@ Control Language="C#" AutoEventWireup="true" CodeFile="formularycomparison.ascx.cs" Inherits="custom_pinso_formularyhistoryreporting_controls_formularycomparison" %>

<%--<div class="tilePaginationHeader" id="divPage" runat="server" visible="false">    
         <pinso:CustomButton ID="btnPrevious" runat="server" Text="Previous" OnClientClick="javascript:RefreshPage('custom/pinso/formularyhistoryreporting/all/FormularyComparison.aspx','Previous','fhrGridContainer');" /> 
          <asp:Label ID="lblPageText" runat="server" Text="Page"></asp:Label>
          <asp:Label ID="lblPageIndex" runat="server" Text="1"></asp:Label>        
         <pinso:CustomButton ID="btnNext" runat="server" Text="Next" OnClientClick="javascript:RefreshPage('custom/pinso/formularyhistoryreporting/all/FormularyComparison.aspx','Next','fhrGridContainer');" />            
</div>--%>
 <div id="divNoRecords" runat="server" visible="false" >
     <asp:Label ID="lblNoRecords" runat="server" Text="No records found."></asp:Label>
 </div>

 <asp:Panel runat="server" id="fhrContainer" CssClass="fhrContainer">
        <asp:GridView ID="gridFHX" runat="server" AutoGenerateColumns="False" Width="100%" 
             OnRowCreated="gridFHX_RowCreated" 
             OnRowDataBound="gridFHX_RowDataBound" 
             OnDataBound="gridFHX_DataBound"
             ShowHeader="true"  
             emptydatatext="No data available."              
             AllowPaging="false"           
            GridLines="None" CssClass="grid gv" HeaderStyle-CssClass="detailedHeader gvh" RowStyle-CssClass="gvr" 
            AlternatingRowStyle-CssClass="alt">     
            <Columns>
                <asp:BoundField DataField="Plan_Name" HeaderText="Account Name" ItemStyle-CssClass="itemList">
                </asp:BoundField>
                <asp:BoundField DataField="Plan_State_ID" HeaderText="State"></asp:BoundField>
                <asp:BoundField DataField="Plan_Pharmacy_Lives" HeaderText="Pharmacy Lives" ItemStyle-CssClass="alignRight" DataFormatString="{0:N0}">       
                </asp:BoundField>
                <asp:BoundField DataField="Product_Name" HeaderText="Product Name" ItemStyle-CssClass="itemList">       
                </asp:BoundField>
                <asp:BoundField DataField="Formulary_Name" HeaderText="Benefit Design" ItemStyle-CssClass="itemList">
                </asp:BoundField>
                <asp:BoundField DataField="Formulary_Lives" HeaderText="Formulary Lives" ItemStyle-CssClass="alignRight" DataFormatString="{0:N0}"></asp:BoundField>                
                <asp:BoundField DataField="Drug_Name" HeaderText="Drug Name" ItemStyle-CssClass="itemList"></asp:BoundField> 
            </Columns>
            
            <HeaderStyle CssClass="detailedHeader gvh"></HeaderStyle>
            
            
        </asp:GridView> 
         <asp:Label ID="gridCount" runat="server" CssClass="grid gridCount" style="display:none;"></asp:Label>   
   </asp:Panel> 
    <div id="fhrColorLegend" runat="server" class="fhrNote" visible="false" >
                <div class="fhrNoteBox"></div>
                <%= Resources.Resource.Label_FHR_Note %>
    </div>  