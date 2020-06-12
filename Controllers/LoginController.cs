using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data;
using LMSProfile.Models;
using System.Security;
using System.Web.Security;
using System.Web.Services.Description;
using System.Linq.Expressions;
using System.Configuration;

namespace LMSProfile.Controllers
{
    [RoutePrefix("/Login")]
    public class LoginController : Controller
    {
        private SqlConnection con;   
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString.ToString();
            con = new SqlConnection(constr);

        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult Index()
        {
            
            LoginLogout ll = new LoginLogout();
            connection();
                try
                {
                    con.Open();
                    using (SqlCommand cmd1 = new SqlCommand("SELECT account_id,account_type FROM Account_Details", con))
                    {

                        DataSet ds = new DataSet();
                        SqlDataAdapter da = new SqlDataAdapter(cmd1);
                        da.Fill(ds);
                        List<LoginLogout> account = new List<LoginLogout>();
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            LoginLogout uobj = new LoginLogout();
                            uobj.AccountID = Convert.ToInt32(ds.Tables[0].Rows[i]["account_id"].ToString());
                            uobj.AccountType = ds.Tables[0].Rows[i]["account_type"].ToString();
                            account.Add(uobj);
                        }
                        ll.AccountList = account;
                    }
                    con.Close();
                    return View(ll);
                }
                catch (SqlException)
                {
                    ViewData["message"] = "Login Attempt Failed! Check Email And Password";
                }
                
                return View();
            
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            Session.RemoveAll();
            Session.Abandon(); // it will clear the session at the end of request
            return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        [Route("Index")]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult Index(LoginLogout ll,FormCollection form) //this is the login page which u will see at first.
        {
            connection();
            try
                {
                    string accountid = Convert.ToString(form["AccountList"]);
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_Login_Logout", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@email", ll.Email);
                        cmd.Parameters.AddWithValue("@password", ll.Password);
                        cmd.Parameters.AddWithValue("@accountid", accountid);//create  A drop down in the login page stating account type and while clicking login dropdown should send the account number .
                        cmd.ExecuteNonQuery();
                        SqlDataReader sqd = cmd.ExecuteReader();
                        if (sqd.Read())
                        {
                            FormsAuthentication.SetAuthCookie(ll.Email, true);
                            Session["UserId"] = sqd["userid"];
                            Session["Accountid"] = sqd["account_id"];
                            
                            return RedirectToAction("Welcome", ll);
                        }
                        else
                        {
                            
                            ViewData["message"] = "Login Attempt Failed! Check Email And Password";
                        }
                    }
                    
                    con.Close();

                }
                catch (SqlException)
                {
                    ViewData["message"] = "Login Attempt Failed! Check Email And Password";
                }
                catch(NullReferenceException)
                {
                    ViewData["message"] = "Login Attempt Failed! Check Email And Password";
                }

                ViewBag.DuplicateMesaage = "Wrong Credentials";
                return RedirectToAction("Index");
        }

        public ActionResult Welcome(LoginLogout ll)
        {
            if (Session["UserId"] != null && Session["Accountid"] != null)
            {
                ViewData["email"] = ll.Email;
                return View("Welcome", "AfterLoginView");
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
            
        }

      
        [Route("AddUsers")]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult AddUsers(ProfileModel model)
        {
            try
            {
                connection();
                con.Open();
                    using (SqlCommand cmd1 = new SqlCommand("SELECT account_id,account_type FROM Account_Details", con))
                    {

                        DataSet ds = new DataSet();
                        SqlDataAdapter da = new SqlDataAdapter(cmd1);
                        da.Fill(ds);
                        List<ProfileModel> account = new List<ProfileModel>();
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            ProfileModel uobj = new ProfileModel();
                            uobj.AccountID = Convert.ToInt32(ds.Tables[0].Rows[i]["account_id"].ToString());
                            uobj.AccountType = ds.Tables[0].Rows[i]["account_type"].ToString();
                            account.Add(uobj);
                        }
                        model.AccountList = account;
                    }
                    con.Close();
                
            }
            catch (SqlException)
            {
                ViewBag.duplicatemessage = "Please Enter All The Data";
                return RedirectToAction("AddUsers", model);
            }
            return View("AddUsers", model);
        }

        [HttpPost]
        [Route("AddUsers1")]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult AddUsers1(ProfileModel model, FormCollection form)
        {
            connection();
                try
                {
                    string accountid = Convert.ToString(form["AccountList"]);
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_Add_Admin_Instructor_User", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@name", model.Name);
                        cmd.Parameters.AddWithValue("@email", model.Email);
                        cmd.Parameters.AddWithValue("@dob", model.DateOfBirth);
                        cmd.Parameters.AddWithValue("@phno", model.Contact);
                        cmd.Parameters.AddWithValue("@gender", model.Gender);
                        cmd.Parameters.AddWithValue("@address", model.Address);
                        cmd.Parameters.AddWithValue("@password", model.Password);
                        cmd.Parameters.AddWithValue("@accountid", accountid); //should be included from the dropdown.
                        int i = cmd.ExecuteNonQuery();
                        con.Close();
                        if (i <0)
                        {
                            ViewBag.Successmessage = "Registration Successful";
                            return RedirectToAction("AddUsers", new ProfileModel());

                        }
                        else
                        {

                            ViewBag.duplicatemessage = "This Email Address is Already taken";
                            return RedirectToAction("AddUsers", model);
                        }
                    }
                }
                catch (SqlException)
                {
                    ViewBag.duplicatemessage = "This Email Address is Already taken";
                    return RedirectToAction("AddUsers", model);
                }
            
        }


        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GetDetails(ProfileModel model) //can be seen in profile page and used to retrieve values inside textboxes.
        {
            if (Session["UserId"] != null && Session["Accountid"] != null)
            {
                connection();
                try
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_Display_Profile", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@status", "GET");
                        cmd.Parameters.AddWithValue("@accountid", Session["Accountid"]);
                        cmd.Parameters.AddWithValue("@userid", Session["UserId"]);
                        cmd.ExecuteNonQuery();

                        SqlDataReader sdr = cmd.ExecuteReader();
                        while (sdr.Read())
                        {
                            ViewBag.Text1 = sdr["name"]; //Where ColumnName is the Field from the DB that you want to display
                            ViewBag.Text2 = sdr["email"];
                            ViewBag.Text3 = sdr["phno"];
                            ViewBag.date = sdr["dob"];
                            ViewBag.Text5 = sdr["gender"];
                            ViewBag.Text6 = sdr["address"];
                        }
                    }
                    con.Close();
                }
                catch (SqlException)
                {
                    ViewBag.duplicatemessage = "Error While Loading Profile";
                    return View("GetDetails", model);
                }

                return View("GetDetails", model);
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
            
        }

        [HttpPost]
        [Route("GetDetails1")]
        public ActionResult GetDetails1(ProfileModel model) //used to update the profile page after clicking the update button in profile page.
        {
            connection();
                try
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_Display_Profile", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@status", "UPDATE");
                        cmd.Parameters.AddWithValue("@ph_no", model.Contact);
                        cmd.Parameters.AddWithValue("@address", model.Address);
                        cmd.Parameters.AddWithValue("@accountid", Session["Accountid"]);
                        cmd.Parameters.AddWithValue("@userid", Session["UserId"]); 
                        cmd.ExecuteNonQuery();
                        
                    }
                    con.Close();
                }
                catch (SqlException)
                {
                    ViewBag.duplicatemessage = "Profile Updation Failed";
                    return RedirectToAction("GetDetails", model);
                }
                ViewBag.Successmessage = "Profile Updated";
                return RedirectToAction("GetDetails", model);
            
        }
    }    
}