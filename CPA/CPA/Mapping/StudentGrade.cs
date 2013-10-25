using System;
using System.Collections.Generic;

namespace CPA.Models
{
    [Serializable]
    public partial class StudentGrade
    {
        public int EnrollmentID { get; set; }
        public int CourseID { get; set; }
        public int StudentID { get; set; }
        public Nullable<decimal> Grade { get; set; }
        public virtual Course Course { get; set; }
        public virtual Person Person { get; set; }
    }
}
