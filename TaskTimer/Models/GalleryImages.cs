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
    
    public partial class GalleryImages
    {
        public int Id { get; set; }
        public Nullable<int> GalleryId { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Alt { get; set; }
        public string BottomText { get; set; }
    
        public virtual GalleryPages GalleryPages { get; set; }
    }
}
