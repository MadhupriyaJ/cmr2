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
    
    public partial class TGoal_GoalProductDetails
    {
        public int Id { get; set; }
        public System.Guid OrgId { get; set; }
        public int GoalRuleConfigId { get; set; }
        public System.Guid ProductId { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public int ModifiedBy { get; set; }
        public bool MarkAsDelete { get; set; }
    
        public virtual CampaignProducts CampaignProducts { get; set; }
        public virtual Organizations Organizations { get; set; }
        public virtual TGoal_GoalRuleConfiguration TGoal_GoalRuleConfiguration { get; set; }
        public virtual Users Users { get; set; }
    }
}
