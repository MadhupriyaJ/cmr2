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
    
    public partial class ABB_BusinessSector
    {
        public ABB_BusinessSector()
        {
            this.ABB_Company = new HashSet<ABB_Company>();
        }
    
        public int Id { get; set; }
        public System.Guid OrgId { get; set; }
        public string Sector { get; set; }
        public string Description { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public bool MarkAsDelete { get; set; }
    
        public virtual Organizations Organizations { get; set; }
        public virtual Users Users { get; set; }
        public virtual ICollection<ABB_Company> ABB_Company { get; set; }
    }
}
