<%@ Control Language="C#" AutoEventWireup="true" CodeFile="RestrictionsReportChartTemplate.ascx.cs" Inherits="restrictionsreport_controls_MedicalPharmacyCoverageChartTemplate" %>

<DCWC:Chart ID="chart" runat="server"   Height="400px" Width="800px"
 EnableTheming="True" ImageType="Flash" 
    BackColor="Transparent" RepeatDelay="0">
     <Legends >
        <DCWC:Legend BackColor="Transparent"
                    BorderStyle="NotSet" Font="Arial, 10pt" Name="Default" 
                    ShadowColor="Transparent" LegendStyle="Table"  Alignment="Center" Docking="Bottom">           
             
        </DCWC:Legend>        
    </Legends>                    
    <Titles>
        <DCWC:Title Color="DarkBlue" Font="Arial, 14pt" Name="Title1" />
    </Titles>
    <Series>
        <DCWC:Series ChartType="StackedColumn" Name="Series1" />
        <DCWC:Series ChartType="StackedColumn" Name="Series2" />
        <DCWC:Series ChartType="StackedColumn" Name="Series3" />
        <DCWC:Series ChartType="StackedColumn" Name="Series4" />
    </Series>     
    <ChartAreas>
        <DCWC:ChartArea  BackColor="Transparent" BackGradientEndColor="Transparent" BackGradientType="TopBottom" BackHatchStyle="None" BackImageAlign="TopLeft"
                            BackImageMode="Tile" BorderWidth="1" ShadowColor="Transparent"
                             BorderStyle="NotSet"  Name="Default">                                                                      
            <AxisY TitleFont="Arial, 9pt, style=Bold" LabelsAutoFitMinFontSize="9" LabelsAutoFitMaxFontSize="10"  LabelsAutoFit="True" 
                Title='<%$ Resources:Resource, Label_TierCoverage_PharmacyLives %>' 
                TitleColor="94, 94, 94">
                <MinorGrid LineColor="Black" LineWidth="1" Enabled="false" />
                <MajorTickMark LineColor="Black" LineWidth="1"  />
                <MinorTickMark LineColor="Black" Size="1" Enabled="false" />
                <MajorGrid LineColor="64, 64, 64, 64" />
                <LabelStyle Font="Arial, 10pt" />
            </AxisY>
            <AxisX LabelsAutoFit="True" LabelsAutoFitMinFontSize="8" LabelsAutoFitMaxFontSize="12" >                            
                <MinorGrid LineColor="Black" LineWidth="1" Enabled="false" />
                <MajorTickMark LineColor="Black" LineWidth="1" LineStyle="Solid" />
                <MinorTickMark LineColor="Black" Size="1" Enabled="false" />
                <MajorGrid LineColor="64, 64, 64, 64" />
                <LabelStyle Font="Arial, 11pt" />
            </AxisX>
        </DCWC:ChartArea>
    </ChartAreas>
</DCWC:Chart>