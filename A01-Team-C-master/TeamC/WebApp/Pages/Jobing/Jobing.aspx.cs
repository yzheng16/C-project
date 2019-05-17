using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TeamCeBikeLab.BLL.JobingCRUD;
using TeamCeBikeLab.Entities.POCOs;
using WebApp.Admin.Security;

namespace WebApp.Pages.Jobing
{
    public partial class Jobing : System.Web.UI.Page
    {
        int? employeeId;
        protected void Page_Load(object sender, EventArgs e)
        {         
            if(!Request.IsAuthenticated || !User.IsInRole(Settings.ServicesRole))
                Response.Redirect("~", true);

            if (User.Identity.Name != "")
            {
                SecurityController control = new SecurityController();
                string userName = User.Identity.Name;
                employeeId = control.GetCurrentUserEmployeeId(userName);
                JobController controller = new JobController();

                EmployeeNameLabel.Text = userName.Substring(0, userName.IndexOf("."));

                JobGridView.DataSource = controller.ListCurrentJobs();
                JobGridView.DataBind();
                
            }
        }

        protected void JobGridView_RowCommand(Object sender, GridViewCommandEventArgs e)
        {
            JobController control = new JobController();
            GridViewRow row = ((LinkButton)e.CommandSource).NamingContainer as GridViewRow;
            currentJobListForm.Visible = false;
            NewJobButton.Visible = false;
            jobInfo.Visible = true;
            JobNumberLabel.Text = row.Cells[0].Text;
            CustomerLabel.Text = row.Cells[4].Text;
            ContactLabel.Text = row.Cells[5].Text;

            List<ServiceDetailPoco> serviceDetail = control.ListServiceDetails(int.Parse(JobNumberLabel.Text));
            if (e.CommandName == "CurrentJob")
            {
                
                pageTitle.InnerHtml = "Current Job";             
                currentJobForm.Visible = true;
                CurrentJobGridView.DataSource = serviceDetail;
                CurrentJobGridView.DataBind();
            }
            else if (e.CommandName == "CurrentJobService")
            {
                pageTitle.InnerHtml = "Current Job Service Details";
                currentJobService.Visible = true;
                ServicesGridView.DataSource = serviceDetail;
                ServicesGridView.DataBind();
            }
        }

        protected void NewJobButton_Click(object sender, EventArgs e)
        {
            NewJobButton.Visible = false;
            NewJobCustomer.Visible = true;
            currentJobListForm.Visible = false;
            NewJobForm.Visible = true;
            currentJobForm.Visible = true;
            AddServiceButton.Visible = false;
            AddJobButton.Visible = true;
        }

        protected void AddJobButton_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                JobController control = new JobController();
                JobPoco newJob = new JobPoco();
                ServiceDetailPoco newService = new ServiceDetailPoco();
                int customerId = int.Parse(CustomerDDL.SelectedValue); 
                if(employeeId == null)
                {
                    throw new Exception("Please sign in as valid employee");
                }
                               
                if(customerId == 0)
                {
                    throw new Exception("please select a valid Customer");
                }
             
                bool success = (decimal.TryParse(ShopRateTB.Text, out decimal shoprate));
                if(!success)
                {
                    throw new Exception("Must enter a valid shop rate value (default 80)");
                }      
                if(string.IsNullOrWhiteSpace(VehicleTB.Text))
                {
                    throw new Exception("Please enter a vehical identifaction");
                }

                success = decimal.TryParse(HoursTB.Text, out decimal hours);
                if (success && hours > 0)
                {
                    newService.JobHours = hours;
                }
                else
                {
                    throw new Exception("Must enter a valid decimal greater than 0 for hours.");
                }
                newJob.EmployeeID = employeeId;
                newJob.CustomerID = customerId;
                newJob.JobDateIn = DateTime.Today;
                newJob.StatusCode = "I";
                newJob.ShopRate = shoprate;
                newJob.VehicleIdentification = VehicleTB.Text;

                newService.Description = DescriptionTB.Text;
                newService.CouponID = int.Parse(CouponDDL.SelectedValue);
                newService.Comments = CommentsTB.Text;

                control.AddJob(newJob, newService);

                DescriptionTB.Text = "";
                HoursTB.Text = "";
                CouponDDL.SelectedIndex = 0;
                CommentsTB.Text = "";

                NewJobButton.Visible = true;
                NewJobCustomer.Visible = false;
                currentJobListForm.Visible = true;
                NewJobForm.Visible = false;
                currentJobForm.Visible = false;
                AddServiceButton.Visible = true;
                AddJobButton.Visible = false;


                JobGridView.DataSource = control.ListCurrentJobs();
                JobGridView.DataBind();
            });
        }

        protected void AddServiceButton_Click(Object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                JobController control = new JobController();


                ServiceDetailPoco newItem = new ServiceDetailPoco();

                newItem.Description = DescriptionTB.Text;
                newItem.JobID = int.Parse(JobNumberLabel.Text);

                bool success = decimal.TryParse(HoursTB.Text, out decimal hours);
                if(success)
                {
                    newItem.JobHours = hours;
                }
                else
                {
                    throw new Exception("Must enter a valid decimal for hours.");
                }
                
                newItem.CouponID = int.Parse(CouponDDL.SelectedValue);

                newItem.Comments = CommentsTB.Text;

                control.AddServiceDetail(newItem);

                List<ServiceDetailPoco> serviceDetail = control.ListServiceDetails(int.Parse(JobNumberLabel.Text));
                CurrentJobGridView.DataSource = serviceDetail;
                CurrentJobGridView.DataBind();

                DescriptionTB.Text = "";
                HoursTB.Text = "";
                CouponDDL.SelectedIndex = 0;
                CommentsTB.Text = "";

            }, "Service Added", "Service was successfully added to job");
        }

        protected void CurrentJobGridView_RowCommand(Object sender, GridViewCommandEventArgs e)
        {
            JobController control = new JobController();
            GridViewRow row = ((LinkButton)e.CommandSource).NamingContainer as GridViewRow;
            List<ServiceDetailPoco> serviceDetail = control.ListServiceDetails(int.Parse(JobNumberLabel.Text));
            int serviceDetailID = int.Parse(CurrentJobGridView.DataKeys[row.RowIndex].Value.ToString());

            if (serviceDetail.Count > 1)
            {
                MessageUserControl.TryRun(() =>
                {
                    
                    control.DeleteServiceDetail(serviceDetailID);


                }, "Service Deleted", "Service was successfully removed");

                serviceDetail = control.ListServiceDetails(int.Parse(JobNumberLabel.Text));
                CurrentJobGridView.DataSource = serviceDetail;
                CurrentJobGridView.DataBind();
            }
            else
            {
                MessageUserControl.TryRun(() =>
                {
                    int jobId = int.Parse(JobNumberLabel.Text);
                    control.DeleteJob(jobId, serviceDetailID);

                    currentJobListForm.Visible = true;
                    currentJobForm.Visible = false;
                    NewJobButton.Visible = true;
                    jobInfo.Visible = false;

                    JobGridView.DataSource = control.ListCurrentJobs();
                    JobGridView.DataBind();
                }, "Job Deleted", "All services removed job has been deleted");
            }


        }

        protected void ServicesGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow row = ((LinkButton)e.CommandSource).NamingContainer as GridViewRow;
            JobController control = new JobController();
            List<ServiceDetailPoco> serviceDetail = control.ListServiceDetails(int.Parse(JobNumberLabel.Text));
            int serviceDetailiD = int.Parse(ServicesGridView.DataKeys[row.RowIndex].Value.ToString());
            int jobId = int.Parse(JobNumberLabel.Text);
            if (e.CommandName == "start")
            {
                MessageUserControl.TryRun(() =>
                {
                    control.ChangeStatus(serviceDetailiD, jobId, "S");
                    serviceDetail = control.ListServiceDetails(int.Parse(JobNumberLabel.Text));
                    ServicesGridView.DataSource = serviceDetail;
                    ServicesGridView.DataBind();

                },"Status Update", row.Cells[0].Text + " status set to start");
            }
            else if(e.CommandName == "done")
            {
                MessageUserControl.TryRun(() =>
                {
                    control.ChangeStatus(serviceDetailiD, jobId, "C");
                    serviceDetail = control.ListServiceDetails(int.Parse(JobNumberLabel.Text));
                    ServicesGridView.DataSource = serviceDetail;
                    ServicesGridView.DataBind();

                }, "Status Update", row.Cells[0].Text + " status set to done");
            }
            else if(e.CommandName == "view")
            {
                foreach(var item in serviceDetail)
                {
                    if(item.ServiceDetailID == serviceDetailiD)
                    {
                        DescriptionLabel.Text = item.Description;
                        HoursLabel.Text = item.JobHours.ToString();
                        CommentsLabel.Text = item.Comments;
                        ServiceDetailIdLabel.Text = item.ServiceDetailID.ToString();
                    }
                }
                ViewServiceDetails.Visible = true;
                PartsObjectDataSource.SelectParameters["serviceDetailId"].DefaultValue = serviceDetailiD.ToString();

            }
            else if(e.CommandName == "remove")
            {
                if (serviceDetail.Count > 1)
                {
                    MessageUserControl.TryRun(() =>
                    {

                        control.DeleteServiceDetail(serviceDetailiD);


                    }, "Service Deleted", "Service was successfully removed");

                    serviceDetail = control.ListServiceDetails(int.Parse(JobNumberLabel.Text));
                    ServicesGridView.DataSource = serviceDetail;
                    ServicesGridView.DataBind();
                }
                else
                {
                    MessageUserControl.TryRun(() =>
                    {
                        control.DeleteJob(jobId, serviceDetailiD);

                        currentJobListForm.Visible = true;
                        currentJobForm.Visible = false;
                        NewJobButton.Visible = true;
                        currentJobService.Visible = false;
                        ViewServiceDetails.Visible = false;

                        JobGridView.DataSource = control.ListCurrentJobs();
                        JobGridView.DataBind();
                    }, "Job Deleted", "All services removed job has been deleted");
                }
            }
        }

        protected void AddCommentButton_Click(object sender, EventArgs e)
        {
            MessageUserControl.TryRun(() =>
            {
                JobController control = new JobController();
                int serviceId = int.Parse(ServiceDetailIdLabel.Text);
                string comment = CommentTB.Text;
                if (string.IsNullOrWhiteSpace(comment))
                {
                    throw new Exception("Must enter a comment first");
                }
                control.AddComment(serviceId, comment);
                CommentsLabel.Text += "; " + comment;
                CommentTB.Text = "";

            }, "Comment added", "Comment successfully added");
        }

        protected void PartsListView_ItemInserting(object sender, ListViewInsertEventArgs e)
        {
            e.Values["ServiceDetailID"] = ServiceDetailIdLabel.Text;
           
        }


        protected void CheckForExceptions(object sender, ObjectDataSourceStatusEventArgs e)
        {
            MessageUserControl.HandleDataBoundException(e);
        }


    }
}