<%@ Control Language="C#" AutoEventWireup="true" CodeFile="SellSheetSteps.ascx.cs" Inherits="custom_controls_SellSheetSteps" %>

<div id="sellSheetSteps" runat="server" class="sellSheetSteps">
    <ul>
        <asp:Repeater runat="server" ID="repeater" DataSourceID="dsSteps">
            <ItemTemplate>        
                <li class='<%#Eval("Step_Order","step{0}") %>'>
                    <a href="javascript:void(0)" onclick="clientManager.set_Module('<%# Eval("Step_Key") %>')" 
                        class='<%# this.GetClassName(Eval("Step_Key") as String) %>' 
                        stepKey='<%# Eval("Step_Key") %>' 
                        stepOrder='<%# Eval("Step_Order") %>'>
                        <div class="stepNumber"><asp:Literal runat="server" ID="litStep" Text='<%# Eval("Step_Order") %>'  /></div>
                        <div class="stepText"><asp:Literal runat="server" ID="Label1" Text='<%# Eval("Step_Short_Name") %>' /></div>
                        <%--<asp:Literal runat="server" ID="litStepName" Text='<%# Eval("Step_Name") %>' />--%>
                    </a>
                </li>                     
            </ItemTemplate>
        </asp:Repeater>
    </ul>
    <div class="clearAll"></div>
</div>
<div id="ssStepSpacer"></div>
<span id="ssStepTitle"><asp:Label runat="server" ID="lblStepTitle" /></span>
<div id="ssStepSpacer"></div>
<span id="ssStepDescription"><asp:Label runat="server" ID="lblStepDescription" /></span>
<div id="ssStepSpacer"></div>
<%-- output Tip to load into side panel - this is not displayed on the page but text is used to update panel --%>
<div id="stepTip" style="display:none">
    <asp:Literal runat="server" ID="litStepTip" />
</div>
<%-- output Template ID to load into side panel - this is not displayed on the page but text is used to initialize the start position of the template selector --%>
<div id="sidebarTemplateID" style="display:none">
    <asp:Literal runat="server" ID="litTemplateID" />
</div>
<div id="sidebarTheraID" style="display:none">
    <asp:Literal runat="server" ID="litTheraID" />
</div>
<div id="sidebarStepOrder" style="display:none;">
    <asp:Literal runat="server" ID="litStepNum" />
</div>
<asp:EntityDataSource runat="server" ID="dsSteps" EntitySetName="SellSheetStepSet" DefaultContainerName="PathfinderClientEntities" OrderBy="it.Step_Order">
</asp:EntityDataSource>
