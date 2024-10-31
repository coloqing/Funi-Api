using SIV.Util;
using SIV.Entity;
using SIV.Util.DTO;
using System.Configuration;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Text.Json;
using Newtonsoft.Json.Serialization;
using SIV.Api.Authorization;

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
            services.AddControllers().AddNewtonsoftJson(options =>
            {
                //配置序列化时时间格式为yyyy-MM-dd HH:mm:ss
                options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss.fff";
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });
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
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT授权(数据将在请求头中进行传输) 直接在下框中输入Bearer {token}（注意两者之间是一个空格）",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
                // 如果有需要，可以在这里添加自定义操作  
            });
            //services.AddHostedService<FaultWarnService>();
            // 如果需要，还可以添加Swagger等   

            services.AddAuthenticationJWTSetup(Configuration);

            services.AddAuthorizationSetup(Configuration);
        }

        // 此方法由运行时调用来配置HTTP请求管道。  
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {


            if (env.IsDevelopment() || true)
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                    c.RoutePrefix = "";
                });
            }

            // 启用CORS策略  
            app.UseCors("MyPolicy");

            //app.UseHttpsRedirection();

            app.UseRouting();


            // 先开启认证
            app.UseAuthentication();

            // 然后是授权中间件
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // 其他中间件配置...  
        }
    }
}
