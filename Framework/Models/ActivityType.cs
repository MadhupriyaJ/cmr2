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
    
    public partial class ActivityType
    {
        public ActivityType()
        {
            this.CompanyCustomerAppoinment = new HashSet<CompanyCustomerAppoinment>();
        }
    
        public int Id { get; set; }
        public Nullable<System.Guid> OrgId { get; set; }
        public string ActivityName { get; set; }
        public Nullable<int> iconclass { get; set; }
        public Nullable<bool> Status { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public string Color { get; set; }
    
        public virtual FavIcons FavIcons { get; set; }
        public virtual Organizations Organizations { get; set; }
        public virtual ICollection<CompanyCustomerAppoinment> CompanyCustomerAppoinment { get; set; }
    }
}
