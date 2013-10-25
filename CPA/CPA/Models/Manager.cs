using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPA.Models
{
    public class Manager
    {
        public void setup()
        {
            if (File.Exists("Course.bin"))
            {
                File.Delete("Course.bin");
            }

            if (File.Exists("Department.bin"))
            {
                File.Delete("Department.bin");
            }

            Merge();
        }

        public void Merge()
        {
            DataSet ds = select();

            CourseModel cm = CourseModel.Load();
            cm.DataMerge(ds.Tables["Course"]);

            DepartmentModel dm = DepartmentModel.Load();
            dm.DataMerge(ds.Tables["Department"]);

            CourseModel.StaticDispose();
            DepartmentModel.StaticDispose();
        }

        private DataSet select()
        {
            // データベースの接続
            SqlConnection dbConnection = new SqlConnection("Data Source=(localdb)\\Projects;Database=School; Integrated Security=True");
            dbConnection.Open();
            DataSet ds = new DataSet();

            using (var dbCommand = new SqlCommand("test", dbConnection))
            {
                // 配置命令情報
                dbCommand.CommandType = CommandType.StoredProcedure;

                // マスタデータ取得
                using (var dataAdapter = new SqlDataAdapter(dbCommand))
                {
                    dbCommand.ExecuteNonQuery();

                    dataAdapter.Fill(ds);
                    ds.Tables[0].TableName = "Course";
                    ds.Tables[1].TableName = "CourseInstructor";
                    ds.Tables[2].TableName = "Department";
                    ds.Tables[3].TableName = "OfficeAssignment";
                    ds.Tables[4].TableName = "OnlineCourse";
                    ds.Tables[5].TableName = "OnsiteCourse";
                    ds.Tables[6].TableName = "Person";
                    ds.Tables[7].TableName = "StudentGrade";

                }
            }

            // 接続を閉じる
            dbConnection.Close();

            return ds;
        }
    }
}
