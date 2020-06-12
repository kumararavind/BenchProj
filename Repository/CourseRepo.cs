using LMSProfile.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMSProfile.Repository
{
    public class CourseRepo
    {
        private SqlConnection con;
        private SqlCommand com;
        //To Handle connection related activities    
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString.ToString();
            con = new SqlConnection(constr);
            com = new SqlCommand("SP_CRUD_Course", con);
            com.CommandType = CommandType.StoredProcedure;

        }

        public bool AddCourse(CourseModel obj,FormCollection form)
        {

            connection();
            string catId = Convert.ToString(form["CategoryList"]);
            com.Parameters.AddWithValue("@course_name", obj.courseName);
            com.Parameters.AddWithValue("@course_length_hrs", obj.courseLength);
            com.Parameters.AddWithValue("@course_tech", obj.courseTech);
            com.Parameters.AddWithValue("@course_desc", obj.courseDesc);
            com.Parameters.AddWithValue("@course_startdate", obj.courseStartDate);
            com.Parameters.AddWithValue("@course_enddate", obj.courseEndDate);
            com.Parameters.AddWithValue("@catid", catId);
            com.Parameters.AddWithValue("@userid", obj.courseInstructorId);
            com.Parameters.AddWithValue("@status", "Create");
            con.Open();
            int i = com.ExecuteNonQuery();
            con.Close();
            if (i < 0)
            {

                return true;

            }
            else
            {

                return false;
            }


        }
        //To view category details with generic list     
        public List<CourseModel> GetAllCourse()
        {
            connection();
            List<CourseModel> CouList = new List<CourseModel>();
            com.Parameters.AddWithValue("@status", "Read");
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();

            con.Open();
            com.ExecuteNonQuery();
            da.Fill(dt);
            con.Close();
            //Bind EmpModel generic list using dataRow     
            foreach (DataRow dr in dt.Rows)
            {

                CouList.Add(
                    new CourseModel
                    {
                        courseId = Convert.ToInt32(dr["course_id"]),
                        courseName = Convert.ToString(dr["course_name"]),
                        courseLength = Convert.ToInt32(dr["course_length_hrs"]),
                        catId=Convert.ToInt32(dr["course_category_id"]),
                        courseTech = Convert.ToString(dr["course_tech"]),
                        courseDesc = Convert.ToString(dr["course_desc"]),
                        courseStartDate = Convert.ToDateTime(dr["course_startdate"]),
                        courseEndDate = Convert.ToDateTime(dr["course_enddate"]),
                        courseInstructorId = Convert.ToInt32(dr["course_instructor_id"])
                    }
                    );
            }

            return CouList;
        }
        //To Update Employee details    
        public bool UpdateCourse(CourseModel obj)
        {

            connection();
            com.Parameters.AddWithValue("@course_id", obj.courseId);
            com.Parameters.AddWithValue("@course_name", obj.courseName);
            com.Parameters.AddWithValue("@course_length_hrs", obj.courseLength);
            com.Parameters.AddWithValue("@course_desc", obj.courseDesc);
            com.Parameters.AddWithValue("@course_startdate", obj.courseStartDate);
            com.Parameters.AddWithValue("@course_enddate", obj.courseEndDate);
            com.Parameters.AddWithValue("@status", "Update");
            con.Open();
            int i = com.ExecuteNonQuery();
            con.Close();
            if (i <0)
            {

                return true;

            }
            else
            {

                return false;
            }


        } 
        public bool DeleteCourse(int Id)
        {

            connection();
            com.Parameters.AddWithValue("@course_id", Id);
            com.Parameters.AddWithValue("@status", "Delete");

            con.Open();
            int i = com.ExecuteNonQuery();
            con.Close();
            if (i >= 1)
            {

                return true;

            }
            else
            {

                return false;
            }


        }
    }
}