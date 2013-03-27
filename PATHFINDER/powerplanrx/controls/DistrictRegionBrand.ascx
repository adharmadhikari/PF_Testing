<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DistrictRegionBrand.ascx.cs" Inherits="controls_DistrictRegionBrand" %>
<DCWC:Chart ID="ChartAll" runat="server" Palette="Pastel" 
    Height="400px" Width="800px"  
    ImageStorageMode="UseHttpHandler" 
    EnableTheming="True" ImageType="Flash" 
    BackColor="Transparent" RepeatDelay="0">
    <Legends>
        <DCWC:Legend Name="Default" Enabled="false">
        </DCWC:Legend>
    </Legends>
    <Titles>
        <DCWC:Title Name="Title1">
        </DCWC:Title>
    </Titles>
    <Series>
        <DCWC:Series  ChartType="Doughnut"              
            CustomAttributes="DoughnutRadius=50, PieLabelStyle=Outside,3DLabelLineSize=30" Name="Series1" 
             ShadowOffset="1" ShowLabelAsValue="false" 
             >             
        </DCWC:Series>        
    </Series>
    <ChartAreas>
        <DCWC:ChartArea BorderColor="" Name="Default">
            <Area3DStyle Enable3D="True" Light="Realistic" />
        </DCWC:ChartArea>
    </ChartAreas>
</DCWC:Chart>


 