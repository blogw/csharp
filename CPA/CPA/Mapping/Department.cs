using System;
using System.Collections.Generic;

namespace CPA.Models
{
    [Serializable]
    public partial class Department
    {
        public Department()
        {
            this.Courses = new List<Course>();
        }

        public int DepartmentID { get; set; }
        public string Name { get; set; }
        public decimal Budget { get; set; }
        public System.DateTime StartDate { get; set; }
        public int Administrator { get; set; }
        public virtual ICollection<Course> Courses { get; set; }

        public override bool Equals(object obj)
        {
            if (null != obj)
            {
                bool result = obj.GetType().IsAssignableFrom(this.GetType());
                if (result && this.DepartmentID == ((Department)obj).DepartmentID)
                {
                    return true;
                }

            }
            return false;
        }

        public override int GetHashCode()
        {
            return this.DepartmentID.GetHashCode();
        }
    }
}
