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
    
    public partial class Cities
    {
        public Cities()
        {
            this.Employees = new HashSet<Employees>();
            this.Person_Company = new HashSet<Person_Company>();
            this.People = new HashSet<People>();
        }
    
        public System.Guid Id { get; set; }
        public System.Guid OrgId { get; set; }
        public System.Guid State_provience_county_ID { get; set; }
        public string Name { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public int ModifiedBy { get; set; }
        public bool MarkAsDelete { get; set; }
    
        public virtual Cities Cities1 { get; set; }
        public virtual Cities Cities2 { get; set; }
        public virtual Organizations Organizations { get; set; }
        public virtual State_Provience_County State_Provience_County { get; set; }
        public virtual Users Users { get; set; }
        public virtual ICollection<Employees> Employees { get; set; }
        public virtual ICollection<Person_Company> Person_Company { get; set; }
        public virtual ICollection<People> People { get; set; }
    }
}
