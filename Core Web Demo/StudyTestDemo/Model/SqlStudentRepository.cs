using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace StudyTestDemo.Model
{
    public class SqlStudentRepository : IStudentRepository
    {
        private readonly AppDbContext _context;

        public SqlStudentRepository(AppDbContext context)
        {
            _context = context;
        }
        public Student GetStudent(int id)
        {
            Student student = _context.Student.Find(id);
            return student;
        }

        public IEnumerable<Student> GetList()
        {
            return _context.Student;
        }

        public Student Add(Student student)
        {
             _context.Student.Add(student);
             _context.SaveChanges();
             return student;
        }

        public Student Delete(int id)
        {
            Student student = _context.Find<Student>(id);
            if (student!=null)
            {
                _context.Student.Remove(student);
                _context.SaveChanges();
            }

            return student;
        }

        public Student Update(Student updateStudent)
        {
            var student = _context.Student.Attach(updateStudent);
            student.State = EntityState.Modified;
            _context.SaveChanges();
            return updateStudent;
        }
    }
}
