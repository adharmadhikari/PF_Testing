<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DistrictProfileChart.ascx.cs" Inherits="controls_DistrictProfileChart" %>
<div class ="districtProfileChart">
<DCWC:Chart ID="ChartComm" runat="server" Palette="Pastel" 
    Height="400px" Width="850px"  
    ImageStorageMode="UseHttpHandler" ImageType="Jpeg">
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

<DCWC:Chart ID="ChartPartD" runat="server" Palette="Pastel" 
    Height="400px" Width="850px" 
    ImageStorageMode="UseHttpHandler" ImageType="Jpeg" CssClass="districtProfileChart">
    <Legends>
        <DCWC:Legend Name="Default" Enabled="false">
        </DCWC:Legend>
    </Legends>
    <Titles>
        <DCWC:Title Name="Title1">
        </DCWC:Title>
    </Titles>
    <Series>
        <DCWC:Series ChartType="Doughnut" 
             CustomAttributes="DoughnutRadius=50, PieLabelStyle=Outside,3DLabelLineSize=30" Name="Series1" 
             ShadowOffset="1" ShowLabelAsValue="false"              
              >
        </DCWC:Series>
    </Series>
    <ChartAreas>
        <DCWC:ChartArea BorderColor="" Name="Default" >
            <Area3DStyle Enable3D="True" Light="Realistic" />
        </DCWC:ChartArea>       
    </ChartAreas>
</DCWC:Chart>
</div>
