using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudyTestDemo.Model
{
    public interface IStudentRepository
    {
        Student GetStudent(int id);
        IEnumerable<Student> GetList();
        Student Add(Student student);
        Student Delete(int id);
        Student Update(Student updateStudent);
    }
}
