using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudyTestDemo.Model
{
    public class MockStudentRepository:IStudentRepository
    {
        private List<Student> _students;
        public MockStudentRepository()
        {
            _students = new List<Student>()
            {
                new Student() {Id = 1, Age = 10, Name = "张三",ClassName = ClassNameEnum.FirstGrade},
                new Student() {Id = 2, Age = 10, Name = "李四",ClassName = ClassNameEnum.SecondGrade },
                new Student() {Id = 3, Age = 10, Name = "王五", ClassName = ClassNameEnum.GradeThree},
                new Student() {Id = 4, Age = 10, Name = "赵六",ClassName = ClassNameEnum.None }
            };
        }
        public Student GetStudent(int id)
        {
            return _students.FirstOrDefault(a => a.Id == id);
        }

        public IEnumerable<Student> GetList()
        {
            return _students;
        }

        public Student Add(Student student)
        {
            student.Id = _students.Max(i => i.Id) + 1;
            _students.Add(student);
            return student;
        }

        public Student Delete(int id)
        {
            Student student = _students.FirstOrDefault(i => i.Id == id);
            if (student!=null)
            {
                _students.Remove(student);
            }

            return student;
        }
        

        public Student Update(Student updateStudent)
        {
            Student student = _students.FirstOrDefault(i => i.Id == updateStudent.Id);
            if (student!=null)
            {
                student.Name = updateStudent.Name;
                student.Age = updateStudent.Age;
                student.Email = updateStudent.Email;
                student.ClassName = updateStudent.ClassName;
            }

            return student;
        }
    }
}
