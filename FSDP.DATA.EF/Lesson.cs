//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FSDP.DATA.EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class Lesson
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Lesson()
        {
            this.LessonViews = new HashSet<LessonView>();
        }
    
        public int LessonID { get; set; }
        public string LessonName { get; set; }
        public int CourseID { get; set; }
        public string Introduction { get; set; }
        public string VideoURL { get; set; }
        public string PDFfilename { get; set; }
        public bool IsActive { get; set; }
    
        public virtual Cours Cours { get; set; }
        public virtual Cours Cours1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LessonView> LessonViews { get; set; }
    }
}
