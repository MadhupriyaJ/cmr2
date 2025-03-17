using Core;
using FramWork.HelloService;
using FramWork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;

namespace FramWork.Controllers
{
    public class CampaignManagmentController : Controller
    {
        // GET: CampaignManagment
        public ActionResult Index()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult CreateCampaign()
        {
            return View();
        }


        #region ProductcampaignDocConfig
        [LoginAuthorize]
        public ActionResult ProductcampaignDocConfig()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion


        #region Add Productdocument
        [LoginAuthorize]
        [HttpPost]
        public bool addproductdocument(string CampaignProductId, List<string> DocumentId)
        {
            try
            {
                if(!String.IsNullOrEmpty(CampaignProductId)&& DocumentId.Count()!=0)
                {
                    foreach (var item in DocumentId)
                    {
                        using(FrameworkEntities dc = new FrameworkEntities())
                        {
                            var dup = dc.CampaignProductDocuments.Where(a => a.OrgId == Login.OrganizationId && a.MarkAsDelete == false &&
                                a.CampaignProductId == new Guid(CampaignProductId) && a.DocumentId == new Guid(item)).FirstOrDefault();
                            if (dup == null)
                            {
                                CampaignProductDocuments tbl = new CampaignProductDocuments();
                                tbl.Id = Guid.NewGuid();
                                tbl.OrgId = Login.OrganizationId;
                                tbl.CampaignProductId = new Guid(CampaignProductId);
                                tbl.DocumentId = new Guid(item);
                                tbl.Ismandatory = false;
                                tbl.CreateDate = tbl.ModifiedDate = DateTime.Now;
                                tbl.ModifiedBy = Login.Id;
                                tbl.MarkAsDelete = false;
                                dc.CampaignProductDocuments.Add(tbl);
                                dc.SaveChanges();
                            }
                        }
                    }
                   
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                
                throw;
            }
           
        }

        #endregion

        #region ListOfdocs
        public ActionResult ListOfdocs(string sortOrder, string currentFilter, string searchString, int? page, Guid? CampaignProductId)
        {
            try
            {
                FrameworkEntities dc = new FrameworkEntities();
                int pageSize = 100;
                int pageNumber = (page ?? 1);
                return View(dc.CampaignProductDocuments.AsEnumerable().Where(a => a.CampaignProductId == CampaignProductId && a.OrgId == Login.OrganizationId && a.MarkAsDelete == false).ToPagedList(pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion


        #region Get document list
        public ActionResult doclist(string someValue)
        {
            FrameworkEntities dc= new FrameworkEntities();
            var info = dc.Documents.Where(a => a.OrgId == Login.OrganizationId&& a.MarkAsDelete==false).Select(a => new { value = a.Id, text = a.Name });
            return Json(info, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region Campaign ProductList
        [LoginAuthorize]
        public ActionResult ProductList(string sortOrder, string currentFilter, string searchString, int? page, Guid? campaignId)
        {
            try
            {
                FrameworkEntities dc = new FrameworkEntities();
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return View(dc.CampaignProducts.AsEnumerable().Where(a => a.CampaignId == campaignId && a.OrgId == Login.OrganizationId && a.MarkAsDelete == false).ToPagedList(pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region List Of Campaign calander For Ajax
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult getcampaigncalandar()
        {
            try
            {

                return View();
            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        [LoginAuthorize]
        public ActionResult CampaignList()
        {
            try
            {

                HelloServiceClient client = new FramWork.HelloService.HelloServiceClient("BasicHttpBinding_IHelloService");
                ViewBag.campaignList = client.GetCampaignList(Login.OrganizationId);
                return View();
            }
            catch (Exception)
            {
                //LogManager.LogException(ex, "Contacts/index");
            }
            return View();
        }
        #region Get Media Source list
        public ActionResult getmediasource(string someValue)
        {
            HelloServiceClient client = new HelloServiceClient("BasicHttpBinding_IHelloService");
            var info = client.GetMediasourceList(Login.OrganizationId).Select(a => new { value = a.Id, text = a.Name });
            return Json(info, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region Add Campaign
        public String addcampaign(String CampaignBankTypeId, String CampaignTypeId,
            String CampaignName, String Budget, String CampaignOwner, String StartDate,
            String EndDate, String Description, List<String> ProductArray)
        {
            try
            {
                HelloServiceClient client = new FramWork.HelloService.HelloServiceClient("BasicHttpBinding_IHelloService");
                Campaign campaigntable = new Campaign();
                campaigntable.Id = Guid.NewGuid();
                campaigntable.OrgId = Login.OrganizationId;
                campaigntable.CampaignBankTypeId = new Guid(CampaignBankTypeId);
                campaigntable.CampaignTypeId = new Guid(CampaignTypeId);
                campaigntable.CampaignName = CampaignName;
                campaigntable.Budget = decimal.Parse(Regex.Replace(Budget, @"[^\d.]", ""));
                campaigntable.CampaignOwner = new Guid(CampaignOwner);
                campaigntable.StartDate = Convert.ToDateTime(StartDate);
                campaigntable.EndDate = Convert.ToDateTime(EndDate);
                campaigntable.Description = Description;
                campaigntable.ModifiedBy = Login.Id;
                //save call
                bool Success = client.AddCampaign(campaigntable);
                if (Success)
                {
                    if (ProductArray.Count != 0)
                    {
                        CampaignProduct ptable = new CampaignProduct();
                        foreach (var item in ProductArray)
                        {
                            ptable.OrgId = Login.OrganizationId;
                            ptable.CampaignId = campaigntable.Id;
                            ptable.ProductName = item;
                            ptable.ModifiedBy = Login.Id;
                            //save Call
                            client.AddCampaignProduct(ptable);
                        }
                    }
                }
                return campaigntable.Id.ToString();
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Add Campaign Media
        public String AddCampaignMedia(String MediaName, String CampaignId, String MediaSource, String MediaCost, String Medianarration)
        {
            try
            {
                if (!String.IsNullOrEmpty(MediaName) || !String.IsNullOrEmpty(CampaignId))
                {
                    HelloServiceClient client = new FramWork.HelloService.HelloServiceClient("BasicHttpBinding_IHelloService");
                    CampaignMedia Mtable = new CampaignMedia();
                    Mtable.OrgId = Login.OrganizationId;
                    Mtable.CampaignId = new Guid(CampaignId);
                    Mtable.MediaName = MediaName;
                    Mtable.MediaSourceId = new Guid(MediaSource);
                    Mtable.Cost = decimal.Parse(Regex.Replace(MediaCost, @"[^\d.]", ""));
                    Mtable.ModifiedBy = Login.Id;
                    client.AddCampaignMedia(Mtable);
                    return "True";
                }

                return "False";
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion


        #region Getcampaign Appoinments
        public ActionResult getcampaignappoinments()
        {
            try
            {
                FramWork.HelloService.HelloServiceClient client = new FramWork.HelloService.HelloServiceClient("BasicHttpBinding_IHelloService");
                var random = new Random();
                var color = String.Format("#{0:X6}", random.Next(0x1000000)); // = "#A197B9"
                var eventList = (from c in client.getcampaignappoinments(Login.OrganizationId).AsEnumerable()
                                 // where c.Id == CompanyID
                                 select new
                                 {
                                     id = c.id,
                                     title = c.title,
                                     start = Convert.ToDateTime(c.start).ToString("HH:mm"),
                                     end = Convert.ToDateTime(c.end).ToString("HH:mm"),
                                     color = String.Format("#{0:X6}", random.Next(0x1000000)),
                                     dow = "[0,1,2,3,4,5,6]",
                                     ranges = new { start = Convert.ToDateTime(c.start).ToString("yyyy-MM-dd"), end = Convert.ToDateTime(c.end).ToString("yyyy-MM-dd") },
                                     repeating = true,
                                     isallday = false

                                 }).ToList();
                return Json(eventList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion



        #region Email Campaign

        public ActionResult EmailCampaign()
        {
            return View();
        }

        #endregion

        #region SMS Campaign

        public ActionResult SmsCampaign()
        {
            return View();
        }

        #endregion





    }
}