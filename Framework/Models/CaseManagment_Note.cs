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
    
    public partial class CaseManagment_Note
    {
        public CaseManagment_Note()
        {
            this.CaseManagment_Note_Attachment = new HashSet<CaseManagment_Note_Attachment>();
            this.CaseManagment_Note_Comment = new HashSet<CaseManagment_Note_Comment>();
            this.CaseManagment_Note_Followers = new HashSet<CaseManagment_Note_Followers>();
        }
    
        public System.Guid Id { get; set; }
        public System.Guid OrgId { get; set; }
        public System.Guid CaseId { get; set; }
        public string CaseNote { get; set; }
        public Nullable<System.DateTime> when_it_happened { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public int ModifiedBy { get; set; }
        public bool MarkAsDelete { get; set; }
    
        public virtual ICollection<CaseManagment_Note_Attachment> CaseManagment_Note_Attachment { get; set; }
        public virtual CaseManagments CaseManagments { get; set; }
        public virtual ICollection<CaseManagment_Note_Comment> CaseManagment_Note_Comment { get; set; }
        public virtual ICollection<CaseManagment_Note_Followers> CaseManagment_Note_Followers { get; set; }
        public virtual Organizations Organizations { get; set; }
        public virtual Users Users { get; set; }
    }
}
