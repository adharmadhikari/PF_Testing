<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ChartTemplate.ascx.cs" Inherits="custom_controls_ccrChartTemplate" %>

<DCWC:Chart  ID="chart" runat="server"   Height="400px" Width="1200px"
 EnableTheming="True" ImageType="Flash" BackColor="Transparent" RepeatDelay="0" Palette="Pastel" >
    <Legends>
        <DCWC:Legend BackColor="Transparent"  Enabled="false"
            BorderStyle="NotSet" Font="Trebuchet MS, 10pt" Name="Default" 
            ShadowColor="Transparent" AutoFitMinFontSize="8" MaxAutoSize="50"
             Alignment="Center" AutoFitText="true" Docking="Bottom" 
            LegendStyle="Table" TextWrapThreshold="0" DockInsideChartArea="false" TableStyle="Wide" >             
        </DCWC:Legend>        
    </Legends>                 
    <Titles>
        <DCWC:Title Color="DarkBlue" Font="Verdana, 11pt" Name="Title1" />
    </Titles>
    <Series >
        <DCWC:Series Name="Series1" FontColor="DarkBlue"  
        CustomAttributes="DrawingStyle=Cylinder" />

    </Series>     
    <ChartAreas>
         <DCWC:ChartArea BackColor="Transparent" BackGradientEndColor="Transparent" 
        BackGradientType="TopBottom" BackHatchStyle="None" BackImageAlign="TopLeft"
        BackImageMode="Tile" BorderWidth="1" ShadowColor="Transparent"
        BorderStyle="NotSet"  Name="Default" >
            <AxisY Title="Hours" TitleFont="Verdana, 11pt, style=Bold"  LabelsAutoFit="false" 
                TitleColor="94, 94, 94" StartFromZero="True" Margin="false" LabelsAutoFitMinFontSize="5" LabelsAutoFitMaxFontSize="11"  >
                <MajorGrid LineColor="64, 64, 64, 64" />
                <MinorGrid LineColor="Black" LineWidth="1" Enabled="false" />
                <MajorTickMark LineColor="Black" LineWidth="1"  />
                <MinorTickMark LineColor="Black" Size="1" Enabled="false" />
                <LabelStyle Font="Verdana, 11pt" />
            </AxisY>
            <AxisX LabelsAutoFitStyle="WordWrap" LabelsAutoFit="true" LabelsAutoFitMinFontSize="5" LabelsAutoFitMaxFontSize="11">
                <MajorGrid Enabled="False" LineColor="64, 64, 64, 64" />
                <MajorTickMark Enabled="false" />
                <LabelStyle Font="Verdana MS, 5pt"   TruncatedLabels="false"  />
            </AxisX>
            <%--<Position Height="68" Width="100" X="2.5" Y="16.53773" />
            <InnerPlotPosition Height="90.73529" Width="100" X="11.78636" 
                Y="4.632347" />
            <Position Height="100" Width="100" X="5" Y="5" />
            <InnerPlotPosition Height="100" Width="100" />--%>
            <Area3DStyle Clustered="True" Perspective="10" RightAngleAxes="False" 
                WallWidth="0" XAngle="15" YAngle="10" />                                                      
        </DCWC:ChartArea>
    </ChartAreas>
</DCWC:Chart>