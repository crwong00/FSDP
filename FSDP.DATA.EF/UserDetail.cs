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
    
    public partial class UserDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserDetail()
        {
            this.LessonViews = new HashSet<LessonView>();
        }
    
        public string UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Nullable<int> CourseCompletetionID { get; set; }
    
        public virtual CourseCompletion CourseCompletion { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LessonView> LessonViews { get; set; }
    }
}
