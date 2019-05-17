using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TeamCeBikeLab.BLL.Purchasing;
using TeamCeBikeLab.Entities.POCOs;
using WebApp.Admin.Security;

namespace WebApp.Pages.Purchasing
{
    public partial class Purchasing : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //THIS WORKS
            if(!EmployeeId.HasValue)
            {
                Response.Redirect("~", true);
            }
            if(!Request.IsAuthenticated || !User.IsInRole(Settings.PurchasingRole))
            {
                Response.Redirect("~", true);
            }
            if(EmployeeId.HasValue)
            {
                Employee.Text = EmployeeId.ToString();
            }
        }
        //THIS WORKS
        private int? EmployeeId
        { 
            get
            {
                int? id = null;
                if(Request.IsAuthenticated)
                {
                    var manager = Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
                    var appUser = manager.Users.SingleOrDefault(x => x.UserName == User.Identity.Name);
                    if (appUser != null)
                    {
                        id = appUser.EmployeeId;
                    }
                }
                return id;
            }
        }
        protected void Get_Create_PO_OnClick(object sender, EventArgs e)
        {
            int employeeID = int.Parse(Employee.Text);

            PurchasingController vendorController = new PurchasingController();
            var vendor = vendorController.Vendor_Get(int.Parse(VendorDropDownList.SelectedValue));
            int vendorID = int.Parse(VendorDropDownList.SelectedValue);

            VendorName.Text = vendor.VendorName;
            Phone.Text = vendor.Phone;
            Location.Text = vendor.City;

            PurchasingController purchaseOrderController = new PurchasingController();
            var purchaseOrderInfo = purchaseOrderController.GetActivePurchaseOrder(vendorID);
            if(purchaseOrderInfo == null)
            {
                purchaseOrderController.CreateSuggestedPurchaseOrder(vendorID, employeeID);
                purchaseOrderInfo = purchaseOrderController.GetActivePurchaseOrder(vendorID);
            }
            ActivePurchaseOrderDetailsGridView.DataSource = purchaseOrderController.GetCurrentActivePurchaseOrderDetails(vendorID);
            ActivePurchaseOrderDetailsGridView.DataBind();

            //Subtotal.Text = purchaseOrderInfo.SubTotal.ToString("C");
            Subtotal.Text = purchaseOrderInfo.SubTotal.ToString();
            //GST.Text = purchaseOrderInfo.TaxAmount.ToString("C");
            GST.Text = purchaseOrderInfo.TaxAmount.ToString();
            //Total.Text = (purchaseOrderInfo.SubTotal + purchaseOrderInfo.TaxAmount).ToString("C");
            Total.Text = (purchaseOrderInfo.SubTotal + purchaseOrderInfo.TaxAmount).ToString();

            PurchasingController inventoryController = new PurchasingController();
            var unorderedInventory = inventoryController.GetVendorStockItemsNotOnOrder(vendorID, purchaseOrderInfo);
            InventoryItemsGridView.DataSource = unorderedInventory;
            InventoryItemsGridView.DataBind();

        }
        protected void Update_OnClick(object sender, EventArgs e)
        {
            //Will do a bulk update processing of the currently displayed purchase order
            //Will update the Quantity and the PurchasePrice

            MessageUserControl.TryRun(() =>
            {
                PurchaseOrderInfo order = BuildPurchaseOrderInfo();
                int employeeID = int.Parse(Employee.Text);
                var controller = new PurchasingController();
                controller.UpdateOrder(order, employeeID);

                decimal subtotal = order.PurchaseOrderDetails.Sum(x => x.Quantity * x.PurchasePrice);
                Subtotal.Text = subtotal.ToString();
                decimal taxAmount = (0.05m * subtotal);
                GST.Text = taxAmount.ToString();
                decimal total = subtotal + taxAmount;
                Total.Text = total.ToString();


            }, "Updated", "Purchase Order has been updated");
            ActivePurchaseOrderDetailsGridView.DataSource = PartsFromPurchaseOrderDetailsGridView();
            ActivePurchaseOrderDetailsGridView.DataBind();
            InventoryItemsGridView.DataSource = PartsFromInventoryItemsGridView(InventoryItemsGridView);
            InventoryItemsGridView.DataBind();
        }

        private PurchaseOrderInfo BuildPurchaseOrderInfo()
        {
            PurchaseOrderInfo purchaseOrder = new PurchaseOrderInfo();
            int theVendorID = int.Parse(VendorDropDownList.SelectedValue);
            purchaseOrder.VendorID = theVendorID;

            int purchaseOrderID = 0;
            foreach (GridViewRow row in ActivePurchaseOrderDetailsGridView.Rows)
            {
                var thePurchaseOrderID = ActivePurchaseOrderDetailsGridView.Rows[row.RowIndex].Cells[0].Text;
                purchaseOrderID = int.Parse(thePurchaseOrderID);
            }
            purchaseOrder.PurchaseOrderID = purchaseOrderID;

            var theSubtotal = Subtotal.Text.ToString();
            decimal subTotal = decimal.Parse(theSubtotal);
            //decimal subTotal = decimal.Parse(Subtotal.Text.ToString());
            purchaseOrder.SubTotal = subTotal;
            decimal taxAmount = (0.05m * subTotal);
            purchaseOrder.TaxAmount = taxAmount;

            var items = PartsFromPurchaseOrderDetailsGridView();
            purchaseOrder.PurchaseOrderDetails = items.Select<PurchaseOrderDetails, PurchaseOrderDetails>(x => new PurchaseOrderDetails {ID = x.ID, Description = x.Description, QOH = x.QOH, ROL = x.ROL, QOO = x.QOO, Quantity = x.Quantity, PurchasePrice = x.PurchasePrice });

            return purchaseOrder;
        }

        protected void Place_OnClick(object sender, EventArgs e)
        {
            //BLL Method will take vendorID, orderID and data -- data = PurchaseOrderDetails
            //Will set the OrderDate and a new PurchaseOrderNumber for the current order - BLL
            //Will update the QuantityOnOrder for each part item on the purchase order 
            //OrderDate and PurchaseOrderNumber are found on the PurchaseOrder table
            //OrderDate = OrderDate = DateTime.Today
            //QuantityOnOrder is found on the Parts table
            MessageUserControl.TryRun(() =>
           {

               PurchaseOrderInfo order = BuildPurchaseOrderInfo();
               int employeeID = int.Parse(Employee.Text);
               int vendorID = int.Parse(VendorDropDownList.SelectedIndex.ToString());
               var controller = new PurchasingController();
               controller.PlaceOrder(vendorID, employeeID, order);
               
           }, "Placed", "Purchase Order has been placed.");
        }
        protected void Delete_OnClick(object sender, EventArgs e)
        {
            //Will delete the current open purchase order and purchase order details from the system
            //Check to see if the order is placed - if OrderDate == null && PurchaseOrderNumber == null
            //Need to delete PurchaseOrderDetails first then the PurchaseOrder
            
        }
        protected void Clear_OnClick(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                //Clear the web page of the current purchase order and reset the vendor list to the vendor prompt line
                VendorDropDownList.SelectedIndex = 0;
                Subtotal.Text = "";
                GST.Text = "";
                Total.Text = "";
                VendorName.Text = "";
                Location.Text = "";
                Phone.Text = "";
                ActivePurchaseOrderDetailsGridView.DataBind();
                InventoryItemsGridView.DataBind();
            }, "Cleared", "Web Page has been cleared.");
        }

        #region Remove and Add LinkButtons
        //ADD ROW COMMANDS TO GRID VIEW BEFORE RUNNING
       protected void ActivePurchaseOrderDetails_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView activePurchaseOrderDetailsGridView = ActivePurchaseOrderDetailsGridView;
            GridView inventoryGridView = InventoryItemsGridView;
            RemovePurchaseOrderDetail(e, activePurchaseOrderDetailsGridView, inventoryGridView);
        }

        protected void InventoryItems_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView activePurchaseOrderDetailsGridView = ActivePurchaseOrderDetailsGridView;
            GridView inventoryGridView = InventoryItemsGridView;
            AddPurchaseOrderDetail(e, activePurchaseOrderDetailsGridView, inventoryGridView);
        }

        private List<VendorStockItems> PartsFromInventoryItemsGridView (GridView gv)
        {
            //Grabbing all the rows from a GridView to create a list of objects
            List<VendorStockItems> vendorStockItems = new List<VendorStockItems>();
            foreach (GridViewRow row in gv.Rows)
            {
                int partID = int.Parse(row.Cells[0].Text);
                string description = row.Cells[1].Text;
                int quantityOnHand = int.Parse(row.Cells[2].Text);
                int quantityOnOrder = int.Parse(row.Cells[3].Text);
                int reorderLevel = int.Parse(row.Cells[4].Text);
                int buffer = int.Parse(row.Cells[5].Text);
                decimal purchasePrice = decimal.Parse(row.Cells[6].Text);

                //Creating a VendorStockItems object
                VendorStockItems item = new VendorStockItems(partID, description, quantityOnHand, quantityOnOrder, reorderLevel, buffer, purchasePrice);
                //Add it to my list
                vendorStockItems.Add(item);
            }
            return vendorStockItems;
        } 

        private List<PurchaseOrderDetails> PartsFromPurchaseOrderDetailsGridView ()
        {
            //Grabbing all the rows from a GridView to create a list of objects
            List<PurchaseOrderDetails> purchaseOrderDetails = new List<PurchaseOrderDetails>();
            foreach (GridViewRow row in ActivePurchaseOrderDetailsGridView.Rows)
            {
                PurchaseOrderDetails newPurchaseOrderDetail = new PurchaseOrderDetails
                {
                    PurchaseOrderID = int.Parse(ActivePurchaseOrderDetailsGridView.Rows[row.RowIndex].Cells[0].Text),
                    ID = int.Parse(ActivePurchaseOrderDetailsGridView.Rows[row.RowIndex].Cells[2].Text),
                    Description = ActivePurchaseOrderDetailsGridView.Rows[row.RowIndex].Cells[3].Text,
                    QOH = int.Parse(ActivePurchaseOrderDetailsGridView.Rows[row.RowIndex].Cells[4].Text),
                    ROL = int.Parse(ActivePurchaseOrderDetailsGridView.Rows[row.RowIndex].Cells[5].Text),
                    QOO = int.Parse(ActivePurchaseOrderDetailsGridView.Rows[row.RowIndex].Cells[6].Text),
                    Quantity = int.Parse((row.FindControl("Quantity") as TextBox).Text),
                    PurchasePrice = decimal.Parse((row.FindControl("PurchasePrice") as TextBox).Text),
                };
                //Add it to my list
                purchaseOrderDetails.Add(newPurchaseOrderDetail);
            }
            return purchaseOrderDetails;
        }

        private void RemovePurchaseOrderDetail (GridViewCommandEventArgs e, GridView activePurchaseOrderDetailsGridView, GridView inventoryGridView)
        {
            MessageUserControl.TryRun(() =>
            {
                //Take the contents out of the gridview and make them into a list
                List<VendorStockItems> vendorStockItems = new List<VendorStockItems>();
                vendorStockItems = PartsFromInventoryItemsGridView(inventoryGridView);

                //Get the GridViewRows so that we can get the partID to use to find the part to remove
                GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
                var thePartID = activePurchaseOrderDetailsGridView.Rows[row.RowIndex].Cells[2].Text;
                int partID = int.Parse(thePartID);

                //Grab a list of the purchase order details from the activ purchase order details grid view using the method we created earlier
                List<PurchaseOrderDetails> purchaseOrderDetails = new List<PurchaseOrderDetails>();
                purchaseOrderDetails = PartsFromPurchaseOrderDetailsGridView();

                //Find the item we want to remove from the list of purchase order details in the grid view by using its partID
                var itemToRemove = purchaseOrderDetails.Single(x => x.ID == partID);
                vendorStockItems.Add(new VendorStockItems
                {
                    ID = partID,
                    Description = itemToRemove.Description,
                    QOH = itemToRemove.QOH,
                    ROL = itemToRemove.ROL,
                    QOO = itemToRemove.QOO,
                    Price = itemToRemove.PurchasePrice
                });

                purchaseOrderDetails.Remove(itemToRemove);

                decimal subtotal = purchaseOrderDetails.Sum(x => x.Quantity * x.PurchasePrice);
                Subtotal.Text = subtotal.ToString();
                decimal taxAmount = (0.05m * subtotal);
                GST.Text = taxAmount.ToString();
                decimal total = subtotal + taxAmount;
                Total.Text = total.ToString();

                activePurchaseOrderDetailsGridView.DataSource = purchaseOrderDetails;
                activePurchaseOrderDetailsGridView.DataBind();
                inventoryGridView.DataSource = vendorStockItems;
                inventoryGridView.DataBind();
            }, "Purchase Order Detail Removed", "Part has been removed from purchase order."); 
        }

        private void AddPurchaseOrderDetail (GridViewCommandEventArgs e, GridView activePurchaseOrderDetailsGridView, GridView inventoryGridView)
        {
            MessageUserControl.TryRun(() =>
            {
                List<PurchaseOrderDetails> details = new List<PurchaseOrderDetails>();
                details = PartsFromPurchaseOrderDetailsGridView();

                List<VendorStockItems> inventory = new List<VendorStockItems>();
                inventory = PartsFromInventoryItemsGridView(inventoryGridView);

                GridViewRow row = (GridViewRow)(((Control)e.CommandSource).NamingContainer);
                var thePartID = inventoryGridView.Rows[row.RowIndex].Cells[0].Text;
                int partID = int.Parse(thePartID);

                var item = inventory.Single(x => x.ID == partID);
                details.Add(new PurchaseOrderDetails
                {
                    PurchaseOrderID = int.Parse(activePurchaseOrderDetailsGridView.Rows[row.RowIndex].Cells[0].Text),
                    ID = item.ID,
                    Description = item.Description,
                    QOH = item.QOH,
                    ROL = item.ROL,
                    QOO = item.QOO,
                    Quantity = 1,
                    PurchasePrice = item.Price
                });

                inventory.Remove(item);

                decimal subtotal = details.Sum(x => x.Quantity * x.PurchasePrice);
                Subtotal.Text = subtotal.ToString();
                decimal taxAmount = (0.05m * subtotal);
                GST.Text = taxAmount.ToString();
                decimal total = subtotal + taxAmount;
                Total.Text = total.ToString();

                activePurchaseOrderDetailsGridView.DataSource = details;
                activePurchaseOrderDetailsGridView.DataBind();
                inventoryGridView.DataSource = inventory;
                inventoryGridView.DataBind();
            }, "Purchase Order Detail Added", "Part has been added to the purchase order");
        }
        #endregion
    }
}