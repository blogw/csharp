using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPA.Models
{
    class DepartmentModel : BaseModel<Department>
    {
        private static DepartmentModel Instance;

        private DepartmentModel()
        {
            this.OpenBinarytoMemory();
        }

        public static DepartmentModel Load()
        {
            return Instance ?? (Instance = new DepartmentModel());
        }

        public Department GetByDepartmentID(int id)
        {
            IEnumerable<Department> result = from d in this.Source
                                             where d.DepartmentID == id
                                             select d;
            return result.FirstOrDefault();
        }

        public void Delete(Department d)
        {
            this.Source.Remove(d);
        }

        public override Department Exists(Department t)
        {
            IEnumerator<Department> tsiShopList = this.GetEnumerator();
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

            return default(Department);
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
