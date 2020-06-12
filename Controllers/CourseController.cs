using LMSProfile.Models;
using LMSProfile.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMSProfile.Controllers
{
    public class CourseController : Controller
    {
        private CourseModel model;
        private void dropdown()
        {
            if (Session["UserId"] != null && Session["Accountid"] != null)
            {
                string constr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString.ToString();
                SqlConnection con = new SqlConnection(constr);
                model = new CourseModel();
                con.Open();
                using (SqlCommand cmd1 = new SqlCommand("SELECT cat_id,cat_name FROM Category_Details", con))
                {

                    DataSet ds = new DataSet();
                    SqlDataAdapter da = new SqlDataAdapter(cmd1);
                    da.Fill(ds);
                    List<CourseModel> category = new List<CourseModel>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        CourseModel uobj = new CourseModel();
                        uobj.catId = Convert.ToInt32(ds.Tables[0].Rows[i]["cat_id"].ToString());
                        uobj.categoryName = ds.Tables[0].Rows[i]["cat_name"].ToString();
                        category.Add(uobj);
                    }
                    model.CategoryList = category;
                }
                con.Close();
            }
            else
            {
                ViewBag.text = "Error While Loading Drodown";
            }
            
        }
        public ActionResult GetAllCourse()
        {
            if (Session["UserId"] != null && Session["Accountid"] != null)
            {
                CourseRepo Repo = new CourseRepo();
                ModelState.Clear();
                return View(Repo.GetAllCourse());
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
            
        }
        public ActionResult AddCourse()
        {
            if (Session["UserId"] != null && Session["Accountid"] != null)
            {
                dropdown();
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
            
        }

        [HttpPost]
        public ActionResult AddCourse(CourseModel model,FormCollection form)
        {
            try
            {
                CourseRepo CouRepo = new CourseRepo();

                if (CouRepo.AddCourse(model,form))
                {
                    ViewBag.Message = "Course details added successfully";
                }
                
                return RedirectToAction("Addcourse",model);
            }
            catch
            {
                ViewBag.Message = "Course details Addition Failed";
                return View();
            }
        }
            
        public ActionResult EditCouDetails(int id)
        {
            if (Session["UserId"] != null && Session["Accountid"] != null)
            {
                CourseRepo CouRepo = new CourseRepo();
                return View(CouRepo.GetAllCourse().Find(Cou => Cou.courseId == id));
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }
   
        [HttpPost]
        public ActionResult EditCouDetails(int id, CourseModel obj)
        {
            if (Session["UserId"] != null && Session["Accountid"] != null)
            {
                try
                {
                    CourseRepo CouRepo = new CourseRepo();

                    CouRepo.UpdateCourse(obj);

                    return RedirectToAction("GetAllCourse");
                }
                catch
                {
                    return View();
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
            
        }
   
        public ActionResult DeleteCou(int id)
        {
            if (Session["UserId"] != null && Session["Accountid"] != null)
            {
                try
                {
                    CourseRepo CouRepo = new CourseRepo();
                    if (CouRepo.DeleteCourse(id))
                    {
                        ViewBag.AlertMsg = "Course details deleted successfully";

                    }
                    return RedirectToAction("GetAllCourse");

                }
                catch
                {
                    return View();
                }
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
            
        }
    }
}