<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AddKeyContacts.ascx.cs" Inherits="controls_AddKeyContacts" EnableTheming="true" %>
<style type="text/css">
    .style1
    {
        width: 12%;
    }
</style>
<br /> 
    <table width="90%" border="1px">
    <tr>
    <td align="left">
    <div id="AddKCMain">
         <div id="header" class="tileContainerHeader" align="left" >
        <asp:Literal runat="server" ID="titleText" Text='Add Key Decision Maker...'/>
        </div>
        <div id="Div1" class="todaysAccounts1">
        <table style="color:Black;width:90%">
        <tr>
            <td class="style1">Prefix </td>
            <td>
                <asp:TextBox ID="Prefixtxt" runat="server" Width="57px" Text='<%# Bind("KC_Prefix") %>'></asp:TextBox>
            </td>
            <td width="15%">First Name </td>
            <td width="25%">
                <asp:TextBox ID="FNametxt" runat="server" Width="237px" Text='<%# Bind("KC_F_Name") %>'></asp:TextBox>
            </td>
            <td width="10%">Last Name </td>
            <td width="25%">
                <asp:TextBox ID="LNametxt" runat="server" Width="237px" Text='<%# Bind("KC_L_Name") %>'></asp:TextBox>
            </td>
            <td width="10%">Suffix </td>
            <td width="25%">
                <asp:TextBox ID="Suffixtxt" runat="server" Width="57px" Text='<%# Bind("KC_Suffix") %>'></asp:TextBox>
            </td>
        </tr> 
        <tr>
            <td  width="5%">Designation </td>
            <td width="20%" colspan="3">
            <telerik:RadComboBox ID="rdcmbDesg" runat="server" Width="65%" EnableEmbeddedSkins="false" SkinID="planInfoCombo" Skin="pathfinder" AppendDataBoundItems="true" >
               <Items>
                    <telerik:RadComboBoxItem Value="" Text="" />
                </Items>
            </telerik:RadComboBox> 
            </td>
            <td>Title </td>
            <td colspan="3">
                <asp:TextBox ID="Titletxt" runat="server" Width="330px" Text='<%# Bind("KC_Title_Name") %>'></asp:TextBox>
            </td>
        </tr> 
        <tr>
            <td  width="5%">Address1 </td>
            <td colspan="3">
                <asp:TextBox ID="Addr1txt" runat="server" Width="331px" Text='<%# Bind("KC_Address1") %>'></asp:TextBox>
            </td>
            <td>Address2 </td>
            <td colspan="3">
                <asp:TextBox ID="Addr2txt" runat="server" Width="330px" Text='<%# Bind("KC_Address2") %>'></asp:TextBox>
            </td>
        </tr> 
        <tr>
            <td width="5%">City </td>
            <td colspan="3">
                <asp:TextBox ID="Citytxt" runat="server" Width="331px" Text='<%# Bind("KC_City") %>'></asp:TextBox>
            </td>
            <td>State </td>
            <td>
            <telerik:RadComboBox EnableEmbeddedSkins="false" SkinID="planInfoCombo" Skin="pathfinder" runat="server" ID="rdlStates" DataSourceID="dsStates" Width="75%" DropDownWidth="180px" DataTextField="Name" DataValueField="ID" AppendDataBoundItems="true" MaxHeight="200" SelectedValue='<%# Bind("KC_State") %>'>
                <Items>
                    <telerik:RadComboBoxItem Value="" Text="" />
                </Items>
            </telerik:RadComboBox>
            </td>
            <td>Zip </td>
            <td>
                <asp:TextBox ID="Ziptxt" runat="server" Width="57px" Text='<%# Bind("KC_Zip") %>'></asp:TextBox>
            </td>
        </tr> 
        <tr>
            <td width="5%">Email 1 </td>
            <td colspan="3">
                <asp:TextBox ID="Email1txt" runat="server" Width="331px" Text='<%# Bind("KC_Email") %>'></asp:TextBox>
            </td>
            <td>Email 2 </td>
            <td colspan="3">
                <asp:TextBox ID="Email2txt" runat="server" Width="331px" Text=""></asp:TextBox>
            </td>
        </tr> 
        <tr>
            <td class="style1">Phone 1 </td>
            <td>
                <asp:TextBox ID="Ph1txt" runat="server" Width="111px" Text='<%# Bind("KC_Phone") %>'></asp:TextBox>
            </td>
            <td width="15%">Phone 2 </td>
            <td>
                <asp:TextBox ID="Ph1txt0" runat="server" Width="111px" Text=""></asp:TextBox>
            </td>
            <td>Mobile </td>
            <td>
                <asp:TextBox ID="Ph1txt1" runat="server" Width="111px" Text='<%# Bind("KC_Mobile") %>'></asp:TextBox>
            </td>
            <td>Fax </td>
            <td>
                <asp:TextBox ID="Ph1txt2" runat="server" Width="111px" Text='<%# Bind("KC_Fax") %>'></asp:TextBox>
            </td>
        </tr> 
        </table>
        </div>

        <div id="Div3" class="tileContainerHeader" align="left" >
        <asp:Literal runat="server" ID="Literal2" Text='Assistant Details...' />
        </div>
        <div id="Div2" class="todaysAccounts2">
        <table style="color:Black;width:100%">
        <tr>
            <td>First Name </td>
            <td colspan="3">
                <asp:TextBox ID="AsstFNmtxt" runat="server" Width="237px" Text='<%# Bind("KC_Admin_Name") %>'></asp:TextBox>
            </td>
            <td>Middle Initial </td>
            <td>
                <asp:TextBox ID="AsstMNmtxt" runat="server" Width="57px" Text=""></asp:TextBox>
            </td>
            <td>Last Name </td>
            <td>
                <asp:TextBox ID="AsstLNmtxt" runat="server" Width="237px" Text='<%# Bind("KC_Admin_Name") %>'></asp:TextBox>
            </td>
        </tr> 
        <tr>
            <td>Office Phone </td>
            <td>
                <asp:TextBox ID="Ph1txt3" runat="server" Width="111px" Text='<%# Bind("KC_Admin_PH") %>'></asp:TextBox>
            </td>
            <td>Mobile Phone </td>
            <td>
                <asp:TextBox ID="Ph1txt4" runat="server" Width="111px" Text=""></asp:TextBox>
            </td>
            <td id="blank"></td>
            <td>Email </td>
            <td colspan="2">
                <asp:TextBox ID="AsstEmailtxt" runat="server" Width="331px" Text='<%# Bind("KC_Admin_Email") %>'></asp:TextBox>
            </td>
        </tr> 
        <tr>
            <td>comments </td>
            <td colspan="7">
                <asp:TextBox ID="Cmtstxt" runat="server" Height="47px" Rows="2" TextMode="MultiLine" Width="841px" Text='<%# Bind("KC_Comments") %>'></asp:TextBox>
            </td>
        </tr> 
        </table>
        </div>
    </div> 
    </td>
    </tr>
    </table>
   