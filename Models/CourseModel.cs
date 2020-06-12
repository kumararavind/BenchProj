using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMSProfile.Models
{
    public class CourseModel
    {
        public int courseId { get; set; }
        public string courseName { get; set; }
        public int courseLength { get; set; }
        public string courseDesc { get; set; }
        public string courseTech { get; set; }
        public DateTime courseStartDate { get; set; }
        public DateTime courseEndDate { get; set; }
        
        public int courseInstructorId { get; set; }

        public int catId { get; set; }
        public string categoryName { get; set; }
        public List<CourseModel> CategoryList { get; set; }

    }
}