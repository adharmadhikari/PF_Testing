<%@ Control Language="C#" AutoEventWireup="true" CodeFile="favoritesList.ascx.cs" Inherits="Controls_favoritesList" %>
    <div class="favList">
        <asp:UpdatePanel runat="server" ID="updatePanel">
            <ContentTemplate>
                <asp:GridView runat="server" ID="gridView" DataSourceID="dsFavorites" DataKeyNames="Favorite_ID" AutoGenerateColumns="false" Width="100%" GridLines="None" BorderStyle="None" ShowHeader="false">
                    <Columns>
                        <asp:TemplateField ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <%--Calling script in parent window to support postback to Dashboard.aspx in main browser window. --%>
                                <a target="_top" href="javascript:void(0)" onclick='<%# string.Format(SelectFavoritesFunctionFormat, Eval("Favorite_ID"), Eval("Data"))%>'><%# Eval("Name") %></a>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="linkDelete" CommandName="delete" Text='<%$ Resources:Resource, Label_Delete %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <asp:Literal runat="server" ID="noFavsMessage" Text='<%$ Resources:Resource, Message_No_Favorites_Added %>' />
                    </EmptyDataTemplate>
                </asp:GridView>               
            </ContentTemplate>
        </asp:UpdatePanel>
        
        <asp:EntityDataSource runat="server" ID="dsFavorites" EntitySetName="FavoriteSet" EnableDelete="true" DefaultContainerName="PathfinderEntities" ConnectionString="name=PathfinderEntities" AutoGenerateWhereClause="true" OrderBy="it.Name">
            <WhereParameters>
                <asp:SessionParameter Name="User_ID" SessionField="UserID" Type="Int32" />
            </WhereParameters>
        </asp:EntityDataSource>
    </div>