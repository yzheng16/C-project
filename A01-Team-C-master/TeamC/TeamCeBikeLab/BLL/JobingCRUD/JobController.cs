using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamCeBikeLab.DAL;
using TeamCeBikeLab.Entities;
using TeamCeBikeLab.Entities.POCOs;

namespace TeamCeBikeLab.BLL.JobingCRUD
{
    [DataObject]
    public class JobController
    {
        // JobsController class
        public List<CustomerJobs> ListCurrentJobs()
        {
            using (var context = new eBikeContext())
            {
                var results = from data in context.Jobs
                              orderby data.Customer.LastName
                              select new CustomerJobs
                              {
                                  JobID = data.JobID,
                                  JobDateIn = data.JobDateIn,
                                  JobDateStarted = data.JobDateStarted,
                                  JobDateDone = data.JobDateDone,
                                  CustomerName = data.Customer.LastName + ", " + data.Customer.FirstName,
                                  CustomerPhone = data.Customer.ContactPhone
                              };
                return results.ToList();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<CustomerPoco> ListCustomers()
        {
            using (var context = new eBikeContext())
            {
                var results = from data in context.Customers
                              select new CustomerPoco
                              {
                                  CustomerID = data.CustomerID,
                                  FirstName = data.FirstName,
                                  LastName = data.LastName
                              };
                return results.ToList();
            }
        }

        public void AddJob(JobPoco job, ServiceDetailPoco service)
        {
            using (var context = new eBikeContext())
            {
                Job newJob = new Job();
                ServiceDetail newService = new ServiceDetail();
                var existing = context.Coupons.Find(service.CouponID);
                if (existing == null && service.CouponID != 0)
                {
                    throw new Exception("Coupon is not valid");
                }

                newService.JobHours = service.JobHours;
                newService.Description = service.Description;
                if (service.CouponID == 0 || existing.CouponID != service.CouponID)
                {
                    newService.CouponID = null;
                }
                else
                {
                    newService.CouponID = service.CouponID;
                }
                newService.Comments = service.Comments;

                newJob.EmployeeID = (int)job.EmployeeID;
                newJob.CustomerID = job.CustomerID;
                newJob.JobDateIn = job.JobDateIn;
                newJob.ShopRate = job.ShopRate;
                newJob.StatusCode = job.StatusCode;
                newJob.VehicleIdentification = job.VehicleIdentification;
                context.Jobs.Add(newJob).ServiceDetails.Add(newService);
                context.SaveChanges();
            }
        }

        // JobsController class
        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<CouponPoco> ListCoupons()
        {
            using (var context = new eBikeContext())
            {
                var results = from data in context.Coupons
                              select new CouponPoco
                              {
                                  CouponID = data.CouponID,
                                  CouponIDValue = data.CouponIDValue
                              };
                return results.ToList();
            }
        }

        // JobsController class
        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<ServiceDetailPoco> ListServiceDetails(int jobId)
        {
            using (var context = new eBikeContext())
            {
                var results = from data in context.ServiceDetails
                              where data.JobID == jobId
                              select new ServiceDetailPoco
                              {
                                  ServiceDetailID = data.ServiceDetailID,
                                  Description = data.Description,
                                  JobHours = data.JobHours,
                                  CouponID = data.Coupon.CouponID,
                                  CouponIDValue = data.Coupon.CouponIDValue,
                                  Comments = data.Comments,
                                  Status = data.Status
                              };
                return results.ToList();
            }
        }

        // JobsController class
        public void AddServiceDetail(ServiceDetailPoco item)
        {
            using (var context = new eBikeContext())
            {
                ServiceDetail newItem = new ServiceDetail();
                var existing = context.Coupons.Find(item.CouponID);
                if(existing == null && item.CouponID != 0)
                {
                    throw new Exception("Coupon is not valid");
                }

                if(item.JobHours <=0 )
                {
                    throw new Exception("Job Hours must be greater than 0");
                }

                newItem.JobID = item.JobID;
                newItem.JobHours = item.JobHours;
                newItem.Description = item.Description;
                newItem.Comments = item.Comments;
                if(item.CouponID == 0 || existing.CouponID != item.CouponID)
                {
                    newItem.CouponID = null;
                }
                else
                {
                    newItem.CouponID = item.CouponID;
                }

                context.ServiceDetails.Add(newItem);
                context.SaveChanges();
            }
        }

        // // JobsController class
        public void DeleteServiceDetail(int serviceDetailId)
        {
            using (var context = new eBikeContext())
            {
                var existing = context.ServiceDetails.Find(serviceDetailId);
                var results = from data in context.ServiceDetailParts
                                   where serviceDetailId == data.ServiceDetailID
                                   select new ServiceDetailPartsPoco
                                   {
                                       ServiceDetailPartID = data.ServiceDetailPartID,
                                       PartID = data.PartID,
                                       Description = data.Part.Description,
                                       Quantity = data.Quantity
                                   };
                List<ServiceDetailPartsPoco> existingParts = results.ToList();
                if (existing.Status != "S")
                {
                    foreach (var item in existingParts)
                    {
                        var existingPart = context.ServiceDetailParts.Find(item.ServiceDetailPartID);
                        context.ServiceDetailParts.Remove(existingPart);
                    }
                    context.ServiceDetails.Remove(existing);
                    context.SaveChanges();
                }
                else if(existing.Status == "C")
                {
                    throw new Exception("Can not remove a service that is done");
                }
                else
                {
                    throw new Exception("Can not remove a service that has already started.");
                }
                


            }
        }

        public void DeleteJob(int jobId, int serviceDetailId)
        {
            using (var context = new eBikeContext())
            {
                var existingJob = context.Jobs.Find(jobId);
                DeleteServiceDetail(serviceDetailId);
                context.Jobs.Remove(existingJob);
                context.SaveChanges();
            }
        }

        public void ChangeStatus (int serviceDetailId, int jobId, string status)
        {
            using (var context = new eBikeContext())
            {
                var statusUpdate = context.ServiceDetails.Find(serviceDetailId);
                List<ServiceDetailPoco> serviceList = ListServiceDetails(jobId);
                var job = context.Jobs.Find(jobId);
                int count = 0;
                if (status == "C" && statusUpdate.Status != "C")
                {
                    count++;
                }
                
                if (status == "S" && job.JobDateStarted == null)
                {
                    job.JobDateStarted = DateTime.Today;
                }

                if(status == "S" && statusUpdate.Status == null)
                {
                    UpdateInventory(serviceDetailId);
                }

                if(job.StatusCode == "I" && status == "S")
                {
                    job.StatusCode = "S";
                }

                foreach(var item in serviceList)
                {
                    if(item.Status == "C")
                    {
                        count++;
                    }
                }

                if(count == serviceList.Count)
                {
                    job.JobDateDone = DateTime.Today;
                    job.StatusCode = "D";
                }

                statusUpdate.Status = status;
                context.SaveChanges();

            }
        }

        public void AddComment(int serviceDetailId, string comment)
        {
            using (var context = new eBikeContext())
            {
                var service = context.ServiceDetails.Find(serviceDetailId);
                service.Comments += ", " + comment;
                context.SaveChanges();
            }
        }

        //// JobsController class
        [DataObjectMethod(DataObjectMethodType.Select)]
        public List<ServiceDetailPartsPoco> ListServiceDetailPart(int serviceDetailId)
        {
            using (var context = new eBikeContext())
            {
                var results = from data in context.ServiceDetailParts
                              where serviceDetailId == data.ServiceDetailID
                              select new ServiceDetailPartsPoco
                              {
                                  ServiceDetailPartID = data.ServiceDetailPartID,
                                  ServiceDetailID = data.ServiceDetailID,
                                  PartID = data.PartID,
                                  Description = data.Part.Description,
                                  Quantity = data.Quantity
                              };
                return results.ToList();
            }
        }

        //// JobsController class
        [DataObjectMethod(DataObjectMethodType.Insert)]
        public void AddPart(ServiceDetailPartsPoco item)
        {
            using (var context = new eBikeContext())
            {
                ServiceDetailPart newPart = new ServiceDetailPart();
                var info = context.Parts.Find(item.PartID);
                var serviceDetail = context.ServiceDetails.Find(item.ServiceDetailID);
                if(info == null)
                {
                    throw new Exception("No part matches that ID");
                }
                newPart.PartID = item.PartID;
                newPart.ServiceDetailID = item.ServiceDetailID;
                if(item.Quantity <= info.QuantityOnHand && serviceDetail.Status == "S")
                {                 
                    info.QuantityOnHand -= item.Quantity;
                    newPart.Quantity = item.Quantity;
                }
                else if(item.Quantity <= info.QuantityOnHand)
                {
                    newPart.Quantity = item.Quantity;
                }
                else
                {
                    throw new Exception("Not enough parts available");
                }
                newPart.SellingPrice = info.SellingPrice;
                
                context.ServiceDetailParts.Add(newPart);
                context.SaveChanges();
                
            }
        }

        public void UpdateInventory(int serivceDetailId)
        {
            using (var context = new eBikeContext())
            {
                var servicePart = from data in context.ServiceDetailParts
                                  where data.ServiceDetailID == serivceDetailId
                                  select new
                                  {
                                      PartID = data.PartID,
                                      Quantity = data.Quantity
                                  };

                foreach(var item in servicePart)
                {
                    var part = context.Parts.Find(item.PartID);
                    if(item.Quantity <= part.QuantityOnHand)
                    {
                        part.QuantityOnHand -= item.Quantity;
                    }
                    else
                    {
                        throw new Exception("Not enough " + part.Description + " available in inventory");
                    }
                   
                }
                context.SaveChanges();
                
            }
        }

        //// JobsController class
        [DataObjectMethod(DataObjectMethodType.Update)]
        public void UpdatePart(ServiceDetailPartsPoco item)
        {
            using (var context = new eBikeContext())
            {
                var ServicePart = context.ServiceDetailParts.Find(item.ServiceDetailPartID);
                var info = context.Parts.Find(item.PartID);
                var serviceDetail = context.ServiceDetails.Find(item.ServiceDetailID);
                if(serviceDetail.Status == "S")
                {
                    info.QuantityOnHand += ServicePart.Quantity;
                    if (item.Quantity <= info.QuantityOnHand)
                    {
                        ServicePart.Quantity = item.Quantity;
                        info.QuantityOnHand -= item.Quantity;
                    }
                    else
                    {
                        throw new Exception("Not enough parts available");
                    }
                }             
                else if(item.Quantity <= info.QuantityOnHand)
                {
                    ServicePart.Quantity = item.Quantity;
                    info.QuantityOnHand -= item.Quantity;
                }
                else
                {
                    throw new Exception("Not enough parts available");
                }
                context.SaveChanges();
                
            }
        }

        //// JobsController class
        [DataObjectMethod(DataObjectMethodType.Delete)]
        public void DeletePart(ServiceDetailPartsPoco item)
        {
            DeletePart(item.PartID, item.ServiceDetailPartID);
        }

        public void DeletePart(int partId, int serviceDetailPartId)
        {
            using (var context = new eBikeContext())
            {
                var existing = context.ServiceDetailParts.Find(serviceDetailPartId);
                var partQoh = context.Parts.Find(partId);
                partQoh.QuantityOnHand += existing.Quantity;
                context.ServiceDetailParts.Remove(existing);
                context.SaveChanges();
            }
        }
    }
}
