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
    
    public partial class TGoal_MetricType
    {
        public TGoal_MetricType()
        {
            this.TGoal_Goal = new HashSet<TGoal_Goal>();
        }
    
        public int Id { get; set; }
        public System.Guid OrgId { get; set; }
        public string MetricType { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public int ModifiedBy { get; set; }
        public bool MarkAsdelete { get; set; }
    
        public virtual Organizations Organizations { get; set; }
        public virtual ICollection<TGoal_Goal> TGoal_Goal { get; set; }
        public virtual Users Users { get; set; }
    }
}
