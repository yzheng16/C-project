using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamCeBikeLab.DAL;
using TeamCeBikeLab.Entities;
using TeamCeBikeLab.Entities.POCOs;
using System.Data.Entity;

namespace TeamCeBikeLab.BLL.Purchasing
{
    [DataObject]
    public class PurchasingController
    {
        #region Inventory
        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<VendorStockItems> GetVendorStockItemsNotOnOrder (int vendorID, PurchaseOrderInfo data)
        {
            using (var context = new eBikeContext())
            {
                var results = from inventory in context.Parts.ToList()
                              where inventory.VendorID == vendorID
                                    && !data.PurchaseOrderDetails.Any(x => x.ID == inventory.PartID)
                              select new VendorStockItems
                              {
                                  ID = inventory.PartID,
                                  Description = inventory.Description,
                                  QOH = inventory.QuantityOnHand,
                                  QOO = inventory.QuantityOnOrder,
                                  Buffer = (inventory.ReorderLevel - (inventory.QuantityOnHand + inventory.QuantityOnOrder)),
                                  Price = inventory.PurchasePrice
                              };
                return results.ToList();
            }
        }
        #endregion
        #region PurchaseOrder
        public PurchaseOrderInfo GetActivePurchaseOrder (int vendorID)
        {
            using (var context = new eBikeContext())
            {
                var results = from purchaseOrder in context.PurchaseOrders
                              where purchaseOrder.VendorID.Equals(vendorID)
                                    && purchaseOrder.PurchaseOrderNumber == null
                                    && purchaseOrder.OrderDate == null
                              select new PurchaseOrderInfo
                              {
                                  PurchaseOrderID = purchaseOrder.PurchaseOrderID,
                                  TaxAmount = purchaseOrder.TaxAmount,
                                  SubTotal = purchaseOrder.SubTotal,
                                  VendorID = purchaseOrder.VendorID,
                                  PurchaseOrderDetails = from detail in purchaseOrder.PurchaseOrderDetails
                                                         select new PurchaseOrderDetails
                                                         {
                                                             PurchaseOrderDetailID = detail.PurchaseOrderDetailID,
                                                             ID = detail.PartID,
                                                             Description = detail.Part.Description,
                                                             QOH = detail.Part.QuantityOnHand,
                                                             ROL = detail.Part.ReorderLevel,
                                                             QOO = detail.Part.QuantityOnOrder,
                                                             Quantity = detail.Quantity,
                                                             PurchasePrice = detail.PurchasePrice
                                                         }
                              };
                return results.SingleOrDefault();
            }
        }

        public void CreateSuggestedPurchaseOrder (int vendorID, int employeeID)
        {
            using (var context = new eBikeContext())
            {
                //creating the newPurchaseOrder that will be added to PurchaseOrders
                PurchaseOrder newPurchaseOrder = context.PurchaseOrders.Add(new PurchaseOrder());

                //setting up information in the new purchase order
                newPurchaseOrder.Closed = false;
                newPurchaseOrder.EmployeeID = employeeID;
                newPurchaseOrder.VendorID = vendorID;

                //grabbing the inventory items for the vendor selected and selecting the parts that are good to order
                var inventoryItems = from inventory in context.Parts
                                     where inventory.VendorID == vendorID && (inventory.QuantityOnHand + inventory.QuantityOnOrder) - inventory.ReorderLevel < 0
                                     select inventory;
                foreach (var item in inventoryItems.ToList())
                {
                    //create a new PODetail to add to the new PO
                    var newDetail = new PurchaseOrderDetail
                    {
                        //Take information from the inventoryItems and add it to the new PODetail
                        PartID = item.PartID,
                        Quantity = item.ReorderLevel - (item.QuantityOnHand + item.QuantityOnOrder),
                        PurchasePrice = item.PurchasePrice
                    };

                    //add the newDetail to the new purchase order
                    newPurchaseOrder.PurchaseOrderDetails.Add(newDetail);
                }

                //Get the subtotal and tax amount
                newPurchaseOrder.SubTotal = newPurchaseOrder.PurchaseOrderDetails.Sum(x => x.PurchasePrice * x.Quantity);
                newPurchaseOrder.TaxAmount = (0.05m * newPurchaseOrder.SubTotal);

                //commit the transaction
                context.SaveChanges();
            }
        }
        #endregion
        #region PurchaseOrderDetails
        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<PurchaseOrderDetails> GetCurrentActivePurchaseOrderDetails(int vendorID)
        {
            using (var context = new eBikeContext())
            {
                var results = from purchaseOrderDetails in context.PurchaseOrderDetails
                              where purchaseOrderDetails.PurchaseOrder.VendorID.Equals(vendorID)
                                    && purchaseOrderDetails.PurchaseOrder.PurchaseOrderNumber == null
                                    && purchaseOrderDetails.PurchaseOrder.OrderDate == null
                              select new PurchaseOrderDetails
                              {
                                  PurchaseOrderID = purchaseOrderDetails.PurchaseOrderID,
                                  ID = purchaseOrderDetails.PartID,
                                  Description = purchaseOrderDetails.Part.Description,
                                  QOH = purchaseOrderDetails.Part.QuantityOnHand,
                                  ROL = purchaseOrderDetails.Part.ReorderLevel,
                                  QOO = purchaseOrderDetails.Part.QuantityOnOrder,
                                  Quantity = purchaseOrderDetails.Quantity,
                                  PurchasePrice = purchaseOrderDetails.PurchasePrice
                              };
                return results.ToList();

            }
        }
        #endregion
        #region Vendor
        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<VendorInformation> SelectVendor()
        {
            using (var context = new eBikeContext())
            {
                var results = from vendor in context.Vendors
                              select new VendorInformation
                              {
                                  VendorID = vendor.VendorID,
                                  VendorName = vendor.VendorName,
                                  Phone = vendor.Phone,
                                  City = vendor.City
                              };
                return results.ToList();
            }
        }

        public VendorInformation Vendor_Get(int vendorID)
        {
            if (vendorID == 0)
                throw new Exception("No vendor selected.");

            using (var context = new eBikeContext())
            {
                var result = from vendor in context.Vendors
                             where vendor.VendorID.Equals(vendorID)
                             select new VendorInformation
                             {
                                 VendorName = vendor.VendorName,
                                 City = vendor.City,
                                 Phone = vendor.Phone
                             };
                return result.Single();
            }
        }
        #endregion
        #region Command Processing Methods
        public void PlaceOrder (int vendorID, int employeeID, PurchaseOrderInfo purchaseOrder)
        {
         
            if (purchaseOrder == null)
                throw new ArgumentNullException("Error", "Can't place order, order information was not available");

            using (var context = new eBikeContext())
            {
                //var orderToPlace = context.PurchaseOrders.Find(purchaseOrder.PurchaseOrderID);
                var orderToPlace = context.PurchaseOrders.Include(x => x.PurchaseOrderDetails).Single(x => x.PurchaseOrderID == purchaseOrder.PurchaseOrderID);
                if (orderToPlace == null)
                {
                    orderToPlace = context.PurchaseOrders.Add(new PurchaseOrder());
                }
                else
                {
                    context.Entry(orderToPlace).State = System.Data.Entity.EntityState.Modified;
                }

                orderToPlace.EmployeeID = employeeID;
                orderToPlace.OrderDate = DateTime.Today;
                orderToPlace.PurchaseOrderNumber = context.PurchaseOrders.Max(x => x.PurchaseOrderNumber) + 1;
                orderToPlace.SubTotal = purchaseOrder.SubTotal;
                orderToPlace.TaxAmount = purchaseOrder.TaxAmount;
                
                
                foreach (var detail in orderToPlace.PurchaseOrderDetails)
                {
                    var changes = purchaseOrder.PurchaseOrderDetails.SingleOrDefault(x => x.ID == detail.PartID);
                    if (changes == null)
                    {
                        context.Entry(detail).State = EntityState.Deleted;
                    }
                    else
                    {
                        detail.Quantity = changes.Quantity;
                        detail.PurchasePrice = changes.PurchasePrice;
                        context.Entry(detail).State = EntityState.Modified;
                    }
                } //closes foreach
                
                foreach (var item in purchaseOrder.PurchaseOrderDetails)
                {
                    if(!orderToPlace.PurchaseOrderDetails.Any(x => x.PartID == item.ID))
                    {
                        var newItem = new PurchaseOrderDetail
                        {
                            PartID = item.ID,
                            Quantity = item.Quantity,
                            PurchasePrice = item.PurchasePrice
                        };
                        orderToPlace.PurchaseOrderDetails.Add(newItem);
                    } //closes if
                } //closes foreach

                context.SaveChanges();

            } //closes using
        } //closes PlaceOrder

        public void UpdateOrder(PurchaseOrderInfo info, int employeeID)
        {
            using (var context = new eBikeContext())
            {
                //var currentPurchaseOrder = context.PurchaseOrders.Single(info.PurchaseOrderID);
                var currentPurchaseOrder = context.PurchaseOrders.Include(x => x.PurchaseOrderDetails).Single(x => x.PurchaseOrderID == info.PurchaseOrderID);


                //Add, Update or Delete Order Details
                foreach (var detail in currentPurchaseOrder.PurchaseOrderDetails.ToList())
                {
                    var changes = info.PurchaseOrderDetails.SingleOrDefault(x => x.ID == detail.PartID);
                    if(changes == null)
                    {
                        context.Entry(detail).State = System.Data.Entity.EntityState.Deleted;
                    }
                    else
                    {
                        detail.Quantity = changes.Quantity;
                        detail.PurchasePrice = changes.PurchasePrice;
                        context.Entry(detail).State = System.Data.Entity.EntityState.Modified;
                    }
                }

                //Loop through the new items to add to the database
                foreach (var item in info.PurchaseOrderDetails)
                {
                    
                    if(!currentPurchaseOrder.PurchaseOrderDetails.Any(x => x.PartID == item.ID))
                    {
                        //Add as a new item
                        var newItem = new PurchaseOrderDetail
                        {
                            PurchaseOrderID = item.PurchaseOrderID,
                            PartID = item.ID,
                            Quantity = item.Quantity,
                            PurchasePrice = item.PurchasePrice
                        };
                        currentPurchaseOrder.PurchaseOrderDetails.Add(newItem);
                    }
                }

                //Do the money and employee stuff here
                currentPurchaseOrder.SubTotal = currentPurchaseOrder.PurchaseOrderDetails.Sum(x => x.PurchasePrice * x.Quantity);
                currentPurchaseOrder.TaxAmount = currentPurchaseOrder.SubTotal * 0.05m;
                currentPurchaseOrder.EmployeeID = employeeID;

                //Save the changes - one save, one transaction
                context.Entry(currentPurchaseOrder).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
            } //closes using
        } //closes update

        public void DeleteOrder (int vendorID, int purchaseOrderID, List<PurchaseOrderDetails> purchaseOrderDetails)
        {

        }
        #endregion
    }
}
