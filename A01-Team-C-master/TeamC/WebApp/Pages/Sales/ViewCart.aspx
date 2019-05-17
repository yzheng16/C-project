<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewCart.aspx.cs" Inherits="WebApp.Pages.Sales.ViewCart" %>

<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <uc1:MessageUserControl runat="server" ID="MessageUserControl" />
    <h2>Your Shopping Cart</h2>
    <asp:HiddenField runat="server" ID="CustomerId"></asp:HiddenField>
    <asp:GridView ID="CartGridView" runat="server" AutoGenerateColumns="False" ItemType="TeamCeBikeLab.Entities.POCOs.CartInfo">
        <Columns>
            <asp:TemplateField HeaderText="UpdatedOn" SortExpression="UpdatedOn">
                <EditItemTemplate>
                    <asp:TextBox runat="server" Text='<%# Bind("UpdatedOn") %>' ID="TextBox1"></asp:TextBox>
                </EditItemTemplate>
                <ItemTemplate>
                    <asp:Label runat="server" Text='<%# Bind("UpdatedOn") %>' ID="Label1"></asp:Label>
                    
                </ItemTemplate>
            </asp:TemplateField>

        </Columns>
    </asp:GridView>
    <asp:ObjectDataSource ID="CartDataSource" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="getCartInfo" TypeName="TeamCeBikeLab.BLL.SalesCRUD.CartController">
        <SelectParameters>
            <asp:Parameter Name="customerId" Type="Int32"></asp:Parameter>
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>
