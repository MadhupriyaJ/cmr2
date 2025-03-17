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
using System.IO;
using PagedList.Mvc;
using PagedList;
using ExceptionLogging;

namespace FramWork.Controllers
{
    public class BB_Company_Doc_folderController : Controller
    {
        private FrameworkEntities db = new FrameworkEntities();

        #region Company
        // GET: BB_Company_Doc_folder
        [LoginAuthorize]
        public async Task<ActionResult> Company()
        {
            try
            {
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
           this.ControllerContext.RouteData.Values["action"].ToString(),
           "The user " + Login.FirstName + " " + Login.LastName + "Entered into Company Page");

                var aBB_Company = db.ABB_Company.Include(a => a.ABB_BusinessSector).Include(a => a.ABB_TypeOfBusiness).Include(a => a.Organizations).Include(a => a.Users);
                return View(await aBB_Company.ToListAsync());
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
            var aBB_Companys = new List<ABB_Company>();
            return View(aBB_Companys.ToList());
        }

        #endregion

        #region Index
        [LoginAuthorize]
        public async Task<ActionResult> Index(long? id)
        {
            try
            {
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
           this.ControllerContext.RouteData.Values["action"].ToString(),
           "The user " + Login.FirstName + " " + Login.LastName + "Entered into Company Document Folder Page");

                var aBB_Company_Doc_folder = db.ABB_Company_Doc_folder.Include(a => a.ABB_Company).Include(a => a.Organizations).Include(a => a.Users);
                //.Where(a=>a.CompanyId==id);
                return View(await aBB_Company_Doc_folder.ToListAsync());
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
            var aBB_Company_Doc_folders = new List<ABB_Company_Doc_folder>();
            return View(aBB_Company_Doc_folders.ToList());
        }

        #endregion

        #region Details
        // GET: BB_Company_Doc_folder/Details/5
        [LoginAuthorize]
        public async Task<ActionResult> Details(long? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ABB_Company_Doc_folder aBB_Company_Doc_folder = await db.ABB_Company_Doc_folder.FindAsync(id);
                if (aBB_Company_Doc_folder == null)
                {
                    return HttpNotFound();
                }
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
          this.ControllerContext.RouteData.Values["action"].ToString(),
          "The user " + Login.FirstName + " " + Login.LastName + "Entered into Company Document Folder Details Page");

                return View(aBB_Company_Doc_folder);
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
            ABB_Company_Doc_folder aBB_Company_Doc_folders = new ABB_Company_Doc_folder();
            return View(aBB_Company_Doc_folders);
        }

        #endregion

        #region DocFolderView
        [LoginAuthorize]
        public ActionResult DocFolderView()
        {
            try
            {
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
         this.ControllerContext.RouteData.Values["action"].ToString(),
         "The user " + Login.FirstName + " " + Login.LastName + "Entered into Document Folder Page");
                return View();
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Createfolder
        // GET: BB_Company_Doc_folder/Create
        [HttpPost]
        [LoginAuthorize]
        public bool Createfolder(string Companyid, string foldername, string description)
        {
            try
            {
                if (!String.IsNullOrEmpty(Companyid) && !String.IsNullOrEmpty(foldername) && !String.IsNullOrEmpty(description))
                {
                    ABB_Company_Doc_folder tbl = new ABB_Company_Doc_folder();
                    tbl.OrgId = Login.OrganizationId;
                    tbl.CompanyId = Convert.ToInt64(Companyid);
                    tbl.FolderName = foldername;
                    tbl.Description = description;
                    tbl.CreateDate = tbl.ModifiedDate = DateTime.Now;
                    tbl.ModifiedBy = Login.Id;
                    tbl.MarkAsDelete = false;
                    db.ABB_Company_Doc_folder.Add(tbl);
                    db.SaveChanges();
                    string subPath1 = "~/assets/Programmer/CompanyDocs/" + Companyid + "/" + tbl.Id; //create folder for emirate
                    bool exists1 = System.IO.Directory.Exists(Server.MapPath(subPath1));
                    if (!exists1)
                        System.IO.Directory.CreateDirectory(Server.MapPath(subPath1));
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

        #region Edit
        // GET: BB_Company_Doc_folder/Edit/5
        [LoginAuthorize]
        public async Task<ActionResult> Edit(long? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ABB_Company_Doc_folder aBB_Company_Doc_folder = await db.ABB_Company_Doc_folder.FindAsync(id);
                if (aBB_Company_Doc_folder == null)
                {
                    return HttpNotFound();
                }
                ViewBag.CompanyId = new SelectList(db.ABB_Company, "Id", "CRNo", aBB_Company_Doc_folder.CompanyId);
                ViewBag.OrgId = new SelectList(db.Organizations, "Id", "Alias", aBB_Company_Doc_folder.OrgId);
                ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Salutation", aBB_Company_Doc_folder.ModifiedBy);
                return View(aBB_Company_Doc_folder);
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
            ABB_Company_Doc_folder aBB_Company_Doc_folders = new ABB_Company_Doc_folder();
            ViewBag.CompanyId = new SelectList(db.ABB_Company, "Id", "CRNo", aBB_Company_Doc_folders.CompanyId);
            ViewBag.OrgId = new SelectList(db.Organizations, "Id", "Alias", aBB_Company_Doc_folders.OrgId);
            ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Salutation", aBB_Company_Doc_folders.ModifiedBy);
           
            return View(aBB_Company_Doc_folders);

        }

        // POST: BB_Company_Doc_folder/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [LoginAuthorize]
        public async Task<ActionResult> Edit([Bind(Include = "Id,OrgId,CompanyId,FolderName,Description,CreateDate,ModifiedDate,ModifiedBy")] ABB_Company_Doc_folder aBB_Company_Doc_folder)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(aBB_Company_Doc_folder).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                   this.ControllerContext.RouteData.Values["action"].ToString(),
                   "The user " + Login.FirstName + " " + Login.LastName + "Edited Document Folder Page");
                    return RedirectToAction("Index");
                }
                ViewBag.CompanyId = new SelectList(db.ABB_Company, "Id", "CRNo", aBB_Company_Doc_folder.CompanyId);
                ViewBag.OrgId = new SelectList(db.Organizations, "Id", "Alias", aBB_Company_Doc_folder.OrgId);
                ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Salutation", aBB_Company_Doc_folder.ModifiedBy);
                return View(aBB_Company_Doc_folder);
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
            ViewBag.CompanyId = new SelectList(db.ABB_Company, "Id", "CRNo", aBB_Company_Doc_folder.CompanyId);
            ViewBag.OrgId = new SelectList(db.Organizations, "Id", "Alias", aBB_Company_Doc_folder.OrgId);
            ViewBag.ModifiedBy = new SelectList(db.Users, "Id", "Salutation", aBB_Company_Doc_folder.ModifiedBy);
            return View(aBB_Company_Doc_folder);
        }

        #endregion

        #region Delete
        // GET: BB_Company_Doc_folder/Delete/5
        [LoginAuthorize]
        public async Task<ActionResult> Delete(long? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ABB_Company_Doc_folder aBB_Company_Doc_folder = await db.ABB_Company_Doc_folder.FindAsync(id);
                if (aBB_Company_Doc_folder == null)
                {
                    return HttpNotFound();
                }
                return View(aBB_Company_Doc_folder);
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
            ABB_Company_Doc_folder aBB_Company_Doc_folders = new ABB_Company_Doc_folder();
            return View(aBB_Company_Doc_folders);
        }

        // POST: BB_Company_Doc_folder/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [LoginAuthorize]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            try
            {
                ABB_Company_Doc_folder aBB_Company_Doc_folder = await db.ABB_Company_Doc_folder.FindAsync(id);
                aBB_Company_Doc_folder.MarkAsDelete = true;
                aBB_Company_Doc_folder.ModifiedDate = DateTime.Now;
                aBB_Company_Doc_folder.ModifiedBy = Login.Id;
                db.Entry(aBB_Company_Doc_folder).State = EntityState.Modified;
                //db.ABB_Company_Doc_folder.Remove(aBB_Company_Doc_folder);
                await db.SaveChangesAsync();
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
       this.ControllerContext.RouteData.Values["action"].ToString(),
       "The user " + Login.FirstName + " " + Login.LastName + "Deleted Document Folder Page");
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
            ABB_Company_Doc_folder aBB_Company_Doc_folders = new ABB_Company_Doc_folder();
            return View(aBB_Company_Doc_folders);

        }

        #endregion

        #region Dispose
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion

        #region Upload async FIles to folder
        [LoginAuthorize]
        public ActionResult Uploadfilestofolder(string company, string fid)
        {
            try
            {
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                   this.ControllerContext.RouteData.Values["action"].ToString(),
                   "The user " + Login.FirstName + " " + Login.LastName + "Entered into  Upload File List Page");
                return View();
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        [LoginAuthorize]
        public ActionResult Multiple(IEnumerable<HttpPostedFileBase> files, FormCollection c)
        {
            try
            {
                FrameworkEntities dc = new FrameworkEntities();
                if (!string.IsNullOrEmpty(c["txtfilename"]) && !string.IsNullOrEmpty(c["txtcompany"]) && !string.IsNullOrEmpty(c["txtfolder"]))
                {
                    foreach (var file in files)
                    {
                        if (file != null && file.ContentLength > 0)
                        {

                            ABB_Company_Doc_folder_file tbl = new ABB_Company_Doc_folder_file();
                            tbl.OrgId = Login.OrganizationId;
                            tbl.FolderId = Convert.ToInt64(c["txtfolder"]);
                            tbl.Name = c["txtfilename"];
                            tbl.filename = c["txtfilename"];
                            tbl.FileType = Path.GetExtension(file.FileName);
                            tbl.IconClass = getfileiconclass(Path.GetExtension(file.FileName));
                            tbl.CreateDate = tbl.ModifiedDate = DateTime.Now;
                            tbl.ModifiedBy = Login.Id;
                            tbl.MarkASDelete = false;
                            dc.ABB_Company_Doc_folder_file.Add(tbl);
                            dc.SaveChanges();
                            file.SaveAs(Path.Combine(Server.MapPath("~/assets/Programmer/CompanyDocs/" + c["txtcompany"] + "/" + c["txtfolder"] + "/"), tbl.Id + Path.GetExtension(file.FileName)));
                        }
                    }
                    Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                   this.ControllerContext.RouteData.Values["action"].ToString(),
                   "The user " + Login.FirstName + " " + Login.LastName + "Added new Company Document Folder File");
                    return RedirectToAction("Uploadfilestofolder", "BB_Company_Doc_folder", new { company = c["txtcompany"], fid = c["txtfolder"] });
                }
                else
                {
                    return RedirectToAction("Uploadfilestofolder");
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
            return RedirectToAction("Uploadfilestofolder");
        }
        #endregion

        #region List of Files in folder
        [LoginAuthorize]
        public ActionResult listoffiles(string sortOrder, string currentFilter, string searchString, int? page, string company, string fid)
        {
            try
            {
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                  this.ControllerContext.RouteData.Values["action"].ToString(),
                  "The user " + Login.FirstName + " " + Login.LastName + "Entered into  Company Document Folder File List Page");
                FrameworkEntities db = new FrameworkEntities();
                var list = new List<ABB_Company_Doc_folder_file>();
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return View(db.ABB_Company_Doc_folder_file.AsEnumerable().Where(a => a.OrgId == Login.OrganizationId &&
                    a.MarkASDelete == false && a.FolderId == Convert.ToInt64(fid)).ToPagedList(pageNumber, pageSize));
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
            List<ABB_Company_Doc_folder_file> lst = new List<ABB_Company_Doc_folder_file>();
            return View(lst.ToPagedList(pageNumberone, 10));
        }
        #endregion


        #region   get extension class
        [LoginAuthorize]
        public string getfileiconclass(string extn)
        {
            try
            {
                switch (extn.ToLower())
                {
                    case ".doc":
                    case ".docx":
                        return "fa fa-file-word-o";

                    case ".xls":
                    case ".xlsx":
                    case ".csv":
                        return "fa fa-file-excel-o";
                    case ".pdf":
                        return "fa fa-file-pdf-o";
                    case ".png":
                    case ".bmp":
                    case ".gif":
                    case ".jpeg":
                    case ".jpg":
                        return "fa fa-file-image-o";
                    case ".ppt":
                    case ".pptx":
                        return "fa fa-file-powerpoint-o";
                    case ".psd":
                        return "fa  fa-life-bouy";
                    case ".zip":
                    case ".rar":
                        return "fa fa-file-zip-o";
                    default:
                        return "fa  fa-paw";

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
            return "";
        }

        #endregion
    }
}
