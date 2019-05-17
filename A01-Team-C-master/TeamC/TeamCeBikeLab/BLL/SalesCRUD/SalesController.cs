using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamCeBikeLab.DAL;
using TeamCeBikeLab.Entities;
using TeamCeBikeLab.Entities.POCOs;

namespace TeamCeBikeLab.BLL.SalesCRUD
{
    [DataObject]
    public class SalesController
    {
        #region Category
        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<CategoryInfo> ListAllCategories()
        {
            using (var context = new eBikeContext())
            {
                var result = from cat in context.Categories
                             select new CategoryInfo
                             {
                                 CategoryID = cat.CategoryID,
                                 Description = cat.Description,
                                 AmountOfParts = cat.Parts.Count(),
                             };
                return result.ToList();
            }
        }
        #endregion

        #region Part
        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<PartInfo> ListPartsByCategory(int categoryId, string name)
        {
            var shoppingCartId = GetShoppingCartId(name);
            using (var context = new eBikeContext())
            {
                
                if (categoryId < 0)
                {
                    var result = from part in context.Parts
                                 where part.Discontinued == false
                                 orderby part.Description
                                 select new PartInfo
                                 {
                                     PartID = part.PartID,
                                     Description = part.Description,
                                     SellingPrice = part.SellingPrice,
                                     QuantityOnHand = part.QuantityOnHand,
                                     QuantityInCart = (from cartItem in part.ShoppingCartItems
                                                       where cartItem.ShoppingCartID == shoppingCartId
                                                       select cartItem.Quantity).FirstOrDefault()
                                 };
                    return result.ToList();
                }
                else
                {
                    var result = from part in context.Parts
                                 where part.Discontinued == false && part.CategoryID == categoryId
                                 orderby part.Description
                                 select new PartInfo
                                 {
                                     PartID = part.PartID,
                                     Description = part.Description,
                                     SellingPrice = part.SellingPrice,
                                     QuantityOnHand = part.QuantityOnHand,
                                     QuantityInCart = (from cartItem in part.ShoppingCartItems
                                                       where cartItem.ShoppingCartID == shoppingCartId
                                                       select cartItem.Quantity).FirstOrDefault()
                                 };
                    return result.ToList();
                }

            }
        }

        public int getAllPartsAmount()
        {
            using (var context = new eBikeContext())
            {
                var amount = (from p in context.Parts select p).Count();
                return amount;
            }
        }

        public void AddToCart(string userName, int productId, int quantity)
        {

            
                //create a new shopping cart with item
            using (var context = new eBikeContext())
            {
                var shoppingCartId = GetShoppingCartId(userName);
                var customerId = GetCustomerId(userName);
                var qOH = GetQOH(productId);
                //0 means this customer don't have shoppingCart, 
                //-1 means this userName is not a online customer

                //this user is not onlineCustomer
                if (customerId == 0)
                {
                    //create a new onlineCustomer
                    var newOnlineCustomer = new OnlineCustomer();
                    newOnlineCustomer.UserName = userName;
                    newOnlineCustomer.CreatedOn = DateTime.Today;
                    context.OnlineCustomers.Add(newOnlineCustomer);

                    var newShoppingCart = context.ShoppingCarts.Add(new ShoppingCart());
                    newShoppingCart.OnlineCustomerID = GetCustomerId(userName);
                    newShoppingCart.CreatedOn = DateTime.Today;
                    newShoppingCart.UpdatedOn = DateTime.Today;

                    //customer can add as many items as they want
                    //if (qOH >= quantity)
                    //{
                        var newCartItem = new ShoppingCartItem();
                        newCartItem.PartID = productId;
                        newCartItem.Quantity = quantity;
                        newShoppingCart.ShoppingCartItems.Add(newCartItem);
                    //}
                    //else
                    //{
                        //throw new Exception("We don't have enough quantity on hand");
                    //}
                    
                    
                }
                else
                {
                    //this online customer doesn't have shopping cart
                    if (shoppingCartId == 0)
                    {
                        var newShoppingCart = context.ShoppingCarts.Add(new ShoppingCart());
                        newShoppingCart.OnlineCustomerID = GetCustomerId(userName);
                        newShoppingCart.CreatedOn = DateTime.Now;
                        newShoppingCart.UpdatedOn = DateTime.Today;

                        //customer can add as many items as they want
                        //if (qOH >= quantity) {
                        var newCartItem = new ShoppingCartItem();
                            newCartItem.PartID = productId;
                            newCartItem.Quantity = quantity;
                            newShoppingCart.ShoppingCartItems.Add(newCartItem);
                        //}
                        //else
                        //{
                            //throw new Exception("We don't have enough quantity on hand");
                        //}


                    }
                    else if (shoppingCartId != 0 && shoppingCartId != -1)//this online customer have a shopping cart
                    {
                        var existingShoppingCart = context.ShoppingCarts.Find(GetShoppingCartId(userName));
                        existingShoppingCart.UpdatedOn = DateTime.Today;

                        var cartItemId = GetCartItemId(shoppingCartId, productId);
                        if (cartItemId == 0)//in the shopping cart, customer doesn't have this part
                        {
                            //customer can add as many items as they want
                            //if (qOH >= quantity)
                            //{
                            //create new cartItem
                            var newCartItem = new ShoppingCartItem();
                            newCartItem.PartID = productId;
                            newCartItem.Quantity = quantity;
                            existingShoppingCart.ShoppingCartItems.Add(newCartItem);
                            //}
                            //else
                            //{
                                //throw new Exception("We don't have enough quantity on hand");
                            //}
                            

                        }
                        else//customer have this part
                        {
                            var exsitingCartItem = context.ShoppingCartItems.Find(cartItemId);
                            //customer can add as many items as they want
                            //if(qOH >= exsitingCartItem.Quantity + quantity)
                            //{
                            exsitingCartItem.Quantity = exsitingCartItem.Quantity + quantity;
                            //}
                            //else
                            //{
                                //throw new Exception("We don't have enough quantity on hand");
                            //}
                            
                        }
                    }
                }
                context.SaveChanges();

            }

            
        }

        

        public int GetCartItemId(int shoppingCartId, int productId)
        {
            using(var context = new eBikeContext())
            {
                var resultAmount = (from shoppingCarItem in context.ShoppingCartItems
                             where shoppingCarItem.PartID == productId && shoppingCarItem.ShoppingCartID == shoppingCartId
                             select shoppingCarItem.ShoppingCartItemID).Count();
                if(resultAmount == 0)
                {
                    return 0;
                }
                else
                {
                    var result = from shoppingCarItem in context.ShoppingCartItems
                                  where shoppingCarItem.PartID == productId && shoppingCarItem.ShoppingCartID == shoppingCartId
                                  select shoppingCarItem.ShoppingCartItemID;
                    return result.Single();
                }
            }
        }

        public int GetQOH(int partId)
        {
            using (var context = new eBikeContext())
            {
                var result = from part in context.Parts
                             where part.PartID == partId
                             select part.QuantityOnHand;
                return result.Single();
            }
        }

        #endregion

        #region Cart
        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<CartInfo> GetCartInfo(string userName)
        {
            var shoppingCartId = GetShoppingCartId(userName);
            using (var context = new eBikeContext())
            {
                var result = from shoppingCartItem in context.ShoppingCartItems
                             where shoppingCartItem.ShoppingCartID == shoppingCartId
                             select new CartInfo
                             {
                                 CartItemId = shoppingCartItem.ShoppingCartItemID,
                                 UpdatedOn = shoppingCartItem.ShoppingCart.UpdatedOn,
                                 Description = shoppingCartItem.Part.Description,
                                 SellingPrice = shoppingCartItem.Part.SellingPrice,
                                 Quantity = shoppingCartItem.Quantity,
                                 QOH = shoppingCartItem.Part.QuantityOnHand
                             };
                return result.ToList();
            }
        }

        public void DeleteCartItem(int cartItemId)
        {
            using (var context = new eBikeContext())
            {
                var existingCartItem = context.ShoppingCartItems.Find(cartItemId);
                var existingShoppingCart = context.ShoppingCarts.Find(existingCartItem.ShoppingCartID);
                existingShoppingCart.UpdatedOn = DateTime.Today;
                context.ShoppingCartItems.Remove(existingCartItem);
                context.SaveChanges();
            }
        }

        public void UpdateCartItem(int cartItemId, int quantity)
        {
            using (var context = new eBikeContext())
            {
                var existingCartItem = context.ShoppingCartItems.Find(cartItemId);
                var part = context.Parts.Find(existingCartItem.PartID);
                //customers can add as many items as they want
                //if(part.QuantityOnHand >= quantity)
                //{
                    existingCartItem.Quantity = quantity;
                //}
                //else
                //{
                    //throw new Exception("We don't have enough quantity on hand");
                //}
                

                var existingShoppingCart = context.ShoppingCarts.Find(existingCartItem.ShoppingCartID);
                existingShoppingCart.UpdatedOn = DateTime.Today;
                
                context.SaveChanges();
            }
        }

        public void PlaceOrder(string name, string paymentType, int couponId)
        {
            //coupon == 0 means didn't select a coupon
            var shoppingCartId = GetShoppingCartId(name);
            var cartItemIds = GetCartItemIdsByShoppingCartId(shoppingCartId);
            
            using (var context = new eBikeContext())
            {
                var coupon = context.Coupons.Find(couponId);
                
                var saleDetailPoco = (from cartItem in context.ShoppingCartItems
                                      where cartItem.ShoppingCartID == shoppingCartId
                                      select new SaleDetailPoco
                                      {
                                          PartID = cartItem.PartID,
                                          Quantity = cartItem.Quantity,
                                          SellingPrice = cartItem.Part.SellingPrice,
                                          //ShippedDate = cartItem.Part.QuantityOnHand >= cartItem.Quantity?DateTime.Today:null,
                                          Backordered = cartItem.Part.QuantityOnHand >= cartItem.Quantity?false:true
                                      }).ToList();
                var subTotal = (from shoppingCart in context.ShoppingCarts
                                      where shoppingCart.ShoppingCartID == shoppingCartId
                                      select shoppingCart.ShoppingCartItems.Sum(x => (x.Part.QuantityOnHand>=x.Quantity)?x.Quantity * x.Part.SellingPrice:0)).Single();
                

                var newSale = context.Sales.Add(new Sale());
                newSale.SaleDate = DateTime.Today;
                newSale.UserName = name;
                newSale.EmployeeID = 1;
                newSale.SubTotal = subTotal;
                newSale.TaxAmount = subTotal * (decimal)0.05;
                newSale.PaymentToken = Guid.NewGuid();
                if(coupon != null)
                {
                    newSale.CouponID = coupon.CouponID;
                }
                newSale.PaymentType = paymentType;
                foreach(var saleDetail in saleDetailPoco)
                {
                    var newSaleDetail = new SaleDetail();
                    newSaleDetail.PartID = saleDetail.PartID;
                    newSaleDetail.Quantity = saleDetail.Quantity;
                    newSaleDetail.SellingPrice = saleDetail.SellingPrice;
                    newSaleDetail.Backordered = saleDetail.Backordered;
                    //if it is not out of stock, assign the day to today. Otherwise is null
                    if (!saleDetail.Backordered)
                        newSaleDetail.ShippedDate = DateTime.Today;
                    newSale.SaleDetails.Add(newSaleDetail);
                }
                
                //delete shoppingCart and shoppingCartItem
                foreach(var cartItemId in cartItemIds)
                {
                    var existingCartItem = context.ShoppingCartItems.Find(cartItemId);
                    //update part table's QuantityOnHand attrabute
                    var part = context.Parts.Find(existingCartItem.PartID);
                    if(part.QuantityOnHand >= existingCartItem.Quantity)
                        part.QuantityOnHand = part.QuantityOnHand - existingCartItem.Quantity;
                    //remove existing Cart Item record
                    context.ShoppingCartItems.Remove(existingCartItem);
                }
                //remove existing ShoppingCart
                var existingShoppingCart = context.ShoppingCarts.Find(shoppingCartId);
                context.ShoppingCarts.Remove(existingShoppingCart);

                context.SaveChanges();


            }
        }


        #endregion

        #region Coupon
        //public int ListAllCoupon()
        public CouponInfo GetCoupon(String couponIdValue)
        {
            using(var context = new eBikeContext())
            {
                var coupons = from coupon in context.Coupons
                              where coupon.CouponIDValue == couponIdValue 
                              //&& coupon.StartDate <= DateTime.Today && DateTime.Today <= coupon.EndDate
                              select new CouponInfo
                              {
                                  CouponID = coupon.CouponID,
                                  CouponDiscount = coupon.CouponDiscount
                              };
                return coupons.SingleOrDefault();
            }
        }
        #endregion

        #region CustomerId ShoppingCartId

        public List<int> GetCartItemIdsByShoppingCartId(int shoppingCartId)
        {
            using(var context = new eBikeContext())
            {
                var cartItemIds = (from shoppingCartItem in context.ShoppingCartItems
                                 where shoppingCartItem.ShoppingCartID == shoppingCartId
                                 select shoppingCartItem.ShoppingCartItemID).ToList();
                return cartItemIds;
            }
        }

        public int GetCustomerId(string userName)
        {
            using (var context = new eBikeContext())
            {
                var onlineCustomerIdAmount = (from onlineCustomer in context.OnlineCustomers
                                             where onlineCustomer.UserName == userName
                                             select onlineCustomer.OnlineCustomerID).Count();
                //don't have online customer
                if (onlineCustomerIdAmount == 0)
                {
                    return 0;
                }
                else
                {
                    var onlineCustomerIdResult = from onlineCustomer in context.OnlineCustomers
                                                 where onlineCustomer.UserName == userName
                                                 select onlineCustomer.OnlineCustomerID;
                    return onlineCustomerIdResult.Single();
                }
            }
        }

        public int GetShoppingCartId(string userName)
        {
            using (var context = new eBikeContext())
            {
                var onlineCustomerIdAmount = (from onlineCustomer in context.OnlineCustomers
                                             where onlineCustomer.UserName == userName
                                             select onlineCustomer.OnlineCustomerID).Count();
                //don't have online customer
                if (onlineCustomerIdAmount == 0)
                {
                    return -1;
                }
                else
                {
                    var onlineCustomerIdResult = from onlineCustomer in context.OnlineCustomers
                                                  where onlineCustomer.UserName == userName
                                                  select onlineCustomer.OnlineCustomerID;
                    int onlineCustomerId = onlineCustomerIdResult.Single();

                    var shoppingCartIdAmount = (from shoppingCart in context.ShoppingCarts
                                         where shoppingCart.OnlineCustomerID == onlineCustomerId
                                         select shoppingCart.ShoppingCartID).Count();
                    if(shoppingCartIdAmount == 0)
                    {
                        return 0;
                    }
                    else
                    {
                        var shoppingCartIdResult = from shoppingCart in context.ShoppingCarts
                                                   where shoppingCart.OnlineCustomerID == onlineCustomerId
                                                   select shoppingCart.ShoppingCartID;
                        return shoppingCartIdResult.Single();
                    }
                }
            }
        }
        #endregion

    }
}
