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

            //�������ݿ�����
            //�������Ժ�,�ڳ�����������̨��ִ��install-package microsoft.entityframeworkcore.tools
            //Ȼ�� get-help about_entityframeworkcore �鿴����,�Ϳ���ʹ��add-migration   ,update-database  ִ�����ݿ��Ǩ�Ʋ�����
            //ʹ��add-migration֮������볷������ֱ��remove-migration,����Ѿ�update-database,��Ҫʹ��update-database�ع��������Ǩ�Ƽ�¼,Ȼ����ִ��remove-migration
            services.AddDbContextPool<AppDbContext>(options => options.UseSqlServer(_configuration.GetConnectionString("StudentDbConnection")));
            
            //AddXmlSerializerFormatters()ʹMVC֧�ַ���XML��ʽ������
            services.AddControllersWithViews().AddXmlSerializerFormatters();
            //services.AddSingleton<IStudentRepository, MockStudentRepository>();//������,�״�Http����ᴴ��һ��ʵ��,��������Ӧ�ó������������ж�ʹ�����ʵ��
            services.AddScoped<IStudentRepository, SqlStudentRepository>();//���ڵ�,��һ��Http���������������,ֻ��һ��ʵ��,������󴴽����
            //services.AddTransient<IStudentRepository, MockStudentRepository>();//��ʱ��,Http����һ�δ���һ��
            //services.AddMvc();
            //services.AddMvcCore();
            //MvcCore ֻ�����˺��ĵ�MVC����,Mvc������MvcCore�Լ����õĵ���������ͷ���
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            //����ҳ�м��Ҫ�ڹܵ�������ע��,���غ������쳣
            //��������(Development),���ɻ���(integration),���Ի���(testing),QA��֤,ģ�⻷��(staging),��������(production)
            //env.IsEnvironment("UAT");
            if (env.IsDevelopment())
            {
                ////���������볤��
                //DeveloperExceptionPageOptions developerExceptionPageOptions = new DeveloperExceptionPageOptions();
                //developerExceptionPageOptions.SourceCodeLineCount = 30;
                //app.UseDeveloperExceptionPage(developerExceptionPageOptions);
                app.UseDeveloperExceptionPage();
            }
            else if (env.IsProduction() || env.IsStaging() || env.IsEnvironment("UAT"))
            {
                //app.UseExceptionHandler("/Home/Error");
                //app.UseStatusCodePages();//ָ��̶�ҳ��,ֻ��ʾ�������ͱ�������,�������Բ�
                //app.UseStatusCodePagesWithRedirects("/Error/{0}");//���Ƽ�,���ִ���ʱ,���ظ������302�ض����״̬��,������ԭʼ������״̬��
                app.UseStatusCodePagesWithReExecute("/Error/{0}");//�Ƽ�,����ʱ,���ظ������ԭʼ������״̬��,404����404,��������302����
            }

            #region ���Ĭ���ļ��м��

            ////index.html index.htm default.html  default.htm
            //DefaultFilesOptions defaultFilesOptions = new DefaultFilesOptions();
            //defaultFilesOptions.DefaultFileNames.Clear();
            //defaultFilesOptions.DefaultFileNames.Add("Hello.html");
            //app.UseDefaultFiles(defaultFilesOptions);

            //UseFileServer()�����UseStaticFiles,UseDefaultFiles��UseDirectoryBrowser�м���Ĺ���
            //FileServerOptions fileServerOptions = new FileServerOptions();
            //fileServerOptions.DefaultFilesOptions.DefaultFileNames.Clear();
            //fileServerOptions.DefaultFilesOptions.DefaultFileNames.Add("Hey.html");
            //app.UseFileServer(fileServerOptions);

            #endregion ���Ĭ���ļ��м��

            //��Ӿ�̬�ļ��м��
            //������̬�ļ�����mvc����ܵ�ǰ��,����о�̬�ļ�������,ֱ�ӵ���һ���ͷ�����,���������mvc�ܵ�,��Լ�˲���Ҫ����������
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider =
                    new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "ProjectStaticFile")),
                RequestPath = "/StaticFiles"
            });

            app.UseRouting();//��������˵�ƥ��

            #region use���ж�ָ��,����response��ֱ����������ܵ�,next��������use��������ִ��
            //app.Use(async (context, next) =>
            //{
            //    context.Response.ContentType = "text/plain;charset=utf-8";

            //    logger.LogInformation("MW1:��������");
            //    await next();
            //    logger.LogInformation("MW1:������Ӧ");
            //});
            //app.Use(async (context,next) =>
            //{
            //    logger.LogInformation("MW2:��������");
            //    await next();
            //    logger.LogInformation("MW2:������Ӧ");

            //});
            //app.Use(async (context, next) =>
            //{
            //    await context.Response.WriteAsync("MW3:��������,��������Ӧ");
            //    logger.LogInformation("MW3:��������,��������Ӧ");

            //});

            #endregion use���ж�ָ��,����response��ֱ����������ܵ�,next��������use��������ִ��

            //app.UseMvcWithDefaultRoute();//ʹ��Ĭ�����õ�·��
            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapGet("/", async context =>
                //{
                //    //context.Response.ContentType = "text/plain;charset=utf-8";
                //    //throw new Exception("���������ڹܵ��з�����һЩ����,����");
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