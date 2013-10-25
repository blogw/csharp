using System;
using System.Collections.Generic;

namespace CPA.Models
{
    [Serializable]
    public partial class OnlineCourse
    {
        public int CourseID { get; set; }
        public string URL { get; set; }
        public virtual Course Course { get; set; }
    }
}
