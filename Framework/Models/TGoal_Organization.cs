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
    
    public partial class TGoal_Organization
    {
        public TGoal_Organization()
        {
            this.TGoal_GoalOrganisationDetails = new HashSet<TGoal_GoalOrganisationDetails>();
        }
    
        public int Id { get; set; }
        public System.Guid OrgId { get; set; }
        public string Name { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public int ModifiedBy { get; set; }
        public bool MarkAsDelete { get; set; }
    
        public virtual Organizations Organizations { get; set; }
        public virtual ICollection<TGoal_GoalOrganisationDetails> TGoal_GoalOrganisationDetails { get; set; }
        public virtual Users Users { get; set; }
    }
}
