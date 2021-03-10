using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudyTestDemo.Model;

namespace StudyTestDemo.ViewModels
{
    public class StudentDetailViewModel
    {
        public Student Student { get; set; }
        public List<Student> Students { get; set; }
        public string PageTitle { get; set; }
    }
}
