using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using StudyTestDemo.Model;
using StudyTestDemo.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace StudyTestDemo.Controllers
{
    //[Route("Home")]
    public class HomeController : Controller
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(IStudentRepository studentRepository, IWebHostEnvironment webHostEnvironment)
        {
            _studentRepository = studentRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        //public string Index()
        //{
        //    return "Hello MVC";
        //}
        //public JsonResult Index()
        //{
        //    return Json(new {id = "1", name = "张三"});
        //}

        #region ObjectResult遵循内容协商
        //public ObjectResult Index()
        //{
        //    Student model = _studentRepository.GetStudent(1);
        //    return new ObjectResult(model);
        //} 
        #endregion

        #region 属性路由,灵活配置路由,特殊情况下使用
        //[Route("")]
        //[Route("Home")]
        //[Route("Home/Index")]
        //[Route("Home/Index/{id?}")]

        //[Route("")]
        //[Route("/")]
        //[Route("Index")] 
        #endregion
        public IActionResult Index()
        {
            #region ViewData,ViewBag的使用
            //Student student = _studentRepository.GetStudent(1);
            //ViewData["Name"] = student.Name;
            //ViewBag.Student = student;
            //ViewData["Student"] = student;
            //return View("~/Views/Home/Index.cshtml",student);
            //return View("Index", student); 
            #endregion

            IEnumerable<Student> students = _studentRepository.GetList();
            return View(students);
        }

        public IActionResult Detail(int id)
        {
            Student student = _studentRepository.GetStudent(id);
            if (student==null)
            {
                Response.StatusCode = 404;
                return View("StudentNotFound",id);
            }

            StudentDetailViewModel studentDetailViewModel = new StudentDetailViewModel()
            {
                PageTitle = "学生信息",
                Student = student
            };
            return View(studentDetailViewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(StudentCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = ProcessUploadFile(model);
                Student student = new Student()
                {
                    Age = model.Age,
                    Name = model.Name,
                    ClassName = model.ClassName,
                    Email = model.Email,
                    PhotoPath = uniqueFileName
                };
                Student newStudent = _studentRepository.Add(student);
                return RedirectToAction("Detail", new { id = newStudent.Id });
            }


            return View();
        }

        public IActionResult UseLayout()
        {
            StudentDetailViewModel studentViewModel = new StudentDetailViewModel()
            {
                PageTitle = "Students Details",
                Students = _studentRepository.GetList().ToList()
            };
            return View(studentViewModel);
        }

        [HttpGet]
        public ViewResult Edit(int id)
        {
            Student student = _studentRepository.GetStudent(id);
            StudentEditViewModel studentEditViewModel = new StudentEditViewModel()
            {
                Id = student.Id,
                Age = student.Age,
                ClassName = student.ClassName,
                Email = student.Email,
                Name = student.Name,
                ExistingPhotoPath = student.PhotoPath
            };

            return View(studentEditViewModel);
        }

        [HttpPost]
        public IActionResult Edit(StudentEditViewModel studentEditViewModel)
        {
            if (ModelState.IsValid)
            {
                Student student = _studentRepository.GetStudent(studentEditViewModel.Id);
                student.Name = studentEditViewModel.Name;
                student.Age = studentEditViewModel.Age;
                student.ClassName = studentEditViewModel.ClassName;
                student.Email = studentEditViewModel.Email;
                if (studentEditViewModel.Photo != null)
                {
                    if (studentEditViewModel.ExistingPhotoPath != null)
                    {
                        string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "img", studentEditViewModel.ExistingPhotoPath);
                        System.IO.File.Delete(filePath);
                    }
                    student.PhotoPath = ProcessUploadFile(studentEditViewModel);
                }

                Student updateStudent = _studentRepository.Update(student);
                return RedirectToAction("Index");
            }

            return View(studentEditViewModel);
        }

        private string ProcessUploadFile(StudentCreateViewModel studentCreateViewModel)
        {
            string uniqueFileName = string.Empty;
            //单文件上传
            if (studentCreateViewModel.Photo != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "img");
                uniqueFileName = Guid.NewGuid() + "_" + studentCreateViewModel.Photo.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    studentCreateViewModel.Photo.CopyTo(fileStream);
                }
            }

            #region 多文件上传
            //if (model.Photos != null && model.Photos.Count >= 1)
            //{
            //    foreach (IFormFile modelPhoto in model.Photos)
            //    {
            //        //必须将图像上传到wwwroot中的img文件夹
            //        //而要获取wwwroot文件夹的路径,我们需要注入Asp.Net Core提供的IWebHostEnvironment服务
            //        //通过IWebHostEnvironment服务去获取wwwroot文件夹的路径
            //        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "img");
            //        //为了确保文件名是唯一的,我们在文件名后附加一个新的GUID值和一个下划线
            //        uniqueFileName = Guid.NewGuid().ToString() + "_" + modelPhoto.FileName;
            //        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            //        using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            //        {
            //        //使用IFormFile接口提供的CopyTo()方法将文件复制到wwwroot/img文件夹
            //            modelPhoto.CopyTo(fileStream);
            //        }
            //    }
            //} 
            #endregion

            return uniqueFileName;
        }
    }
}