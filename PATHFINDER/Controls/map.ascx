<%@ Control Language="C#" AutoEventWireup="true" CodeFile="map.ascx.cs" Inherits="controls_map" %>

<%--<img runat="server" id="imgInfo" alt="info" src="~/images/information.png" style="display:none;z-index:900;position:absolute;top:30px;right:0px;" />--%>
<div id="mapTools" class="insetMap" runat="server" visible="false">
    <img src="content/images/previous.png" onclick="clientManager.restoreMap();" />
</div>
<dundas:MapControl runat="server" ID="map" 
    RenderingImageUrl="~/images/ajax-loader.gif" Width="280px" Height="175px" 
    GridUnderContent="True" Projection="Robinson" ResourceKey="#MapControlResKey#map#"
   
    ProjectionCenter-X="-97" ShadowIntensity="0" RenderType="ImageTag"
    BackColor="White" Font-Size="6pt">

    <Frame BackColor="54, 85, 116" BorderColor="Gainsboro" />
    <Parallels LabelColor="Gray" LineColor="224, 224, 224" Visible="false" ShowLabels="False" />
    <Shapes>
        <dundas:Shape  FieldData="STATE_FIPS=_x0035_3&amp;NAME=Washington&amp;ALT_NAME1=WA"
            Name="Washington" Color="#5390cf" />
        <dundas:Shape FieldData="STATE_FIPS=_x0033_0&amp;NAME=Montana&amp;ALT_NAME1=MT" Name="Montana"
            Color="#85c53d" />
        <dundas:Shape FieldData="STATE_FIPS=_x0032_3&amp;NAME=Maine&amp;ALT_NAME1=ME" Name="Maine"
            Color="#85c53d" />
        <dundas:Shape FieldData="STATE_FIPS=_x0033_8&amp;NAME=North_x0020_Dakota&amp;ALT_NAME1=ND"
            Name="North Dakota" Color="#5390cf" />
        <dundas:Shape FieldData="STATE_FIPS=_x0034_6&amp;NAME=South_x0020_Dakota&amp;ALT_NAME1=SD"
            Name="South Dakota" Color="#5390cf" />
        <dundas:Shape FieldData="STATE_FIPS=_x0035_6&amp;NAME=Wyoming&amp;ALT_NAME1=WY" Name="Wyoming"
            Color="#5390cf" />
        <dundas:Shape FieldData="STATE_FIPS=_x0035_5&amp;NAME=Wisconsin&amp;ALT_NAME1=WI"
            Name="Wisconsin" Color="#85c53d" />
        <dundas:Shape FieldData="STATE_FIPS=_x0031_6&amp;NAME=Idaho&amp;ALT_NAME1=ID" Name="Idaho"
            Color="#85c53d" />
        <dundas:Shape FieldData="STATE_FIPS=_x0035_0&amp;NAME=Vermont&amp;ALT_NAME1=VT" Name="Vermont" TextVisibility="Shown" CentralPointOffset-X="-0.5" CentralPointOffset-Y="1" 
            Color="#5390cf" />
        <dundas:Shape FieldData="STATE_FIPS=_x0032_7&amp;NAME=Minnesota&amp;ALT_NAME1=MN"
            Name="Minnesota" Color="#5390cf" />
        <dundas:Shape FieldData="STATE_FIPS=_x0034_1&amp;NAME=Oregon&amp;ALT_NAME1=OR" Name="Oregon"
            Color="#5390cf" />
        <dundas:Shape FieldData="STATE_FIPS=_x0033_3&amp;NAME=New_x0020_Hampshire&amp;ALT_NAME1=NH" TextVisibility="Shown" 
            Name="New Hampshire" Color="#85c53d" />
        <dundas:Shape FieldData="STATE_FIPS=_x0031_9&amp;NAME=Iowa&amp;ALT_NAME1=IA" Name="Iowa"
            Color="#5390cf" />
        <dundas:Shape FieldData="STATE_FIPS=_x0032_5&amp;NAME=Massachusetts&amp;ALT_NAME1=MA"
            Name="Massachusetts" Color="#85c53d"  TextVisibility="Shown" TextAlignment="BottomLeft" CentralPointOffset-X="5.25" CentralPointOffset-Y="2.25" />
        <dundas:Shape FieldData="STATE_FIPS=_x0033_1&amp;NAME=Nebraska&amp;ALT_NAME1=NE"
            Name="Nebraska" Color="#5390cf" />
        <dundas:Shape FieldData="STATE_FIPS=_x0033_6&amp;NAME=New_x0020_York&amp;ALT_NAME1=NY"
            Name="New York" Color="#5390cf" />
        <dundas:Shape FieldData="STATE_FIPS=_x0034_2&amp;NAME=Pennsylvania&amp;ALT_NAME1=PA"
            Name="Pennsylvania" Color="#5390cf" />
        <dundas:Shape FieldData="STATE_FIPS=_x0030_9&amp;NAME=Connecticut&amp;ALT_NAME1=CT"
            Name="Connecticut" Color="#85c53d" TextVisibility="Shown" TextAlignment="BottomLeft" CentralPointOffset-X="4.5"  />
        <dundas:Shape FieldData="STATE_FIPS=_x0034_4&amp;NAME=Rhode_x0020_Island&amp;ALT_NAME1=RI"
            Name="Rhode Island" Color="#85c53d"  TextVisibility="Shown" TextAlignment="BottomLeft" CentralPointOffset-X="3.75" CentralPointOffset-Y="1.5"/>
        <dundas:Shape FieldData="STATE_FIPS=_x0033_4&amp;NAME=New_x0020_Jersey&amp;ALT_NAME1=NJ" TextVisibility="Shown" 
            Name="New Jersey" Color="#5390cf" />
        <dundas:Shape FieldData="STATE_FIPS=_x0031_8&amp;NAME=Indiana&amp;ALT_NAME1=IN" Name="Indiana"
            Color="#5390cf" />
        <dundas:Shape FieldData="STATE_FIPS=_x0033_2&amp;NAME=Nevada&amp;ALT_NAME1=NV" Name="Nevada"
            Color="#85c53d" />
        <dundas:Shape FieldData="STATE_FIPS=_x0034_9&amp;NAME=Utah&amp;ALT_NAME1=UT" Name="Utah"
            Color="#e65c2e" />
        <dundas:Shape FieldData="STATE_FIPS=_x0030_6&amp;NAME=California&amp;ALT_NAME1=CA"
            Name="California" Color="#85c53d" />
        <dundas:Shape FieldData="STATE_FIPS=_x0033_9&amp;NAME=Ohio&amp;ALT_NAME1=OH" Name="Ohio"
            Color="#85c53d" />
        <dundas:Shape FieldData="STATE_FIPS=_x0031_7&amp;NAME=Illinois&amp;ALT_NAME1=IL"
            Name="Illinois" Color="#85c53d" />
        <dundas:Shape FieldData="STATE_FIPS=_x0031_1&amp;NAME=District_x0020_of_x0020_Columbia&amp;ALT_NAME1=DC"
            Name="District of Columbia" Color="#85c53d" TextVisibility="Shown" TextAlignment="BottomLeft" CentralPointOffset-X="6.5" />
        <dundas:Shape FieldData="STATE_FIPS=_x0031_0&amp;NAME=Delaware&amp;ALT_NAME1=DE" TextVisibility="Shown" TextAlignment="BottomLeft" CentralPointOffset-X="5.75" CentralPointOffset-Y="1.5"
            Name="Delaware" Color="#85c53d" />
        <dundas:Shape FieldData="STATE_FIPS=_x0035_4&amp;NAME=West_x0020_Virginia&amp;ALT_NAME1=WV"
            Name="West Virginia" Color="#5390cf" />
        <dundas:Shape FieldData="STATE_FIPS=_x0032_4&amp;NAME=Maryland&amp;ALT_NAME1=MD" TextVisibility="Shown" TextAlignment="BottomLeft" CentralPointOffset-X="5.5" CentralPointOffset-Y="-1.5" 
            Name="Maryland" Color="#5390cf" />
        <dundas:Shape FieldData="STATE_FIPS=_x0030_8&amp;NAME=Colorado&amp;ALT_NAME1=CO"
            Name="Colorado" Color="#85c53d" />
        <dundas:Shape FieldData="STATE_FIPS=_x0032_1&amp;NAME=Kentucky&amp;ALT_NAME1=KY"
            Name="Kentucky" Color="#85c53d" />
        <dundas:Shape FieldData="STATE_FIPS=_x0032_0&amp;NAME=Kansas&amp;ALT_NAME1=KS" Name="Kansas"
            Color="#5390cf" />
        <dundas:Shape FieldData="STATE_FIPS=_x0035_1&amp;NAME=Virginia&amp;ALT_NAME1=VA"
            Name="Virginia" Color="#5390cf" />
        <dundas:Shape FieldData="STATE_FIPS=_x0032_9&amp;NAME=Missouri&amp;ALT_NAME1=MO"
            Name="Missouri" Color="#5390cf" />
        <dundas:Shape FieldData="STATE_FIPS=_x0030_4&amp;NAME=Arizona&amp;ALT_NAME1=AZ" Name="Arizona"
            Color="#85c53d" />
        <dundas:Shape FieldData="STATE_FIPS=_x0034_0&amp;NAME=Oklahoma&amp;ALT_NAME1=OK"
            Name="Oklahoma" Color="#5390cf" />
        <dundas:Shape FieldData="STATE_FIPS=_x0033_7&amp;NAME=North_x0020_Carolina&amp;ALT_NAME1=NC"
            Name="North Carolina" Color="#5390cf" />
        <dundas:Shape FieldData="STATE_FIPS=_x0034_7&amp;NAME=Tennessee&amp;ALT_NAME1=TN"
            Name="Tennessee" Color="#5390cf" />
        <dundas:Shape FieldData="STATE_FIPS=_x0034_8&amp;NAME=Texas&amp;ALT_NAME1=TX" Name="Texas"
            Color="#85c53d" />
        <dundas:Shape FieldData="STATE_FIPS=_x0033_5&amp;NAME=New_x0020_Mexico&amp;ALT_NAME1=NM"
            Name="New Mexico" Color="#5390cf" />
        <dundas:Shape FieldData="STATE_FIPS=_x0030_1&amp;NAME=Alabama&amp;ALT_NAME1=AL" Name="Alabama"
            Color="#85c53d" />
        <dundas:Shape FieldData="STATE_FIPS=_x0032_8&amp;NAME=Mississippi&amp;ALT_NAME1=MS"
            Name="Mississippi" Color="#5390cf" />
        <dundas:Shape FieldData="STATE_FIPS=_x0031_3&amp;NAME=Georgia&amp;ALT_NAME1=GA" Name="Georgia"
            Color="#85c53d" />
        <dundas:Shape FieldData="STATE_FIPS=_x0034_5&amp;NAME=South_x0020_Carolina&amp;ALT_NAME1=SC"
            Name="South Carolina" Color="#5390cf" />
        <dundas:Shape FieldData="STATE_FIPS=_x0030_5&amp;NAME=Arkansas&amp;ALT_NAME1=AR"
            Name="Arkansas" Color="#5390cf" />
        <dundas:Shape FieldData="STATE_FIPS=_x0032_2&amp;NAME=Louisiana&amp;ALT_NAME1=LA"
            Name="Louisiana" Color="#85c53d" />
        <dundas:Shape FieldData="STATE_FIPS=_x0031_2&amp;NAME=Florida&amp;ALT_NAME1=FL" Name="Florida"
            Color="#85c53d" />
        <dundas:Shape FieldData="STATE_FIPS=_x0031_5&amp;NAME=Hawaii&amp;ALT_NAME1=HI" Name="Hawaii" TextVisibility="Shown" CentralPointOffset-X="1" CentralPointOffset-Y="1"
            Offset-X="51" Offset-Y="3" Color="#5390cf" ScaleFactor="1.0" />
        <dundas:Shape FieldData="STATE_FIPS=_x0030_2&amp;NAME=Alaska&amp;ALT_NAME1=AK" Name="Alaska"
            Offset-X="38" Offset-Y="-38" Color="#5390cf" ScaleFactor="0.4" />
        <dundas:Shape FieldData="STATE_FIPS=_x0032_6&amp;NAME=Michigan&amp;ALT_NAME1=MI" 
            Name="Michigan" Color="#85c53d" />
    </Shapes>
    <ShapeRules>
        <dundas:ShapeRule BorderColor="50, 50, 50, 50" FromColor="White" MiddleColor="" Name="ShapeRule1"
            ToColor="5, 100, 146" ColorPalette="Dundas">
        </dundas:ShapeRule>
    </ShapeRules>
    <ColorSwatchPanel BackColor="SteelBlue" BackGradientType="None" BackShadowOffset="0"
        BorderWidth="0" LabelColor="DimGray" TitleColor="DimGray">
        <Size Width="178" Height="78"></Size>
        <Location X="0" Y="79.4117661"></Location>
    </ColorSwatchPanel>
    <ShapeFields>
        <dundas:Field Name="ALT_NAME1" Type="System.String" />
    </ShapeFields>
    <Meridians LabelColor="Gray" LineColor="224, 224, 224" Visible="False" ShowLabels="False" />
</dundas:MapControl>
