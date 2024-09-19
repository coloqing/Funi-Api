using SIV.Util;
using SIV.Entity;
using SIV.Util.DTO;
using System.Configuration;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System.Reflection;

namespace SIV.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // 此方法由运行时调用以添加服务到容器中。  
        public void ConfigureServices(IServiceCollection services)
        {                 
            // 添加框架服务。  
            services.AddControllers();
            //添加数据库服务
            services.AddSqlSugar(Configuration);
            //添加映射服务
            services.AddAutoMapperExt();

            //注册配置服务
            var appSettings = new AppSettings(); 
            Configuration.GetSection("AppSettings").Bind(appSettings);

            // 将AppSettings注册为服务
            services.AddSingleton(appSettings);


            // 其他服务注册...  

            // 例如，添加CORS策略  
            services.AddCors(options =>
            {
                options.AddPolicy(name: "MyPolicy",
                                  builder =>
                                  {
                                      builder.AllowAnyOrigin()
                                             .AllowAnyHeader()
                                             .AllowAnyMethod();
                                  });
            });
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                // 如果有需要，可以在这里添加自定义操作  
            });
            //services.AddHostedService<FaultWarnService>();
            // 如果需要，还可以添加Swagger等  
        }

        // 此方法由运行时调用来配置HTTP请求管道。  
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                    c.RoutePrefix ="";
                });
            }

            // 启用CORS策略  
            app.UseCors("MyPolicy");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // 其他中间件配置...  
        }
    }
}
