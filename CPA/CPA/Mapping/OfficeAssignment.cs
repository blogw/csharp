using System;
using System.Collections.Generic;

namespace CPA.Models
{
    [Serializable]
    public partial class OfficeAssignment
    {
        public int InstructorID { get; set; }
        public string Location { get; set; }
        public byte[] Timestamp { get; set; }
        public virtual Person Person { get; set; }
    }
}
