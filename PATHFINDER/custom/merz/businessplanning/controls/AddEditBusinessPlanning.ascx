<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AddEditBusinessPlanning.ascx.cs" Inherits="custom_merz_businessplanning_controls_AddEditBusinessPlanning" %>
<%@ Register src="~/custom/merz/businessplanning/controls/KeyContacts.ascx" tagname="Contacts" tagprefix="merz" %>
<%@ Register src="~/custom/merz/businessplanning/controls/PBMAffiliations.ascx" tagname="PBM" tagprefix="merz" %>
<%@ Register src="~/custom/merz/businessplanning/controls/DermatologyCoverage.ascx" tagname="DermCoverage" tagprefix="merz" %>


<div class="bpTile">
    <div class="tileContainerHeader"><div class="title">Key Contacts</div></div>
    <div class="dashboardTable">
        <merz:Contacts ID="Contacts1" runat="server"/>
    </div>
</div>

<asp:HiddenField ID="frmvmMode" runat="server" Value="readonly" />

<asp:FormView  runat="server" ID="formVWBP" DefaultMode="ReadOnly"      
           DataSourceID="dsBusinessPlanning" CellPadding="0" CellSpacing="0" Width="100%" DataKeyNames="Business_Plan_ID" valign="top">
  <EmptyDataTemplate>
        <asp:HiddenField ID="MedCEnrollment" Value="" runat="server" />

        <div class="bpTile" id="AffiliationsDiv" runat="server" >
            <div class="tileContainerHeader"><div class="title">Affiliations</div></div>
            <div class="leftTile dashboardTable leftBgPDFTile pbmTile" id="PBMDiv" runat="server" >
               <merz:PBM ID="PBM1" runat="server"/>
            </div>
         
            <div class="rightTile dashboardTable sppPDFTile sppTile"  id="SPPDiv" runat="server">
                <asp:GridView ID="grdvwSPPAffliations" runat="server" AutoGenerateColumns="False" DataSourceID="dsSPPAffiliations"
                    Width="100%" GridLines="Both" BorderStyle="None" CssClass="staticTable"  DataKeyNames="SPP_ID" SkinID="basic">
                    <%-- Parent_ID = SPP_ID, will be stored in Business_Plan_Preferred_SPP table--%>
                    <Columns>
                        <asp:BoundField DataField="Plan_Name" HeaderText="SPP Name" Visible="true" ItemStyle-CssClass="firstCol firstGeneric" HeaderStyle-CssClass="firstCol firstGeneric"  />
                        <asp:CheckBoxField DataField="Preferred_SPP"  HeaderText="Preferred" ReadOnly="true" /> 
                    </Columns>
                    <EmptyDataTemplate>
                        <asp:Label ID="Label1" text="No records present for SPP." runat="server"></asp:Label>
                    </EmptyDataTemplate>
                </asp:GridView>
                <asp:EntityDataSource ID="dsSPPAffiliations" runat="server" ConnectionString="name=PathfinderMerzEntities"
                DefaultContainerName="PathfinderMerzEntities" EntitySetName="PlanAffiliationsForSPPSet"
                AutoGenerateWhereClause="true" OrderBy="it.[Plan_Name]">
                <WhereParameters>
                    <asp:QueryStringParameter QueryStringField="Plan_ID" Name="Child_ID" Type="Int32" ConvertEmptyStringToNull="true"/>
                    <asp:ControlParameter ControlID="ctl00$Tile3$BP_ID" PropertyName="Value" Name="Business_Plan_ID" Type="Int32" />  
                </WhereParameters>          
                </asp:EntityDataSource>
            </div>
            <div class="clearAll"></div>
        </div>
              
        <div class="bpTile">
            <div class="tileContainerHeader"><div class="title">Coverage</div></div>
            <div id="NeurologyGrid" runat="server" class="dashboardTable">
                <asp:GridView ID="grdvwNeuCoverage" runat="server" AutoGenerateColumns="False" DataSourceID="dsCoverage"
                    Width="100%" SkinID="basic">
                    <Columns>
                        <asp:BoundField DataField="Drug_Name" HeaderText="Drug Name" Visible="true" ItemStyle-CssClass="firstCol" HeaderStyle-CssClass="firstCol" />
                        <asp:BoundField DataField="Benefit_Name" HeaderText="Benefits" Visible="true" />
                        <asp:BoundField DataField="Medical_Policy_Name" HeaderText="Medical Policy" Visible="true" />
                        <asp:BoundField DataField="Formulary_Status_Name" HeaderText="Formulary Status" Visible="true" />
                        <asp:BoundField DataField="Other_Restrictions" HeaderText="Restrictions" Visible="true" />
                        <asp:BoundField DataField="Num_Allocations" HeaderText="#of Indications" Visible="true" />
                        <asp:BoundField DataField="Copay_Coinsurance" HeaderText="$Co-pay/%Co-ins" Visible="true" />
                        <asp:BoundField DataField="Market_Share" HeaderText="Market Share" Visible="true" />
                        <asp:BoundField DataField="Gross_Sales" HeaderText="Gross Sales($)" Visible="true" />
                        
                        <asp:BoundField DataField="Contact_with_Manufacturer" HeaderText="Contract with Manufacturer" Visible="true" />
                    </Columns>
                    <EmptyDataTemplate>
                        <asp:Label ID="Label1" text="No records present." runat="server"></asp:Label>
                    </EmptyDataTemplate>
                </asp:GridView>
                <asp:EntityDataSource ID="dsCoverage" runat="server" ConnectionString="name=PathfinderMerzEntities"
                DefaultContainerName="PathfinderMerzEntities" EntitySetName="BusinessPlanningCoverageSet"
                AutoGenerateWhereClause="true" OrderBy="it.[Drug_Name]">
                <WhereParameters>
                    <asp:ControlParameter ControlID="ctl00$Tile3$BP_ID" PropertyName="Value" Name="Business_Plan_ID" Type="Int32" ConvertEmptyStringToNull="true"  />  
                </WhereParameters>          
            </asp:EntityDataSource>
            </div>
            <div id="DermatologyGrid" runat="server" class="dashboardTable">
                <merz:DermCoverage ID="DermCoverage1" runat="server"/>
            </div>
        </div>
        
        <div class="bpTile bpPDFTile"  id="BPInfoDetailsDiv" runat="server" >
            <div class="tileContainerHeader PDFDivHeader" id="BPInfoHeader" runat="server"><div class="title">Business Plan Information</div></div>
            <div class="bpInfo">
                <table border="0" cellpadding ="0" cellspacing ="0" width="100%">
                <tr id="AccOverviewTR1" runat="server">
                    <td class="bpLabel">Account Overview</td>
                </tr>
                <tr id="AccOverviewTR2" runat="server">
                    <td>
                    <asp:TextBox ReadOnly="true" ID="AccountOverview" Columns = "150" MaxLength="2500"  Rows="5" TextMode="MultiLine" runat="server"></asp:TextBox> 
                    </td>
                </tr>
                <tr id="CurrStatusTR1" runat="server">
                    <td class="bpLabel">Current Status</td>
                </tr>
                <tr id="CurrStatusTR2" runat="server">
                    <td>
                    <asp:TextBox ReadOnly="true" ID="CurrentStatus" Columns = "150" MaxLength="2500"  Rows="5" TextMode="MultiLine" runat="server"></asp:TextBox> 
                    </td>
                </tr>
                <tr  id="IssuesTR1" runat="server">
                    <td class="bpLabel">Issues</td>
                </tr>
                <tr id="IssuesTR2" runat="server" >
                    <td>
                    <asp:TextBox ReadOnly="true" ID="Issues" Columns = "150" MaxLength="2500"  Rows="5" TextMode="MultiLine" runat="server"></asp:TextBox> 
                    </td>
                </tr>
                
                <tr id="StrategiesTR1" runat="server">
                    <td class="bpLabel">Strategies</td>
                </tr>
                <tr id="StrategiesTR2" runat="server">
                    <td>
                    <asp:TextBox ReadOnly="true" ID="Strategies" Columns = "150" MaxLength="2500"  Rows="5" TextMode="MultiLine" runat="server"></asp:TextBox> 
                    </td>
                </tr>
                
                <tr id="TacticsTR1" runat="server">
                    <td class="bpLabel">Tactics</td>
                </tr>
                <tr id="TacticsTR2" runat="server">
                    <td>
                    <asp:TextBox ReadOnly="true" ID="Tactics" Columns = "150" MaxLength="2500"  Rows="5" TextMode="MultiLine" runat="server" ></asp:TextBox> 
                    </td>
                </tr>
                <tr id="MedPolicyTR1" runat="server">
                    <td class="bpLabel">Medical Policy Development Process and Influences</td>
                </tr>
                <tr  id="MedPolicyTR2" runat="server">
                    <td>
                    <asp:TextBox ReadOnly="true" ID="MedPolicyDev" Columns = "150" MaxLength="2500"  Rows="5" TextMode="MultiLine" runat="server"></asp:TextBox> 
                    </td>
                </tr>
                
                <tr id="PandTTR1" runat="server">
                    <td class="bpLabel"><asp:Label Text="P&T Committee Members, Process and Influencers" ID="PTHeaderText" runat="server" ></asp:Label></td>
                </tr>
                <tr id="PandTTR2" runat="server">
                    <td>
                    <asp:TextBox ReadOnly="true" ID="PandT" Columns = "150" MaxLength="2500"  Rows="5" TextMode="MultiLine" runat="server"></asp:TextBox> 
                    </td>
                </tr>
                <tr id="NotesTR1" runat="server">
                    <td class="bpLabel">Notes</td>
                </tr>
                <tr id="NotesTR2" runat="server">
                    <td>
                    <asp:TextBox ReadOnly="true" ID="Notes" Columns = "150" MaxLength="2500"  Rows="5" TextMode="MultiLine" runat="server" Visible="true"></asp:TextBox> 
                    </td>
                </tr>
                
                </table>
                
                <div id="NeurologyDIV" runat="server" visible="false" class="dashboardTable">
                    <div id="AccountOverviewDivHeader" class="PDFDivHeader1">Account Overview</div>                   
                    <div class="PDFDivMain" id="AccountOverviewDIV" runat="server"></div>
                    <div id="CurrentStatusDIVHeader" class="PDFDivHeader">Current Status</div>
                    <div id="CurrentStatusDIV" class="PDFDivMain" runat="server"></div>
                    <div id="IssuesDIVHeader" class="PDFDivHeader">Issues</div>
                    <div id="IssuesDIV" class="PDFDivMain"  runat="server"></div>
                    <div id="StrategiesDIVHeader" class="PDFDivHeader">Strategies</div>
                    <div id="StrategiesDIV"  class="PDFDivMain" runat="server"></div>
                    <div id="TacticsDIVHeader" class="PDFDivHeader">Tactics</div>
                    <div id="TacticsDIV"  class="PDFDivMain" runat="server"></div>
                    <div id="MedPolicyDevDIVHeader" class="PDFDivHeader">Medical Policy Development Process and Influences</div>
                    <div id="MedPolicyDevDIV" class="PDFDivMain"  runat="server" ></div>
                    <div id="PandTDIVHeader" class="PDFDivHeader">P&T Committee Members, Process and Influencers</div>
                    <div id="PandTDIV" class="PDFDivMain"  runat="server" ></div>
                </div>
                
                <div id="DermatologyDIV" runat="server" visible="false" class="dashboardTable">
                    <div id="IssuesDIVHeader1" class="PDFDivHeader1">Issues</div>
                    <div id="IssuesDIV1"  class="PDFDivMain" runat="server"></div>
                    <div id="StrategiesDIVHeader1" class="PDFDivHeader">Strategies</div>
                    <div id="StrategiesDIV1" class="PDFDivMain"  runat="server"></div>
                    <div id="TacticsDIVHeader1" class="PDFDivHeader">Tactics</div>
                    <div id="TacticsDIV1" class="PDFDivMain"  runat="server"></div>
                    <div id="NotesDivHeader" class="PDFDivHeader">Notes</div>
                    <div id="NotesDiv"  class="PDFDivMain" runat="server"></div>
                </div>
            </div>
        </div>
 </EmptyDataTemplate>
  <ItemTemplate>
        <asp:HiddenField ID="MedCEnrollment" Value='<%# Eval("Medicare_PartB_Enrollment")%>' runat="server" />

        <div class="bpTile" id="AffiliationsDiv" runat="server" >
            <div class="tileContainerHeader"><div class="title">Affiliations</div></div>
            <div class="dashboardTable leftBPTile pbmTile leftBgPDFTile" id="PBMDiv" runat="server" >
                <merz:PBM ID="PBM1" runat="server"/>
            </div>
         
            <div class="dashboardTable rightBPTile sppTile sppPDFTile" id="SPPDiv" runat="server">
                <asp:GridView ID="grdvwSPPAffliations" runat="server" AutoGenerateColumns="False" DataSourceID="dsSPPAffiliations"
                    Width="100%" GridLines="Both" BorderStyle="None" CssClass="staticTable" DataKeyNames="SPP_ID" SkinID="basic">
                    <Columns>
                        <asp:BoundField DataField="Plan_Name" HeaderText="SPP Name" Visible="true"  HeaderStyle-CssClass="firstCol firstGeneric" ItemStyle-CssClass="firstCol firstGeneric" />
                        <asp:CheckBoxField DataField="Preferred_SPP"  HeaderText="Preferred" ReadOnly="true"/> 
                    </Columns>
                    <EmptyDataTemplate>
                        <asp:Label ID="Label1" text="No records present for SPP." runat="server"></asp:Label>
                    </EmptyDataTemplate>
                </asp:GridView>
                <asp:EntityDataSource ID="dsSPPAffiliations" runat="server" ConnectionString="name=PathfinderMerzEntities"
                DefaultContainerName="PathfinderMerzEntities" EntitySetName="PlanAffiliationsForSPPSet"
                AutoGenerateWhereClause="true" OrderBy="it.[Plan_Name]">
                <WhereParameters>
                    <asp:QueryStringParameter QueryStringField="Plan_ID" Name="Child_ID" Type="Int32" ConvertEmptyStringToNull="true"/>
                    <asp:ControlParameter ControlID="ctl00$Tile3$BP_ID" PropertyName="Value" Name="Business_Plan_ID" Type="Int32" />  
                </WhereParameters>          
                </asp:EntityDataSource>
            </div>
            <div class="clearAll"></div>
        </div>
        
        <div class="bpTile">
            <div class="tileContainerHeader"><div class="title">Coverage</div></div>
            <div id="NeurologyGrid" runat="server" class="dashboardTable">
                <asp:GridView ID="grdvwNeuCoverage" runat="server" AutoGenerateColumns="False" DataSourceID="dsCoverage"
                    Width="100%" SkinID="basic">
                    <Columns>
                        <asp:BoundField DataField="Drug_Name" HeaderText="Drug Name" Visible="true" ItemStyle-CssClass="firstCol" HeaderStyle-CssClass="firstCol" />
                        <asp:BoundField DataField="Benefit_Name" HeaderText="Benefits" Visible="true" />
                        <asp:BoundField DataField="Medical_Policy_Name" HeaderText="Medical Policy" Visible="true" />
                        <asp:BoundField DataField="Formulary_Status_Name" HeaderText="Formulary Status" Visible="true" />
                        <asp:BoundField DataField="Other_Restrictions" HeaderText="Restrictions" Visible="true" />
                        <asp:BoundField DataField="Num_Allocations" HeaderText="#of Indications" Visible="true" />
                        <asp:BoundField DataField="Copay_Coinsurance" HeaderText="$Co-pay/%Co-ins" Visible="true" />
                        <asp:BoundField DataField="Market_Share" HeaderText="Market Share" Visible="true" />
                        <asp:BoundField DataField="Gross_Sales" HeaderText="Gross Sales($)" Visible="true" />
                        <asp:BoundField DataField="Contact_with_Manufacturer" HeaderText="Contract with Manufacturer" Visible="true" />
                    </Columns>
                    <EmptyDataTemplate>
                        <asp:Label ID="Label1" text="No records present." runat="server"></asp:Label>
                    </EmptyDataTemplate>
                </asp:GridView>
                <asp:EntityDataSource ID="dsCoverage" runat="server" ConnectionString="name=PathfinderMerzEntities"
                DefaultContainerName="PathfinderMerzEntities" EntitySetName="BusinessPlanningCoverageSet"
                AutoGenerateWhereClause="true" OrderBy="it.[Drug_Name]">
                <WhereParameters>
                    <asp:ControlParameter ControlID="ctl00$Tile3$BP_ID" PropertyName="Value" Name="Business_Plan_ID" Type="Int32" ConvertEmptyStringToNull="true"  />  
                </WhereParameters>          
            </asp:EntityDataSource>
            </div>
            <div id="DermatologyGrid" runat="server" class="dashboardTable">
                <merz:DermCoverage ID="DermCoverage1" runat="server"/>
            </div>
        </div>
        
        <div class="bpTile bpPDFTile" id="BPInfoDetailsDiv"  runat="server" >
            <div class="tileContainerHeader PDFDivHeader" id="BPInfoHeader" runat="server"><div class="title">Business Plan Information</div></div>
            <div class="bpInfo">
               <table border="0" cellpadding ="0" cellspacing ="0" width="100%">
               <tr id="AccOverviewTR1" runat="server">
                    <td class="bpLabel">Account Overview</td>
                </tr>
                <tr id="AccOverviewTR2" runat="server" style="page-break-before: always;">
                    <td>
                    <asp:TextBox ReadOnly="true" ID="AccountOverview" Columns = "150" MaxLength="2500"  Rows="5" TextMode="MultiLine" runat="server" Text='<%# Eval("Account_Overview")%>' visible="true"></asp:TextBox> 
                    </td>
                </tr>
                <tr id="CurrStatusTR1" runat="server">
                    <td class="bpLabel">Current Status</td>
                </tr>
                <tr id="CurrStatusTR2" runat="server">
                    <td>
                    <asp:TextBox ReadOnly="true" ID="CurrentStatus" Columns = "150" MaxLength="2500"  Rows="5" TextMode="MultiLine" runat="server" Text='<%# Eval("Current_Status")%>' visible="true"></asp:TextBox> 
                    </td>
                </tr>
                <tr  id="IssuesTR1" runat="server">
                    <td class="bpLabel">Issues</td>
                </tr>
                <tr id="IssuesTR2" runat="server" >
                    <td>
                    <asp:TextBox ReadOnly="true" ID="Issues" Columns = "150" MaxLength="2500"  Rows="5" TextMode="MultiLine" runat="server" Text='<%# Eval("Issues")%>' visible="true"></asp:TextBox> 
                    </td>
                </tr>
                
                <tr id="StrategiesTR1" runat="server">
                    <td class="bpLabel">Strategies</td>
                </tr>
                <tr id="StrategiesTR2" runat="server" >
                    <td>
                    <asp:TextBox ReadOnly="true" ID="Strategies" Columns = "150" MaxLength="2500"  Rows="5" TextMode="MultiLine" runat="server" Text='<%# Eval("Strategies")%>' visible="true"></asp:TextBox> 
                    </td>
                </tr>
                
                <tr id="TacticsTR1" runat="server">
                    <td class="bpLabel">Tactics</td>
                </tr>
                <tr id="TacticsTR2" runat="server" >
                    <td>
                    <asp:TextBox ReadOnly="true" ID="Tactics" Columns = "150" MaxLength="2500"  Rows="5" TextMode="MultiLine" runat="server" Text='<%# Eval("Tactics")%>' visible="true"></asp:TextBox> 
                    </td>
                </tr>
                <tr id="MedPolicyTR1" runat="server">
                    <td class="bpLabel">Medical Policy Development Process and Influences</td>
                </tr>
                <tr  id="MedPolicyTR2" runat="server">
                    <td>
                    <asp:TextBox ReadOnly="true" ID="MedPolicyDev" Columns = "150" MaxLength="2500"  Rows="5" TextMode="MultiLine" runat="server" Text='<%# Eval("Med_Policy_Development")%>' visible="true"></asp:TextBox> 
                    </td>
                </tr>
                
                <tr id="PandTTR1" runat="server">
                    <td class="bpLabel"><asp:Label Text="P&T Committee Members, Process and Influencers" ID="PTHeaderText" runat="server" ></asp:Label></td>
                </tr>
                <tr id="PandTTR2" runat="server">
                    <td>
                    <asp:TextBox ReadOnly="true" ID="PandT" Columns = "150" MaxLength="2500"  Rows="5" TextMode="MultiLine" runat="server" Text='<%# Eval("P_and_T")%>' visible="true"></asp:TextBox> 
                    </td>
                </tr>
                <tr id="NotesTR1" runat="server">
                    <td class="bpLabel">Notes</td>
                </tr>
                <tr id="NotesTR2" runat="server" >
                    <td>
                    <asp:TextBox ReadOnly="true" ID="Notes" Columns = "150" MaxLength="2500"  Rows="5" TextMode="MultiLine" runat="server" Text='<%# Eval("Notes")%>' visible="true"></asp:TextBox> 
                    </td>
                </tr>
                
                </table>
                
                <div id="NeurologyDIV" runat="server" visible="false" class="dashboardTable">
                    <div id="AccountOverviewDivHeader" class="PDFDivHeader1">Account Overview</div>
                    <div class="dashboardTable PDFDivMain" id="AccountOverviewDIV" runat="server"><%# ConvertLineBreaks(Convert.ToString(Eval("Account_Overview")))%></div>
                    <div id="CurrentStatusDIVHeader" class="PDFDivHeader">Current Status</div>
                    <div id="CurrentStatusDIV" class="dashboardTable PDFDivMain" runat="server"><%# ConvertLineBreaks(Convert.ToString(Eval("Current_Status")))%></div>
                    <div id="IssuesDIVHeader" class="PDFDivHeader">Issues</div>
                    <div id="IssuesDIV" class="dashboardTable PDFDivMain"  runat="server"><%# ConvertLineBreaks(Convert.ToString(Eval("Issues")))%></div>
                    <div id="StrategiesDIVHeader" class="PDFDivHeader">Strategies</div>
                    <div id="StrategiesDIV"  class="PDFDivMain" runat="server"><%# ConvertLineBreaks(Convert.ToString(Eval("Strategies")))%></div>
                    <div id="TacticsDIVHeader" class="PDFDivHeader">Tactics</div>
                    <div id="TacticsDIV"  class="dashboardTable PDFDivMain" runat="server"><%# ConvertLineBreaks(Convert.ToString(Eval("Tactics")))%></div>
                    <div id="MedPolicyDevDIVHeader" class="PDFDivHeader">Medical Policy Development Process and Influences</div>
                    <div id="MedPolicyDevDIV" class="dashboardTable PDFDivMain"  runat="server" ><%# ConvertLineBreaks(Convert.ToString(Eval("Med_Policy_Development")))%></div>
                    <div id="PandTDIVHeader" class="PDFDivHeader">P&T Committee Members, Process and Influencers</div>
                    <div id="PandTDIV" class="dashboardTable PDFDivMain"  runat="server" ><%# ConvertLineBreaks(Convert.ToString(Eval("P_and_T")))%></div>
                </div>
                
                <div id="DermatologyDIV" runat="server" visible="false" class="dashboardTable">
                    <div id="IssuesDIVHeader1" class="PDFDivHeader1">Issues</div>
                    <div id="IssuesDIV1"  class="dashboardTable PDFDivMain" runat="server"><%# ConvertLineBreaks(Convert.ToString(Eval("Issues")))%></div>
                    <div id="StrategiesDIVHeader1" class="PDFDivHeader">Strategies</div>
                    <div id="StrategiesDIV1" class="dashboardTable PDFDivMain"  runat="server"><%# ConvertLineBreaks(Convert.ToString(Eval("Strategies")))%></div>
                    <div id="TacticsDIVHeader1" class="PDFDivHeader">Tactics</div>
                    <div id="TacticsDIV1" class="dashboardTable PDFDivMain"  runat="server"><%# ConvertLineBreaks(Convert.ToString(Eval("Tactics")))%></div>
                    <div id="NotesDivHeader" class="PDFDivHeader">Notes</div>
                    <div id="NotesDiv"  class="dashboardTable PDFDivMain" runat="server"><%# ConvertLineBreaks(Convert.ToString(Eval("Notes")))%></div>
                </div>
            </div>
        </div>
        
        <div id="FormvwEditButton" runat="server" style="display: none;" >
            <asp:LinkButton ID="Editbtn" runat="server" Text="Edit" onclick="ViewEditbtn_Click" CssClass="postback"/>
        </div>                   
  </ItemTemplate>
  
  <EditItemTemplate>
        <asp:HiddenField ID="MedCEnrollment" Value='<%# Bind("Medicare_PartB_Enrollment")%>' runat="server" />

          <div class="bpTile"  id="AffiliationsDiv" runat="server" >
                <div class="tileContainerHeader"><div class="title">Affiliations</div></div>
                <div class="dashboardTable leftBPTile pbmTile leftBgPDFTile" id="PBMDiv" runat="server" >
                    <merz:PBM ID="PBM1" runat="server"/>
                </div>

                <div class="rightTile sppTile dashboardTable sppPDFTile" id="SPPDiv" runat="server">
                <asp:GridView ID="grdvwSPPAffliations" runat="server" AutoGenerateColumns="False" DataSourceID="dsSPPAffiliations"
                    Width="100%" GridLines="Both" BorderStyle="None" CssClass="staticTable"  DataKeyNames="SPP_ID" SkinID="basic">
                    <%-- Parent_ID = SPP_ID, will be stored in Business_Plan_Preferred_SPP table--%>
                    <Columns>
                        <asp:TemplateField HeaderText="SPP_ID" Visible="false" ItemStyle-CssClass="firstCol" HeaderStyle-CssClass="firstCol" >
                            <ItemTemplate>
                                <asp:Label ID="SPP_ID" runat="server" Text='<%# Eval("SPP_ID")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Plan_Name" HeaderText="SPP Name" Visible="true" />
                        <asp:TemplateField HeaderText="Preferred">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkSPP" Checked='<%# Eval("Preferred_SPP") %>' runat="server" /> 
                            </ItemTemplate> 
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <asp:Label ID="Label1" text="No records present for SPP." runat="server"></asp:Label>
                    </EmptyDataTemplate>
                </asp:GridView>
                <asp:EntityDataSource ID="dsSPPAffiliations" runat="server" ConnectionString="name=PathfinderMerzEntities"
                DefaultContainerName="PathfinderMerzEntities" EntitySetName="PlanAffiliationsForSPPSet"
                AutoGenerateWhereClause="true" OrderBy="it.[Plan_Name]">
                <WhereParameters>
                    <asp:QueryStringParameter QueryStringField="Plan_ID" Name="Child_ID" Type="Int32" ConvertEmptyStringToNull="true"/>
                    <asp:ControlParameter ControlID="ctl00$Tile3$BP_ID" PropertyName="Value" Name="Business_Plan_ID" Type="Int32" />  
                </WhereParameters>          
                </asp:EntityDataSource>
            </div>
            <div class="clearAll"></div>
         </div>
        
        <div class="bpTile">
            <div class="tileContainerHeader"><div class="title">Coverage</div></div>
            <div id="NeurologyGrid" runat="server">
                <asp:GridView ID="grdvwNeuCoverage" runat="server" AutoGenerateColumns="False" DataSourceID="dsCoverage1" SkinID="basic"
                    Width="100%">
                    <Columns>
                        <asp:TemplateField HeaderText="Drug_ID" Visible="false" ItemStyle-CssClass="firstCol" HeaderStyle-CssClass="firstCol" >
                            <ItemTemplate>
                                <asp:Label ID="Drug_ID" runat="server" Text='<%# Eval("Drug_ID")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Drug_Name" HeaderText="Drug Name" Visible="true" ItemStyle-CssClass="CoverageTile" HeaderStyle-CssClass="CoverageTile" />
                        <asp:TemplateField HeaderText="Benefits"  ItemStyle-Width="12%" ItemStyle-CssClass="CoverageTile" HeaderStyle-CssClass="CoverageTile" >
                            <ItemTemplate>
                                <telerik:RadComboBox ID="rdcmbBenefits" runat="server" Width="80%" DropDownWidth="100px" EnableEmbeddedSkins="false" SkinID="planInfoCombo" Skin="pathfinder" AppendDataBoundItems="true" DataSourceID="dsBenefits" DataTextField="Name" DataValueField="ID"  SelectedValue='<%# Eval("Benefit_ID") %>'>
                                    <Items>
                                        <telerik:RadComboBoxItem  Text="--No Selection--" Value="0"/> 
                                    </Items> 
                                </telerik:RadComboBox>
                            </ItemTemplate> 
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="Medical Policy" ItemStyle-Width="18%" ItemStyle-CssClass="CoverageTile" HeaderStyle-CssClass="CoverageTile" >
                            <ItemTemplate>
                                <telerik:RadComboBox ID="rdcmbMedPolicy" runat="server" Width="80%" DropDownWidth="130px" EnableEmbeddedSkins="false" SkinID="planInfoCombo" Skin="pathfinder" AppendDataBoundItems="true" DataSourceID="dsMedPolicy" DataTextField="Name" DataValueField="ID"  SelectedValue='<%# Eval("Medical_Policy_ID") %>'>
                                   <Items>
                                        <telerik:RadComboBoxItem  Text="--No Selection--" Value="0"/> 
                                    </Items> 
                                </telerik:RadComboBox>
                             </ItemTemplate> 
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Formulary Status"  ItemStyle-Width="14%" ItemStyle-CssClass="CoverageTile" HeaderStyle-CssClass="CoverageTile" >
                            <ItemTemplate>
                                <telerik:RadComboBox ID="rdcmbFormularyStatus" runat="server" Width="80%" DropDownWidth="100px" EnableEmbeddedSkins="false" SkinID="planInfoCombo" Skin="pathfinder" AppendDataBoundItems="true" DataSourceID="dsFStatus" DataTextField="Name" DataValueField="ID"  SelectedValue='<%# Eval("Formulary_Status_ID") %>'>
                                    <Items>
                                        <telerik:RadComboBoxItem  Text="--No Selection--" Value="0"/> 
                                    </Items> 
                                </telerik:RadComboBox>
                             </ItemTemplate> 
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="Restrictions"  ItemStyle-Width="12%" ItemStyle-CssClass="CoverageTile" HeaderStyle-CssClass="CoverageTile" >
                            <ItemTemplate>
                                <telerik:RadComboBox ID="rdcmbOtherRestrictions" runat="server" Width="80%" DropDownWidth="70px" EnableEmbeddedSkins="false" SkinID="planInfoCombo" Skin="pathfinder" AppendDataBoundItems="true" CssClass="restrictions">
                                </telerik:RadComboBox>
                                <asp:HiddenField ID="SelectedRestrictions" runat="server" Value='<%# Eval("Other_Restrictions") %>' />
                            </ItemTemplate> 
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="#of Indications"  ItemStyle-CssClass="CoverageTile" HeaderStyle-CssClass="CoverageTile" >
                            <ItemTemplate>
                                <asp:TextBox ID="Num_Allocationstxt" Width="50px" MaxLength="9" runat="server" Text='<%# Eval("Num_Allocations") %>'></asp:TextBox>
                                <pinso:ClientValidator ID="cv4" Target="Num_Allocationstxt" Text ='<%# Eval("Drug_Name","Please enter valid #of Indications for {0}.") %>' Required="false" runat="server" DataType="Integer" />  
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="$Co-pay/%Co-ins" ItemStyle-CssClass="CoverageTile" HeaderStyle-CssClass="CoverageTile" >
                            <ItemTemplate>
                                <asp:TextBox ID="Copay_Coinsurancetxt" Width="50px" MaxLength="50" runat="server" Text='<%# Eval("Copay_Coinsurance") %>'></asp:TextBox>
                                <pinso:ClientValidator ID="cv5" Target="Copay_Coinsurancetxt" Text ='<%# Eval("Drug_Name","Please enter valid $Co-pay/%Co-ins for {0}.") %>' Required="false" runat="server" DataType="String"  />  
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Market Share"  ItemStyle-Width="9%" ItemStyle-CssClass="CoverageTile" HeaderStyle-CssClass="CoverageTile" >
                            <ItemTemplate>
                                <asp:TextBox ID="Market_Sharetxt" Width="60px" runat="server" Text='<%# Eval("Market_Share") %>'></asp:TextBox>
                                <pinso:ClientValidator ID="cv6" Target="Market_Sharetxt" Text ='<%# Eval("Drug_Name","Please enter valid Market Share for {0}.") %>' Required="false" runat="server" CompareToValue="100" CompareOperator="LessThanEqual"  DataType="Double"  />  
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Gross Sales($)"  ItemStyle-Width="9%" ItemStyle-CssClass="CoverageTile" HeaderStyle-CssClass="CoverageTile" >
                            <ItemTemplate>
                                <asp:TextBox ID="Gross_Salestxt" Width="60px" MaxLength="15" runat="server" Text='<%# Eval("Gross_Sales") %>'></asp:TextBox>
                                <pinso:ClientValidator ID="cv7" Target="Gross_Salestxt" Text ='<%# Eval("Drug_Name","Please enter valid Gross Sales($) for {0}.") %>' Required="false" runat="server" DataType="Double"  />  
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Contract with Manufacturer" ItemStyle-CssClass="CoverageTile" HeaderStyle-CssClass="CoverageTile" >
                            <ItemTemplate>
                                <asp:TextBox ID="Contact_with_Manufacturertxt" Width="90px" MaxLength="50" runat="server" Text='<%# Eval("Contact_with_Manufacturer") %>'></asp:TextBox>
                                <pinso:ClientValidator ID="cv8" Target="Contact_with_Manufacturertxt" Text ='<%# Eval("Drug_Name","Please enter valid Contract with Manufacturer for {0}.") %>' Required="false" runat="server" DataType="String" />  
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                                                  
                <asp:EntityDataSource ID="dsCoverage1" runat="server" ConnectionString="name=PathfinderMerzEntities"
                DefaultContainerName="PathfinderMerzEntities" EntitySetName="BusinessPlanningCoverageSet"
                AutoGenerateWhereClause="true" OrderBy="it.[Drug_Name]">
                <WhereParameters>
                    <asp:ControlParameter ControlID="ctl00$Tile3$BP_ID" PropertyName="Value" Name="Business_Plan_ID" Type="Int32" />  
                </WhereParameters>          
            </asp:EntityDataSource> 
            <asp:EntityDataSource ID="dsBenefits" runat="server" ConnectionString="name=PathfinderMerzEntities"
                DefaultContainerName="PathfinderMerzEntities" EntitySetName="BusinessPlanBenefitSet"
                AutoGenerateWhereClause="true" OrderBy="it.[Name]">
            </asp:EntityDataSource>
            <asp:EntityDataSource ID="dsMedPolicy" runat="server" ConnectionString="name=PathfinderMerzEntities"
                DefaultContainerName="PathfinderMerzEntities" EntitySetName="BusinessPlanMedicalPolicySet"
                AutoGenerateWhereClause="true" OrderBy="it.[Name]">
            </asp:EntityDataSource>  
            <asp:EntityDataSource ID="dsFStatus" runat="server" ConnectionString="name=PathfinderEntities"
                DefaultContainerName="PathfinderEntities" EntitySetName="FormularyStatusSet"
                AutoGenerateWhereClause="true" OrderBy="it.[Name]">
            </asp:EntityDataSource> 
            </div>
            <div id="DermatologyGrid" runat="server" class="dashboardTable">
                <merz:DermCoverage ID="DermCoverage1" runat="server" />
            </div>
        </div>
        
        
        <div class="bpTile bpPDFTile" id="BPInfoDetailsDiv"  runat="server" >
            <div class="tileContainerHeader PDFDivHeader" id="BPInfoHeader" runat="server"><div class="title">Business Plan Information</div></div>
            <div class="bpInfo">
            <div id="NeurologyTextBoxesDiv" runat="server">
                <table border="0" cellpadding ="0" cellspacing ="0" width="100%">
                   <tr>
                        <td class="bpLabel">Account Overview</td>
                    </tr>
                  <tr>
                    <td>
                    <asp:TextBox ID="AccountOverview1" Columns = "150" MaxLength="2500"  Rows="5" TextMode="MultiLine" runat="server" Text='<%# Bind("Account_Overview")%>'></asp:TextBox> 
                     <pinso:ClientValidator ID="cv9" Target="AccountOverview1" Text ="Maximum 2500 characters allowed for Account Overview." Required="false" runat="server" DataType="String" MaxLength="2500"  />  
                    </td>
                </tr>
                <tr>
                    <td class="bpLabel">Current Status</td>
                </tr>
                <tr>
                    <td>
                    <asp:TextBox ID="CurrentStatus1" Columns = "150" MaxLength="2500"  Rows="5" TextMode="MultiLine" runat="server" Text='<%# Bind("Current_Status")%>'></asp:TextBox> 
                    <pinso:ClientValidator ID="ClientValidator1" Target="CurrentStatus1" Text ="Maximum 2500 characters allowed for Current Status." Required="false" runat="server" DataType="String" MaxLength="2500"  />  
                    </td>
                </tr>
                <tr>
                    <td class="bpLabel">Issues</td>
                </tr>
                <tr>
                    <td>
                    <asp:TextBox ID="Issues1" Columns = "150" MaxLength="2500"  Rows="5" TextMode="MultiLine" runat="server" Text='<%# Bind("Issues")%>'></asp:TextBox> 
                    <pinso:ClientValidator ID="ClientValidator2" Target="Issues1" Text ="Maximum 2500 characters allowed for Issues." Required="false" runat="server" DataType="String" MaxLength="2500"  />  
                    </td>
                </tr>
                
                <tr>
                    <td class="bpLabel">Strategies</td>
                </tr>
                <tr>
                    <td>
                    <asp:TextBox ID="Strategies1" Columns = "150" MaxLength="2500"  Rows="5" TextMode="MultiLine" runat="server" Text='<%# Bind("Strategies")%>'></asp:TextBox> 
                    <pinso:ClientValidator ID="ClientValidator3" Target="Strategies1" Text ="Maximum 2500 characters allowed for Strategies." Required="false" runat="server" DataType="String" MaxLength="2500"  />  
                    </td>
                </tr>
                
                <tr>
                    <td class="bpLabel">Tactics</td>
                </tr>
                <tr>
                    <td>
                    <asp:TextBox ID="Tactics1" Columns = "150" MaxLength="2500"  Rows="5" TextMode="MultiLine" runat="server" Text='<%# Bind("Tactics")%>'></asp:TextBox> 
                    <pinso:ClientValidator ID="ClientValidator4" Target="Tactics1" Text ="Maximum 2500 characters allowed for Tactics." Required="false" runat="server" DataType="String" MaxLength="2500"  />  
                    </td>
                </tr>
                <tr>
                    <td class="bpLabel">Medical Policy Development Process and Influences</td>
                </tr>
                <tr>
                    <td>
                    <asp:TextBox ID="MedPolicyDev1" Columns = "150" MaxLength="2500"  Rows="5" TextMode="MultiLine" runat="server" Text='<%# Bind("Med_Policy_Development")%>'></asp:TextBox> 
                    <pinso:ClientValidator ID="ClientValidator5" Target="MedPolicyDev1" Text ="Maximum 2500 characters allowed for Medical Policy Development Process and Influences." Required="false" runat="server" DataType="String" MaxLength="2500"  />                      
                    </td>
                </tr>
                
                <tr>
                    <td class="bpLabel"><asp:Label Text="P&T Committee Members, Process and Influencers" ID="PTHeaderText" runat="server" ></asp:Label></td>
                </tr>
                <tr>
                    <td>
                    <asp:TextBox ID="PandT1" Columns = "150" MaxLength="2500"  Rows="5" TextMode="MultiLine" runat="server" Text='<%# Bind("P_and_T")%>'></asp:TextBox> 
                    <pinso:ClientValidator ID="ClientValidator6" Target="PandT1" Text ="Maximum 2500 characters allowed for P&T Committee Members, Process and Influencers." Required="false" runat="server" DataType="String" MaxLength="2500"  />  
                    </td>
                </tr>
                
                
                </table>
            </div>
            
            <div id="DermatologyTextBoxesDiv" runat="server">
            <table border="0" cellpadding ="0" cellspacing ="0" width="100%">
                <tr>
                    <td>Issues</td>
                </tr>
                <tr>
                    <td>
                    <asp:TextBox ID="Issues2" Columns = "150" MaxLength="2500"  Rows="5" TextMode="MultiLine" runat="server" Text='<%# Bind("Issues")%>'></asp:TextBox> 
                    <pinso:ClientValidator ID="ClientValidator8" Target="Issues2" Text ="Maximum 2500 characters allowed for Issues." Required="false" runat="server" DataType="String" MaxLength="2500"  />  
                    </td>
                </tr>
                
                <tr>
                    <td class="bpLabel">Strategies</td>
                </tr>
                <tr>
                    <td>
                    <asp:TextBox ID="Strategies2" Columns = "150" MaxLength="2500"  Rows="5" TextMode="MultiLine" runat="server" Text='<%# Bind("Strategies")%>'></asp:TextBox> 
                    <pinso:ClientValidator ID="ClientValidator9" Target="Strategies2" Text ="Maximum 2500 characters allowed for Strategies." Required="false" runat="server" DataType="String" MaxLength="2500"  />  
                    </td>
                </tr>
                
                <tr>
                    <td class="bpLabel">Tactics</td>
                </tr>
                <tr>
                    <td>
                    <asp:TextBox ID="Tactics2" Columns = "150" MaxLength="2500"  Rows="5" TextMode="MultiLine" runat="server" Text='<%# Bind("Tactics")%>'></asp:TextBox> 
                    <pinso:ClientValidator ID="ClientValidator10" Target="Tactics2" Text ="Maximum 2500 characters allowed for Tactics." Required="false" runat="server" DataType="String" MaxLength="2500"  />  
                    </td>
                </tr>
                <tr>
                    <td class="bpLabel">Notes</td>
                </tr>
                <tr>
                    <td>
                    <asp:TextBox ID="Notes2" Columns = "150" MaxLength="2500"  Rows="5" TextMode="MultiLine" runat="server" Text='<%# Bind("Notes")%>'></asp:TextBox> 
                    <pinso:ClientValidator ID="ClientValidator7" Target="Notes2" Text ="Maximum 2500 characters allowed for Notes." Required="false" runat="server" DataType="String" MaxLength="2500"  />  
                    </td>
                </tr>
                
                </table>
            </div>
        </div>
        </div>
        <div id="Div1" class="FormVWSaveButton" style="display:none;" runat="server" >
            <asp:LinkButton ID="Savebtn" runat="server" Text="Save" width="50px" CommandName="Update"  CssClass="postback validate"/>
        </div>
  </EditItemTemplate>
  
  </asp:FormView>

 <asp:EntityDataSource ID="dsBusinessPlanning" runat="server" EntitySetName="BusinessPlansSet" DefaultContainerName="PathfinderMerzEntities" ConnectionString="name=PathfinderMerzEntities" 
        AutoGenerateWhereClause="true" EnableInsert="true" EnableUpdate="true" OnUpdating="EditData" OnUpdated="ConfirmUpdate" OnInserted="ConfirmUpdate" 
        ContextTypeName="Pinsonault.Application.Merz.PathfinderMerzEntities, Pinsonault.Application.Merz">
        <WhereParameters>       
            <asp:QueryStringParameter QueryStringField="Plan_ID" Name="Plan_ID" Type="Int32" ConvertEmptyStringToNull="true"/>
            <asp:QueryStringParameter QueryStringField="Thera_ID" Name="Thera_ID" Type="Int32" ConvertEmptyStringToNull="true" />
        </WhereParameters>
</asp:EntityDataSource>    
    
    