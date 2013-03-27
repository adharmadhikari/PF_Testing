<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ChartTemplateTrx.ascx.cs" Inherits="custom_warner_formularyhistoryreporting_controls_ChartTemplateTrx" %>
<DCWC:Chart ID="chart" runat="server" Height="400px" Width="800px" ImageStorageMode="UseHttpHandler" 
    EnableTheming="True"   ImageType="Flash" 
    BackColor="Transparent" RepeatDelay="0"
    PaletteCustomColors="0, 202, 56; 248, 206, 12; 237, 0, 38; 50, 90, 255">
     <Legends>
                <DCWC:Legend AutoFitText="False" BackColor="Transparent" Docking="Bottom" 
                    Alignment="Center" TextWrapThreshold="40"
                    BorderStyle="NotSet" Font="Arial, 10pt" Name="Default" 
                    ShadowColor="Transparent" TableStyle="Wide" >
                </DCWC:Legend>
     </Legends>      
                        
    <Titles>
                <DCWC:Title Color="DarkBlue" Font="Arial, 14pt" Name="Title1">
                </DCWC:Title>
            </Titles>
             <Titles>
                <DCWC:Title Color="DarkBlue" Font="Arial, 14pt" Name="Title2">
                </DCWC:Title>
            </Titles>
    <Series>
           <DCWC:Series Name="Series1" Font="Arial, 8pt" FontColor="DarkBlue"  
            CustomAttributes="DrawingStyle=Cylinder"/>
           <DCWC:Series Name="Series2" Font="Arial, 8pt" FontColor="DarkBlue"  
            CustomAttributes="DrawingStyle=Cylinder"/>
    </Series>     
    <ChartAreas>
        <DCWC:ChartArea BackColor="Transparent" BackGradientEndColor="Transparent" 
        BackGradientType="TopBottom" BackHatchStyle="None" BackImageAlign="TopLeft"
        BackImageMode="Tile" BorderWidth="1" ShadowColor="Transparent"
        BorderStyle="NotSet"  Name="Default">                                                                      
            <AxisY TitleFont="Arial, 9pt, style=Bold"  LabelsAutoFit="false" 
                Title='<%$ Resources:Resource, Label_TierCoverage_PharmacyLives %>'  TitleColor="94, 94, 94" StartFromZero="True" Margin="true">
                <MajorGrid LineColor="64, 64, 64, 64" />
                <MinorGrid LineColor="Black" LineWidth="1" Enabled="false" />
                <MajorTickMark LineColor="Black" LineWidth="1"  />
                <MinorTickMark LineColor="Black" Size="1" Enabled="false" />
                <LabelStyle Font="Arial, 10pt" />
            </AxisY>
            <AxisX TitleColor="94, 94, 94" LabelsAutoFit="true" 
                TitleFont="Arial, 8pt">
                <MajorGrid Enabled="False" LineColor="64, 64, 64, 64" />
                <MajorTickMark Style="None" />
                <LabelStyle Font="Arial, 11pt" />
            </AxisX>
            <%--<Position Height="68" Width="89.30075" X="2.5" Y="16.53773" />
            <InnerPlotPosition Height="90.73529" Width="88.21363" X="11.78636" 
                Y="4.632347" />--%>
            <%--<Area3DStyle Clustered="True" Perspective="10" RightAngleAxes="False" 
                WallWidth="0" XAngle="15" YAngle="10" />--%>
        </DCWC:ChartArea>
    </ChartAreas>
     
</DCWC:Chart>
