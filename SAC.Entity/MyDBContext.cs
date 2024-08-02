using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;
using System.Configuration;
using System.Reflection;


namespace SAC.Entity
{

    public static class MyDbContext 
    {
        public static IServiceCollection AddSqlSugarTest(this IServiceCollection services, IConfiguration configuration)
        { 
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("数据库连接字符串未配置");
            }

            var db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = connectionString,
                DbType = DbType.SqlServer, // 根据你的数据库类型进行调整  
                IsAutoCloseConnection = true,
                // 其他配置...  
            });

            services.AddSingleton<ISqlSugarClient>(db);

            return services;
        }

        public static IServiceCollection AddSqlSugar(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(provider =>
            {
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    throw new InvalidOperationException("数据库连接字符串未配置");
                }

                return new SqlSugarClient(new ConnectionConfig()
                {
                    ConnectionString = connectionString,
                    DbType = DbType.SqlServer, // 根据你的数据库类型进行调整  
                    IsAutoCloseConnection = true, // 注意：这个设置可能不是必需的，因为连接池会管理连接的打开和关闭  
                                                  // 其他配置...  
                });
            });

            return services;
        }
    }
}       
