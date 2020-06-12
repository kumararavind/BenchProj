using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace LMSProfile.Repository
{
    public class LoginRepo
    {
        private SqlConnection con;
        //To Handle connection related activities    
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["Connection"].ConnectionString.ToString();
            con = new SqlConnection(constr);

        }


    }
}