using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using StudyTestDemo.Model;

namespace StudyTestDemo.ViewModels
{
    public class StudentCreateViewModel
    {
        //模型验证
        //[Required]指定该字段是必填的
        //[Range]指定允许的最小值和最大值
        //[MinLength],[MaxLength]使用指定字符串的最小长度或最大长度
        //Compare 比较模型的2个属性,例如比较Email和ConfirmEmail属性
        //RegularExpression 正则表达式验证提供的值是否与正则表达式指定的模式匹配
        [Display(Name = "年龄")]
        [Required(ErrorMessage = "请输入年龄")]
        public int Age { get; set; }

        [Display(Name = "姓名")]
        [Required(ErrorMessage = "请输入姓名"), MaxLength(50, ErrorMessage = "名字的长度不能超过50个字符")]
        public string Name { get; set; }

        [Display(Name = "年级")]
        [Required(ErrorMessage = "请选择班级")]
        public ClassNameEnum? ClassName { get; set; }

        [Display(Name = "邮箱")]
        [Required(ErrorMessage = "请输入邮箱")]
        [RegularExpression(@"^[a-zA-Z0-9_.+ -]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$", ErrorMessage = "邮箱的格式不正确")]
        public string Email { get; set; }

        //Dto模型,设定上传图片的类型
        [Display(Name = "图片")]
        //使用List集合可以支持多文件上传
        //public List<IFormFile> Photos { get; set; }
        public IFormFile Photo { get; set; }

    }
}
