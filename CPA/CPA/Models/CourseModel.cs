using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPA.Models
{
    public class CourseModel : BaseModel<Course>
    {
        private static CourseModel Instance;

        private CourseModel()
        {
            this.Source = null;
            this.OpenBinarytoMemory();

            DepartmentModel dm = DepartmentModel.Load();
            foreach (Course c in this)
            {
                c.Department = dm.GetByDepartmentID(c.DepartmentID);
            }
        }

        public static CourseModel Load()
        {
            return Instance ?? (Instance = new CourseModel());
        }         

        public override Course Exists(Course t)
        {
            IEnumerator<Course> tsiShopList = this.GetEnumerator();
            {
                while (tsiShopList.MoveNext())
                {
                    // キーがファイルに存在の場合
                    if (tsiShopList.Current.Equals(t))
                    {
                        return tsiShopList.Current;
                    }
                }
            }

            return default(Course);
        }

        public static void StaticDispose()
        {
            if (Instance != null)
            {
                Instance.Dispose();
                Instance = null;
            }
        }
    }
}
