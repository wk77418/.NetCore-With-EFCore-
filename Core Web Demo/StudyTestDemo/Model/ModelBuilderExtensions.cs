using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace StudyTestDemo.Model
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>().HasData(
                new Student()
                {
                    Id = 1,
                    Name = "张三",
                    Age = 10,
                    ClassName = ClassNameEnum.FirstGrade,
                    Email = "dasfaf@qq.com"
                },
                new Student()
                {
                    Id = 2,
                    Name = "李四",
                    Age = 11,
                    ClassName = ClassNameEnum.SecondGrade,
                    Email = "dafadf@qeq.com"
                });
        }
    }
}
