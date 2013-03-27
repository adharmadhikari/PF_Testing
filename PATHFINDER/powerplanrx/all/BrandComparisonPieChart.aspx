<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BrandComparisonPieChart.aspx.cs" Inherits="BrandComparisonPieChart" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div class="BrandComparison">
        <DCWC:Chart ID="Chart1" runat="server" DataSourceID="dsBrandComparison" Palette="Pastel"
            ImageType="Jpeg" Width="600px" Height="300px" ImageStorageMode="UseHttpHandler"  >
            <Legends>
                <DCWC:Legend Name="Default" Enabled="false">
                </DCWC:Legend>
            </Legends>
            <Titles>
                <DCWC:Title Name="Title1" Text="Brand Comparison Pie Chart" 
                    Font="Arial, 8pt, style=Bold">
                </DCWC:Title>
            </Titles>
            <Series>
                <DCWC:Series ChartType="Pie" Label="#VALX - #VAL{P2}" 
                    Name="Default" ShadowOffset="1" ValueMembersY="Brand_Mst" 
                    ValueMemberX="Brand_Name" 
                    CustomAttributes="PieLabelStyle=Outside, PieLineColor=InfoText" 
                    ShowLabelAsValue="False" ToolTip="#VALX - #VAL{P2}">
                </DCWC:Series>
            </Series>
            <ChartAreas>
                <DCWC:ChartArea Name="Default" BorderColor="">                  
                </DCWC:ChartArea>
            </ChartAreas>
        </DCWC:Chart>  
        <asp:GridView ID="grvBrandComparison" runat="server" AutoGenerateColumns="False" DataSourceID="dsBrandComparison" Width="85%" HorizontalAlign="Center">
            <Columns>            
                <asp:BoundField DataField="Brand_Name" HeaderText="Brand Name" HeaderStyle-CssClass="headerList" ItemStyle-CssClass="itemList" ItemStyle-HorizontalAlign="Left"  ItemStyle-Width="20%"/>
                <asp:BoundField DataField="Brand_Trx" HeaderText="Brand Trx"  DataFormatString="{0:n0}" HeaderStyle-CssClass="headerList" ItemStyle-CssClass="itemList" ItemStyle-HorizontalAlign="Right"  ItemStyle-Width="10%"/> 
                <asp:TemplateField HeaderText="Brand Market Share (%)" HeaderStyle-CssClass="headerList" ItemStyle-CssClass="itemList" ItemStyle-HorizontalAlign="Right"  ItemStyle-Width="5%">
                    <ItemTemplate>
                        <asp:Label ID="lblBrandMarketShare" runat="server" 
                            Text='<%# (string.Concat(Eval("Brand_Mst","{0:n2}"),"%")) %>'></asp:Label>
                    </ItemTemplate>           
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        
    
        <asp:SqlDataSource ID="dsBrandComparison" runat="server" 
            ConnectionString="<%$ ConnectionStrings:PathfinderClientDB_Format %>"
            SelectCommand="pprx.usp_GetCampaignBrandComparisonInfo"
            SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:QueryStringParameter Name="Plan_ID" QueryStringField="plan_id" />
                <asp:QueryStringParameter Name="MB_ID" QueryStringField="mb_id" /> 
                <asp:QueryStringParameter Name="Segment_ID" QueryStringField="segment_id" />                   
            </SelectParameters>
        </asp:SqlDataSource>
    
    </div>
    </form>
</body>
</html>
