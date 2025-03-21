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
    
    public partial class People
    {
        public People()
        {
            this.PersonLeads = new HashSet<PersonLeads>();
        }
    
        public System.Guid Id { get; set; }
        public System.Guid OrgId { get; set; }
        public string RPnumber { get; set; }
        public string Passport { get; set; }
        public System.Guid PersonModeId { get; set; }
        public System.Guid SalutationId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public Nullable<System.Guid> GenderId { get; set; }
        public string NickName { get; set; }
        public Nullable<System.DateTime> Dob { get; set; }
        public string Mobile { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public Nullable<System.Guid> Nationality { get; set; }
        public System.Guid LanguageId { get; set; }
        public Nullable<System.Guid> Country { get; set; }
        public Nullable<System.Guid> State_province { get; set; }
        public Nullable<System.Guid> City { get; set; }
        public string Street { get; set; }
        public string LocationLatitude { get; set; }
        public string Locationlongitude { get; set; }
        public string ZipPostalcode { get; set; }
        public Nullable<int> Occupation { get; set; }
        public Nullable<System.Guid> Company { get; set; }
        public string Profilepicture { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public bool MarkAsDelete { get; set; }
    
        public virtual Cities Cities { get; set; }
        public virtual Countries Countries { get; set; }
        public virtual Countries Countries1 { get; set; }
        public virtual Genders Genders { get; set; }
        public virtual Languages Languages { get; set; }
        public virtual Occupation Occupation1 { get; set; }
        public virtual Organizations Organizations { get; set; }
        public virtual Person_Company Person_Company { get; set; }
        public virtual Person_Mode Person_Mode { get; set; }
        public virtual Salutations Salutations { get; set; }
        public virtual State_Provience_County State_Provience_County { get; set; }
        public virtual Users Users { get; set; }
        public virtual ICollection<PersonLeads> PersonLeads { get; set; }
    }
}
