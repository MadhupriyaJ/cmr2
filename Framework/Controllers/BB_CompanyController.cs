using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FramWork.Models;
using Core;
using PagedList.Mvc;
using PagedList;
using System.Text.RegularExpressions;
using System.IO;
using ExceptionLogging;

namespace FramWork.Controllers
{
    public class BB_CompanyController : Controller
    {

        #region Auto Gentrated
        private FrameworkEntities db = new FrameworkEntities();

        // GET: BB_Company
        [LoginAuthorize]
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            try
            {
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
              this.ControllerContext.RouteData.Values["action"].ToString(),
              "The user " + Login.FirstName + " " + Login.LastName + "Entered into Company Page");
                FrameworkEntities dc = new FrameworkEntities();
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return View(db.ABB_Company.AsEnumerable().Where(a => a.OrgId == Login.OrganizationId && a.MarkAsDelete == false).ToPagedList(pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                #region Exception Logging
                var filename = this.GetType().Name + "_" + System.Reflection.MethodBase.GetCurrentMethod().Name + "_" + (DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Millisecond).ToString() + ".txt";
                var dir = Server.MapPath("~/Exceptions/" + (DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year).ToString());  // folder location
                if (!Directory.Exists(dir))  // if it doesn't exist, create
                    Directory.CreateDirectory(dir);
                System.IO.File.WriteAllText(Path.Combine(dir, filename), LogHelper.CreateExceptionString(ex));
                #endregion
            }
            int pageNumberone = (page ?? 1);
            List<ABB_Company> lst = new List<ABB_Company>();
            return View(lst.ToPagedList(pageNumberone, 10));
        }


        // GET: BB_Company/Details/5
        [LoginAuthorize]
        public ActionResult Details(long? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ABB_Company aBB_Company = db.ABB_Company.Find(id);
                if (aBB_Company == null)
                {
                    return HttpNotFound();
                }
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
             this.ControllerContext.RouteData.Values["action"].ToString(),
             "The user " + Login.FirstName + " " + Login.LastName + "Entered into Company Details Page");
                return View(aBB_Company);
            }
            catch (Exception ex)
            {
                #region Exception Logging
                var filename = this.GetType().Name + "_" + System.Reflection.MethodBase.GetCurrentMethod().Name + "_" + (DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Millisecond).ToString() + ".txt";
                var dir = Server.MapPath("~/Exceptions/" + (DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year).ToString());  // folder location
                if (!Directory.Exists(dir))  // if it doesn't exist, create
                    Directory.CreateDirectory(dir);
                System.IO.File.WriteAllText(Path.Combine(dir, filename), LogHelper.CreateExceptionString(ex));
                #endregion
            }
            ABB_Company aBB_Company1 = new ABB_Company();
            return View(aBB_Company1);

        }

        // GET: BB_Company/Create
        [LoginAuthorize]
        public ActionResult Create()
        {
            try
            {
                ViewBag.BusinessSectorId = new SelectList(db.ABB_BusinessSector, "Id", "Sector");
                ViewBag.DealStageId = new SelectList(db.ABB_DealStages, "Id", "Name");
                ViewBag.BusinessTypeId = new SelectList(db.ABB_TypeOfBusiness, "Id", "TypeOfBusiness");
                ViewBag.OrgId = new SelectList(db.Organizations, "Id", "Alias");
                ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Salutation");
                return View();
            }
            catch (Exception ex)
            {
                #region Exception Logging
                var filename = this.GetType().Name + "_" + System.Reflection.MethodBase.GetCurrentMethod().Name + "_" + (DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Millisecond).ToString() + ".txt";
                var dir = Server.MapPath("~/Exceptions/" + (DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year).ToString());  // folder location
                if (!Directory.Exists(dir))  // if it doesn't exist, create
                    Directory.CreateDirectory(dir);
                System.IO.File.WriteAllText(Path.Combine(dir, filename), LogHelper.CreateExceptionString(ex));
                #endregion
            }
            return RedirectToAction("Index");
        }

        // POST: BB_Company/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [LoginAuthorize]
        public ActionResult Create([Bind(Include = "Id,OrgId,CRNo,TradeLicense,OpeningDate,EnglishName,ArabicName,Lattitude,Longitude,OfficeAddress,BusinessTypeId,BusinessSectorId,TradeLicenseExpiry,DealStageId,ImageName,CreateDate,ModifiedDate,ModifiedBy,MarkAsDelete")] ABB_Company tbl
            , HttpPostedFileBase imageLoader)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    tbl.OrgId = Login.OrganizationId;
                    tbl.CreateDate = tbl.ModifiedDate = DateTime.Now;
                    tbl.ModifiedBy = Login.Id;
                    tbl.MarkAsDelete = false;
                    tbl.PortletclassId = 1;
                    db.ABB_Company.Add(tbl);
                    db.SaveChanges();
                    if (imageLoader != null && imageLoader.ContentLength > 0)
                    {
                        // extract only the filename
                        var fileName = Path.GetFileName(imageLoader.FileName);
                        // store the file inside ~/App_Data/uploads folder
                        string subPath1 = "~/assets/Programmer/CompanyLogos/" + tbl.Id; //create folder for emirate
                        bool exists1 = System.IO.Directory.Exists(Server.MapPath(subPath1));
                        if (!exists1)
                            System.IO.Directory.CreateDirectory(Server.MapPath(subPath1));


                        var path = Path.Combine(Server.MapPath("~/assets/Programmer/CompanyLogos/" + tbl.Id), tbl.Id + ".png");
                        imageLoader.SaveAs(path);
                    }
                    //if (tbl.ImageName != null)
                    //{

                    //    string subPath1 = "~/assets/Programmer/CompanyLogos/" + tbl.Id; //create folder for emirate
                    //    bool exists1 = System.IO.Directory.Exists(Server.MapPath(subPath1));
                    //    if (!exists1)
                    //        System.IO.Directory.CreateDirectory(Server.MapPath(subPath1));
                    //    byte[] bytes = Convert.FromBase64String(tbl.ImageName);
                    //    using (var imageFile = new FileStream(Server.MapPath("~/assets/Programmer/CompanyLogos/" + tbl.Id + "/emratefront.png"), FileMode.Create))
                    //    {
                    //        imageFile.Write(bytes, 0, bytes.Length);
                    //        imageFile.Flush();
                    //    }
                    //}
                    Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
             this.ControllerContext.RouteData.Values["action"].ToString(),
             "The user " + Login.FirstName + " " + Login.LastName + "Added new Company Page");
                    return RedirectToAction("Index");
                }

                ViewBag.BusinessSectorId = new SelectList(db.ABB_BusinessSector, "Id", "Sector", tbl.BusinessSectorId);
                ViewBag.BusinessTypeId = new SelectList(db.ABB_TypeOfBusiness, "Id", "TypeOfBusiness", tbl.BusinessTypeId);
                ViewBag.OrgId = new SelectList(db.Organizations, "Id", "Alias", tbl.OrgId);
                ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Salutation", tbl.ModifiedBy);
                return View(tbl);
            }
            catch (Exception ex)
            {
                #region Exception Logging
                var filename = this.GetType().Name + "_" + System.Reflection.MethodBase.GetCurrentMethod().Name + "_" + (DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Millisecond).ToString() + ".txt";
                var dir = Server.MapPath("~/Exceptions/" + (DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year).ToString());  // folder location
                if (!Directory.Exists(dir))  // if it doesn't exist, create
                    Directory.CreateDirectory(dir);
                System.IO.File.WriteAllText(Path.Combine(dir, filename), LogHelper.CreateExceptionString(ex));
                #endregion
            }
            ViewBag.BusinessSectorId = new SelectList(db.ABB_BusinessSector, "Id", "Sector", tbl.BusinessSectorId);
            ViewBag.BusinessTypeId = new SelectList(db.ABB_TypeOfBusiness, "Id", "TypeOfBusiness", tbl.BusinessTypeId);
            ViewBag.OrgId = new SelectList(db.Organizations, "Id", "Alias", tbl.OrgId);
            ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Salutation", tbl.ModifiedBy);
            return View(tbl);

        }

        // GET: BB_Company/Edit/5
        [LoginAuthorize]
        public async Task<ActionResult> Edit(long? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ABB_Company aBB_Company = await db.ABB_Company.FindAsync(id);
                if (aBB_Company == null)
                {
                    return HttpNotFound();
                }
                ViewBag.BusinessSectorId = new SelectList(db.ABB_BusinessSector, "Id", "Sector", aBB_Company.BusinessSectorId);
                ViewBag.BusinessTypeId = new SelectList(db.ABB_TypeOfBusiness, "Id", "TypeOfBusiness", aBB_Company.BusinessTypeId);
                ViewBag.OrgId = new SelectList(db.Organizations, "Id", "Alias", aBB_Company.OrgId);
                ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Salutation", aBB_Company.ModifiedBy);
                return View(aBB_Company);
            }
            catch (Exception ex)
            {
                #region Exception Logging
                var filename = this.GetType().Name + "_" + System.Reflection.MethodBase.GetCurrentMethod().Name + "_" + (DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Millisecond).ToString() + ".txt";
                var dir = Server.MapPath("~/Exceptions/" + (DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year).ToString());  // folder location
                if (!Directory.Exists(dir))  // if it doesn't exist, create
                    Directory.CreateDirectory(dir);
                System.IO.File.WriteAllText(Path.Combine(dir, filename), LogHelper.CreateExceptionString(ex));
                #endregion
            }

            ABB_Company aBB_Company1 = new ABB_Company();
            return View(aBB_Company1);

        }

        // POST: BB_Company/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [LoginAuthorize]
        public async Task<ActionResult> Edit([Bind(Include = "Id,OrgId,CRNo,TradeLicense,OpeningDate,EnglishName,ArabicName,Lattitude,Longitude,OfficeAddress,BusinessTypeId,BusinessSectorId,TradeLicenseExpiry,DealStageId,ImageName,CreateDate,ModifiedDate,ModifiedBy,MarkAsDelete")] ABB_Company aBB_Company)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(aBB_Company).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
             this.ControllerContext.RouteData.Values["action"].ToString(),
             "The user " + Login.FirstName + " " + Login.LastName + "Edited Company Page");
                    return RedirectToAction("Index");
                }
                ViewBag.BusinessSectorId = new SelectList(db.ABB_BusinessSector, "Id", "Sector", aBB_Company.BusinessSectorId);
                ViewBag.BusinessTypeId = new SelectList(db.ABB_TypeOfBusiness, "Id", "TypeOfBusiness", aBB_Company.BusinessTypeId);
                ViewBag.OrgId = new SelectList(db.Organizations, "Id", "Alias", aBB_Company.OrgId);
                ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Salutation", aBB_Company.ModifiedBy);
                return View(aBB_Company);
            }
            catch (Exception ex)
            {
                #region Exception Logging
                var filename = this.GetType().Name + "_" + System.Reflection.MethodBase.GetCurrentMethod().Name + "_" + (DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Millisecond).ToString() + ".txt";
                var dir = Server.MapPath("~/Exceptions/" + (DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year).ToString());  // folder location
                if (!Directory.Exists(dir))  // if it doesn't exist, create
                    Directory.CreateDirectory(dir);
                System.IO.File.WriteAllText(Path.Combine(dir, filename), LogHelper.CreateExceptionString(ex));
                #endregion
            }
            ViewBag.BusinessSectorId = new SelectList(db.ABB_BusinessSector, "Id", "Sector", aBB_Company.BusinessSectorId);
            ViewBag.BusinessTypeId = new SelectList(db.ABB_TypeOfBusiness, "Id", "TypeOfBusiness", aBB_Company.BusinessTypeId);
            ViewBag.OrgId = new SelectList(db.Organizations, "Id", "Alias", aBB_Company.OrgId);
            ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Salutation", aBB_Company.ModifiedBy);
            return View(aBB_Company);

        }

        // GET: BB_Company/Delete/5
        [LoginAuthorize]
        public async Task<ActionResult> Delete(long? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ABB_Company aBB_Company = await db.ABB_Company.FindAsync(id);
                if (aBB_Company == null)
                {
                    return HttpNotFound();
                }
                return View(aBB_Company);
            }
            catch (Exception ex)
            {
                #region Exception Logging
                var filename = this.GetType().Name + "_" + System.Reflection.MethodBase.GetCurrentMethod().Name + "_" + (DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Millisecond).ToString() + ".txt";
                var dir = Server.MapPath("~/Exceptions/" + (DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year).ToString());  // folder location
                if (!Directory.Exists(dir))  // if it doesn't exist, create
                    Directory.CreateDirectory(dir);
                System.IO.File.WriteAllText(Path.Combine(dir, filename), LogHelper.CreateExceptionString(ex));
                #endregion
            }
            ABB_Company aBB_Company1 = new ABB_Company();
            return View(aBB_Company1);

        }

        // POST: BB_Company/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [LoginAuthorize]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            try
            {
                ABB_Company aBB_Company = await db.ABB_Company.FindAsync(id);
                db.ABB_Company.Remove(aBB_Company);
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
             this.ControllerContext.RouteData.Values["action"].ToString(),
             "The user " + Login.FirstName + " " + Login.LastName + "Deleted  Company Page");
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                #region Exception Logging
                var filename = this.GetType().Name + "_" + System.Reflection.MethodBase.GetCurrentMethod().Name + "_" + (DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Millisecond).ToString() + ".txt";
                var dir = Server.MapPath("~/Exceptions/" + (DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year).ToString());  // folder location
                if (!Directory.Exists(dir))  // if it doesn't exist, create
                    Directory.CreateDirectory(dir);
                System.IO.File.WriteAllText(Path.Combine(dir, filename), LogHelper.CreateExceptionString(ex));
                #endregion
            }
            ABB_Company aBB_Companys = new ABB_Company();
            return View(aBB_Companys);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion

        #region Get 360 degree View
        [LoginAuthorize]
        public ActionResult get360view()
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

        #region Manage Staff
        [LoginAuthorize]
        public ActionResult manageStaff()
        {
            try
            {
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
             this.ControllerContext.RouteData.Values["action"].ToString(),
             "The user " + Login.FirstName + " " + Login.LastName + "Entered into Staff Page");
                return View();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region Manage ManDate
        [LoginAuthorize]
        public ActionResult manageManDate()
        {
            try
            {
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
             this.ControllerContext.RouteData.Values["action"].ToString(),
             "The user " + Login.FirstName + " " + Login.LastName + "Entered into Mandate Page");
                return View();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region Request Finance
        [LoginAuthorize]
        public ActionResult RequestFinance(string companyid)
        {
            try
            {
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
             this.ControllerContext.RouteData.Values["action"].ToString(),
             "The user " + Login.FirstName + " " + Login.LastName + "Entered into Request Finance Page");
                Session["companyid"] = companyid;
                return View();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region  Relationship with other Banks
        [LoginAuthorize]
        public ActionResult RelationshipwithotherBanks()
        {
            try
            {
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
             this.ControllerContext.RouteData.Values["action"].ToString(),
             "The user " + Login.FirstName + " " + Login.LastName + "Entered into Relationship with other Banks Page");
                return View();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region Get image from Base64 string
        [LoginAuthorize]
        public ActionResult getimg(string bstring)
        {
            try
            {
                byte[] imageData = new byte[bstring.Length * sizeof(char)];
                return File(imageData, "image/png"); // Might need to adjust the content type based on your actual image type
            }
            catch (Exception ex)
            {
                throw;
                //#region Exception Logging
                //var filename = this.GetType().Name + "_" + System.Reflection.MethodBase.GetCurrentMethod().Name + "_" + (DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Millisecond).ToString() + ".txt";
                //var dir = Server.MapPath("~/Exceptions/" + (DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year).ToString());  // folder location
                //if (!Directory.Exists(dir))  // if it doesn't exist, create
                //    Directory.CreateDirectory(dir);
                //System.IO.File.WriteAllText(Path.Combine(dir, filename), LogHelper.CreateExceptionString(ex));
                //#endregion
            }

        }
        #endregion

        #region Add Share Holder
        [HttpPost]
        [LoginAuthorize]
        public async Task<bool> AddShareholder(string CompanyId, string FirstName, string MiddleName,
            string LastName, string RPNumber, string RPExpiary, string PassportNumber, string PassportExpiary)
        {
            try
            {
                if (!string.IsNullOrEmpty(CompanyId)
                    && !string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(LastName)
                    && !string.IsNullOrEmpty(RPNumber) && !string.IsNullOrEmpty(RPExpiary)
                    && !string.IsNullOrEmpty(PassportNumber) && !string.IsNullOrEmpty(PassportExpiary))
                {
                    ABB_CompanyShareHolders tb = new ABB_CompanyShareHolders();
                    tb.OrgId = Login.OrganizationId;
                    tb.CompanyId = Convert.ToInt64(CompanyId);
                    tb.FirstName = FirstName;
                    tb.MiddleName = MiddleName;
                    tb.LastName = LastName;
                    tb.RPNumber = RPNumber;
                    tb.RPExpiary = Convert.ToDateTime(RPExpiary);
                    tb.PassportNumber = PassportNumber;
                    tb.PassportExpiary = Convert.ToDateTime(PassportExpiary);
                    tb.CreateDate = tb.ModifiedDate = DateTime.Now;
                    tb.MarkAsDelete = false;
                    tb.ModifiedBy = Login.Id;
                    db.ABB_CompanyShareHolders.Add(tb);
                    await db.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                #region Exception Logging
                var filename = this.GetType().Name + "_" + System.Reflection.MethodBase.GetCurrentMethod().Name + "_" + (DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Millisecond).ToString() + ".txt";
                var dir = Server.MapPath("~/Exceptions/" + (DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year).ToString());  // folder location
                if (!Directory.Exists(dir))  // if it doesn't exist, create
                    Directory.CreateDirectory(dir);
                System.IO.File.WriteAllText(Path.Combine(dir, filename), LogHelper.CreateExceptionString(ex));
                #endregion
            }
            return false;


        }
        #endregion

        #region Edit Share Holder Get
        [LoginAuthorize]
        public ActionResult EditShareHolder(int? ID)
        {
            try
            {
                if (ID == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Session["EditShareId"] = ID;
                //ABB_CompanyShareHolders bshareholder =db.ABB_CompanyShareHolders.Find(id);
                var result = (from c in db.ABB_CompanyShareHolders.AsEnumerable()
                              where c.Id == ID
                              select new
                              {
                                  Id = c.Id,
                                  FirstName = c.FirstName,
                                  LastName = c.LastName,
                                  MiddleName = c.MiddleName,
                                  RPNumber = c.RPNumber,
                                  RPExpiary = c.RPExpiary.ToShortDateString(),
                                  PassportNumber = c.PassportNumber,
                                  PassportExpiary = c.PassportExpiary.ToShortDateString()
                              }).FirstOrDefault();

                if (result == null)
                {
                    return HttpNotFound();
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                #region Exception Logging
                var filename = this.GetType().Name + "_" + System.Reflection.MethodBase.GetCurrentMethod().Name + "_" + (DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Millisecond).ToString() + ".txt";
                var dir = Server.MapPath("~/Exceptions/" + (DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year).ToString());  // folder location
                if (!Directory.Exists(dir))  // if it doesn't exist, create
                    Directory.CreateDirectory(dir);
                System.IO.File.WriteAllText(Path.Combine(dir, filename), LogHelper.CreateExceptionString(ex));
                #endregion
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Edit Share Holder Post
        [HttpPost]
        [LoginAuthorize]
        public bool EditShareHolderPost(string CompanyId, string FirstName, string MiddleName,
            string LastName, string RPNumber, string RPExpiary, string PassportNumber, string PassportExpiary)
        {
            String EditShareId = Session["EditShareId"].ToString();
            try
            {
                if (!String.IsNullOrEmpty(EditShareId) && !String.IsNullOrEmpty(CompanyId) && !String.IsNullOrEmpty(FirstName) &&
                    !String.IsNullOrEmpty(MiddleName) && !String.IsNullOrEmpty(LastName) && !String.IsNullOrEmpty(RPNumber) && !String.IsNullOrEmpty(RPExpiary)
                    && !String.IsNullOrEmpty(PassportNumber) && !String.IsNullOrEmpty(PassportExpiary))
                {

                    int ID = Convert.ToInt32(EditShareId);
                    var result = db.ABB_CompanyShareHolders.Where(a => a.Id == ID).FirstOrDefault();
                    result.CompanyId = Convert.ToInt64(CompanyId);
                    result.FirstName = FirstName;
                    result.MiddleName = MiddleName;
                    result.LastName = LastName;
                    result.RPNumber = RPNumber;
                    result.RPExpiary = Convert.ToDateTime(RPExpiary);
                    result.PassportNumber = PassportNumber;
                    result.PassportExpiary = Convert.ToDateTime(PassportExpiary);
                    result.OrgId = Login.OrganizationId;
                    result.ModifiedBy = Login.Id;
                    result.ModifiedDate = DateTime.Now;
                    db.SaveChanges();
                    return true;

                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                #region Exception Logging
                var filename = this.GetType().Name + "_" + System.Reflection.MethodBase.GetCurrentMethod().Name + "_" + (DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Millisecond).ToString() + ".txt";
                var dir = Server.MapPath("~/Exceptions/" + (DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year).ToString());  // folder location
                if (!Directory.Exists(dir))  // if it doesn't exist, create
                    Directory.CreateDirectory(dir);
                System.IO.File.WriteAllText(Path.Combine(dir, filename), LogHelper.CreateExceptionString(ex));
                #endregion
            }
            return false;
        }
        #endregion

        #region Delete Share Holder Post
        [HttpPost]
        [LoginAuthorize]
        public bool DeleteShareHolderPost(string id)
        {
            try
            {
                int ID = Convert.ToInt32(id);
                ABB_CompanyShareHolders tbl = db.ABB_CompanyShareHolders.Find(ID);
                tbl.MarkAsDelete = true;
                tbl.OrgId = Login.OrganizationId;
                tbl.ModifiedDate = DateTime.Now;
                tbl.ModifiedBy = Login.Id;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                #region Exception Logging
                var filename = this.GetType().Name + "_" + System.Reflection.MethodBase.GetCurrentMethod().Name + "_" + (DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Millisecond).ToString() + ".txt";
                var dir = Server.MapPath("~/Exceptions/" + (DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year).ToString());  // folder location
                if (!Directory.Exists(dir))  // if it doesn't exist, create
                    Directory.CreateDirectory(dir);
                System.IO.File.WriteAllText(Path.Combine(dir, filename), LogHelper.CreateExceptionString(ex));
                #endregion
            }
            return false;

        }
        #endregion

        #region Edit Board Member Get
        [LoginAuthorize]
        public ActionResult EditBoardMember(int? ID)
        {
            try
            {
                if (ID == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Session["EditBoardId"] = ID;
                //ABB_CompanyShareHolders bshareholder =db.ABB_CompanyShareHolders.Find(id);
                var result = (from c in db.ABB_CompanyBoardMembers.AsEnumerable()
                              where c.Id == ID
                              select new
                              {
                                  Id = c.Id,
                                  FirstName = c.FirstName,
                                  LastName = c.LastName,
                                  MiddleName = c.MiddleName,
                                  RPNumber = c.RPNumber,
                                  RPExpiary = c.RPExpiary.ToShortDateString(),
                                  PassportNumber = c.PassportNumber,
                                  PassportExpiary = c.PassportExpiary.ToShortDateString()
                              }).FirstOrDefault();

                if (result == null)
                {
                    return HttpNotFound();
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                #region Exception Logging
                var filename = this.GetType().Name + "_" + System.Reflection.MethodBase.GetCurrentMethod().Name + "_" + (DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Millisecond).ToString() + ".txt";
                var dir = Server.MapPath("~/Exceptions/" + (DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year).ToString());  // folder location
                if (!Directory.Exists(dir))  // if it doesn't exist, create
                    Directory.CreateDirectory(dir);
                System.IO.File.WriteAllText(Path.Combine(dir, filename), LogHelper.CreateExceptionString(ex));
                #endregion
            }
            return Json(null, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region Edit Board Member Post
        [HttpPost]
        [LoginAuthorize]
        public bool EditBoardMemberPost(string CompanyId, string FirstName, string MiddleName,
            string LastName, string RPNumber, string RPExpiary, string PassportNumber, string PassportExpiary)
        {
            String EditBoardId = Session["EditBoardId"].ToString();
            try
            {
                if (!String.IsNullOrEmpty(EditBoardId) && !String.IsNullOrEmpty(CompanyId) && !String.IsNullOrEmpty(FirstName) &&
                    !String.IsNullOrEmpty(MiddleName) && !String.IsNullOrEmpty(LastName) && !String.IsNullOrEmpty(RPNumber) && !String.IsNullOrEmpty(RPExpiary)
                    && !String.IsNullOrEmpty(PassportNumber) && !String.IsNullOrEmpty(PassportExpiary))
                {

                    int ID = Convert.ToInt32(EditBoardId);
                    var result = db.ABB_CompanyBoardMembers.Where(a => a.Id == ID).FirstOrDefault();
                    result.CompanyId = Convert.ToInt64(CompanyId);
                    result.FirstName = FirstName;
                    result.MiddleName = MiddleName;
                    result.LastName = LastName;
                    result.RPNumber = RPNumber;
                    result.RPExpiary = Convert.ToDateTime(RPExpiary);
                    result.PassportNumber = PassportNumber;
                    result.PassportExpiary = Convert.ToDateTime(PassportExpiary);
                    result.OrgId = Login.OrganizationId;
                    result.ModifiedBy = Login.Id;
                    result.ModifiedDate = DateTime.Now;
                    db.SaveChanges();
                    return true;

                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                #region Exception Logging
                var filename = this.GetType().Name + "_" + System.Reflection.MethodBase.GetCurrentMethod().Name + "_" + (DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Millisecond).ToString() + ".txt";
                var dir = Server.MapPath("~/Exceptions/" + (DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year).ToString());  // folder location
                if (!Directory.Exists(dir))  // if it doesn't exist, create
                    Directory.CreateDirectory(dir);
                System.IO.File.WriteAllText(Path.Combine(dir, filename), LogHelper.CreateExceptionString(ex));
                #endregion
            }
            return false;
        }
        #endregion

        #region Delete Board Member Post
        [HttpPost]
        [LoginAuthorize]
        public bool DeleteBoardMemberPost(string id)
        {
            try
            {
                int ID = Convert.ToInt32(id);
                ABB_CompanyBoardMembers tbl = db.ABB_CompanyBoardMembers.Find(ID);
                tbl.MarkAsDelete = true;
                tbl.OrgId = Login.OrganizationId;
                tbl.ModifiedDate = DateTime.Now;
                tbl.ModifiedBy = Login.Id;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                #region Exception Logging
                var filename = this.GetType().Name + "_" + System.Reflection.MethodBase.GetCurrentMethod().Name + "_" + (DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Millisecond).ToString() + ".txt";
                var dir = Server.MapPath("~/Exceptions/" + (DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year).ToString());  // folder location
                if (!Directory.Exists(dir))  // if it doesn't exist, create
                    Directory.CreateDirectory(dir);
                System.IO.File.WriteAllText(Path.Combine(dir, filename), LogHelper.CreateExceptionString(ex));
                #endregion
            }
            return false;

        }
        #endregion

        #region Edit Management Team Get
        [LoginAuthorize]
        public ActionResult EditManagementTeam(int? ID)
        {
            try
            {
                if (ID == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Session["EditManagementId"] = ID;
                //ABB_CompanyShareHolders bshareholder =db.ABB_CompanyShareHolders.Find(id);
                var result = (from c in db.ABB_CompanyManagmentTeam_Staff.AsEnumerable()
                              where c.Id == ID
                              select new
                              {
                                  Id = c.Id,
                                  FirstName = c.FIrstName,
                                  LastName = c.LastName,
                                  MiddleName = c.MiddleName,
                                  RPNumber = c.RPNumber,
                                  RPExpiary = c.RPExpiry.ToShortDateString(),
                                  PassportNumber = c.PassportNumber,
                                  PassportExpiary = c.Passportexpiry.ToShortDateString(),
                                  JobTitleId = c.JobTitleId
                              }).FirstOrDefault();

                if (result == null)
                {
                    return HttpNotFound();
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                #region Exception Logging
                var filename = this.GetType().Name + "_" + System.Reflection.MethodBase.GetCurrentMethod().Name + "_" + (DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Millisecond).ToString() + ".txt";
                var dir = Server.MapPath("~/Exceptions/" + (DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year).ToString());  // folder location
                if (!Directory.Exists(dir))  // if it doesn't exist, create
                    Directory.CreateDirectory(dir);
                System.IO.File.WriteAllText(Path.Combine(dir, filename), LogHelper.CreateExceptionString(ex));
                #endregion
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Edit Management Team Post
        [HttpPost]
        [LoginAuthorize]
        public bool EditManagementTeamPost(string CompanyId, string FirstName, string MiddleName,
            string LastName, string RPNumber, string RPExpiary, string PassportNumber, string PassportExpiary, string JobTitleId)
        {
            String EditManagementId = Session["EditManagementId"].ToString();
            try
            {
                if (!String.IsNullOrEmpty(EditManagementId) && !String.IsNullOrEmpty(CompanyId) && !String.IsNullOrEmpty(FirstName) &&
                    !String.IsNullOrEmpty(MiddleName) && !String.IsNullOrEmpty(LastName) && !String.IsNullOrEmpty(RPNumber) && !String.IsNullOrEmpty(RPExpiary)
                    && !String.IsNullOrEmpty(PassportNumber) && !String.IsNullOrEmpty(PassportExpiary) && !String.IsNullOrEmpty(JobTitleId))
                {

                    int ID = Convert.ToInt32(EditManagementId);
                    var result = db.ABB_CompanyManagmentTeam_Staff.Where(a => a.Id == ID).FirstOrDefault();
                    result.CompanyId = Convert.ToInt64(CompanyId);
                    result.FIrstName = FirstName;
                    result.MiddleName = MiddleName;
                    result.LastName = LastName;
                    result.RPNumber = RPNumber;
                    result.RPExpiry = Convert.ToDateTime(RPExpiary);
                    result.PassportNumber = PassportNumber;
                    result.Passportexpiry = Convert.ToDateTime(PassportExpiary);
                    result.JobTitleId = Convert.ToInt32(JobTitleId);
                    result.OrgId = Login.OrganizationId;
                    result.ModifiedBy = Login.Id;
                    result.ModifiedDate = DateTime.Now;
                    db.SaveChanges();
                    return true;

                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                #region Exception Logging
                var filename = this.GetType().Name + "_" + System.Reflection.MethodBase.GetCurrentMethod().Name + "_" + (DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Millisecond).ToString() + ".txt";
                var dir = Server.MapPath("~/Exceptions/" + (DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year).ToString());  // folder location
                if (!Directory.Exists(dir))  // if it doesn't exist, create
                    Directory.CreateDirectory(dir);
                System.IO.File.WriteAllText(Path.Combine(dir, filename), LogHelper.CreateExceptionString(ex));
                #endregion
            }
            return false;
        }
        #endregion

        #region Delete Management Team Post
        [HttpPost]
        [LoginAuthorize]
        public bool DeleteManagementTeamPost(string id)
        {
            try
            {
                int ID = Convert.ToInt32(id);
                ABB_CompanyManagmentTeam_Staff tbl = db.ABB_CompanyManagmentTeam_Staff.Find(ID);
                tbl.MarkAsDelete = true;
                tbl.OrgId = Login.OrganizationId;
                tbl.ModifiedDate = DateTime.Now;
                tbl.ModifiedBy = Login.Id;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                #region Exception Logging
                var filename = this.GetType().Name + "_" + System.Reflection.MethodBase.GetCurrentMethod().Name + "_" + (DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Millisecond).ToString() + ".txt";
                var dir = Server.MapPath("~/Exceptions/" + (DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year).ToString());  // folder location
                if (!Directory.Exists(dir))  // if it doesn't exist, create
                    Directory.CreateDirectory(dir);
                System.IO.File.WriteAllText(Path.Combine(dir, filename), LogHelper.CreateExceptionString(ex));
                #endregion
            }
            return false;

        }
        #endregion

        #region ListofShareHolders
        [LoginAuthorize]
        public ActionResult ListofShareHolders(string sortOrder, string currentFilter, string searchString, int? page)
        {
            try
            {
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
             this.ControllerContext.RouteData.Values["action"].ToString(),
             "The user " + Login.FirstName + " " + Login.LastName + "Entered into Share Holder List Page");
                FrameworkEntities dc = new FrameworkEntities();
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return View(dc.ABB_CompanyShareHolders.AsEnumerable().Where(a => a.MarkAsDelete == false && a.OrgId == Login.OrganizationId).ToPagedList(pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                #region Exception Logging
                var filename = this.GetType().Name + "_" + System.Reflection.MethodBase.GetCurrentMethod().Name + "_" + (DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Millisecond).ToString() + ".txt";
                var dir = Server.MapPath("~/Exceptions/" + (DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year).ToString());  // folder location
                if (!Directory.Exists(dir))  // if it doesn't exist, create
                    Directory.CreateDirectory(dir);
                System.IO.File.WriteAllText(Path.Combine(dir, filename), LogHelper.CreateExceptionString(ex));
                #endregion
            }
            int pageNumberone = (page ?? 1);
            List<ABB_CompanyShareHolders> lst = new List<ABB_CompanyShareHolders>();
            return View(lst.ToPagedList(pageNumberone, 10));
        }

        #endregion

        #region Add Board Members
        [HttpPost]
        [LoginAuthorize]
        public async Task<bool> AddBoardMember(string CompanyId, string FirstName, string MiddleName,
            string LastName, string RPNumber, string RPExpiary, string PassportNumber, string PassportExpiary)
        {
            try
            {
                if (!string.IsNullOrEmpty(CompanyId)
                    && !string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(LastName)
                    && !string.IsNullOrEmpty(RPNumber) && !string.IsNullOrEmpty(RPExpiary)
                    && !string.IsNullOrEmpty(PassportNumber) && !string.IsNullOrEmpty(PassportExpiary))
                {
                    ABB_CompanyBoardMembers tb = new ABB_CompanyBoardMembers();
                    tb.OrgId = Login.OrganizationId;
                    tb.CompanyId = Convert.ToInt64(CompanyId);
                    tb.FirstName = FirstName;
                    tb.MiddleName = MiddleName;
                    tb.LastName = LastName;
                    tb.RPNumber = RPNumber;
                    tb.RPExpiary = Convert.ToDateTime(RPExpiary);
                    tb.PassportNumber = PassportNumber;
                    tb.PassportExpiary = Convert.ToDateTime(PassportExpiary);
                    tb.CreateDate = tb.ModifiedDate = DateTime.Now;
                    tb.MarkAsDelete = false;
                    tb.ModifiedBy = Login.Id;
                    db.ABB_CompanyBoardMembers.Add(tb);
                    await db.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                #region Exception Logging
                var filename = this.GetType().Name + "_" + System.Reflection.MethodBase.GetCurrentMethod().Name + "_" + (DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Millisecond).ToString() + ".txt";
                var dir = Server.MapPath("~/Exceptions/" + (DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year).ToString());  // folder location
                if (!Directory.Exists(dir))  // if it doesn't exist, create
                    Directory.CreateDirectory(dir);
                System.IO.File.WriteAllText(Path.Combine(dir, filename), LogHelper.CreateExceptionString(ex));
                #endregion
            }
            return false;


        }
        #endregion

        #region Listofboardmembers
        [LoginAuthorize]
        public ActionResult Listofboardmembers(string sortOrder, string currentFilter, string searchString, int? page)
        {
            try
            {
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(),
            "The user " + Login.FirstName + " " + Login.LastName + "Entered into Board Member List Page");
                FrameworkEntities dc = new FrameworkEntities();
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return View(dc.ABB_CompanyBoardMembers.AsEnumerable().Where(a => a.MarkAsDelete == false && a.OrgId == Login.OrganizationId).ToPagedList(pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                #region Exception Logging
                var filename = this.GetType().Name + "_" + System.Reflection.MethodBase.GetCurrentMethod().Name + "_" + (DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Millisecond).ToString() + ".txt";
                var dir = Server.MapPath("~/Exceptions/" + (DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year).ToString());  // folder location
                if (!Directory.Exists(dir))  // if it doesn't exist, create
                    Directory.CreateDirectory(dir);
                System.IO.File.WriteAllText(Path.Combine(dir, filename), LogHelper.CreateExceptionString(ex));
                #endregion
            }
            int pageNumberone = (page ?? 1);
            List<ABB_CompanyBoardMembers> lst = new List<ABB_CompanyBoardMembers>();
            return View(lst.ToPagedList(pageNumberone, 10));
        }

        #endregion

        #region Add Staff
        [HttpPost]
        [LoginAuthorize]
        public async Task<bool> AddStaff(string CompanyId, string FirstName, string MiddleName,
            string LastName, string RPNumber, string RPExpiary, string PassportNumber, string PassportExpiary, string JobTitleId)
        {
            try
            {
                if (!string.IsNullOrEmpty(CompanyId)
                    && !string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(LastName)
                    && !string.IsNullOrEmpty(RPNumber) && !string.IsNullOrEmpty(RPExpiary)
                    && !string.IsNullOrEmpty(PassportNumber) && !string.IsNullOrEmpty(PassportExpiary))
                {
                    ABB_CompanyManagmentTeam_Staff tb = new ABB_CompanyManagmentTeam_Staff();
                    tb.OrgId = Login.OrganizationId;
                    tb.CompanyId = Convert.ToInt64(CompanyId);
                    tb.FIrstName = FirstName;
                    tb.MiddleName = MiddleName;
                    tb.LastName = LastName;
                    tb.JobTitleId = Convert.ToInt32(JobTitleId);
                    tb.RPNumber = RPNumber;
                    tb.RPExpiry = Convert.ToDateTime(RPExpiary).Date;
                    tb.PassportNumber = PassportNumber;
                    tb.Passportexpiry = Convert.ToDateTime(PassportExpiary);
                    tb.CreateDate = tb.ModifiedDate = DateTime.Now;
                    tb.MarkAsDelete = false;
                    tb.ModifiedBy = Login.Id;
                    db.ABB_CompanyManagmentTeam_Staff.Add(tb);
                    await db.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                #region Exception Logging
                var filename = this.GetType().Name + "_" + System.Reflection.MethodBase.GetCurrentMethod().Name + "_" + (DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Millisecond).ToString() + ".txt";
                var dir = Server.MapPath("~/Exceptions/" + (DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year).ToString());  // folder location
                if (!Directory.Exists(dir))  // if it doesn't exist, create
                    Directory.CreateDirectory(dir);
                System.IO.File.WriteAllText(Path.Combine(dir, filename), LogHelper.CreateExceptionString(ex));
                #endregion
            }
            return false;


        }
        #endregion

        #region Listofstaff
        [LoginAuthorize]
        public ActionResult Listofstaff(string sortOrder, string currentFilter, string searchString, int? page)
        {
            try
            {
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(),
            "The user " + Login.FirstName + " " + Login.LastName + "Entered into Staff List Page");
                FrameworkEntities dc = new FrameworkEntities();
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return View(dc.ABB_CompanyManagmentTeam_Staff.AsEnumerable().Where(a => a.MarkAsDelete == false && a.OrgId == Login.OrganizationId).ToPagedList(pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                #region Exception Logging
                var filename = this.GetType().Name + "_" + System.Reflection.MethodBase.GetCurrentMethod().Name + "_" + (DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Millisecond).ToString() + ".txt";
                var dir = Server.MapPath("~/Exceptions/" + (DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year).ToString());  // folder location
                if (!Directory.Exists(dir))  // if it doesn't exist, create
                    Directory.CreateDirectory(dir);
                System.IO.File.WriteAllText(Path.Combine(dir, filename), LogHelper.CreateExceptionString(ex));
                #endregion
            }
            int pageNumberone = (page ?? 1);
            List<ABB_CompanyManagmentTeam_Staff> lst = new List<ABB_CompanyManagmentTeam_Staff>();
            return View(lst.ToPagedList(pageNumberone, 10));
        }

        #endregion

        #region Add Mandate assigned
        [HttpPost]
        [LoginAuthorize]
        public bool AddMandate(String CompanyId, String AccountId, string StaffId,
            string AssetId, string ChannelId, string FromDate, string ToDate, string FromValue, string ToValue, string JoinTypeId)
        {
            try
            {
                if (!String.IsNullOrEmpty(CompanyId) && !String.IsNullOrEmpty(AccountId) &&
                    !String.IsNullOrEmpty(StaffId) && !String.IsNullOrEmpty(AssetId) && !String.IsNullOrEmpty(ChannelId) &&
                    !String.IsNullOrEmpty(FromDate) && !String.IsNullOrEmpty(ToDate) &&
                    !String.IsNullOrEmpty(FromValue) && !String.IsNullOrEmpty(ToValue) && !String.IsNullOrEmpty(JoinTypeId))
                {
                    ABB_CompanyManDateAssigned tbl = new ABB_CompanyManDateAssigned();
                    tbl.OrgId = Login.OrganizationId;
                    tbl.CompanyId = Convert.ToInt64(CompanyId);
                    tbl.AccountId = Convert.ToInt32(AccountId);
                    tbl.StaffId = Convert.ToInt64(StaffId);
                    tbl.AssetId = Convert.ToInt32(AssetId);
                    tbl.ChannelId = Convert.ToInt32(ChannelId);
                    tbl.FromDate = Convert.ToDateTime(FromDate);
                    tbl.ToDate = Convert.ToDateTime(ToDate);
                    tbl.FromValue = decimal.Parse(Regex.Replace(FromValue, @"[^\d.]", ""));
                    tbl.ToValue = decimal.Parse(Regex.Replace(ToValue, @"[^\d.]", ""));
                    tbl.JoinTypeId = Convert.ToInt32(StaffId);
                    tbl.CreateDate = tbl.ModifiedDate = DateTime.Now;
                    tbl.ModifiedBy = Login.Id;
                    tbl.MarkAsDelete = false;
                    FrameworkEntities dc = new FrameworkEntities();
                    dc.ABB_CompanyManDateAssigned.Add(tbl);
                    dc.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                #region Exception Logging
                var filename = this.GetType().Name + "_" + System.Reflection.MethodBase.GetCurrentMethod().Name + "_" + (DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Millisecond).ToString() + ".txt";
                var dir = Server.MapPath("~/Exceptions/" + (DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year).ToString());  // folder location
                if (!Directory.Exists(dir))  // if it doesn't exist, create
                    Directory.CreateDirectory(dir);
                System.IO.File.WriteAllText(Path.Combine(dir, filename), LogHelper.CreateExceptionString(ex));
                #endregion
            }
            return false;
        }
        #endregion

        #region EditMandates
        [LoginAuthorize]
        public ActionResult EditMandates(int? ID)
        {
            try
            {

                if (ID == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Session["EditMandateId"] = ID;
                //ABB_CompanyShareHolders bshareholder =db.ABB_CompanyShareHolders.Find(id);
                var result = (from c in db.ABB_CompanyManDateAssigned.AsEnumerable()
                              where c.Id == ID
                              select new
                              {
                                  CompanyId = c.CompanyId,
                                  AccountId = c.AccountId,
                                  StaffId = c.StaffId,
                                  AssetId = c.AssetId,
                                  ChannelId = c.ChannelId,
                                  FromDate = c.FromDate.ToShortDateString(),
                                  ToDate = c.ToDate.ToShortDateString(),
                                  FromValue = c.FromValue,
                                  ToValue = c.ToValue,
                                  JoinTypeId = c.JoinTypeId
                              }).FirstOrDefault();

                if (result == null)
                {
                    return HttpNotFound();
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                #region Exception Logging
                var filename = this.GetType().Name + "_" + System.Reflection.MethodBase.GetCurrentMethod().Name + "_" + (DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Millisecond).ToString() + ".txt";
                var dir = Server.MapPath("~/Exceptions/" + (DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year).ToString());  // folder location
                if (!Directory.Exists(dir))  // if it doesn't exist, create
                    Directory.CreateDirectory(dir);
                System.IO.File.WriteAllText(Path.Combine(dir, filename), LogHelper.CreateExceptionString(ex));
                #endregion
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Edit Mandates Post

        [HttpPost]
        [LoginAuthorize]
        public bool EditMandatesPost(String CompanyId, String AccountId, string StaffId,
            string AssetId, string ChannelId, string FromDate, string ToDate, string FromValue, string ToValue, string JoinTypeId)
        {
            String EditMandateId = Session["EditMandateId"].ToString();
            try
            {
                if (!String.IsNullOrEmpty(EditMandateId) && !String.IsNullOrEmpty(CompanyId) && !String.IsNullOrEmpty(AccountId) &&
                    !String.IsNullOrEmpty(StaffId) && !String.IsNullOrEmpty(ChannelId) && !String.IsNullOrEmpty(FromDate) && !String.IsNullOrEmpty(ToDate)
                    && !String.IsNullOrEmpty(FromValue) && !String.IsNullOrEmpty(ToValue) && !String.IsNullOrEmpty(JoinTypeId))
                {

                    int ID = Convert.ToInt32(EditMandateId);
                    var result = db.ABB_CompanyManDateAssigned.Where(a => a.Id == ID).FirstOrDefault();
                    result.CompanyId = Convert.ToInt64(CompanyId);
                    result.AccountId = Convert.ToInt32(AccountId);
                    result.StaffId = Convert.ToInt64(StaffId);
                    result.AssetId = Convert.ToInt32(AssetId);
                    result.ChannelId = Convert.ToInt32(ChannelId);
                    result.FromDate = Convert.ToDateTime(FromDate);
                    result.ToDate = Convert.ToDateTime(ToDate);
                    result.FromValue = decimal.Parse(Regex.Replace(FromValue, @"[^\d.]", ""));
                    result.ToValue = decimal.Parse(Regex.Replace(ToValue, @"[^\d.]", ""));
                    result.JoinTypeId = Convert.ToInt32(JoinTypeId);
                    result.OrgId = Login.OrganizationId;
                    result.ModifiedBy = Login.Id;
                    result.ModifiedDate = DateTime.Now;
                    db.SaveChanges();
                    return true;

                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                #region Exception Logging
                var filename = this.GetType().Name + "_" + System.Reflection.MethodBase.GetCurrentMethod().Name + "_" + (DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Millisecond).ToString() + ".txt";
                var dir = Server.MapPath("~/Exceptions/" + (DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year).ToString());  // folder location
                if (!Directory.Exists(dir))  // if it doesn't exist, create
                    Directory.CreateDirectory(dir);
                System.IO.File.WriteAllText(Path.Combine(dir, filename), LogHelper.CreateExceptionString(ex));
                #endregion

            }
            return false;
        }

        #endregion

        #region Delete Mandate Post
        [HttpPost]
        [LoginAuthorize]
        public bool DeleteMandatePost(string id)
        {
            try
            {
                int ID = Convert.ToInt32(id);
                ABB_CompanyManDateAssigned tbl = db.ABB_CompanyManDateAssigned.Find(ID);
                tbl.MarkAsDelete = true;
                tbl.OrgId = Login.OrganizationId;
                tbl.ModifiedDate = DateTime.Now;
                tbl.ModifiedBy = Login.Id;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                #region Exception Logging
                var filename = this.GetType().Name + "_" + System.Reflection.MethodBase.GetCurrentMethod().Name + "_" + (DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Millisecond).ToString() + ".txt";
                var dir = Server.MapPath("~/Exceptions/" + (DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year).ToString());  // folder location
                if (!Directory.Exists(dir))  // if it doesn't exist, create
                    Directory.CreateDirectory(dir);
                System.IO.File.WriteAllText(Path.Combine(dir, filename), LogHelper.CreateExceptionString(ex));
                #endregion
            }
            return false;

        }
        #endregion

        #region ListofMandates
        [LoginAuthorize]
        public ActionResult ListofMandates(string sortOrder, string currentFilter, string searchString, int? page)
        {
            try
            {
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(),
            "The user " + Login.FirstName + " " + Login.LastName + "Entered into Mandate List Page");
                FrameworkEntities dc = new FrameworkEntities();
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return View(dc.ABB_CompanyManDateAssigned.AsEnumerable().Where(a => a.MarkAsDelete == false && a.OrgId == Login.OrganizationId).ToPagedList(pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                #region Exception Logging
                var filename = this.GetType().Name + "_" + System.Reflection.MethodBase.GetCurrentMethod().Name + "_" + (DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Millisecond).ToString() + ".txt";
                var dir = Server.MapPath("~/Exceptions/" + (DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year).ToString());  // folder location
                if (!Directory.Exists(dir))  // if it doesn't exist, create
                    Directory.CreateDirectory(dir);
                System.IO.File.WriteAllText(Path.Combine(dir, filename), LogHelper.CreateExceptionString(ex));
                #endregion
            }
            int pageNumberone = (page ?? 1);
            List<ABB_CompanyManDateAssigned> lst = new List<ABB_CompanyManDateAssigned>();
            return View(lst.ToPagedList(pageNumberone, 10));
        }

        #endregion

        #region Add Request Finance

        [HttpPost]
        [LoginAuthorize]
        public bool addrequestfinance(string CompanyId, string Purpose, string ProductId, string RequiredOn, string Tenure, string Amount, string PaymentDetails)
        {
            try
            {
                if (!string.IsNullOrEmpty(CompanyId) && !string.IsNullOrEmpty(Purpose) &&
                    !string.IsNullOrEmpty(ProductId) && !string.IsNullOrEmpty(RequiredOn) &&
                    !string.IsNullOrEmpty(Tenure) && !string.IsNullOrEmpty(Amount)
                    && !string.IsNullOrEmpty(PaymentDetails))
                {
                    ABB_RequestFinance tbl = new ABB_RequestFinance();
                    tbl.OrgId = Login.OrganizationId;
                    tbl.CompanyId = Convert.ToInt64(CompanyId);
                    tbl.ProductId = Convert.ToInt64(ProductId);
                    tbl.Purpose = Purpose;
                    tbl.RequiredOn = Convert.ToDateTime(RequiredOn);
                    tbl.Tenure = Tenure;
                    tbl.Amount = decimal.Parse(Regex.Replace(Amount, @"[^\d.]", ""));
                    tbl.PaymentDetails = PaymentDetails;
                    tbl.DealSatageId = Convert.ToInt32(Core.DealStagedef.Lead);
                    tbl.CreateDate = tbl.ModifiedDate = DateTime.Now;
                    tbl.ModifiedBy = Login.Id;
                    tbl.MarkAsDelete = false;
                    FrameworkEntities dc = new FrameworkEntities();
                    dc.ABB_RequestFinance.Add(tbl);
                    dc.SaveChanges();
                    return true;

                }
                return false;
            }
            catch (Exception ex)
            {
                #region Exception Logging
                var filename = this.GetType().Name + "_" + System.Reflection.MethodBase.GetCurrentMethod().Name + "_" + (DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Millisecond).ToString() + ".txt";
                var dir = Server.MapPath("~/Exceptions/" + (DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year).ToString());  // folder location
                if (!Directory.Exists(dir))  // if it doesn't exist, create
                    Directory.CreateDirectory(dir);
                System.IO.File.WriteAllText(Path.Combine(dir, filename), LogHelper.CreateExceptionString(ex));
                #endregion
            }
            return false;
        }

        #endregion

        #region Edit Request Finance
        [LoginAuthorize]
        public ActionResult EditRequestFinance(int? ID)
        {
            try
            {

                if (ID == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Session["EditRequestId"] = ID;
                //ABB_CompanyShareHolders bshareholder =db.ABB_CompanyShareHolders.Find(id);
                var result = (from c in db.ABB_RequestFinance.AsEnumerable()
                              where c.Id == ID
                              select new
                              {
                                  CompanyId = c.CompanyId,
                                  ProductId = c.ProductId,
                                  Purpose = c.Purpose,
                                  RequiredOn = c.RequiredOn.ToShortDateString(),
                                  Tenure = c.Tenure,
                                  Amount = c.Amount,
                                  PaymentDetails = c.PaymentDetails,
                                  DealSatageId = c.DealSatageId
                              }).FirstOrDefault();

                if (result == null)
                {
                    return HttpNotFound();
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                #region Exception Logging
                var filename = this.GetType().Name + "_" + System.Reflection.MethodBase.GetCurrentMethod().Name + "_" + (DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Millisecond).ToString() + ".txt";
                var dir = Server.MapPath("~/Exceptions/" + (DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year).ToString());  // folder location
                if (!Directory.Exists(dir))  // if it doesn't exist, create
                    Directory.CreateDirectory(dir);
                System.IO.File.WriteAllText(Path.Combine(dir, filename), LogHelper.CreateExceptionString(ex));
                #endregion
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Edit Request Finances Post

        [HttpPost]
        [LoginAuthorize]
        public bool EditRequestFinancePost(string CompanyId, string Purpose, string ProductId, string RequiredOn, string Tenure, string Amount, string PaymentDetails)
        {
            String EditRequestId = Session["EditRequestId"].ToString();
            try
            {
                if (!string.IsNullOrEmpty(EditRequestId) && !string.IsNullOrEmpty(CompanyId) && !string.IsNullOrEmpty(Purpose) &&
                    !string.IsNullOrEmpty(ProductId) && !string.IsNullOrEmpty(RequiredOn) &&
                    !string.IsNullOrEmpty(Tenure) && !string.IsNullOrEmpty(Amount)
                    && !string.IsNullOrEmpty(PaymentDetails))
                {
                    int ID = Convert.ToInt32(EditRequestId);
                    var result = db.ABB_RequestFinance.Where(a => a.Id == ID).FirstOrDefault();
                    result.CompanyId = Convert.ToInt64(CompanyId);
                    result.ProductId = Convert.ToInt64(ProductId);
                    result.Purpose = Purpose;
                    result.RequiredOn = Convert.ToDateTime(RequiredOn);
                    result.Tenure = Tenure;
                    result.Amount = decimal.Parse(Regex.Replace(Amount, @"[^\d.]", ""));
                    result.PaymentDetails = PaymentDetails;
                    result.DealSatageId = Convert.ToInt32(Core.DealStagedef.Lead);
                    result.OrgId = Login.OrganizationId;
                    result.ModifiedBy = Login.Id;
                    result.ModifiedDate = DateTime.Now;
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                #region Exception Logging
                var filename = this.GetType().Name + "_" + System.Reflection.MethodBase.GetCurrentMethod().Name + "_" + (DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Millisecond).ToString() + ".txt";
                var dir = Server.MapPath("~/Exceptions/" + (DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year).ToString());  // folder location
                if (!Directory.Exists(dir))  // if it doesn't exist, create
                    Directory.CreateDirectory(dir);
                System.IO.File.WriteAllText(Path.Combine(dir, filename), LogHelper.CreateExceptionString(ex));
                #endregion

            }
            return false;
        }

        #endregion

        #region Delete Request Finance Post
        [HttpPost]
        [LoginAuthorize]
        public bool DeleteRequestFinancePost(string id)
        {
            try
            {
                int ID = Convert.ToInt32(id);
                ABB_RequestFinance tbl = db.ABB_RequestFinance.Find(ID);
                tbl.MarkAsDelete = true;
                tbl.OrgId = Login.OrganizationId;
                tbl.ModifiedDate = DateTime.Now;
                tbl.ModifiedBy = Login.Id;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                #region Exception Logging
                var filename = this.GetType().Name + "_" + System.Reflection.MethodBase.GetCurrentMethod().Name + "_" + (DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Millisecond).ToString() + ".txt";
                var dir = Server.MapPath("~/Exceptions/" + (DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year).ToString());  // folder location
                if (!Directory.Exists(dir))  // if it doesn't exist, create
                    Directory.CreateDirectory(dir);
                System.IO.File.WriteAllText(Path.Combine(dir, filename), LogHelper.CreateExceptionString(ex));
                #endregion
            }
            return false;

        }
        #endregion

        #region Listofrequestfinance
        [LoginAuthorize]
        public ActionResult Listofrequestfinance(string sortOrder, string currentFilter, string searchString, int? page)
        {
            try
            {
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                            this.ControllerContext.RouteData.Values["action"].ToString(),
                            "The user " + Login.FirstName + " " + Login.LastName + "Entered into Request Finance Page");
                FrameworkEntities dc = new FrameworkEntities();
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                // List<ABB_RequestFinance> list = new List<ABB_RequestFinance>();
                //if (Session["companyid"] != null)
                var list = dc.ABB_RequestFinance.AsEnumerable().Where(a => a.MarkAsDelete == false && a.CompanyId == Convert.ToInt64(Session["companyid"]) && a.OrgId == Login.OrganizationId).ToPagedList(pageNumber, pageSize);
                return View(list);
            }
            catch (Exception ex)
            {
                #region Exception Logging
                var filename = this.GetType().Name + "_" + System.Reflection.MethodBase.GetCurrentMethod().Name + "_" + (DateTime.Now.Hour + "_" + DateTime.Now.Minute + "_" + DateTime.Now.Millisecond).ToString() + ".txt";
                var dir = Server.MapPath("~/Exceptions/" + (DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year).ToString());  // folder location
                if (!Directory.Exists(dir))  // if it doesn't exist, create
                    Directory.CreateDirectory(dir);
                System.IO.File.WriteAllText(Path.Combine(dir, filename), LogHelper.CreateExceptionString(ex));
                #endregion
            }
            int pageNumberone = (page ?? 1);
            List<ABB_RequestFinance> lst = new List<ABB_RequestFinance>();
            return View(lst.ToPagedList(pageNumberone, 10));
        }

        #endregion
    }
}
