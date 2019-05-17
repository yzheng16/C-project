<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Purchasing.aspx.cs" Inherits="WebApp.Pages.Purchasing.Purchasing" %>

<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Purchasing</h1>
    <asp:Label runat="server" ID="EmployeeLabel">Employee: </asp:Label>
    <asp:Label runat="server" ID="Employee"></asp:Label>
    <uc1:MessageUserControl runat="server" ID="MessageUserControl" />
    <div class="row">
        <div class="col-md-6">
            <asp:Label runat="server" ID="SelectVendorLabel">Vendor</asp:Label>
            <asp:DropDownList runat="server" ID="VendorDropDownList" 
                              AppendDataBoundItems="true"
                              DataSourceID="VendorDataSource" 
                              DataTextField="VendorName" 
                              DataValueField="VendorID">
                <asp:ListItem Value="0">[ Select a vendor ]</asp:ListItem>
            </asp:DropDownList>
            <asp:LinkButton runat="server" ID="Get_Create_PO" OnClick="Get_Create_PO_OnClick" CssClass="btn btn-default">Get/Create PO</asp:LinkButton>
        </div>
        <div class="col-md-6">
            <asp:Label runat="server" ID="VendorNameLabel">Vendor: </asp:Label>
            <asp:Label runat="server" ID="VendorName"></asp:Label>
            <asp:Label runat="server" ID="LocationLabel">Location: </asp:Label>
            <asp:Label runat="server" ID="Location"></asp:Label>
            <asp:Label runat="server" ID="PhoneLabel">Phone: </asp:Label>
            <asp:Label runat="server" ID="Phone"></asp:Label>
        </div>
    </div>
    <h1>Purchase Order</h1>
    <div class="row">
        <div class="col-md-12">
            <asp:GridView runat="server" ID="ActivePurchaseOrderDetailsGridView" 
                          OnRowCommand="ActivePurchaseOrderDetails_RowCommand"
                          ItemType="TeamCeBikeLab.Entities.POCOs.PurchaseOrderDetails"
                          AutoGenerateColumns="False" 
                          CellPadding="4" 
                          ForeColor="#333333" 
                          GridLines="None">
                <AlternatingRowStyle BackColor="White" ForeColor="#284775"></AlternatingRowStyle>
                <Columns>
                    <asp:BoundField DataField="PurchaseOrderID" HeaderText="PurchaseOrderID" SortExpression="PurchaseOrderID"></asp:BoundField>
                    <asp:BoundField DataField="PurchaseOrderDetailID" HeaderText="PurchaseOrderDetailID" SortExpression="PurchaseOrderDetailID" Visible="false"></asp:BoundField>
                    <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID"></asp:BoundField>
                    <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description"></asp:BoundField>
                    <asp:BoundField DataField="QOH" HeaderText="QOH" SortExpression="QOH"></asp:BoundField>
                    <asp:BoundField DataField="ROL" HeaderText="ROL" SortExpression="ROL"></asp:BoundField>
                    <asp:BoundField DataField="QOO" HeaderText="QOO" SortExpression="QOO"></asp:BoundField>
                    <asp:TemplateField HeaderText="Quantity" SortExpression="Quantity">
                        <ItemTemplate>
                            <asp:TextBox runat="server" Text='<%# Bind("Quantity") %>' ID="Quantity"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="PurchasePrice" SortExpression="PurchasePrice">
                        <ItemTemplate>
                            <asp:TextBox runat="server" Text='<%# Bind("PurchasePrice") %>' ID="PurchasePrice"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton runat="server" ID="RemoveButton" Text="Remove" CommandName="Remove"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EditRowStyle BackColor="#999999"></EditRowStyle>

                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White"></FooterStyle>

                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White"></HeaderStyle>

                <PagerStyle HorizontalAlign="Center" BackColor="#284775" ForeColor="White"></PagerStyle>

                <RowStyle BackColor="#F7F6F3" ForeColor="#333333"></RowStyle>

                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333"></SelectedRowStyle>

                <SortedAscendingCellStyle BackColor="#E9E7E2"></SortedAscendingCellStyle>

                <SortedAscendingHeaderStyle BackColor="#506C8C"></SortedAscendingHeaderStyle>

                <SortedDescendingCellStyle BackColor="#FFFDF8"></SortedDescendingCellStyle>

                <SortedDescendingHeaderStyle BackColor="#6F8DAE"></SortedDescendingHeaderStyle>
            </asp:GridView>
        </div>
    </div>
    <div class="row">
        <div class="col-md-12">
            <asp:Button runat="server" ID="UpdateButton" Text="Update" OnClick="Update_OnClick" />
            <asp:Button runat="server" ID="PlaceButton" Text="Place" OnClick="Place_OnClick" />
            <asp:Button runat="server" ID="DeleteButton" Text="Delete" OnClick="Delete_OnClick" />
            <asp:Button runat="server" ID="ClearButton" Text="Clear" OnClick="Clear_OnClick" />
        </div>
    </div>
    <div class="row">
        <div class="col-md-12">
            <asp:Label runat="server" ID="SubtotalLabel">Subtotal: </asp:Label>
            <asp:Label runat="server" ID="SubtotalDollarSign">$</asp:Label>
            <asp:Label runat="server" ID="Subtotal"></asp:Label>
            <asp:Label runat="server" ID="GSTLabel">GST: </asp:Label>
            <asp:Label runat="server" ID="GSTDollarSign">$</asp:Label>
            <asp:Label runat="server" ID="GST"></asp:Label>
            <asp:Label runat="server" ID="TotalLabel">Total: </asp:Label>
            <asp:Label runat="server" ID="TotalDollarSign">$</asp:Label>
            <asp:Label runat="server" ID="Total"></asp:Label>
        </div>
    </div>
    <h1>Vendor Inventory</h1>
    <div class="row">
        <div class="col-md-12">
            <asp:GridView ID="InventoryItemsGridView" runat="server" 
                          ItemType="TeamCeBikeLab.Entities.POCOs.VendorStockItems"
                          OnRowCommand="InventoryItems_RowCommand"
                          CssClass="table table-hover table-striped" 
                          CellPadding="4" 
                          ForeColor="#333333" 
                          GridLines="None" 
                          AutoGenerateColumns="False">
                <AlternatingRowStyle BackColor="White" ForeColor="#284775"></AlternatingRowStyle>
                <Columns>
                    <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID"></asp:BoundField>
                    <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description"></asp:BoundField>
                    <asp:BoundField DataField="QOH" HeaderText="QOH" SortExpression="QOH"></asp:BoundField>
                    <asp:BoundField DataField="QOO" HeaderText="QOO" SortExpression="QOO"></asp:BoundField>
                    <asp:BoundField DataField="ROL" HeaderText="ROL" SortExpression="ROL"></asp:BoundField>
                    <asp:BoundField DataField="Buffer" HeaderText="Buffer" SortExpression="Buffer"></asp:BoundField>
                    <asp:BoundField DataField="Price" HeaderText="Price" SortExpression="Price"></asp:BoundField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton runat="server" ID="AddButton" Text="Add" CommandName="Add"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>

                <EditRowStyle BackColor="#999999"></EditRowStyle>
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White"></FooterStyle>
                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White"></HeaderStyle>
                <PagerStyle HorizontalAlign="Center" BackColor="#284775" ForeColor="White"></PagerStyle>
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333"></RowStyle>
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333"></SelectedRowStyle>
                <SortedAscendingCellStyle BackColor="#E9E7E2"></SortedAscendingCellStyle>
                <SortedAscendingHeaderStyle BackColor="#506C8C"></SortedAscendingHeaderStyle>
                <SortedDescendingCellStyle BackColor="#FFFDF8"></SortedDescendingCellStyle>
                <SortedDescendingHeaderStyle BackColor="#6F8DAE"></SortedDescendingHeaderStyle>
            </asp:GridView>
        </div>
    </div>
    <!--------------------------------------------------- OBJECT DATA SOURCE ------------------------------------------------------>
    <asp:ObjectDataSource ID="VendorDataSource" runat="server"
        OldValuesParameterFormatString="original_{0}"
        SelectMethod="SelectVendor"
        TypeName="TeamCeBikeLab.BLL.Purchasing.PurchasingController"></asp:ObjectDataSource>

    <!------------------------------------------------END OBJECT DATA SOURCE ------------------------------------------------------>
</asp:Content>
