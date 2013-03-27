<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TierCoveragePieChartTemplate.ascx.cs" Inherits="standardreports_controls_TierCoveragePieChartTemplate" %>
 <DCWC:Chart ID="Chart1" runat="server" Height="500px" Width="650px"
 EnableTheming="True" BackColor="Transparent" RepeatDelay="0" AutoSize="true" >
      <Legends>
        <DCWC:Legend LegendStyle="Row" Docking="Bottom" Name="Default1">
        </DCWC:Legend>
        <DCWC:Legend LegendStyle="Column" Docking="Right" Name="Default2">
        </DCWC:Legend>
      </Legends>
        <Titles>
            <DCWC:Title Name="Title1" Text="Formulary Status">
            </DCWC:Title>
        </Titles>
        <Series>
        </Series>
        <ChartAreas>
            <DCWC:ChartArea BorderColor="" Name="Default">
                <Area3DStyle Light="Realistic"  Clustered="true"  />
            </DCWC:ChartArea>
        </ChartAreas>
    </DCWC:Chart>
     