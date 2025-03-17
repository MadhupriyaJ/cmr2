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
    
    public partial class Commision_ProductCommision
    {
        public int Id { get; set; }
        public System.Guid OrgId { get; set; }
        public Nullable<System.Guid> ProductId { get; set; }
        public Nullable<System.Guid> TeamId { get; set; }
        public System.DateTime FromDate { get; set; }
        public System.DateTime ToDate { get; set; }
        public decimal MinValue { get; set; }
        public decimal FromValue { get; set; }
        public decimal ToValue { get; set; }
        public bool ToInfinity { get; set; }
        public Nullable<int> CommisionTypeId { get; set; }
        public decimal Commision { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public bool MarkAsDelete { get; set; }
    
        public virtual CampaignProducts CampaignProducts { get; set; }
        public virtual Commision_CommisionType Commision_CommisionType { get; set; }
        public virtual Organizations Organizations { get; set; }
        public virtual Teams Teams { get; set; }
        public virtual Users Users { get; set; }
    }
}
