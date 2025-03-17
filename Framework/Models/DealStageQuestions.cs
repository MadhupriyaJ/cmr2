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
    
    public partial class DealStageQuestions
    {
        public DealStageQuestions()
        {
            this.DealStageAnswers = new HashSet<DealStageAnswers>();
            this.DealStageQuestionValues = new HashSet<DealStageQuestionValues>();
        }
    
        public System.Guid Id { get; set; }
        public System.Guid OrgId { get; set; }
        public Nullable<System.Guid> DealStageNameId { get; set; }
        public string Title { get; set; }
        public string Question { get; set; }
        public System.Guid ControlTypeId { get; set; }
        public string ControlId { get; set; }
        public string ControlName { get; set; }
        public bool Ismandatory { get; set; }
        public int SortOrder { get; set; }
        public Nullable<System.Guid> QuestionTypeId { get; set; }
        public System.Guid DealstageQuestionModeId { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.DateTime ModifiedDate { get; set; }
        public int ModifiedBy { get; set; }
        public bool MarkAsDelete { get; set; }
    
        public virtual ICollection<DealStageAnswers> DealStageAnswers { get; set; }
        public virtual DealStageNames DealStageNames { get; set; }
        public virtual DealStageQuestionModes DealStageQuestionModes { get; set; }
        public virtual DealStageQuestionTypes DealStageQuestionTypes { get; set; }
        public virtual HtmlControlNames HtmlControlNames { get; set; }
        public virtual Organizations Organizations { get; set; }
        public virtual Users Users { get; set; }
        public virtual ICollection<DealStageQuestionValues> DealStageQuestionValues { get; set; }
    }
}
