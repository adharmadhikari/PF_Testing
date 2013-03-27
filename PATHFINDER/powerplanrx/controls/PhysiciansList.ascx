<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PhysiciansList.ascx.cs" Inherits="controls_PhysiciansList" %>

        <asp:FormView runat="server" ID="frmPhysician" CssClass="physList"  >
            <ItemTemplate>            
                <asp:Label runat="server" ID="Label1" Text="District:" />
                <asp:Label runat="server" ID="lblDistrict" Text='<%# (Eval("District_ID").ToString()+ Eval("District_Name").ToString()) %>' />&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp; 
                <asp:Label runat="server" ID="Label2" Text="Brand:" />
                <asp:Label runat="server" ID="lblBrandName" Text='<%# Eval("Brand_Name") %>' />&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp; 
                <asp:Label runat="server" ID="Label3" Text="Plan Name:" />
                <asp:Label runat="server" ID="lblPlanName" Text='<%# Eval("Plan_Name") %>' />&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;  
                <asp:Label runat="server" ID="Label4" Text="Data Month:" />
                <asp:Label runat="server" ID="lblDataMonthYear" Text='<%# (Eval("Data_Month").ToString() + " " + Eval("Data_Year").ToString()) %>' />
                <br />       
            </ItemTemplate>
            <EmptyDataTemplate>No records present.</EmptyDataTemplate>
        </asp:FormView>
        <asp:DataGrid ID="grdPhysicians" runat="server" HorizontalAlign="Center" AutoGenerateColumns="false" OnItemDataBound="grdPhysicians_ItemDataBound"  >
        
            <Columns>           
                <asp:BoundColumn DataField="Territory_ID" HeaderText="" HeaderStyle-CssClass="headerList" ItemStyle-CssClass="itemList" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="20%"/>
                <asp:BoundColumn DataField="NNPI_ID" HeaderText="" HeaderStyle-CssClass="headerList" ItemStyle-CssClass="itemList" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="20%"/>            
                <asp:BoundColumn DataField="First_Name" HeaderText="" HeaderStyle-CssClass="headerList" ItemStyle-CssClass="itemList" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="10%"/>
                <asp:BoundColumn DataField="MB_Trx" HeaderText="" HeaderStyle-CssClass="headerList" ItemStyle-CssClass="itemList" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="10%"/>               
                <asp:BoundColumn DataField="Brand_Trx" HeaderText="" HeaderStyle-CssClass="headerList" ItemStyle-CssClass="itemList" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="10%"/>               
        </Columns>         
        </asp:DataGrid>  


       
         