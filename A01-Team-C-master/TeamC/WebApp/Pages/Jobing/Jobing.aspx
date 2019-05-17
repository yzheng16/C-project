<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Jobing.aspx.cs" Inherits="WebApp.Pages.Jobing.Jobing" %>

<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="jumbotron">
        <h1 id="pageTitle" runat="server">Current Job List</h1>
        <uc1:MessageUserControl runat="server" ID="MessageUserControl" />
    </div>


    <div class="row">
        <asp:Label ID="Label1" runat="server" Text="Employee: " Font-Bold="true"></asp:Label>
        <asp:Label ID="EmployeeNameLabel" runat="server" Text=""></asp:Label>
        <asp:Button ID="NewJobButton" runat="server" Text="New Job" OnClick="NewJobButton_Click" />
        <span id="NewJobCustomer" runat="server" visible="false">
            <asp:Label ID="Label14" runat="server" Text="Customer: " Font-Bold="true"></asp:Label>
            <asp:DropDownList ID="CustomerDDL" runat="server" DataSourceID="CustomerDataSource" DataTextField="FullName" DataValueField="CustomerID" AppendDataBoundItems="true">
                <asp:ListItem Value="0">Select...</asp:ListItem>
            </asp:DropDownList>
        </span>
        <span id="jobInfo" runat="server" visible="false">
            <asp:Label ID="Label9" runat="server" Text="Job: " Font-Bold="true"></asp:Label>
            <asp:Label ID="JobNumberLabel" runat="server" Text=""></asp:Label>
            <asp:Label ID="Label10" runat="server" Text="Customer: " Font-Bold="true"></asp:Label>

            <asp:Label ID="CustomerLabel" runat="server" Text=""></asp:Label>
            <asp:Label ID="Label11" runat="server" Text="Contact: " Font-Bold="true"></asp:Label>
            <asp:Label ID="ContactLabel" runat="server" Text=""></asp:Label>
        </span>

    </div>
    <asp:Panel ID="currentJobListForm" runat="server">
        <div class="row">
            <asp:GridView ID="JobGridView" runat="server" AutoGenerateColumns="false" CssClass="table" OnRowCommand="JobGridView_RowCommand" DataKeyNames="JobID">
                <Columns>
                    <asp:BoundField DataField="JobID" HeaderText="Job ID" />
                    <asp:BoundField DataField="JobDateIn" HeaderText="In" />
                    <asp:BoundField DataField="JobDateStarted" HeaderText="Started" />
                    <asp:BoundField DataField="JobDateDone" HeaderText="Done" />
                    <asp:BoundField DataField="CustomerName" HeaderText="Customer" />
                    <asp:BoundField DataField="CustomerPhone" HeaderText="Contact Number" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="ViewButton" runat="server" CommandName="CurrentJob">View</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="ServingButton" runat="server" CommandName="CurrentJobService">Serving</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </asp:Panel>

    <asp:Panel ID="NewJobForm" runat="server" Visible="false">
        <div class="row">
            <asp:Label ID="Label12" runat="server" Text="Shop Rate" Font-Bold="true"></asp:Label>
            <asp:TextBox ID="ShopRateTB" runat="server" Text="80"></asp:TextBox>
            <asp:Label ID="Label13" runat="server" Text="Vehicle ID" Font-Bold="true"></asp:Label>
            <asp:TextBox ID="VehicleTB" runat="server"></asp:TextBox>
            <h2>Service Details</h2>
        </div>

        <asp:ObjectDataSource ID="CustomerDataSource" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="ListCustomers" TypeName="TeamCeBikeLab.BLL.JobingCRUD.JobController"></asp:ObjectDataSource>
    </asp:Panel>


    <asp:Panel ID="currentJobForm" runat="server" Visible="false">
        <div class="row">
            <div class="row">
                <asp:Label ID="Label2" runat="server" Text="Description " Font-Bold="true"></asp:Label>
                <asp:TextBox ID="DescriptionTB" runat="server"></asp:TextBox>
                <asp:Button ID="AddServiceButton" runat="server" Text="Add Service" OnClick="AddServiceButton_Click" />
                <asp:Button ID="AddJobButton" runat="server" Text="Add Job" Visible="false" OnClick="AddJobButton_Click" />
            </div>
            <div class="row">
                <asp:Label ID="Label3" runat="server" Text="Coupon " Font-Bold="true"></asp:Label>
                <asp:DropDownList ID="CouponDDL" runat="server" DataSourceID="CouponDataSource" DataTextField="CouponIDValue" DataValueField="CouponID" AppendDataBoundItems="true">
                    <asp:ListItem Value="0">Select...</asp:ListItem>
                </asp:DropDownList>
                <asp:Label ID="Label4" runat="server" Text="Hours " Font-Bold="true"></asp:Label>
                <asp:TextBox ID="HoursTB" runat="server" Width="51px"></asp:TextBox>
            </div>
            <div class="row">
                <asp:Label ID="Label5" runat="server" Text="Comments" Font-Bold="true"></asp:Label>
                <asp:TextBox ID="CommentsTB" runat="server" Height="82px" Width="403px"></asp:TextBox>
            </div>
            <div class="row">
                <asp:GridView ID="CurrentJobGridView" runat="server" CssClass="table" OnRowCommand="CurrentJobGridView_RowCommand" AutoGenerateColumns="false" DataKeyNames="ServiceDetailID">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="RemoveButton" runat="server">Remove</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Description" HeaderText="Description" />
                        <asp:BoundField DataField="JobHours" HeaderText="Hours" />
                        <asp:BoundField DataField="CouponIDValue" HeaderText="Coupon" />
                        <asp:BoundField DataField="Comments" HeaderText="Comments" />
                    </Columns>
                </asp:GridView>

            </div>

            <asp:ObjectDataSource ID="CouponDataSource" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="ListCoupons" TypeName="TeamCeBikeLab.BLL.JobingCRUD.JobController"></asp:ObjectDataSource>
        </div>
    </asp:Panel>


    <asp:Panel ID="currentJobService" runat="server" Visible="false">
        <div class="row">
            <div class="row">
                <h2>Services</h2>
                <asp:GridView ID="ServicesGridView" runat="server" CssClass="table" AutoGenerateColumns="false" OnRowCommand="ServicesGridView_RowCommand" DataKeyNames="ServiceDetailID">
                    <Columns>
                        <asp:BoundField DataField="Description" HeaderText="Description" />
                        <asp:BoundField DataField="Status" HeaderText="Status" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="ViewServiceButton" runat="server" CommandName="view">View</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="StarteButton" runat="server" CommandName="start">Start</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="DoneButton" runat="server" CommandName="done">Done</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="RemoveButton" runat="server" CommandName="remove">Remove</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <asp:Panel ID="ViewServiceDetails" runat="server" Visible="false">
                <div class="row">
                    <asp:Label ID="Label6" runat="server" Text="Description" Font-Bold="true"></asp:Label>
                    <asp:Label ID="DescriptionLabel" runat="server" Text=""></asp:Label>
                    <asp:Label ID="Label7" runat="server" Text="Hours" Font-Bold="true"></asp:Label>
                    <asp:Label ID="HoursLabel" runat="server" Text=""></asp:Label>
                    <asp:Label ID="ServiceDetailIdLabel" runat="server" Text="" Visible="false"></asp:Label>
                </div>
                <div class="row">
                    <asp:Label ID="Label8" runat="server" Text="Comments" Font-Bold="true"></asp:Label>
                    <asp:Label ID="CommentsLabel" runat="server" Text=""></asp:Label>
                </div>
                <div class="row">
                    <asp:Button ID="AddCommentButton" runat="server" Text="Add" OnClick="AddCommentButton_Click" />
                    <asp:TextBox ID="CommentTB" runat="server" placeholder="Add any additional comments" Text="" Width="300px"></asp:TextBox>
                </div>
                <div class="row">
                    <asp:ListView ID="PartsListView" runat="server" DataSourceID="PartsObjectDataSource" InsertItemPosition="LastItem" DataKeyNames="ServiceDetailPartID, ServiceDetailID, PartID" OnItemInserting="PartsListView_ItemInserting">
                        <AlternatingItemTemplate>
                            <tr style="">
                                <td>
                                    <asp:Button runat="server" CommandName="Delete" Text="Delete" ID="DeleteButton" />
                                    <asp:Button runat="server" CommandName="Edit" Text="Edit" ID="EditButton" />
                                </td>
                                <td>
                                    <asp:Label Text='<%# Eval("PartID") %>' runat="server" ID="PartIDLabel" /></td>
                                <td>
                                    <asp:Label Text='<%# Eval("Description") %>' runat="server" ID="DescriptionLabel" /></td>
                                <td>
                                    <asp:Label Text='<%# Eval("Quantity") %>' runat="server" ID="QuantityLabel" /></td>
                            </tr>
                        </AlternatingItemTemplate>
                        <EditItemTemplate>
                            <tr style="">
                                <td>
                                    <asp:Button runat="server" CommandName="Update" Text="Update" ID="UpdateButton" />
                                    <asp:Button runat="server" CommandName="Cancel" Text="Cancel" ID="CancelButton" />
                                </td>
                                <td>
                                    <asp:TextBox Text='<%# Bind("PartID") %>' runat="server" ID="PartIDTextBox" Enabled="false" /></td>
                                <td>
                                    <asp:TextBox Text='<%# Bind("Description") %>' runat="server" ID="DescriptionTextBox" Enabled="false" /></td>
                                <td>
                                    <asp:TextBox Text='<%# Bind("Quantity") %>' runat="server" ID="QuantityTextBox" /></td>
                            </tr>
                        </EditItemTemplate>
                        <EmptyDataTemplate>
                            <table runat="server" style="">
                                <tr>
                                    <td>No data was returned.</td>
                                </tr>
                            </table>
                        </EmptyDataTemplate>
                        <InsertItemTemplate>
                            <tr style="">
                                <td>
                                    <asp:Button runat="server" CommandName="Insert" Text="Insert" ID="InsertButton" />
                                    <asp:Button runat="server" CommandName="Cancel" Text="Clear" ID="CancelButton" />
                                </td>
                                <td>
                                    <asp:TextBox Text='<%# Bind("PartID") %>' runat="server" ID="PartIDTextBox" /></td>
                                <td>
                                    <asp:TextBox Text='<%# Bind("Description") %>' runat="server" ID="DescriptionTextBox" Enabled="false" /></td>
                                <td>
                                    <asp:TextBox Text='<%# Bind("Quantity") %>' runat="server" ID="QuantityTextBox" /></td>
                                <td>
                                    <asp:TextBox Text='<%# Bind("ServiceDetailID") %>' runat="server" ID="TextBox1" hidden="true" /></td>
                            </tr>
                        </InsertItemTemplate>
                        <ItemTemplate>
                            <tr style="">
                                <td>
                                    <asp:Button runat="server" CommandName="Delete" Text="Delete" ID="DeleteButton" />
                                    <asp:Button runat="server" CommandName="Edit" Text="Edit" ID="EditButton" />
                                </td>
                                <td>
                                    <asp:Label Text='<%# Eval("PartID") %>' runat="server" ID="PartIDLabel" /></td>
                                <td>
                                    <asp:Label Text='<%# Eval("Description") %>' runat="server" ID="DescriptionLabel" /></td>
                                <td>
                                    <asp:Label Text='<%# Eval("Quantity") %>' runat="server" ID="QuantityLabel" /></td>
                            </tr>
                        </ItemTemplate>
                        <LayoutTemplate>
                            <table runat="server">
                                <tr runat="server">
                                    <td runat="server">
                                        <table class="table" runat="server" id="itemPlaceholderContainer" style="" border="0">
                                            <tr runat="server" style="">
                                                <th runat="server"></th>
                                                <th runat="server">PartID</th>
                                                <th runat="server">Description</th>
                                                <th runat="server">Quantity</th>
                                            </tr>
                                            <tr runat="server" id="itemPlaceholder"></tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr runat="server">
                                    <td runat="server" style=""></td>
                                </tr>
                            </table>
                        </LayoutTemplate>
                        <SelectedItemTemplate>
                            <tr style="">
                                <td>
                                    <asp:Button runat="server" CommandName="Delete" Text="Delete" ID="DeleteButton" />
                                    <asp:Button runat="server" CommandName="Edit" Text="Edit" ID="EditButton" />
                                </td>
                                <td>
                                    <asp:Label Text='<%# Eval("PartID") %>' runat="server" ID="PartIDLabel" /></td>
                                <td>
                                    <asp:Label Text='<%# Eval("Description") %>' runat="server" ID="DescriptionLabel" /></td>
                                <td>
                                    <asp:Label Text='<%# Eval("Quantity") %>' runat="server" ID="QuantityLabel" /></td>
                                <td>
                                    <asp:TextBox Text='<%# Bind("ServiceDetailID") %>' runat="server" ID="TextBox1" hidden="true" /></td>
                            </tr>
                        </SelectedItemTemplate>
                    </asp:ListView>
                </div>

                <asp:ObjectDataSource ID="PartsObjectDataSource" runat="server" DataObjectTypeName="TeamCeBikeLab.Entities.POCOs.ServiceDetailPartsPoco" DeleteMethod="DeletePart" InsertMethod="AddPart" OldValuesParameterFormatString="original_{0}" SelectMethod="ListServiceDetailPart" TypeName="TeamCeBikeLab.BLL.JobingCRUD.JobController" UpdateMethod="UpdatePart"
                    OnUpdated="CheckForExceptions"
                    OnDeleted="CheckForExceptions"
                    OnInserted="CheckForExceptions">
                    <SelectParameters>
                        <asp:Parameter Name="serviceDetailId" Type="Int32" DefaultValue="xyz"></asp:Parameter>
                    </SelectParameters>
                </asp:ObjectDataSource>
            </asp:Panel>


        </div>
    </asp:Panel>



</asp:Content>
