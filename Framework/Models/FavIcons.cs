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
    
    public partial class FavIcons
    {
        public FavIcons()
        {
            this.ABB_ActivityTypes = new HashSet<ABB_ActivityTypes>();
            this.ABB_InteractionType = new HashSet<ABB_InteractionType>();
            this.ActivityType = new HashSet<ActivityType>();
        }
    
        public int Id { get; set; }
        public System.Guid OrgId { get; set; }
        public string Iconclassname { get; set; }
        public string Name { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public bool MarkAsDelete { get; set; }
    
        public virtual ICollection<ABB_ActivityTypes> ABB_ActivityTypes { get; set; }
        public virtual ICollection<ABB_InteractionType> ABB_InteractionType { get; set; }
        public virtual ICollection<ActivityType> ActivityType { get; set; }
        public virtual Organizations Organizations { get; set; }
        public virtual Users Users { get; set; }
    }
}
