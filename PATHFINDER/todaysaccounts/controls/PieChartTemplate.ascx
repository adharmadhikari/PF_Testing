<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PieChartTemplate.ascx.cs" Inherits="todaysaccounts_controls_PieChartTemplate" %>
 <DCWC:Chart ID="Chart1" runat="server" Height="560px" Width="620px"
 EnableTheming="True" AutoSize="true" ImageType="Flash" BackColor="Transparent" RepeatDelay="0" Palette="Pastel">
       <%-- <Legends>
            <DCWC:Legend Name="Legend1" Docking="Top" 
        </Legends>--%>
        <Legends>
<DCWC:Legend Name="Default" Alignment="Center" Docking="Bottom"></DCWC:Legend>
</Legends>
        <Titles>
            <DCWC:Title Name="Title1" Text="Lives Distribution"  Color="DarkBlue" Font="Verdana, 11pt">
            </DCWC:Title>
        </Titles>
        <Series>
            <DCWC:Series Name="Series1" />
        </Series>
        <ChartAreas>
            <DCWC:ChartArea Name="Default" Area3DStyle-Enable3D="true">
<Area3DStyle Enable3D="True" ></Area3DStyle>
            </DCWC:ChartArea>
        </ChartAreas>
    </DCWC:Chart>
     