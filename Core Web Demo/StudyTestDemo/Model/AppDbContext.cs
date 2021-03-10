using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace StudyTestDemo.Model
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
           
        }
        public DbSet<Student> Student { get; set; }


        //在数据库初始化时,添加种子数据
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //把所有内容都写在AppDbContext中,会使得AppDbContext越来越庞大, 因此使用扩展方法
            modelBuilder.Seed();
            //modelBuilder.Entity<Student>().HasData(
            //    new Student()
            //    {
            //        Id = 1,
            //        Name = "张三",
            //        Age = 10,
            //        ClassName = ClassNameEnum.FirstGrade,
            //        Email = "dasfaf@qq.com"
            //    },
            //    new Student()
            //    {
            //        Id = 2,
            //        Name = "李四",
            //        Age = 11,
            //        ClassName = ClassNameEnum.SecondGrade,
            //        Email = "dafadf@qeq.com"
            //    });
        }
    }
}
