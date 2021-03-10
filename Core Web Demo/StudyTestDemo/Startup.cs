using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StudyTestDemo.Model;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace StudyTestDemo
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            //配置数据库连接
            //配置完以后,在程序包管理控制台中执行install-package microsoft.entityframeworkcore.tools
            //然后 get-help about_entityframeworkcore 查看帮助,就可以使用add-migration   ,update-database  执行数据库的迁移操作了
            //使用add-migration之后如果想撤销可以直接remove-migration,如果已经update-database,需要使用update-database回滚到更早的迁移记录,然后再执行remove-migration
            services.AddDbContextPool<AppDbContext>(options => options.UseSqlServer(_configuration.GetConnectionString("StudentDbConnection")));
            
            //AddXmlSerializerFormatters()使MVC支持返回XML格式的内容
            services.AddControllersWithViews().AddXmlSerializerFormatters();
            //services.AddSingleton<IStudentRepository, MockStudentRepository>();//单例的,首次Http请求会创建一个实例,后续整个应用程序生命周期中都使用这个实例
            services.AddScoped<IStudentRepository, SqlStudentRepository>();//组内的,在一次Http请求的生命周期中,只有一个实例,多次请求创建多个
            //services.AddTransient<IStudentRepository, MockStudentRepository>();//临时的,Http请求一次创建一个
            //services.AddMvc();
            //services.AddMvcCore();
            //MvcCore 只包含了核心的MVC功能,Mvc包含了MvcCore以及常用的第三方服务和方法
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            //错误页中间件要在管道中提早注入,拦截后续的异常
            //开发环境(Development),集成环境(integration),测试环境(testing),QA验证,模拟环境(staging),生产环境(production)
            //env.IsEnvironment("UAT");
            if (env.IsDevelopment())
            {
                ////定义错误代码长度
                //DeveloperExceptionPageOptions developerExceptionPageOptions = new DeveloperExceptionPageOptions();
                //developerExceptionPageOptions.SourceCodeLineCount = 30;
                //app.UseDeveloperExceptionPage(developerExceptionPageOptions);
                app.UseDeveloperExceptionPage();
            }
            else if (env.IsProduction() || env.IsStaging() || env.IsEnvironment("UAT"))
            {
                //app.UseExceptionHandler("/Home/Error");
                //app.UseStatusCodePages();//指向固定页面,只显示错误编码和编码名称,可配置性差
                //app.UseStatusCodePagesWithRedirects("/Error/{0}");//不推荐,出现错误时,返回给浏览器302重定向的状态码,隐藏了原始的请求状态码
                app.UseStatusCodePagesWithReExecute("/Error/{0}");//推荐,出错时,返回给浏览器原始的请求状态码,404就是404,而不是用302覆盖
            }

            #region 添加默认文件中间件

            ////index.html index.htm default.html  default.htm
            //DefaultFilesOptions defaultFilesOptions = new DefaultFilesOptions();
            //defaultFilesOptions.DefaultFileNames.Clear();
            //defaultFilesOptions.DefaultFileNames.Add("Hello.html");
            //app.UseDefaultFiles(defaultFilesOptions);

            //UseFileServer()结合了UseStaticFiles,UseDefaultFiles和UseDirectoryBrowser中间件的功能
            //FileServerOptions fileServerOptions = new FileServerOptions();
            //fileServerOptions.DefaultFilesOptions.DefaultFileNames.Clear();
            //fileServerOptions.DefaultFilesOptions.DefaultFileNames.Add("Hey.html");
            //app.UseFileServer(fileServerOptions);

            #endregion 添加默认文件中间件

            //添加静态文件中间件
            //将请求静态文件放在mvc请求管道前面,如果有静态文件的请求,直接到这一步就返回了,不会继续走mvc管道,节约了不必要的请求流程
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider =
                    new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "ProjectStaticFile")),
                RequestPath = "/StaticFiles"
            });

            app.UseRouting();//将请求与端点匹配

            #region use是中断指令,碰到response会直接跳出请求管道,next可以跳出use继续向下执行
            //app.Use(async (context, next) =>
            //{
            //    context.Response.ContentType = "text/plain;charset=utf-8";

            //    logger.LogInformation("MW1:传入请求");
            //    await next();
            //    logger.LogInformation("MW1:传出响应");
            //});
            //app.Use(async (context,next) =>
            //{
            //    logger.LogInformation("MW2:传入请求");
            //    await next();
            //    logger.LogInformation("MW2:传出响应");

            //});
            //app.Use(async (context, next) =>
            //{
            //    await context.Response.WriteAsync("MW3:处理请求,并生成响应");
            //    logger.LogInformation("MW3:处理请求,并生成响应");

            //});

            #endregion use是中断指令,碰到response会直接跳出请求管道,next可以跳出use继续向下执行

            //app.UseMvcWithDefaultRoute();//使用默认配置的路由
            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapGet("/", async context =>
                //{
                //    //context.Response.ContentType = "text/plain;charset=utf-8";
                //    //throw new Exception("您的请求在管道中发生了一些错误,请检查");
                //    //var configVal = _configuration["MyKey"];
                //    await context.Response.WriteAsync("Hosting Environment:" + env.EnvironmentName);
                //});
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}