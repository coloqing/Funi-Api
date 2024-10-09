using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SIV.Api.Authorization
{

    /// <summary>
    /// JWT权限 认证服务
    /// </summary>
    public static class AuthenticationSetup
    {
        public static void AddAuthenticationJWTSetup(this IServiceCollection services,IConfiguration Configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            #region 参数
            //读取配置文件
            var symmetricKeyAsBase64 = Configuration["Audience:Secret"];
            var keyByteArray = Encoding.ASCII.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);
            var Issuer = Configuration["Audience:Issuer"];
            var Audience = Configuration["Audience:Audience"];

            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            // 角色与接口的权限要求参数
            var permissionRequirement = new SSORequirement(
            "/api/denied",      // 拒绝授权的跳转地址（目前无用）
            ClaimTypes.Role,    //基于角色的授权
            Issuer,             //发行人
            Audience,           //听众
            signingCredentials, //签名凭据
            expiration: TimeSpan.FromSeconds(7 * 24 * 60 * 60)//接口的过期时间
            );
            #endregion

            services.AddSingleton(permissionRequirement);

            
            // 令牌验证参数
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,//密钥
                ValidateIssuer = true,
                ValidIssuer = Issuer,       //发行人
                ValidateAudience = true,
                ValidAudience = Audience,   //订阅人
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromSeconds(60),
                RequireExpirationTime = true

            };
            // 开启Bearer认证
            services.AddAuthentication(o =>
            {
                o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = nameof(CustomerAuthenticationHandler);
                o.DefaultForbidScheme = nameof(CustomerAuthenticationHandler);
            })
             .AddJwtBearer(o =>
             {
                 o.RequireHttpsMetadata = false;
                 o.TokenValidationParameters = tokenValidationParameters;
                 o.Events = new JwtBearerEvents
                 {
                     OnChallenge = context =>
                     {
                         context.Response.Headers.Add("Token-Error", context.ErrorDescription);
                         return Task.CompletedTask;
                     },
                     OnAuthenticationFailed = context =>
                     {
                         var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                         try
                         {
                             var jwtToken = (new JwtSecurityTokenHandler()).ReadJwtToken(token);

                             if (jwtToken.Issuer != Issuer)
                             {
                                 context.Response.Headers.Add("Token-Error-Iss", "issuer is wrong!");
                             }

                             if (jwtToken.Audiences.FirstOrDefault() != Audience)
                             {
                                 context.Response.Headers.Add("Token-Error-Aud", "Audience is wrong!");
                             }


                             // 如果过期，则把<是否过期>添加到，返回头信息中
                             if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                             {
                                 context.Response.Headers.Add("Token-Expired", "true");
                             }
                         }
                         catch (Exception)
                         {
                             return Task.CompletedTask;
                         }

                         return Task.CompletedTask;
                     }
                 };
             }).AddScheme<AuthenticationSchemeOptions, CustomerAuthenticationHandler>
             (nameof(CustomerAuthenticationHandler), o => { });

        }
    }
}
