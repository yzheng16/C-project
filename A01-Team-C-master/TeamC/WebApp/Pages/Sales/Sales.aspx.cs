using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TeamCeBikeLab.BLL;
using TeamCeBikeLab.BLL.SalesCRUD;
using WebApp.Admin.Security;

namespace WebApp.Pages.Sales
{
    public partial class Sales : System.Web.UI.Page
    {
        protected void Page_PreRender(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                MessageUserControl.ShowInfo("Once you login you can add parts.");
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() => {
            //have your log in yte
                if (!Request.IsAuthenticated)
                {
                    ProductGridView.Columns[0].Visible = false;
                    ProductGridView.Columns[2].Visible = false;
                    var controller = new SalesController();
                    AllPartsAmount.Text = controller.getAllPartsAmount().ToString();
                }
                else
                {
                    UserName.Value = User.Identity.Name;
                    ProductGridView.Columns[0].Visible = true;
                    ProductGridView.Columns[2].Visible = true;
                    var controller = new SalesController();
                    AllPartsAmount.Text = controller.getAllPartsAmount().ToString();
                    ProductGridView.DataBind();
                }

            });
        }

        protected void All_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Pages/Sales/Sales.aspx", true);
            ProductGridView.DataBind();
            //QUESTION
        }


        protected void ProductGridView_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                GridViewRow row = ProductGridView.Rows[e.NewSelectedIndex];

                int partId = int.Parse((row.FindControl("PartID") as HiddenField).Value);
                var qutity = int.Parse((row.FindControl("Qutity") as TextBox).Text);

                if (qutity > 0)
                {
                    var securityController = new SecurityController();
                    var employeeId = securityController.GetCurrentUserEmployeeId(User.Identity.Name);
                    if (employeeId != null || User.IsInRole("Administrators"))
                    {
                        throw new Exception("Employee or Administrators can't shopping");
                    }
                    else
                    {
                        var controller = new SalesController();
                        controller.AddToCart(User.Identity.Name, partId, qutity);
                        //Item.QuantityInCart != 0?Item.QuantityInCart.ToString():""
                        ProductGridView.DataBind();
                        CategoryGridView.DataBind();
                    }

                }
                else
                {
                    throw new Exception("Quantities should geater than 0");
                }

            }, "Successful", "you added a part");
        }

      

        protected void CartInfoGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                var controller = new SalesController();
                var cartItemId = 0;
                switch (e.CommandName)
                {
                    case "Delete":
                        cartItemId = int.Parse(e.CommandArgument.ToString());
                        controller.DeleteCartItem(cartItemId);
                        ProductGridView.DataBind();
                        break;
                    case "Update":
                        cartItemId = int.Parse(e.CommandArgument.ToString().Split(';')[0]);
                        var idx = int.Parse(e.CommandArgument.ToString().Split(';')[1]);
                        var quantity = int.Parse((CartInfoGridView.Rows[idx].FindControl("QutityChanged") as TextBox).Text);
                        controller.UpdateCartItem(cartItemId, quantity);
                        ProductGridView.DataBind();
                        break;
                }
            });

        }

        protected void CartInfoGridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            var controller = new SalesController();
            var data = controller.GetCartInfo(User.Identity.Name);
            
            var totalPrice = 0.0;
            var totalQutity = 0;
            string updatedOn = "";
            CartInfoGridView.DataSource = data;
            CartInfoGridView.DataBind();
            foreach (GridViewRow row in CartInfoGridView.Rows)
            {
                if (int.Parse((row.FindControl("QOH") as HiddenField).Value) - int.Parse((row.FindControl("Quantity") as Label).Text) < 0)
                {
                    (row.FindControl("OutOfStockLabel") as Label).Text = "Out of Stock. We only have " + int.Parse((row.FindControl("QOH") as HiddenField).Value) + " in stock.";
                    //have to change
                    //totalQutity = totalQutity + int.Parse((row.FindControl("QOH") as HiddenField).Value);

                }
                //else
                //{
                //    totalQutity = totalQutity + int.Parse((row.FindControl("Quantity") as Label).Text);

                //}
                totalQutity++;
                totalPrice = totalPrice + double.Parse((row.FindControl("TotalPrice") as Label).Text.Substring(1));

                updatedOn = (row.FindControl("UpdatedOn") as HiddenField).Value;
            }
            TotalPrice.Text = "" + totalPrice.ToString("C");
            ItemAmountUpdateOn.Text = totalQutity + " items in your cart( last updated on " + updatedOn + " )";
        }

        protected void CartInfoGridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            var controller = new SalesController();
            var data = controller.GetCartInfo(User.Identity.Name);
            var totalPrice = 0.0;
            var totalQutity = 0;
            string updatedOn = "";
            CartInfoGridView.DataSource = data;
            CartInfoGridView.DataBind();

            foreach (GridViewRow row in CartInfoGridView.Rows)
            {
                if (int.Parse((row.FindControl("QOH") as HiddenField).Value) - int.Parse((row.FindControl("Quantity") as Label).Text) < 0)
                {
                    (row.FindControl("OutOfStockLabel") as Label).Text = "Out of Stock. We only have " + int.Parse((row.FindControl("QOH") as HiddenField).Value) + " in stock.";
                    //have to change
                    //totalQutity = totalQutity + int.Parse((row.FindControl("QOH") as HiddenField).Value);

                }
                //else
                //{
                //    totalQutity = totalQutity + int.Parse((row.FindControl("Quantity") as Label).Text);

                //}
                totalQutity++;
                totalPrice = totalPrice + double.Parse((row.FindControl("TotalPrice") as Label).Text.Substring(1));

                updatedOn = (row.FindControl("UpdatedOn") as HiddenField).Value;
            }
            TotalPrice.Text = "" + totalPrice.ToString("C");
            ItemAmountUpdateOn.Text = totalQutity + " items in your cart( last updated on " + updatedOn + " )";

        }

        protected void CheckOut_Click(object sender, EventArgs e)
        {
            PartCatelogPanel.Visible = false;
            ViewCartPanel.Visible = true;
            NavigationPanel.Visible = true;
            PurchaseInfoPanel.Visible = false;
            PlaceOrderPanel.Visible = false;

            var controller = new SalesController();
            var data = controller.GetCartInfo(User.Identity.Name);
            var totalPrice = 0.0;
            var totalQutity = 0;
            string updatedOn = "";
            CartInfoGridView.DataSource = data;
            CartInfoGridView.DataBind();

            foreach(GridViewRow row in CartInfoGridView.Rows)
            {
                if(int.Parse((row.FindControl("QOH") as HiddenField).Value) - int.Parse((row.FindControl("Quantity") as Label).Text) < 0)
                {
                    (row.FindControl("OutOfStockLabel") as Label).Text = "Out of Stock. We only have " + int.Parse((row.FindControl("QOH") as HiddenField).Value) + " in stock.";
                    //have to change
                    //totalQutity = totalQutity + int.Parse((row.FindControl("QOH") as HiddenField).Value);

                }
                //else
                //{
                //    totalQutity = totalQutity + int.Parse((row.FindControl("Quantity") as Label).Text);

                //}
                totalQutity++;
                
                totalPrice = totalPrice + double.Parse((row.FindControl("TotalPrice") as Label).Text.Substring(1));
                
                updatedOn = (row.FindControl("UpdatedOn") as HiddenField).Value;
                //updatedOn = String.Format("{0:MM/d/yyyy}", (row.FindControl("UpdatedOn") as HiddenField).Value);
            }
            TotalPrice.Text = "" + totalPrice.ToString("C");
            ItemAmountUpdateOn.Text = totalQutity + " items in your cart( last updated on " + updatedOn + " )";
        }

        protected void PlaceOrderButton_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() => {
                var controller = new SalesController();
                var paymentType = paymentRadioButton.SelectedValue;
                var coupon = controller.GetCoupon(CouponTextBox.Text);

                if(coupon != null)
                {
                    controller.PlaceOrder(User.Identity.Name, paymentType, coupon.CouponID);
                }
                else
                {
                    controller.PlaceOrder(User.Identity.Name, paymentType, 0);
                }
                
                
            },"Place Order","You place a order, thank you for shopping");
            
        }

        protected void PurchaseInfoButton_Click(object sender, EventArgs e)
        {
            PartCatelogPanel.Visible = false;
            ViewCartPanel.Visible = false;
            PlaceOrderPanel.Visible = false;
            NavigationPanel.Visible = true;
            PurchaseInfoPanel.Visible = true;
        }

        protected void GoPlaceOrderButton_Click(object sender, EventArgs e)
        {
            PurchaseInfoPanel.Visible = false;
            PlaceOrderPanel.Visible = true;
            ViewCartPanel.Visible = false;
            PartCatelogPanel.Visible = false;
            NavigationPanel.Visible = true;


            var subTotalPrice = 0.0;
            var totalQutity = 0;
            string updatedOn = "";

            var controller = new SalesController();
            var data = controller.GetCartInfo(User.Identity.Name);
            PlaceOrderCartInfoGridView.DataSource = data;
            PlaceOrderCartInfoGridView.DataBind();

            foreach (GridViewRow row in PlaceOrderCartInfoGridView.Rows)
            {
                if (int.Parse((row.FindControl("QOH") as HiddenField).Value) - int.Parse((row.FindControl("Quantity") as Label).Text) < 0)
                {
                    (row.FindControl("OutOfStockLabelInPlaceOrderPage") as Label).Text = "Out of Stock. We only have " + int.Parse((row.FindControl("QOH") as HiddenField).Value) + " in stock.";
                    
                    PlaceOrderButton.Text = "Order In-Stock Items Only";
                }
                else
                {
                    subTotalPrice = subTotalPrice + double.Parse((row.FindControl("TotalPrice") as Label).Text.Substring(1));
                }
                totalQutity++;
                
                
                updatedOn = (row.FindControl("UpdatedOn") as HiddenField).Value;
            }
            
            SubTotal.Text = "" + subTotalPrice.ToString("C");
            ItemAmountUpdateDateOnPlaceOrderPage.Text = totalQutity + " items in your cart( last updated on " + updatedOn + " )";
            Total.Text = subTotalPrice.ToString("C");
        }

        protected void GoPurchaseInfoButton_Click(object sender, EventArgs e)
        {
            ViewCartPanel.Visible = false;
            PlaceOrderPanel.Visible = false;
            PartCatelogPanel.Visible = false;
            PurchaseInfoPanel.Visible = true;
            NavigationPanel.Visible = true;
        }

        protected void Refresh_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() => {

                var controller = new SalesController();
                var coupon = controller.GetCoupon(CouponTextBox.Text);
                if(coupon == null)
                {
                    Discount.Text = "$0.00";
                    Total.Text = SubTotal.Text;
                    throw new Exception("This coupon doesn't exist. Try again.");
                }
                else
                {
                    Discount.Text = (coupon.CouponDiscount / 100.0 * double.Parse(SubTotal.Text.Substring(1))).ToString("C");
                    Total.Text = (double.Parse(SubTotal.Text.Substring(1)) - double.Parse(Discount.Text.Substring(1))).ToString("C");
                }
            },"Refresh","updated discount");
        }

        protected void ContinueShoppingButton_Click(object sender, EventArgs e)
        {
            PartCatelogPanel.Visible = true;
            PurchaseInfoPanel.Visible = false;
            NavigationPanel.Visible = false;
            ViewCartPanel.Visible = false;
            PlaceOrderPanel.Visible = false;
            ProductGridView.DataBind();
        }
        
    }
}