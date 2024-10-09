using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace SIV.Api.Authorization
{
    /// <summary>
    /// 授权设置
    /// </summary>
    public static class AuthorizationSetup
    {
        /// <summary>
        /// 授权设置
        /// </summary>
        /// <param name="services"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddAuthorizationSetup(this IServiceCollection services, IConfiguration Configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            
            #region 参数
            //读取配置文件
            var symmetricKeyAsBase64 = Configuration ["Audience:Secret"];
            var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);
            var Issuer = Configuration["Audience:Issuer"]; 
            var Audience = Configuration["Audience:Audience"];

            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            // 角色与接口的权限要求参数
            var permissionRequirement = new SSORequirement(
            "/api/denied",      // 拒绝授权的跳转地址（目前无用）
            ClaimTypes.Sid,    //基于用户ID的授权
            Issuer,             //发行人
            Audience,           //听众
            signingCredentials, //签名凭据
            expiration: TimeSpan.FromSeconds(7 * 24 * 60 * 60)//接口的过期时间
            );
            #endregion

            services.AddSingleton(permissionRequirement);

            //授权
            services.AddAuthorization(options =>
            {
                options.AddPolicy("role",
                         policy => policy.Requirements.Add(permissionRequirement));
            });

            services.AddSingleton<AuthDataService>();
            services.AddHostedService<AuthDataService>(provider => provider.GetRequiredService<AuthDataService>());

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IAuthorizationHandler, CustomerAuthorizationHandler>();
        }
    }

    public class SSORequirement : IAuthorizationRequirement
    {
        /// <summary>
        /// 请求路径
        /// </summary>
        public string LoginPath { get; set; } = "/Api/Login";

        /// <summary>
        /// 用户接口权限
        /// </summary>
        public Dictionary<int, UserRoleHierarchy>? Permissions { get; set; }
        /// <summary>
        /// 认证授权类型
        /// </summary>
        public string ClaimType { get; set; }

        /// <summary>
        /// 拒绝授权时跳转API
        /// </summary>
        public string DeniedApi { get; set; }
        /// <summary>
        /// 发行人
        /// </summary>
        public string Issuer { get; set; }
        /// <summary>
        /// 订阅人
        /// </summary>
        public string Audience { get; set; }
        /// <summary>
        /// 过期时间
        /// </summary>
        public TimeSpan Expiration { get; set; }
        /// <summary>
        /// 签名验证
        /// </summary>
        public SigningCredentials SigningCredentials { get; set; }

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="claimType">声明类型</param>
        /// <param name="issuer">发行人</param>
        /// <param name="audience">订阅人</param>
        /// <param name="signingCredentials">签名验证实体</param>
        /// <param name="expiration">过期时间</param>
        public SSORequirement(string deniedApi,
            string claimType,
            string issuer,
            string audience,
            SigningCredentials signingCredentials,
            TimeSpan expiration)
        {
            DeniedApi = deniedApi;
            ClaimType = claimType;
            Issuer = issuer;
            Audience = audience;
            Expiration = expiration;
            SigningCredentials = signingCredentials;
        }
    }

   
}
