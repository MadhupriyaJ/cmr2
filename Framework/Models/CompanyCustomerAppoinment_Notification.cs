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
    
    public partial class CompanyCustomerAppoinment_Notification
    {
        public int Id { get; set; }
        public int OrgId { get; set; }
        public int AppoinmentId { get; set; }
        public int NotificationTypeId { get; set; }
        public int Timeperiod { get; set; }
        public int TimeTypeID { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public int ModifiedBy { get; set; }
    
        public virtual CompanyCustomerAppoinment CompanyCustomerAppoinment { get; set; }
        public virtual CompanyCustomerAppoinment_NotificationType CompanyCustomerAppoinment_NotificationType { get; set; }
        public virtual CompanyCustomerAppoinment_TimeType CompanyCustomerAppoinment_TimeType { get; set; }
    }
}
