using Core;
using FramWork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;
using System.IO;
using ExceptionLogging;

namespace FramWork.Controllers
{
    public class GoalDetailsController : Controller
    {

        #region Index(List)
        [LoginAuthorize]
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            try
            {
                Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
            this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Entered into Goal Configuration List Page.");
                FrameworkEntities dc = new FrameworkEntities();
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return View(dc.TGoal_Goal.AsEnumerable().Where(a => a.OrgId == Login.OrganizationId && a.MarkAsDelete == false).ToPagedList(pageNumber, pageSize));
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
            List<TGoal_Goal> lst = new List<TGoal_Goal>();
            return View(lst.ToPagedList(pageNumberone, 10));
        }
        #endregion

        #region Goal Rule Configuration
        [LoginAuthorize]
        public ActionResult GoalRuleConfiguration()
        {
            Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
           this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Entered into Goal  Rule Configuration Page.");
            return View();
        }
        #endregion

        #region Goal Rule Configuration
        // GET: GoalDetails       
        [LoginAuthorize]
        [HttpPost]
        public bool SaveGoalRuleconfig(string GoalId, string Period,
            List<string> products, List<String> Gorgs, List<String> Teamlst, List<string> agentlist)
        {
            try
            {
                if (!string.IsNullOrEmpty(GoalId) && !string.IsNullOrEmpty(Period))
                {
                    FrameworkEntities dc = new FrameworkEntities();
                    TGoal_GoalRuleConfiguration tb = new TGoal_GoalRuleConfiguration();
                    tb.OrgId = Login.OrganizationId;
                    tb.GoalId = Convert.ToInt32(GoalId);
                    tb.FinancialPeriodId = Convert.ToInt32(Period);
                    tb.CreateDate = tb.ModifiedDate = DateTime.Now;
                    tb.ModifiedBy = Login.Id;
                    tb.MarkAsDelete = false;
                    dc.TGoal_GoalRuleConfiguration.Add(tb);
                    dc.SaveChanges();
                    foreach (var prod in products)
                    {
                        TGoal_GoalProductDetails tbl = new TGoal_GoalProductDetails();
                        tbl.OrgId = tb.OrgId;
                        tbl.GoalRuleConfigId = tb.Id;
                        tbl.ProductId = new Guid(prod);
                        tbl.CreateDate = tbl.ModifiedDate = tb.CreateDate;
                        tbl.ModifiedBy = tb.ModifiedBy;
                        tbl.MarkAsDelete = false;
                        dc.TGoal_GoalProductDetails.Add(tbl);
                        dc.SaveChanges();
                    }
                    if (Convert.ToInt32(GoalType.Organisation_Level_Goal) == Convert.ToInt32(GoalId))
                    {

                        foreach (var item in Gorgs)
                        {
                            TGoal_GoalOrganisationDetails orgtbl = new TGoal_GoalOrganisationDetails();
                            orgtbl.OrgId = tb.OrgId;
                            orgtbl.GoalRuleConfigId = tb.Id;
                            orgtbl.GoalOrgId = Convert.ToInt32(item);
                            orgtbl.CreateDate = orgtbl.ModifiedDate = tb.CreateDate;
                            orgtbl.ModifiedBy = tb.ModifiedBy;
                            orgtbl.MarkAsDelete = false;
                            dc.TGoal_GoalOrganisationDetails.Add(orgtbl);
                            Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
          this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Added Organization Goal");
                            dc.SaveChanges();
                        }

                    }
                    else if (Convert.ToInt32(GoalId) == Convert.ToInt32(GoalType.Sales_Team_Level_Goal))
                    {
                        foreach (var item in Teamlst)
                        {
                            TGoal_GoalTeamDetails orgtbl = new TGoal_GoalTeamDetails();
                            orgtbl.OrgId = tb.OrgId;
                            orgtbl.GoalRuleConfigId = tb.Id;
                            orgtbl.TeamId = new Guid(item);
                            orgtbl.CreateDate = orgtbl.ModifiedDate = tb.CreateDate;
                            orgtbl.ModifiedBy = tb.ModifiedBy;
                            orgtbl.MarkAsDelete = false;
                            dc.TGoal_GoalTeamDetails.Add(orgtbl);
                            Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                            this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Added Sales Team Level Goal.");
                            dc.SaveChanges();
                        }
                    }
                    else if (Convert.ToInt32(GoalId) == Convert.ToInt32(GoalType.Agent_Level_Goal))
                    {
                        foreach (var item in agentlist)
                        {
                            TGoal_GoalAgentDetails orgtbl = new TGoal_GoalAgentDetails();
                            orgtbl.OrgId = tb.OrgId;
                            orgtbl.GoalRuleConfigId = tb.Id;
                            orgtbl.AgentId = Convert.ToInt32(item);
                            orgtbl.CreateDate = orgtbl.ModifiedDate = tb.CreateDate;
                            orgtbl.ModifiedBy = tb.ModifiedBy;
                            orgtbl.MarkAsDelete = false;
                            dc.TGoal_GoalAgentDetails.Add(orgtbl);
                            Core.Basic.TimeLineDescription(this.ControllerContext.RouteData.Values["controller"].ToString(),
                              this.ControllerContext.RouteData.Values["action"].ToString(), "The user " + Login.FirstName + " " + Login.LastName + "Added Agent Level Goal.");
                            dc.SaveChanges();
                        }
                    }

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
    }
}