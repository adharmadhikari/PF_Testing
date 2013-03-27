<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DistrictRegionBrand_All.ascx.cs" Inherits="controls_DistrictRegionBrand_All" %>
<asp:FormView ID="frmHeader" runat="server" DataSourceID="dsReport" CssClass="DistrictProfile">
    <ItemTemplate>      
        <asp:Label ID="lblBrandName" runat="server" Text='<%#(string.Format("Brand Name: {0}",Eval("PP_Brand_Name")))%>' ></asp:Label><br>
        <asp:Label ID="lblSegmentName" runat="server" Text='<%#(string.Format("Segment: {0}",Eval("Segment_Name")))%>' ></asp:Label>
    </ItemTemplate>
</asp:FormView>

<br />

<asp:GridView ID="grvRegion" runat="server" AutoGenerateColumns="false" DataSourceID="dsReport" Visible="false" CssClass="DistrictProfile">
    <Columns>        
        <asp:BoundField DataField="District_Name" HeaderText="District" HeaderStyle-CssClass="headerList" Visible="false" ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="itemList"/>
        <asp:BoundField DataField="Plan_Name" HeaderText="Top 5 Accounts in the Region" HeaderStyle-CssClass="headerList" ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="itemList"/>
        <asp:BoundField DataField="Brand_Trx" HeaderText="TRx Volume" DataFormatString="{0:n0}" Visible="false" HeaderStyle-CssClass="headerList" ItemStyle-HorizontalAlign="Right" ItemStyle-CssClass="itemList"/>
        <asp:TemplateField HeaderText="Percentage (%)" Visible="false" HeaderStyle-CssClass="headerList" ItemStyle-HorizontalAlign="Right" ItemStyle-CssClass="itemList">
            <ItemTemplate>
                <asp:Label ID="lblBrandPercentComm" runat="server" 
                    Text='<%# (string.Format("{0}{1}",Eval("PercentBrandTrxRegionTotal","{0:n2}"),"%")) %>'></asp:Label>
            </ItemTemplate>           
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Percentage (%)" Visible="false" HeaderStyle-CssClass="headerList" ItemStyle-HorizontalAlign="Right" ItemStyle-CssClass="itemList">
            <ItemTemplate>
                <asp:Label ID="lblMBPercentComm" runat="server" 
                    Text='<%# (string.Format("{0}{1}",Eval("PercentMBTrxRegionTotal","{0:n2}"),"%")) %>'></asp:Label>
            </ItemTemplate>           
        </asp:TemplateField>
        <asp:BoundField DataField="MB_Trx" HeaderText="Market Trx" DataFormatString="{0:n0}" HeaderStyle-CssClass="headerList" ItemStyle-HorizontalAlign="Right" ItemStyle-CssClass="itemList"/>
    </Columns>
</asp:GridView>

<asp:GridView ID="grvDistrict" runat="server" AutoGenerateColumns="False" DataSourceID="dsReport" Visible="false" CssClass="DistrictProfile">
    <Columns>       
        <asp:BoundField DataField="Plan_Name" HeaderText="Top 5 Accounts in the District" HeaderStyle-CssClass="headerList" ItemStyle-HorizontalAlign="Left"/>
        <asp:BoundField DataField="Brand_Trx" HeaderText="TRx Volume"  DataFormatString="{0:n0}" Visible="false" HeaderStyle-CssClass="headerList" ItemStyle-HorizontalAlign="Right"/>
        <asp:TemplateField HeaderText="Percentage (%)" Visible="false" HeaderStyle-CssClass="headerList" ItemStyle-HorizontalAlign="Right">
            <ItemTemplate>
                <asp:Label ID="lblBrandPercentComm" runat="server" 
                    Text='<%# (string.Format("{0}{1}",Eval("PercentBrandTrxDistrictTotal","{0:n2}"),"%")) %>'></asp:Label>
            </ItemTemplate>           
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Percentage (%)" Visible="false" HeaderStyle-CssClass="headerList" ItemStyle-HorizontalAlign="Right">
            <ItemTemplate>
                <asp:Label ID="lblMBPercentComm" runat="server" 
                    Text='<%# (string.Format("{0}{1}",Eval("PercentMBTrxDistrictTotal","{0:n2}"),"%")) %>'></asp:Label>
            </ItemTemplate>           
        </asp:TemplateField>
        <asp:BoundField DataField="MB_Trx" HeaderText="Market Trx" DataFormatString="{0:n0}" HeaderStyle-CssClass="headerList" ItemStyle-HorizontalAlign="Right"/>
    </Columns>
</asp:GridView>


<asp:GridView ID="grvOther" runat="server" AutoGenerateColumns="False" DataSourceID="dsOther" Visible="false" CssClass="DistrictProfile">
    <Columns>        
        <asp:TemplateField HeaderText="All Other Accounts in the District" HeaderStyle-CssClass="headerList" ItemStyle-HorizontalAlign="Left">
            <ItemTemplate><asp:Label ID="lblOtherAccounts" Text="All Other Plans" runat="server"></asp:Label></ItemTemplate>
        </asp:TemplateField>  
        <asp:BoundField DataField="OtherBrandTrxTotal" HeaderText="TRx Volume" DataFormatString="{0:n0}" HeaderStyle-CssClass="headerList" ItemStyle-HorizontalAlign="Right"/>
        <asp:TemplateField HeaderText="Percentage (%)" HeaderStyle-CssClass="headerList" ItemStyle-HorizontalAlign="Right">
            <ItemTemplate>
                <asp:Label ID="lblPercentOther" runat="server" 
                    Text='<%# (string.Format("{0}{1}",Eval("PercentBrandTrxDistrictTotal","{0:n2}"),"%")) %>'></asp:Label>
            </ItemTemplate>           
        </asp:TemplateField>
        <asp:BoundField DataField="OtherMarketTrxTotal" HeaderText="Market Trx" DataFormatString="{0:n0}" HeaderStyle-CssClass="headerList" ItemStyle-HorizontalAlign="Right"/>  
    </Columns>
</asp:GridView>


<asp:GridView ID="grvPartD" runat="server" AutoGenerateColumns="False" DataSourceID="dsPartD" Visible="false" CssClass="DistrictProfile">
    <Columns>          
        <asp:BoundField DataField="Plan_Name" HeaderText="Top 5 Medicare Part D Accounts in the District" HeaderStyle-CssClass="headerList" ItemStyle-HorizontalAlign="Left"/>
        <asp:BoundField DataField="Brand_Trx" HeaderText="TRx Volume" DataFormatString="{0:n0}" HeaderStyle-CssClass="headerList" ItemStyle-HorizontalAlign="Right"/>
        <asp:TemplateField HeaderText="Percentage (%)" HeaderStyle-CssClass="headerList" ItemStyle-HorizontalAlign="Right">
            <ItemTemplate>
                <asp:Label ID="lblPercentPartD" runat="server" 
                    Text='<%# (string.Format("{0}{1}",Eval("PercentBrandTrxDistrictTotal","{0:n2}"),"%")) %>'></asp:Label>
            </ItemTemplate>           
        </asp:TemplateField>
        <asp:BoundField DataField="MB_Trx" HeaderText="Market Trx" DataFormatString="{0:n0}" HeaderStyle-CssClass="headerList" ItemStyle-HorizontalAlign="Right"/>
    </Columns>   
</asp:GridView>


<asp:SqlDataSource ID="dsReport" runat="server" 
    ConnectionString='<%$ConnectionStrings:PathfinderClientDB_Format %>'
    ProviderName="System.Data.SqlClient" 
    SelectCommand="usp_GetSV_BaseByRegionIDDistrictIDBrandID" 
    SelectCommandType="StoredProcedure">
    <SelectParameters>
        <asp:QueryStringParameter DefaultValue="0" Name="Type_ID" 
            QueryStringField="regionid" Type="String" />
        <asp:QueryStringParameter DefaultValue="0" Name="Region_ID" 
            QueryStringField="regionid" Type="String" />
        <asp:QueryStringParameter DefaultValue="0" Name="District_ID" 
            QueryStringField="dist" Type="String" />
        <asp:QueryStringParameter DefaultValue="0" Name="Brand_ID" 
            QueryStringField="brandid" Type="Int32" />
        <asp:QueryStringParameter DefaultValue="1" Name="Segment_ID" 
            QueryStringField="segment" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>




<asp:SqlDataSource ID="dsOther" runat="server" 
    ConnectionString='<%$ConnectionStrings:PathfinderClientDB_Format %>'
    ProviderName="System.Data.SqlClient" 
    SelectCommand="usp_GetSV_Base_Other" 
    SelectCommandType="StoredProcedure">
    <SelectParameters>
        <asp:QueryStringParameter DefaultValue="0" Name="District_ID" 
            QueryStringField="dist" Type="String" />
        <asp:QueryStringParameter DefaultValue="0" Name="Campaign_ID" 
            QueryStringField="id" Type="Int32" />        
    </SelectParameters>
</asp:SqlDataSource>

<asp:SqlDataSource ID="dsPartD" runat="server" 
    ConnectionString='<%$ConnectionStrings:PathfinderClientDB_Format %>'
    ProviderName="System.Data.SqlClient" 
    SelectCommand="usp_GetSV_BaseByDistrictIDBrandID" 
    SelectCommandType="StoredProcedure">
    <SelectParameters>
        <asp:QueryStringParameter DefaultValue="0" Name="District_ID" 
            QueryStringField="dist" Type="String" />
        <asp:QueryStringParameter DefaultValue="0" Name="Brand_ID" 
            QueryStringField="brandid" Type="Int32" />
        <asp:Parameter DefaultValue="2" Name="Segment_ID" Type="Int32" />
    </SelectParameters>
</asp:SqlDataSource>




