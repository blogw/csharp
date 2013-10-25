using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPA.Models;
using System.IO;

namespace CPA
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (Course c in CourseModel.Load())
            {
                Console.WriteLine(c.CourseID+","+c.Department.Name+","+c.Department.DepartmentID);
            }

            Console.WriteLine("=========");

            Manager manager = new Manager();
            manager.Merge();

            foreach (Course c in CourseModel.Load())
            {
                Console.WriteLine(c.CourseID + "," + c.Department.Name + "," + c.Department.DepartmentID);
            }

            Console.ReadLine();
        }
        
    }
}
