<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ccrChartTemplate.ascx.cs" Inherits="custom_controls_ccrChartTemplate" %>

<DCWC:Chart  ID="chart" runat="server"   Height="265px" Width="255px"
 EnableTheming="True" ImageType="Flash" BackColor="Transparent" RepeatDelay="0" Palette="Pastel" >
<%--     <Legends  >
        <DCWC:Legend BackColor="Transparent" 
                    BorderStyle="NotSet" Font="Trebuchet MS, 8pt" Name="Default" 
                    ShadowColor="Transparent" AutoFitMinFontSize="8" LegendStyle="Column"  Alignment="Center" AutoFitText="true">           
             
        </DCWC:Legend>        
    </Legends>  --%>                  
    <Titles>
        <DCWC:Title Color="DarkBlue" Font="Verdana, 11pt" Name="Title1" />
    </Titles>
    <Series >
        <DCWC:Series Name="Series1" />

    </Series>     
    <ChartAreas>
         <DCWC:ChartArea Name="Default" Area3DStyle-Enable3D="true">                                                                  
        </DCWC:ChartArea>
    </ChartAreas>
</DCWC:Chart>