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
    
    public partial class CampaignProducts
    {
        public CampaignProducts()
        {
            this.CampaignProductDocuments = new HashSet<CampaignProductDocuments>();
            this.Commision_ProductCommision = new HashSet<Commision_ProductCommision>();
            this.Commision_SalesAgentRules = new HashSet<Commision_SalesAgentRules>();
            this.PersonLeadProducts = new HashSet<PersonLeadProducts>();
            this.TGoal_GoalProductDetails = new HashSet<TGoal_GoalProductDetails>();
        }
    
        public System.Guid Id { get; set; }
        public System.Guid OrgId { get; set; }
        public System.Guid CampaignId { get; set; }
        public string ProductName { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public int ModifiedBy { get; set; }
        public bool MarkAsDelete { get; set; }
    
        public virtual ICollection<CampaignProductDocuments> CampaignProductDocuments { get; set; }
        public virtual Campaigns Campaigns { get; set; }
        public virtual Organizations Organizations { get; set; }
        public virtual Users Users { get; set; }
        public virtual ICollection<Commision_ProductCommision> Commision_ProductCommision { get; set; }
        public virtual ICollection<Commision_SalesAgentRules> Commision_SalesAgentRules { get; set; }
        public virtual ICollection<PersonLeadProducts> PersonLeadProducts { get; set; }
        public virtual ICollection<TGoal_GoalProductDetails> TGoal_GoalProductDetails { get; set; }
    }
}
