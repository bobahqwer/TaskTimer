//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TaskTimer.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class EfarganPagesParagraphs
    {
        public int Id { get; set; }
        public string ParagraphTitle { get; set; }
        public string ParagraphText { get; set; }
        public string ParagraphLinkText { get; set; }
        public string ParagraphLinkURL { get; set; }
        public string ParagraphImageURL { get; set; }
        public string ParagraphImageFileName { get; set; }
        public Nullable<int> ParagraphEfarganPagesId { get; set; }
        public Nullable<int> ParagraphEfarganPagesLanguageId { get; set; }
    
        public virtual EfarganPages EfarganPages { get; set; }
        public virtual EfarganPagesLanguage EfarganPagesLanguage { get; set; }
    }
}
