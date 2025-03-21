//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FramWork.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Employees
    {
        public Employees()
        {
            this.Campaigns = new HashSet<Campaigns>();
            this.CaseManagment_Followers = new HashSet<CaseManagment_Followers>();
            this.CaseManagment_Note_Followers = new HashSet<CaseManagment_Note_Followers>();
            this.CaseManagment_Task = new HashSet<CaseManagment_Task>();
            this.CaseManagments = new HashSet<CaseManagments>();
            this.EmployeeQueue = new HashSet<EmployeeQueue>();
            this.Employees1 = new HashSet<Employees>();
            this.PersonLeads = new HashSet<PersonLeads>();
        }
    
        public System.Guid Id { get; set; }
        public System.Guid OrgId { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<System.Guid> DepartmentId { get; set; }
        public Nullable<System.Guid> DesignationId { get; set; }
        public Nullable<System.Guid> CountryId { get; set; }
        public Nullable<System.Guid> State_province_county_Id { get; set; }
        public Nullable<System.Guid> CityId { get; set; }
        public string Street { get; set; }
        public Nullable<System.Guid> LocationId { get; set; }
        public string ZipPostalcode { get; set; }
        public Nullable<System.Guid> LanguageId { get; set; }
        public string ReportSto { get; set; }
        public Nullable<System.Guid> ParentEmployee { get; set; }
        public Nullable<int> SalesAfffliateId { get; set; }
        public Nullable<System.Guid> TeamId { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public bool MarkAsDelete { get; set; }
    
        public virtual ICollection<Campaigns> Campaigns { get; set; }
        public virtual ICollection<CaseManagment_Followers> CaseManagment_Followers { get; set; }
        public virtual ICollection<CaseManagment_Note_Followers> CaseManagment_Note_Followers { get; set; }
        public virtual ICollection<CaseManagment_Task> CaseManagment_Task { get; set; }
        public virtual ICollection<CaseManagments> CaseManagments { get; set; }
        public virtual Cities Cities { get; set; }
        public virtual Countries Countries { get; set; }
        public virtual Departments Departments { get; set; }
        public virtual Designations Designations { get; set; }
        public virtual ICollection<EmployeeQueue> EmployeeQueue { get; set; }
        public virtual ICollection<Employees> Employees1 { get; set; }
        public virtual Employees Employees2 { get; set; }
        public virtual Languages Languages { get; set; }
        public virtual Locations Locations { get; set; }
        public virtual Organizations Organizations { get; set; }
        public virtual State_Provience_County State_Provience_County { get; set; }
        public virtual Teams Teams { get; set; }
        public virtual Users Users { get; set; }
        public virtual Users Users1 { get; set; }
        public virtual ICollection<PersonLeads> PersonLeads { get; set; }
    }
}
