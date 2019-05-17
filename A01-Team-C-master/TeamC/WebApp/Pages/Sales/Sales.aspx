<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Sales.aspx.cs" Inherits="WebApp.Pages.Sales.Sales" %>

<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <uc1:MessageUserControl runat="server" ID="MessageUserControl" />
    <asp:HiddenField runat="server" Value="" ID="UserName"></asp:HiddenField>
    <h1>Sales</h1>
    <asp:Panel ID="PartCatelogPanel" runat="server" Visible="true" CssClass="row">
        <div class="col-md-4">
            <h2>Browse by Category</h2>
            <asp:LinkButton ID="All" runat="server" CssClass="btn" OnClick="All_Click">
                All&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="AllPartsAmount" runat="server"></asp:Label>
            </asp:LinkButton>
            
            <asp:GridView ID="CategoryGridView" runat="server" AutoGenerateColumns="False" DataSourceID="CategoryDataSource"  BorderStyle="None" CssClass="table table-hover  table-condensed" Width="300px" ShowHeader="False" GridLines="None"  DataKeyNames="CategoryID">
                <Columns>
                    <asp:CommandField ShowSelectButton="true" ButtonType="Link" >
                    </asp:CommandField>
                    <asp:TemplateField ShowHeader="False" >
                        
                        <ItemTemplate>
                            
                                <asp:HiddenField runat="server" Value='<%# Bind("CategoryID") %>' ID="CategoryID"></asp:HiddenField>
                                <asp:Label runat="server" Text='<%# Bind("Description") %>' ID="Label1"></asp:Label>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Label runat="server" Text='<%# Bind("AmountOfParts") %>' ID="Label2"></asp:Label>
                            
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:ObjectDataSource ID="CategoryDataSource" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="ListAllCategories" TypeName="TeamCeBikeLab.BLL.SalesCRUD.SalesController"></asp:ObjectDataSource>
        </div>
        <div class="col-md-8">
            <h2>Products</h2>  
            <asp:GridView ID="ProductGridView" runat="server" AutoGenerateColumns="False" DataSourceID="ProductDataSource" AllowPaging="True" ShowHeader="False" GridLines="None" OnSelectedIndexChanging="ProductGridView_SelectedIndexChanging"   ItemType="TeamCeBikeLab.Entities.POCOs.PartInfo" CssClass="table table-condensed">
                <Columns>
                    <asp:CommandField ShowSelectButton="true" ButtonType="button" SelectText="Add"></asp:CommandField>
                    <asp:TemplateField ShowHeader="false">
                        
                        <ItemTemplate>
                            <asp:HiddenField runat="server" Value='<%# Bind("PartID") %>' ID="PartID"></asp:HiddenField>
                            <asp:Label ID="QuantityInCart" runat="server" Text='<%# Item.QuantityInCart != 0?Item.QuantityInCart.ToString():""  %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ShowHeader="false">
                        
                        <ItemTemplate>
                            <asp:TextBox ID="Qutity" runat="server" Text="1"></asp:TextBox>
                            
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField ShowHeader="false">
                        
                        <ItemTemplate>
                            <asp:Label runat="server" Text='<%# Item.SellingPrice.ToString("C") %>' ID="Label3" ></asp:Label>&nbsp;
                            <asp:Label runat="server" Text='<%# Bind("Description") %>' ID="Label2"></asp:Label>&nbsp;
                            <asp:Label runat="server" Text='<%# Bind("QuantityOnHand") %>' ID="Label4"></asp:Label>&nbsp;
                            in stock
                            
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:LinkButton ID="CheckOut" runat="server" OnClick="CheckOut_Click" CssClass="btn btn-default">View Cart</asp:LinkButton>
            <asp:ObjectDataSource ID="ProductDataSource" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="ListPartsByCategory" TypeName="TeamCeBikeLab.BLL.SalesCRUD.SalesController">
                <SelectParameters>
                    <asp:ControlParameter ControlID="CategoryGridView" PropertyName="SelectedValue" Name="categoryId" Type="Int32" DefaultValue="-1"></asp:ControlParameter>
                    <asp:ControlParameter ControlID="UserName" PropertyName="Value" DefaultValue=" " Name="name" Type="String"></asp:ControlParameter>
                </SelectParameters>
            </asp:ObjectDataSource>
        </div>
        
    </asp:Panel>

    <asp:Panel ID="NavigationPanel" runat="server" Visible="false">
        <div class="row">
            <div class="col-md-4">
                <asp:LinkButton ID="ViewCartNav" runat="server" Text="View Cart" OnClick="CheckOut_Click"></asp:LinkButton>
                <ul>
                    <li>change qty</li>
                    <li>Remove item</li>
                    <li><asp:LinkButton ID="ContinueShopping" runat="server" OnClick="ContinueShoppingButton_Click">Continue shopping</asp:LinkButton></li>
                </ul>
            </div>
            <div class="col-md-4">
                <asp:LinkButton ID="LinkButton2" runat="server" Text="Purchase Info" OnClick="GoPurchaseInfoButton_Click"></asp:LinkButton>
                <ul>
                    <li>Customer Info</li>
                    <li>How to purchase/pay type</li>
                </ul>
            </div>
            <div class="col-md-4">
                <asp:LinkButton ID="LinkButton4" runat="server" Text="Place order" OnClick="GoPlaceOrderButton_Click"></asp:LinkButton>
                <ul>
                    <li>Customer info</li>
                    <li>Coupon_Apply</li>
                    <li>subtotal/GST/Total</li>
                    <li><asp:LinkButton ID="LinkButton5" runat="server" OnClick="GoPlaceOrderButton_Click">Place Order</asp:LinkButton></li>
                </ul>
            </div>
        </div>
        
        
        
    </asp:Panel>

    <asp:Panel ID="ViewCartPanel" runat="server" Visible="false" CssClass="row">
        <h2>Your Shopping Cart</h2>
        <asp:GridView ID="CartInfoGridView" runat="server" AutoGenerateColumns="False" ItemType="TeamCeBikeLab.Entities.POCOs.CartInfo" ShowHeader="False" GridLines="None" OnRowCommand="CartInfoGridView_RowCommand" OnRowDeleting="CartInfoGridView_RowDeleting" OnRowUpdating="CartInfoGridView_RowUpdating" CssClass="table table-hover  table-condensed">

            <Columns>
                <asp:TemplateField ShowHeader="false">
                    <ItemTemplate>
                        <asp:Label runat="server" Text='<%# Bind("Description") %>' ID="Label1"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="false">                    
                    <ItemTemplate>
                        <asp:Label runat="server" Text='<%# Bind("Quantity") %>' ID="Quantity"></asp:Label>
                        <asp:HiddenField ID="QOH" runat="server" Value="<%# Item.QOH %>" />
                        
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="false">
                    <ItemTemplate>
                        <asp:Label runat="server" Text='<%# Item.SellingPrice.ToString("C") %>' ID="Label2"></asp:Label>ea
                        <asp:Label ID="TotalPrice" runat="server" Text='<%# Item.QOH>=Item.Quantity?(Item.SellingPrice * Item.Quantity).ToString("C"):(Item.SellingPrice * Item.QOH).ToString("C") %>' ></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                
                <asp:TemplateField ShowHeader="false">                    
                    <ItemTemplate>
                        <asp:LinkButton ID="Remove" runat="server" CommandName="Delete" CommandArgument="<%# Item.CartItemId %>" CssClass="btn btn-danger" ToolTip="Remove Item"><i class="glyphicon glyphicon-remove"></i></asp:LinkButton>
                        <asp:TextBox ID="QutityChanged" runat="server" Text='<%# Bind("Quantity") %>'></asp:TextBox>
                        <asp:LinkButton ID="Refresh" runat="server" CommandName="Update" CommandArgument="<%# Item.CartItemId.ToString() + ';' + ((GridViewRow)Container).RowIndex.ToString()%>" CssClass="btn btn-info" ToolTip="Update Totals"><i class="glyphicon glyphicon-refresh"></i></asp:LinkButton>
                        <asp:HiddenField runat="server" Value='<%# String.Format("{0:MM/d/yyyy}",Item.UpdatedOn)%>' ID="UpdatedOn"></asp:HiddenField>
                        <asp:Label ID="OutOfStockLabel" runat="server" CssClass="bg-danger"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>
        </asp:GridView>
        <asp:Label ID="Label5" runat="server">Total: </asp:Label>
        <asp:Label ID="TotalPrice" runat="server"></asp:Label><br />
        <asp:Label ID="ItemAmountUpdateOn" runat="server"></asp:Label><br />
        <asp:LinkButton ID="ContinueShoppingButton" runat="server" OnClick="ContinueShoppingButton_Click" CssClass="btn btn-primary">Continue Shopping</asp:LinkButton>
        <asp:LinkButton ID="PurchaseInfoButton" runat="server" OnClick="PurchaseInfoButton_Click" CssClass="btn btn-primary">Continue</asp:LinkButton>
    </asp:Panel>
    <asp:Panel ID="PurchaseInfoPanel" runat="server" Visible="false">
        <h2>Purchase Detail</h2>
        <p>Enter your information for shipping and billing here</p>
        
        <form ID="PurchaseDetails"  class="form-horizontal">
            <h3>Billing Details</h3>
            <div class="form-group">
                <asp:Label ID="NameLabel" runat="server" CssClass="col-sm-2 control-label">Name</asp:Label>
                <div class="col-sm-10">
                    <asp:TextBox  CssClass="form-control" ID="NameTextBox" runat="server"></asp:TextBox>
                </div>
            </div>

            <div class="form-group">
                <asp:Label ID="Label9" runat="server" CssClass="col-sm-2 control-label">Email</asp:Label>
                <div class="col-sm-10">
                    <asp:TextBox  CssClass="form-control" ID="TextBox1" runat="server"></asp:TextBox>
                </div>
            </div>

            <div class="form-group">
                <asp:Label ID="Label10" runat="server" CssClass="col-sm-2 control-label">Address</asp:Label>
                <div class="col-sm-10">
                    <asp:TextBox  CssClass="form-control" ID="TextBox2" runat="server"></asp:TextBox>
                </div>
            </div>

            <div class="form-group">
                <asp:Label ID="Label11" runat="server" CssClass="col-sm-2 control-label">Phone</asp:Label>
                <div class="col-sm-10">
                    <asp:TextBox  CssClass="form-control" ID="TextBox3" runat="server"></asp:TextBox>
                </div>
            </div>

            <h3>Shipping Details</h3>
            <div class="form-group">
                <asp:Label ID="Label12" runat="server" CssClass="col-sm-2 control-label">Name</asp:Label>
                <div class="col-sm-10">
                    <asp:TextBox  CssClass="form-control" ID="TextBox4" runat="server"></asp:TextBox>
                </div>
            </div>

            <div class="form-group">
                <asp:Label ID="Label13" runat="server" CssClass="col-sm-2 control-label">Email</asp:Label>
                <div class="col-sm-10">
                    <asp:TextBox  CssClass="form-control" ID="TextBox5" runat="server"></asp:TextBox>
                </div>
            </div>

            <div class="form-group">
                <asp:Label ID="Label14" runat="server" CssClass="col-sm-2 control-label">Address</asp:Label>
                <div class="col-sm-10">
                    <asp:TextBox  CssClass="form-control" ID="TextBox6" runat="server"></asp:TextBox>
                </div>
            </div>

            <div class="form-group">
                <asp:Label ID="Label15" runat="server" CssClass="col-sm-2 control-label">Phone</asp:Label>
                <div class="col-sm-10">
                    <asp:TextBox  CssClass="form-control" ID="TextBox7" runat="server"></asp:TextBox>
                </div>
            </div>
        </form>
        <asp:LinkButton ID="LinkButton1" runat="server" OnClick="CheckOut_Click" CssClass="btn btn-primary">Back</asp:LinkButton>
        <asp:LinkButton ID="GoPlaceOrderButton" runat="server" OnClick="GoPlaceOrderButton_Click" CssClass="btn btn-primary">Continue</asp:LinkButton>
    </asp:Panel>

    <asp:Panel ID="PlaceOrderPanel" runat="server" Visible="false">
        <h2>Your Shopping Cart</h2>
        <asp:Label ID="ItemAmountUpdateDateOnPlaceOrderPage" runat="server"></asp:Label>
        <asp:GridView ID="PlaceOrderCartInfoGridView" runat="server" ItemType="TeamCeBikeLab.Entities.POCOs.CartInfo" AutoGenerateColumns="false" ShowHeader="False" GridLines="None" CssClass="table table-hover  table-condensed">
            <Columns>
                <asp:TemplateField ShowHeader="false">
                    <ItemTemplate>
                        <asp:HiddenField runat="server" Value='<%# String.Format("{0:MM/d/yyyy}",Item.UpdatedOn) %>' ID="UpdatedOn"></asp:HiddenField>
                        <asp:HiddenField ID="QOH" runat="server" Value="<%# Item.QOH %>" />
                        <asp:Label runat="server" Text='<%# Bind("Description") %>' ID="Label1"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="false">                    
                    <ItemTemplate>
                        <asp:Label runat="server" Text='<%# Bind("Quantity") %>' ID="Quantity"></asp:Label>
                        <asp:Label ID="OutOfStockLabel" runat="server"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="false">
                    <ItemTemplate>
                        <asp:Label runat="server" Text='<%# Item.SellingPrice.ToString("C") %>' ID="Label2"></asp:Label>ea
                        <asp:Label ID="TotalPrice" runat="server" Text='<%# Item.QOH>=Item.Quantity?(Item.SellingPrice * Item.Quantity).ToString("C"):(0).ToString("C") %>' ></asp:Label>
                        <asp:Label ID="OutOfStockLabelInPlaceOrderPage" runat="server" CssClass="bg-danger"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>

        <div class="row">
            <div class="col-md-4">
                 <asp:TextBox ID="CouponTextBox" runat="server" Text="OilChg01"></asp:TextBox>
                 <asp:LinkButton ID="Refresh" runat="server"  CssClass="btn btn-info" ToolTip="Update Discount" OnClick="Refresh_Click"><i class="glyphicon glyphicon-refresh"></i></asp:LinkButton>
            </div>
            <div class="col-md-4">
                <asp:Label ID="Label6" runat="server">SubTotal: </asp:Label>
                <asp:Label ID="SubTotal" runat="server"></asp:Label><br />
                <asp:Label ID="Label7" runat="server">Discount: </asp:Label>
                <asp:Label ID="Discount" runat="server">$0.00</asp:Label><br />
                <asp:Label ID="Label8" runat="server">Total: </asp:Label>
                <asp:Label ID="Total" runat="server"></asp:Label><br />
            </div>
            <div class="col-md-4">
                <asp:RadioButtonList ID="paymentRadioButton" runat="server">
                    <asp:ListItem Text="Credit" Value="C" Selected="True"/>
                    <asp:ListItem Text="Debit" Value="D" />
                </asp:RadioButtonList>
                <asp:LinkButton ID="PlaceOrderButton" runat="server" OnClick="PlaceOrderButton_Click" Text="Place Order" CssClass="btn btn-primary"></asp:LinkButton>
            </div>
        </div>
        <asp:LinkButton ID="GoPurchaseInfoButton" runat="server" OnClick="GoPurchaseInfoButton_Click" CssClass="btn btn-primary">Back</asp:LinkButton>
    </asp:Panel>
</asp:Content>
