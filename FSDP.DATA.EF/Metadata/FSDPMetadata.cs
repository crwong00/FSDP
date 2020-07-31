using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FSDP.DATA.EF //Metadata
{
    #region Course
    public class CourseMetaData
    {
        [Display(Name ="Course")]
        [Required(ErrorMessage ="Course name required.")]
        public string CourseName { get; set; }

        [Display(Name ="Description")]
        public string CourseDescription { get; set; }

        [Display(Name = "Offered at this time?")]
        [Required(ErrorMessage ="** is it currently offered? **")]
        public bool isActive { get; set; }
        
        [Display(Name ="Photo")]
        public string Photo { get; set; }
    }
    [MetadataType(typeof(CourseMetaData))]
    public partial class Course { }
    #endregion

    #region completedcourse
    public class CourseCompletionMetaData
    {
        [Display(Name = "Date Completed ")]
        [Required(ErrorMessage ="Must post when completed")]
        public System.DateTime DateCompleted { get; set; }
    }
    [MetadataType(typeof(CourseMetaData))]
    public partial class CourseCompletion { }
    #endregion

    #region Lesson
    public class LessonMetaData
    {
        [Display(Name = "Class")]
        [Required(ErrorMessage ="**Class name required**")]
        public string LessonName { get; set; }
        
        [Display(Name ="intro")]
        [Required(ErrorMessage ="** Must have a short introduction to class **")]
        public string Introduction { get; set; }

        [Display(Name ="Video")]
        public string VideoURL { get; set; }

        [Display(Name ="file upload")]
        public string PDFfilename { get; set; }

        [Display(Name ="currently offered?")]
        public bool IsActive { get; set; }
    }

    [MetadataType(typeof(LessonMetaData))]
    public partial class Lesson { }
    #endregion

    #region Lesson View
    public class LessonViewMetaData
    {
        [Display(Name ="viewed")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public System.DateTime DateViewed { get; set; }
    }
    [MetadataType(typeof(LessonViewMetaData))]
    public partial class LessonView {}
    #endregion

    #region User details view
    [MetadataType(typeof(UserDetailsMetaData))]
    public partial class UserDetail
    {
        [Display(Name ="User")]
        public string FullName
        {
            get { return FirstName + " " + LastName; }     
        }

    }

    public class UserDetailsMetaData
    {
        [Display(Name ="First Name")]
        [Required(ErrorMessage ="** First name is required **")]
        public string FirstName { get; set; }

        [Display(Name ="Last Name")]
        [Required(ErrorMessage = "** Last Name is required **")]
        public string LastName { get; set; }

    }

    #endregion

}
